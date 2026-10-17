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
app.UseSwaggerDocs();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfire();
app.MapControllers();

app.Run();


//Resend API Key : re_XHLioiwe_JqKq99PM25nzGUkRXqVH4oSV
// Code 
/*
 using Resend;

IResend resend = ResendClient.Create( "re_XHLioiwe_JqKq99PM25nzGUkRXqVH4oSV" );

var resp = await resend.EmailSendAsync( new EmailMessage()
{
    From = "onboarding@resend.dev",
    To = "mohamed.gamil.khalaf1@gmail.com",
    Subject = "Hello World",
    HtmlBody = "<p>Congrats on sending your <strong>first email</strong>!</p>",
} );

 
 
 
 
 
 
 
 
 
 
 */