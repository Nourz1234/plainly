using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Plainly.Infrastructure.Persistence.AppDatabase;
using Plainly.Infrastructure.Persistence.AppDatabase.Seeders;
using Plainly.Infrastructure.Persistence.LogDatabase;

namespace Plainly.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup(builder.Configuration);

        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        await startup.Configure(app, builder.Environment);

        if (args.Contains("--migrate"))
        {
            using var scope = app.Services.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            appDbContext.Database.Migrate();
            var logDbContext = scope.ServiceProvider.GetRequiredService<LogDbContext>();
            logDbContext.Database.Migrate();
        }
        else if (args.Contains("--seed"))
        {
            using var scope = app.Services.CreateScope();
            await DatabaseSeeder.SeedAllAsync(scope.ServiceProvider);
        }
        else if (args.Contains("--gen-jwt-keys"))
        {
            using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            var privateKeyBytes = ecdsa.ExportPkcs8PrivateKey();
            var publicKeyBytes = ecdsa.ExportSubjectPublicKeyInfo();

            var data = new
            {
                KeyId = Guid.NewGuid().ToString(),
                PrivateKey = Convert.ToBase64String(privateKeyBytes),
                PublicKey = Convert.ToBase64String(publicKeyBytes)
            };
            File.WriteAllText("ec_keys.json", JsonSerializer.Serialize(data));
        }
        else
        {
            // var rsa = RSA.Create();
            // rsa.ImportFromPem(app.Configuration["Jwt:PublicKey"]);
            // var rsaParameters = rsa.ExportParameters(false);
            // var data = new
            // {
            //     Exponent = Convert.ToBase64String(rsaParameters.Exponent!),
            //     Modulus = Convert.ToBase64String(rsaParameters.Modulus!)
            // };
            // File.WriteAllText("rsaParameters.json", JsonSerializer.Serialize(data));

            await app.RunAsync();
        }
    }
}
