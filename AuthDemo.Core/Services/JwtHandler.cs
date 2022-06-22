using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthDemo.Core.Config;
using AuthDemo.Core.Models;
using AuthDemo.Core.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AuthDemo.Core.Services;

public class JwtHandler : IJwtHandler
{
    private readonly JwtSettings _settings;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    private SecurityKey _issuerSigningKey;
    private SigningCredentials _signingCredentials;
    private JwtHeader _jwtHeader;
    private TokenValidationParameters Parameters { get; set; }

    public JwtHandler(JwtSettings settings)
    {
        _settings = settings;

        if (_settings.UseRsa)
        {
            InitializeRsa();
        }
        else
        {
            InitializeHmac();
        }
        InitializeJwtParameters();
    }

    private void InitializeRsa()
    {
        throw new NotImplementedException("RSA is not yet implemented");
    }

    private void InitializeHmac()
    {
        _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
    }

    private void InitializeJwtParameters()
    {
        _jwtHeader = new JwtHeader(_signingCredentials);
        Parameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidIssuer = _settings.Issuer,
            IssuerSigningKey = _issuerSigningKey
        };
    }

    public Tokens Create(Guid userId)
    {
        var nowUtc = DateTime.UtcNow;
        var expires = nowUtc.AddMinutes(_settings.ExpiryMinutes);
        var centuryBegin = new DateTime(1970, 1, 1);
        var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
        var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
        var issuer = _settings.Issuer ?? string.Empty;
        var payload = new JwtPayload
        {
            {"sub", userId.ToString()},
            {"unique_name", userId.ToString()},
            {"iss", issuer},
            {"iat", now},
            {"nbf", now},
            {"exp", exp},
            {"jti", Guid.NewGuid().ToString("N")}
        };
        var jwt = new JwtSecurityToken(_jwtHeader, payload);
        var token = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new Tokens
        {
            AccessToken = token
        };
    }
}