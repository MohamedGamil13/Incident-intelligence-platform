using Domain.Contracts.Logs;
using Domain.Entities.Logs;

namespace Presistance.Repositories.Logs
{
    public class LogsRepo : ILogsRepo
    {
        public Task Add(Log log)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckExist(Log log)
        {
            throw new NotImplementedException();
        }

        public void Delete(Log log)
        {
            throw new NotImplementedException();
        }

        public Task<Log> GetLog(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Log>> GetLogs(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Update(Log log)
        {
            throw new NotImplementedException();
        }
    }
}
