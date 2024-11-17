using IdentityDataProtection.Dto;
using IdentityDataProtection.Jwt;
using IdentityDataProtection.Models;
using IdentityDataProtection.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDataProtection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(IdentityDataProtection.Models.RegisterRequest request)
        {
            var addUserDto = new AddUserDto { Email = request.Email, Password = request.Password };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(IdentityDataProtection.Models.LoginRequest request)
        {

            var loginUserDto = new LoginUserDto { Email = request.Email, Password = request.Password };

            var result = await _userService.LoginUser(loginUserDto);

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            var user = result.Data;

            var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwt(new JwtDto { Id = user.Id, Email = user.Email, UserRole = user.UserRole, SecretKey = config["Jwt:SecretKey"]!, Issuer = config["Jwt:Issuer"]!, Audience = config["Jwt:Audience"]!, ExpireMinutes = int.Parse(config["Jwt:ExpireMinutes"]!) });

            return Ok(new LoginResponse { Message = "Login succesfull", Token = token });
        }
    }
}
