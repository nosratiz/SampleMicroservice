using FluentValidation;
using Frontliners.Inventory.InfraStructure;

namespace Frontliners.Inventory.Application.Products.Command.Update;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().WithMessage(InventoryErrorMessage.NameRequired);
        RuleFor(x => x.Description).NotEmpty().WithMessage(InventoryErrorMessage.DescriptionRequired);
        RuleFor(x => x.Price).GreaterThan(0).WithMessage(InventoryErrorMessage.PriceRequired);
        RuleFor(x => x.Stock).GreaterThan(0).WithMessage(InventoryErrorMessage.StockRequired);
    }
}