using Frontliners.Common.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Frontliners.Inventory.Domain.Entities;

public sealed class Product :  IIdentifiable
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    
    public Product(string name, string description, decimal price, int stock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void Update(string name, string description, decimal price, int stock)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
    
    public void Restock(int quantity)
    {
        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Sell(int quantity)
    {
        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
}