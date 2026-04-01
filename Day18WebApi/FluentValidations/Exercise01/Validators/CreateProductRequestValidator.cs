using System.Data;
using Exercise01.Models;
using FluentValidation;
namespace Exercise01.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        // 1. Name: Required, 3-100 characters, no special characters except spaces and hyphens
        public CreateProductRequestValidator(IProductRepository repository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 100)
                .Matches(@"^[a-zA-Z0-9\s\-]+$")
                .WithMessage("Name can only contain letters, numbers, spaces, and hyphens");
            // 2. Price: Required, between 0.01 and 999999.99, max 2 decimal places
            RuleFor(x => x.Price)
                .InclusiveBetween(0.01m, 999999.99m)
                .Must(p => decimal.Round(p, 2) == p)
                .WithMessage("Price must have 2 decimal Places");
            // 3. SKU: Required, format XX9999 (2 uppercase letters + 4 digits), must be unique
            RuleFor(x => x.Sku)
                .NotEmpty()
                .Matches(@"^[A-Z]{2}\d{4}$")
                .MustAsync(async (sku, ct) =>
                            !await repository.SkuExistsAsync(sku, ct))
                .WithMessage("Sku must be Unique");
            // 4. Category: Required, must be one of: Electronics, Clothing, Food, Books, Sports, Home
            string[] validCategories = { "Electronics", "Clothing", "Food", "Books", "Sports", "Home" };
            RuleFor(x => x.Category)
                    .NotEmpty()
                    .Must(c => validCategories.Contains(c))
                    .WithMessage("Invalid Category");
            // 5. Description: Optional, max 1000 characters
            RuleFor(x => x.Description)
                    .MaximumLength(1000);
            // 6. Stock: Required, 0-10000
            RuleFor(x => x.Stock)
                    .NotEmpty()
                    .InclusiveBetween(0, 10000);
            // 7. Tags: Max 10 tags, each tag 2-30 characters
            RuleFor(X => X.Tags)
                    .Must(tags => tags.Count <= 10)
                    .WithMessage("Maximum 10 tags allowed");
            RuleForEach(x => x.Tags)
                    .Length(2, 20);
            // 8. Weight: If provided, must be 0.01-1000 kg
            // 9. Dimensions: If provided, all values must be > 0 and < 1000 cm
            // 10. Color: If provided, must be valid hex color (#RRGGBB) or color name
            When(x => x.Specifications != null, () =>
         {
             RuleFor(x => x.Specifications!.Weight)
                 .InclusiveBetween(0.01m, 1000m)
                 .When(x => x.Specifications!.Weight.HasValue);

             RuleFor(x => x.Specifications!.Color)
                 .Matches(@"^#([A-Fa-f0-9]{6})$|^[a-zA-Z]+$")
                 .When(x => !string.IsNullOrEmpty(x.Specifications!.Color))
                 .WithMessage("Color must be hex (#RRGGBB) or name");

             When(x => x.Specifications!.Dimensions != null, () =>
             {
                 RuleFor(x => x.Specifications!.Dimensions!.Length)
                     .GreaterThan(0).LessThan(1000);

                 RuleFor(x => x.Specifications!.Dimensions!.Width)
                     .GreaterThan(0).LessThan(1000);

                 RuleFor(x => x.Specifications!.Dimensions!.Height)
                     .GreaterThan(0).LessThan(1000);
             });
         });



        }


    }
}