import { Component, inject } from '@angular/core';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-toast-container',
  template: `
    <div class="pointer-events-none fixed bottom-4 right-4 z-50 flex w-[360px] flex-col gap-2">
      @for (toast of notifications.toasts(); track toast.id) {
        <div
          class="pointer-events-auto flex items-start gap-3 rounded-xl border bg-surface-container-lowest p-3 shadow-float"
          [class]="borderClass(toast.kind)"
        >
          <span class="material-symbols-outlined text-[20px]" [class]="iconClass(toast.kind)">
            {{ icon(toast.kind) }}
          </span>
          <div class="min-w-0 flex-1">
            <div class="font-label-md text-label-md text-on-surface">{{ toast.title }}</div>
            @if (toast.message) {
              <div class="mt-0.5 font-body-sm text-body-sm text-secondary">{{ toast.message }}</div>
            }
          </div>
          <button
            type="button"
            class="text-secondary hover:text-on-surface"
            (click)="notifications.dismiss(toast.id)"
          >
            <span class="material-symbols-outlined text-[18px]">close</span>
          </button>
        </div>
      }
    </div>
  `,
})
export class ToastContainerComponent {
  readonly notifications = inject(NotificationService);

  icon(kind: string): string {
    return (
      { success: 'check_circle', error: 'error', warning: 'warning', info: 'info' }[kind] ?? 'info'
    );
  }

  iconClass(kind: string): string {
    return (
      {
        success: 'text-tertiary',
        error: 'text-sev-critical',
        warning: 'text-sev-high',
        info: 'text-primary-container',
      }[kind] ?? 'text-secondary'
    );
  }

  borderClass(kind: string): string {
    return (
      {
        success: 'border-sev-resolved/40',
        error: 'border-sev-critical/40',
        warning: 'border-sev-high/40',
        info: 'border-sev-info/40',
      }[kind] ?? 'border-border-subtle'
    );
  }
}
