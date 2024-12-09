using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class GetALLSaleProfile : Profile
    {
        public GetALLSaleProfile()
        {

            // Map SaleItem to GetAllSaleItemDto
            CreateMap<SaleItem, GetAllSaleItemDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));




            CreateMap<GetAllSaleItemDto, GetAllSalesItemDtoResponse>()
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                    .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                    .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));
       
                    
            CreateMap<Sale, GetAllSaleResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // Map GetAllSaleResult to GetAllSalesResponse
            CreateMap<GetAllSaleResult, GetAllSalesResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // Map GetAllSalesResult (containing list of sales) to a list of GetAllSalesResponse
            CreateMap<GetAllSalesResult, List<GetAllSalesResponse>>()
                .ConvertUsing((src, dest, context) =>
                    context.Mapper.Map<List<GetAllSalesResponse>>(src.Sales));
        }

    }
}
