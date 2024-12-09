namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the newly created sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number of the newly created sale.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total amount of the newly created sale.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the status of whether the sale is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public string Branch { get; set; }

        public List<CreateSaleItemDtoResponse> Items { get; set; } = new List<CreateSaleItemDtoResponse>();
    }
}