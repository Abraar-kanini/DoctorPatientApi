using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorPatient.DTO
{
    public class DoctorPostDto
    {
        public string doctorName { get; set; }

        public string doctorSpecialist { get; set; }

       
        public IFormFile File { get; set; }

        public string fileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
