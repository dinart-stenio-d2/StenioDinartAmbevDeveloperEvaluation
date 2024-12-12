using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;


namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {

        public static class SaleValidationExtensions
        {
            public static bool BeWithinValidDateRange(DateTime saleDate)
            {
                var today = DateTime.UtcNow.Date;
                var pastLimit = today.AddDays(-30);
                var futureLimit = today.AddDays(7);

                return saleDate.Date >= pastLimit && saleDate.Date <= futureLimit;
            }
        }
        public SaleValidator()
        {
            RuleForEach(sale => sale.Items).ChildRules(items =>
            {
                items.RuleFor(item => item.Product)
                     .NotEmpty().WithMessage("Product name is required.");

                items.RuleFor(item => item.UnitPrice)
                     .GreaterThan(0).WithMessage("Unit price must be greater than 0.");

                items.RuleFor(item => item.Quantity)
                    .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 items of the same product.");

                items.RuleFor(item => item.Quantity)
                    .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");
            });

            RuleFor(sale => sale.Id)
                  .NotEmpty().WithMessage("Sale ID is required.")
                  .Must(id => id != Guid.Empty).WithMessage("Sale ID cannot be the default GUID.");

            RuleFor(sale => sale.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

   
                RuleFor(sale => sale.SaleDate)
                    .NotEmpty().WithMessage("Sale date is required.")
                    .Must(SaleValidationExtensions.BeWithinValidDateRange)
                    .WithMessage("Sale date must be within the last 30 days and no more than 7 days in the future.");
            

            RuleFor(sale => sale.Items.Sum(item => item.Quantity))
             .GreaterThanOrEqualTo(1)
             .WithMessage("A sale must have at least one item.");

            RuleFor(sale => sale.Customer)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(sale => sale.TotalAmount)
              .GreaterThanOrEqualTo(0).WithMessage("Total amount must be greater than or equal to 0.")
              .PrecisionScale(18, 2, true).WithMessage("Total amount must have up to 18 digits in total and 2 decimal places.");

            RuleFor(sale => sale.IsCancelled)
                .NotNull().WithMessage("IsCancelled field is required.");
            
            RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("A sale must have at least one item.");


            RuleForEach(sale => sale.Items).SetValidator(new SaleItemValidator());

        }
    }
}
