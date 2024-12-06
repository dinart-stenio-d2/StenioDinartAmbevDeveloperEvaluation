using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleItemValidator : AbstractValidator<SaleItem>
    {
        public SaleItemValidator()
        {
            RuleFor(item => item.Id)
           .NotEmpty().WithMessage("SaleItem ID is required.");

            RuleFor(item => item.Product)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(item => item.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price must be greater than or equal to 0.")
                .PrecisionScale(18, 2, true).WithMessage("Unit price must have up to 18 digits in total and 2 decimal places.");

            RuleFor(item => item.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount must be greater than or equal to 0.")
                .PrecisionScale(18, 2, true).WithMessage("Discount must have up to 18 digits in total and 2 decimal places.");

            RuleFor(item => item.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be greater than or equal to 0.")
                .PrecisionScale(18, 2, true).WithMessage("Total amount must have up to 18 digits in total and 2 decimal places.");
        }
    }
}
