using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace PDM.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private AuthMessageSenderOptions _options;

        public EmailSender()
        {

        }

        public EmailSender(IOptions<AuthMessageSenderOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Sending emails from application
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("PDM Fiorano", "no-replay@fiorano.rs"));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };
            var client = new MailKit.Net.Smtp.SmtpClient();
            //client.LocalDomain = "fiorano.rs";
            await client.ConnectAsync("irelay.calzedonia.com", 25, SecureSocketOptions.None).ConfigureAwait(false);
            await client.SendAsync(emailMessage).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
            //return Task.CompletedTask;
        }

        /// <summary>
        /// Sneding confirmaion link for new users
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendConfirmationEmailAsync(string email, string subject, string message)
        {
            return Execute(_options.SendGridKey, subject, message, email);
        }



        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-replay@fiorano.rs", "No Replay"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
