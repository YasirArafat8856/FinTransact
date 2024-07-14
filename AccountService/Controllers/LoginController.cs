using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public LoginController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            // In a real application, you would validate the user's credentials here.
            if (login.Username == "test" && login.Password == "password")
            {
                var token = _tokenService.GenerateToken(login.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
