using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile() 
        {

           
            CreateMap<GetSaleItemDto, GetSaleSaleItemDtoResponse>();

          
            CreateMap<GetSaleResult, GetSaleResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
