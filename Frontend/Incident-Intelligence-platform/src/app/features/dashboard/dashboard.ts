import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { IncidentService } from '../../core/services/incident.service';
import { ServiceCatalogService } from '../../core/services/service-catalog.service';
import { NotificationService } from '../../core/services/notification.service';
import { Incident } from '../../core/models/incident.model';
import { Service } from '../../core/models/service.model';
import { StatusBadgeComponent } from '../../shared/components/status-badge';
import { isToday, relativeTime } from '../../core/utils/date.util';
import { environment } from '../../../environments/environment';

interface ServiceHealthRow {
  id: number;
  name: string;
  state: 'Down' | 'Degraded' | 'Healthy';
  openCount: number;
}

@Component({
  selector: 'app-dashboard',
  imports: [RouterLink, StatusBadgeComponent],
  templateUrl: './dashboard.html',
})
export class DashboardComponent {
  private readonly incidentService = inject(IncidentService);
  private readonly serviceCatalog = inject(ServiceCatalogService);
  private readonly notifications = inject(NotificationService);

  readonly loading = signal(true);
  readonly refreshAt = signal<Date>(new Date());
  readonly evalInterval = environment.evalIntervalMs;

  readonly incidents = signal<Incident[]>([]);
  readonly services = signal<Service[]>([]);

  readonly activeIncidents = computed(() =>
    this.incidents()
      .filter((i) => i.status !== 'Resolved')
      .sort((a, b) => +new Date(b.createdAt) - +new Date(a.createdAt))
  );

  readonly openCount = computed(() => this.activeIncidents().length);

  readonly criticalCount = computed(
    () => this.activeIncidents().filter((i) => i.severity === 'Critical').length
  );

  readonly resolvedTodayCount = computed(
    () =>
      this.incidents().filter((i) => i.status === 'Resolved' && isToday(i.resolvedAt ?? i.createdAt))
        .length
  );

  readonly mostAffectedService = computed(() => {
    const counts = new Map<string, number>();
    for (const incident of this.incidents()) {
      const name = incident.serviceName ?? 'Unassigned';
      counts.set(name, (counts.get(name) ?? 0) + 1);
    }
    let top = 'None';
    let max = 0;
    for (const [name, count] of counts) {
      if (count > max) {
        max = count;
        top = name;
      }
    }
    return top;
  });

  readonly recentIncidents = computed(() => this.activeIncidents().slice(0, 5));

  readonly serviceHealth = computed<ServiceHealthRow[]>(() => {
    return this.services().map((service) => {
      const related = this.incidents().filter((i) => i.serviceId === service.id);
      const open = related.filter((i) => i.status !== 'Resolved');
      const state: ServiceHealthRow['state'] = open.some(
        (i) => i.severity === 'Critical' && i.status === 'Open'
      )
        ? 'Down'
        : open.length > 0
          ? 'Degraded'
          : 'Healthy';
      return { id: service.id, name: service.name, state, openCount: open.length };
    });
  });

  readonly healthyServices = computed(
    () => this.serviceHealth().filter((s) => s.state === 'Healthy').length
  );

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    let completed = 0;
    const done = () => {
      completed += 1;
      if (completed >= 2) {
        this.refreshAt.set(new Date());
        this.loading.set(false);
      }
    };

    this.incidentService
      .getAll({ pageIndex: 1, pageSize: environment.maxPageSize })
      .subscribe({
        next: (res) => {
          this.incidents.set(res.data ?? []);
          done();
        },
        error: () => {
          this.notifications.error('Dashboard unavailable', 'Failed to load incidents.');
          done();
        },
      });

    this.serviceCatalog.getAll(1, environment.maxPageSize).subscribe({
      next: (res) => {
        this.services.set(res.data ?? []);
        done();
      },
      error: () => {
        this.notifications.error('Services unavailable', 'Failed to load service catalog.');
        done();
      },
    });
  }

  stateDot(state: ServiceHealthRow['state']): string {
    return { Down: 'bg-sev-critical', Degraded: 'bg-sev-high', Healthy: 'bg-sev-resolved' }[state];
  }

  stateBadge(state: ServiceHealthRow['state']): string {
    return {
      Down: 'bg-sev-critical-bg text-sev-critical border-sev-critical/20',
      Degraded: 'bg-sev-high-bg text-sev-high border-sev-high/20',
      Healthy: 'bg-surface-container-low text-secondary border-outline-variant',
    }[state];
  }

  relative(value: string): string {
    return relativeTime(value);
  }
}
