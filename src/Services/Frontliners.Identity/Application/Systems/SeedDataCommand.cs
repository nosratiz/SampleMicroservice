using Frontliners.Common.Mongo.Repository;
using Frontliners.Identity.Domain.Entities;
using MassTransit;
using MediatR;

namespace Frontliners.Identity.Application.Systems;

public sealed record SeedDataCommand : IRequest;

public sealed class SeedDataCommandHandler(IMongoRepository<User> userRepository,IPublishEndpoint publishEndpoint)
    : IRequestHandler<SeedDataCommand>
{
    public async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        await new SeedData(userRepository,publishEndpoint).SeedAsync(cancellationToken);
    }
}