using Domain.Entities.Logs;

namespace Domain.Contracts.Logs
{
    public interface ILogsRepo
    {
        // Simple CRUD
        Task AddAsync(Log log);
        void Update(Log log);
        void Delete(Log log);
        Task<IEnumerable<Log>> GetLogsByTraceIdAsync(Guid traceId);
        Task<Log?> GetLogByIdAsync(int logId);
        Task<IEnumerable<Log>> GetLogsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Log>> GetLogsByServiceIdAsync(int serviceId, int pageNumber, int pageSize);

        //Window Functions
        Task<int> GetErrorsNumberByWindowFunction(int serviceId, int time = 5);
        Task<double> GetAvgLatencyPerService(int serviceId, int timeWindowInMin = 5);

        //Helper Functions
        Task<bool> CheckExistAsync(int logId);
        public Task SaveChangesAsync();
    }
}