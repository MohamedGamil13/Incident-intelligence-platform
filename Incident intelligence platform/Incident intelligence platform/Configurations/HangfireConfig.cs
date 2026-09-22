using Hangfire;
using ServiceAbstraction.Contracts.ServiceMangment;
using Shared.Dtos.ServiceDTOs;

namespace Incident_intelligence_platform.Extensions
{
    public static class HangfireJobExtensions
    {
        public static IApplicationBuilder RegisterHangfireJobs(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();


                var analysisRequest = new AnalyzeServiceRequest
                {
                    ServiceId = 2,
                    MaxLatancy = 500,
                    MaxIncidentsPerService = 3,
                    ServiceErrorTimeWindow = 5,
                    IncidentTimeWindow = 15
                };


                recurringJobManager.AddOrUpdate<IAnalyzeServiceJob>(
                    "analyze-service-health-job",
                    job => job.AnalyzeServices(analysisRequest),
                    Cron.MinuteInterval(5)
                );
            }

            return app;
        }
    }
}