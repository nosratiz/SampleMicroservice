namespace Frontliners.Contract.Products;

public sealed record ProductUpdated
{
    public Guid ProductId { get; init; }
    
    public string Name { get; init; } = null!;
    
    public decimal Price { get; init; }
    
    public int Stock { get; init; }
}