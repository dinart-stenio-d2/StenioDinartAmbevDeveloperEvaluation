using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.TestCases.CreateSale
{
    public class CreateSaleHandlerTests : BaseHandlerTest
    {

        private readonly IMediator _mediator;

        public CreateSaleHandlerTests(ServiceLocatorFixture fixture) : base(fixture)
        {
            _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task CreateSale_ShouldCreateSaleSuccessfully()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleDate = DateTime.UtcNow, // SaleDate is required
                Customer = "John Doe", // Customer is required and cannot exceed 100 characters
                Branch = "Main Office", // Branch is required and cannot exceed 50 characters
                Items = new List<SaleItemDto> // At least one sale item is required
            {
                new SaleItemDto
                {
                    Product = "Product A", // Product is required and cannot exceed 100 characters
                    Quantity = 5, // Quantity must be greater than 0
                    UnitPrice = 10.50m, // Unit price must be greater than or equal to 0, with 18 total digits and 2 decimal places
                    Discount = 5.00m, // Discount must be greater than or equal to 0, with 18 total digits and 2 decimal places
                    TotalAmount = 47.50m // Total amount must be greater than or equal to 0, with 18 total digits and 2 decimal places
                },
                new SaleItemDto
                {
                    Product = "Product B",
                    Quantity = 2,
                    UnitPrice = 20.00m,
                    Discount = 0.00m,
                    TotalAmount = 40.00m
                }
             }
            };

                // Act
                var result = await _mediator.Send(command);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().NotBe(Guid.Empty, "the ID should not be the default GUID (Guid.Empty).");
                result.SaleNumber.Should().NotBeNullOrEmpty("SaleNumber should not be null or empty.");
                result.SaleNumber.Length.Should().BeLessOrEqualTo(50, "SaleNumber cannot exceed 50 characters.");
                result.SaleDate.Should().NotBe(default, "SaleDate is required and cannot be the default value.");
                result.Customer.Should().NotBeNullOrEmpty("Customer name should not be null or empty.");
                result.Customer.Length.Should().BeLessOrEqualTo(100, "Customer name cannot exceed 100 characters.");
                result.TotalAmount.Should().BeGreaterOrEqualTo(0, "TotalAmount must be greater than or equal to 0.")
                    .And.BeLessThanOrEqualTo(decimal.MaxValue, "TotalAmount must fit within a valid range.");
                result.Items.Should().NotBeNullOrEmpty("A Sale must have at least one item.");
        }


        [Fact]
        public async Task CreateSale_ShouldFailWhenValidationFails()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleDate = default, // Invalid SaleDate (not set)
                Customer = "", // Invalid Customer (empty string)
                Branch = "", // Invalid Branch (empty string)
                Items = new List<SaleItemDto>() // Invalid items (empty list)
            };

            // Act
            Func<Task> action = async () => await _mediator.Send(command);

            // Assert
            var exception = await action.Should().ThrowAsync<FluentValidation.ValidationException>();
            exception.Which.Errors.Should().NotBeNullOrEmpty("Validation should detect issues with the input data.");

            var errors = exception.Which.Errors.Select(e => e.ErrorMessage);

            // Validate specific error messages
            errors.Should().Contain("SaleDate is required.", "Validation should fail for an empty SaleDate.");
            errors.Should().Contain("Customer is required.", "Validation should fail for an empty Customer.");
            errors.Should().Contain("Branch is required.", "Validation should fail for an empty Branch.");
            errors.Should().Contain("At least one sale item is required.", "Validation should fail for missing sale items.");
        }
    }
}
