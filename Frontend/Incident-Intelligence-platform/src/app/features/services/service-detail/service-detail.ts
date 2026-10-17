import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { ServiceCatalogService } from '../../../core/services/service-catalog.service';
import { IncidentService } from '../../../core/services/incident.service';
import { LogsService } from '../../../core/services/logs.service';
import { ServiceDeploymentService } from '../../../core/services/service-deployment.service';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Service } from '../../../core/models/service.model';
import { Incident } from '../../../core/models/incident.model';
import { LogEntry } from '../../../core/models/log.model';
import { ApiResponse } from '../../../core/models/api-response.model';
import { StatusBadgeComponent } from '../../../shared/components/status-badge';
import { formatDateTime, relativeTime } from '../../../core/utils/date.util';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-service-detail',
  imports: [ReactiveFormsModule, RouterLink, StatusBadgeComponent],
  templateUrl: './service-detail.html',
})
export class ServiceDetailComponent {
  readonly serviceId = input.required<string>();

  private readonly fb = inject(FormBuilder);
  private readonly catalog = inject(ServiceCatalogService);
  private readonly incidentService = inject(IncidentService);
  private readonly logsService = inject(LogsService);
  private readonly deploymentService = inject(ServiceDeploymentService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);

  readonly canManage = this.auth.canManage;

  readonly service = signal<Service | null>(null);
  readonly incidents = signal<Incident[]>([]);
  readonly logs = signal<LogEntry[]>([]);
  readonly loading = signal(true);

  readonly editOpen = signal(false);
  readonly deployOpen = signal(false);
  readonly submitting = signal(false);

  readonly editForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
  });

  readonly deployForm = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    version: ['', [Validators.required, Validators.maxLength(20)]],
    deployedBy: ['', [Validators.required, Validators.maxLength(50)]],
  });

  private numericId = 0;

  readonly activeIncidents = computed(() =>
    this.incidents().filter((i) => i.status !== 'Resolved')
  );
  readonly activeCount = computed(() => this.activeIncidents().length);

  readonly healthState = computed<'Down' | 'Degraded' | 'Healthy'>(() => {
    const open = this.activeIncidents();
    if (open.some((i) => i.severity === 'Critical')) return 'Down';
    return open.length > 0 ? 'Degraded' : 'Healthy';
  });

  readonly healthIndex = computed(() => {
    const total = this.incidents().length;
    if (total === 0) return '100.00';
    const resolved = this.incidents().filter((i) => i.status === 'Resolved').length;
    return ((resolved / total) * 100).toFixed(2);
  });

  constructor() {
    effect(() => {
      const id = this.serviceId();
      if (id) {
        this.numericId = Number(id);
        this.load();
      }
    });
  }

  load(): void {
    this.loading.set(true);
    forkJoin({
      service: this.catalog.getById(this.numericId),
      incidents: this.incidentService.getAll({
        pageIndex: 1,
        pageSize: environment.maxPageSize,
        serviceId: this.numericId,
      }),
      logs: this.logsService.getByService(this.numericId, 1, 30),
    }).subscribe({
      next: ({ service, incidents, logs }) => {
        this.service.set(service.data);
        this.editForm.patchValue({ name: service.data.name, description: service.data.description });
        this.incidents.set(incidents.data ?? []);
        this.logs.set(logs.data ?? []);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.notifications.error('Service unavailable', `Service #${this.numericId} could not be loaded.`);
      },
    });
  }

  openEdit(): void {
    this.editOpen.set(true);
  }

  saveEdit(): void {
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.catalog.update(this.numericId, this.editForm.getRawValue()).subscribe({
      next: (res) => {
        this.submitting.set(false);
        this.editOpen.set(false);
        this.service.set(res.data);
        this.notifications.success('Service updated', 'Changes saved.');
      },
      error: () => {
        this.submitting.set(false);
        this.notifications.error('Update failed', 'Could not save service changes.');
      },
    });
  }

  recordDeployment(): void {
    if (!this.auth.hasRole('Admin', 'IncidentManager')) {
      this.notifications.warning('Permission denied', 'Only admins and incident managers can record deployments.');
      return;
    }
    this.deployOpen.set(true);
  }

  submitDeployment(): void {
    if (this.deployForm.invalid) {
      this.deployForm.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.deploymentService.record(this.numericId, this.deployForm.getRawValue()).subscribe({
      next: (res) => {
        this.submitting.set(false);
        this.deployOpen.set(false);
        this.deployForm.reset({ title: '', version: '', deployedBy: '' });
        this.notifications.success('Deployment recorded', `v${res.data.version} logged.`);
        this.refreshLogs();
      },
      error: (error: HttpErrorResponse) => {
        this.submitting.set(false);
        const body = error.error as ApiResponse<unknown> | null;
        this.notifications.error('Deployment rejected', body?.message ?? 'Could not record deployment.');
      },
    });
  }

  refreshLogs(): void {
    this.logsService.getByService(this.numericId, 1, 30).subscribe({
      next: (res) => this.logs.set(res.data ?? []),
      error: () => undefined,
    });
  }

  declareIncident(): void {
    this.router.navigate(['/incidents/new'], { queryParams: { serviceId: this.numericId } });
  }

  copyTrace(traceId: string): void {
    if (!traceId) return;
    navigator.clipboard?.writeText(traceId);
    this.notifications.info('Trace ID copied', traceId);
  }

  relative(value: string): string {
    return relativeTime(value);
  }

  fullDate(value: string): string {
    return formatDateTime(value);
  }

  healthDot(): string {
    return {
      Down: 'bg-sev-critical',
      Degraded: 'bg-sev-high',
      Healthy: 'bg-tertiary',
    }[this.healthState()];
  }

  healthBadge(): string {
    return {
      Down: 'border-sev-critical bg-sev-critical-bg text-sev-critical',
      Degraded: 'border-sev-high bg-sev-high-bg text-sev-high',
      Healthy: 'border-sev-resolved bg-sev-resolved-bg text-sev-resolved',
    }[this.healthState()];
  }
}
