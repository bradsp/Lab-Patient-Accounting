using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core.Models;
using Xunit;

namespace LabBillingUnitTesting
{
    public class ExpressionBuilderTest
    {
        [Fact]
        public void ShouldBuildExpression()
        {
            // Arrange - set up variables and data
            int expected = 1;

            // Act
            int actual = 1;

            // Assert
            Assert.Equal(expected, actual);     
        }

        [Fact]
        public void EvaluateTest()
        {
            Account account = new Account();
            account.Insurances = new List<Ins>()
            {
                new Ins { InsCode = "TC" },
                new Ins { InsCode = "BC"}
            };
            account.Charges = new List<Chrg>();
            Chrg myChrg = new Chrg();
            myChrg.cdm = "5525154";
            myChrg.ChrgDetails = new List<ChrgDetail>()
            {
                new ChrgDetail { cpt4 = "85100"}
            };
            account.Charges.Add(myChrg);

            var insCodeMatch = account.Insurances.All(x => x.InsCode == "WIN");
            var cptMatch = account.Charges.All(x => x.ChrgDetails.All(y => y.cpt4 == "80101"));

            Assert.False(insCodeMatch);

            Assert.False(cptMatch);


        }

    }
}
