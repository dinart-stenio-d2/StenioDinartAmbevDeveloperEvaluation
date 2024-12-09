using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.TestCases.DeleteSale
{
    public class DeleteSaleHandlerTests : BaseHandlerTest
    {
        private readonly IMediator _mediator;

        public DeleteSaleHandlerTests(ServiceLocatorFixture fixture) : base(fixture)
        {
            _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task HandleDeleteSale_ShouldDeleteSaleSuccessfully()
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
                }
            }
            };

            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();
                await repository.AddAsync(existingSale);
            }

            // Create a delete command
            var command = new DeleteSaleCommand { Id = saleId };

            // Act
            var result = await _mediator.Send(command);

            // Assert
            result.Should().NotBeNull("the handler should return a response.");
            result.Success.Should().BeTrue("the sale should be deleted successfully.");

            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();
                var sale = await repository.GetByIdAsync(saleId);
                sale.Should().BeNull("the sale should no longer exist in the repository.");
            }
        }

        [Fact]
        public async Task HandleDeleteSale_ShouldFailWhenSaleDoesNotExist()
        {
            // Arrange
            var nonExistentSaleId = Guid.NewGuid();

            // Create a delete command for a non-existent sale
            var command = new DeleteSaleCommand { Id = nonExistentSaleId };

            // Act
            Func<Task> action = async () => await _mediator.Send(command);

            // Assert
            await action.Should()
                .ThrowAsync<KeyNotFoundException>("the sale does not exist in the repository and should throw an exception.")
                .WithMessage($"Sale with ID {nonExistentSaleId} not found");
        }
    }
}
