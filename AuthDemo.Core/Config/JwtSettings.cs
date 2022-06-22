namespace AuthDemo.Core.Config;

public class JwtSettings
{
    public string Issuer { get; set; }
    public int ExpiryMinutes { get; set; }
    public string Secret { get; set; }
    public bool UseRsa { get; set; }
    public string RsaPrivateKeyXml { get; set; }
    public string RsaPublicKeyXml { get; set; }
}