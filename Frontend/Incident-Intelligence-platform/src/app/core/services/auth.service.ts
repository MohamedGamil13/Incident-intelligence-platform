import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AuthResult,
  AuthSession,
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
  readonly canManage = computed(() =>
    this.roles().some((r) => r === 'Admin' || r === 'IncidentManager')
  );

  login(request: LoginRequest): Observable<ApiResponse<LoginResult>> {
    return this.http
      .post<ApiResponse<LoginResult>>(`${this.baseUrl}/Login`, request)
      .pipe(tap((res) => this.persist(res.data, request.email)));
  }

  register(request: RegisterRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}/Register`, request);
  }

  resetPassword(request: ResetPasswordRequest): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.baseUrl}/ForgetPassword`, request);
  }

  myRoles(): Observable<ApiResponse<string[]>> {
    return this.http.get<ApiResponse<string[]>>(`${this.baseUrl}/my-roles`);
  }

  hasRole(...roles: AppUserRole[]): boolean {
    return this.roles().some((r) => roles.includes(r as AppUserRole));
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
