using Ambev.DeveloperEvaluation.Application.Sales.UpdateSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.TestCases.UpdateSale
{
    public class UpdateSaleHandlerTests : BaseHandlerTest
    {
        private readonly IMediator _mediator;

        public UpdateSaleHandlerTests(ServiceLocatorFixture fixture) : base(fixture)
        {
            _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task UpdateSale_ShouldUpdateSaleSuccessfully()
        {
            // Arrange
            var existingSaleId = Guid.NewGuid();

            // Seed an existing sale into the repository
            var existingSale = new Sale
            {
                Id = existingSaleId,
                SaleNumber = "SALE456",
                SaleDate = DateTime.UtcNow.AddDays(-1),
                Customer = "John Doe",
                Branch = "Main Office",
                TotalAmount = 100.00m,
                IsCancelled = false,
                Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    Product = "Product A",
                    Quantity = 2,
                    UnitPrice = 25.00m,
                    Discount = 0.00m,
                    TotalAmount = 50.00m
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
            // Create an update command
            var command = new UpdateSaleCommand
            {
                Id = existingSaleId,
                SaleNumber = "SALE456",
                SaleDate = DateTime.UtcNow,
                Customer = "John Doe",
                Branch = "Updated Branch",
                Items = new List<UpdateSaleItemDto>
            {
                new UpdateSaleItemDto
                {
                    Product = "Updated Product A",
                    Quantity = 3,
                    UnitPrice = 20.00m,
                    Discount = 0.00m,
                    TotalAmount = 60.00m
                }
            }
            };

            // Act
            var result = await _mediator.Send(command);

            
            // Assert
            result.Id.Should().Be(existingSaleId, "the updated sale should retain the original ID.");
            result.SaleNumber.Should().Be("SALE456", "The SaleNumber should be updated.");
            result.Customer.Should().Be("John Doe", "The Customer should be updated.");
            result.Branch.Should().Be("Updated Branch", "The Branch should be updated.");
            result.Items.Should().HaveCount(1, "There should be one item in the updated sale.");
            result.Items.First().Product.Should().Be("Updated Product A", "The item product name should be updated.");
            result.Items.First().Quantity.Should().Be(3, "The item quantity should be updated.");
            result.Items.First().UnitPrice.Should().Be(20.00m, "The item unit price should be updated.");
            result.Items.First().TotalAmount.Should().Be(60.00m, "The item total amount should reflect the updates.");
        }

        [Fact]
        public async Task UpdateSale_ShouldFailWhenValidationFails()
        {
            // Arrange
            var nonExistentSaleId = Guid.NewGuid(); // Simula um ID de venda inexistente

            // Cria um comando de atualização com dados inválidos
            var command = new UpdateSaleCommand
            {
                Id = nonExistentSaleId, // Venda inexistente
                SaleNumber = "", // SaleNumber inválido (vazio)
                SaleDate = default, // Data inválida
                Customer = "", // Cliente inválido (vazio)
                Branch = "", // Filial inválida (vazio)
                Items = new List<UpdateSaleItemDto>() // Lista de itens vazia (inválido)
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _mediator.Send(command));

          
            exception.Errors.Should().NotBeNullOrEmpty("Validation should fail for invalid input data.");
            var errorMessages = exception.Errors.Select(e => e.ErrorMessage).ToList();

            errorMessages.Should().NotBeEmpty();

        }
    }
}

