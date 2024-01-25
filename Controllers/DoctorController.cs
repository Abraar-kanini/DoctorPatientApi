using AutoMapper;
using DoctorPatient.Data;
using DoctorPatient.DTO;
using DoctorPatient.model;
using DoctorPatient.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace DoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DoctorController : ControllerBase
    {
        private readonly DoctorPatientDbContext doctorPatientDb;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IDoctor doctor;

        public DoctorController(DoctorPatientDbContext doctorPatientDb,IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,IMapper mapper,IDoctor doctor)
        {
            this.doctorPatientDb = doctorPatientDb;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.doctor = doctor;
        }

        [HttpPost]

        public async Task<IActionResult> Upload([FromForm] DoctorPostDto doctor)
        {
            validateFileUpload(doctor);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var doctordomainmodel = new Doctor
                {
                    doctorName=doctor.doctorName,
                    doctorSpecialist=doctor.doctorSpecialist,
                    File=doctor.File,
                    FileExtension=Path.GetExtension(doctor.File.FileName),
                    FileSizeInBytes=doctor.File.Length,
                    fileName=doctor.fileName,
                    FileDescription=doctor.FileDescription
                };
                var localimagepath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{doctordomainmodel.fileName}{doctordomainmodel.FileExtension}");
                using var stream = new FileStream(localimagepath, FileMode.Create);
                await doctordomainmodel.File.CopyToAsync(stream);
                var urlfilepath= $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{doctordomainmodel.fileName}{doctordomainmodel.FileExtension}";
                doctordomainmodel.FilePath = urlfilepath;
                await doctorPatientDb.AddAsync(doctordomainmodel);
                await doctorPatientDb.SaveChangesAsync();
                return Ok(doctordomainmodel);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error"); // Return a generic error message
            }
        }

        private void validateFileUpload(DoctorPostDto doctor)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtension.Contains(Path.GetExtension(doctor.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file extension");
            }
            if(doctor.File.Length> 10485760)
            {
                ModelState.AddModelError("file", "the file size is more than 10 mb");
            }
        }

        [HttpGet]
       // [Authorize(Roles ="Reader")]           
       

        
        public async Task<IActionResult> Get()
        {
            var domainmodel = await doctor.GetAll();
            if (domainmodel==null)
            {
                ModelState.AddModelError("", "There is no data");
                return BadRequest(ModelState);
            }
            return Ok(domainmodel);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] DoctorUpdateDto doctor)
        {
            var doctorId = await doctorPatientDb.doctors.FindAsync(id);
            if (doctorId == null)
            {
                return BadRequest();
            }
         
            doctorId.doctorName = doctor.doctorName;
            doctorId.doctorSpecialist = doctor.doctorSpecialist;

            await doctorPatientDb.SaveChangesAsync();
            return Ok(mapper.Map<DoctorUpdateDto>(doctorId));
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById(Guid id)
        {
            var domainmodel= await doctor.GetById(id);
            if (domainmodel == null)
            {
                ModelState.AddModelError("", "There is no such id found");
                return BadRequest(ModelState);
            }
            return Ok(domainmodel);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
           var domainmodel= await doctor.GetFilter(filterOn,filterQuery);
            if (domainmodel == null)
            {
                ModelState.AddModelError("", "there is no data found");
                return BadRequest(ModelState);
            }

            return Ok(domainmodel);

            
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var domainmodel=await doctor.Delete(id);
            if (domainmodel == null)
            {
                ModelState.AddModelError("", "there is no data found");
                return BadRequest(ModelState);

            }
            return Ok(domainmodel);
        }


    }
}
