using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InternProjectBackEnd
{
    public class ReminderEmailService
    {
        private readonly ILogger<ReminderEmailService> _logger;

        public ReminderEmailService(ILogger<ReminderEmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendReminderEmailAsync(List<string> emails, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Schedule", "rodelutz@gmail.com"));

            foreach (var email in emails)
            {
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };
            }

            using (var client = new SmtpClient())
            {
                try
                {
                    _logger.LogInformation("Connecting to SMTP server...");
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("rodelutz@gmail.com", "hikw rzqc yebj miiu");
                    await client.SendAsync(message);
                    _logger.LogInformation("Email sent successfully.");
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error sending email: {ex.Message}");
                }
            }
        }
    }
}
