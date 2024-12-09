using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.TestCases.GeSale
{
    public class GetSaleHandlerTests : BaseHandlerTest
    {
        private readonly IMediator _mediator;

        public GetSaleHandlerTests(ServiceLocatorFixture fixture) : base(fixture)
        {
            _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
        }
        [Fact]
        public async Task GetSale_ShouldReturnSaleSuccessfully()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            // Seed an existing sale into the repository
            var existingSale = new Sale
            {
                Id = saleId,
                SaleNumber = "SALE123",
                SaleDate = DateTime.UtcNow.AddDays(-1),
                Customer = "Jane Doe",
                Branch = "Main Office",
                TotalAmount = 150.00m,
                IsCancelled = false,
                Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    Product = "Product A",
                    Quantity = 2,
                    UnitPrice = 50.00m,
                    Discount = 0.00m,
                    TotalAmount = 100.00m
                },
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    Product = "Product B",
                    Quantity = 1,
                    UnitPrice = 50.00m,
                    Discount = 0.00m,
                    TotalAmount = 50.00m
                }
            }
            };

            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();
                await repository.AddAsync(existingSale);
            }

            // Create a query to fetch the sale by ID
            var query = new GetSaleCommand(saleId);

            // Act
            var result = await _mediator.Send(query);

            // Assert
            result.Should().NotBeNull("the handler should return a sale object.");
            result.Id.Should().Be(saleId, "the returned sale should have the same ID as the queried ID.");
            result.SaleNumber.Should().Be("SALE123", "the sale number should match the seeded sale.");
            result.Customer.Should().Be("Jane Doe", "the customer name should match the seeded sale.");
            result.Branch.Should().Be("Main Office", "the branch should match the seeded sale.");
            result.TotalAmount.Should().Be(150.00m, "the total amount should match the seeded sale.");
        }

        [Fact]
        public async Task GetSale_ShouldFailWhenSaleDoesNotExist()
        {
            // Arrange
            var nonExistentSaleId = Guid.NewGuid(); // Generate a random ID that does not exist in the repository

            // Create a query to fetch the sale by a non-existent ID
            var query = new GetSaleCommand(nonExistentSaleId);

            // Act
            Func<Task> action = async () => await _mediator.Send(query);

            // Assert
            await action.Should()
                .ThrowAsync<KeyNotFoundException>("the sale does not exist in the repository and should throw an exception.")
                .WithMessage($"Sale with ID {nonExistentSaleId} not found");
        }
    }
}
