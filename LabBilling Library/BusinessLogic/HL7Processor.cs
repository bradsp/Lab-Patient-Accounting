using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using NHapi;
using NHapi.Base.Parser;
using NHapiTools;

namespace LabBilling.Core.BusinessLogic
{
    public class HL7Processor
    {
        

        public void Parse(string message)
        {
            var parser = new PipeParser();

            var parsed = parser.Parse(message);


        }


    }
}
