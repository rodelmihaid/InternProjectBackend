using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternProjectBackEnd.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            await _emailService.SendEmailAsync(request.Email, request.Password);
            return Ok(new { message = "Email trimis cu succes!" });
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
