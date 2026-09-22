using Hangfire;
using ServiceAbstraction.Contracts.ServiceMangment;
using Shared.Dtos.ServiceDTOs;

namespace Incident_intelligence_platform.Config
{
    public static class HangfireJobExtensions
    {
        public static IApplicationBuilder RegisterHangfireJobs(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();


                var globalThresholds = new AnalyzeAllServicesRequest
                {
                    MaxLatancy = 500,
                    MaxIncidentsPerService = 3,
                    ServiceErrorTimeWindow = 5,
                    IncidentTimeWindow = 15,
                    MaxErrorsPerService = 3,
                };


                recurringJobManager.AddOrUpdate<IAnalyzeServiceJob>(
                    recurringJobId: "analyze-all-services-health-job",
                    methodCall: job => job.AnalyzeAllServices(globalThresholds),
                    cronExpression: Cron.MinuteInterval(5));
            }

            return app;
        }
    }
}