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
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            serviceCollection.AddDbContext<DefaultContext>(options =>
                    options.UseNpgsql(connectionString));


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


            // Register AutoMapper
            serviceCollection.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

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
