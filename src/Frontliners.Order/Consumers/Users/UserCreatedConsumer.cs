using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Users;
using Frontliners.Order.Domain.Entities;
using MapsterMapper;
using MassTransit;
using MongoDB.Driver;

namespace Frontliners.Order.Consumers.Users;

public sealed class UserCreatedConsumer(IMongoRepository<User> userRepository, 
    ILogger<UserCreatedConsumer> logger,
    IMapper mapper)
    : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var newUser = mapper.Map<User>(context.Message);
        try
        {
            await userRepository.AddAsync(newUser, context.CancellationToken);

            logger.LogInformation($"User with id {context.Message.UserId} created");
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            logger.LogError($"User with id {context.Message.UserId} already exists.");
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while creating user with id {context.Message.UserId}");
            throw;
        }  
     
    }
}