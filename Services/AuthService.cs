using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskMasterAPI.Data;
using TaskMasterAPI.Models.DTOs;
using TaskMasterAPI.Models.Responses;
using TaskMasterAPI.Models.Entities;
using Org.BouncyCastle.Crypto.Generators;
using System.Data.Entity;

namespace TaskMasterAPI.Services
{
    public class AuthService
    {
        private readonly ApDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<AuthResponse> Login(LoginDTO loginDto)
        {
            // Validación básica del DTO
            if (string.IsNullOrWhiteSpace(loginDto.Email))
            {
                return new AuthResponse(false, "Email is required");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return new AuthResponse(false, "Password is required");
            }

            // Buscar usuario por email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            // Verificar si el usuario existe y la contraseña coincide
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponse(false, "Invalid email or password");
            }

            // Generar token JWT
            var token = GenerateJwtToken(user);

            // Registrar último login (opcional)
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new AuthResponse(true, "Login successful", token, user);
        }

        public async Task<AuthResponse> Register(RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return new AuthResponse(false, "Email already in use");
            }

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResponse(true, "User registered successfully", token);
        }

       

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JsonWebTokenKeys:IssuerSigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("fullName", $"{user.FirstName} {user.LastName}")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JsonWebTokenKeys:ValidIssuer"],
                audience: _configuration["JsonWebTokenKeys:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
