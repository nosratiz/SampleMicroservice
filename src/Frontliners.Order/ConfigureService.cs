using System.Reflection;
using Frontliners.Order.InfraStructures.Configurations;
using Frontliners.Order.InfraStructures.Profile;
using Mapster;
using Microsoft.OpenApi.Models;

namespace Frontliners.Order;

public static class ConfigureService
{
    public static IServiceCollection AddOrderService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoCollection();
        services.AddMapster();
        ProfileConfiguration.ConfigureProfile();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo {Title = "Order service", Version = "v1"});

            opts.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description =
                        "Please enter into field the word 'Bearer' following by space and JWT",
                }
            );

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


            opts.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            );
        });

        
        return services;
    }
}