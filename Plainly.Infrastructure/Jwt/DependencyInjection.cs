using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Plainly.Application.Interface;

namespace Plainly.Infrastructure.Jwt;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Add authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                var publicKeyBytes = Convert.FromBase64String(configuration["Jwt:PublicKey"]!);
                var ecdsa = ECDsa.Create();
                ecdsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new ECDsaSecurityKey(ecdsa) { KeyId = configuration["Jwt:KeyId"] }
                };
            });
        // Add service
        services.AddScoped<IJwtService, JwtService>();
        // Add options
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        return services;
    }
}