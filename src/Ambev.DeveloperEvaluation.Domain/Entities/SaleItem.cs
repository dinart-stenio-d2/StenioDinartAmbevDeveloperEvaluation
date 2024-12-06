using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem :BaseEntity
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
    }
}
