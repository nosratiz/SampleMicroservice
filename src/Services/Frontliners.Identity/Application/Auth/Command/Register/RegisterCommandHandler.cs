using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Users;
using Frontliners.Identity.Domain.Entities;
using Frontliners.Identity.InfraStructure.Interfaces;
using MassTransit;
using MediatR;

namespace Frontliners.Identity.Application.Auth.Command.Register;

public sealed class RegisterCommandHandler(
    IMongoRepository<User> userRepository,
    [FromKeyedServices("Email")] INotificationService notificationService,IPublishEndpoint publishEndpoint)
    : IRequestHandler<RegisterCommand, Result<Unit>>
{
    
    public async Task<Result<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName
        );

        await userRepository.AddAsync(newUser, cancellationToken);

        await notificationService.SendMessagesAsync("Welcome to our platform", request.Email, cancellationToken);
        
        await publishEndpoint.Publish
            (new UserCreated(newUser.Id, 
                    newUser.Email,
                    newUser.FirstName,
                    newUser.LastName)
                , cancellationToken);

        return Result.Ok();
    }
}