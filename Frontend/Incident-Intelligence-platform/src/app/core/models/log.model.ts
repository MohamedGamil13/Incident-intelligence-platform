import { LogLevel } from './enums';

export interface LogEntry {
  id: number;
  logLevel: LogLevel;
  message: string;
  description: string;
  timestamp: string;
  traceId: string;
  serviceId: number;
  serviceName: string;
}

export interface CreateLogRequest {
  logLevel: LogLevel;
  message: string;
  description: string;
  serviceId: number;
  latencyMs: number;
}
