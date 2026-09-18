using Shared.Dtos.Logs;

namespace ServiceAbstraction.Contracts.Logs
{
    public interface ILogsService
    {
        Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> CreateLogAsync(CreateLogDto dto, Guid traceId);

        Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetAllLogsAsync();

        Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> GetLogByIdAsync(int logId);

        Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByServiceIdAsync(int serviceId);

        Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByTraceIdAsync(Guid traceId);

    }
}