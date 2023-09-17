using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthContorller : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthContorller(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }
        //api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequiredDTO registerRequiredDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequiredDTO.UserName,
                Email = registerRequiredDTO.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequiredDTO.Password);

            if (identityResult.Succeeded)
            {
                //Add roles to user
                if (registerRequiredDTO.Roles != null && registerRequiredDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequiredDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("Register request success ! plz login ");
                    }
                }

            }
            return BadRequest();
        }

        //api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JWTToken = jwtToken,
                        };
                        return Ok(response);
                    }


                }
            }

            return BadRequest("");
        }
    }
}
