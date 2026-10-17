import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { NotificationService } from '../../core/services/notification.service';
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
  imports: [ReactiveFormsModule],
  templateUrl: './settings.html',
})
export class SettingsComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly notifications = inject(NotificationService);

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

  readonly submitting = signal(false);
  readonly savedPrefs = signal<UiPreferences>(this.loadPrefs());

  showNew = false;
  showConfirm = false;

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

  readonly prefsForm = this.fb.nonNullable.group({
    landingPage: [this.savedPrefs().landingPage],
    autoRefresh: [this.savedPrefs().autoRefresh],
    refreshInterval: [this.savedPrefs().refreshInterval],
    compactTables: [this.savedPrefs().compactTables],
  });

  initials(value: string | null): string {
    return (value ?? '?').slice(0, 2).toUpperCase();
  }

  submitPassword(): void {
    const { newPassword, confirmPassword } = this.passwordForm.getRawValue();
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }
    if (newPassword !== confirmPassword) {
      this.notifications.error('Passwords do not match', 'Confirm password must match the new password.');
      return;
    }
    const address = this.email();
    if (!address) {
      this.notifications.error('No session email', 'Sign in again to change your password.');
      return;
    }
    this.submitting.set(true);
    this.auth.resetPassword({ email: address, newPassword, confirmPassword }).subscribe({
      next: () => {
        this.submitting.set(false);
        this.passwordForm.reset({ newPassword: '', confirmPassword: '' });
        this.notifications.success('Password updated', 'Use your new credentials on next sign-in.');
      },
      error: () => {
        this.submitting.set(false);
        this.notifications.error('Update failed', 'The password could not be changed.');
      },
    });
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
