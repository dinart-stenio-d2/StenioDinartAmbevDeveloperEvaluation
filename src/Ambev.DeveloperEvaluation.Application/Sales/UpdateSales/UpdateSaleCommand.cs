using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSales
{
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to be updated.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of the sale.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the branch where the sale was made.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets whether the sale is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the list of sale items.
        /// </summary>
        public List<UpdateSaleItemDto> Items { get; set; } = new List<UpdateSaleItemDto>();

        
        /// <summary>
        /// Validates the current UpdateSaleCommand instance.
        /// </summary>
        /// <returns>A ValidationResultDetail object containing validation results.</returns>
        //public ValidationResultDetail Validate()
        //{
        //    var validator = new UpdateSaleCommandValidator();
        //    var result = validator.Validate(this);
        //    return new ValidationResultDetail
        //    {
        //        IsValid = result.IsValid,
        //        Errors = result.Errors.Select(e => new ValidationErrorDetail
        //        {
        //            PropertyName = e.PropertyName,
        //            ErrorMessage = e.ErrorMessage
        //        })
        //    };
        //}
    }
}
