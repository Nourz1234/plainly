using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
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
        try
        {
            return handler.ValidateToken(token, validationParameters, out _);

        }
        catch (SecurityTokenException e)
        {
            if (e.Message.Contains("IDX10223"))
                throw new AuthError(Messages.SessionExpired);
            else
                throw new AuthError(e.Message);
        }
    }
}