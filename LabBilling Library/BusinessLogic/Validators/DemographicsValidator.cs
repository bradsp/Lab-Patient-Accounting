using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.BusinessLogic.Validators
{
    public class DemographicsValidator : AbstractValidator<Pat>
    {
        public DemographicsValidator()
        {
            RuleFor(a => a)
                .Must(a => a.Diagnoses.Count >= 1).WithMessage("Patient has no diagnosis codes.");
            RuleFor(a => a.Address1)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidAddress).WithMessage("{PropertyName} has invalid characters.");
            RuleFor(a => a.State)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2).WithMessage("Patient State {PropertyName} must have 2 characters.");
            RuleFor(a => a.City)
                .NotEmpty()
                .WithMessage("Patient {PropertyName} is empty.");
            RuleFor(a => a.ZipCode)
                .NotEmpty()
                .WithMessage("Patient {PropertyName} is empty.");
            RuleFor(a => a.GuarRelationToPatient)
                .NotEmpty().WithMessage("Guarantor relationship is not selected.");
            RuleFor(a => a.GuarantorLastName)
                .NotEmpty().WithMessage("Guarantor Last name is empty.");
            RuleFor(a => a.GuarantorFirstName)
                .NotEmpty().WithMessage("Guarantor First name is empty.");
            RuleFor(a => a.GuarantorAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidAddress).WithMessage("Guarantor Address has invalid characters.");
            RuleFor(a => a.GuarantorCity)
                .NotEmpty().WithMessage("Guarantor City is empty.");
            RuleFor(a => a.GuarantorState)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2).WithMessage("Guarantor State must have 2 characters.");
            RuleFor(a => a.GuarantorZipCode)
                .NotEmpty();
            RuleFor(a => a.ProviderId)
                .NotEmpty().WithMessage("No ordering provider.");
            RuleFor(a => a.Physician).SetValidator(new PhysicianValidator())
                .When(a => !string.IsNullOrEmpty(a.ProviderId));

        }

        private bool BeAValidAddress(string address)
        {
            char[] invalidChar = new char[] { '`', '!', '@', '#', '$', '~', '%', '^', '*', '|', '\\', '/', '<', '>' };

            bool hasInvalidChar = !address.Any(c => invalidChar.Contains(c));

            return hasInvalidChar;
        }
    }
}
