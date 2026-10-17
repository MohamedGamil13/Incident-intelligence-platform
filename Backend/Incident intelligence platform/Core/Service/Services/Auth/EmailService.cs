using ServiceAbstraction.Contracts.Auth;
using Resend;

namespace ServiceLayer.Services.Auth
{
    public class EmailService(IResend resend) : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage emailMassage)
        { 
            var resp = await resend.EmailSendAsync(emailMassage);
        }
    }
}
