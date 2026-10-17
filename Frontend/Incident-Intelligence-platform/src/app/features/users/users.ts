import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { forkJoin, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserService } from '../../core/services/user.service';
import { AuthService } from '../../core/services/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { AppUser } from '../../core/models/user.model';
import { APP_USER_ROLES, AppUserRole } from '../../core/models/enums';
import { PaginationComponent } from '../../shared/components/pagination';
import { environment } from '../../../environments/environment';

interface UserRow {
  user: AppUser;
  roles: AppUserRole[];
}

@Component({
  selector: 'app-users',
  imports: [ReactiveFormsModule, PaginationComponent],
  templateUrl: './users.html',
})
export class UsersComponent {
  private readonly fb = inject(FormBuilder);
  private readonly userService = inject(UserService);
  private readonly auth = inject(AuthService);
  private readonly notifications = inject(NotificationService);

  readonly allRoles = APP_USER_ROLES;
  readonly isAdmin = this.auth.isAdmin;
  readonly currentEmail = this.auth.userEmail;

  readonly rows = signal<UserRow[]>([]);
  readonly loading = signal(true);
  readonly pageIndex = signal(1);
  readonly pageSize = signal(environment.defaultPageSize);
  readonly hasNext = signal(false);

  readonly pendingDelete = signal<UserRow | null>(null);
  readonly assignTarget = signal<UserRow | null>(null);
  readonly submitting = signal(false);

  readonly filters = this.fb.nonNullable.group({
    search: [''],
    role: [''],
  });

  readonly assignForm = this.fb.nonNullable.group({
    role: ['Viewer' as AppUserRole],
  });

  readonly visible = computed<UserRow[]>(() => {
    const term = this.filters.controls.search.value.trim().toLowerCase();
    const role = this.filters.controls.role.value;
    return this.rows().filter((row) => {
      const matchesRole = !role || row.roles.includes(role as AppUserRole);
      if (!matchesRole) return false;
      if (!term) return true;
      const name = (row.user.userName ?? '').toLowerCase();
      const email = (row.user.email ?? '').toLowerCase();
      return name.includes(term) || email.includes(term);
    });
  });

  readonly stats = computed(() => {
    const rows = this.rows();
    return {
      total: rows.length,
      admins: rows.filter((r) => r.roles.includes('Admin')).length,
      responders: rows.filter(
        (r) => r.roles.includes('IncidentManager') || r.roles.includes('Developer')
      ).length,
      confirmed: rows.filter((r) => r.user.emailConfirmed).length,
    };
  });

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.userService.getAll(this.pageIndex(), this.pageSize()).subscribe({
      next: (res) => {
        const users = res.data ?? [];
        this.hasNext.set(users.length === this.pageSize());
        if (users.length === 0) {
          this.rows.set([]);
          this.loading.set(false);
          return;
        }
        this.attachRoles(users);
      },
      error: () => {
        this.loading.set(false);
        this.notifications.error('Load failed', 'Unable to retrieve platform users.');
      },
    });
  }

  private attachRoles(users: AppUser[]): void {
    const roleQueries = this.allRoles.map((role) =>
      this.userService.getByRole(role, 1, environment.maxPageSize).pipe(
        map((res) => ({ role, ids: (res.data ?? []).map((u) => u.id) })),
        catchError(() => of({ role, ids: [] as string[] }))
      )
    );

    forkJoin(roleQueries).subscribe({
      next: (results) => {
        const rows: UserRow[] = users.map((user) => ({
          user,
          roles: results.filter((r) => r.ids.includes(user.id)).map((r) => r.role),
        }));
        this.rows.set(rows);
        this.loading.set(false);
      },
      error: () => {
        this.rows.set(users.map((user) => ({ user, roles: [] })));
        this.loading.set(false);
      },
    });
  }

  changePage(next: number): void {
    if (next < 1) return;
    this.pageIndex.set(next);
    this.load();
  }

  openAssign(row: UserRow): void {
    if (!this.isAdmin()) {
      this.notifications.warning('Permission denied', 'Only administrators can modify roles.');
      return;
    }
    this.assignForm.reset({ role: 'Viewer' });
    this.assignTarget.set(row);
  }

  confirmAssign(): void {
    const target = this.assignTarget();
    if (!target) return;
    this.submitting.set(true);
    this.userService.addRole(target.user.id, this.assignForm.controls.role.value).subscribe({
      next: () => {
        this.submitting.set(false);
        this.assignTarget.set(null);
        this.notifications.success('Role assigned', `${this.displayName(target.user)} updated.`);
        this.load();
      },
      error: () => {
        this.submitting.set(false);
        this.notifications.error('Assignment failed', 'Could not assign the role.');
      },
    });
  }

  askDelete(row: UserRow): void {
    this.pendingDelete.set(row);
  }

  confirmDelete(): void {
    const target = this.pendingDelete();
    if (!target) return;
    this.submitting.set(true);
    this.userService.delete(target.user.id).subscribe({
      next: () => {
        this.submitting.set(false);
        this.pendingDelete.set(null);
        this.notifications.success('User removed', `${this.displayName(target.user)} deleted.`);
        this.load();
      },
      error: () => {
        this.submitting.set(false);
        this.notifications.error('Delete failed', 'Could not delete the user.');
      },
    });
  }

  resetFilters(): void {
    this.filters.reset({ search: '', role: '' });
  }

  displayName(user: AppUser): string {
    return user.userName ?? user.email ?? 'Unknown user';
  }

  initials(user: AppUser): string {
    const name = user.userName ?? user.email ?? '?';
    return name.slice(0, 2).toUpperCase();
  }

  statusLabel(user: AppUser): { label: string; classes: string } {
    if (user.lockoutEnd && new Date(user.lockoutEnd) > new Date()) {
      return { label: 'Locked', classes: 'border-sev-high bg-sev-high-bg text-sev-high' };
    }
    if (user.emailConfirmed) {
      return { label: 'Active', classes: 'border-sev-resolved bg-sev-resolved-bg text-sev-resolved' };
    }
    return { label: 'Pending', classes: 'border-border-subtle bg-surface-container text-secondary' };
  }
}
