using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class UpdateSaleItemsValidatorTests
    {
        private readonly UpdateSaleItemsValidator _validator;

        public UpdateSaleItemsValidatorTests()
        {
            _validator = new UpdateSaleItemsValidator();
        }

        [Fact(DisplayName = "Valid SaleItems List Should Pass Validation")]
        public void ValidSaleItemsList_ShouldPassValidation()
        {
            // Arrange
            var saleItems = new List<SaleItem>
        {
            new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product 1",
                Quantity = 5,
                UnitPrice = 20.00m,
                Discount = 0.00m,
                TotalAmount = 100.00m
            },
            new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product 2",
                Quantity = 2,
                UnitPrice = 50.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            }
        };

            // Act
            var result = _validator.TestValidate(saleItems);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}
