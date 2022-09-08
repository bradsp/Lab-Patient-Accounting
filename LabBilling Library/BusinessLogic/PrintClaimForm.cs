using OopFactory.X12.Hipaa.Claims.Services;
using OopFactory.X12.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System.Reflection;
using Fonet.Pdf;
using iText.Kernel.Pdf;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using iText.Forms;
using iText.Forms.Fields;

namespace LabBilling.Core.BusinessLogic
{
    public class PrintClaimForm
    {

        public bool ThrowExceptionOnSyntaxErrors { get; set; } = false;

        private const string ub04FormFileName = "UB04_Red.gif";
        private const string hcfa1500FormFileName = "HCFA1500_Red.gif";
        private const string adaFormFileName = "ADAJ400_Red.gif";
        private readonly string _connectionString;

        public PrintClaimForm(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void PrintAlt(string x12Text)
        {
            //string src = @"c:\temp\cms1500_form.pdf";
            string src = @"c:\temp\ub04-claim-form.pdf";

            string dest = @"c:\temp\test_ub.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));


            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);

            IDictionary<string, PdfFormField> fields = form.GetFormFields();
            //PdfFormField toSet;

            

            pdfDoc.Close();
        }

        public void Print(string x12Text, bool makeXml, bool makePdf)
        {

            string ub04FormPath = Path.Combine(Environment.CurrentDirectory, "BusinessLogic\\Resources\\", ub04FormFileName);
            string hcfa1500FormPath = Path.Combine(Environment.CurrentDirectory, "BusinessLogic\\Resources\\", hcfa1500FormFileName);
            string adaFormPath = Path.Combine(Environment.CurrentDirectory, "BusinessLogic\\Resources\\", adaFormFileName);

            bool throwException = ThrowExceptionOnSyntaxErrors;

            InstitutionalClaimToUB04ClaimFormTransformation institutionalClaimToUB04ClaimFormTransformation =
                new InstitutionalClaimToUB04ClaimFormTransformation(ub04FormPath);
            var service = new ClaimFormTransformationService(
                new ProfessionalClaimToHcfa1500FormTransformation(hcfa1500FormPath),
                institutionalClaimToUB04ClaimFormTransformation,
                new DentalClaimToJ400FormTransformation(adaFormPath),
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

                RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(_connectionString);

                var revCodes = revenueCodeRepository.GetAll();

                Dictionary<string, string> revenueDictionary = new Dictionary<string, string>();
                foreach (RevenueCode revCode in revCodes)
                {
                    revenueDictionary[revCode.Code] = revCode.Description;
                }
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
                    string path = $"c:\\temp\\";
                    string fileName = $"claim{DateTime.Now.ToString("yyyymmddhhmmss")}.pdf";

                    string outputFilename = $"{path}{fileName}";
                    using (FileStream pdfOutput = new FileStream(outputFilename, FileMode.Create, FileAccess.Write))
                    {
                        XmlDocument foDoc = new XmlDocument();
                        string foXml = service.TransformClaimDocumentToFoXml(claimDoc);
                        foDoc.LoadXml(foXml);

                        var driver = Fonet.FonetDriver.Make();
                        driver.Render(foDoc, pdfOutput);
                        pdfOutput.Close();
                        System.Diagnostics.Process.Start(outputFilename);
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
