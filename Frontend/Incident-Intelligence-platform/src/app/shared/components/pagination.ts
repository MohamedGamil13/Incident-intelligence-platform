import { Component, computed, inject, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  template: `
    <div class="flex flex-wrap items-center justify-between gap-3 border-t border-border-subtle px-4 py-2.5">
      <span class="font-code-sm text-code-sm text-secondary">{{ summary() }}</span>
      <div class="flex items-center gap-2">
        <button
          type="button"
          class="rounded-xl border border-outline-variant bg-surface-container-lowest px-3 py-1.5 font-label-md text-label-md text-on-surface transition-colors hover:bg-surface-container-low disabled:opacity-40"
          [disabled]="page() <= 1"
          (click)="go(page() - 1)"
        >
          Prev
        </button>
        <span class="font-code-sm text-code-sm text-on-surface">Page {{ page() }}</span>
        <button
          type="button"
          class="rounded-xl border border-outline-variant bg-surface-container-lowest px-3 py-1.5 font-label-md text-label-md text-on-surface transition-colors hover:bg-surface-container-low disabled:opacity-40"
          [disabled]="!hasNext()"
          (click)="go(page() + 1)"
        >
          Next
        </button>
      </div>
    </div>
  `,
})
export class PaginationComponent {
  readonly page = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly total = input<number>(0);
  readonly totalLabel = input<string>('records');
  readonly hasNext = input<boolean>(false);
  readonly pageChange = output<number>();

  readonly summary = computed(() => {
    const start = (this.page() - 1) * this.pageSize() + 1;
    const end = this.page() * this.pageSize();
    return `Showing ${start}\u2013${end} ${this.totalLabel()}`;
  });

  go(next: number): void {
    if (next < 1) return;
    this.pageChange.emit(next);
  }
}
