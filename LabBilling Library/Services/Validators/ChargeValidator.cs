using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.Services.Validators
{
    public sealed class ChargeValidator : AbstractValidator<Chrg>
    {
        public ChargeValidator()
        {
            RuleFor(c => c.NetAmount)
                .GreaterThan(0)
                .When(c => !c.IsCredited && c.Status != "N/A" && c.CDMCode != "CBILL");

            RuleForEach(c => c.ChrgDetails)
                .SetValidator(new ChargeDetailValidator())
                .When(c => !c.IsCredited && c.FinancialType == "M");
        }
    }

    public sealed class ChargeDetailValidator : AbstractValidator<ChrgDetail>
    {
        public ChargeDetailValidator()
        {
            RuleFor(c => c.Cpt4)
                .NotEmpty();

            RuleFor(c => c.Type)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(c => c == "NORM" || c == "N/A" || c == "TC")
                .WithMessage("Charge detail type is not valid. Must be NORM, N/A, or TC.");

            //RuleFor(c => c.DiagCodePointer)
            //    .NotNull().WithMessage("Cpt diagnosis pointer is null.");

            //RuleFor(c => c.DiagCodePointer)
            //    .Cascade(CascadeMode.Stop)
            //    .NotEmpty().WithMessage("Cpt diagnosis pointer is empty.")
            //    .NotNull().WithMessage("Cpt diagnosis pointer is null.");
        }
    }
}
