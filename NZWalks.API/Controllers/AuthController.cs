using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly ITokenRepository tokenRepository;


        public AuthController(UserManager<IdentityUser> userManagerObj, ITokenRepository tokenRepository)
        {
            userManager = userManagerObj;
            this.tokenRepository = tokenRepository;
        }



        //POST : /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {

            var indentity = new IdentityUser()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(indentity, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                // Add Roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(indentity, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registerd ! Please login");
                    }

                }
            }
            return BadRequest("Something went wrong");

        }

        [HttpPost]
        [Route("Login")]
        // POST: /api/Auth/Login
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {

            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (isPasswordValid)
                {
                    /// Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var reponse = new LoginResponseDto()
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(reponse);
                    }
                    // Generate JWT Token and return it

                }
            }
            return BadRequest("Invalid username or password");

        }
    }
}
