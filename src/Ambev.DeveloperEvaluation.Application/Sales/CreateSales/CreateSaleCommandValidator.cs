using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    internal class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {

            RuleFor(sale => sale.SaleDate)
                .NotEmpty().WithMessage("SaleDate is required.");

            RuleFor(sale => sale.Customer)
                .NotEmpty().WithMessage("Customer is required.")
                .MaximumLength(100).WithMessage("Customer cannot exceed 100 characters.");

            RuleFor(sale => sale.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .MaximumLength(50).WithMessage("Branch cannot exceed 50 characters.");

            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("At least one sale item is required.");

            RuleForEach(sale => sale.Items).ChildRules(items =>
            {
                items.RuleFor(item => item.Product)
                    .NotEmpty().WithMessage("Product is required.")
                    .MaximumLength(100).WithMessage("Product cannot exceed 100 characters.");

                items.RuleFor(item => item.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

                items.RuleFor(item => item.UnitPrice)
                     .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.")
                     .PrecisionScale(18, 2, true).WithMessage("UnitPrice must have up to 18 digits in total and 2 decimal places.");
            });
        }
    }
}