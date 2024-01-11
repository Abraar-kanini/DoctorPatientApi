using Microsoft.AspNetCore.Identity;

namespace DoctorPatient.Repository
{
    public interface IAuthentication
    {
        string CreateJwtToken(IdentityUser identityUser, List<string> roles);
    }
}
