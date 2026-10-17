import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IncidentService } from '../../../core/services/incident.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Incident } from '../../../core/models/incident.model';
import { INCIDENT_SEVERITIES, INCIDENT_STATUSES, IncidentSeverity } from '../../../core/models/enums';
import { StatusBadgeComponent } from '../../../shared/components/status-badge';
import { relativeTime } from '../../../core/utils/date.util';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-incident-list',
  imports: [ReactiveFormsModule, RouterLink, StatusBadgeComponent],
  templateUrl: './incident-list.html',
})
export class IncidentListComponent {
  private readonly fb = inject(FormBuilder);
  private readonly incidentService = inject(IncidentService);
  private readonly notifications = inject(NotificationService);

  readonly severities = INCIDENT_SEVERITIES;
  readonly statuses = INCIDENT_STATUSES;

  readonly incidents = signal<Incident[]>([]);
  readonly loading = signal(true);
  readonly pageIndex = signal(1);
  readonly pageSize = signal(environment.defaultPageSize);
  readonly hasNext = signal(false);

  readonly filters = this.fb.nonNullable.group({
    status: [''],
    severity: [''],
    search: [''],
  });

  readonly visibleIncidents = computed(() => {
    const severity = this.filters.controls.severity.value as IncidentSeverity | '';
    if (!severity) return this.incidents();
    return this.incidents().filter((i) => i.severity === severity);
  });

  readonly activeCount = computed(() => this.incidents().filter((i) => i.status !== 'Resolved').length);

  readonly unmitigatedP0P1 = computed(
    () =>
      this.incidents().filter(
        (i) =>
          (i.severity === 'Critical' || i.severity === 'High') &&
          i.status !== 'Resolved' &&
          i.status !== 'Mitigated'
      ).length
  );

  readonly distinctServices = computed(
    () => new Set(this.incidents().map((i) => i.serviceId)).size
  );

  constructor() {
    this.filters.controls.search.valueChanges
      .pipe(debounceTime(350), distinctUntilChanged(), takeUntilDestroyed())
      .subscribe(() => {
        this.pageIndex.set(1);
        this.load();
      });

    this.filters.controls.status.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => {
      this.pageIndex.set(1);
      this.load();
    });

    this.load();
  }

  load(): void {
    this.loading.set(true);
    const { status, search } = this.filters.getRawValue();
    this.incidentService
      .getAll({
        pageIndex: this.pageIndex(),
        pageSize: this.pageSize(),
        status: status || null,
        search: search || null,
      })
      .subscribe({
        next: (res) => {
          const data = res.data ?? [];
          this.incidents.set(data);
          this.hasNext.set(data.length === this.pageSize());
          this.loading.set(false);
        },
        error: () => {
          this.loading.set(false);
          this.notifications.error('Load failed', 'Unable to retrieve incidents.');
        },
      });
  }

  changePage(next: number): void {
    if (next < 1) return;
    this.pageIndex.set(next);
    this.load();
  }

  trackByIncidentId(_index: number, incident: Incident): number {
    return incident.id;
  }

  relative(value: string): string {
    return relativeTime(value);
  }

  exportCsv(): void {
    const rows = this.visibleIncidents();
    if (rows.length === 0) {
      this.notifications.info('Nothing to export', 'No incidents match the current filters.');
      return;
    }
    const header = ['ID', 'Title', 'Severity', 'Status', 'Service', 'CreatedAt', 'ResolvedAt'];
    const lines = rows.map((i) =>
      [
        i.id,
        `"${i.title.replace(/"/g, '""')}"`,
        i.severity,
        i.status,
        i.serviceName ?? '',
        i.createdAt,
        i.resolvedAt ?? '',
      ].join(',')
    );
    const blob = new Blob([[header.join(','), ...lines].join('\r\n')], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `incidents-${new Date().toISOString().slice(0, 10)}.csv`;
    link.click();
    URL.revokeObjectURL(url);
    this.notifications.success('Export complete', `${rows.length} incidents exported.`);
  }
}
