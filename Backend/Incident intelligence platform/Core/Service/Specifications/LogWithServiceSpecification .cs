using Domain.Entities.Logs;

namespace ServiceLayer.Services.Specifications
{
    public class LogWithServiceSpecification : BaseSpecification<Log, int>
    {
        public LogWithServiceSpecification(int id) : base(l => l.Id == id)
        {
            AddInclude(l => l.Service!);
        }
    }

    public class LogsPageSpecification : BaseSpecification<Log, int>
    {
        public LogsPageSpecification(int pageNumber, int pageSize, int? serviceId = null)
            : base(l => !serviceId.HasValue || l.ServiceId == serviceId.Value)
        {
            AddInclude(l => l.Service!);
            AddOrderByDescending(l => l.Timestamp);
            ApplyPagination(pageSize, pageNumber);
        }
    }

    public class LogsByTraceSpecification : BaseSpecification<Log, int>
    {
        public LogsByTraceSpecification(Guid traceId) : base(l => l.TraceId == traceId)
        {
            AddInclude(l => l.Service!);
            AddOrderByDescending(l => l.Timestamp);
        }
    }

    public class ErrorLogsInWindowSpecification : BaseSpecification<Log, int>
    {
        public ErrorLogsInWindowSpecification(int serviceId, int timeWindowInMin = 5)
            : base(l => l.ServiceId == serviceId
                     && l.LogLevel >= Domain.Enums.Logs.LogLevel.Error
                     && l.Timestamp >= DateTime.UtcNow.AddMinutes(-timeWindowInMin))
        {
        }
    }
}