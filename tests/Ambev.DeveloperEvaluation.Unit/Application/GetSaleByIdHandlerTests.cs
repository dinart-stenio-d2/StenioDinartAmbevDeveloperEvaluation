using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetSaleByIdHandlerTests
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<GetSaleByIdHandler> _logger;
        private readonly IMapper _mapper;
        private readonly GetSaleByIdHandler _handler;

        public GetSaleByIdHandlerTests()
        {
            _saleRepository = Substitute.For<IRepository<Sale>>();
            _logger = Substitute.For<ILogger<GetSaleByIdHandler>>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleByIdHandler(_saleRepository, _logger, _mapper);
        }

        [Fact(DisplayName = "Given valid Sale ID When fetching sale Then returns sale result")]
        public async Task Handle_ValidSaleId_ReturnsSaleResult()
        {
            // Arrange
            var command = GetSaleByIdHandlerTestData.GenerateValidCommand();
            var sale = GetSaleByIdHandlerTestData.GenerateMockExistingSale();
            var expectedResult = GetSaleByIdHandlerTestData.GenerateMockGetSaleResult();

            _saleRepository.GetByIdAsync(command.Id).Returns(sale);
            _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Fetching Sale with ID")),
                null,
                Arg.Any<Func<object, Exception, string>>());
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Sale retrieved successfully for ID")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact(DisplayName = "Given non-existent Sale ID When fetching sale Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentSaleId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = GetSaleByIdHandlerTestData.GenerateValidCommand();

            _saleRepository.GetByIdAsync(command.Id).Returns((Sale)null);

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"*Sale with ID {command.Id} not found*");
            await _saleRepository.Received(1).GetByIdAsync(command.Id);
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains($"Sale with ID: {command.Id} not found")),
                null,
                Arg.Any<Func<object, Exception, string>>()
            );
        }

        [Fact(DisplayName = "Given null Sale ID When fetching sale Then throws ArgumentNullException")]
        public async Task Handle_NullSaleId_ThrowsArgumentNullException()
        {
            // Arrange
            var command = new GetSaleCommand(Guid.Empty);

            // Act
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
            _logger.Received(0).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }
    }
}
