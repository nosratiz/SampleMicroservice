using FluentAssertions;
using Frontliners.Order.Domain.Enum;

namespace Frontliners.Order.UnitTest;

public class OrderTest
{
       [Fact]
    public void AddOrder_Should_Set_Initial_Values()
    {
        var order = new Domain.Entities.Order();
        var userId = Guid.NewGuid();

        order.AddOrder(userId);

        order.Id.Should().NotBeEmpty();
        order.UserId.Should().Be(userId);
        order.Status.Should().Be(Status.InBasket);
        order.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddItem_Should_Add_New_Item()
    {
        var order = new Domain.Entities.Order();
        var productId = Guid.NewGuid();
        var quantity = 2;
        var price = 10.0m;
        var productName = "Product A";

        order.AddItem(productId, quantity, price, productName);

        order.Items.Should().ContainSingle();
        var item = order.Items.First();
        item.ProductId.Should().Be(productId);
        item.Quantity.Should().Be(quantity);
        item.Price.Should().Be(price);
        item.ProductName.Should().Be(productName);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void RemoveItem_Should_Remove_Existing_Item()
    {
        var order = new Domain.Entities.Order();
        var productId = Guid.NewGuid();
        order.AddItem(productId, 1, 10.0m, "Product A");

        order.RemoveItem(productId);

        order.Items.Should().BeEmpty();
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void RemoveItem_Should_Not_Change_If_Item_Not_Found()
    {
        var order = new Domain.Entities.Order();
        var productId = Guid.NewGuid();

        order.RemoveItem(productId);

        order.Items.Should().BeEmpty();
    }

    [Fact]
    public void UpdateItem_Should_Update_Quantity_Of_Existing_Item()
    {
        var order = new Domain.Entities.Order();
        var productId = Guid.NewGuid();
        order.AddItem(productId, 1, 10.0m, "Product A");

        order.UpdateItem(productId, 5);

        var item = order.Items.First();
        item.Quantity.Should().Be(5);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateItem_Should_Not_Change_If_Item_Not_Found()
    {
        var order = new Domain.Entities.Order();
        var productId = Guid.NewGuid();

        order.UpdateItem(productId, 5);

        order.Items.Should().BeEmpty();
    }

    [Fact]
    public void Checkout_Should_Set_Status_To_Ordered()
    {
        var order = new Domain.Entities.Order();
        order.AddOrder(Guid.NewGuid());

        order.Checkout();

        order.Status.Should().Be(Status.Ordered);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Cancel_Should_Set_Status_To_Cancelled()
    {
        var order = new Domain.Entities.Order();
        order.AddOrder(Guid.NewGuid());

        order.Cancel();

        order.Status.Should().Be(Status.Cancelled);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Ship_Should_Set_Status_To_Shipped()
    {
        var order = new Domain.Entities.Order();
        order.AddOrder(Guid.NewGuid());

        order.Ship();

        order.Status.Should().Be(Status.Shipped);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Deliver_Should_Set_Status_To_Delivered()
    {
        var order = new Domain.Entities.Order();
        order.AddOrder(Guid.NewGuid());

        order.Deliver();

        order.Status.Should().Be(Status.Delivered);
        order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void TotalPrice_Should_Calculate_Correctly()
    {
        var order = new Domain.Entities.Order();
        order.AddItem(Guid.NewGuid(), 2, 10.0m, "Product A");
        order.AddItem(Guid.NewGuid(), 3, 20.0m, "Product B");

        order.TotalPrice.Should().Be(80.0m);
    }
}