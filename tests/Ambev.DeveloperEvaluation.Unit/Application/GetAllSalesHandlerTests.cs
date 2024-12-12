using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class GetAllSalesHandlerTests
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<GetAllSalesHandler> _logger;
        private readonly IMapper _mapper;
        private readonly GetAllSalesHandler _handler;

        public GetAllSalesHandlerTests()
        {
            _saleRepository = Substitute.For<IRepository<Sale>>();
            _logger = Substitute.For<ILogger<GetAllSalesHandler>>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllSalesHandler(_saleRepository, _logger, _mapper);
        }

        [Fact(DisplayName = "Given sales exist when fetching all sales then returns list of sales")]
        public async Task Handle_SalesExist_ReturnsListOfSales()
        {
            // Arrange
            var mockSales = GetAllSalesHandlerTestData.GenerateMockSales(3);
            var expectedResults = GetAllSalesHandlerTestData.GenerateMockGetAllSaleResults(3);

            _saleRepository.GetAllAsync(Arg.Any<Expression<Func<Sale, object>>>()).Returns(mockSales);
            _mapper.Map<IEnumerable<GetAllSaleResult>>(mockSales).Returns(expectedResults);

            // Act
            var result = await _handler.Handle(GetAllSalesHandlerTestData.GenerateValidQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedResults);
            await _saleRepository.Received(1).GetAllAsync(Arg.Any<Expression<Func<Sale, object>>>());
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Fetching all sales")),
                null,
                Arg.Any<Func<object, Exception, string>>());
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains($"Successfully retrieved {mockSales.Count} sales")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact(DisplayName = "Given no sales exist when fetching all sales then returns empty list")]
        public async Task Handle_NoSalesExist_ReturnsEmptyList()
        {
            // Arrange
            var emptySales = new List<Sale>();

            _saleRepository.GetAllAsync(Arg.Any<Expression<Func<Sale, object>>>()).Returns(emptySales);
            _mapper.Map<IEnumerable<GetAllSaleResult>>(emptySales).Returns(Enumerable.Empty<GetAllSaleResult>());

            // Act
            var result = await _handler.Handle(GetAllSalesHandlerTestData.GenerateValidQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
            await _saleRepository.Received(1).GetAllAsync(Arg.Any<Expression<Func<Sale, object>>>());
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("No sales records found")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }
    }

}
