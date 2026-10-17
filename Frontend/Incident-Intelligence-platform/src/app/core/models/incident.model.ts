import { IncidentSeverity, IncidentStatus } from './enums';

export interface Incident {
  id: number;
  title: string;
  description: string;
  severity: IncidentSeverity;
  status: IncidentStatus;
  serviceId: number;
  serviceName: string | null;
  createdAt: string;
  resolvedAt: string | null;
}

export interface CreateIncidentRequest {
  title: string;
  description: string;
  severity: IncidentSeverity;
  serviceId: number;
}

export interface UpdateIncidentRequest {
  title: string;
  description: string;
  severity: IncidentSeverity;
  status: IncidentStatus;
}
