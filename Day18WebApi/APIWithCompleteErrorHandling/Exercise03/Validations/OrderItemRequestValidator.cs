using Exercise03.Repository;
using Exercise03.Models;
using FluentValidation;
namespace Exercise03.Validations
{
    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        private readonly IProductRepository productRepository;

        public OrderItemRequestValidator(IProductRepository productRepository)
        {
            this.productRepository = productRepository;

            RuleFor(x => x.ProductId).MustAsync(async (id, cancellationToken) => await productRepository.ExistsAsync(id, cancellationToken))
                .WithMessage("Product not found.");
            RuleFor(x => x.Quantity).InclusiveBetween(1, 100).WithMessage("Quantity must be between 1 and 100.");
            RuleFor(x => x).MustAsync(async (item, cancellationToken) => await productRepository.HasSufficientStockAsync(item.ProductId, item.Quantity, cancellationToken))
                .WithMessage("Insufficient stock available.");
        }
    }

}