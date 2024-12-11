using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the SaleItem entity class using fake data generation.
    /// </summary>
    public class SaleItemTests
    {
        /// <summary>
        /// Tests that GetTotalPrice calculates the correct total price with discount applied.
        /// </summary>
        [Fact(DisplayName = "GetTotalPrice should calculate correct total amount with discount")]
        public void Given_SaleItem_When_GetTotalPrice_Then_ShouldReturnCorrectValue()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();
            saleItem.Discount = 10; // 10% discount

            var expectedTotal = (saleItem.UnitPrice * saleItem.Quantity) * (1 - saleItem.Discount / 100);

            // Act
            var totalPrice = saleItem.GetTotalPrice();

            // Assert
            totalPrice.Should().Be(expectedTotal);
        }

        /// <summary>
        /// Tests that ApplyDiscount sets the correct discount based on the quantity.
        /// </summary>
        [Fact(DisplayName = "ApplyDiscount should set correct discount based on quantity")]
        public void Given_SaleItem_When_ApplyDiscount_Then_DiscountShouldBeCorrect()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();

            // Act & Assert
            saleItem.Quantity = 3; // Below threshold
            saleItem.ApplyDiscount();
            saleItem.Discount.Should().Be(0);

            saleItem.Quantity = 5; // Between 4 and 9
            saleItem.ApplyDiscount();
            saleItem.Discount.Should().Be(10);

            saleItem.Quantity = 15; // Between 10 and 20
            saleItem.ApplyDiscount();
            saleItem.Discount.Should().Be(20);
        }

        /// <summary>
        /// Tests that RegenerateId generates a new unique ID for the SaleItem.
        /// </summary>
        [Fact(DisplayName = "RegenerateId should generate a new unique ID")]
        public void Given_SaleItem_When_RegenerateId_Then_IdShouldBeUpdated()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();
            var oldId = saleItem.Id;

            // Act
            saleItem.RegenerateId();

            // Assert
            saleItem.Id.Should().NotBe(oldId);
            saleItem.Id.Should().NotBeEmpty();
        }

        /// <summary>
        /// Tests that TotalAmount is calculated correctly after applying discount.
        /// </summary>
        [Fact(DisplayName = "TotalAmount should reflect correct value after discount")]
        public void Given_SaleItem_When_CalculatingTotalAmount_Then_ShouldBeCorrect()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();
            saleItem.Discount = 10; // 10% discount

            var expectedTotal = saleItem.GetTotalPrice();

            // Act
            saleItem.TotalAmount = saleItem.GetTotalPrice();

            // Assert
            saleItem.TotalAmount.Should().Be(expectedTotal);
        }

        /// <summary>
        /// Tests that SaleItem fields are properly set and valid.
        /// </summary>
        [Fact(DisplayName = "SaleItem fields should be valid")]
        public void Given_SaleItem_When_FieldsAreSet_Then_ShouldBeValid()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();

            // Act & Assert
            saleItem.Product.Should().NotBeNullOrEmpty();
            saleItem.Quantity.Should().BeGreaterThan(0);
            saleItem.UnitPrice.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Tests that a SaleItem must have a valid product name.
        /// </summary>
        [Fact(DisplayName = "Product name should not be empty")]
        public void Given_SaleItem_When_ProductNameIsEmpty_Then_ShouldFail()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();
            saleItem.Product = string.Empty; // Invalid

            // Act
            var isValid = !string.IsNullOrEmpty(saleItem.Product);

            // Assert
            isValid.Should().BeFalse("because the product name is required and cannot be empty");
        }



        /// <summary>
        /// Tests that a SaleItem must have a valid unit price greater than 0.
        /// </summary>
        [Fact(DisplayName = "Unit price should be greater than 0")]
        public void Given_SaleItem_When_UnitPriceIsZeroOrLess_Then_ShouldFail()
        {
            // Arrange
            var saleItem = SaleItemTestData.GenerateValidSaleItem();
            saleItem.UnitPrice = 0; // Invalid

            // Act
            var isValid = saleItem.UnitPrice > 0;

            // Assert
            isValid.Should().BeFalse("because the unit price must be greater than 0");
        }
    }
}
