using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSaleHandler> _logger;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<IRepository<Sale>>();
            _saleService = Substitute.For<ISaleService>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<CreateSaleHandler>>();
            _handler = new CreateSaleHandler(_saleRepository, _saleService, _logger, _mapper);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = Substitute.For<Sale>();
            var result = new CreateSaleResult { Id = Guid.NewGuid(), SaleNumber = "12345" };

            _mapper.Map<Sale>(command).Returns(sale);
            _mapper.Map<CreateSaleResult>(sale).Returns(result);
            _saleService.ValidateSale(sale).Returns(new ValidationResult());
            _saleRepository.AddAsync(sale).Returns(Task.CompletedTask);

            // Act
            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            createSaleResult.Should().NotBeNull();
            createSaleResult.Id.Should().Be(result.Id);
            createSaleResult.SaleNumber.Should().Be(result.SaleNumber);
            await _saleRepository.Received(1).AddAsync(sale);
        }

        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand(); // Invalid command

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then IDs are regenerated")]
        public async Task Handle_ValidRequest_RegeneratesIds()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = Substitute.For<Sale>();
            sale.Items = command.Items.ConvertAll(item => new SaleItem { Id = Guid.Empty });

            _mapper.Map<Sale>(command).Returns(sale);
            _saleService.ValidateSale(sale).Returns(new ValidationResult());
            _saleRepository.AddAsync(sale).Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            sale.Items.Should().OnlyContain(item => item.Id != Guid.Empty);
            await _saleRepository.Received(1).AddAsync(sale);
        }

        [Fact(DisplayName = "Given sale fails validation When creating sale Then logs warning")]
        public async Task Handle_ValidationFails_LogsWarning()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = Substitute.For<Sale>();

            _mapper.Map<Sale>(command).Returns(sale);
            _saleService.ValidateSale(sale).Returns(new ValidationResult
            {
                Errors = { new ValidationFailure("Customer", "Customer is invalid") }
            });

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("*Customer is invalid*");
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Validation failed for SaleNumber")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()
            );
        }
    }
}
