import { Routes } from '@angular/router';
import { AppShellComponent } from './core/layout/app-shell';
import { authGuard, guestGuard, roleGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    canActivate: [guestGuard],
    children: [
      {
        path: 'login',
        loadComponent: () =>
          import('./features/auth/login/login').then((m) => m.LoginComponent),
        title: 'Sign In · Incident Intelligence',
      },
      {
        path: 'register',
        loadComponent: () =>
          import('./features/auth/register/register').then((m) => m.RegisterComponent),
        title: 'Create Account · Incident Intelligence',
      },
      {
        path: 'forgot-password',
        loadComponent: () =>
          import('./features/auth/forgot-password/forgot-password').then(
            (m) => m.ForgotPasswordComponent
          ),
        title: 'Reset Password · Incident Intelligence',
      },
      { path: '', pathMatch: 'full', redirectTo: 'login' },
    ],
  },
  {
    path: 'errors',
    children: [
      {
        path: '403',
        loadComponent: () =>
          import('./features/errors/forbidden').then((m) => m.ForbiddenComponent),
        title: 'Access Denied · Incident Intelligence',
      },
      {
        path: '404',
        loadComponent: () =>
          import('./features/errors/not-found').then((m) => m.NotFoundComponent),
        title: 'Not Found · Incident Intelligence',
      },
      {
        path: '500',
        loadComponent: () =>
          import('./features/errors/server-error').then((m) => m.ServerErrorComponent),
        title: 'Server Error · Incident Intelligence',
      },
    ],
  },
  {
    path: '',
    component: AppShellComponent,
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard').then((m) => m.DashboardComponent),
        title: 'Dashboard · Incident Intelligence',
      },
      {
        path: 'incidents',
        loadComponent: () =>
          import('./features/incidents/incident-list/incident-list').then(
            (m) => m.IncidentListComponent
          ),
        title: 'Incidents · Incident Intelligence',
      },
      {
        path: 'incidents/new',
        loadComponent: () =>
          import('./features/incidents/incident-create/incident-create').then(
            (m) => m.IncidentCreateComponent
          ),
        title: 'Declare Incident · Incident Intelligence',
      },
      {
        path: 'incidents/:incidentId',
        loadComponent: () =>
          import('./features/incidents/incident-detail/incident-detail').then(
            (m) => m.IncidentDetailComponent
          ),
        title: 'Incident Detail · Incident Intelligence',
      },
      {
        path: 'services',
        loadComponent: () =>
          import('./features/services/services-list/services-list').then(
            (m) => m.ServicesListComponent
          ),
        title: 'Services · Incident Intelligence',
      },
      {
        path: 'services/:serviceId',
        loadComponent: () =>
          import('./features/services/service-detail/service-detail').then(
            (m) => m.ServiceDetailComponent
          ),
        title: 'Service Detail · Incident Intelligence',
      },
      {
        path: 'logs',
        canActivate: [roleGuard('Admin', 'IncidentManager', 'Developer')],
        loadComponent: () => import('./features/logs/logs').then((m) => m.LogsComponent),
        title: 'Logs · Incident Intelligence',
      },
      {
        path: 'users',
        canActivate: [roleGuard('Admin')],
        loadComponent: () => import('./features/users/users').then((m) => m.UsersComponent),
        title: 'Users & Roles · Incident Intelligence',
      },
      {
        path: 'settings',
        loadComponent: () =>
          import('./features/settings/settings').then((m) => m.SettingsComponent),
        title: 'Settings · Incident Intelligence',
      },
    ],
  },
  {
    path: '**',
    loadComponent: () => import('./features/errors/not-found').then((m) => m.NotFoundComponent),
    title: 'Not Found · Incident Intelligence',
  },
];
