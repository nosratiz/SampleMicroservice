namespace Frontliners.Inventory.Application.Products.Dto;

public sealed record ProductListDto(Guid ProductId, string Name, decimal Price, int Stock,DateTime CreatedAt);