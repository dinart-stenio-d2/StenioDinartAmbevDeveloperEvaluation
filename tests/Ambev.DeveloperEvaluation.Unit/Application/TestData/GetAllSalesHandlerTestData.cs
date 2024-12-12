using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class GetAllSalesHandlerTestData
    {
        /// <summary>
        /// Generates a valid GetAllSalesQuery instance.
        /// </summary>
        /// <returns>A valid GetAllSalesQuery instance.</returns>
        public static GetAllSalesQuery GenerateValidQuery()
        {
            return new GetAllSalesQuery(); // Assuming GetAllSalesQuery has no properties to initialize
        }

        /// <summary>
        /// Generates a list of mock Sale entities using NSubstitute.
        /// </summary>
        /// <param name="count">The number of Sale entities to generate.</param>
        /// <returns>A list of mocked Sale entities.</returns>
        public static List<Sale> GenerateMockSales(int count)
        {
            var sales = new List<Sale>();

            for (int i = 0; i < count; i++)
            {
                var sale = Substitute.For<Sale>();
                sale.Id = Guid.NewGuid();
                sale.SaleNumber = "12345";
                sale.SaleDate = DateTime.UtcNow.AddDays(-i);
                sale.Customer = $"Customer {i + 1}";
                sale.Branch = $"Branch {i + 1}";
                sale.Items = GenerateMockSaleItems(3);
                sales.Add(sale);
            }

            return sales;
        }

        /// <summary>
        /// Generates a list of mock GetAllSaleResult entities using NSubstitute.
        /// </summary>
        /// <param name="count">The number of GetAllSaleResult entities to generate.</param>
        /// <returns>A list of mocked GetAllSaleResult entities.</returns>
        public static List<GetAllSaleResult> GenerateMockGetAllSaleResults(int count)
        {
            var results = new List<GetAllSaleResult>();

            for (int i = 0; i < count; i++)
            {
                var result = Substitute.For<GetAllSaleResult>();
                result.Id = Guid.NewGuid();
                result.SaleNumber = "12345";
                result.SaleDate = DateTime.UtcNow.AddDays(-i);
                result.Customer = $"Customer {i + 1}";
                result.Branch = $"Branch {i + 1}";
                result.Items = new List<GetAllSaleItemDto>(GenerateMockGetSaleItemDtos(3));
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Generates a list of mock SaleItem entities using NSubstitute.
        /// </summary>
        /// <param name="count">The number of SaleItem entities to generate.</param>
        /// <returns>A list of mocked SaleItem entities.</returns>
        private static List<SaleItem> GenerateMockSaleItems(int count)
        {
            var saleItems = new List<SaleItem>();

            for (int i = 0; i < count; i++)
            {
                var item = Substitute.For<SaleItem>();
                item.Product = $"Product {i + 1}";
                item.Quantity = i + 1;
                item.UnitPrice = (i + 1) * 10;
                saleItems.Add(item);
            }

            return saleItems;
        }

        /// <summary>
        /// Generates a list of mock GetAllSaleItemDto entities using NSubstitute.
        /// </summary>
        /// <param name="count">The number of GetAllSaleItemDto entities to generate.</param>
        /// <returns>A list of mocked GetAllSaleItemDto entities.</returns>
        private static List<GetAllSaleItemDto> GenerateMockGetSaleItemDtos(int count)
        {
            var saleItemDtos = new List<GetAllSaleItemDto>();

            for (int i = 0; i < count; i++)
            {
                var itemDto = Substitute.For<GetAllSaleItemDto>();
                itemDto.Product = $"Product {i + 1}";
                itemDto.Quantity = i + 1;
                itemDto.UnitPrice = (i + 1) * 10;
                saleItemDtos.Add(itemDto);
            }

            return saleItemDtos;
        }
    }
}
