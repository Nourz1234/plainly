using System.Security.Cryptography;

namespace Plainly.Frontend;

public class AppConfig
{
    public string ApiBaseUrl { get; set; } = "";
    public JwtSettings Jwt { get; set; } = new();

    public class JwtSettings
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public RSAParameters PublicKeyRSAParameters { get; set; } = new();
    }
}