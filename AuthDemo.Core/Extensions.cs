using System.Text;
using AuthDemo.Core.Config;
using AuthDemo.Core.Models;
using AuthDemo.Core.Services;
using AuthDemo.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthDemo.Core;

public static class Extensions
{
    public static void AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new JwtSettings();
        configuration.GetSection("Auth").Bind(config);
        services.AddSingleton<JwtSettings>(config);
        
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IJwtHandler, JwtHandler>();

        services.AddAuth(config);
    }

    private static void AddAuth(this IServiceCollection services, JwtSettings settings)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
    }
}