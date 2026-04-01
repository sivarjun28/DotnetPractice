using Exercise03.Models;
using FluentValidation;
namespace Exercise03.Validations
{
    public class ShippingAddressValidator: AbstractValidator<ShippingAddress>
    {
        public ShippingAddressValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required.");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Street address is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode is required.")
                .Matches(@"^\d{5}(-\d{4})?$").WithMessage("Invalid ZipCode format.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }
    }
}