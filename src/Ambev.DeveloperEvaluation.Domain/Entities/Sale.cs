using Ambev.DeveloperEvaluation.Common.SharedKernel;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity, IAggregateRoot
    {
        public string SaleNumber { get; set; } 
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public string Branch { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();


        /// <summary>
        /// Generates a random SaleNumber.
        /// </summary>
        public void GenerateSaleNumber()
        {
            if (string.IsNullOrEmpty(SaleNumber))
            {
                string uniqueBase = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}{Guid.NewGuid().ToString("N")}";
                SaleNumber = uniqueBase.Substring(0, 10);
            }
        }

        /// <summary>
        /// Calculate the TotalAmount to all items in the sale
        /// </summary>
        public void CalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var item in Items)
            {
                TotalAmount += item.GetTotalPrice();
            }
        }

        /// <summary>
        /// Applies discounts to all items in the sale
        /// </summary>
        public void ApplyDiscounts()
        {
            foreach (var item in Items)
            {
                item.ApplyDiscount();
            }
        }
    }
}
