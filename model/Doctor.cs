using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorPatient.model
{
    public class Doctor
    {
        public Guid id { get; set; }
        public string doctorName { get; set; }

        public string doctorSpecialist { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string fileName { get; set; }

        public string? FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }

       
    }
}
