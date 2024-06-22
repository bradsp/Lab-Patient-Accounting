using FluentValidation;
using LabBilling.Core.Models;
using System;
using System.Linq;

namespace LabBilling.Core.Services.Validators;

public sealed class PhysicianValidator : AbstractValidator<Phy>
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
            .Must(BeAValidName).WithMessage(a => $"Provider Name {a.FullName} contains invalid characters.");
        RuleFor(a => a.NpiId)
            .NotEmpty().WithMessage(a => $"Physician {a.FullName} does not have valid NPI.");
        RuleFor(a => a)
            .Must(NotBeExcludedProvider).WithMessage(a => $"Provider {a.FullName} is on the OIG Exclusion list. DO NOT BILL.");
    }

    private bool BeAValidName(string name)
    {
        name = name.Replace(" ", "");
        name = name.Replace("-", "");
        name = name.Replace("'", "");
        return name.All(Char.IsLetter);
    }

    private bool NotBeExcludedProvider(Phy phy)
    {
        if (phy.SanctionedProvider != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
