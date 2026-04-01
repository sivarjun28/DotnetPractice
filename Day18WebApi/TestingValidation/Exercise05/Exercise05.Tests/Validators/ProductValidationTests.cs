using Exercise05.Models;
using Exercise05.Validators;
using FluentAssertions;

namespace Exercise05.Exercise05.Tests.Validators
{
    public class ProductValidationTests
    {
        private readonly CreateProductRequestValidator validator;

        public ProductValidationTests()
        {
            validator = new CreateProductRequestValidator();
        }

        // Test 1: Valid Product
        [Fact]
        public void Validate_ValidProduct_ShouldPass()
        {
            var product = new CreateProductRequest
            {
                Name = "Valid Product",
                Price = 50.0m,
                Sku = "AB1234"
            };

            var result = validator.Validate(product);

            result.IsValid.Should().BeTrue();
        }

        // Test 2: Name Validation
        [Theory]
        [InlineData("", false)]  
        [InlineData("AB", false)]  
        [InlineData("Valid Product", true)] 
        public void Validate_Name_ReturnsExpectedResult(string name, bool isValid)
        {
            var product = new CreateProductRequest { Name = name, Price = 50.0m, Sku = "AB1234" };

            var result = validator.Validate(product);

            result.IsValid.Should().Be(isValid);
        }

        // Test 3: Price Validation
        [Theory]
        [InlineData(0, false)]  // Zero price
        [InlineData(-10, false)]  // Negative price
        [InlineData(0.01, true)]  // Min valid price
        [InlineData(100.50, true)]  // Valid price
        [InlineData(1000000, false)]  // Too high price
        public void Validate_Price_ReturnsExpectedResult(decimal price, bool isValid)
        {
            var product = new CreateProductRequest { Name = "Valid Product", Price = price, Sku = "AB1234" };

            var result = validator.Validate(product);

            result.IsValid.Should().Be(isValid);
        }

        // Test 4: SKU Validation
        [Theory]
        [InlineData("AB1234", true)]  // Valid format
        [InlineData("AB123", false)]  // Too short
        [InlineData("123456", false)]  // No letters
        [InlineData("ABCD12", false)]  // Incorrect format
        public void Validate_Sku_ReturnsExpectedResult(string sku, bool isValid)
        {
            var product = new CreateProductRequest { Name = "Valid Product", Price = 50.0m, Sku = sku };

            var result = validator.Validate(product);

            result.IsValid.Should().Be(isValid);
        }
    }
}