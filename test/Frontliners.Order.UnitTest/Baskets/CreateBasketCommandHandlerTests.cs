using System.Linq.Expressions;
using FluentAssertions;
using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Applications.Basket.Command.Create;
using Frontliners.Order.Domain.Entities;
using Moq;

namespace Frontliners.Order.UnitTest.Baskets;

public class CreateBasketCommandHandlerTests
{
       [Fact]
    public async Task Handle_Should_Create_New_Order_If_None_Exists()
    {
        var orderRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Order>>();
        var productRepositoryMock = new Mock<IMongoRepository<Product>>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        var handler = new CreateBasketCommandHandler(orderRepositoryMock.Object, productRepositoryMock.Object, currentUserServiceMock.Object);

        currentUserServiceMock.Setup(service => service.UserId).Returns("9f77b3b5-e9ba-4796-9332-6ab2a84493f7");
        orderRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.Order)null);
        productRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Product(Guid.NewGuid(), "Product A", 10.0m,1));

        var command = new CreateBasketCommand(Guid.NewGuid(), 1);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Add_Item_To_Existing_Order()
    {
        var orderRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Order>>();
        var productRepositoryMock = new Mock<IMongoRepository<Product>>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        var handler = new CreateBasketCommandHandler(orderRepositoryMock.Object, productRepositoryMock.Object, currentUserServiceMock.Object);

        var existingOrder = new Domain.Entities.Order();
        
        existingOrder.AddOrder(Guid.NewGuid());
        orderRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingOrder);
        
        productRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Product(Guid.NewGuid(), "Product A", 10.0m,1));

        var command = new CreateBasketCommand(Guid.NewGuid(), 1);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        existingOrder.Items.Should().ContainSingle();
        orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Update_Quantity_Of_Existing_Item()
    {
        var orderRepositoryMock = new Mock<IMongoRepository<Domain.Entities.Order>>();
        var productRepositoryMock = new Mock<IMongoRepository<Product>>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        var handler = new CreateBasketCommandHandler(orderRepositoryMock.Object, productRepositoryMock.Object, currentUserServiceMock.Object);

        var existingOrder = new Domain.Entities.Order();
        existingOrder.AddOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        existingOrder.AddItem(productId, 1, 10.0m, "Product A");
        orderRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Domain.Entities.Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingOrder);

        var command = new CreateBasketCommand(productId, 5);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        existingOrder.Items.First().Quantity.Should().Be(5);
        orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Order>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
}