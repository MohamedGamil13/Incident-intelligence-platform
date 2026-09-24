using Microsoft.Extensions.Primitives;

namespace Incident_intelligence_platform.Middleware
{
    public class HandleTraceIdMiddleware
    {
        private readonly RequestDelegate next;
        private const string TraceIdHeader = "X-Trace-ID";

        public HandleTraceIdMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (!ContainHeader(context, out var traceId) || string.IsNullOrWhiteSpace(traceId))
            {
                traceId = Guid.NewGuid().ToString();
            }
            context.TraceIdentifier = traceId!;
            context.Response.Headers[TraceIdHeader] = traceId!;
            await next(context);

        }
        private bool ContainHeader(HttpContext context, out StringValues traceId)
        {
            return context.Request.Headers.TryGetValue(TraceIdHeader, out traceId);
        }

    }
}
