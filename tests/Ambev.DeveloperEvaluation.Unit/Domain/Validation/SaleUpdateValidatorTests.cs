using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleUpdateValidatorTests
    {
        private readonly SaleUpdateValidator _validator;

        public SaleUpdateValidatorTests()
        {
            _validator = new SaleUpdateValidator();
        }

        [Fact(DisplayName = "Valid Sale Update Should Pass Validation")]
        public void ValidSaleUpdate_ShouldPassValidation()
        {
            // Arrange
            var existingSale = new Sale
            {
                SaleNumber = "SN123",
                Customer = "Customer A"
            };

            var newSalesData = new Sale
            {
                SaleNumber = "SN123",
                Customer = "Customer A"
            };

            var salePair = (ExistingSale: existingSale, NewSalesData: newSalesData);

            // Act
            var result = _validator.TestValidate(salePair);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "Sale Update with Different SaleNumber Should Fail Validation")]
        public void SaleUpdateWithDifferentSaleNumber_ShouldFailValidation()
        {
            // Arrange
            var existingSale = new Sale
            {
                SaleNumber = "SN123",
                Customer = "Customer A"
            };

            var newSalesData = new Sale
            {
                SaleNumber = "SN124",
                Customer = "Customer A"
            };

            var salePair = (ExistingSale: existingSale, NewSalesData: newSalesData);

            // Act
            var result = _validator.TestValidate(salePair);

            // Assert
            result.ShouldHaveValidationErrorFor(salePair => salePair.NewSalesData.SaleNumber)
                  .WithErrorMessage("SaleNumber cannot be updated.");
        }

        [Fact(DisplayName = "Sale Update with Different Customer Should Fail Validation")]
        public void SaleUpdateWithDifferentCustomer_ShouldFailValidation()
        {
            // Arrange
            var existingSale = new Sale
            {
                SaleNumber = "SN123",
                Customer = "Customer A"
            };

            var newSalesData = new Sale
            {
                SaleNumber = "SN123",
                Customer = "Customer B"
            };

            var salePair = (ExistingSale: existingSale, NewSalesData: newSalesData);

            // Act
            var result = _validator.TestValidate(salePair);

            // Assert
            result.ShouldHaveValidationErrorFor(salePair => salePair.NewSalesData.Customer)
                  .WithErrorMessage("Customer cannot be updated.");
        }
    }

}
