using FluentValidation;
using InventoryManager.API.Models;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");
    }
}

