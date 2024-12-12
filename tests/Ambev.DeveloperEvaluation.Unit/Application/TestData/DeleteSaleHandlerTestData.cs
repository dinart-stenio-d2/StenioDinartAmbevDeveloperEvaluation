using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class DeleteSaleHandlerTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid DeleteSaleCommand entities.
        /// </summary>
        /// <returns>A valid DeleteSaleCommand instance.</returns>
        public static DeleteSaleCommand GenerateValidCommand()
        {
            var faker = new Faker();

            return new DeleteSaleCommand
            {
                Id = Guid.NewGuid()
            };
        }

        /// <summary>
        /// Generates a valid Sale entity using Bogus.
        /// </summary>
        /// <returns>A valid Sale entity.</returns>
        public static Sale GenerateMockExistingSale()
        {
            var faker = new Faker();

            return new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = faker.Random.Replace("#####"),
                SaleDate = faker.Date.Past(),
                Customer = faker.Person.FullName,
                Branch = faker.Company.CompanyName(),
                Items = new Faker<SaleItem>()
                    .RuleFor(i => i.Product, f => f.Commerce.ProductName())
                    .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                    .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(1, 100))
                    .Generate(3)
            };
        }
    }
}
