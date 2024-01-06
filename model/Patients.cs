using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace DoctorPatient.model
{
    public class Patients
    {
        public Guid id { get; set; }

        public string patientName { get; set; }

        [NotMapped]
        public DateTime BookingDate { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }



        //navigational property
        public Guid DoctorId { get; set; }

        public Doctor doctor { get; set; }

        
    }
}
