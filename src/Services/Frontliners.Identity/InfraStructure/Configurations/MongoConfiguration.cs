using Frontliners.Common.Mongo;
using Frontliners.Identity.Domain.Entities;

namespace Frontliners.Identity.InfraStructure.Configurations;

public static class MongoConfiguration
{
    public static void AddMongoCollection(this IServiceCollection services)
    {
        services.AddMongoRepository<User>("User");
    }
}