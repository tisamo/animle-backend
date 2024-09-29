using Animle.Classes;
using Animle.Services.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Animle.Controllers
{
    [Route("email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [EnableRateLimiting("fixed")]
        [Route("form")]
        [HttpPost]
        public IActionResult SendEmail(EmailDto email)
        {
      
            var simpleResponse = new SimpleResponse();

            var emailSent = _emailService.SendEmail(email);
            if (emailSent == false)
            {
                simpleResponse.Response = "There was a problem send the email. Try it later please";
                return BadRequest(simpleResponse);
            }

            simpleResponse.Response = "ok";
            return Ok(simpleResponse);
        }

        private class ApiResult
        {
            public bool IsSuccess { get; set; }
            public string? Message { get; set; }
            
            private ApiResult(bool success, string? message = null)
            {
                IsSuccess = success;
                Message = message;
            }
            
            public static ApiResult Ok(string? message = null) => new(true, message);
            public static ApiResult Error(string? message = null) => new(false, message);
        }
    }
}