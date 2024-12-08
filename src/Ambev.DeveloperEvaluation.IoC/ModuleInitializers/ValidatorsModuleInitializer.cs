using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers
{
    public class ValidatorsModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<Sale>, SaleValidator>();
            builder.Services.AddScoped<IValidator<SaleItem>, SaleItemValidator>();
            builder.Services.AddScoped<IValidator<(Sale ExistingSale, Sale NewSalesData)>, SaleUpdateValidator>();
            builder.Services.AddScoped<IValidator<List<SaleItem>>, UpdateSaleItemsValidator>();
            builder.Services.AddScoped<IValidator<DeleteSaleCommand>, DeleteSaleValidator>();
            builder.Services.AddScoped<IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>, DeleteSaleHandler>();
        }
    }
}
