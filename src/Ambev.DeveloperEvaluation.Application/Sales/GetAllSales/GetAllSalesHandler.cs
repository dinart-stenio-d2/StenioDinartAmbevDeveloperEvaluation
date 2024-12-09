using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, IEnumerable<GetAllSaleResult>>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<GetAllSalesHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllSalesHandler(IRepository<Sale> saleRepository, ILogger<GetAllSalesHandler> logger , IMapper mapper)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _mapper = mapper;   
        }

        public async Task<IEnumerable<GetAllSaleResult>> Handle(GetAllSalesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all sales");

            var sales = await _saleRepository.GetAllAsync(
                sale => sale.Items 
            );
           
            if (!sales.Any())
            {
                _logger.LogWarning("No sales records found");
                return Enumerable.Empty<GetAllSaleResult>();
            }

            var result = _mapper.Map<IEnumerable<GetAllSaleResult>>(sales);
            _logger.LogInformation("Successfully retrieved {Count} sales", sales.Count());
            return result;
            
        }
    }
}
