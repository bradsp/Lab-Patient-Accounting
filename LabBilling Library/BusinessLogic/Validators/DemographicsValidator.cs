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
            RuleFor(a => a.Address1)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidAddress).WithMessage("{PropertyName} has invalid characters.");
            RuleFor(a => a.CityStateZip)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} does not have a value")
                .Equal(",").WithMessage("{PropertyName} has no value");
            RuleFor(a => a.State)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2).WithMessage("{PropertyName} must have 2 characters.");
            RuleFor(a => a.City).NotEmpty();
            RuleFor(a => a.ZipCode).NotEmpty();
            RuleFor(a => a.BirthDate).NotNull();
            RuleFor(a => a.Sex)
                .Must(sex => sex == "M" || sex == "F")
                .WithMessage("{PropertyName} is not a valid value.");
            RuleFor(a => a.GuarRelationToPatient).NotEmpty();
            RuleFor(a => a.GuarantorLastName)
                .NotEmpty();
            RuleFor(a => a.GuarantorFirstName)
                .NotEmpty();
            RuleFor(a => a.GuarantorAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidAddress).WithMessage("{PropertyName} has invalid characters.");
            RuleFor(a => a.GuarantorCity)
                .NotEmpty();
            RuleFor(a => a.GuarantorState)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2).WithMessage("{PropertyName} must have 2 characters.");
            RuleFor(a => a.GuarantorZipCode)
                .NotEmpty();
            RuleFor(a => a.ProviderId)
                .NotEmpty();
            RuleFor(a => a.Physician).SetValidator(new PhysicianValidator());

        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

        private bool BeAValidAddress(string address)
        {
            char[] invalidChar = new char[] { '`', '!', '@', '#', '$', '~', '%', '^', '*', '|', '\\', '/', '<', '>' };

            bool hasInvalidChar = !address.Any(c => invalidChar.Contains(c));

            return hasInvalidChar;
        }
    }
}
