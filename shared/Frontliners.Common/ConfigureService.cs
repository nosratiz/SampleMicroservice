using System.Reflection;
using System.Text;
using FluentValidation;
using Frontliners.Common.InfraStructure.Exceptions;
using Frontliners.Common.InfraStructure.Helper;
using Frontliners.Common.InfraStructure.Interfaces;
using Frontliners.Common.Mongo;
using Frontliners.Common.Options;
using Frontliners.Common.Pipeline;
using Frontliners.Common.RabbitMQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Frontliners.Common;

public static class ConfigureService
{
    public static IServiceCollection AddCommon(this IServiceCollection services,IConfiguration configuration, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        services.AddHttpContextAccessor();
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddProblemDetails();
        services.AddMongo(configuration);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddAllScoped<IBaseService>(AssemblyHelper.FindAllTypes());
        services.AddMassTransit(configuration);
 
        services.AddTokenService(configuration);
        services.AddAuthorization();
        
        return services;
    }
    
    static void AddTokenService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtSetting = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSetting);
        services.AddSingleton(jwtSetting);

        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(jwtSetting.SecretKey)
            ),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            RequireExpirationTime = false
        };

        services.AddSingleton(tokenValidationParameter);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameter;
            });
    }
}