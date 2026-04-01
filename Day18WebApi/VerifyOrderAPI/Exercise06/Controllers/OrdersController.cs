using Exercise06.Exceptions;
using Exercise06.Models;
using Exercise06.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IValidator<CreateOrderRequest> _validator;
    private readonly IAddressValidationService _addressService;

    public OrdersController(IValidator<CreateOrderRequest> validator, IAddressValidationService addressService)
    {
        _validator = validator;
        _addressService = addressService;
    }
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Working");
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new CustomValidationException(result.Errors);

        var isValidAddress = await _addressService.ValidateAsync(request.Address);
        if (!isValidAddress)
            throw new BusinessRuleException("INVALID_ADDRESS", "Address is invalid");

        return Ok(new { message = "Order created successfully" });
    }

}