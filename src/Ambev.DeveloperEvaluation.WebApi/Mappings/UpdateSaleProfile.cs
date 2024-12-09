using Ambev.DeveloperEvaluation.Application.Sales.UpdateSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class UpdateSaleProfile: Profile
    {
        public UpdateSaleProfile()
        {
            CreateMap<UpdateSaleItemDtoRequest, UpdateSaleItemDto>();
            
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));


            CreateMap<UpdateSaleItemDto, UpdateSaleItemDtoResponse>();

            CreateMap<UpdateSaleResult, UpdateSaleResponse>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
       
    }
}
