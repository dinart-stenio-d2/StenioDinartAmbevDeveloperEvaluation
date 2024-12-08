using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSales
{
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {

            CreateMap<UpdateSaleCommand, Sale>()
               .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
               .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) 
               .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
               .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber));


            CreateMap<UpdateSaleItemDto, SaleItem>();

            CreateMap<Sale, UpdateSaleResult>()
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled));
        }
    }
}
