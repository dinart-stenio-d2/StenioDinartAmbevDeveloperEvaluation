using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly IValidator<Sale> _saleValidator;
        private readonly ILogger<SaleService> _logger;

        public SaleService(IValidator<Sale> saleValidator, ILogger<SaleService> logger)
        {
            _saleValidator = saleValidator;
            _logger = logger;
        }

        public ValidationResult ValidateSale(Sale sale)
        {
            _logger.LogInformation("Starting validation for Sale with ID: {SaleId}", sale.Id);

            var validationResult = _saleValidator.Validate(sale);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for Sale with ID: {SaleId}. Errors: {Errors}",
                    sale.Id, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult;
            }

            _logger.LogInformation("Validation succeeded for Sale with ID: {SaleId}. Applying discounts and calculating total amount.", sale.Id);

            try
            {
                sale.ApplyDiscounts();
                sale.CalculateTotalAmount();
                _logger.LogInformation("Successfully applied discounts and calculated total amount for Sale with ID: {SaleId}", sale.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying discounts or calculating total amount for Sale with ID: {SaleId}", sale.Id);
                throw;
            }

            return validationResult;
        }
    }
}
