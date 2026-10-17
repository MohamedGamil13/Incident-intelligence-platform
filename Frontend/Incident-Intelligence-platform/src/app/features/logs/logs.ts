import { Component, computed, inject, input, signal } from '@angular/core';
import { DestroyRef } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LogsService } from '../../core/services/logs.service';
import { ServiceCatalogService } from '../../core/services/service-catalog.service';
import { NotificationService } from '../../core/services/notification.service';
import { LogEntry } from '../../core/models/log.model';
import { Service } from '../../core/models/service.model';
import { LOG_LEVELS } from '../../core/models/enums';
import { StatusBadgeComponent } from '../../shared/components/status-badge';
import { PaginationComponent } from '../../shared/components/pagination';
import { formatTime, formatDateTime } from '../../core/utils/date.util';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-logs',
  imports: [ReactiveFormsModule, StatusBadgeComponent, PaginationComponent],
  templateUrl: './logs.html',
})
export class LogsComponent {
  readonly serviceId = input<string | undefined>();

  private readonly fb = inject(FormBuilder);
  private readonly logsService = inject(LogsService);
  private readonly catalog = inject(ServiceCatalogService);
  private readonly notifications = inject(NotificationService);
  private readonly destroyRef = inject(DestroyRef);

  readonly levels = LOG_LEVELS;

  readonly logs = signal<LogEntry[]>([]);
  readonly services = signal<Service[]>([]);
  readonly loading = signal(true);
  readonly liveTail = signal(false);
  readonly pageIndex = signal(1);
  readonly pageSize = signal(environment.defaultPageSize);
  readonly hasNext = signal(false);

  private timer: ReturnType<typeof setInterval> | null = null;

  readonly filters = this.fb.nonNullable.group({
    serviceId: [''],
    level: [''],
    search: [''],
  });

  readonly visibleLogs = computed(() => {
    const level = this.filters.controls.level.value;
    const term = this.filters.controls.search.value.trim().toLowerCase();
    return this.logs().filter((log) => {
      if (level && log.logLevel !== level) return false;
      if (!term) return true;
      return (
        log.message.toLowerCase().includes(term) ||
        log.description?.toLowerCase().includes(term) ||
        log.traceId?.toLowerCase().includes(term)
      );
    });
  });

  readonly errorCount = computed(
    () => this.logs().filter((l) => l.logLevel === 'Error' || l.logLevel === 'Critical').length
  );
  readonly warnCount = computed(() => this.logs().filter((l) => l.logLevel === 'Warn').length);

  constructor() {
    this.catalog.getAll(1, environment.maxPageSize).subscribe({
      next: (res) => this.services.set(res.data ?? []),
      error: () => undefined,
    });

    const preset = this.serviceId();
    if (preset) {
      this.filters.controls.serviceId.setValue(preset);
    }

    this.filters.controls.serviceId.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => {
      this.pageIndex.set(1);
      this.load();
    });

    this.load();

    this.destroyRef.onDestroy(() => {
      if (this.timer) clearInterval(this.timer);
    });
  }

  load(): void {
    this.loading.set(true);
    const serviceId = this.filters.controls.serviceId.value;
    const source$ = serviceId
      ? this.logsService.getByService(Number(serviceId), this.pageIndex(), this.pageSize())
      : this.logsService.getAll(this.pageIndex(), this.pageSize());

    source$.subscribe({
      next: (res) => {
        const data = res.data ?? [];
        this.logs.set(data);
        this.hasNext.set(data.length === this.pageSize());
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.notifications.error('Load failed', 'Unable to retrieve log entries.');
      },
    });
  }

  changePage(next: number): void {
    if (next < 1) return;
    this.pageIndex.set(next);
    this.load();
  }

  toggleLiveTail(): void {
    const next = !this.liveTail();
    this.liveTail.set(next);
    if (next) {
      this.timer = setInterval(() => this.load(), environment.evalIntervalMs);
      this.notifications.info('Live tail enabled', 'Streaming new log entries.');
    } else if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
      this.notifications.info('Live tail paused', 'Automatic refresh stopped.');
    }
  }

  copyTrace(traceId: string): void {
    if (!traceId) return;
    navigator.clipboard?.writeText(traceId);
    this.notifications.info('Trace ID copied', traceId);
  }

  shortTime(value: string): string {
    return formatTime(value);
  }

  fullDate(value: string): string {
    return formatDateTime(value);
  }
}
