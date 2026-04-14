using ECommerceAPI.Models.Requests;
using ECommerceAPI.Repositories.Interfaces;
using FluentValidation;

namespace ECommerceAPI.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _repository;

        public CreateProductRequestValidator(IProductRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Sku)
                .NotEmpty()
                .Length(3, 50)
                .MustAsync(BeUniqueSku)
                .WithMessage("SKU already exists");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .LessThan(1000000);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be Negative");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");
        }

        private async Task<bool> BeUniqueSku(string sku, CancellationToken cancellationToken)
        {
            return !await _repository.SkuExistsAsync(sku);
        }
        
    }
}