using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleItemValidatorTests
    {
        private readonly SaleItemValidator _validator;

        public SaleItemValidatorTests()
        {
            _validator = new SaleItemValidator();
        }

        [Fact(DisplayName = "Valid SaleItem Should Pass Validation")]
        public void ValidSaleItem_ShouldPassValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product Name",
                Quantity = 10,
                UnitPrice = 100.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "SaleItem with Empty ID Should Fail Validation")]
        public void SaleItemWithEmptyId_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.Empty,
                Product = "Product Name",
                Quantity = 10,
                UnitPrice = 100.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Id)
                  .WithErrorMessage("SaleItem ID is required.");
        }

        [Fact(DisplayName = "SaleItem with Empty Product Name Should Fail Validation")]
        public void SaleItemWithEmptyProductName_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = string.Empty,
                Quantity = 10,
                UnitPrice = 100.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Product)
                  .WithErrorMessage("Product name is required.");
        }

        [Fact(DisplayName = "SaleItem with Excessive Product Name Length Should Fail Validation")]
        public void SaleItemWithExcessiveProductNameLength_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = new string('A', 101),  // Exceeds 100 characters
                Quantity = 10,
                UnitPrice = 100.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Product)
                  .WithErrorMessage("Product name cannot exceed 100 characters.");
        }

        [Fact(DisplayName = "SaleItem with Zero Quantity Should Fail Validation")]
        public void SaleItemWithZeroQuantity_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product Name",
                Quantity = 0,
                UnitPrice = 100.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Quantity)
                  .WithErrorMessage("Quantity must be greater than 0.");
        }

        [Fact(DisplayName = "SaleItem with Negative Unit Price Should Fail Validation")]
        public void SaleItemWithNegativeUnitPrice_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product Name",
                Quantity = 10,
                UnitPrice = -1.00m,
                Discount = 5.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.UnitPrice)
                  .WithErrorMessage("Unit price must be greater than or equal to 0.");
        }

        [Fact(DisplayName = "SaleItem with Negative Discount Should Fail Validation")]
        public void SaleItemWithNegativeDiscount_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product Name",
                Quantity = 10,
                UnitPrice = 100.00m,
                Discount = -1.00m,
                TotalAmount = 95.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.Discount)
                  .WithErrorMessage("Discount must be greater than or equal to 0.");
        }

        [Fact(DisplayName = "SaleItem with Negative Total Amount Should Fail Validation")]
        public void SaleItemWithNegativeTotalAmount_ShouldFailValidation()
        {
            // Arrange
            var saleItem = new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = "Product Name",
                Quantity = 10,
                UnitPrice = 100.00m,
                Discount = 5.00m,
                TotalAmount = -1.00m
            };

            // Act
            var result = _validator.TestValidate(saleItem);

            // Assert
            result.ShouldHaveValidationErrorFor(item => item.TotalAmount)
                  .WithErrorMessage("Total amount must be greater than or equal to 0.");
        }
    }
}
