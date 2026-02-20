using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameDb.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameDb.Api.IntegrationTests;

internal static class TestJwtTokenFactory
{
    public static string CreateToken(IServiceProvider serviceProvider, string role, string subject = "integration-test-user")
    {
        var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var now = DateTimeOffset.UtcNow;

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims:
            [
                new Claim("sub", subject),
                new Claim("role", role)
            ],
            notBefore: now.UtcDateTime,
            expires: now.AddMinutes(15).UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
