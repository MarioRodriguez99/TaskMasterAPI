using Microsoft.AspNetCore.Mvc;
using TaskMasterAPI.Models.DTOs;
using TaskMasterAPI.Services;
using TaskMasterAPI.Models.Responses;
using System.Security.Claims;

namespace TaskMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(false, "Invalid data", ModelState));
            }

            var result = await _authService.Register(registerDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(false, "Invalid data", ModelState));
            }

            var result = await _authService.Login(loginDto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpGet("me")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult GetCurrentUser()
        {
            // Obtener información del usuario actual desde el token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var fullName = User.FindFirst("fullName")?.Value;

            return Ok(new ApiResponse(true, "Current user info", new
            {
                UserId = userId,
                Email = userEmail,
                FullName = fullName
            }));
        }
    }
}