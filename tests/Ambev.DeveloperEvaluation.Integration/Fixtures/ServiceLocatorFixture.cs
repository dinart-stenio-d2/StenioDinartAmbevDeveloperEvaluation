using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories.Generic;
using Ambev.DeveloperEvaluation.WebApi;
using AutoMapper;
using FluentAssertions.Common;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace Ambev.DeveloperEvaluation.Integration.Fixtures
{
    public class ServiceLocatorFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }

        public ServiceLocatorFixture()
        {
            var serviceCollection = new ServiceCollection();

            // Register DbContext with in-memory database for testing
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

       
           
            // Register DbContext with scoped lifetime (default)
            serviceCollection.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(connectionString),
                ServiceLifetime.Scoped // This is optional as scoped is the default.
            );



            // Register repositories and services
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddScoped<ISaleService, SaleService>();

            //Validations 
            serviceCollection.AddScoped<IValidator<Sale>, SaleValidator>();
            serviceCollection.AddScoped<IValidator<SaleItem>, SaleItemValidator>();
            serviceCollection.AddScoped<IValidator<(Sale ExistingSale, Sale NewSalesData)>, SaleUpdateValidator>();
            serviceCollection.AddScoped<IValidator<List<SaleItem>>, UpdateSaleItemsValidator>();
            serviceCollection.AddScoped<IValidator<DeleteSaleCommand>, DeleteSaleValidator>();
            serviceCollection.AddScoped<IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>, DeleteSaleHandler>();


            /// Register AutoMapper with assemblies
            serviceCollection.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly); // Register profiles
            });

          

            // Register AutoMapper
            //serviceCollection.ad(cfg =>
            //{
            //    // Add mappings from assemblies
            //    cfg.AddMaps(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            //    // Globally ignore unmapped properties
            //    cfg.ForAllMaps((typeMap, mapConfig) =>
            //    {
            //        foreach (var unmappedProperty in typeMap.GetUnmappedPropertyNames())
            //        {
            //            mapConfig.ForMember(unmappedProperty, opt => opt.Ignore());
            //        }
            //    });
            //});

            // Validate AutoMapper configuration
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mapperConfiguration = serviceProvider.GetRequiredService<IMapper>().ConfigurationProvider;
            mapperConfiguration.AssertConfigurationIsValid();


            // Register MediatR handlers
            serviceCollection.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            // Add logging
            serviceCollection.AddLogging();

            ServiceProvider = serviceCollection.BuildServiceProvider();
           
            // Validate AutoMapper configuration
            //using (var scope = ServiceProvider.CreateScope())
            //{
            //    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            //    try
            //    {
            //        mapper.ConfigurationProvider.AssertConfigurationIsValid();
            //        Console.WriteLine("AutoMapper configuration is valid.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"AutoMapper configuration error: {ex.Message}");
            //        throw; // Re-throw the exception to prevent further execution
            //    }
            //}
        }

        public void Dispose()
        {
            ((IDisposable)ServiceProvider)?.Dispose();
        }
    }
}
