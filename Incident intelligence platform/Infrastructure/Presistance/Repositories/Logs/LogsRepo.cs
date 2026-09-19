using Domain.Contracts.Logs;
using Domain.Entities.Logs;
using Domain.Enums.Logs;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories.Logs
{
    public class LogsRepo : ILogsRepo
    {
        private readonly AppDbcontext _context;

        public LogsRepo(AppDbcontext context)
        {
            _context = context;
        }

        public async Task AddAsync(Log log)
        {
            await _context.Logs.AddAsync(log);
        }

        public async Task<bool> CheckExistAsync(int logId)
        {
            return await _context.Logs
                .AsNoTracking()
                .AnyAsync(l => l.Id == logId);
        }

        public void Delete(Log log)
        {
            _context.Logs.Remove(log);
        }

        public void Update(Log log)
        {
            _context.Logs.Update(log);
        }

        public async Task<Log?> GetLogByIdAsync(int logId)
        {
            return await _context.Logs
                .AsNoTracking()
                .Include(l => l.Service)
                .FirstOrDefaultAsync(l => l.Id == logId);
        }

        public async Task<IEnumerable<Log>> GetLogsAsync(int pageNumber, int pageSize)
        {
            return await _context.Logs
                .AsNoTracking()
                .Include(l => l.Service)
                .OrderByDescending(l => l.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetLogsByServiceIdAsync(int serviceId, int pageNumber, int pageSize)
        {
            return await _context.Logs
                .AsNoTracking()
                .Include(l => l.Service)
                .Where(l => l.ServiceId == serviceId)
                .OrderByDescending(l => l.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetLogsByTraceIdAsync(Guid traceId)
        {
            return await _context.Logs
                .AsNoTracking()
                .Include(l => l.Service)
                .Where(l => l.TraceId == traceId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<int> GetErrorsNumberByWindowFunction(int serviceId, int timeWindowInMinutes = 5)
        {
            var windowTime = DateTime.UtcNow.AddMinutes(-timeWindowInMinutes);

            return await _context.Logs
                .AsNoTracking()
                .Where(l => l.ServiceId == serviceId
                         && l.LogLevel >= LogLevel.Error
                         && l.Timestamp >= windowTime)
                .CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}