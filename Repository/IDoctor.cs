using DoctorPatient.model;

namespace DoctorPatient.Repository
{
    public interface IDoctor
    {
        Task<List<Doctor>> GetAll();

        Task<Doctor?> GetById(Guid id);

        Task<List<Doctor?>> GetFilter(string? filterOn=null, string? filterQuery = null);

        Task<Doctor> Delete(Guid id);
    }
}
