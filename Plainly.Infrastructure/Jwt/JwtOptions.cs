namespace Plainly.Infrastructure.Jwt;

public class JwtOptions
{
    public required string PrivateKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpiresInMinutes { get; set; }
}