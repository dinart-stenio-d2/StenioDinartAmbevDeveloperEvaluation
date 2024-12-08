using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSales
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ILogger<DeleteSaleHandler> _logger;


        /// <summary>
        /// Initializes a new instance of DeleteSaleHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="logger">Logger instance</param>
        public DeleteSaleHandler(IRepository<Sale> saleRepository, ILogger<DeleteSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the DeleteSaleCommand request
        /// </summary>
        /// <param name="request">The DeleteSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the delete operation</returns>
        public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Validating DeleteSaleCommand for Sale ID: {SaleId}", request.Id);

            var validator = new DeleteSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for DeleteSaleCommand. Errors: {Errors}",
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            _logger.LogInformation("Attempting to delete Sale with ID: {SaleId}", request.Id);

            var sucess = await _saleRepository.DeleteAsync(request.Id);
           
            if (!sucess)
            {
                _logger.LogWarning("Sale with ID: {SaleId} not found", request.Id);
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
            }

            _logger.LogInformation("Sale successfully deleted with ID: {SaleId}", request.Id);

            return new DeleteSaleResponse { Success = true };
        }
    }
}
