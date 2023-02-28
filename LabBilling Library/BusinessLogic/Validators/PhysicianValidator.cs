using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.BusinessLogic.Validators
{
    public class PhysicianValidator : AbstractValidator<Phy>
    {
        public PhysicianValidator()
        {
            RuleFor(a => a.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Provider Last Name is empty.")
                .Must(BeAValidName).WithMessage("Provider {PropertyName} contains invalid characters.");
            RuleFor(a => a.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Provider First Name is empty.")
                .Must(BeAValidName).WithMessage("Provider {PropertyName} contains invalid characters.");
            RuleFor(a => a.NpiId)
                .NotEmpty().WithMessage("Physician does not have valid NPI.");
        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

    }
}
