using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly IValidator<Sale> _saleValidator;
        private readonly IValidator<(Sale ExistingSale, Sale NewSalesData)> _saleUpdateValidator;
        private readonly IValidator<List<SaleItem>> _updateSaleItemsValidator;
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

        public ValidationResult ValidateUpdateSale(Sale existingSale, Sale newSalesData)
        {
            _logger.LogInformation("Starting validation for Sale update with ID: {SaleId}", newSalesData.Id);


            var validationResult = _saleUpdateValidator.Validate((existingSale, newSalesData));

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for Sale with ID: {SaleId}. Errors: {Errors}",
                    newSalesData.Id, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return validationResult;
            }

            _logger.LogInformation("Sale base fields validated successfully for ID: {SaleId}", newSalesData.Id);


            if (newSalesData.Items.Any())
            {
                _logger.LogInformation("Validating SaleItems for Sale with ID: {SaleId}", newSalesData.Id);

                validationResult = _updateSaleItemsValidator.Validate(newSalesData.Items);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for SaleItems in Sale with ID: {SaleId}. Errors: {Errors}",
                        newSalesData.Id, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                    return validationResult;
                }

                _logger.LogInformation("SaleItems validated successfully for Sale with ID: {SaleId}", newSalesData.Id);
            }
            else
            {
                _logger.LogWarning("No SaleItems provided for Sale with ID: {SaleId}", newSalesData.Id);
            }

            try
            {
                _logger.LogInformation("Applying discounts and calculating total amount for Sale with ID: {SaleId}", newSalesData.Id);

                newSalesData.ApplyDiscounts();
                newSalesData.CalculateTotalAmount();

                _logger.LogInformation("Discounts applied and total amount calculated successfully for Sale with ID: {SaleId}", newSalesData.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying discounts or calculating total amount for Sale with ID: {SaleId}", newSalesData.Id);
                throw;
            }

            _logger.LogInformation("Validation and processing completed successfully for Sale with ID: {SaleId}", newSalesData.Id);

            return validationResult;
        }
    }
}
