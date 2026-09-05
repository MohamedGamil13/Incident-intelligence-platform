using Serilog;

namespace Incident_intelligence_platform.Config
{
    public static class SerilogExtensions
    {
        public static void AddSerilogLogging(this IHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/myapp-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            host.UseSerilog();
        }
    }
}