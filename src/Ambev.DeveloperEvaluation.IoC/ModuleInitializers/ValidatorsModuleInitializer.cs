using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;
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
        }
    }
}
