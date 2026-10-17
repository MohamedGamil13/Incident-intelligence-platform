export type IncidentSeverity = 'Low' | 'Medium' | 'High' | 'Critical';
export type IncidentStatus = 'Open' | 'Investigating' | 'Mitigated' | 'Resolved';
export type IncidentEventTimeStamp = 'Created' | 'Working' | 'Solved';
export type LogLevel = 'Info' | 'Warn' | 'Error' | 'Critical';
export type AppUserRole = 'Admin' | 'IncidentManager' | 'Developer' | 'Viewer';

export const INCIDENT_SEVERITIES: IncidentSeverity[] = ['Low', 'Medium', 'High', 'Critical'];
export const INCIDENT_STATUSES: IncidentStatus[] = ['Open', 'Investigating', 'Mitigated', 'Resolved'];
export const INCIDENT_EVENT_TIMESTAMPS: IncidentEventTimeStamp[] = ['Created', 'Working', 'Solved'];
export const LOG_LEVELS: LogLevel[] = ['Info', 'Warn', 'Error', 'Critical'];
export const APP_USER_ROLES: AppUserRole[] = ['Admin', 'IncidentManager', 'Developer', 'Viewer'];
