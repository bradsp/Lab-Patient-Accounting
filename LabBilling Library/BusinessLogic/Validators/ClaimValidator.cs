﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;


namespace LabBilling.Core.BusinessLogic.Validators
{
    public class ClaimValidator : AbstractValidator<Account>
    {
        public ClaimValidator()
        {

            RuleFor(a => a.TransactionDate)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is empty.");
            RuleFor(a => a.PatLastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Patient Last Name is empty.")
                .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");
            RuleFor(a => a.PatFirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Patient First Name is empty.")
                .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");
            RuleFor(a => a.Charges.Count)
                .GreaterThan(0).WithMessage("No charges to bill.");


            When(a => a.Fin.type == "M", () =>
            {
                RuleFor(a => a.Pat)
                    .SetValidator(new DemographicsValidator());

                RuleFor(a => a.Insurances)
                    .Must(HaveValidInsuranceCoverageCodes).WithMessage("Insurances have invalid or out of order coverage codes.");

                RuleForEach(a => a.Insurances)
                    .SetValidator(new InsuranceValidator()).When(a => a.FinCode != "E");

                RuleFor(a => a.FinCode)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("{PropertyName} is empty.")
                    .Must((a, f) => f == a.InsurancePrimary.FinCode)
                    .WithMessage("Account fin code does not equal insurance fin code")
                    .When(ac => ac.InsurancePrimary != null);

                RuleFor(a => a.TotalCharges)
                    .GreaterThan(0);
                RuleFor(a => a.Charges.Sum(x => x.Quantity))
                    .GreaterThan(0).WithMessage("Charge qty nets zero")
                    .When(ac => ac.Charges.Count > 0);
                RuleForEach(a => a.Charges)
                    .SetValidator(new ChargeValidator())
                    .When(ac => ac.Charges.Count > 0);

                RuleFor(a => a.TotalPayments)
                    .LessThanOrEqualTo(0).WithMessage("Account has payments recorded.");

                RuleFor(c => c)
                    .Must(NotContainOnlyVenipunctureCharge)
                    .WithMessage("Venipuncture is only charge on account");

                RuleFor(a => a.Charges)
                    .Must(NotContainMultipleVenipunctures).WithMessage("Multiple venipuncture charges on account")
                    .When(a => a.Charges != null);

                RuleFor(a => a)
                    .Must(HaveMatchingPersonAndInsRelation).WithMessage("Person relation and insurance relation do not match")
                    .Must(MatchPatientNameAndInsuranceHolderName).WithMessage("Person Name and Insurance Holder Name do not match")
                    .When(a => a.Pat != null && a.InsurancePrimary != null);

                RuleFor(a => a.LmrpErrors)
                    .Empty().WithMessage("LMRP Rule Violation");
            });

            When(a => a.Fin.type != "M", () =>
            {


            });

        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

        private bool NotContainOnlyVenipunctureCharge(Account account)
        {
            if (account.cpt4List.Distinct().Count() == 1)
            {
                if (account.cpt4List[0] == "36415")
                {
                    return false;
                }
            }
            return true;
        }

        private bool NotContainMultipleVenipunctures(List<Chrg> charges)
        {
            var total = charges.Where(c => c.ChrgDetails.Any(_ => _.Cpt4 == "36415"))
                .Sum(x => x.Quantity);

            if (total > 1)
                return false;
            else
                return true;
        }

        private bool HaveMatchingPersonAndInsRelation(Account account)
        {
            return account.Pat.GuarRelationToPatient == account.InsurancePrimary.Relation;
        }

        private bool MatchPatientNameAndInsuranceHolderName(Account account)
        {
            bool process = false;
            if (account.FinCode == "A" || account.FinCode == "D" || account.FinCode == "M")
                process = true;
            if (account.FinCode == "H" && account.InsurancePrimary.InsCode == "HESP")
                process = true;
            if (account.FinCode == "L" && new string[] { "SEHZ", "WIN", "SECP", "HUM" }.Contains(account.InsurancePrimary.InsCode))
                process = true;

            if (process)
            {
                if (account.Pat.GuarRelationToPatient == "01")
                {
                    if (account.PatLastName == account.InsurancePrimary.HolderLastName &&
                        account.PatFirstName == account.InsurancePrimary.HolderFirstName &&
                        account.PatMiddleName == account.InsurancePrimary.HolderMiddleName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool HaveValidInsuranceCoverageCodes(List<Ins> insurances)
        {
            bool hasA = false;
            bool hasB = false;
            bool hasC = false;

            foreach(Ins ins in insurances.Where(i => !i.IsDeleted))
            {
                if (ins.IsDeleted)
                    continue;

                if(ins.Coverage == "A")
                    hasA = true;
                if(ins.Coverage == "B")
                    hasB = true;
                if (ins.Coverage == "C")
                    hasC = true;
            }

            if(insurances.Where(i => !i.IsDeleted).Count() > 0)
            {
                if (!hasA)
                    return false;
                if (hasC && !hasB)
                    return false;

                return true;
            }
            else
            {
                return true;
            }
        }

    }
}