using Domain.Contracts.Logs;
using Domain.Contracts.Services;
using Domain.Entities.Logs;
using Domain.Enums.Logs;
using ServiceAbstraction.Contracts.Logs;
using Shared.Dtos.Logs;

namespace ServiceLayer.Services.Logs
{
    public class LogsService : ILogsService
    {
        private readonly ILogsRepo _logsRepo;
        private readonly IServiceRepo _serviceRepo;

        public LogsService(ILogsRepo logsRepo, IServiceRepo serviceRepo)
        {
            _logsRepo = logsRepo;
            _serviceRepo = serviceRepo;
        }

        public async Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> CreateLogAsync(CreateLogDto dto, Guid traceId)
        {
            var serviceExists = await _serviceRepo.GetByIdAsync(dto.ServiceId);
            if (serviceExists == null)
            {
                return (false, null, $"Service with ID {dto.ServiceId} does not exist.");
            }

            var log = new Log
            {
                LogLevel = dto.LogLevel ?? LogLevel.Info,
                Message = dto.Message,
                Description = dto.Description,
                ServiceId = dto.ServiceId,
                TraceId = traceId,
                Timestamp = DateTime.UtcNow
            };

            await _logsRepo.AddAsync(log);
            await _serviceRepo.SaveChangesAsync();


            var createdLog = await _logsRepo.GetLogByIdAsync(log.Id);

            return (true, MapToResponseDto(createdLog!), null);
        }

        public async Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetAllLogsAsync(int pageNumber = 1, int pageSize = 50)
        {
            var logs = await _logsRepo.GetLogsAsync(pageNumber, pageSize);
            var result = logs.Select(MapToResponseDto);

            return (true, result, null);
        }

        public async Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> GetLogByIdAsync(int logId)
        {
            var log = await _logsRepo.GetLogByIdAsync(logId);
            if (log == null)
            {
                return (false, null, $"Log with ID {logId} was not found.");
            }

            return (true, MapToResponseDto(log), null);
        }

        public async Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByServiceIdAsync(int serviceId, int pageNumber = 1, int pageSize = 50)
        {
            var serviceExists = await _serviceRepo.GetByIdAsync(serviceId);
            if (serviceExists == null)
            {
                return (false, Enumerable.Empty<LogResponseDto>(), $"Service with ID {serviceId} does not exist.");
            }

            var logs = await _logsRepo.GetLogsByServiceIdAsync(serviceId, pageNumber, pageSize);
            var result = logs.Select(MapToResponseDto);

            return (true, result, null);
        }

        public async Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByTraceIdAsync(Guid traceId)
        {
            var logs = await _logsRepo.GetLogsByTraceIdAsync(traceId);
            var result = logs.Select(MapToResponseDto);

            return (true, result, null);
        }

        #region Private Helper Methods
        private static LogResponseDto MapToResponseDto(Log log)
        {
            return new LogResponseDto
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

        #endregion
    }
}