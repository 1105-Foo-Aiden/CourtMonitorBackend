
using System.Net;
using System.Net.Mail;

namespace CourtMonitorBackend.Services.Interfaces
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "afoo154@gmail.com";
            var pw = "Hellothere15";
            
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