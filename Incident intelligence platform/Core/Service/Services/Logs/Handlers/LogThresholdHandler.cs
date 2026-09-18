using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using MediatR;
using ServiceLayer.Services.Logs.Commands;

public class LogThresholdHandler : INotificationHandler<LogIngestedEvent>
{
    private readonly ILogsRepo _logsRepo;
    private readonly IIncidentRepo _incidentRepo;

    public LogThresholdHandler(ILogsRepo logsRepo, IIncidentRepo incidentRepo)
    {
        _logsRepo = logsRepo;
        _incidentRepo = incidentRepo;
    }

    public async Task Handle(LogIngestedEvent notification, CancellationToken cancellationToken)
    {
        int threshold = 5;
        int timeWindowMinutes = 10;

        int errorsNumber = await _logsRepo.GetErrorsNumberByWindowFunction(notification.ServiceId, timeWindowMinutes);

        if (errorsNumber >= threshold)
        {
            var incident = new Incident()
            {
                Title = $"Automated Alert: High Error Rate on Service #{notification.ServiceId}",
                Description = $"Threshold breached! Received {errorsNumber} errors within the last {timeWindowMinutes} minutes.\n" +
                              $"Latest Error Message: '{notification.log.Message}'\n" +
                              $"Triggering TraceId: {notification.log.TraceId}",
                CreatedAt = DateTime.UtcNow,
                Severity = IncidentSeverity.Critical,
                Status = IncidentStatus.Open,
                ServiceId = notification.ServiceId
            };

            await _incidentRepo.AddAsync(incident);
            await _incidentRepo.SaveChangesAsync();
        }
    }
}