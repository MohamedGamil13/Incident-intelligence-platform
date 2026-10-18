import { Component, DestroyRef, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { OtpInputComponent } from '../../shared/components/otp-input';
import { environment } from '../../../environments/environment';

const PREFS_KEY = 'iip.ui.preferences';

interface UiPreferences {
  landingPage: string;
  autoRefresh: boolean;
  refreshInterval: number;
  compactTables: boolean;
}

@Component({
  selector: 'app-settings',
  imports: [ReactiveFormsModule, OtpInputComponent],
  templateUrl: './settings.html',
})
export class SettingsComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly notifications = inject(NotificationService);
  private readonly destroyRef = inject(DestroyRef);

  readonly email = this.auth.userEmail;
  readonly roles = this.auth.roles;
  readonly isAdmin = this.auth.isAdmin;

  readonly intervalOptions = [5000, 10000, 30000, 60000];
  readonly landingOptions = [
    { value: '/dashboard', label: 'Dashboard' },
    { value: '/incidents', label: 'Incidents' },
    { value: '/services', label: 'Services' },
    { value: '/logs', label: 'Logs' },
  ];

  readonly savedPrefs = signal<UiPreferences>(this.loadPrefs());

  readonly passwordStep = signal<'password' | 'otp'>('password');
  readonly sending = signal(false);
  readonly confirming = signal(false);
  readonly secondsLeft = signal(0);
  readonly maskedEmail = computed(() => this.email() ?? 'your account email');

  showNew = false;
  showConfirm = false;

  private pendingPassword: { newPassword: string; confirmPassword: string } | null = null;
  private timer: ReturnType<typeof setInterval> | null = null;

  readonly passwordForm = this.fb.nonNullable.group({
    newPassword: [
      '',
      [
        Validators.required,
        Validators.minLength(12),
        Validators.pattern(/^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$/),
      ],
    ],
    confirmPassword: ['', [Validators.required]],
  });

  readonly otpForm = this.fb.nonNullable.group({
    otp: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
  });

  readonly prefsForm = this.fb.nonNullable.group({
    landingPage: [this.savedPrefs().landingPage],
    autoRefresh: [this.savedPrefs().autoRefresh],
    refreshInterval: [this.savedPrefs().refreshInterval],
    compactTables: [this.savedPrefs().compactTables],
  });

  constructor() {
    this.destroyRef.onDestroy(() => this.stopCountdown());
  }

  initials(value: string | null): string {
    return (value ?? '?').slice(0, 2).toUpperCase();
  }

  countdown(): string {
    const total = this.secondsLeft();
    const minutes = Math.floor(total / 60);
    const seconds = total % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  submitPassword(): void {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }
    const { newPassword, confirmPassword } = this.passwordForm.getRawValue();
    if (newPassword !== confirmPassword) {
      this.notifications.error('Passwords do not match', 'Confirm password must match the new password.');
      return;
    }
    const address = this.email();
    if (!address) {
      this.notifications.error('No session email', 'Sign in again to change your password.');
      return;
    }
    this.sending.set(true);
    this.auth.requestPasswordReset({ email: address }).subscribe({
      next: (res) => {
        this.sending.set(false);
        this.pendingPassword = { newPassword, confirmPassword };
        this.otpForm.reset({ otp: '' });
        this.passwordStep.set('otp');
        this.startCountdown();
        this.notifications.success(
          'Verification code sent',
          res.message || 'Check your inbox for the 6-digit code.'
        );
      },
      error: () => {
        this.sending.set(false);
        this.notifications.error('Could not send code', 'The verification code could not be sent.');
      },
    });
  }

  confirmOtp(): void {
    if (this.otpForm.invalid) {
      this.otpForm.markAllAsTouched();
      return;
    }
    const address = this.email();
    if (!address || !this.pendingPassword) {
      this.notifications.error('Reset session expired', 'Please request a new verification code.');
      this.backToPassword();
      return;
    }
    this.confirming.set(true);
    this.auth
      .resetPassword({
        email: address,
        newPassword: this.pendingPassword.newPassword,
        confirmPassword: this.pendingPassword.confirmPassword,
        OTP: this.otpForm.controls.otp.value,
      })
      .subscribe({
        next: () => {
          this.confirming.set(false);
          this.notifications.success('Password updated', 'Use your new credentials on next sign-in.');
          this.backToPassword();
        },
        error: () => {
          this.confirming.set(false);
          this.notifications.error('Invalid code', 'The verification code is incorrect or expired.');
        },
      });
  }

  resendCode(): void {
    const address = this.email();
    if (!address || this.secondsLeft() > 0) return;
    this.sending.set(true);
    this.auth.requestPasswordReset({ email: address }).subscribe({
      next: (res) => {
        this.sending.set(false);
        this.otpForm.reset({ otp: '' });
        this.startCountdown();
        this.notifications.success('Code resent', res.message || 'A new code is on its way.');
      },
      error: () => {
        this.sending.set(false);
        this.notifications.error('Could not resend', 'The verification code could not be resent.');
      },
    });
  }

  backToPassword(): void {
    this.passwordStep.set('password');
    this.pendingPassword = null;
    this.passwordForm.reset({ newPassword: '', confirmPassword: '' });
    this.otpForm.reset({ otp: '' });
    this.stopCountdown();
    this.secondsLeft.set(0);
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

  savePreferences(): void {
    const prefs = this.prefsForm.getRawValue();
    localStorage.setItem(PREFS_KEY, JSON.stringify(prefs));
    this.savedPrefs.set(prefs);
    this.notifications.success('Preferences saved', 'UI preferences stored for this browser.');
  }

  resetPreferences(): void {
    localStorage.removeItem(PREFS_KEY);
    const defaults: UiPreferences = {
      landingPage: '/dashboard',
      autoRefresh: false,
      refreshInterval: environment.evalIntervalMs,
      compactTables: false,
    };
    this.prefsForm.reset(defaults);
    this.savedPrefs.set(defaults);
    this.notifications.info('Preferences reset', 'Defaults restored.');
  }

  private loadPrefs(): UiPreferences {
    const defaults: UiPreferences = {
      landingPage: '/dashboard',
      autoRefresh: false,
      refreshInterval: environment.evalIntervalMs,
      compactTables: false,
    };
    try {
      const raw = localStorage.getItem(PREFS_KEY);
      return raw ? { ...defaults, ...(JSON.parse(raw) as Partial<UiPreferences>) } : defaults;
    } catch {
      return defaults;
    }
  }
}
