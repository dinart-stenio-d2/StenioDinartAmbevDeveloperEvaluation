using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods to generate fake data for the SaleItem entity.
    /// This includes generating valid SaleItem instances and its properties.
    /// </summary>
    public static class SaleItemTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid SaleItem entities.
        /// The generated items will have valid:
        /// - ProductName (random product name)
        /// - Quantity (random integer between 1 and 10)
        /// - UnitPrice (random decimal between 1 and 100)
        /// - IsDiscountApplied (default to false)
        /// </summary>
        private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
            .RuleFor(i => i.Product, f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 100))
            .RuleFor(i => i.Discount, f => 0);

        /// <summary>
        /// Generates a valid list of SaleItem entities with randomized data.
        /// The generated list will contain the specified number of items.
        /// </summary>
        /// <param name="count">The number of SaleItem entities to generate.</param>
        /// <returns>A list of valid SaleItem entities.</returns>
        public static List<SaleItem> GenerateSaleItems(int count)
        {
            return SaleItemFaker.Generate(count);
        }

        /// <summary>
        /// Generates a single valid SaleItem entity with randomized data.
        /// The generated SaleItem will have all properties populated with valid values.
        /// </summary>
        /// <returns>A valid SaleItem entity.</returns>
        public static SaleItem GenerateValidSaleItem()
        {
            return SaleItemFaker.Generate();
        }
    }
}
