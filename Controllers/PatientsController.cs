using AutoMapper;
using DoctorPatient.Data;
using DoctorPatient.DTO;
using DoctorPatient.model;
using DoctorPatient.Repository;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace DoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DoctorPatientDbContext doctorPatientDbContext;
        private readonly IMapper mapper;
        private readonly IPatients patients;

        public PatientsController(DoctorPatientDbContext doctorPatientDbContext, IMapper mapper,IPatients patients)
        {
            this.doctorPatientDbContext = doctorPatientDbContext;
            this.mapper = mapper;
            this.patients = patients;
        }



        [HttpPost]
        public async Task<IActionResult> PatientsPost([FromBody] PatientsCreateDto patientsCreateDto)
        {
            DateTime bookingDateTime = patientsCreateDto.BookingDate;
            string date = bookingDateTime.ToString("yyyy-MM-dd");
            string time = bookingDateTime.ToString("HH:mm:ss");

            Guid DoctorId = patientsCreateDto.DoctorId;

            if (CheckDateAndTime(date, time, DoctorId))
            {

                ModelState.AddModelError("", "select some other time slot");
                return BadRequest(ModelState);
            }

            else {

                try
                {
                    var domainmodel = await patients.CreateAsync(patientsCreateDto, date, time);

                    if(domainmodel == null) {
                        ModelState.AddModelError("", "couldnt able to create");
                        return BadRequest(ModelState);
                    }
                    SendMail();
                    return Ok(mapper.Map<PatientsCreateDto>(domainmodel));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "Internal server error");
                }
            }
        }

        private bool CheckDateAndTime(string date, string time, Guid DoctorId)
        {
            var existingRecord = doctorPatientDbContext.patients
                .Any(x => x.Date == date && x.Time == time && x.DoctorId == DoctorId);

            return existingRecord; // Returns true if the combination of date and time exists
        }

        [HttpGet]

        public async Task<List<Patients>> GetAll()
        {
            var result = await doctorPatientDbContext.patients.Include(a => a.doctor).ToListAsync();
            return result;
        }


        [HttpGet("PatientsName")]

        public async Task<List<string>> GetPatients()
        {
            var patientsName = await doctorPatientDbContext.patients.Select(a => a.patientName).ToListAsync();
            return patientsName;
        }
        [HttpGet("count")]

        public async Task<int> GetPatientsCount()
        {
            var result = await doctorPatientDbContext.patients.CountAsync(x => x.patientName != null);
            return result;
        }


        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetByiD(Guid id)
        {

            var resultId=doctorPatientDbContext.patients.FirstOrDefault(a=>a.id==id);

            if (resultId == null)
            {
                ModelState.AddModelError("", "The ID You Provide is Missing");
                return BadRequest(ModelState);
            }

            return Ok(resultId);

        }



        private void SendMail()
        {
            // Generate a random 6-digit number
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); // Generates a random number between 100000 and 999999

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("jabraar01@gmail.com"));
            email.To.Add(MailboxAddress.Parse("abraar.kanini@gmail.com"));
            email.Subject = "Registered Successfully";

            // Concatenate the random number with the email body
            string body = $"Your OTP is: {randomNumber} you registered successfully";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jabraar01@gmail.com", "vcfg espi csts buzv");
            smtp.Send(email);
            smtp.Disconnect(true);

            
        }







    }
}
