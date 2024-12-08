namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesResult
    {
        /// <summary>
        /// Gets or sets the list of sales.
        /// </summary>
        public List<GetAllSaleResult> Sales { get; set; } = new List<GetAllSaleResult>();
    }
}