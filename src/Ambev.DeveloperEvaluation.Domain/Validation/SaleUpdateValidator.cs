using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleUpdateValidator : AbstractValidator<(Sale ExistingSale, Sale NewSalesData)>
    {
        public SaleUpdateValidator()
        {

            RuleFor(salePair => salePair.NewSalesData.SaleNumber)
                .Equal(salePair => salePair.ExistingSale.SaleNumber)
                .When(salePair => salePair.NewSalesData.SaleNumber != salePair.ExistingSale.SaleNumber)
                .WithMessage("SaleNumber cannot be updated.");

            RuleFor(salePair => salePair.NewSalesData.Customer)
                .Equal(salePair => salePair.ExistingSale.Customer)
                .When(salePair => salePair.NewSalesData.Customer != salePair.ExistingSale.Customer)
                .WithMessage("Customer cannot be updated.");

        }
    }
}
