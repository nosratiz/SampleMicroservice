using Frontliners.Common.Mongo;
using Frontliners.Order.Domain.Entities;

namespace Frontliners.Order.InfraStructures.Configurations;

public static class MongoConfiguration
{
    public static void AddMongoCollection(this IServiceCollection services)
    {
        services.AddMongoRepository<User>("User");
        services.AddMongoRepository<Product>("Product");
        services.AddMongoRepository<Domain.Entities.Order>("Order");
    }
}