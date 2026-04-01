using Exercise01.Models;
using FluentValidation;

namespace Exercise01.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
           
            // Name
            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name!)
                    .Length(3, 100)
                    .Matches(@"^[a-zA-Z0-9\s\-]+$");
            });

            // Price
            When(x => x.Price.HasValue, () =>
            {
                RuleFor(x => x.Price!.Value)
                    .InclusiveBetween(0.01m, 999999.99m)
                    .Must(p => decimal.Round(p, 2) == p);
            });

            // Description
            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description!)
                    .MaximumLength(1000);
            });

            // Stock
            When(x => x.Stock.HasValue, () =>
            {
                RuleFor(x => x.Stock!.Value)
                    .InclusiveBetween(0, 10000);
            });

            // Tags
            When(x => x.Tags != null, () =>
            {
                RuleFor(x => x.Tags!)
                    .Must(tags => tags.Count <= 10);

                RuleForEach(x => x.Tags!)
                    .Length(2, 30);
            });
        }
    }
}