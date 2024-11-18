using Frontliners.Contract.Products;
using Frontliners.Inventory.Application.Products.Dto;
using Frontliners.Inventory.Domain.Entities;
using Mapster;

namespace Frontliners.Inventory.InfraStructure.Profile;

public static class ProfileConfiguration
{
    public static void ConfigureProfile()
    {
        TypeAdapterConfig<Product, ProductListDto>.NewConfig()
            .Map(dest => dest.ProductId, src => src.Id);


        TypeAdapterConfig<Product, ProductDto>.NewConfig()
            .Map(dest => dest.ProductId, src => src.Id);


        TypeAdapterConfig<Product, ProductCreated>.NewConfig()
            .Map(dest => dest.ProductId, src => src.Id);


        TypeAdapterConfig<Product, ProductUpdated>.NewConfig();
        
        
        TypeAdapterConfig<Product, ProductDeleted>.NewConfig()
            .Map(dest => dest.ProductId, src => src.Id);


    }
}