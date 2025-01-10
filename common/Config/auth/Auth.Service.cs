using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Entity;

namespace TaskManagement.Services
{
    public class AuthService
    {
        private readonly IConfiguration configuration;

        public AuthService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

       public string GenerateToken(User user, string role, List<string> permissions){
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, role)
    };

    claims.AddRange(permissions.Select(p => new Claim("Permission", p)));

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: configuration["JwtSettings:Issuer"],
        audience: configuration["JwtSettings:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
    }
}