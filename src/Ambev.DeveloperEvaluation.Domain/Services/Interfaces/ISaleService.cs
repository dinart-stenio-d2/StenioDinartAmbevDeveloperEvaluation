using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services.Interfaces
{
    public interface ISaleService
    {
        void ApplySalesDiscountRules(Sale sale);
    }
}
