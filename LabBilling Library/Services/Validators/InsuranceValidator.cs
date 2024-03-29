using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.Services.Validators
{
    public sealed class InsuranceValidator : AbstractValidator<Ins>
    {
        public InsuranceValidator()
        {
            RuleFor(a => a.InsCompany.IsDeleted)
                .Equal(false)
                .WithMessage(a => $"Account is using inactive insurance {a.InsCode}");
            RuleFor(a => a.InsCompany.BillForm)
                .NotEmpty().WithMessage(a => $"Insurance company's billing form is not defined. {a.InsCode} {a.PlanName}");
            RuleFor(a => a.HolderLastName)
                .NotEmpty().WithMessage(a => $"Ins Holder Last Name is empty. {a.InsCode} {a.PlanName}");
            RuleFor(a => a.HolderFirstName)
                .NotEmpty().WithMessage(a => $"Ins Holder First Name is empty. {a.InsCode} {a.PlanName}");
            RuleFor(a => a.HolderStreetAddress)
                .NotEmpty().WithMessage(a => $"Ins Holder Address is empty. {a.InsCode} {a.PlanName}");
            RuleFor(a => a.HolderCity)
                .NotEmpty().WithMessage(a => $"Ins Holder City is empty. {a.InsCode} {a.PlanName}");
            RuleFor(a => a.HolderState)
                .NotEmpty().WithMessage(a => $"Ins Holder State is empty. {a.InsCode} {a.PlanName}");
            RuleFor(a => a.HolderZip)
                .NotEmpty().WithMessage(a => $"Ins Holder Zip is empty. {a.InsCode} {a.PlanName}");
            RuleFor(a => a)
                .Must((a) =>
                {
                    if (string.IsNullOrEmpty(a.PolicyNumber) && string.IsNullOrEmpty(a.GroupNumber))
                        return false;
                    else
                        return true;
                }).WithMessage(a => $"Both Policy Number and Group Number are empty. {a.InsCode} {a.PlanName}");

            //RuleFor(a => a.PolicyNumber)
            //    .Must(BeAValidPolicyNumber).WithMessage("Ins Policy Number is not correct format.")
            //    .When(a => !string.IsNullOrEmpty(a.PolicyNumber));

            RuleFor(a => a.GroupNumber)
                .Must(BeAValidGroupNumber).WithMessage(a => $"Ins Group Number is not a valid format. {a.InsCode} {a.PlanName}")
                .When(a => !string.IsNullOrEmpty(a.GroupNumber));

            RuleFor(a => a.PlanName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(a => $"Ins Plan Name is empty. {a.Coverage} {a.InsCode} {a.PlanName}")
                .Must(BeAValidName).WithMessage("Ins Plan Name contains invalid characters.");

            RuleFor(a => a.InsCompany)
                .Must((insc) =>
                {
                    if (string.IsNullOrEmpty(insc.Address1) || string.IsNullOrEmpty(insc.CityStateZip))
                        return false;
                    else
                        return true;
                }).WithMessage(a => $"Plan must contain an address. {a.InsCode} {a.PlanName}")
                .Must((insc) =>
                {
                    if (string.IsNullOrWhiteSpace(insc.Zip))
                        return false;
                    else
                        return true;
                }).WithMessage(a => $"Plan does not have a zip code. {a.InsCode} {a.PlanName}")
                .When(a => !a.InsCompany.IsGenericPayor);

            RuleFor(a => a.PlanStreetAddress1)
                .NotEmpty().WithMessage(a => $"Plan address is empty. {a.InsCode} {a.PlanName}")
                .When(a => a.InsCompany.IsGenericPayor);

            RuleFor(a => a.PlanCity)
                .NotEmpty().WithMessage(a => $"Plan City is empty. {a.InsCode} {a.PlanName}")
                .When(a => a.InsCompany.IsGenericPayor);

            RuleFor(a => a.PlanZip)
                .NotEmpty().WithMessage(a => $"Plan state is empty. {a.InsCode} {a.PlanName}")
                .When(a => a.InsCompany.IsGenericPayor);

            RuleFor(a => a.InsCompany.NThrivePayerNo)
                .NotEmpty().WithMessage(a => $"NThrive payer code is not defined for payer {a.InsCode} {a.PlanName}");

            RuleFor(a => a.Coverage)
                .Must((a) =>
                {
                    if (a != "A" && a != "B" && a != "C")
                        return false;
                    else
                        return true;
                }).WithMessage("Insurance coverage code is not valid.");

            RuleFor(a => a.Relation)
                .NotEmpty().WithMessage(a => $"Relation is not defined for ins {a.Coverage} - {a.PlanName}");                
        }

        private bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }

        private bool BeAValidPolicyNumber(Ins ins, string policyNum)
        {
            if((ins.InsCode == "HUM" && ins.FinCode == "L")
                || (ins.InsCode == "HESP" && ins.FinCode == "H"))
            {
                //must be alphanumeric
                bool isAlphaNumeric = policyNum.All(Char.IsLetterOrDigit);
                //length must be >= 9
                bool hasCorrectLength = policyNum.Length == 9 || policyNum.Length == 11 ? true : false;

                return isAlphaNumeric && hasCorrectLength;
            }
            else if(ins.FinCode == "B")
            {
                bool hasCorrectPrefix = Char.IsLetter(policyNum[0]);
                bool hasCorrectLength = policyNum.Length <= 17;
                bool isAlphaNumeric = policyNum.Substring(1).All(Char.IsLetterOrDigit);

                return hasCorrectPrefix && hasCorrectLength && isAlphaNumeric;
            }
            else if(ins.FinCode == "D" && ins.InsCode == "TNBC")
            {
                bool hasCorrectPrefix = policyNum.StartsWith("ZEDM") || policyNum.StartsWith("ZECM");
                bool hasCorrectLength = policyNum.Length == 12 ? true : false;
                bool isAlphaNumeric = policyNum.Substring(4).All(Char.IsNumber);

                return hasCorrectLength && hasCorrectPrefix && isAlphaNumeric;
            }
            else if (ins.FinCode == "L" && (ins.InsCode == "SEHZ" || ins.InsCode == "UHC"))
            {
                bool hasCorrectLength = policyNum.Length == 9;
                bool hasCorrectFormat = policyNum.Count(Char.IsLetter) > 2;
                return hasCorrectLength && hasCorrectFormat;
            }
            else if (ins.FinCode == "L" && ins.InsCode == "WIN")
            {
                return policyNum.StartsWith("WX");
            }
            else if (ins.FinCode == "C")
            {
                return policyNum.All(Char.IsNumber);
            }
            else if(ins.InsCode == "SEHZ" 
                || ins.InsCode == "UHC" 
                || ins.InsCode == "CHAMPUS" 
                || ins.InsCode == "HESP" 
                || ins.InsCode == "SECP")
            {
                bool isNumeric = policyNum.All(Char.IsNumber);
                return isNumeric;
            }
            else
            {
                //bool hasCorrectLength = policyNum.Length >= 7 && policyNum.Length <= 12 ? true : false;
                return true;
            }
        }
        private bool BeAValidGroupNumber(Ins ins, string groupNumber)
        {
            if(ins.FinCode == "A" || ins.FinCode == "C")
            {
                return string.IsNullOrEmpty(groupNumber) ? true : false;
            }
            else if (ins.FinCode == "B")
            {
                return groupNumber.All(Char.IsLetterOrDigit);
            }
            else if (ins.FinCode == "L" &&  ins.InsCode == "HP")
            {
                return !string.IsNullOrEmpty(groupNumber);
            }

            return true;
        }
    }
}
