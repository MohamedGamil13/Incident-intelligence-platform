import { Injectable, signal } from '@angular/core';

export interface PendingPasswordReset {
  email: string;
  newPassword: string;
  confirmPassword: string;
}

@Injectable({ providedIn: 'root' })
export class PasswordResetStateService {
  private readonly _pending = signal<PendingPasswordReset | null>(null);

  readonly pending = this._pending.asReadonly();

  set(state: PendingPasswordReset): void {
    this._pending.set(state);
  }

  patchEmail(email: string): void {
    const current = this._pending();
    if (current) {
      this._pending.set({ ...current, email });
    }
  }

  clear(): void {
    this._pending.set(null);
  }
}
