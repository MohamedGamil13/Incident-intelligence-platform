export interface Service {
  id: number;
  name: string;
  description: string;
}

export interface CreateServiceRequest {
  name: string;
  description: string;
}

export interface UpdateServiceRequest {
  name: string;
  description: string;
}
