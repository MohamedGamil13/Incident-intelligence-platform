import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { IncidentService } from '../../../core/services/incident.service';
import { ServiceCatalogService } from '../../../core/services/service-catalog.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Service } from '../../../core/models/service.model';
import { ApiResponse } from '../../../core/models/api-response.model';
import { IncidentSeverity } from '../../../core/models/enums';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-incident-create',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './incident-create.html',
})
export class IncidentCreateComponent {
  private readonly fb = inject(FormBuilder);
  private readonly incidentService = inject(IncidentService);
  private readonly serviceCatalog = inject(ServiceCatalogService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);

  readonly submitting = signal(false);
  readonly services = signal<Service[]>([]);
  readonly serverErrors = signal<string[]>([]);

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
    severity: ['Critical' as IncidentSeverity, [Validators.required]],
    serviceId: [null as number | null, [Validators.required, Validators.min(1)]],
  });

  readonly severity = signal<IncidentSeverity>('Critical');

  private readonly severityConfig: Record<IncidentSeverity, { color: string; caption: string; text: string; pulse: boolean }> = {
    Critical: { color: 'bg-sev-critical', caption: 'Pages primary executive & ops team immediately (P0)', text: 'text-sev-critical', pulse: true },
    High: { color: 'bg-sev-high', caption: 'Direct escalation to secondary on-call engineer (P1)', text: 'text-on-surface', pulse: false },
    Medium: { color: 'bg-sev-medium', caption: 'Notifies service team roster during business hours (P2)', text: 'text-on-surface-variant', pulse: false },
    Low: { color: 'bg-outline', caption: 'Enters standard backlog queue for review (P3)', text: 'text-on-surface-variant', pulse: false },
  };

  readonly severityIndicator = computed(() => this.severityConfig[this.severity()]);
  readonly titleLength = signal(0);
  readonly descriptionLength = signal(0);

  constructor() {
    this.serviceCatalog.getAll(1, environment.maxPageSize).subscribe({
      next: (res) => this.services.set(res.data ?? []),
      error: () => this.notifications.error('Services unavailable', 'Could not load the service catalog.'),
    });

    this.form.controls.severity.valueChanges.subscribe((value) =>
      this.severity.set(value as IncidentSeverity)
    );
    this.form.controls.title.valueChanges.subscribe((v) => this.titleLength.set(v.length));
    this.form.controls.description.valueChanges.subscribe((v) =>
      this.descriptionLength.set(v.length)
    );
  }

  showError(field: 'title' | 'description' | 'serviceId'): boolean {
    const control = this.form.controls[field];
    return control.invalid && (control.dirty || control.touched);
  }

  submit(): void {
    this.serverErrors.set([]);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    this.submitting.set(true);
    this.incidentService
      .create({
        title: value.title,
        description: value.description,
        severity: value.severity,
        serviceId: Number(value.serviceId),
      })
      .subscribe({
        next: (res) => {
          this.submitting.set(false);
          this.notifications.success('Incident created', `#INC-${res.data?.id} dispatched to on-call rotas.`);
          this.router.navigate(['/incidents', res.data?.id]);
        },
        error: (error: HttpErrorResponse) => {
          this.submitting.set(false);
          const body = error.error as ApiResponse<unknown> | null;
          this.serverErrors.set(body?.errors?.length ? body.errors : body?.message ? [body.message] : ['Failed to create incident.']);
          this.notifications.error('Creation failed', 'Review the highlighted errors and retry.');
        },
      });
  }
}
