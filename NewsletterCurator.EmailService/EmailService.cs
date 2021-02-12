using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NewsletterCurator.EmailService
{
    public class EmailService
    {
        private readonly SmtpClient smtpClient;
        private readonly string unsubscribeEmail;
        private readonly string fromEmail;
        private readonly string fromName;

        public EmailService(SmtpClient smtpClient, string fromEmail, string fromName, string unsubscribeEmail)
        {
            this.smtpClient = smtpClient;
            this.unsubscribeEmail = unsubscribeEmail;
            this.fromEmail = fromEmail;
            this.fromName = fromName;
        }

        public async Task SendAsync(string html, string subjectLine, List<string> bcc)
        {
            foreach (var address in bcc)
            {
                var mail = new MailMessage()
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subjectLine,
                    Body = html,
                    IsBodyHtml = true
                };
                mail.To.Add(address);

                mail.Headers.Add("List-Unsubscribe", $"<mailto:{unsubscribeEmail}?subject=unsubscribe>");
                await smtpClient.SendMailAsync(mail);
            }
        }

        public async Task SendValidationEmailAsync(string email, string validationURL)
        {
            var mail = new MailMessage(new MailAddress("curated@newsletters.cdemi.io", "cdemi's Curated Newsletter"), new MailAddress(email))
            {
                Subject = $"Validate your Email",
                Body = $"<h1>Welcome to cdemi's Curated Newsletter!</h1><br/><br/>Please validate your email by clicking this link: <a href='{validationURL}'>{validationURL}</a>",
                IsBodyHtml = true
            };
            mail.Headers.Add("List-Unsubscribe", $"<mailto:{unsubscribeEmail}?subject=unsubscribe>");
            await smtpClient.SendMailAsync(mail);
        }
    }
}
