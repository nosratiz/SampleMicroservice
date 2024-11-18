using FluentValidation;
using Frontliners.Inventory.InfraStructure;

namespace Frontliners.Inventory.Application.Products.Command.Create;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(InventoryErrorMessage.NameRequired);
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(InventoryErrorMessage.DescriptionRequired);
        
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage(InventoryErrorMessage.PriceRequired)
            .GreaterThan(0).WithMessage(InventoryErrorMessage.InvalidPrice);
        
        RuleFor(x => x.Stock)
            .NotEmpty().WithMessage(InventoryErrorMessage.StockRequired)
            .GreaterThan(0).WithMessage(InventoryErrorMessage.InvalidStock);
    }
}