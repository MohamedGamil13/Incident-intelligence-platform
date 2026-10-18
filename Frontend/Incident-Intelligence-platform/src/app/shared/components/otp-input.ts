import {
  Component,
  ElementRef,
  afterNextRender,
  effect,
  forwardRef,
  input,
  signal,
  viewChildren,
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-otp-input',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => OtpInputComponent),
      multi: true,
    },
  ],
  template: `
    <div class="flex items-center justify-between gap-space-xs" (paste)="onPaste($event)">
      @for (digit of digits(); track $index) {
        <input
          #slot
          type="text"
          inputmode="numeric"
          autocomplete="one-time-code"
          maxlength="1"
          [value]="digit"
          [disabled]="disabled()"
          class="h-[52px] w-12 rounded-lg bg-surface-container-lowest text-center font-code-md text-[20px] font-semibold text-on-surface shadow-sm outline-none transition-colors focus:bg-surface-container-low focus:ring-2 focus:ring-primary-container focus:ring-offset-2 focus:ring-offset-surface-container-lowest disabled:opacity-50"
          (input)="onInput($index, $event)"
          (keydown)="onKeydown($index, $event)"
          (focus)="markTouched()"
        />
      }
    </div>
  `,
})
export class OtpInputComponent implements ControlValueAccessor {
  readonly length = input<number>(6);
  readonly disabled = signal(false);

  private readonly slots = viewChildren<ElementRef<HTMLInputElement>>('slot');
  readonly digits = signal<string[]>(Array.from({ length: 6 }, () => ''));

  private onChange: (value: string) => void = () => undefined;
  private onTouched: () => void = () => undefined;

  constructor() {
    effect(() => {
      const size = this.length();
      this.digits.update((current) =>
        current.length === size
          ? current
          : Array.from({ length: size }, (_, index) => current[index] ?? '')
      );
    });

    afterNextRender(() => this.focusSlot(0));
  }

  writeValue(value: string | null): void {
    const chars = (value ?? '').split('');
    this.digits.set(
      Array.from({ length: this.length() }, (_, index) => chars[index] ?? '')
    );
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled.set(isDisabled);
  }

  markTouched(): void {
    this.onTouched();
  }

  focus(): void {
    this.focusSlot(0);
  }

  onInput(index: number, event: Event): void {
    const target = event.target as HTMLInputElement;
    const digit = target.value.replace(/\D/g, '').slice(-1);
    target.value = digit;
    this.digits.update((current) => {
      const next = [...current];
      next[index] = digit;
      return next;
    });
    this.emit();
    if (digit && index < this.length() - 1) {
      this.focusSlot(index + 1);
    }
  }

  onKeydown(index: number, event: KeyboardEvent): void {
    if (event.key === 'Backspace') {
      if (!this.digits()[index] && index > 0) {
        event.preventDefault();
        this.digits.update((current) => {
          const next = [...current];
          next[index - 1] = '';
          return next;
        });
        this.emit();
        this.focusSlot(index - 1);
      }
      return;
    }
    if (event.key === 'ArrowLeft' && index > 0) {
      event.preventDefault();
      this.focusSlot(index - 1);
    } else if (event.key === 'ArrowRight' && index < this.length() - 1) {
      event.preventDefault();
      this.focusSlot(index + 1);
    }
  }

  onPaste(event: ClipboardEvent): void {
    const pasted = (event.clipboardData?.getData('text') ?? '')
      .replace(/\D/g, '')
      .slice(0, this.length());
    if (!pasted) return;
    event.preventDefault();
    const chars = pasted.split('');
    this.digits.set(
      Array.from({ length: this.length() }, (_, index) => chars[index] ?? '')
    );
    this.emit();
    this.focusSlot(Math.min(chars.length, this.length() - 1));
  }

  private emit(): void {
    this.onChange(this.digits().join(''));
  }

  private focusSlot(index: number): void {
    const el = this.slots()[index]?.nativeElement;
    el?.focus();
    el?.select();
  }
}
