using Resend;

namespace ServiceAbstraction.Contracts.Auth
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailMessage email);
        public Task SendResetPasswordEmail(string Receiver, string otp);
    }
}