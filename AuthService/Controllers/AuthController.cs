using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ECommerce.Shared.Service.AuthService _authService;

        public AuthController(ECommerce.Shared.Service.AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCredentials credentials)
        {
            var existingUser = _authService.GetUserAsync(credentials.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            if (_authService.RegisterUser(credentials.Email, credentials.Password))
            {
                return Ok("User registered successfully");
            }

            return BadRequest("User already exists");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            if (_authService.ValidateUser(credentials.Email, credentials.Password))
            {
                var existingUser = _authService.GetUserAsync(credentials.Email);
                var token = _authService.GenerateJwtToken(existingUser);
                return Ok(new { token });
            }
            return Unauthorized("Invalid username or password");
        }
    }
}
