using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechMove.Shared.Auth;

namespace TechMove.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto req)
        {
            if (req.Username != "admin" || req.Password != "password")
                return Unauthorized("Invalid credentials");

            // ✅ Same key as Program.cs
            var jwtKey = _config["JwtKey"] ?? "THIS_IS_A_SUPER_SECRET_KEY_12345";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, req.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TechMoveApi",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResponseDto
            {
                Token = tokenString,
                ExpiresAt = token.ValidTo
            });
        }
    }
}