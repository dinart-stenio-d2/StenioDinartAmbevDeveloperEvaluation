using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSales
{
    public class DeleteSaleCommand : IRequest<DeleteSaleResponse>
    {
        public Guid Id { get; set; }
    }
}