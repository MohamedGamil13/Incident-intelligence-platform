using Domain.Entities.Logs;

namespace Domain.Contracts.Logs
{
    public interface ILogsRepo
    {
        public Task<IEnumerable<Log>> GetLogs(int pageNumber, int pageSize);
        public Task<Log> GetLog(int Id);
        public void Delete(Log log);
        public void Update(Log log);
        public Task Add(Log log);
        public Task<bool> CheckExist(Log log);
    }
}