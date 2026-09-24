using Domain.Entities.Logs;
using MediatR;

namespace ServiceLayer.Services.Logs.Commands
{
    public record LogIngestedEvent(Log log, int ServiceId) : INotification;
}
