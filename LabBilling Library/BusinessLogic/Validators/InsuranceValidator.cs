using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LabBilling.Core.Models;

namespace LabBilling.Core.BusinessLogic.Validators
{
    public class InsuranceValidator : AbstractValidator<Ins>
    {
        public InsuranceValidator()
        {
            RuleFor(a => a.HolderLastName)
                .NotEmpty().WithMessage("Ins Holder Last Name is empty.");
            RuleFor(a => a.HolderFirstName)
                .NotEmpty().WithMessage("Ins Holder First Name is empty.");
            RuleFor(a => a.HolderAddress)
                .NotEmpty().WithMessage("Ins Holder Address is empty.");
            RuleFor(a => a.HolderCity)
                .NotEmpty().WithMessage("Ins Holder City is empty.");
            RuleFor(a => a.HolderState)
                .NotEmpty().WithMessage("Ins Holder State is empty.");
            RuleFor(a => a.HolderZip)
                .NotEmpty().WithMessage("Ins Holder Zip is empty.");
            RuleFor(a => a)
                .Must((a) =>
                {
                    if (string.IsNullOrEmpty(a.PolicyNumber) && string.IsNullOrEmpty(a.GroupNumber))
                        return false;
                    else
                        return true;
                }).WithMessage("Both Policy Number and Group Number are empty.");
            RuleFor(a => a.PolicyNumber)
                .Must(BeAValidPolicyNumber).WithMessage("Ins Policy Number is not correct format.")
                .When(a => !string.IsNullOrEmpty(a.PolicyNumber));
            RuleFor(a => a.GroupNumber)
                .Must(BeAValidGroupNumber).WithMessage("Ins Group Number is not a valid format.")
                .When(a => !string.IsNullOrEmpty(a.GroupNumber));
            RuleFor(a => a.PlanName)
                .Must(BeAValidName).WithMessage("Ins Plan Name contains invalid characters.")
                .NotEmpty().WithMessage("Ins Plan Name is empty.");
            RuleFor(a => a.InsCompany)
                .Must((insc) =>
                {
                    if (string.IsNullOrEmpty(insc.Address1) || string.IsNullOrEmpty(insc.CityStateZip))
                        return false;
                    else
                        return true;
                }).WithMessage("Plan must contain an address.");
            RuleFor(a => a.InsCompany.NThrivePayerNo)
                .NotEmpty().WithMessage("NThrive payer code is not defined for this payer.");
            RuleFor(a => a.Coverage)
                .Must((a) =>
                {
                    if (a != "A" && a != "B" && a != "C")
                        return false;
                    else
                        return true;
                }).WithMessage("Insurance coverage code is not valid.");
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
                bool hasCorrectLength = policyNum.Length >= 7 && policyNum.Length <= 12 ? true : false;
                return hasCorrectLength;
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
