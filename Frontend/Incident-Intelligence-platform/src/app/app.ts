import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastContainerComponent } from './shared/components/toast-container';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastContainerComponent],
  templateUrl: './app.html',
})
export class App {
  private readonly auth = inject(AuthService);

  constructor() {
    if (this.auth.isAuthenticated()) {
      this.auth.refreshRoles().subscribe({ error: () => undefined });
    }
  }
}
