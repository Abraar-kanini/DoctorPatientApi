namespace DoctorPatient.DTO
{
    public class PatientsCreateDto
    {
        public string patientName { get; set; }

        public DateTime BookingDate { get; set; }



        //navigational property
        public Guid DoctorId { get; set; }
    }
}
