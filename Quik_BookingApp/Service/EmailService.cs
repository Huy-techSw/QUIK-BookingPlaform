using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Quik_BookingApp.BOs.Request;
using Quik_BookingApp.Helper;
using Quik_BookingApp.Repos.Interface;

namespace Quik_BookingApp.Service
{
    public class EmailService : IEmailService
    {

        //private readonly SmtpClient _smtpClient;

        //public EmailService(SmtpClient smtpClient)
        //{
        //    this._smtpClient = smtpClient;
        //}

        //public async Task SendVerificationEmailAsync(string email, string verificationLink)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Quik", "huylqse173543@fpt.edu.vn"));
        //    message.To.Add(new MailboxAddress("", email));
        //    message.Subject = "Verify your email";

        //    // Email body (plain text)
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = $"Please verify your email by clicking on this link: {verificationLink}"
        //    };
        //    await _smtpClient.SendAsync(message);
        //}
        private readonly IConfiguration _config;
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> options, IConfiguration config)
        {
            emailSettings = options.Value;
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config["SmtpSettings:SenderName"], _config["SmtpSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            if (string.IsNullOrEmpty(mailrequest.ToEmail))
            {
                throw new ArgumentNullException(nameof(mailrequest.ToEmail), "Recipient email cannot be null or empty.");
            }

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject ?? "No Subject";
            var builder = new BodyBuilder();


            if (File.Exists("Attachment/dummy.pdf"))
            {
                try
                {
                    using (FileStream file = new FileStream("Attachment/dummy.pdf", FileMode.Open, FileAccess.Read))
                    {
                        byte[] fileBytes;
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add("attachment.pdf", fileBytes, ContentType.Parse("application/pdf"));
                        builder.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/pdf"));
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException("Error attaching file", ex);
                }
            }

            builder.HtmlBody = mailrequest.Body ?? "No content";
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error sending email", ex);
            }
        }
    }
}

