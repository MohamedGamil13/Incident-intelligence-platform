using Domain.Contracts;
using Domain.Entities.Logs;
using Domain.Entities.Services;
using Domain.Enums.Logs;
using MediatR;
using ServiceAbstraction.Contracts.Logs;
using ServiceLayer.Services.Logs.Commands;
using ServiceLayer.Services.Specifications;
using Shared.Dtos.Logs;

namespace ServiceLayer.Services.Logs
{
    public class LogsService : ILogsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public LogsService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> CreateLogAsync(CreateLogDto dto, Guid traceId, long LatancyTime)
        {
            var service = await _unitOfWork.GetRepository<Service, int>().GetByIdAsync(dto.ServiceId);
            if (service is null)
                return (false, null, $"Service with ID {dto.ServiceId} does not exist.");

            var log = new Log
            {
                LogLevel = dto.LogLevel ?? LogLevel.Info,
                Message = dto.Message,
                Description = dto.Description,
                ServiceId = dto.ServiceId,
                Service = service,
                TraceId = traceId,
                Timestamp = DateTime.UtcNow,
                LatencyMs = LatancyTime
            };

            await _unitOfWork.GetRepository<Log, int>().AddAsync(log);
            await _unitOfWork.SaveChangesAsync();

            if (log.LogLevel is LogLevel.Error or LogLevel.Critical)
                await _mediator.Publish(new LogIngestedEvent(log, log.ServiceId));

            return (true, MapToResponseDto(log), null);
        }

        public async Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetAllLogsAsync(int pageNumber = 1, int pageSize = 50)
        {
            var logs = await _unitOfWork.GetRepository<Log, int>()
                .GetAllAsync(new LogsPageSpecification(pageNumber, pageSize));

            return (true, logs.Select(MapToResponseDto).ToList(), null);
        }

        public async Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> GetLogByIdAsync(int logId)
        {
            var log = await _unitOfWork.GetRepository<Log, int>()
                .GetByIdAsync(new LogWithServiceSpecification(logId));

            return log is null
                ? (false, null, $"Log with ID {logId} was not found.")
                : (true, MapToResponseDto(log), null);
        }

        public async Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByServiceIdAsync(int serviceId, int pageNumber = 1, int pageSize = 50)
        {
            var service = await _unitOfWork.GetRepository<Service, int>().GetByIdAsync(serviceId);
            if (service is null)
                return (false, Enumerable.Empty<LogResponseDto>(), $"Service with ID {serviceId} does not exist.");

            var logs = await _unitOfWork.GetRepository<Log, int>()
                .GetAllAsync(new LogsPageSpecification(pageNumber, pageSize, serviceId));

            return (true, logs.Select(MapToResponseDto).ToList(), null);
        }

        public async Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByTraceIdAsync(Guid traceId)
        {
            var logs = await _unitOfWork.GetRepository<Log, int>()
                .GetAllAsync(new LogsByTraceSpecification(traceId));

            return (true, logs.Select(MapToResponseDto).ToList(), null);
        }

        private static LogResponseDto MapToResponseDto(Log log) => new()
        {
            Id = log.Id,
            LogLevel = log.LogLevel,
            Message = log.Message,
            Description = log.Description,
            Timestamp = log.Timestamp,
            TraceId = log.TraceId,
            ServiceId = log.ServiceId,
            ServiceName = log.Service?.Name ?? string.Empty
        };
    }
}