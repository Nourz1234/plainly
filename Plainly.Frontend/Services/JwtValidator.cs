using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace Plainly.Frontend.Services;

public class JwtValidator
{
    private readonly AppConfig appConfig;
    private readonly RsaSecurityKey _IssuerSigningKey;

    public JwtValidator(AppConfig appConfig)
    {
        this.appConfig = appConfig;

        _IssuerSigningKey = new RsaSecurityKey(new RSAParameters
        {
            Exponent = Convert.FromBase64String(appConfig.Jwt.PublicKeyRSAParameters.Exponent),
            Modulus = Convert.FromBase64String(appConfig.Jwt.PublicKeyRSAParameters.Modulus)
        });
    }

    public bool ValidateToken(string token, out ClaimsPrincipal? principal)
    {
        principal = null;

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = appConfig.Jwt.Issuer,
                ValidAudience = appConfig.Jwt.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _IssuerSigningKey,
                ClockSkew = TimeSpan.Zero
            };

            var handler = new JwtSecurityTokenHandler();
            principal = handler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}