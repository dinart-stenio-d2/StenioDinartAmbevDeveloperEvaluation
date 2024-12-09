using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleItemDtoRequest, SaleItemDto>();
            CreateMap<CreateSaleRequest, CreateSaleCommand>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));


            CreateMap<SaleItemDto, CreateSaleItemDtoResponse>();

            CreateMap<CreateSaleResult, CreateSaleResponse>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)); 


        }
    }
}
