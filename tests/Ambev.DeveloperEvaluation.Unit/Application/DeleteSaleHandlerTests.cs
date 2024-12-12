using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class DeleteSaleHandlerTests
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<DeleteSaleHandler> _logger;
        private readonly DeleteSaleHandler _handler;

        public DeleteSaleHandlerTests()
        {
            _saleRepository = Substitute.For<IRepository<Sale>>();
            _logger = Substitute.For<ILogger<DeleteSaleHandler>>();
            _handler = new DeleteSaleHandler(_saleRepository, _logger);
        }

        [Fact(DisplayName = "Given valid DeleteSaleCommand When deleting sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var command = DeleteSaleHandlerTestData.GenerateValidCommand();

            _saleRepository.DeleteAsync(command.Id).Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            await _saleRepository.Received(1).DeleteAsync(command.Id);
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Sale successfully deleted with ID")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact(DisplayName = "Given invalid DeleteSaleCommand When deleting sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = new DeleteSaleCommand(); // Invalid command

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Validation failed for DeleteSaleCommand")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact(DisplayName = "Given non-existent Sale ID When deleting sale Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentSaleId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = DeleteSaleHandlerTestData.GenerateValidCommand();

            _saleRepository.DeleteAsync(command.Id).Returns(false);

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"*Sale with ID {command.Id} not found*");

            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains($"Sale with ID: {command.Id} not found")),
                null,
                Arg.Any<Func<object, Exception, string>>()
            );
        }
    }
}
