using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class DeleteSaleProfile : Profile
    {
        public DeleteSaleProfile()
        {
            CreateMap<DeleteSaleRequest, DeleteSaleCommand>().ReverseMap();
            CreateMap<Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale.DeleteSaleResponse, Application.Sales.DeleteSales.DeleteSaleResponse>().ReverseMap();
            
        }
    }
}
