using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        /// <summary>
        /// Generates a list of mock SaleItem entities using NSubstitute.
        /// </summary>
        /// <param name="count">The number of SaleItem entities to generate.</param>
        /// <returns>A list of mocked SaleItem entities.</returns>
        private static List<SaleItemDto> GenerateMockSaleItemDtos(int count)
        {
            var saleItems = new List<SaleItemDto>();

            for (int i = 0; i < count; i++)
            {
                var saleItemDto = Substitute.For<SaleItemDto>();
                saleItemDto.Product = $"Product {i + 1}";
                saleItemDto.Quantity = i + 1;
                saleItemDto.UnitPrice = (i + 1) * 10;
                saleItems.Add(saleItemDto);
            }

            return saleItems;
        }

        /// <summary>
        /// Configures the NSubstitute mock to generate valid CreateSaleCommand entities.
        /// </summary>
        /// <returns>A valid CreateSaleCommand instance.</returns>
        public static CreateSaleCommand GenerateValidCommand()
        {
            var command = Substitute.For<CreateSaleCommand>();
            command.SaleNumber = "12345";
            command.SaleDate = DateTime.UtcNow;
            command.Customer = "Valid Customer";
            command.Branch = "Branch A";
            command.Items = GenerateMockSaleItemDtos(3);

            return command;
        }
    }
}
