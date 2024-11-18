using Frontliners.Identity.InfraStructure.Configurations;
using Frontliners.Identity.InfraStructure.Interfaces;
using Frontliners.Identity.InfraStructure.Services;

namespace Frontliners.Identity;

public static class ConfigureService
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMongoCollection();
        services.AddKeyedScoped<INotificationService, EmailService>("Email");
        services.AddKeyedScoped<INotificationService, SmsService>("Sms");
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }
}