using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
        public Guid SaleId { get; set; } 
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public Sale Sale { get; set; }
        public decimal GetTotalPrice()
        {
            var subtotal = UnitPrice * Quantity;
            return subtotal - (subtotal * Discount / 100);
        }

        /// <summary>
        /// Applies the discount based on the quantity
        /// </summary>
        public void ApplyDiscount()
        {
            if (Quantity < 4)
            {
                Discount = 0;
            }
            else if (Quantity >= 4 && Quantity < 10)
            {
                Discount = 10;
            }
            else if (Quantity >= 10 && Quantity <= 20)
            {
                Discount = 20;
            }
        }

        /// <summary>
        /// Regenerates the ID for the SaleItem.
        /// </summary>
        public void RegenerateId()
        {
            GenerateNewId();
        }
    }
}
