using Domain.Contracts;
using Domain.Entities.Incidents;
using Domain.Entities.Logs;
using Domain.Entities.ServiceDeployments;
using Domain.Enums.Incident;
using MediatR;
using ServiceLayer.Services.Logs.Commands;
using ServiceLayer.Services.Specifications;

namespace ServiceLayer.Services.Logs
{
    public class LogThresholdHandler : INotificationHandler<LogIngestedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogThresholdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(LogIngestedEvent notification, CancellationToken cancellationToken)
        {
            const int threshold = 5;
            const int timeWindowMinutes = 10;
            const int deploymentCorrelationWindowMinutes = 30;

            var errorsNumber = await _unitOfWork.GetRepository<Log, int>()
                .CountAsync(new ErrorLogsInWindowSpecification(notification.ServiceId, timeWindowMinutes));

            if (errorsNumber < threshold) return;

            var lastDeploy = await _unitOfWork.GetRepository<ServiceDeployment, int>()
                .GetByIdAsync(new DeploymentsByServiceSpecification(notification.ServiceId));

            var deploymentCorrelationDetails = "\n\n[Correlation Analysis]: No recent deployments detected.";

            if (lastDeploy is not null &&
                (DateTime.UtcNow - lastDeploy.DeployedAt).TotalMinutes <= deploymentCorrelationWindowMinutes)
            {
                deploymentCorrelationDetails =
                    $"\n\n[POSSIBLE ROOT CAUSE - RECENT DEPLOYMENT DETECTED]:" +
                    $"\n- Title: {lastDeploy.Title}" +
                    $"\n- Version: {lastDeploy.Version}" +
                    $"\n- Deployed By: {lastDeploy.DeployedBy}" +
                    $"\n- Deployed At: {lastDeploy.DeployedAt:yyyy-MM-dd HH:mm:ss} UTC";
            }

            var incident = new Incident
            {
                Title = $"Automated Alert: High Error Rate on Service #{notification.ServiceId}",
                Description = $"Threshold breached! Received {errorsNumber} errors within the last {timeWindowMinutes} minutes.\n" +
                              $"Latest Error Message: '{notification.log.Message}'\n" +
                              $"Triggering TraceId: {notification.log.TraceId}" +
                              deploymentCorrelationDetails,
                CreatedAt = DateTime.UtcNow,
                Severity = IncidentSeverity.Critical,
                Status = IncidentStatus.Open,
                ServiceId = notification.ServiceId
            };

            await _unitOfWork.GetRepository<Incident, int>().AddAsync(incident);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}