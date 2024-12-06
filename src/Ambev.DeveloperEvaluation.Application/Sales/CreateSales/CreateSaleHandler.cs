using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResult>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ISaleService _saleService;
        private readonly ILogger<CreateSaleHandler> _logger;

        public CreateSaleHandler(IRepository<Sale> saleRepository, ISaleService saleService, ILogger<CreateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _logger = logger;
        }

        public async Task<SaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting CreateSaleHandler for SaleNumber: {SaleNumber}", command.SaleNumber);

            var validationResult = _saleService.ValidateSale(command.ToSaleEntity());
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for SaleNumber: {SaleNumber}", command.SaleNumber);
                throw new ValidationException(validationResult.Errors);
            }

            var sale = command.ToSaleEntity();
            await _saleRepository.AddAsync(sale, cancellationToken);

            _logger.LogInformation("Sale successfully created with SaleNumber: {SaleNumber}", sale.SaleNumber);

            return sale.ToSaleResult();
        }
    }
}
