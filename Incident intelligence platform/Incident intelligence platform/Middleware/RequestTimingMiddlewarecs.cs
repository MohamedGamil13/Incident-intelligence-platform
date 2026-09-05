using System.Diagnostics;

namespace Incident_intelligence_platform.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var stopwatch = Stopwatch.StartNew();

            try
            {

                await _next(context);
            }
            finally
            {

                stopwatch.Stop();

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                var requestPath = context.Request.Path;
                var requestMethod = context.Request.Method;
                var statusCode = context.Response.StatusCode;


                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms",
                    requestMethod, requestPath, statusCode, elapsedMilliseconds);


                if (elapsedMilliseconds > 500)
                {
                    _logger.LogWarning(
                        "SLOW REQUEST WARNING: HTTP {Method} {Path} took {ElapsedMs} ms!",
                        requestMethod, requestPath, elapsedMilliseconds);
                }
            }
        }
    }
}