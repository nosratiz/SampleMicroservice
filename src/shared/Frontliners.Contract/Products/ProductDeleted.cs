namespace Frontliners.Contract.Products;

public sealed record ProductDeleted
{
    public Guid ProductId { get; init; }
}