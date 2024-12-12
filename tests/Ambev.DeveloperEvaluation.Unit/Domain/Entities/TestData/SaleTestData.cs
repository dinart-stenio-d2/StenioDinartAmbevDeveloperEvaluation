using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    /// <summary>
    /// Provides methods to generate fake data for the Sale entity.
    /// This includes generating valid Sale instances and its properties.
    /// </summary>
    public static class SaleTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid Sale entities.
        /// The generated sales will have valid:
        /// - SaleNumber (nullable by default)
        /// - SaleDate (random past date)
        /// - Customer (random full name)
        /// - Branch (random company name)
        /// - TotalAmount (default to 0)
        /// - IsCancelled (random boolean value)
        /// - Items (random list of SaleItem entities)
        /// </summary>
        private static Faker<Sale> GetSaleFaker(int itemCount)
        {
            return new Faker<Sale>()
                .RuleFor(s => s.SaleNumber, f => null)
                .RuleFor(s => s.SaleDate, f => f.Date.Past())
                .RuleFor(s => s.Customer, f => f.Person.FullName)
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.TotalAmount, f => 0)
                .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
                .RuleFor(s => s.Items, f => SaleItemTestData.GenerateSaleItems(itemCount));
        }

        /// <summary>
        /// Generates a valid Sale entity with a specified number of SaleItems.
        /// </summary>
        /// <param name="itemCount">The number of SaleItems to generate. Default is 2.</param>
        /// <returns>A valid Sale entity with the specified number of SaleItems.</returns>
        public static Sale GenerateValidSale(int itemCount = 2)
        {
            return GetSaleFaker(itemCount).Generate();
        }
    }
}

