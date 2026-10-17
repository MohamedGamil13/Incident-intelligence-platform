using Resend;
using ServiceAbstraction.Contracts.Auth;


namespace ServiceLayer.Services.Auth
{
    public class EmailService(IResend resend) : IEmailService
    {
        public async Task SendEmailAsync(EmailMessage emailMassage)
        {
            var resp = await resend.EmailSendAsync(emailMassage);
        }

        public async Task SendResetPasswordEmail(string receiver, string otp)
        {
            EmailMessage emailMessage = new EmailMessage()
            {
                To = receiver,
                From = "onboarding@resend.dev",
                Subject = "Password Reset Request",
                TextBody = $"""
        Hello,

        We received a request to reset the password for your account.

        Your password reset verification code is: {otp}

        For your security, this code will expire in 10 minutes and can only be used once.

        If you did not request a password reset, please ignore this email.
        Your account will remain secure.

        Best regards,
        The Incident Intelligence Platform Security Team
        """
            };
            var resp = await resend.EmailSendAsync(emailMessage);


        }

    }

}