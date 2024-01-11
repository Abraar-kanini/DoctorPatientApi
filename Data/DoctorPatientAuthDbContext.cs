using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoctorPatient.Data
{
    public class DoctorPatientAuthDbContext : IdentityDbContext
    {
        public DoctorPatientAuthDbContext(DbContextOptions<DoctorPatientAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "76313199 - 3b93 - 4b3a - 9e63 - ab0eea4f74f7";
            var writerRoleId = "981a0ef1-b3b2-439d-a222-4048c13f63b6";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=readerRoleId,
                    ConcurrencyStamp=readerRoleId,
                    Name="Reader",
                    NormalizedName="Reader".ToUpper()

                },
                new IdentityRole
                {
                    Id=writerRoleId,
                    ConcurrencyStamp=writerRoleId,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()

                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
