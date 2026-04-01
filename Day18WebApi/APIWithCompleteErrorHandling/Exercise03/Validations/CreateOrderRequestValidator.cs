using Exercise03.Models;
using Exercise03.Repository;
using FluentValidation;

namespace Exercise03.Validations
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.")
                .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("Invalid Customer ID format.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .Must(items => items.Count <= 50).WithMessage("A maximum of 50 items are allowed.");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemRequestValidator(new ProductRepository())); // Apply item-level validation here

            RuleFor(x => x.ShippingAddress)
                .SetValidator(new ShippingAddressValidator());

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required.")
                .Must(pm => new[] { "CreditCard", "PayPal", "BankTransfer" }.Contains(pm))
                .WithMessage("Invalid payment method.");

            RuleFor(x => x.CouponCode)
                .Matches(@"^[A-Za-z0-9]{6,20}$").WithMessage("Coupon code must be 6-20 alphanumeric characters.")
                .When(x => !string.IsNullOrEmpty(x.CouponCode));
        }
    }
}