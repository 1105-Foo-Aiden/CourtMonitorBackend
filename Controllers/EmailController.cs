using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CourtMonitorBackend.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            var reciever = "aidenfoo154@gmail.com";
            var subject = "Test";
            var message = "Hello World";
            await _emailSender.SendEmailAsync(reciever, subject, message);
            return View();
        }

        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel emailModel ){
            await _emailSender.SendEmailAsync(emailModel.Sender, emailModel.Subject, emailModel.Message);
            return Ok();
        }
    }
}