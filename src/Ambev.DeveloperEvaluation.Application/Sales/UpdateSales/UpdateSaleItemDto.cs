namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSales
{
    /// <summary>
    /// Represents a result object for a sale item in the updated sale.
    /// </summary>
    public class UpdateSaleItemDto
    {
        public Guid Id { get; set; }
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}