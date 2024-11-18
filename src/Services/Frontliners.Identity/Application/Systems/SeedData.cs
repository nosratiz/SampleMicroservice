using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Users;
using Frontliners.Identity.Domain.Entities;
using MassTransit;

namespace Frontliners.Identity.Application.Systems;

public class SeedData(IMongoRepository<User> userRepository,IPublishEndpoint publishEndpoint)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (userRepository.GetAll().Any() == false)
        {
            var user = new User("NimaNosrati93@gmail.com",
                "123456"
                ,"Nima",
                "Nosrati");
           
            user.Activate();
            
            await userRepository.AddAsync(user, cancellationToken);
            
            await publishEndpoint.Publish(new UserCreated(user.Id, user.Email, user.FirstName, user.LastName), cancellationToken);
        }
    }
}