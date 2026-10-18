import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { PasswordResetStateService } from '../../../core/services/password-reset-state.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ApiResponse } from '../../../core/models/api-response.model';

@Component({
  selector: 'app-forgot-password',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.html',
})
export class ForgotPasswordComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly resetState = inject(PasswordResetStateService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);

  readonly submitting = signal(false);
  readonly showPassword = signal(false);
  readonly errorMessage = signal<string | null>(null);

  private static passwordsMatch(control: AbstractControl): { mismatch: boolean } | null {
    const password = control.get('newPassword')?.value;
    const confirm = control.get('confirmPassword')?.value;
    return password && confirm && password !== confirm ? { mismatch: true } : null;
  }

  readonly form = this.fb.nonNullable.group(
    {
      email: ['', [Validators.required, Validators.email]],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: ForgotPasswordComponent.passwordsMatch }
  );

  submit(): void {
    this.errorMessage.set(null);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { email, newPassword, confirmPassword } = this.form.getRawValue();
    this.submitting.set(true);
    this.auth.requestPasswordReset({ email }).subscribe({
      next: (res) => {
        this.submitting.set(false);
        this.resetState.set({ email, newPassword, confirmPassword });
        this.notifications.success(
          'Verification code sent',
          res.message || 'Check your inbox for the 6-digit code.'
        );
        this.router.navigate(['/auth/verify-otp'], { queryParams: { email } });
      },
      error: (error: HttpErrorResponse) => {
        this.submitting.set(false);
        const body = error.error as ApiResponse<unknown> | null;
        const message = body?.errors?.length ? body.errors.join(' ') : body?.message;
        this.errorMessage.set(message ?? 'Unable to send the verification code.');
      },
    });
  }
}
