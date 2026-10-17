export interface CreateServiceDeploymentRequest {
  title: string;
  version: string;
  deployedBy: string;
  serviceId: number;
}

export interface ServiceDeployment {
  id: number;
  serviceId: number;
  title: string;
  version: string;
  deployedBy: string;
  deployedAt: string;
}
