using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, IEnumerable<SaleResult>>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<GetAllSalesHandler> _logger;

        public GetAllSalesHandler(IRepository<Sale> saleRepository, ILogger<GetAllSalesHandler> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<SaleResult>> Handle(GetAllSalesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all sales");

            var sales = await _saleRepository.GetAllAsync(cancellationToken);

            _logger.LogInformation("Successfully retrieved {Count} sales", sales.Count());

            return sales.Select(s => s.ToSaleResult());
        }
    }
}
