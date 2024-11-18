using Frontliners.Common.Mongo;
using Frontliners.Inventory.Domain.Entities;

namespace Frontliners.Inventory.InfraStructure.Configurations;

public static class MongoConfiguration
{
    public static void AddMongoCollection(this IServiceCollection services)
    {
        services.AddMongoRepository<Product>("Product");
    }
}