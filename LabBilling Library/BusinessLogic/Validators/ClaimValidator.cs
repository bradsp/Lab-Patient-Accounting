using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;


namespace LabBilling.Core.BusinessLogic.Validators
{
    public sealed class ClaimValidator : AbstractValidator<Account>
    {
        public ClaimValidator()
        {

            RuleFor(a => a.TransactionDate)
                .NotNull()
                .NotEmpty().WithMessage("{PropertyName} is empty.")
                .GreaterThan(DateTime.Now.AddYears(-20)).WithMessage("{PropertyName} not a valid date.");

            RuleFor(a => a.PatLastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Patient Last Name is empty.")
                .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(a => a.PatFirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Patient First Name is empty.")
                .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            RuleFor(a => a.BirthDate).NotNull();

            RuleFor(a => a.Sex)
                .Must(sex => sex == "M" || sex == "F")
                .WithMessage("{PropertyName} is not a valid value.");
            
            RuleFor(a => a.Charges.Count)
                .GreaterThan(0).WithMessage("No charges to bill.");

            RuleFor(a => a.Fin.FinClass)
                .Equal("M").WithMessage("This is a client bill account. Do not bill insurance.");

            //rules for all 3rd party bill accounts
            When(a => a.Fin.FinClass == "M", () =>
            {
                RuleFor(a => a)
                    .SetValidator(new DemographicsValidator());

                RuleFor(a => a.TotalCharges)
                    .GreaterThan(0);

                RuleFor(a => a.Charges.Where(x => x.FinancialType == "M").Sum(x => x.Quantity))
                    .GreaterThan(0).WithMessage("Charge qty nets zero")
                    .When(ac => ac.Charges.Count > 0);

                RuleForEach(a => a.Charges)
                    .SetValidator(new ChargeValidator())
                    .When(ac => ac.Charges.Count > 0);

                RuleFor(a => a)
                    .Must(NotContainOnlyVenipunctureCharge)
                    .WithMessage("Venipuncture is only charge on account");

                //RuleFor(c => c.Charges)
                //    .Must(NotHaveDuplicateCdms)
                //    .WithMessage("Mutually Exclusive CDMs 5686066 and 5686078")
                //    .When(a => a.Charges != null);

                RuleFor(a => a.Charges)
                    .Must(NotContainMultipleVenipunctures).WithMessage("Multiple venipuncture charges on account")
                    .When(a => a.Charges != null);

                RuleFor(a => a)
                    .Must(MatchChargeServiceDateAndAccountTransDate).WithMessage("Account DOS does not match charge service date.");

                RuleFor(a => a.Charges.Where(x => x.FinancialType == "M").Count())
                    .GreaterThan(0)
                    .WithMessage("No claim billable charges.");
                    

                //todo: rule - each diagnosis must be used in a dx Pointer.                


            });

            //rules for insurance billed accounts (exclude self-pay)
            When(a => a.Fin.FinClass == "M" && a.FinCode != "E", () =>
            {
                RuleFor(a => a.Insurances)
                    .Must(HaveValidInsuranceCoverageCodes).WithMessage("Insurances have invalid or out of order coverage codes.");

                RuleFor(a => a.Insurances)
                    .Must(x => x.Count > 0)
                    .WithMessage("No insurances defined for account.")
                    .When(x => x.FinCode != "E");

                RuleForEach(a => a.Insurances)
                    .SetValidator(new InsuranceValidator()).When(a => a.FinCode != "E");

                RuleFor(a => a)
                    .Must(NotHaveUnusedDiagnosis).WithMessage("All diagnosis codes are not used in a pointer.")
                    .When(a => a.Fin.ClaimType == "1500");

                RuleFor(a => a.FinCode)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("{PropertyName} is empty.")
                    .Must((a, f) => f == a.InsurancePrimary.FinCode)
                    .WithMessage("Account fin code does not equal insurance fin code")
                    .When(ac => ac.InsurancePrimary != null);

                RuleFor(a => a.TotalPayments)
                    .LessThanOrEqualTo(0).WithMessage("Account has payments recorded.")
                    .When(a => a.Status == "NEW");

                RuleFor(a => a.Charges)
                    .Must(NotNeedRepeatModifier)
                    .WithMessage("Duplicate cpt - needs modifier.")
                    .When(a => a.Charges != null);

                RuleFor(a => a)
                    .Must(HaveMatchingPersonAndInsRelation).WithMessage("Person relation and insurance relation do not match")
                    .Must(MatchPatientNameAndInsuranceHolderName).WithMessage("Person Name and Insurance Holder Name do not match")
                    .When(a => a.Pat != null && a.InsurancePrimary != null && (a.FinCode == "A" || a.FinCode == "D"));

                RuleFor(a => a)
                    .Must(MatchPatientNameAndInsuranceHolderName).WithMessage("Ins holder with self relation must match patient");

                RuleFor(a => a)
                    .Must(NotHaveBundledOBPanel).WithMessage("Insurance does not accept OB Panel charge")
                    .When(a => a.FinCode == "L" && a.PrimaryInsuranceCode == "SEHZ");

                RuleFor(a => a.LmrpErrors)
                    .Empty().WithMessage("LMRP Rule Violation")
                    .When(a => a.FinCode == "A");

            });

        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

        private bool NotHaveUnusedDiagnosis(Account account)
        {

            foreach (var dx in account.Pat.Diagnoses)
            {
                bool dxUsed = false;
                foreach (var chrg in account.Charges.Where(x => x.IsCredited == false && x.FinancialType == "M"))
                {
                    foreach (var chrgDetail in chrg.ChrgDetails)
                    {
                        List<string> dxPtrs = new List<string>();

                        if (chrgDetail.DiagnosisPointer != null)
                            dxPtrs = chrgDetail.DiagnosisPointer.DiagnosisPointer.Split(':').ToList();

                        foreach(var ptr in dxPtrs)
                        {
                            if (!string.IsNullOrEmpty(ptr))
                            {
                                if (dx.No == Convert.ToInt32(ptr))
                                    dxUsed = true;
                            }
                        }
                    }
                }
                if (!dxUsed)
                    return false;
            }

            return true;
        }

        private bool NotContainOnlyVenipunctureCharge(Account account)
        {
            if (account.Cpt4List.Distinct().Count() == 1)
            {
                if (account.Cpt4List[0] == "36415")
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
            bool valid = true;
            foreach(var ins in account.Insurances)
            {
                if (ins.Relation == "01")
                {
                    if ((account.PatLastName ?? "") == (ins.HolderLastName ?? "") &&
                        (account.PatFirstName ?? "") == (ins.HolderFirstName ?? "") &&
                        (account.PatMiddleName ?? "") == (ins.HolderMiddleName ?? ""))
                    {
                        valid &= true;
                    }
                    else
                    {
                        valid &= false;
                    }
                }
            }

            return valid;
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

        private bool NotNeedRepeatModifier(List<Chrg> chrgs)
        {
            bool isOK = true;
            
            var list = chrgs.Where(x => x.IsCredited == false && x.ChrgDetails.Any(y => y.Cpt4 == "80202"));
            
            if (list.Count() > 1)
            {
                isOK = false;
                int cptCount = 0;
                foreach (var item in list)
                {
                    cptCount++;
                    var details = item.ChrgDetails;
                    if (cptCount == 1)
                    {
                        isOK = true;
                    }
                    else
                    {
                        isOK = false;
                        foreach (var detail in details)
                        {
                            if (detail.Modifier == "59")
                            {
                                isOK = true;
                            }
                        }
                    }
                }
            }

            return isOK;
        }

        private bool NotHaveDuplicateCdms(List<Chrg> chrgs)
        {

            Dictionary<string, bool> duplicates = new Dictionary<string, bool>() 
            {
                {"5686078", false },
                {"5686066", false } 
            };


            foreach(var chrg in chrgs.Where(x => !x.IsCredited))
            {
                if(duplicates.ContainsKey(chrg.CDMCode))
                {
                    duplicates[chrg.CDMCode] = true;
                }
            }

            bool isDuplicate = true;
            foreach (var item in duplicates)
            {
                if (item.Value == false)
                    isDuplicate = false;
            }

            return !isDuplicate;
        }

        private bool NotHaveBundledOBPanel(Account account)
        {
            var list = account.Charges.Where(c => c.CDMCode == "MCL0021" && c.IsCredited == false);

            if(list.Count() > 0)
            {
                return false;
            }

            return true;
        }

        private bool MatchChargeServiceDateAndAccountTransDate(Account account)
        {
            foreach(var chrg in account.Charges.Where(x => x.IsCredited == false && x.CDMCode != "CBILL"))
            {
                if(((DateTime)chrg.ServiceDate).Date != account.TransactionDate.Date && !chrg.IsCredited)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
