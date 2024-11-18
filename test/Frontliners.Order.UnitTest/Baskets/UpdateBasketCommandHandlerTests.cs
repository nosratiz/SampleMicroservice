using System.Linq.Expressions;
using FluentAssertions;
using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Applications.Basket.Command.Update;
using Frontliners.Order.InfraStructures;
using Moq;

namespace Frontliners.Order.UnitTest.Baskets;

public class UpdateBasketCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Item_Quantity_If_Order_And_Item_Exist()
    {
        var orderRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Order>>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        var handler = new UpdateBasketCommandHandler(orderRepositoryMock.Object, currentUserServiceMock.Object);

        var existingOrder = new Domain.Entities.Order();
        existingOrder.AddOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        existingOrder.AddItem(productId, 1, 10.0m, "Product A");
        orderRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);
        currentUserServiceMock.Setup(service => service.UserId).Returns(existingOrder.UserId.ToString());

        var command = new UpdateBasketCommand(productId,5);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        existingOrder.Items.First().Quantity.Should().Be(5);
        orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_If_Order_Not_Found()
    {
        var orderRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Order>>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        var handler = new UpdateBasketCommandHandler(orderRepositoryMock.Object, currentUserServiceMock.Object);

        orderRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.Order)null);
        currentUserServiceMock.Setup(service => service.UserId).Returns(Guid.NewGuid().ToString());

        var command = new UpdateBasketCommand(Guid.NewGuid(),5);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == OrderErrorMessage.OrderNotFound);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_If_Order_Item_Not_Found()
    {
        var orderRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Order>>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        var handler = new UpdateBasketCommandHandler(orderRepositoryMock.Object, currentUserServiceMock.Object);

        var existingOrder = new Domain.Entities.Order();
        existingOrder.AddOrder(Guid.NewGuid());
        orderRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);
        currentUserServiceMock.Setup(service => service.UserId).Returns(existingOrder.UserId.ToString());

        var command = new UpdateBasketCommand(Guid.NewGuid(),5);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == OrderErrorMessage.OrderItemNotFound);
    }
}