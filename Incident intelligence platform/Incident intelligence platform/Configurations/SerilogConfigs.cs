using Serilog;

namespace Incident_intelligence_platform.Config
{
    public static class SerilogExtensions
    {
        public static void AddSerilogLogging(this ConfigureHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/myapp-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            host.UseSerilog();
        }
    }
}