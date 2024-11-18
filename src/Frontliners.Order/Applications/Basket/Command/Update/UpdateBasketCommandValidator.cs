using FluentValidation;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Domain.Entities;
using Frontliners.Order.InfraStructures;

namespace Frontliners.Order.Applications.Basket.Command.Update;

public sealed class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
{
    private readonly IMongoRepository<Product> _productRepository;
    public UpdateBasketCommandValidator(IMongoRepository<Product> productRepository)
    {
        _productRepository = productRepository;
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(OrderErrorMessage.ProductIsRequired)
            .MustAsync(async (productId, cancellationToken) =>
            {
                var product = await productRepository.GetAsync(productId, cancellationToken);
                return product is not null;
            }).WithMessage(OrderErrorMessage.ProductNotFound);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage(OrderErrorMessage.QuantityMustBeGreaterThanZero);
        
        RuleFor(x => x).MustAsync(QuantityIsGreaterThanStock)
            .WithMessage(OrderErrorMessage.QuantityMustBeLessThanStock);
    }
    
    private async Task<bool> QuantityIsGreaterThanStock(UpdateBasketCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetAsync(command.ProductId, cancellationToken);
        
        return product?.Stock >= command.Quantity;
        
    }
}