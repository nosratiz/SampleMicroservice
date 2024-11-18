using Frontliners.Inventory.InfraStructure.Configurations;
using Frontliners.Inventory.InfraStructure.Profile;
using Mapster;

namespace Frontliners.Inventory;

public static class ConfigureService
{
    public static IServiceCollection AddInventoryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoCollection();
        services.AddMapster();
        ProfileConfiguration.ConfigureProfile();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }
}