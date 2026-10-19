import { Component, DestroyRef, computed, effect, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { IncidentService } from '../../../core/services/incident.service';
import { IncidentEventService } from '../../../core/services/incident-event.service';
import { LogsService } from '../../../core/services/logs.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Incident } from '../../../core/models/incident.model';
import { IncidentEvent } from '../../../core/models/incident-event.model';
import { LogEntry } from '../../../core/models/log.model';
import {
  INCIDENT_EVENT_TIMESTAMPS,
  INCIDENT_SEVERITIES,
  INCIDENT_STATUSES,
  IncidentEventTimeStamp,
  IncidentSeverity,
  IncidentStatus,
} from '../../../core/models/enums';
import { ApiResponse } from '../../../core/models/api-response.model';
import { StatusBadgeComponent } from '../../../shared/components/status-badge';
import { formatDateTime, formatTime, relativeTime } from '../../../core/utils/date.util';
import { AuthService } from '../../../core/services/auth.service';

type Tab = 'overview' | 'timeline' | 'logs' | 'rca';

@Component({
  selector: 'app-incident-detail',
  imports: [ReactiveFormsModule, RouterLink, StatusBadgeComponent],
  templateUrl: './incident-detail.html',
})
export class IncidentDetailComponent {
  readonly incidentId = input.required<string>();

  private readonly fb = inject(FormBuilder);
  private readonly incidentService = inject(IncidentService);
  private readonly eventService = inject(IncidentEventService);
  private readonly logsService = inject(LogsService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);
  private readonly auth = inject(AuthService);

  readonly severities = INCIDENT_SEVERITIES;
  readonly statuses = INCIDENT_STATUSES;
  readonly eventTimestamps = INCIDENT_EVENT_TIMESTAMPS;

  readonly incident = signal<Incident | null>(null);
  readonly events = signal<IncidentEvent[]>([]);
  readonly logs = signal<LogEntry[]>([]);
  readonly loading = signal(true);
  readonly activeTab = signal<Tab>('overview');
  readonly moreOpen = signal(false);
  readonly editing = signal(false);
  readonly confirmDelete = signal(false);
  readonly submitting = signal(false);
  readonly addingEvent = signal(false);
  readonly now = signal(Date.now());
  readonly serverErrors = signal<string[]>([]);

  private numericId = 0;

  readonly canUpdate = signal(this.auth.canUpdateIncident());
  readonly canDelete = signal(this.auth.canDeleteIncident());
  readonly canAddEvent = signal(this.auth.canAddIncidentEvent());
  readonly canShowMoreActions = computed(() => this.canUpdate() || this.canDelete());

  readonly statusForm = this.fb.nonNullable.group({
    status: ['Open' as IncidentStatus, [Validators.required]],
  });

  readonly editForm = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
    severity: ['Medium' as IncidentSeverity, [Validators.required]],
    status: ['Open' as IncidentStatus, [Validators.required]],
  });

  readonly eventForm = this.fb.nonNullable.group({
    timeStamp: ['Working' as IncidentEventTimeStamp, [Validators.required]],
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
  });

  readonly duration = computed(() => {
    const incident = this.incident();
    if (!incident) return '00:00:00';
    const start = new Date(incident.createdAt).getTime();
    const end = incident.resolvedAt ? new Date(incident.resolvedAt).getTime() : this.now();
    const diff = Math.max(0, end - start);
    const hours = Math.floor(diff / 3_600_000);
    const minutes = Math.floor((diff % 3_600_000) / 60_000);
    const seconds = Math.floor((diff % 60_000) / 1000);
    return [hours, minutes, seconds].map((n) => n.toString().padStart(2, '0')).join(':');
  });

  readonly isActive = computed(() => this.incident()?.status !== 'Resolved');
  readonly timelineCount = computed(() => this.events().length);

  constructor() {
    effect(() => {
      const id = this.incidentId();
      if (id) {
        this.numericId = Number(id);
        this.loadAll();
      }
    });

    const timer = setInterval(() => this.now.set(Date.now()), 1000);
    inject(DestroyRef).onDestroy(() => clearInterval(timer));
  }

  loadAll(): void {
    this.loading.set(true);
    this.incidentService.getById(this.numericId).subscribe({
      next: (res) => {
        this.incident.set(res.data);
        this.statusForm.patchValue({ status: res.data.status });
        this.editForm.patchValue({
          title: res.data.title,
          description: res.data.description,
          severity: res.data.severity,
          status: res.data.status,
        });
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.notifications.error('Not found', `Incident #${this.numericId} could not be loaded.`);
      },
    });
    this.loadTimeline();
  }

  loadTimeline(): void {
    this.eventService.getTimeline(this.numericId, 1, 50).subscribe({
      next: (res) => this.events.set(res.data ?? []),
      error: () => this.notifications.error('Timeline unavailable', 'Could not load incident events.'),
    });
  }

  loadLogs(): void {
    const incident = this.incident();
    if (!incident) return;
    this.logsService.getByService(incident.serviceId, 1, 50).subscribe({
      next: (res) => this.logs.set(res.data ?? []),
      error: () => this.logs.set([]),
    });
  }

  selectTab(tab: Tab): void {
    this.activeTab.set(tab);
    if (tab === 'logs' && this.logs().length === 0) {
      this.loadLogs();
    }
  }

  toggleMore(): void {
    this.moreOpen.update((v) => !v);
  }

  transitionStatus(): void {
    const incident = this.incident();
    if (!incident) return;
    const status = this.statusForm.controls.status.value as IncidentStatus;
    if (status === incident.status) return;

    this.incidentService
      .update(incident.id, {
        title: incident.title,
        description: incident.description,
        severity: incident.severity,
        status,
      })
      .subscribe({
        next: (res) => {
          this.incident.set(res.data);
          this.editForm.patchValue({ status: res.data.status });
          this.notifications.success('Status updated', `Incident moved to ${res.data.status}.`);
          this.loadTimeline();
        },
        error: () => this.notifications.error('Update failed', 'Could not transition status.'),
      });
  }

  startEdit(): void {
    this.editing.set(true);
    this.moreOpen.set(false);
  }

  cancelEdit(): void {
    this.editing.set(false);
  }

  saveEdit(): void {
    const incident = this.incident();
    if (!incident) return;
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.incidentService.update(incident.id, this.editForm.getRawValue()).subscribe({
      next: (res) => {
        this.submitting.set(false);
        this.editing.set(false);
        this.incident.set(res.data);
        this.statusForm.patchValue({ status: res.data.status });
        this.notifications.success('Incident updated', 'Changes saved successfully.');
      },
      error: () => {
        this.submitting.set(false);
        this.notifications.error('Update failed', 'Could not save incident changes.');
      },
    });
  }

  deleteIncident(): void {
    const incident = this.incident();
    if (!incident) return;
    this.incidentService.delete(incident.id).subscribe({
      next: () => {
        this.notifications.success('Incident deleted', `#INC-${incident.id} removed.`);
        this.router.navigate(['/incidents']);
      },
      error: () => this.notifications.error('Delete failed', 'Could not delete the incident.'),
    });
  }

  addEvent(): void {
    if (this.eventForm.invalid) {
      this.eventForm.markAllAsTouched();
      return;
    }
    this.addingEvent.set(true);
    this.eventService.addEvent(this.numericId, this.eventForm.getRawValue()).subscribe({
      next: () => {
        this.addingEvent.set(false);
        this.eventForm.patchValue({ title: '', description: '' });
        this.eventForm.controls.title.markAsUntouched();
        this.eventForm.controls.description.markAsUntouched();
        this.notifications.success('Event added', 'Timeline updated.');
        this.loadTimeline();
      },
      error: (error: HttpErrorResponse) => {
        this.addingEvent.set(false);
        const body = error.error as ApiResponse<unknown> | null;
        this.notifications.error('Event rejected', body?.message ?? 'Could not add timeline event.');
      },
    });
  }

  relative(value: string): string {
    return relativeTime(value);
  }

  fullDate(value: string): string {
    return formatDateTime(value);
  }

  timeOnly(value: string): string {
    return formatTime(value);
  }

  eventDot(timestamp: IncidentEventTimeStamp): string {
    return timestamp === 'Solved'
      ? 'bg-sev-resolved'
      : timestamp === 'Working'
        ? 'bg-sev-high'
        : 'bg-primary';
  }
}
