using Ambev.DeveloperEvaluation.Application.Sales.UpdateSales;
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
    public class UpdateSaleHandlerTests
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleHandler> _logger;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<IRepository<Sale>>();
            _saleService = Substitute.For<ISaleService>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
            _handler = new UpdateSaleHandler(_saleRepository, _saleService, _logger, _mapper);
        }

        [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();
            var existingSale = UpdateSaleHandlerTestData.GenerateMockExistingSale();
            var updatedSale = Substitute.For<Sale>();
            var result = new UpdateSaleResult { Id = command.Id };

            _saleRepository.GetByIdAsync(command.Id).Returns(existingSale);
            _mapper.Map<Sale>(command).Returns(updatedSale);
            _saleService.ValidateUpdateSale(existingSale, updatedSale).Returns(new ValidationResult());
            _saleRepository.UpdateAsync(updatedSale).Returns(Task.CompletedTask);
            _mapper.Map<UpdateSaleResult>(updatedSale).Returns(result);

            // Act
            var updateSaleResult = await _handler.Handle(command, CancellationToken.None);

            // Assert
            updateSaleResult.Should().NotBeNull();
            updateSaleResult.Id.Should().Be(result.Id);
            await _saleRepository.Received(1).UpdateAsync(updatedSale);
        }

        [Fact(DisplayName = "Given invalid sale data When updating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = new UpdateSaleCommand(); // Invalid command

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given non-existent sale ID When updating sale Then throws validation exception")]
        public async Task Handle_NonExistentSaleId_ThrowsValidationException()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();

            _saleRepository.GetByIdAsync(command.Id).Returns((Sale)null);

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("*not found*");
        }

        [Fact(DisplayName = "Given validation fails during sale update When updating sale Then logs warning")]
        public async Task Handle_ValidationFails_LogsWarning()
        {
            // Arrange
            var command = UpdateSaleHandlerTestData.GenerateValidCommand();
            var existingSale = UpdateSaleHandlerTestData.GenerateMockExistingSale();
            var updatedSale = Substitute.For<Sale>();

            _saleRepository.GetByIdAsync(command.Id).Returns(existingSale);
            _mapper.Map<Sale>(command).Returns(updatedSale);
            _saleService.ValidateUpdateSale(existingSale, updatedSale).Returns(new ValidationResult
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
                Arg.Is<object>(o => o.ToString().Contains("Validation failed for Sale with ID")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()
            );
        }
    }
}
