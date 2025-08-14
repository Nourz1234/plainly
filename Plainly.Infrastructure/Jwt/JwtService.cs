using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plainly.Application.Interface;
using Plainly.Application.Interface.Repositories;
using Plainly.Domain.Interfaces;

namespace Plainly.Infrastructure.Jwt;

public class JwtService : IJwtService
{
    private readonly IUserRepository _UserRepository;
    private readonly JwtOptions _Options;
    private readonly SigningCredentials _SigningCredentials;

    public JwtService(IUserRepository userRepository, IOptions<JwtOptions> options)
    {
        _UserRepository = userRepository;
        _Options = options.Value;

        var privateKeyBytes = Convert.FromBase64String(_Options.PrivateKey);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
        _SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa) { KeyId = _Options.KeyId }, SecurityAlgorithms.EcdsaSha256);
    }

    public async Task<string> GenerateToken(IUser user)
    {
        var userRoles = await _UserRepository.GetRolesAsync(user);
        var userClaims = await _UserRepository.GetClaimsAsync(user);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
            ..userRoles.Select(role => new Claim(ClaimTypes.Role, role)),
            ..userClaims.Where(c => c.Type == "scopes"),
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
