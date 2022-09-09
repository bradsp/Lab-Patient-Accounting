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

        public void PrintAlt(ClaimData claim)
        {
            //string src = @"c:\temp\cms1500_form.pdf";
            string src = @"c:\temp\ub04-claim-form.pdf";

            string dest = @"c:\temp\test_ub.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));


            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);

            IDictionary<string, PdfFormField> fields = form.GetFormFields();

            SetFieldValue(fields, "box1a", claim.BillingProviderName);
            SetFieldValue(fields, "box1b", claim.BillingProviderAddress);
            SetFieldValue(fields, "box1c", $"{claim.BillingProviderCity} {claim.BillingProviderState} {claim.BillingProviderZipCode}");
            SetFieldValue(fields, "box1d", claim.BillingProviderContactPhone);

            SetFieldValue(fields, "box3a", claim.claimAccount.AccountNo);
            SetFieldValue(fields, "box4", $"{claim.FacilityCode}{claim.FacilityCodeQualifier}");
            SetFieldValue(fields, "box5", claim.BillingProviderTaxId);
            SetFieldValue(fields, "box6a", ((DateTime)claim.StatementFromDate).ToString("MMddyy"));
            SetFieldValue(fields, "box6b", ((DateTime)claim.StatementThruDate).ToString("MMddyy"));
            SetFieldValue(fields, "box8b", claim.claimAccount.PatFullName);
            SetFieldValue(fields, "box9a", claim.claimAccount.Pat.Address1);
            SetFieldValue(fields, "box9b", claim.claimAccount.Pat.City);
            SetFieldValue(fields, "box9c", claim.claimAccount.Pat.State);
            SetFieldValue(fields, "box9d", claim.claimAccount.Pat.ZipCode);
            SetFieldValue(fields, "box10", ((DateTime)claim.claimAccount.Pat.BirthDate).ToString("MMddyyyy"));
            SetFieldValue(fields, "box11", claim.claimAccount.Pat.Sex);


            SetFieldValue(fields, "38a", claim.claimAccount.Pat.GuarantorFullName);
            SetFieldValue(fields, "38b", claim.claimAccount.Pat.GuarantorAddress);
            SetFieldValue(fields, "38c", $"{claim.claimAccount.Pat.City}, {claim.claimAccount.Pat.State} {claim.claimAccount.Pat.ZipCode}");
            SetFieldValue(fields, $"box56", ""); //phy npi

            int j = 0;
            foreach(var subscriber in claim.Subscribers)
            {
                SetFieldValue(fields, $"box50a.{j}", subscriber.PayerName);
                SetFieldValue(fields, $"box51a.{j}", subscriber.PayerIdentifier);
                SetFieldValue(fields, $"box52a.{j}", "Y"); //rel info
                SetFieldValue(fields, $"box53a.{j}", ""); //assign benefits
                SetFieldValue(fields, $"box54a.{j}", ""); //prior payments
                SetFieldValue(fields, $"box55a.{j}", ""); //est amt due
                SetFieldValue(fields, $"box58a.{j}", $"{subscriber.LastName},{subscriber.FirstName} {subscriber.MiddleName}"); //insureds name
                SetFieldValue(fields, $"box59a.{j}", subscriber.IndividualRelationshipCode); // patient relation
                SetFieldValue(fields, $"box60a.{j}", subscriber.PrimaryIdentifier); //insureds unique id
                SetFieldValue(fields, $"box61a.{j}", ""); //group name
                SetFieldValue(fields, $"box62a.{j}", ""); // ins group number
                SetFieldValue(fields, $"box63.{j}", ""); //treatment authorization codes
                SetFieldValue(fields, $"box64.{j}", ""); //document control number
                SetFieldValue(fields, $"box65.{j}", ""); //employer name

                j++;
            }

            int k = 1;
            SetFieldValue(fields, $"box66", "X"); //dx type flag
            foreach (var dx in claim.claimAccount.Pat.Diagnoses)
            {
                switch (k)
                {
                    case 1:
                        SetFieldValue(fields, $"box67", dx.Code); //dx codes
                        break;
                    case 2:
                        SetFieldValue(fields, $"box67a", dx.Code);
                        break;
                    case 3:
                        SetFieldValue(fields, $"box67b", dx.Code);
                        break;
                    case 4:
                        SetFieldValue(fields, $"box67c", dx.Code);
                        break;
                    case 5:
                        SetFieldValue(fields, $"box67d", dx.Code);
                        break;
                    case 6:
                        SetFieldValue(fields, $"box67e", dx.Code);
                        break;
                    case 7:
                        SetFieldValue(fields, $"box67f", dx.Code);
                        break;
                    case 8:
                        SetFieldValue(fields, $"box67g", dx.Code);
                        break;
                    case 9:
                        SetFieldValue(fields, $"box67h", dx.Code);
                        break;
                    default:
                        break;
                }
                k++;
            }

            int i = 1;
            foreach(var line in claim.ClaimLines)
            {
                SetFieldValue(fields, $"box42.{i}", line.RevenueCode);
                SetFieldValue(fields, $"box43.{i}", line.RevenueCodeDescription);
                SetFieldValue(fields, $"box44.{i}", $"{line.ProcedureCode}{line.ProcedureModifier1}{line.ProcedureModifier2}{line.ProcedureModifier3}");
                SetFieldValue(fields, $"box45.{i}", ((DateTime)line.ServiceDate).ToString("MMddyy"));
                SetFieldValue(fields, $"box46.{i}", "EA");
                SetFieldValue(fields, $"box47.{i}", line.Amount.ToString("F2"));
                i++;
            }
            SetFieldValue(fields, $"covered_totals", claim.TotalChargeAmount.ToString("F2"));


            pdfDoc.Close();
        }

        private void SetFieldValue(IDictionary<string, PdfFormField> fields, string fieldName, string value)
        {
            fields.TryGetValue(fieldName, out PdfFormField field);
            field.SetValue(value);
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
