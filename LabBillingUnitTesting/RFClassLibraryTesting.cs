using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFClassLibrary;
using Xunit;

namespace LabBillingUnitTesting
{
    public class RFClassLibraryTesting
    {
        [Theory]
        [InlineData("0001234567", "1234567")]
        [InlineData("003216547", "3216547")]
        [InlineData("023176541", "23176541")]
        public void StripZerosTest(string source, string expected)
        {

            string returned = StringExtensions.StripZeros(source);

            Assert.Equal(expected, returned);

        }

        [Theory]
        [InlineData("123456789", "123-45-6789")]
        public void FormatSSNTest(string source, string expected)
        {
            string returned = source.FormatSSN();

            Assert.Equal(returned, expected);
        }

    }
}
