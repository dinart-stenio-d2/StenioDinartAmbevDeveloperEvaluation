using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSales
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, SaleResult>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ISaleService _saleService;
        private readonly ILogger<UpdateSaleHandler> _logger;

        public UpdateSaleHandler(IRepository<Sale> saleRepository, ISaleService saleService, ILogger<UpdateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _logger = logger;
        }

        public async Task<SaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating Sale with ID: {SaleId}", command.Id);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale with ID: {SaleId} not found", command.Id);
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
            }

            var updatedSale = command.ToSaleEntity();
            var validationResult = _saleService.ValidateSale(updatedSale);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for Sale with ID: {SaleId}", command.Id);
                throw new ValidationException(validationResult.Errors);
            }

            await _saleRepository.UpdateAsync(updatedSale, cancellationToken);

            _logger.LogInformation("Sale successfully updated with ID: {SaleId}", updatedSale.Id);

            return updatedSale.ToSaleResult();
        }
    }

}
