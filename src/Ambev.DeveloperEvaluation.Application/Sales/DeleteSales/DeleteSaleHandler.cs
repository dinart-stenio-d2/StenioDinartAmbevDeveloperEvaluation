using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSales
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<DeleteSaleHandler> _logger;

        public DeleteSaleHandler(IRepository<Sale> saleRepository, ILogger<DeleteSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting Sale with ID: {SaleId}", command.Id);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale with ID: {SaleId} not found", command.Id);
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
            }

            await _saleRepository.DeleteAsync(command.Id, cancellationToken);

            _logger.LogInformation("Sale successfully deleted with ID: {SaleId}", command.Id);

            return true;
        }
    }
}
