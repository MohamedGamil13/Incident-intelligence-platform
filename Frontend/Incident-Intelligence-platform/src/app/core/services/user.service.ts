import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { AppUser } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/UserMangement`;

  getAll(pageNumber = 1, pageSize = 10): Observable<ApiResponse<AppUser[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<ApiResponse<AppUser[]>>(this.baseUrl, { params });
  }

  getByRole(role: string, pageNumber = 1, pageSize = 10): Observable<ApiResponse<AppUser[]>> {
    const params = new HttpParams()
      .set('role', role)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<ApiResponse<AppUser[]>>(`${this.baseUrl}/by-role`, { params });
  }

  delete(userId: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.baseUrl}/${userId}`);
  }

  addRole(userId: string, role: string): Observable<ApiResponse<string>> {
    const params = new HttpParams().set('role', role);
    return this.http.post<ApiResponse<string>>(`${this.baseUrl}/${userId}/roles`, null, {
      params,
    });
  }
}
