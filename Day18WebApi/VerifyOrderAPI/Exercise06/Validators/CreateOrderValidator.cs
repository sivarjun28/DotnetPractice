using Exercise06.Models;
using FluentValidation;
namespace Exercise06.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.OrderDate).NotEmpty();

            RuleFor(x => x.DeliveryDate).NotEmpty();

            RuleFor(x => x)
                .Must(x => x.DeliveryDate > x.OrderDate)
                .WithMessage("Delivery date must be after order date")
                .WithErrorCode("INVALID_DATE_RANGE");

            RuleFor(x => x)
                .Must(x => !x.IsExpress || x.DeliveryDate <= x.OrderDate.AddDays(2))
                .WithMessage("Express delivery must be within 2 days")
                .WithErrorCode("EXPRESS_DELIVERY_INVALID");

            RuleFor(x => x.Items)
                .NotEmpty()
                .Must(items => items.Select(i => i.SellerId).Distinct().Count() == 1)
                .WithMessage("All items must be from same seller")
                .WithErrorCode("MULTIPLE_SELLERS_NOT_ALLOWED");
        }
    }
}
