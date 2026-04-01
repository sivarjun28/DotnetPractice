// In Exercise05/Validators/CreateProductRequestValidator.cs
using FluentValidation;
using Exercise05.Models;

namespace Exercise05.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            // Name should not be empty and must be at least 3 characters
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MinimumLength(3).WithMessage("Product name must be at least 3 characters long.");

            // Price must be greater than 0 (no zero or negative prices allowed)
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            // SKU must match the pattern of two letters followed by four digits (e.g., AB1234)
            RuleFor(x => x.Sku)
                .Matches(@"^[A-Za-z]{2}\d{4}$").WithMessage("SKU must follow the pattern: AA1234");
        }
    }
}