using Frontliners.Common.Mongo.Repository;
using Frontliners.Identity.Domain.Entities;
using MediatR;

namespace Frontliners.Identity.Application.Systems;

public sealed record SeedDataCommand : IRequest;

public sealed class SeedDataCommandHandler(IMongoRepository<User> userRepository)
    : IRequestHandler<SeedDataCommand>
{
    public async Task Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        await new SeedData(userRepository).SeedAsync(cancellationToken);
    }
}