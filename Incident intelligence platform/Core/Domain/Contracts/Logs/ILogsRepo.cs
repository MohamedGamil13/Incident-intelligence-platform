using Domain.Entities.Logs;

namespace Domain.Contracts.Logs
{
    public interface ILogsRepo
    {
        Task<IEnumerable<Log>> GetLogsAsync(int pageNumber, int pageSize);
        Task<Log?> GetLogByIdAsync(int logId);
        Task<IEnumerable<Log>> GetLogsByServiceIdAsync(int serviceId, int pageNumber, int pageSize);
        Task<IEnumerable<Log>> GetLogsByTraceIdAsync(Guid traceId);
        Task AddAsync(Log log);
        void Update(Log log);
        void Delete(Log log);
        Task<bool> CheckExistAsync(int logId);
        Task<int> GetErrorsNumberByWindowFunction(int serviceId, int time = 5);
        public Task SaveChangesAsync();
    }
}