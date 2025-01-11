using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Entity;

namespace TaskManagement.Services
{
    public class AuthServiceJwt
    {
        private readonly IConfiguration _configuration;

        private const string JwtKey = "JwtSettings:Key";
        private const string JwtIssuer = "JwtSettings:Issuer";
        private const string JwtAudience = "JwtSettings:Audience";

        public AuthServiceJwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user, string role, List<string> permissions)
        {
            var key = GetConfigurationValue(JwtKey);
            var issuer = GetConfigurationValue(JwtIssuer);
            var audience = GetConfigurationValue(JwtAudience);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetConfigurationValue(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Configuration key '{key}' is not configured properly.");
            }
            return value;
        }
    }
}