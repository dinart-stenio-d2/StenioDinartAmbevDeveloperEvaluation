using Ambev.DeveloperEvaluation.Application.Sales.UpdateSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class UpdateSaleHandlerTestData
    {
        /// <summary>
        /// Generates a list of mock SaleItemDto entities using NSubstitute.
        /// </summary>
        /// <param name="count">The number of SaleItemDto entities to generate.</param>
        /// <returns>A list of mocked SaleItemDto entities.</returns>
        private static List<UpdateSaleItemDto> GenerateMockSaleItemDtos(int count)
        {
            var saleItems = new List<UpdateSaleItemDto>();

            for (int i = 0; i < count; i++)
            {
                var saleItemDto = Substitute.For<UpdateSaleItemDto>();
                saleItemDto.Product = $"Product {i + 1}";
                saleItemDto.Quantity = i + 1;
                saleItemDto.UnitPrice = (i + 1) * 10;
                saleItems.Add(saleItemDto);
            }

            return saleItems;
        }


        /// <summary>
        /// Configures the NSubstitute mock to generate valid UpdateSaleCommand entities.
        /// </summary>
        /// <returns>A valid UpdateSaleCommand instance.</returns>
        public static UpdateSaleCommand GenerateValidCommand()
        {
            var command = Substitute.For<UpdateSaleCommand>();
            command.Id = Guid.NewGuid();
            command.SaleNumber = "12345";
            command.SaleDate = DateTime.UtcNow;
            command.Customer = "Valid Customer";
            command.Branch = "Branch A";
            command.Items = GenerateMockSaleItemDtos(3); 

            return command;
        }
        /// <summary>
        /// Generates a mock existing Sale entity using NSubstitute.
        /// </summary>
        /// <returns>A mocked Sale entity.</returns>
        public static Sale GenerateMockExistingSale()
        {
            var sale = Substitute.For<Sale>();
            sale.Id = Guid.NewGuid();
            sale.SaleNumber = "ExistingSale12345";
            sale.SaleDate = DateTime.UtcNow.AddDays(-1);
            sale.Customer = "Existing Customer";
            sale.Branch = "Existing Branch";
            sale.Items = new List<SaleItem>
            {
                new SaleItem { Id = Guid.NewGuid(), Product = "Existing Product", Quantity = 5, UnitPrice = 20 }
            };

            return sale;
        }
    }
}
