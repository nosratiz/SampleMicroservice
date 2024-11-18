using System.Reflection;
using Frontliners.Common.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frontliners.Common.RabbitMQ;

public static class Extensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        
        var rabbitMqSetting = new RabbitMqSettings();
        configuration.Bind(nameof(RabbitMqSettings), rabbitMqSetting);
        
        services.AddSingleton(rabbitMqSetting);
        
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            var entryAssembly = Assembly.GetEntryAssembly();
            x.AddConsumers(entryAssembly);
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSetting.HostName, h =>
                {
                    h.Username(rabbitMqSetting.UserName);
                    h.Password(rabbitMqSetting.Password);
                });
                cfg.UseDelayedRedelivery(r =>
                {
                    r.Intervals(
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromMinutes(15),
                        TimeSpan.FromMinutes(30));
                });

                cfg.ConfigureEndpoints(context);
            });
            
            
        });
        
        return services;
    }
}