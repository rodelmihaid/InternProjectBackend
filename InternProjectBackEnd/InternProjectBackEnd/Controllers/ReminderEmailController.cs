using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InternProjectBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReminderEmailController : ControllerBase
    {
        private readonly EmailScheduler _emailScheduler;

        public ReminderEmailController(EmailScheduler emailScheduler)
        {
            _emailScheduler = emailScheduler;
        }

        [HttpPost]
        public IActionResult ScheduleReminderEmail([FromBody] ScheduledEmailRequest request)
        {
            var currentDate = DateTime.Now.Day;
            var scheduledDate = request.ScheduledTime.Day;
            var dayDifference = (scheduledDate - currentDate);
            Console.WriteLine($"Day difference: {dayDifference}");

            if (dayDifference != request.MaxDayDifference)
            {
                return BadRequest(new { message = "Diferența dintre data curentă și data programată depășește limita permisă." });
            }

            var scheduledEmail = new ScheduledEmail
            {
                Emails = request.Emails,
                Subject = request.Subject,
                Body = request.Body,
                ScheduledTime = request.ScheduledTime
            };

            _emailScheduler.ScheduleEmail(scheduledEmail);
            Console.WriteLine("Email scheduled successfully!");
            return Ok(new { message = "Email scheduled successfully!" });
        }
    }

    public class ScheduledEmailRequest
    {
        public List<string> Emails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime ScheduledTime { get; set; }
        public int MaxDayDifference { get; set; } = 1; // Pragul de diferență în zile
    }
}
