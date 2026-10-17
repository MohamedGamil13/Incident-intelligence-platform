import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import {
  CreateServiceRequest,
  Service,
  UpdateServiceRequest,
} from '../models/service.model';

@Injectable({ providedIn: 'root' })
export class ServiceCatalogService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/Services`;

  getAll(pageNumber = 1, pageSize = environment.maxPageSize): Observable<ApiResponse<Service[]>> {
    return this.http.get<ApiResponse<Service[]>>(`${this.baseUrl}/${pageNumber}/${pageSize}`);
  }

  getById(serviceId: number): Observable<ApiResponse<Service>> {
    return this.http.get<ApiResponse<Service>>(`${this.baseUrl}/${serviceId}`);
  }

  create(request: CreateServiceRequest): Observable<ApiResponse<Service>> {
    return this.http.post<ApiResponse<Service>>(this.baseUrl, request);
  }

  update(serviceId: number, request: UpdateServiceRequest): Observable<ApiResponse<Service>> {
    return this.http.put<ApiResponse<Service>>(`${this.baseUrl}/${serviceId}`, request);
  }

  delete(serviceId: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${serviceId}`);
  }
}
