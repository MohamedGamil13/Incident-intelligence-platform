using Domain.Contracts.Logs;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories.Logs
{
    public class LogsRepo : ILogsRepo
    {
        private readonly AppDbcontext _context;

        public LogsRepo(AppDbcontext context) => _context = context;

        public async Task<double> GetAvgLatencyPerService(int serviceId, int timeWindowInMin = 5)
        {
            var since = DateTime.UtcNow.AddMinutes(-timeWindowInMin);

            var avg = await _context.Logs
                .AsNoTracking()
                .Where(l => l.ServiceId == serviceId && l.Timestamp >= since)
                .Select(l => (double?)l.LatencyMs)
                .AverageAsync();

            return avg ?? 0.0;
        }
    }
}