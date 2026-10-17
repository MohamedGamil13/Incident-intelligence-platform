import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import {
  CreateServiceDeploymentRequest,
  ServiceDeployment,
} from '../models/deployment.model';

@Injectable({ providedIn: 'root' })
export class ServiceDeploymentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/services`;

  record(
    serviceId: number,
    request: Omit<CreateServiceDeploymentRequest, 'serviceId'>
  ): Observable<ApiResponse<ServiceDeployment>> {
    return this.http.post<ApiResponse<ServiceDeployment>>(
      `${this.baseUrl}/${serviceId}/deployments`,
      { ...request, serviceId }
    );
  }
}
