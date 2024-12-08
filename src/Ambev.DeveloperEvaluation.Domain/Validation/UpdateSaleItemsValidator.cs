using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class UpdateSaleItemsValidator : AbstractValidator<List<SaleItem>>
    {
        public UpdateSaleItemsValidator()
        {
            RuleForEach(items => items).SetValidator(new SaleItemValidator())
                .WithMessage("One or more SaleItems have validation errors.");
        }
    }
}
