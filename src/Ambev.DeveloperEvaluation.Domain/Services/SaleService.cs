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
        private readonly IValidator<(Sale ExistingSale, Sale NewSalesData)> _saleUpdateValidator;
        private readonly IValidator<List<SaleItem>> _updateSaleItemsValidator;
        private readonly ILogger<SaleService> _logger;
        private readonly IRepository<SaleItem> _saleItemRepository;

        public SaleService(
        IValidator<Sale> saleValidator,
        IValidator<(Sale ExistingSale, Sale NewSalesData)> saleUpdateValidator,
        IValidator<List<SaleItem>> updateSaleItemsValidator,
        ILogger<SaleService> logger,
        IRepository<SaleItem> saleItemRepository)
        {
            _saleValidator = saleValidator ?? throw new ArgumentNullException(nameof(saleValidator));
            _saleUpdateValidator = saleUpdateValidator ?? throw new ArgumentNullException(nameof(saleUpdateValidator));
            _updateSaleItemsValidator = updateSaleItemsValidator ?? throw new ArgumentNullException(nameof(updateSaleItemsValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _saleItemRepository= saleItemRepository; 
        }

        public ValidationResult ValidateSale(Sale sale)
        {
            if (sale == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(sale)), "An error occurred while validating Sale.");
                throw new ArgumentNullException(nameof(sale));
            }

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
            if (existingSale == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(existingSale)), "An error occurred while validating Sale update: existingSale is null.");
                throw new ArgumentNullException(nameof(existingSale));
            }

            if (newSalesData == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(newSalesData)), "An error occurred while validating Sale update: newSalesData is null.");
                throw new ArgumentNullException(nameof(newSalesData));
            }

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
                if (newSalesData.Items.Any(newItem => existingSale.Items.Any(existingItem => existingItem.Id == newItem.Id)))
                {
                    // Item already exists logic
                    validationResult = _updateSaleItemsValidator.Validate(newSalesData.Items);
                }
                else
                {
                    foreach (var newItem in newSalesData.Items)
                    {
                        newItem.RegenerateId();
                        newItem.SaleId = existingSale.Id;
                    }
                    validationResult = _updateSaleItemsValidator.Validate(newSalesData.Items);
                }
                _logger.LogInformation("Validating SaleItems for Sale with ID: {SaleId}", newSalesData.Id);

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

                validationResult = _saleValidator.Validate(newSalesData);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for SaleItems in Sale with ID: {SaleId}. Errors: {Errors}",
                        newSalesData.Id, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                    return validationResult;
                }

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
