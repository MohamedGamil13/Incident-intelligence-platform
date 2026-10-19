import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, map, of, switchMap, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AuthSession,
  ForgetPasswordRequest,
  LoginRequest,
  LoginResult,
  RegisterRequest,
  ResetPasswordRequest,
} from '../models/auth.model';
import { ApiResponse } from '../models/api-response.model';
import { AppUserRole } from '../models/enums';
import { decodeJwt } from '../utils/jwt.util';

const STORAGE_KEY = 'iip.auth.session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly baseUrl = `${environment.apiBaseUrl}/Account`;

  private readonly _session = signal<AuthSession | null>(this.restore());

  readonly session = this._session.asReadonly();
  readonly isAuthenticated = computed(() => {
    const session = this._session();
    if (!session) return false;
    return !session.expires || new Date(session.expires).getTime() > Date.now();
  });
  readonly userEmail = computed(() => this._session()?.email ?? null);
  readonly roles = computed(() => this._session()?.roles ?? []);
  readonly isAdmin = computed(() => this.roles().includes('Admin'));
  readonly isIncidentManager = computed(() => this.roles().includes('IncidentManager'));
  readonly isDeveloper = computed(() => this.roles().includes('Developer'));
  readonly canManage = computed(() =>
    this.roles().some((r) => r === 'Admin' || r === 'IncidentManager')
  );

  login(request: LoginRequest): Observable<ApiResponse<LoginResult>> {
    return this.http
      .post<ApiResponse<LoginResult>>(`${this.baseUrl}/Login`, request)
      .pipe(
        tap((res) => this.persist(res.data, request.email)),
        switchMap((res) =>
          this.refreshRoles().pipe(
            map(() => res),
            catchError(() => of(res))
          )
        )
      );
  }

  register(request: RegisterRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}/Register`, request);
  }

  requestPasswordReset(request: ForgetPasswordRequest): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.baseUrl}/ForgetPassword`, request);
  }

  resetPassword(request: ResetPasswordRequest): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.baseUrl}/ResetPassword`, request);
  }

  myRoles(): Observable<ApiResponse<string[]>> {
    return this.http.get<ApiResponse<string[]>>(`${this.baseUrl}/my-roles`);
  }

  refreshRoles(): Observable<string[]> {
    return this.myRoles().pipe(
      map((res) => res.data ?? []),
      tap((roles) => this.updateRoles(roles))
    );
  }

  hasRole(...roles: AppUserRole[]): boolean {
    return this.roles().some((r) => roles.includes(r as AppUserRole));
  }

  canCreateIncident(): boolean {
    return this.canManage();
  }
  canUpdateIncident(): boolean {
    return this.hasRole('Admin', 'IncidentManager', 'Developer');
  }
  canDeleteIncident(): boolean {
    return this.canManage();
  }
  canAddIncidentEvent(): boolean {
    return this.hasRole('Admin', 'IncidentManager', 'Developer');
  }

  get token(): string | null {
    return this._session()?.token ?? null;
  }

  logout(redirect = true): void {
    this._session.set(null);
    localStorage.removeItem(STORAGE_KEY);
    if (redirect) {
      this.router.navigate(['/auth/login']);
    }
  }

  private persist(data: LoginResult, email: string): void {
    const payload = decodeJwt(data.token);
    const rawRole = payload?.role;
    const roles = Array.isArray(rawRole) ? rawRole : rawRole ? [rawRole] : [];
    const session: AuthSession = {
      token: data.token,
      expires: data.expires,
      email: (payload?.email as string) ?? email,
      roles,
    };
    this._session.set(session);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
  }

  private updateRoles(roles: string[]): void {
    const current = this._session();
    if (!current) return;
    const session: AuthSession = { ...current, roles };
    this._session.set(session);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
  }

  private restore(): AuthSession | null {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (!raw) return null;
      const session = JSON.parse(raw) as AuthSession;
      if (session.expires && new Date(session.expires).getTime() <= Date.now()) {
        localStorage.removeItem(STORAGE_KEY);
        return null;
      }
      return session;
    } catch {
      return null;
    }
  }
}
