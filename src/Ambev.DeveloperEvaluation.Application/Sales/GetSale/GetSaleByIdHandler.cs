using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleByIdHandler : IRequestHandler<GetSaleByIdQuery, SaleResult>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<GetSaleByIdHandler> _logger;

        public GetSaleByIdHandler(IRepository<Sale> saleRepository, ILogger<GetSaleByIdHandler> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<SaleResult> Handle(GetSaleByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching Sale with ID: {SaleId}", query.Id);

            var sale = await _saleRepository.GetByIdAsync(query.Id, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale with ID: {SaleId} not found", query.Id);
                throw new KeyNotFoundException($"Sale with ID {query.Id} not found");
            }

            _logger.LogInformation("Sale retrieved successfully for ID: {SaleId}", query.Id);

            return sale.ToSaleResult();
        }
    }
