import { IncidentEventTimeStamp, IncidentStatus } from './enums';

export interface IncidentEvent {
  id: number;
  timeStamp: IncidentEventTimeStamp;
  description: string;
  title: string;
  date: string;
  incidentId: number;
  incident?: unknown;
}

export interface AddIncidentEventRequest {
  timeStamp: IncidentEventTimeStamp;
  title: string;
  description: string;
}

export interface AddIncidentEventResponse {
  incidentId: number;
  incidentName: string;
  title: string;
  description: string;
  icidentStatus: IncidentStatus;
  incidentDate: string;
}
