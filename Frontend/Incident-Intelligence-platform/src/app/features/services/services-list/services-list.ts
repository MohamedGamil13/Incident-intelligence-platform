import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ServiceCatalogService } from '../../../core/services/service-catalog.service';
import { IncidentService } from '../../../core/services/incident.service';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Service } from '../../../core/models/service.model';
import { Incident } from '../../../core/models/incident.model';
import { environment } from '../../../../environments/environment';

interface ServiceRow {
  service: Service;
  openCount: number;
  criticalCount: number;
  state: 'Down' | 'Degraded' | 'Healthy';
}

@Component({
  selector: 'app-services-list',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './services-list.html',
})
export class ServicesListComponent {
  private readonly fb = inject(FormBuilder);
  private readonly catalog = inject(ServiceCatalogService);
  private readonly incidents = inject(IncidentService);
  private readonly auth = inject(AuthService);
  private readonly notifications = inject(NotificationService);

  readonly canManage = this.auth.canManage;
  readonly isAdmin = this.auth.isAdmin;

  readonly services = signal<Service[]>([]);
  readonly allIncidents = signal<Incident[]>([]);
  readonly search = signal('');
  readonly loading = signal(true);

  readonly modalOpen = signal(false);
  readonly editingId = signal<number | null>(null);
  readonly submitting = signal(false);
  readonly confirmDeleteId = signal<number | null>(null);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
  });

  readonly rows = computed<ServiceRow[]>(() => {
    const term = this.search().trim().toLowerCase();
    return this.services()
      .map((service) => {
        const related = this.allIncidents().filter(
          (i) => i.serviceId === service.id && i.status !== 'Resolved'
        );
        const state: ServiceRow['state'] =
          related.length === 0
            ? 'Healthy'
            : related.some((i) => i.severity === 'Critical')
              ? 'Down'
              : 'Degraded';
        return {
          service,
          openCount: related.length,
          criticalCount: related.filter((i) => i.severity === 'Critical').length,
          state,
        };
      })
      .filter(
        (row) =>
          !term ||
          row.service.name.toLowerCase().includes(term) ||
          row.service.description.toLowerCase().includes(term)
      )
      .sort((a, b) => b.openCount - a.openCount);
  });

  readonly healthPct = computed(() => {
    const total = this.services().length;
    if (total === 0) return '100%';
    const healthy = this.rows().filter((r) => r.state === 'Healthy').length;
    return `${((healthy / total) * 100).toFixed(1)}%`;
  });

  readonly degradedCount = computed(() => this.rows().filter((r) => r.state === 'Degraded').length);
  readonly downCount = computed(() => this.rows().filter((r) => r.state === 'Down').length);
  readonly linkedIncidents = computed(() => this.rows().reduce((sum, r) => sum + r.openCount, 0));

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    forkJoin({
      services: this.catalog.getAll(1, environment.maxPageSize),
      incidents: this.incidents.getAll({ pageIndex: 1, pageSize: environment.maxPageSize }),
    }).subscribe({
      next: ({ services, incidents }) => {
        this.services.set(services.data ?? []);
        this.allIncidents.set(incidents.data ?? []);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.notifications.error('Load failed', 'Unable to load the service catalog.');
      },
    });
  }

  openCreate(): void {
    this.editingId.set(null);
    this.form.reset({ name: '', description: '' });
    this.modalOpen.set(true);
  }

  openEdit(service: Service, event: Event): void {
    event.stopPropagation();
    this.editingId.set(service.id);
    this.form.reset({ name: service.name, description: service.description });
    this.modalOpen.set(true);
  }

  closeModal(): void {
    this.modalOpen.set(false);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    const id = this.editingId();
    const request = this.form.getRawValue();

    const op$ = id ? this.catalog.update(id, request) : this.catalog.create(request);
    op$.subscribe({
      next: () => {
        this.submitting.set(false);
        this.modalOpen.set(false);
        this.notifications.success(id ? 'Service updated' : 'Service created', 'Catalog synchronized.');
        this.load();
      },
      error: () => {
        this.submitting.set(false);
        this.notifications.error('Save failed', 'Could not persist the service record.');
      },
    });
  }

  deleteService(id: number, event: Event): void {
    event.stopPropagation();
    this.confirmDeleteId.set(id);
  }

  confirmDelete(): void {
    const id = this.confirmDeleteId();
    if (!id) return;
    this.catalog.delete(id).subscribe({
      next: () => {
        this.confirmDeleteId.set(null);
        this.notifications.success('Service deleted', 'Catalog entry removed.');
        this.load();
      },
      error: () => this.notifications.error('Delete failed', 'Could not delete the service.'),
    });
  }

  stateDot(state: ServiceRow['state']): string {
    return {
      Down: 'bg-sev-critical ring-sev-critical/20',
      Degraded: 'bg-secondary ring-secondary/20',
      Healthy: 'bg-tertiary ring-tertiary/20',
    }[state];
  }
}
