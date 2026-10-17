import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-error-panel',
  imports: [RouterLink],
  template: `
    <div class="flex min-h-[70vh] items-center justify-center px-gutter">
      <div class="relative mx-auto flex w-full max-w-lg flex-col items-center">
        <div class="pointer-events-none absolute -top-16 h-72 w-72 rounded-full bg-primary/5 blur-3xl"></div>

        <div class="relative z-10 flex w-full flex-col items-center rounded-xl bg-surface-container-lowest p-space-lg text-center shadow-float">
          <div class="mb-space-md flex items-center gap-space-xs rounded-full bg-surface-container-low px-3 py-1.5">
            <span class="h-2 w-2 rounded-full bg-error animate-pulse"></span>
            <div class="flex items-center gap-1.5 text-on-surface-variant">
              <span class="material-symbols-outlined text-[16px] text-primary">dns</span>
              <span class="font-code-sm text-code-sm font-semibold uppercase tracking-wider text-secondary">{{ eyeline() }}</span>
            </div>
          </div>

          <div class="mb-1 select-none">
            <span class="font-headline-xl text-[56px] font-semibold leading-none tracking-tight text-secondary">{{ code() }}</span>
          </div>

          <h1 class="mb-2 font-headline-lg text-headline-lg tracking-tight text-on-surface">{{ heading() }}</h1>
          <p class="mb-space-md max-w-sm font-body-md leading-relaxed text-body-md text-secondary">{{ description() }}</p>

          <div class="mb-space-lg w-full rounded-lg bg-surface-container-low p-3.5 text-left">
            <div class="mb-2 flex items-center justify-between">
              <span class="flex items-center gap-1.5 font-label-sm text-label-sm uppercase tracking-widest text-secondary">
                <span class="material-symbols-outlined text-[14px]">terminal</span>
                Diagnostic Payload
              </span>
              <span class="rounded bg-surface-container px-2 py-0.5 font-code-sm text-code-sm text-on-surface-variant">{{ statusTag() }}</span>
            </div>
            <div class="space-y-1 font-code-sm text-code-sm text-on-surface-variant">
              <div class="flex items-center justify-between">
                <span class="select-none text-secondary">CODE:</span>
                <span class="font-semibold text-on-surface">HTTP {{ code() }}</span>
              </div>
              <div class="flex items-center justify-between">
                <span class="select-none text-secondary">STATE:</span>
                <span class="font-medium text-error">{{ state() }}</span>
              </div>
              <div class="flex items-center justify-between">
                <span class="select-none text-secondary">TRACE_ID:</span>
                <span class="select-all text-secondary transition-colors hover:text-on-surface">{{ traceId() }}</span>
              </div>
            </div>
          </div>

          <div class="flex w-full flex-col items-center gap-space-xs sm:flex-row">
            <a
              routerLink="/dashboard"
              class="inline-flex h-9 w-full items-center justify-center gap-2 rounded-xl bg-primary-container font-label-md text-label-md text-on-primary transition-all duration-150 hover:bg-primary active:scale-[0.99] sm:flex-1"
            >
              <span class="material-symbols-outlined text-[18px]">dashboard</span>
              <span>Back to Dashboard</span>
            </a>
            <button
              type="button"
              class="inline-flex h-9 w-full items-center justify-center gap-1.5 rounded-xl bg-surface-container-low px-4 font-label-md text-label-md text-on-surface transition-all duration-150 hover:bg-surface-container sm:w-auto"
              (click)="goBack()"
            >
              <span class="material-symbols-outlined text-[18px] text-secondary">arrow_back</span>
              <span>Go Back</span>
            </button>
          </div>

          <div class="mt-space-md flex w-full items-center justify-between pt-space-md text-secondary">
            <span class="flex items-center gap-1 font-code-sm text-code-sm">
              <span class="inline-block h-1.5 w-1.5 rounded-full bg-tertiary"></span>
              Cluster: ingress-alpha-09
            </span>
            <span class="font-label-sm text-label-sm">US-EAST-VA</span>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ErrorPanelComponent {
  readonly code = input.required<string>();
  readonly heading = input.required<string>();
  readonly description = input.required<string>();
  readonly state = input<string>('Unexpected condition');
  readonly eyeline = input<string>('INCIDENT OPS // GATEWAY');
  readonly statusTag = input<string>('STATUS: ERR_UNRESOLVED');
  readonly traceId = input<string>('8f3a9d21-gw04.prod');

  goBack(): void {
    window.history.back();
  }
}
