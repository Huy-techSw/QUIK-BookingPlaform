using Quik_BookingApp.BOs.Request;

namespace Quik_BookingApp.Repos.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest email);
    }

    //public interface IEmailService
    //{
    //    Task SendVerificationEmailAsync(string email, string verificationLink);
    //}
}
