import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import {
  AddIncidentEventRequest,
  AddIncidentEventResponse,
  IncidentEvent,
} from '../models/incident-event.model';

@Injectable({ providedIn: 'root' })
export class IncidentEventService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/incidents`;

  getTimeline(
    incidentId: number,
    pageNumber = 1,
    pageSize = 10
  ): Observable<ApiResponse<IncidentEvent[]>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get<ApiResponse<IncidentEvent[]>>(
      `${this.baseUrl}/${incidentId}/timeline`,
      { params }
    );
  }

  addEvent(
    incidentId: number,
    request: AddIncidentEventRequest
  ): Observable<ApiResponse<AddIncidentEventResponse>> {
    return this.http.post<ApiResponse<AddIncidentEventResponse>>(
      `${this.baseUrl}/${incidentId}/events`,
      request
    );
  }
}
