using Incident_intelligence_platform.DTOs;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Incidents;

namespace Incident_intelligence_platform.Extensions
{
    public static class WebApiExtensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(typeof(IncidentsController).Assembly).AddControllersAsServices(); ;

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(e => e.Value!.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = ApiResponse<object>.FailureResponse(
                        message: "Validation failed",
                        errors: errors,
                        statusCode: 400);

                    return new BadRequestObjectResult(response);
                };
            });

            services.RegisterMapsterConfiguration();

            return services;
        }
    }
}