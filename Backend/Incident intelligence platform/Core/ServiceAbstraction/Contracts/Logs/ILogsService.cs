using Shared.Dtos.Logs;

namespace ServiceAbstraction.Contracts.Logs
{
    public interface ILogsService
    {
        Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> CreateLogAsync(CreateLogDto dto, Guid traceId, long LatancyTime);

        Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetAllLogsAsync(int pageNumber = 1, int pageSize = 50);

        Task<(bool Success, LogResponseDto? Data, string? ErrorMessage)> GetLogByIdAsync(int logId);

        Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByServiceIdAsync(int serviceId, int pageNumber = 1, int pageSize = 50);

        Task<(bool Success, IEnumerable<LogResponseDto> Data, string? ErrorMessage)> GetLogsByTraceIdAsync(Guid traceId);

    }
}