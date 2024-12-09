using Ambev.DeveloperEvaluation.Application.Sales.UpdateSales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(s => s.Id)
                .NotEmpty().WithMessage("Sale ID is required.");

           
            RuleFor(s => s.SaleNumber)
                .MaximumLength(10)
                .WithMessage("SaleNumber must not exceed 10 characters.");

            
            RuleFor(s => s.Customer)
                .MaximumLength(50)
                .WithMessage("Customer must not exceed 50 characters.");

            
            RuleFor(s => s.Branch)
                .MaximumLength(50)
                .WithMessage("Branch must not exceed 50 characters.");

            RuleForEach(sale => sale.Items).ChildRules(items =>
            {
                items.RuleFor(item => item.Product)
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