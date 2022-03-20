using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public async Task<bool> SendMail(Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            string subect = email.Subject;
            var to = new EmailAddress(email.To);
            string emailBody = email.Body;

            var from = new EmailAddress
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(from, to, subect, emailBody, emailBody);
            Response response = await client.SendEmailAsync(sendGridMessage);

            _logger.LogInformation("Email sent.");
            if (response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK)
            {
                return true;
            }

            _logger.LogError("Email sending failed.");
            return false;
        }
    }
}