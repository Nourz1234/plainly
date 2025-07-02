using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Plainly.Api.Models;

namespace Plainly.Api.Services;


public class JwtService(IConfiguration configuration)
{
    private readonly IConfiguration _Configuration = configuration;

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
        };

        var rsa = RSA.Create();
        rsa.ImportFromPem(_Configuration["Jwt:PublicKey"]);

        var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        var expiresInMinutes = _Configuration["Jwt:ExpiresInMinutes"] ?? throw new InvalidOperationException("Jwt:ExpiresInMinutes is not set.");

        var token = new JwtSecurityToken(
            issuer: "Plainly.Api",
            audience: "Plainly.Client",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(expiresInMinutes)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}