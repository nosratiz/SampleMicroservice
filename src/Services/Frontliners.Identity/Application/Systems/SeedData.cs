using Frontliners.Common.Mongo.Repository;
using Frontliners.Identity.Domain.Entities;

namespace Frontliners.Identity.Application.Systems;

public class SeedData(IMongoRepository<User> userRepository)
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
        }
    }
}