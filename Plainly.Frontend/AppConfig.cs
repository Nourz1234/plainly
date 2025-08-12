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
        public JwtRsaParameters PublicKeyRSAParameters { get; set; } = new();
    }

    public class JwtRsaParameters
    {
        public string Exponent { get; set; } = "";
        public string Modulus { get; set; } = "";
    }
}