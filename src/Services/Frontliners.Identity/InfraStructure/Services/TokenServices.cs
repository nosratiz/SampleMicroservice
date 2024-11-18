using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Frontliners.Common.Options;
using Frontliners.Identity.Domain.Entities;
using Frontliners.Identity.InfraStructure.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Frontliners.Identity.InfraStructure.Services;

public sealed class TokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    public  string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new("userId", user.Id.ToString())
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Value.SecretKey);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(jwtSettings.Value.ExpireDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            )
        };
        
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }
}