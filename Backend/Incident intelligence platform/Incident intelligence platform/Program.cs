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
    .AddApplicationServices();

var app = builder.Build();


app.UseCustomMiddlewares();
app.UseSwaggerDocs();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfire();
app.MapControllers();

app.Run();