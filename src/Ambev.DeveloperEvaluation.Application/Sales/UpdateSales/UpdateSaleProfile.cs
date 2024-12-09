using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSales
{
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            CreateMap<UpdateSaleCommand, Sale>()
           .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // TotalAmount será calculado separadamente
            CreateMap<UpdateSaleItemDto, SaleItem>().ReverseMap();

            CreateMap<Sale, UpdateSaleResult>().ReverseMap();

        }
    }
}
