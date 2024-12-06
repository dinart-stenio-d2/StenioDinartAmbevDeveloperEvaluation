using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers
{
    public class DomainServicesModuleInitializer : IModuleInitializer
    {
        public void Initialize(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISaleService,SaleService>();
        }
    }
}
