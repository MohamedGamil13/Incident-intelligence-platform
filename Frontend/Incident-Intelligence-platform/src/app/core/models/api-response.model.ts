export interface ApiResponse<T> {
  isSuccess: boolean;
  statusCode: number;
  message: string;
  data: T;
  errors: string[] | null;
}

export interface PaginatedParams {
  pageNumber?: number;
  pageSize?: number;
}

export interface IncidentSpecParams {
  pageIndex?: number;
  pageSize?: number;
  serviceId?: number | null;
  status?: string | null;
  sort?: string | null;
  search?: string | null;
}
