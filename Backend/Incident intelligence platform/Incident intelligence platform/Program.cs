using Domain.Contracts;
using Incident_intelligence_platform.Config;
using Incident_intelligence_platform.Extensions;
using Presistance.Extensions;
using ServiceLayer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogLogging();

builder.Services
    .AddWebApi()
    .AddSwaggerDocs()
    .AddJwtAuthentication(builder.Configuration)
    .AddRedisCaching(builder.Configuration)
    .AddHangfireServices(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();


app.UseCustomMiddlewares();
using var scope = app.Services.CreateScope();
var objOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeed>();
await objOfDataSeeding.SeedDataAsync();
await objOfDataSeeding.SeedIdentityDataAsync();
app.UseSwaggerDocs();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfire();
app.MapControllers();

app.Run();
