using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Contracts.Services;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using ServiceAbstraction.Contracts.ServiceMangment;
using Shared.Dtos.ServiceDTOs;

namespace ServiceLayer.Services.ServiceMangment
{
    public class AnalyzeServicesJob : IAnalyzeServiceJob
    {
        private readonly IIncidentRepo _incidentRepo;
        private readonly ILogsRepo _logsRepo;
        private readonly IServiceRepo _serviceRepo;

        public AnalyzeServicesJob(IIncidentRepo incidentRepo, ILogsRepo logsRepo, IServiceRepo serviceRepo)
        {
            _incidentRepo = incidentRepo;
            _logsRepo = logsRepo;
            _serviceRepo = serviceRepo;
        }

        public async Task AnalyzeAllServices(AnalyzeAllServicesRequest dto)
        {
            var servicesId = await _serviceRepo.GetAllServiceIdsAsync();
            foreach (int serviceId in servicesId)
            {
                await AnalyzeOneService(new AnalyzeServiceRequest(dto, serviceId));
            }
        }

        public async Task AnalyzeOneService(AnalyzeServiceRequest dto)
        {
            long maxLatency = dto.MaxLatancy;
            int serviceId = dto.ServiceId;

            var activeIncidentsCount = await _incidentRepo.GetActiveIncidentCountPerService(serviceId, dto.MaxIncidentsPerService);
            var errorCount = await _logsRepo.GetErrorsNumberByWindowFunction(serviceId, dto.ServiceErrorTimeWindow);
            var avgLatency = await _logsRepo.GetAvgLatencyPerService(serviceId, dto.ServiceErrorTimeWindow);

            if (activeIncidentsCount > dto.MaxIncidentsPerService)
            {
                await CreateIncident(new CreateIncidentRequestDTO
                {
                    ServiceId = serviceId,
                    Title = $"High Incident Accumulation on Service #{serviceId}",
                    Description = $"CRITICAL: Service #{serviceId} currently has {activeIncidentsCount} active incidents within the last {dto.IncidentTimeWindow} minutes, exceeding the threshold of {dto.MaxIncidentsPerService}.",
                    Severity = IncidentSeverity.High
                });
            } //High Amout of Incident in Service


            int maxAllowedErrors = dto.MaxErrorsPerService;
            if (errorCount > maxAllowedErrors)
            {
                await CreateIncident(new CreateIncidentRequestDTO
                {
                    ServiceId = serviceId,
                    Title = $"High Error Rate Threshold Exceeded on Service #{serviceId}",
                    Description = $"WARNING: Service #{serviceId} reported {errorCount} errors within the last {dto.ServiceErrorTimeWindow} minutes window. Threshold is {maxAllowedErrors} errors.",
                    Severity = IncidentSeverity.Critical
                });
            }//High amount of Errors


            if (avgLatency > maxLatency)
            {
                await CreateIncident(new CreateIncidentRequestDTO
                {
                    ServiceId = serviceId,
                    Title = $"High Latency / Slow Performance on Service #{serviceId}",
                    Description = $"PERFORMANCE DEGRADATION: Average latency for Service #{serviceId} reached {avgLatency:F2} ms over the last {dto.ServiceErrorTimeWindow} minutes (Exceeded max threshold of {maxLatency} ms).",
                    Severity = IncidentSeverity.Medium
                });
            }//High Latency 
        }


        public async Task CreateIncident(CreateIncidentRequestDTO dto)
        {
            var incident = new Incident
            {
                Title = dto.Title,
                Description = dto.Description,
                Severity = dto.Severity ?? IncidentSeverity.Medium,
                ServiceId = dto.ServiceId,
                Status = IncidentStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _incidentRepo.AddAsync(incident);
            await _incidentRepo.SaveChangesAsync();
        }
    }
}