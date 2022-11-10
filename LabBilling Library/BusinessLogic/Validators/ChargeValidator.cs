using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.BusinessLogic.Validators
{
    public class ChargeValidator : AbstractValidator<Chrg>
    {
        public ChargeValidator()
        {
            RuleFor(c => c.NetAmount)
                .GreaterThan(0)
                .When(c => !c.IsCredited);

            RuleForEach(c => c.ChrgDetails)
                .SetValidator(new ChargeDetailValidator())
                .When(c => !c.IsCredited);
        }

        private bool NotContainOnlyVenipunctureCharge(List<ChrgDetail> chrgDetails)
        {
            if (chrgDetails.Count == 1)
                if(chrgDetails.Where(x => x.Cpt4 == "36415").Count() > 0)
                    return false;

            return true;
        }

    }

    public class ChargeDetailValidator : AbstractValidator<ChrgDetail>
    {
        public ChargeDetailValidator()
        {
            RuleFor(c => c.Cpt4)
                .NotEmpty();
            RuleFor(c => c.Type)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(c => c == "NORM" || c == "N/A" || c == "TC");
            RuleFor(c => c.DiagCodePointer)
                .NotEmpty().WithMessage("Cpt diagnosis pointer is empty.")
                .NotNull().WithMessage("Cpt diagnosis pointer is null");
        }
    }
}
