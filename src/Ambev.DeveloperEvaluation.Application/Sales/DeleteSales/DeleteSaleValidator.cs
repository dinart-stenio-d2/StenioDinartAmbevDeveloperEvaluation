using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSales
{
    public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
    {
        public DeleteSaleValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Sale ID is required");
        }
    }
}