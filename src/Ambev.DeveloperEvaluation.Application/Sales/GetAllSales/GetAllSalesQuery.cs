using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    public class GetAllSalesQuery : IRequest<IEnumerable<GetAllSaleResult>>
    {

    }
}