using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace DoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        [HttpPost]
        public IActionResult SendMail(string body) { 

        var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("jabraar01@gmail.com"));
            email.To.Add(MailboxAddress.Parse("abraar.kanini@gmail.com"));
            email.Subject = "Test EmailSubject";
            email.Body=new TextPart(TextFormat.Html) { Text=body };


            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jabraar01@gmail.com", "passkey");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok(); // this is email

        }
    }
}
