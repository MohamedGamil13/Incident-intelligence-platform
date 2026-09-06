using Incident_intelligence_platform;
using Incident_intelligence_platform.Config;
using Incident_intelligence_platform.Configurations;
using Incident_intelligence_platform.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var JWToptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.RegisterMapsterConfiguration();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = JWToptions?.Audience,
            ValidIssuer = JWToptions?.Issuer,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JWToptions!.SigningKey)
            )
        };
    });



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddDbContext<AppDbcontext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//Hosts
builder.Host.AddSerilogLogging();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Custom Middlewares
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
