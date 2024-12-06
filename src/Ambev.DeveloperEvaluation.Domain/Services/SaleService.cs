using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SaleService : ISaleService
    {
        public void ApplySalesDiscountRules(Sale sale)
        {
            foreach (var item in sale.Items)
            {
                if (item.Quantity < 4)
                {
                    item.Discount = 0;
                }
                else if (item.Quantity >= 4 && item.Quantity < 10)
                {
                    item.Discount = 10;
                }
                else if (item.Quantity >= 10 && item.Quantity <= 20)
                {
                    item.Discount = 20;
                }
                else
                {
                    //TODO : Apply fluent validations rules here 
                    throw new InvalidOperationException("Cannot sell more than 20 items of the same product.");
                }
            }
            sale.CalculateTotalAmount();
        }
    }
}
