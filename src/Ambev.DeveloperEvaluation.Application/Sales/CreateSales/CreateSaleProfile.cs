using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {

            //CreateMap<CreateSaleCommand, Sale>()
            //    .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // TotalAmount será calculado

            //CreateMap<SaleItemDto, SaleItem>();

            //CreateMap<Sale, CreateSaleResult>();

            CreateMap<SaleItemDto, SaleItem>();

            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // TotalAmount será calculado

            // Mapear Sale para CreateSaleResult
            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)); // Mapear a lista de Items

            // Mapear SaleItem para SaleItemDto
            CreateMap<SaleItem, SaleItemDto>();
        }
    }
}
