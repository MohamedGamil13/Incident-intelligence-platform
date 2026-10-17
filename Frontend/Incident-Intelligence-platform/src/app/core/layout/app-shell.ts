import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../services/auth.service';

interface NavItem {
  label: string;
  icon: string;
  path: string;
  roles?: string[];
  badge?: string;
}

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app-shell.html',
})
export class AppShellComponent {
  private readonly auth = inject(AuthService);

  readonly sidebarOpen = signal(false);
  readonly profileOpen = signal(false);
  readonly email = this.auth.userEmail;
  readonly roles = this.auth.roles;

  private readonly allNav: NavItem[] = [
    { label: 'Dashboard', icon: 'dashboard', path: '/dashboard' },
    { label: 'Incidents', icon: 'crisis_alert', path: '/incidents' },
    { label: 'Services', icon: 'dns', path: '/services' },
    { label: 'Logs', icon: 'receipt_long', path: '/logs', roles: ['Admin', 'IncidentManager', 'Developer'] },
    { label: 'Users & Roles', icon: 'group', path: '/users', roles: ['Admin'] },
    { label: 'Settings', icon: 'settings', path: '/settings' },
  ];

  readonly navItems = computed(() =>
    this.allNav.filter((item) => !item.roles || this.auth.hasRole(...(item.roles as never[])))
  );

  readonly initials = computed(() => {
    const email = this.email();
    return email ? email.charAt(0).toUpperCase() : 'U';
  });

  toggleSidebar(): void {
    this.sidebarOpen.update((v) => !v);
  }

  closeSidebar(): void {
    this.sidebarOpen.set(false);
  }

  toggleProfile(): void {
    this.profileOpen.update((v) => !v);
  }

  logout(): void {
    this.auth.logout();
  }
}
