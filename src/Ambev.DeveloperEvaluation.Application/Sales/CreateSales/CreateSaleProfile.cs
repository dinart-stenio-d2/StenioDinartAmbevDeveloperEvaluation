using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
         
            CreateMap<SaleItemDto, SaleItem>();

            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); 


            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)); 

            CreateMap<SaleItem, SaleItemDto>();
        }
    }
}
