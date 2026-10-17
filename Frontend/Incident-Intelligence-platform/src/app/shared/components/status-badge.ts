import { Component, computed, input } from '@angular/core';

type BadgeKind = 'severity' | 'status' | 'loglevel';

@Component({
  selector: 'app-status-badge',
  template: `<span
    [class]="classes()"
    class="inline-flex items-center rounded border px-2.5 py-1 text-xs font-semibold"
  >
    {{ label() }}
  </span>`,
})
export class StatusBadgeComponent {
  readonly value = input.required<string>();
  readonly kind = input<BadgeKind>('status');

  readonly label = computed(() => {
    const v = this.value();
    if (this.kind() === 'loglevel') {
      return v.toUpperCase();
    }
    return v;
  });

  private static readonly map: Record<string, string> = {
    Critical: 'bg-sev-critical-bg text-sev-critical border-sev-critical/20',
    High: 'bg-sev-high-bg text-sev-high border-sev-high/20',
    Medium: 'bg-sev-medium-bg text-sev-medium border-sev-high/20',
    Low: 'bg-sev-low-bg text-sev-low border-sev-low-border',
    Open: 'bg-sev-critical-bg text-sev-critical border-sev-critical/20',
    Investigating: 'bg-sev-high-bg text-sev-high border-sev-high/20',
    Mitigated: 'bg-sev-info-bg text-sev-info border-sev-info/20',
    Resolved: 'bg-sev-resolved-bg text-sev-resolved border-sev-resolved/20',
    Info: 'bg-sev-info-bg text-sev-info border-sev-info/20',
    Warn: 'bg-sev-high-bg text-sev-high border-sev-high/20',
    Error: 'bg-sev-critical-bg text-sev-critical border-sev-critical/20',
  };

  readonly classes = computed(
    () => StatusBadgeComponent.map[this.value()] ?? 'bg-surface-container text-secondary border-outline-variant'
  );
}
