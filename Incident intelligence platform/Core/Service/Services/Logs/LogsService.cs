using ServiceAbstraction.Contracts.Logs;
using Shared.Dtos.Logs;

namespace ServiceLayer.Services.Logs
{
    public class LogsService : ILogsService
    {
        public Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> CreateLogAsync(CreateLogDto dto, Guid traceId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetAllLogsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> GetLogByIdAsync(int logId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByServiceIdAsync(int serviceId)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByTraceIdAsync(Guid traceId)
        {
            throw new NotImplementedException();
        }
    }
}
