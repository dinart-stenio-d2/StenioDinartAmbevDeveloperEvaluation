using Ambev.DeveloperEvaluation.Common.SharedKernel;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity, IAggregateRoot
    {
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

        public void CalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var item in Items)
            {
                TotalAmount += item.GetTotalPrice();
            }
        }
    }
}
