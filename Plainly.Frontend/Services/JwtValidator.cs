using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace Plainly.Frontend.Services;

public class JwtValidator(AppConfig appConfig)
{

    public bool ValidateToken(string token, string publicKeyPem, out ClaimsPrincipal? principal)
    {
        principal = null;

        try
        {
            // var rsa = RSA.Create();
            // rsa.ImportFromPem(appConfig.Jwt.PublicKeyPem);
            // rsa.ExportParameters()

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = appConfig.Jwt.Issuer,
                ValidAudience = appConfig.Jwt.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(new RSAParameters {}),
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