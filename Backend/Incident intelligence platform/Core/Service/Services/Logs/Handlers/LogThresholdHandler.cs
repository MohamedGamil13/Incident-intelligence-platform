using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Contracts.ServiceDeployments;
using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using MediatR;
using ServiceLayer.Services.Logs.Commands;

public class LogThresholdHandler : INotificationHandler<LogIngestedEvent>
{
    private readonly ILogsRepo _logsRepo;
    private readonly IIncidentRepo _incidentRepo;
    private readonly IServiceDeploymentsRepo _deploymentsRepo;

    public LogThresholdHandler(ILogsRepo logsRepo, IIncidentRepo incidentRepo, IServiceDeploymentsRepo deploymentsRepo)
    {
        _logsRepo = logsRepo;
        _incidentRepo = incidentRepo;
        _deploymentsRepo = deploymentsRepo;
    }

    public async Task Handle(LogIngestedEvent notification, CancellationToken cancellationToken)
    {
        int threshold = 5;
        int timeWindowMinutes = 10;
        int deploymentCorrelationWindowMinutes = 30;

        int errorsNumber = await _logsRepo.GetErrorsNumberByWindowFunction(notification.ServiceId, timeWindowMinutes);

        if (errorsNumber >= threshold)
        {

            var lastDeploy = await _deploymentsRepo.GetLatestDeploymentByServiceIdAsync(notification.ServiceId);

            string deploymentCorrelationDetails = "\n\n[Correlation Analysis]: No recent deployments detected.";

            if (lastDeploy != null && (DateTime.UtcNow - lastDeploy.DeployedAt).TotalMinutes <= deploymentCorrelationWindowMinutes)
            {
                deploymentCorrelationDetails = $"\n\n[POSSIBLE ROOT CAUSE - RECENT DEPLOYMENT DETECTED]:" +
                                                $"\n- Title: {lastDeploy.Title}" +
                                                $"\n- Version: {lastDeploy.Version}" +
                                                $"\n- Deployed By: {lastDeploy.DeployedBy}" +
                                                $"\n- Deployed At: {lastDeploy.DeployedAt:yyyy-MM-dd HH:mm:ss} UTC";
            }

            var incident = new Incident()
            {
                Title = $"Automated Alert: High Error Rate on Service #{notification.ServiceId}",
                Description = $"Threshold breached! Received {errorsNumber} errors within the last {timeWindowMinutes} minutes.\n" +
                              $"Latest Error Message: '{notification.log.Message}'\n" +
                              $"Triggering TraceId: {notification.log.TraceId}" +
                              $"{deploymentCorrelationDetails}",
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