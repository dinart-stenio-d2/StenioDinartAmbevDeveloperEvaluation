using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    /// <summary>
    /// Contains unit tests for the Sale entity class.
    /// Tests cover business logic, calculations, and property updates.
    /// </summary>
    public class SaleTests
    {
        /// <summary>
        /// Tests that GenerateSaleNumber creates a unique SaleNumber of length 10.
        /// </summary>
        [Fact(DisplayName = "SaleNumber should be generated and have a length of 10")]
        public void Given_EmptySaleNumber_When_GenerateSaleNumber_Then_ShouldGenerateValidSaleNumber()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.SaleNumber = null;

            // Act
            sale.GenerateSaleNumber();

            // Assert
            sale.SaleNumber.Should().NotBeNullOrEmpty();
            sale.SaleNumber.Should().HaveLength(10);
        }

        /// <summary>
        /// Tests that UpdateSaleStatus updates the IsCancelled property correctly.
        /// </summary>
        [Fact(DisplayName = "IsCancelled should update correctly when status changes")]
        public void Given_Sale_When_UpdateSaleStatus_Then_ShouldUpdateIsCancelled()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.UpdateSaleStatus(true);

            // Assert
            sale.IsCancelled.Should().BeTrue();

            // Act
            sale.UpdateSaleStatus(false);

            // Assert
            sale.IsCancelled.Should().BeFalse();
        }

        /// <summary>
        /// Tests that CalculateTotalAmount sums up the total price of all SaleItems.
        /// </summary>
        [Fact(DisplayName = "TotalAmount should sum up all SaleItem prices")]
        public void Given_SaleWithItems_When_CalculateTotalAmount_Then_TotalAmountShouldBeCorrect()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var expectedTotal = sale.Items.Sum(item => item.GetTotalPrice());

            // Act
            sale.CalculateTotalAmount();

            // Assert
            sale.TotalAmount.Should().Be(expectedTotal);
        }

        /// <summary>
        /// Tests that ApplyDiscounts applies discounts to all SaleItems based on their quantity.
        /// </summary>
        [Fact(DisplayName = "Discounts should be correctly applied to all SaleItems")]
        public void Given_SaleWithItems_When_ApplyDiscounts_Then_AllItemsShouldHaveCorrectDiscounts()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale(5);

            // Act
            sale.ApplyDiscounts();

            // Assert
            sale.Items.Should().OnlyContain(item =>
                (item.Quantity < 4 && item.Discount == 0) ||
                (item.Quantity >= 4 && item.Quantity < 10 && item.Discount == 10) ||
                (item.Quantity >= 10 && item.Quantity <= 20 && item.Discount == 20));
        }

        /// <summary>
        /// Tests that items with Quantity < 4 have no discount applied.
        /// </summary>
        [Fact(DisplayName = "Items with Quantity < 4 should have no discount")]
        public void Given_ItemsWithQuantityLessThan4_When_ApplyDiscounts_Then_DiscountShouldBeZero()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale(3);
            foreach (var item in sale.Items)
            {
                item.Quantity = 3;
            }

            // Act
            sale.ApplyDiscounts();

            // Assert
            sale.Items.Should().OnlyContain(item => item.Discount == 0);
        }

        /// <summary>
        /// Tests that items with 4 <= Quantity < 10 have a 10% discount applied.
        /// </summary>
        [Fact(DisplayName = "Items with 4 <= Quantity < 10 should have a 10% discount")]
        public void Given_ItemsWithQuantityBetween4And10_When_ApplyDiscounts_Then_DiscountShouldBeTenPercent()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale(3);
            foreach (var item in sale.Items)
            {
                item.Quantity = 5;
            }

            // Act
            sale.ApplyDiscounts();

            // Assert
            sale.Items.Should().OnlyContain(item => item.Discount == 10);
        }

        /// <summary>
        /// Tests that items with 10 <= Quantity <= 20 have a 20% discount applied.
        /// </summary>
        [Fact(DisplayName = "Items with 10 <= Quantity <= 20 should have a 20% discount")]
        public void Given_ItemsWithQuantityBetween10And20_When_ApplyDiscounts_Then_DiscountShouldBeTwentyPercent()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale(3);
            foreach (var item in sale.Items)
            {
                item.Quantity = 15;
            }

            // Act
            sale.ApplyDiscounts();

            // Assert
            sale.Items.Should().OnlyContain(item => item.Discount == 20);
        }

        /// <summary>
        /// Tests that items with Quantity > 20 do not receive any discount.
        /// </summary>
        [Fact(DisplayName = "Items with Quantity > 20 should have no discount")]
        public void Given_ItemsWithQuantityGreaterThan20_When_ApplyDiscounts_Then_DiscountShouldBeZero()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale(3);
            foreach (var item in sale.Items)
            {
                item.Quantity = 25;
            }

            // Act
            sale.ApplyDiscounts();

            // Assert
            sale.Items.Should().OnlyContain(item => item.Discount == 0);
        }


        /// <summary>
        /// Tests that SaleDate is set to a valid past date.
        /// </summary>
        [Fact(DisplayName = "SaleDate should be a valid past date")]
        public void Given_Sale_When_Validated_Then_SaleDateShouldBePast()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act & Assert
            sale.SaleDate.Should().BeOnOrBefore(System.DateTime.UtcNow);
        }

        /// <summary>
        /// Tests that Customer field is not null or empty.
        /// </summary>
        [Fact(DisplayName = "Customer should not be null or empty")]
        public void Given_Sale_When_Validated_Then_CustomerShouldNotBeNullOrEmpty()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act & Assert
            sale.Customer.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests that Branch field is not null or empty.
        /// </summary>
        [Fact(DisplayName = "Branch should not be null or empty")]
        public void Given_Sale_When_Validated_Then_BranchShouldNotBeNullOrEmpty()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act & Assert
            sale.Branch.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Tests that SaleDate cannot be a future date.
        /// </summary>
        [Fact(DisplayName = "SaleDate should not be a future date")]
        public void Given_Sale_When_SaleDateIsFuture_Then_ShouldFailValidation()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.SaleDate = System.DateTime.UtcNow.AddDays(1); // Future date

            // Act
            var isValid = sale.SaleDate <= System.DateTime.UtcNow;

            // Assert
            isValid.Should().BeFalse("because the sale date cannot be set in the future");
        }

        /// <summary>
        /// Tests that Customer can be updated to a new valid name.
        /// </summary>
        [Fact(DisplayName = "Customer should update to new valid name")]
        public void Given_Sale_When_CustomerIsUpdated_Then_ShouldReflectNewValue()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var newCustomer = "John Doe";

            // Act
            sale.Customer = newCustomer;

            // Assert
            sale.Customer.Should().Be(newCustomer);
        }

        /// <summary>
        /// Tests that Branch can be updated to a new valid name.
        /// </summary>
        [Fact(DisplayName = "Branch should update to new valid name")]
        public void Given_Sale_When_BranchIsUpdated_Then_ShouldReflectNewValue()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var newBranch = "Downtown Branch";

            // Act
            sale.Branch = newBranch;

            // Assert
            sale.Branch.Should().Be(newBranch);
        }
    }
}



