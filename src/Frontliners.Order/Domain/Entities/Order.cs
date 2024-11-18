using Frontliners.Common.Mongo;
using Frontliners.Order.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Frontliners.Order.Domain.Entities;

public sealed class Order : IIdentifiable
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } 
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; private set; }
    public Status Status { get; private set; } 
    
    public List<OrderItem> Items { get; set; } = new();
    
    public void AddOrder(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Status = Status.InBasket;
        CreatedAt = DateTime.UtcNow;
    }
   

    public void AddItem(Guid productId, int quantity, decimal price,string productName)
    {
        Items.Add(new OrderItem(productId, quantity, price,productName));
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RemoveItem(Guid productId)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);
        
        if (item is null) return;
       
        Items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateItem(Guid productId, int quantity)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);
        
        if (item is null) 
            return;
        
        item.UpdateQuantity(quantity);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Checkout()
    {
        Status = Status.Ordered;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Cancel()
    {
        Status = Status.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Ship()
    {
        Status = Status.Shipped;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Deliver()
    {
        Status = Status.Delivered;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public decimal TotalPrice => Items.Sum(x => x.TotalPrice);
    
    
}

public sealed class OrderItem(Guid productId, int quantity, decimal price,string productName)
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProductId { get; private set; } = productId;
    public int Quantity { get; private set; } = quantity;
    public decimal Price { get; private set; } = price;

    public string ProductName { get; private set; } = productName;
    public decimal TotalPrice => Quantity * Price;

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}