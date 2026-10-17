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
import { NotificationService } from '../../../core/services/notification.service';
import { ApiResponse } from '../../../core/models/api-response.model';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly notifications = inject(NotificationService);

  readonly submitting = signal(false);
  readonly showPassword = signal(false);
  readonly showConfirm = signal(false);
  readonly errorMessage = signal<string | null>(null);

  private static passwordsMatch(control: AbstractControl): { mismatch: boolean } | null {
    const password = control.get('password')?.value;
    const confirm = control.get('confirmPassword')?.value;
    return password && confirm && password !== confirm ? { mismatch: true } : null;
  }

  readonly form = this.fb.nonNullable.group(
    {
      name: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: RegisterComponent.passwordsMatch }
  );

  showError(field: 'name' | 'email' | 'password' | 'confirmPassword'): boolean {
    const control = this.form.controls[field];
    const invalid = control.invalid || (field === 'confirmPassword' && this.form.hasError('mismatch'));
    return invalid && (control.dirty || control.touched);
  }

  submit(): void {
    this.errorMessage.set(null);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { name, email, password } = this.form.getRawValue();
    this.submitting.set(true);
    this.auth.register({ name, email, password }).subscribe({
      next: () => {
        this.submitting.set(false);
        this.notifications.success('Account created', 'You can now sign in.');
        this.router.navigate(['/auth/login']);
      },
      error: (error: HttpErrorResponse) => {
        this.submitting.set(false);
        const body = error.error as ApiResponse<unknown> | null;
        const message = body?.errors?.length ? body.errors.join(' ') : body?.message;
        this.errorMessage.set(message ?? 'Registration failed. Please review your details.');
      },
    });
  }
}
