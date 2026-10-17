using Resend;

namespace ServiceAbstraction.Contracts.Auth
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailMessage email);
    }
}