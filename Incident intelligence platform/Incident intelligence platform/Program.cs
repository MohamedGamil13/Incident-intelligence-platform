using Incident_intelligence_platform;
using Incident_intelligence_platform.Config;
using Incident_intelligence_platform.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.RegisterMapsterConfiguration();
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
