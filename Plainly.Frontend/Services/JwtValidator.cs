using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.JSInterop;
using Microsoft.IdentityModel.Logging;

namespace Plainly.Frontend.Services;

public class JwtValidator
{
    private readonly AppConfig appConfig;
    private readonly IJSRuntime jsRuntime;

    public JwtValidator(AppConfig appConfig, IJSRuntime jsRuntime)
    {
        this.appConfig = appConfig;
        this.jsRuntime = jsRuntime;
    }

    public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        try
        {
            var isValidSignature = await jsRuntime.InvokeAsync<bool>(
                "cryptoFunctions.verifyEcdsaSignature",
                token,
                appConfig.Jwt.PublicKey
            );
            if (!isValidSignature) return null;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("test")),
                ValidIssuer = appConfig.Jwt.Issuer,
                ValidAudience = appConfig.Jwt.Audience,
                SignatureValidator = (t, vp) => new JwtSecurityToken(t),
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    // public bool ValidateToken(string token, out ClaimsPrincipal? principal)
    // {
    //     principal = null;

    //     try
    //     {
    //         var validationParameters = new TokenValidationParameters
    //         {
    //             ValidateIssuer = true,
    //             ValidateAudience = true,
    //             ValidateLifetime = true,
    //             ValidateIssuerSigningKey = true,
    //             ValidIssuer = appConfig.Jwt.Issuer,
    //             ValidAudience = appConfig.Jwt.Audience,
    //             IssuerSigningKey = _IssuerSigningKey
    //         };

    //         var handler = new JwtSecurityTokenHandler();
    //         principal = handler.ValidateToken(token, validationParameters, out _);
    //         return true;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return false;
    //     }
    // }

}