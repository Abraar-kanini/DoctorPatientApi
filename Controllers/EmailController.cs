using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System;

namespace DoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendMail()
        {
            // Generate a random 6-digit number
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); // Generates a random number between 100000 and 999999

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("jabraar01@gmail.com"));
            email.To.Add(MailboxAddress.Parse("keerthanar310502@gmail.com"));
            email.Subject = "Test EmailSubject";

            // Concatenate the random number with the email body
            string body = $"Your OTP is: {randomNumber}";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jabraar01@gmail.com", "App Password");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok(); 
        }
    }
}
