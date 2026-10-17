import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { CreateLogRequest, LogEntry } from '../models/log.model';

@Injectable({ providedIn: 'root' })
export class LogsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/Logs`;

  getAll(pageNumber = 1, pageSize = 50): Observable<ApiResponse<LogEntry[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<ApiResponse<LogEntry[]>>(this.baseUrl, { params });
  }

  getById(logId: number): Observable<ApiResponse<LogEntry>> {
    return this.http.get<ApiResponse<LogEntry>>(`${this.baseUrl}/${logId}`);
  }

  getByService(
    serviceId: number,
    pageNumber = 1,
    pageSize = 50
  ): Observable<ApiResponse<LogEntry[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<ApiResponse<LogEntry[]>>(`${this.baseUrl}/service/${serviceId}`, {
      params,
    });
  }

  getByTrace(traceId: string): Observable<ApiResponse<LogEntry[]>> {
    return this.http.get<ApiResponse<LogEntry[]>>(`${this.baseUrl}/trace/${traceId}`);
  }

  create(request: CreateLogRequest): Observable<ApiResponse<LogEntry>> {
    return this.http.post<ApiResponse<LogEntry>>(this.baseUrl, request);
  }
}
