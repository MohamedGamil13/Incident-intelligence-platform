import { Component, DestroyRef, computed, effect, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { PasswordResetStateService } from '../../../core/services/password-reset-state.service';
import { NotificationService } from '../../../core/services/notification.service';
import { OtpInputComponent } from '../../../shared/components/otp-input';
import { ApiResponse } from '../../../core/models/api-response.model';

@Component({
  selector: 'app-verify-otp',
  imports: [ReactiveFormsModule, RouterLink, OtpInputComponent],
  templateUrl: './verify-otp.html',
})
export class VerifyOtpComponent {
  readonly email = input<string | undefined>();

  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly resetState = inject(PasswordResetStateService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);
  private readonly destroyRef = inject(DestroyRef);

  readonly submitting = signal(false);
  readonly resending = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly secondsLeft = signal(60);
  readonly maskedEmail = computed(() => this.resolveEmail() ?? 'your registered email');

  readonly form = this.fb.nonNullable.group({
    otp: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  private timer: ReturnType<typeof setInterval> | null = null;
  private completed = false;

  constructor() {
    effect(() => {
      this.email();
      if (this.completed || this.hasPendingReset()) {
        return;
      }
      this.notifications.warning(
        'Reset session expired',
        'Please enter your email to receive a new verification code.'
      );
      this.router.navigate(['/auth/forgot-password']);
    });

    this.startCountdown();
    this.destroyRef.onDestroy(() => this.stopCountdown());
  }

  submit(): void {
    this.errorMessage.set(null);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const pending = this.resetState.pending();
    const address = this.resolveEmail();
    if (!pending || !address) {
      this.notifications.error('Reset session expired', 'Please restart the password reset flow.');
      this.router.navigate(['/auth/forgot-password']);
      return;
    }

    this.submitting.set(true);
    this.auth
      .resetPassword({
        email: address,
        newPassword: pending.newPassword,
        confirmPassword: pending.confirmPassword,
        OTP: this.form.controls.otp.value,
      })
      .subscribe({
        next: (res) => {
          this.submitting.set(false);
          this.completed = true;
          this.resetState.clear();
          this.notifications.success(
            'Password reset',
            res.message || 'Your credentials have been updated.'
          );
          this.router.navigate(['/auth/login']);
        },
        error: (error: HttpErrorResponse) => {
          this.submitting.set(false);
          const body = error.error as ApiResponse<unknown> | null;
          const message = body?.errors?.length ? body.errors.join(' ') : body?.message;
          this.errorMessage.set(message ?? 'Invalid or expired verification code.');
        },
      });
  }

  resend(): void {
    const address = this.resolveEmail();
    if (!address || this.secondsLeft() > 0) return;
    this.resending.set(true);
    this.errorMessage.set(null);
    this.auth.requestPasswordReset({ email: address }).subscribe({
      next: (res) => {
        this.resending.set(false);
        this.form.reset({ otp: '' });
        this.startCountdown();
        this.notifications.success(
          'Code resent',
          res.message || 'A new verification code is on its way.'
        );
      },
      error: (error: HttpErrorResponse) => {
        this.resending.set(false);
        const body = error.error as ApiResponse<unknown> | null;
        this.errorMessage.set(body?.message ?? 'Unable to resend the verification code.');
      },
    });
  }

  countdown(): string {
    const total = this.secondsLeft();
    const minutes = Math.floor(total / 60);
    const seconds = total % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  private resolveEmail(): string | null {
    return this.email() ?? this.resetState.pending()?.email ?? null;
  }

  private hasPendingReset(): boolean {
    return this.resetState.pending() !== null;
  }

  private startCountdown(): void {
    this.stopCountdown();
    this.secondsLeft.set(60);
    this.timer = setInterval(() => {
      const next = this.secondsLeft() - 1;
      if (next <= 0) {
        this.secondsLeft.set(0);
        this.stopCountdown();
      } else {
        this.secondsLeft.set(next);
      }
    }, 1000);
  }

  private stopCountdown(): void {
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
    }
  }
}
