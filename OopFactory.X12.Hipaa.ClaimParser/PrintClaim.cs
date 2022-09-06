using OopFactory.X12.Hipaa.Claims.Services;
using OopFactory.X12.Parsing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace OopFactory.X12.Hipaa.ClaimParser
{
    public class PrintClaim
    {

        public bool ThrowExceptionOnSyntaxErrors { get; set; } = false;

        public void Print(string x12Text, bool makeXml, bool makePdf)
        {
            bool throwException = ThrowExceptionOnSyntaxErrors;

            //var opts = new ExecutionOptions(args);
            InstitutionalClaimToUB04ClaimFormTransformation institutionalClaimToUB04ClaimFormTransformation =
                new InstitutionalClaimToUB04ClaimFormTransformation("UB04_Red.gif");
            var service = new ClaimFormTransformationService(
                new ProfessionalClaimToHcfa1500FormTransformation("HCFA1500_Red.gif"),
                institutionalClaimToUB04ClaimFormTransformation,
                new DentalClaimToJ400FormTransformation("ADAJ400_Red.gif"),
                new X12Parser(throwException));

            try
            {
//#if DEBUG
//                var parser = new X12Parser();
//                var interchange = parser.ParseMultiple(stream).First();
//                File.WriteAllText(filename + ".dat", interchange.SerializeToX12(true));
//#endif
                DateTime start = DateTime.Now;

                System.IO.MemoryStream mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(x12Text));

                Dictionary<string, string> revenueDictionary = new Dictionary<string, string>();
                revenueDictionary["0572"] = "Test Code";
                service.FillRevenueCodeDescriptionMapping(revenueDictionary);

                var claimDoc = service.Transform837ToClaimDocument(mStream);
                institutionalClaimToUB04ClaimFormTransformation.PerPageTotalChargesView = true;

                if (makeXml)
                {
                    string outputFilename = $"c:\\temp\\testx12toxml.xml";

                    string xml = claimDoc.Serialize();
                    xml = xml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
                    File.WriteAllText(outputFilename, xml);
                }

                if (makePdf)
                {
                    string outputFilename = $"c:\\temp\\testclaimform.pdf";
                    using (FileStream pdfOutput = new FileStream(outputFilename, FileMode.Create, FileAccess.Write))
                    {
                        XmlDocument foDoc = new XmlDocument();
                        string foXml = service.TransformClaimDocumentToFoXml(claimDoc);
                        foDoc.LoadXml(foXml);

                        var driver = Fonet.FonetDriver.Make();
                        driver.Render(foDoc, pdfOutput);
                        pdfOutput.Close();
                    }
                }

                //opts.WriteLine(string.Format("{0} parsed in {1}.", filename, DateTime.Now - start));
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Exception occurred: {exc.GetType()}.  {exc.Message}.", exc);
            }

        }
    }
}
