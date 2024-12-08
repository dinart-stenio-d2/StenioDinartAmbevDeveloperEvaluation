namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleItemDto
    {
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }
}