using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PGManagementSystem.Domain.Entities;

public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(UserMaster user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User"),
            new Claim("phone", user.Phone ?? string.Empty)
        };

        return CreateJwtToken(claims);
    }

    public string GenerateTokenForTenant(TenantMaster tenant)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, tenant.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, tenant.Id.ToString()),
            new Claim(ClaimTypes.Email, tenant.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, tenant.Name ?? string.Empty),
            new Claim(ClaimTypes.Role, "Tenant"),
            new Claim("phone", tenant.Phone ?? string.Empty)
        };

        return CreateJwtToken(claims);
    }

    private string CreateJwtToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "THIS_IS_MY_VERY_SECURE_SECRET_KEY_FOR_JWT_TOKEN_GENERATION_123456")
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}