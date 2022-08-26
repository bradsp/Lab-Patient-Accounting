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
                .NotEmpty();
            RuleFor(a => a.HolderFirstName)
                .NotEmpty();
            RuleFor(a => a.PolicyNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidPolicyNumber);
            RuleFor(a => a.GroupNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeAValidGroupNumber);
            RuleFor(a => a.PlanName)
                .Must(BeAValidName)
                .NotEmpty();
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
