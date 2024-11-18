using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Users;
using Frontliners.Order.Consumers.Users;
using Frontliners.Order.Domain.Entities;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace Frontliners.Order.UnitTest.Consumers;

public class UserCreatedConsumerTests
{
    [Fact]
    public async Task Consume_Should_Add_New_User()
    {
        var userRepositoryMock = new Mock<IMongoRepository<User>>();
        var loggerMock = new Mock<ILogger<UserCreatedConsumer>>();
        var mapperMock = new Mock<IMapper>();
        var consumer = new UserCreatedConsumer(userRepositoryMock.Object, loggerMock.Object, mapperMock.Object);

        var userCreated = new UserCreated(Guid.NewGuid(), "x@gmail.com", "x", "y");
        var user = new User { Id = userCreated.UserId, Email = userCreated.Email };
        mapperMock.Setup(m => m.Map<User>(userCreated)).Returns(user);

        var contextMock = new Mock<ConsumeContext<UserCreated>>();
        contextMock.Setup(c => c.Message).Returns(userCreated);
        contextMock.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        await consumer.Consume(contextMock.Object);

        userRepositoryMock.Verify(repo => repo.AddAsync(user, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Consume_Should_Log_Information()
    {
        var userRepositoryMock = new Mock<IMongoRepository<User>>();
        var loggerMock = new Mock<ILogger<UserCreatedConsumer>>();
        var mapperMock = new Mock<IMapper>();
        var consumer = new UserCreatedConsumer(userRepositoryMock.Object, loggerMock.Object, mapperMock.Object);

        var userCreated = new UserCreated(Guid.NewGuid(), "x@gmail.com", "x", "y");
        var user = new User { Id = userCreated.UserId, Email = userCreated.Email };
        mapperMock.Setup(m => m.Map<User>(userCreated)).Returns(user);

        var contextMock = new Mock<ConsumeContext<UserCreated>>();
        contextMock.Setup(c => c.Message).Returns(userCreated);
        contextMock.Setup(c => c.CancellationToken).Returns(CancellationToken.None);

        await consumer.Consume(contextMock.Object);

        loggerMock.Verify(logger => logger.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"User with id {userCreated.UserId} created")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);

    }

}