import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse, IncidentSpecParams } from '../models/api-response.model';
import {
  CreateIncidentRequest,
  Incident,
  UpdateIncidentRequest,
} from '../models/incident.model';

@Injectable({ providedIn: 'root' })
export class IncidentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/Incidents`;

  getAll(params: IncidentSpecParams = {}): Observable<ApiResponse<Incident[]>> {
    let httpParams = new HttpParams();
    if (params.pageIndex != null) httpParams = httpParams.set('pageIndex', params.pageIndex);
    if (params.pageSize != null) httpParams = httpParams.set('pageSize', params.pageSize);
    if (params.serviceId != null) httpParams = httpParams.set('serviceId', params.serviceId);
    if (params.status) httpParams = httpParams.set('status', params.status);
    if (params.sort) httpParams = httpParams.set('sort', params.sort);
    if (params.search) httpParams = httpParams.set('search', params.search);
    return this.http.get<ApiResponse<Incident[]>>(this.baseUrl, { params: httpParams });
  }

  getById(incidentId: number): Observable<ApiResponse<Incident>> {
    return this.http.get<ApiResponse<Incident>>(`${this.baseUrl}/${incidentId}`);
  }

  create(request: CreateIncidentRequest): Observable<ApiResponse<Incident>> {
    return this.http.post<ApiResponse<Incident>>(this.baseUrl, request);
  }

  update(incidentId: number, request: UpdateIncidentRequest): Observable<ApiResponse<Incident>> {
    return this.http.put<ApiResponse<Incident>>(`${this.baseUrl}/${incidentId}`, request);
  }

  delete(incidentId: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${incidentId}`);
  }
}
