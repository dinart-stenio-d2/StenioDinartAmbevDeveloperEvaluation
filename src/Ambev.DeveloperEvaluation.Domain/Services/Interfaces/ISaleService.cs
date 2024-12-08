using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Services.Interfaces
{
    public interface ISaleService 
    {
        public ValidationResult ValidateSale(Sale sale);
        public ValidationResult ValidateUpdateSale(Sale existingSale, Sale newSalesData);
    }
}
