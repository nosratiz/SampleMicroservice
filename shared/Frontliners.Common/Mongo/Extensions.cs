using Frontliners.Common.Mongo.Repository;
using Frontliners.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Serilog;

namespace Frontliners.Common.Mongo;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection("MongoDbOptions"));

        var mongoDbOptions = new MongoDbOptions();
        configuration.Bind(nameof(MongoDbOptions), mongoDbOptions);

        services.AddSingleton(mongoDbOptions);

        var mongoClient = new MongoClient(mongoDbOptions.ConnectionString);
        
      
        services.AddSingleton(mongoClient);

        services.AddScoped(_ => mongoClient.GetDatabase(mongoDbOptions.Database));
        

        var mongoConnectionUrl = new MongoUrl(mongoDbOptions.ConnectionString);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
      
        mongoClientSettings.ClusterConfigurator = cb =>
        {
            cb.Subscribe<CommandStartedEvent>(e =>
            {
                Log.Information("{ObjCommandName} - {Json}", e.CommandName, e.Command.ToJson());
            });

            cb.Subscribe<CommandSucceededEvent>(e =>
            {
                Log.Information("{ObjCommandName} - {Json}", e.CommandName, e.Reply.ToJson());
            });

            cb.Subscribe<CommandFailedEvent>(e =>
            {
                Log.Error("{ObjCommandName} - {Json}", e.CommandName, e.Failure.ToJson());
            });

        };
            
        var mongoCfgClient = new MongoClient(mongoClientSettings);

        services.AddSingleton(mongoCfgClient);
        
        return services;

    }


    public static void AddMongoRepository<TEntity>(this IServiceCollection services, string collectionName)
        where TEntity : IIdentifiable
        => services.AddScoped<IMongoRepository<TEntity>>(provider =>
            new MongoRepository<TEntity>(provider.GetService<IMongoDatabase>()!, collectionName));
}