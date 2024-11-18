namespace Frontliners.Inventory.Application.Products.Dto;

public sealed record ProductDto(Guid ProductId, string Name, string Description, decimal Price, int Stock);