using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleByIdHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<GetSaleByIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetSaleByIdHandler(IRepository<Sale> saleRepository, ILogger<GetSaleByIdHandler> logger, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GetSaleResult> Handle(GetSaleCommand query, CancellationToken cancellationToken)
        {

            if (query.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(query.Id), "Sale ID cannot be null or empty.");
            }

            _logger.LogInformation("Fetching Sale with ID: {SaleId}", query.Id);

            var sale = await _saleRepository.GetByIdAsync(query.Id);
            if (sale == null)
            {
                _logger.LogWarning("Sale with ID: {SaleId} not found", query.Id);
                throw new KeyNotFoundException($"Sale with ID {query.Id} not found");
            }

            var result = _mapper.Map<GetSaleResult>(sale);

            _logger.LogInformation("Sale retrieved successfully for ID: {SaleId}", query.Id);

            return result;
        }
    }
}