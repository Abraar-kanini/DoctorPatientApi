using DoctorPatient.DTO;
using DoctorPatient.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoctorPatient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthentication authentication;

        public AuthController(UserManager<IdentityUser> userManager,IAuthentication authentication)
        {
            this.userManager = userManager;
            this.authentication = authentication;
        }

        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {

            var identityuser = new IdentityUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityuser, registerDto.Password);

            if (identityResult.Succeeded)
            {
                //addd the roles

                if (registerDto.Roles != null && registerDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityuser, registerDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("Register Successfully Now You Can Login ");
                    }
                }
            }
            return BadRequest("Something Went Wrong");
        }



        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.UserName);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if (checkPasswordResult)

                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = authentication.CreateJwtToken(user, roles.ToList());
                        return Ok(jwtToken);
                    }

                }

            }
            return BadRequest("TRy Again");
        }
    }
}
