using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services
{
    public class SaleServiceTests
    {
        private readonly IValidator<Sale> _saleValidator;
        private readonly IValidator<(Sale, Sale)> _saleUpdateValidator;
        private readonly IValidator<List<SaleItem>> _updateSaleItemsValidator;
        private readonly ILogger<SaleService> _logger;
        private readonly IRepository<SaleItem> _saleItemRepository;

        public SaleServiceTests()
        {
            _saleValidator = Substitute.For<IValidator<Sale>>();
            _saleUpdateValidator = Substitute.For<IValidator<(Sale, Sale)>>();
            _updateSaleItemsValidator = Substitute.For<IValidator<List<SaleItem>>>();
            _logger = Substitute.For<ILogger<SaleService>>();
            _saleItemRepository = Substitute.For<IRepository<SaleItem>>();
        }

        [Fact(DisplayName = "Validate Sale should return valid result when Sale is valid")]
        public void Given_ValidSale_When_ValidateSale_Then_ShouldReturnValidResult()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = "Valid Customer",
                Branch = "Branch A",
                Items = new List<SaleItem>
                {
                    new SaleItem { Id = Guid.NewGuid(), Product = "Product A", Quantity = 5, UnitPrice = 10 }
                }
            };

            _saleValidator.Validate(sale).Returns(new ValidationResult());

            var service = new SaleService(_saleValidator, _saleUpdateValidator, _updateSaleItemsValidator, _logger, _saleItemRepository);

            // Act
            var result = service.ValidateSale(sale);

            // Assert
            result.IsValid.Should().BeTrue();
            _logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains($"Starting validation for Sale with ID: {sale.Id}")),
                null,
                Arg.Any<Func<object, Exception, string>>()
            );

            _logger.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains($"Validation succeeded for Sale with ID: {sale.Id}")),
                null,
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact(DisplayName = "ValidateSale should log warnings when Sale is invalid")]
        public void Given_InvalidSale_When_ValidateSale_Then_ShouldLogWarnings()
        {
            // Arrange
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = DateTime.UtcNow,
                Customer = string.Empty, // Invalid
                Branch = "Branch A",
                Items = new List<SaleItem>
                {
                    new SaleItem { Id = Guid.NewGuid(), Product = "Product A", Quantity = 5, UnitPrice = 10 }
                }
            };

            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("Customer", "Customer name is required."));

            _saleValidator.Validate(sale).Returns(validationResult);

            var service = new SaleService(_saleValidator, _saleUpdateValidator, _updateSaleItemsValidator, _logger, _saleItemRepository);

            // Act
            var result = service.ValidateSale(sale);

            // Assert
            result.IsValid.Should().BeFalse();
            _logger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains($"Validation failed for Sale with ID: {sale.Id}")),
                null,
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact(DisplayName = "ValidateSale should throw exception if Sale is null")]
        public void Given_NullSale_When_ValidateSale_Then_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = new SaleService(_saleValidator, _saleUpdateValidator, _updateSaleItemsValidator, _logger, _saleItemRepository);

            // Act
            Action act = () => service.ValidateSale(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*sale*");

            _logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("An error occurred while validating Sale.")),
                Arg.Any<ArgumentNullException>(),
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact(DisplayName = "ValidateUpdateSale should throw exception if existingSale is null")]
        public void Given_NullExistingSale_When_ValidateUpdateSale_Then_ShouldThrowArgumentNullException()
        {
            // Arrange
            var newSale = new Sale { Id = Guid.NewGuid() };

            var service = new SaleService(_saleValidator, _saleUpdateValidator, _updateSaleItemsValidator, _logger, _saleItemRepository);

            // Act
            Action act = () => service.ValidateUpdateSale(null, newSale);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*existingSale*");

            _logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("An error occurred while validating Sale update: existingSale is null.")),
                Arg.Any<ArgumentNullException>(),
                Arg.Any<Func<object, Exception, string>>()
            );
        }
    }
}
