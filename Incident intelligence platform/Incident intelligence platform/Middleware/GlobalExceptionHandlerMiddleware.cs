using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Incident_intelligence_platform.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";


            var statusCode = HttpStatusCode.InternalServerError;
            var title = "An unexpected error occurred on the server.";

            switch (exception)
            {
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    title = "The requested resource was not found.";
                    break;

                case InvalidOperationException:
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad request or invalid operation.";
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Unauthorized access.";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;


            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };


            if (_env.IsDevelopment())
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(problemDetails, options);
            return context.Response.WriteAsync(json);
        }
    }
}