using AutoMapper;
using DoctorPatient.Data;
using DoctorPatient.DTO;
using DoctorPatient.model;
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

        public DoctorController(DoctorPatientDbContext doctorPatientDb,IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            this.doctorPatientDb = doctorPatientDb;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
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
       

        
        public async Task<List<Doctor>> Get()
        {
            var domainmodel = await doctorPatientDb.doctors.ToListAsync();
            return domainmodel;
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
            var doctorId= await doctorPatientDb.doctors.FirstOrDefaultAsync(x=>x.id==id);

            if (doctorId == null)
            {
                return BadRequest();
            }

            return Ok(doctorId);
        }

        [HttpGet("filter")]
        public async Task<List<Doctor>> GetByFilter([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var domainmodel = doctorPatientDb.doctors.AsQueryable();

            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("doctorName", StringComparison.OrdinalIgnoreCase))
                {
                    domainmodel = domainmodel.Where(a => a.doctorName.Contains(filterQuery));
                }
            }

            var result = await domainmodel.ToListAsync();
            return result;

            
        }


    }
}
