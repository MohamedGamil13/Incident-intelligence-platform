using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Incident_intelligence_platform.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["JWT:Key"]
                ?? throw new InvalidOperationException("JWT:Key is missing.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        return WriteJsonAsync(context.Response, StatusCodes.Status401Unauthorized, "Unauthorized",
                            "You are not authorized to access this resource. Please provide a valid token.");
                    },
                    OnForbidden = context => WriteJsonAsync(context.Response, StatusCodes.Status403Forbidden, "Forbidden",
                        "You do not have permission to access this resource.")
                };
            });

            return services;
        }

        private static Task WriteJsonAsync(HttpResponse response, int statusCode, string error, string message)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            return response.WriteAsync(JsonSerializer.Serialize(new { status = statusCode, error, message }));
        }
    }
}