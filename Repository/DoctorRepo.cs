using System.Runtime.InteropServices;
using DoctorPatient.Data;
using DoctorPatient.model;
using Microsoft.EntityFrameworkCore;

namespace DoctorPatient.Repository
{
    public class DoctorRepo : IDoctor
    {
        private readonly DoctorPatientDbContext doctorPatientDbContext;

        public DoctorRepo( DoctorPatientDbContext doctorPatientDbContext)
        {
            this.doctorPatientDbContext = doctorPatientDbContext;
        }

        public async Task<Doctor> Delete(Guid id)
        {
            var domainmodel = await doctorPatientDbContext.doctors.FindAsync(id);
            if (domainmodel == null)
            {
                return null;
            }
            doctorPatientDbContext.doctors.Remove(domainmodel);
            await doctorPatientDbContext.SaveChangesAsync();
            return domainmodel;


        }

        public async Task<List<Doctor>> GetAll()
        {
            var domainmodel = await doctorPatientDbContext.doctors.ToListAsync();
            return domainmodel;
        }

        public async Task<Doctor?> GetById(Guid id)
        {
            var domainmodel= await doctorPatientDbContext.doctors.FirstOrDefaultAsync(x=>x.id== id);
            if (domainmodel == null)
            {
                return null;
            }

            return domainmodel;
        }

        public async Task<List<Doctor?>> GetFilter(string? filterOn =null, string? filterQuery = null)
        {
            var domainmodel =  doctorPatientDbContext.doctors.AsQueryable();
            if(string.IsNullOrWhiteSpace(filterOn)==false&& string.IsNullOrWhiteSpace(filterQuery)==false)
            {
                if (filterOn.Equals("doctorName", StringComparison.OrdinalIgnoreCase))
                {
                    domainmodel = domainmodel.Where(x => x.doctorName.Contains(filterQuery));
                }

            }
            else
            {
                return null;
            }
            var result= await domainmodel.ToListAsync();
            return result;
        }
    }
}
