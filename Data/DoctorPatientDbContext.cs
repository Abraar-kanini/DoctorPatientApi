using DoctorPatient.model;
using Microsoft.EntityFrameworkCore;

namespace DoctorPatient.Data
{
    public class DoctorPatientDbContext:DbContext
    {
        public DoctorPatientDbContext( DbContextOptions<DoctorPatientDbContext> dbContextOptions):base(dbContextOptions)
        {


        }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Patients> patients { get; set; }
    }
}
