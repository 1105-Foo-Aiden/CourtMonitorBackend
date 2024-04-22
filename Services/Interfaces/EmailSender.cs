
using System.Net;
using System.Net.Mail;

namespace CourtMonitorBackend.Services.Interfaces
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "courtmonitorservices@gmail.com";
            var pw = "courtmonitorservices123!@#";
            
            var client = new SmtpClient("smtp.gmail.com", 587){
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };
            return client.SendMailAsync(
                new MailMessage(
                    from: mail,
                    to: email,
                    subject,
                    message
                ));
        }
    }
}