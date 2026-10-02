using Incident_intelligence_platform.Middleware;
using Incident_intelligence_platform.Middlewares;

namespace Incident_intelligence_platform.Extensions
{
    public static class MiddlewareExtensions
    {

        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseMiddleware<RequestTimingMiddleware>();
            app.UseMiddleware<HandleTraceIdMiddleware>();
            return app;
        }
    }
}