using AutoMapper;
using DoctorPatient.Data;
using DoctorPatient.DTO;
using DoctorPatient.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace DoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DoctorPatientDbContext doctorPatientDbContext;
        private readonly IMapper mapper;

        public PatientsController(DoctorPatientDbContext doctorPatientDbContext, IMapper mapper)
        {
            this.doctorPatientDbContext = doctorPatientDbContext;
            this.mapper = mapper;
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
                    var domainmodel = new Patients()
                    {
                        patientName = patientsCreateDto.patientName,
                        Date = date,
                        Time = time,
                        DoctorId = patientsCreateDto.DoctorId
                    };

                    await doctorPatientDbContext.AddAsync(domainmodel);
                    await doctorPatientDbContext.SaveChangesAsync();

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



 





    }
}
