using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;

namespace LabBilling.Core.Services.Validators
{
    public sealed class InterfaceMessageValidator : AbstractValidator<Account>
    {
        private readonly string connectionString;
        //ClientRepository clientRepository;

        public InterfaceMessageValidator(string connectionString)
        {
            this.connectionString = connectionString;

            RuleFor(a => a.PatLastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Patient Last Name is empty.");
            RuleFor(a => a.PatFirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Patient First Name is empty.");
            RuleFor(a => a.TransactionDate)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is empty.");

            RuleFor(a => a.ClientMnem)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is empty.");

        }

    }
}
