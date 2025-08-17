using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.JSInterop;
using Plainly.Frontend.Errors;

namespace Plainly.Frontend.Services;

public class JwtTokenValidationService(IConfiguration configuration, IJSRuntime jsRuntime)
{
    public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
    {
        var isValidSignature = await jsRuntime.InvokeAsync<bool>(
            "cryptoFunctions.verifyEcdsaSignature",
            token,
            configuration["Jwt:PublicKey"]
        );
        if (!isValidSignature)
            throw new AuthError(Messages.InvalidJWTSignature);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            SignatureValidator = (t, vp) => new JwtSecurityToken(t),
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.ValidateToken(token, validationParameters, out _);
    }
}