using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Plainly.Api.Models;

namespace Plainly.Api.Services;

public class JwtService
{
    private readonly string _Issuer;
    private readonly string _Audience;
    private readonly double _ExpiresInMinutes;
    private readonly SigningCredentials _SigningCredentials;

    public JwtService(IConfiguration configuration)
    {
        _Issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not set.");
        _Audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not set.");
        _ExpiresInMinutes = double.TryParse(configuration["Jwt:ExpiresInMinutes"], out var minutes)
            ? minutes
            : throw new InvalidOperationException("Jwt:ExpiresInMinutes is not set or invalid.");
        var privateKey = configuration["Jwt:PrivateKey"] ?? throw new InvalidOperationException("Jwt:PrivateKey is not set.");

        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);
        _SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateToken(User user)
    {
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
        ];

        var token = new JwtSecurityToken(
            issuer: _Issuer,
            audience: _Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_ExpiresInMinutes),
            signingCredentials: _SigningCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
