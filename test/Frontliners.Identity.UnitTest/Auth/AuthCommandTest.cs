using System.Linq.Expressions;
using FluentAssertions;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Users;
using Frontliners.Identity.Application.Auth.Command.Login;
using Frontliners.Identity.Application.Auth.Command.Register;
using Frontliners.Identity.Domain.Entities;
using Frontliners.Identity.InfraStructure;
using Frontliners.Identity.InfraStructure.Interfaces;
using MassTransit;
using Moq;

namespace Frontliners.Identity.UnitTest.Auth;

public class AuthCommandTest
{
    private readonly Mock<IMongoRepository<User>> _userRepositoryMock = new();
    private readonly Mock<INotificationService> _notificationServiceMock = new();
    private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();


    [Fact]
    public async Task Handle_Should_Add_New_User()
    {
        // Arrange
        var registerCommand = new RegisterCommand("X@gmail.com", "123456", "X", "Y");

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

        _notificationServiceMock.Setup(x =>
            x.SendMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        _publishEndpointMock.Setup(x =>
            x.Publish(It.IsAny<UserCreated>(), It.IsAny<CancellationToken>()));

        var handler = new RegisterCommandHandler(_userRepositoryMock.Object, _notificationServiceMock.Object,
            _publishEndpointMock.Object);

        // Act
        var result = await handler.Handle(registerCommand, CancellationToken.None);


        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_Should_Publish_UserCreated_Event()
    {
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

        _notificationServiceMock.Setup(x =>
            x.SendMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        _publishEndpointMock.Setup(x =>
            x.Publish(It.IsAny<UserCreated>(), It.IsAny<CancellationToken>()));
        
        var handler = new RegisterCommandHandler(_userRepositoryMock.Object, _notificationServiceMock.Object, _publishEndpointMock.Object);

        // Arrange
        var registerCommand = new RegisterCommand("X@gmail.com", "123456", "X", "Y");

        await handler.Handle(registerCommand, CancellationToken.None);

        _publishEndpointMock.Verify(endpoint => endpoint.Publish(It.IsAny<UserCreated>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Handle_Should_Send_Notification()
    {
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

        _notificationServiceMock.Setup(x =>
            x.SendMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        _publishEndpointMock.Setup(x =>
            x.Publish(It.IsAny<UserCreated>(), It.IsAny<CancellationToken>()));

        var handler = new RegisterCommandHandler(_userRepositoryMock.Object, _notificationServiceMock.Object,
            _publishEndpointMock.Object);

        // Arrange
        var registerCommand = new RegisterCommand("X@gmail.com", "123456", "X", "Y");

        await handler.Handle(registerCommand, CancellationToken.None);

        _notificationServiceMock.Verify(
            service => service.SendMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);

    }
    
    
      [Fact]
    public async Task Handle_Should_Return_Token_For_Valid_Credentials()
    {
        var userRepositoryMock = new Mock<IMongoRepository<User>>();
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginCommandHandler(userRepositoryMock.Object, tokenServiceMock.Object);

        var user = new User("X@gmail.com", "123456", "X", "Y");
        user.Activate();
        userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        tokenServiceMock.Setup(service => service.GenerateToken(It.IsAny<User>())).Returns("token");

        var command = new LoginCommand("X@gmail.com", "123456");
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be("token");
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_For_Invalid_Credentials()
    {
        var userRepositoryMock = new Mock<IMongoRepository<User>>();
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginCommandHandler(userRepositoryMock.Object, tokenServiceMock.Object);

        userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

        var command = new LoginCommand("X@gmail.com", "123456");
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == IdentityErrorMessage.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_For_Inactive_User()
    {
        var userRepositoryMock = new Mock<IMongoRepository<User>>();
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginCommandHandler(userRepositoryMock.Object, tokenServiceMock.Object);

        var user = new User("X@gmail.com", "123456", "X", "Y");
        userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var command = new LoginCommand("X@gmail.com", "123456");
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == IdentityErrorMessage.UserIsInactive);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_For_Wrong_Password()
    {
        var userRepositoryMock = new Mock<IMongoRepository<User>>();
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginCommandHandler(userRepositoryMock.Object, tokenServiceMock.Object);

        var user = new User("X@gmail.com", "123456", "X", "Y");
        user.Activate();
        userRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var command = new LoginCommand("X@gmail.com", "123456677");
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == IdentityErrorMessage.InvalidCredentials);
    }
}
    
    
