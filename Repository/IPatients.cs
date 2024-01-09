using DoctorPatient.DTO;
using DoctorPatient.model;

namespace DoctorPatient.Repository
{
    public interface IPatients
    {
        Task<Patients> CreateAsync(PatientsCreateDto patients ,string date,string time);
    }
}
