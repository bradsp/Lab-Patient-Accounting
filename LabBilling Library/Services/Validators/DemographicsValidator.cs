using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.Services.Validators
{
    public sealed class DemographicsValidator : AbstractValidator<Account>
    {
        public DemographicsValidator()
        {
            RuleFor(a => a)
                .Must(a => a.Pat.Diagnoses.Count >= 1).WithMessage("Patient has no diagnosis codes.")
                .When(a => a.FinCode != "E");
            RuleFor(a => a.Pat.Address1)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidAddress).WithMessage("{PropertyName} has invalid characters.");
            RuleFor(a => a.Pat.State)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2).WithMessage("Patient State {PropertyName} must have 2 characters.");
            RuleFor(a => a.Pat.City)
                .NotEmpty()
                .WithMessage("Patient {PropertyName} is empty.");
            RuleFor(a => a.Pat.ZipCode)
                .NotEmpty()
                .WithMessage("Patient {PropertyName} is empty.");
            RuleFor(a => a.Pat.GuarRelationToPatient)
                .NotEmpty().WithMessage("Guarantor relationship is not selected.");
            RuleFor(a => a.Pat.GuarantorLastName)
                .NotEmpty().WithMessage("Guarantor Last name is empty.");
            RuleFor(a => a.Pat.GuarantorFirstName)
                .NotEmpty().WithMessage("Guarantor First name is empty.");
            RuleFor(a => a.Pat.GuarantorAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidAddress).WithMessage("Guarantor Address has invalid characters.");
            RuleFor(a => a.Pat.GuarantorCity)
                .NotEmpty().WithMessage("Guarantor City is empty.");
            RuleFor(a => a.Pat.GuarantorState)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2).WithMessage("Guarantor State must have 2 characters.");
            RuleFor(a => a.Pat.GuarantorZipCode)
                .NotEmpty();
            RuleFor(a => a.Pat.ProviderId)
                .NotEmpty().WithMessage("No ordering provider.");
            RuleFor(a => a.Pat.Physician).SetValidator(new PhysicianValidator())
                .When(a => !string.IsNullOrEmpty(a.Pat.ProviderId));

        }

        private bool BeAValidAddress(string address)
        {
            char[] invalidChar = new char[] { '`', '!', '@', '#', '$', '~', '%', '^', '*', '|', '\\', '/', '<', '>' };

            bool hasInvalidChar = !address.Any(c => invalidChar.Contains(c));

            return hasInvalidChar;
        }
    }
}
