using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plainly.Api.Entities;

namespace Plainly.Api.Infrastructure.Jwt;

public class JwtService
{
    private readonly UserManager<User> _UserManager;
    private readonly JwtOptions _Options;
    private readonly SigningCredentials _SigningCredentials;

    public JwtService(UserManager<User> userManager, IOptions<JwtOptions> options)
    {
        _UserManager = userManager;
        _Options = options.Value;

        var rsa = RSA.Create();
        rsa.ImportFromPem(_Options.PrivateKey);
        _SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public async Task<string> GenerateToken(User user)
    {
        var userRoles = await _UserManager.GetRolesAsync(user);
        var userClaims = await _UserManager.GetClaimsAsync(user);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
            ..userRoles.Select(role => new Claim(ClaimTypes.Role, role)),
            ..userClaims.Where(c => c.Type == "scopes").Select(c => new Claim(c.Type, c.Value)),
        ];

        var token = new JwtSecurityToken(
            issuer: _Options.Issuer,
            audience: _Options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_Options.ExpiresInMinutes),
            signingCredentials: _SigningCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
