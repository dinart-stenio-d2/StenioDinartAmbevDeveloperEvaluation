using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.TestCases.GetAllSales
{
    public class GetAllSalesHandlerTests : BaseHandlerTest
    {
        private readonly IMediator _mediator;

        public GetAllSalesHandlerTests(ServiceLocatorFixture fixture) : base(fixture)
        {
            _mediator = _fixture.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task HandleGetAllSales_ShouldReturnAllSalesSuccessfully()
        {
        
            // Create a query to fetch all sales
            var query = new GetAllSalesQuery();

            // Act
            var result = await _mediator.Send(query);

            // Assert
            result.Should().NotBeNullOrEmpty("there are sales in the repository.");
           
        }
    }
}
