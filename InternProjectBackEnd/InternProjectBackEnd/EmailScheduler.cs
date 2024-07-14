using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InternProjectBackEnd
{
    public class EmailScheduler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly List<ScheduledEmail> _scheduledEmails;
        private readonly Timer _timer;

        public EmailScheduler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _scheduledEmails = new List<ScheduledEmail>();
            _timer = new Timer(CheckEmailsToSend, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            Console.WriteLine("EmailScheduler initialized and timer started.");
        }

        public void ScheduleEmail(ScheduledEmail email)
        {
            _scheduledEmails.Add(email);
            Console.WriteLine($"Email scheduled for {email.ScheduledTime}: {email.Subject}");
            Console.WriteLine($"Current scheduled emails count: {_scheduledEmails.Count}");
        }

        private async void CheckEmailsToSend(object state)
        {
            Console.WriteLine("Checking emails to send...");
            using (var scope = _scopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<ReminderEmailService>();
                Console.WriteLine($"Total emails scheduled: {_scheduledEmails.Count}");

                var now = DateTime.UtcNow; // Folosește UTC pentru comparație
                Console.WriteLine($"Current time (UTC): {now}");

                var emailsToSend = _scheduledEmails
                    .Where(e => e.ScheduledTime >= now)
                    .ToList();

                Console.WriteLine($"Emails to send count: {emailsToSend.Count}");

                foreach (var email in emailsToSend)
                {
                    Console.WriteLine($"Sending email: {email.Subject} scheduled for {email.ScheduledTime}");
                    await emailService.SendReminderEmailAsync(email.Emails, email.Subject, email.Body);
                    _scheduledEmails.Remove(email);
                    Console.WriteLine($"Email sent and removed from schedule: {email.Subject}");
                }
            }
        }

    }

    public class ScheduledEmail
    {
        public List<string> Emails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime ScheduledTime { get; set; }
    }
}
