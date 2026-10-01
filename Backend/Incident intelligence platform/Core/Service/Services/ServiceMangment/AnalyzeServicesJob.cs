using Domain.Contracts;
using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Contracts.Services;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using Incident_intelligence_platform.DTOs.IncidentDTOs;
using ServiceAbstraction.Contracts.ServiceMangment;
using ServiceLayer.Services.Specifications;
using Shared.Dtos.ServiceDTOs;

namespace ServiceLayer.Services.ServiceMangment
{
    public class AnalyzeServicesJob : IAnalyzeServiceJob
    {
        private readonly IIncidentRepo _incidentRepo;
        private readonly ILogsRepo _logsRepo;
        private readonly IServiceRepo _serviceRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AnalyzeServicesJob(
            IIncidentRepo incidentRepo,
            ILogsRepo logsRepo,
            IServiceRepo serviceRepo,
            IUnitOfWork unitOfWork)
        {
            _incidentRepo = incidentRepo;
            _logsRepo = logsRepo;
            _serviceRepo = serviceRepo;
            _unitOfWork = unitOfWork;
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

            // استخدام CountAsync المعتمدة على الـ Specification بدلاً من الدالة المحذوفة
            var spec = new ActiveIncidentsPerServiceSpecification(serviceId, dto.IncidentTimeWindow);
            var activeIncidentsCount = await _incidentRepo.CountAsync(spec);

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
            }

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
            }

            if (avgLatency > maxLatency)
            {
                await CreateIncident(new CreateIncidentRequestDTO
                {
                    ServiceId = serviceId,
                    Title = $"High Latency / Slow Performance on Service #{serviceId}",
                    Description = $"PERFORMANCE DEGRADATION: Average latency for Service #{serviceId} reached {avgLatency:F2} ms over the last {dto.ServiceErrorTimeWindow} minutes (Exceeded max threshold of {maxLatency} ms).",
                    Severity = IncidentSeverity.Medium
                });
            }
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
            await _unitOfWork.SaveChangesAsync();
        }
    }
}