using DoctorPatient.Data;
using DoctorPatient.DTO;
using DoctorPatient.model;

namespace DoctorPatient.Repository
{
    public class PateintsRepo : IPatients
    {
        private readonly DoctorPatientDbContext doctorPatientDbContext;

        public PateintsRepo(DoctorPatientDbContext doctorPatientDbContext)
        {
            this.doctorPatientDbContext = doctorPatientDbContext;
        }
        public async Task<Patients> CreateAsync(PatientsCreateDto patients, string date, string time)
        {
            var domainmodel = new Patients()
            {
                patientName = patients.patientName,
                Date = date,
                Time = time,
                DoctorId = patients.DoctorId
            };

            await doctorPatientDbContext.AddAsync(domainmodel);
            await doctorPatientDbContext.SaveChangesAsync();
            return domainmodel;

        }
    }
}
