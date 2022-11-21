using System;
using System.Diagnostics;
using LabBilling.Core.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace LabBilling.Core.BusinessLogic
{
    public class InvoicePrintPdfSharp
    {
        private Document document = null;
        private InvoiceModel model = null;
        private TextFrame addressFrame = null;
        private TextFrame invoiceFrame = null;
        private TextFrame clientAddressFrame = null;
        private Table table = null;
        private Section section = null;

        public InvoicePrintPdfSharp()
        {

        }

        public void CreateInvoicePdf(InvoiceModel model, string outFilePath)
        {
            if (model.StatementType != InvoiceModel.StatementTypeEnum.Invoice)
                throw new ArgumentOutOfRangeException("InvoiceModel.StatementType", "Expected an invoice model.");

            this.model = model;

            this.document = new Document();
            this.document.Info.Title = "Invoice";
            this.document.Info.Subject = $"Invoice for {model.ClientName}";
            this.document.Info.Author = $"{model.BillingCompanyName}";

            DefineStyles();

            CreateInvoicePage();

            FillInvoiceContent();

            const bool unicode = false;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode);

            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save(outFilePath);

            return;
        }

        public void CreateStatementPdf(InvoiceModel model, string outFilePath)
        {
            if (model.StatementType != InvoiceModel.StatementTypeEnum.Statement)
                throw new ArgumentOutOfRangeException("InvoiceModel.StatementType", "Expected a statement type model.");

            this.model = model;
            this.document = new Document();
            this.document.Info.Title = "Statement";
            this.document.Info.Subject = $"Statement for {model.ClientName}";
            this.document.Info.Author = $"{model.BillingCompanyName}";

            DefineStyles();

            CreateStatementPage();

            FillStatementContent();

            const bool unicode = false;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode);

            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save(outFilePath);

            return;
        }

        private void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Calibri";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Calibri";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        private void CreateHeaderFooter()
        {
            // Each MigraDoc document needs at least one section.
            section = this.document.AddSection();

            // Put a logo in the header
            Image image = section.Headers.Primary.AddImage(model.ImageFilePath);
            image.Width = "2.5in";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText($"{model.BillingCompanyName} · {model.BillingCompanyAddress} · {model.BillingCompanyCity} · {model.BillingCompanyState} {model.BillingCompanyZipCode} · {model.BillingCompanyPhone}");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            //Create the text frame for the company address
            clientAddressFrame = section.AddTextFrame();
            clientAddressFrame.Width = "7.0cm";
            clientAddressFrame.Height = "3.0cm";
            clientAddressFrame.Left = ShapePosition.Left;
            clientAddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            clientAddressFrame.RelativeVertical = RelativeVertical.Page;
            clientAddressFrame.Top = "1.0cm";

            // Create the text frame for the client address
            this.addressFrame = section.AddTextFrame();
            this.addressFrame.Height = "3.0cm";
            this.addressFrame.Width = "7.0cm";
            this.addressFrame.Left = ShapePosition.Left;
            this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.addressFrame.Top = "3.1cm";
            this.addressFrame.RelativeVertical = RelativeVertical.Page;

            // Put sender in address frame
            paragraph = clientAddressFrame.AddParagraph();
            paragraph.AddText(model.BillingCompanyName);
            paragraph.AddLineBreak();
            paragraph.AddText(model.BillingCompanyAddress);
            paragraph.AddLineBreak();
            paragraph.AddText($"{model.BillingCompanyCity}, {model.BillingCompanyState} {model.BillingCompanyZipCode}");
            paragraph.AddLineBreak();
            paragraph.AddText(model.BillingCompanyPhone);

            paragraph.Format.Font.Name = "Calibri";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;

            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "3cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText(model.StatementType == InvoiceModel.StatementTypeEnum.Invoice ? "INVOICE" : "STATEMENT", TextFormat.Bold);
            //paragraph.AddTab();
            ////paragraph.AddText(model.BillingCompanyCity);
            //paragraph.AddDateField(model.InvoiceDate.ToShortDateString());
        }

        private void CreateInvoicePage()
        {
            CreateHeaderFooter();

            // create text frame for invoice information
            this.invoiceFrame = section.AddTextFrame();
            this.invoiceFrame.Height = "3.0cm";
            this.invoiceFrame.Width = "2.5in";
            this.invoiceFrame.Left = "4.0in";
            this.invoiceFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.invoiceFrame.Top = "3.1cm";
            this.invoiceFrame.RelativeVertical = RelativeVertical.Page;

            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = Color.Parse("Blue");
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            //account
            Column column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            //quantity
            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            //cdm
            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            //description
            column = this.table.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            //item amount
            column = this.table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            //account total amount
            column = this.table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Color.Parse("LightBlue");
            //account no
            row.Cells[0].AddParagraph("Account");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[0].MergeDown = 1;
            //date of service 
            row.Cells[1].AddParagraph("Date of Service");
            row.Cells[1].Format.Font.Bold = true;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;
            //patient name
            row.Cells[2].AddParagraph("Patient Name");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[2].MergeRight = 2;
            //account total
            row.Cells[5].AddParagraph("Account Total");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
            row.Cells[5].MergeDown = 1;

            row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Color.Parse("LightBlue");
            //date of Service
            row.Cells[0].AddParagraph("Date of Service");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            
            //quantity
            row.Cells[1].AddParagraph("Quantity");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;
            //cdm
            row.Cells[2].AddParagraph("CDM");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;
            //description
            row.Cells[3].AddParagraph("Description");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Bottom;
            //amount
            row.Cells[4].AddParagraph("Amount");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[4].VerticalAlignment = VerticalAlignment.Bottom;

            this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        }

        private void FillInvoiceContent()
        {
            // Fill address in address text frame
            
            Paragraph paragraph = this.addressFrame.AddParagraph();
            paragraph.AddText(model.ClientName);
            paragraph.AddLineBreak();
            paragraph.AddText(model.Address1);
            paragraph.AddLineBreak();
            paragraph.AddText($"{model.City}, {model.State} {model.ZipCode}");

            paragraph = this.invoiceFrame.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddText("Invoice:  ");
            paragraph.AddFormattedText(model.InvoiceNo, TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText("Invoice Date:  ");
            paragraph.AddFormattedText(model.InvoiceDate.ToShortDateString(), TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText("Invoice Total: ");
            paragraph.AddFormattedText(model.InvoiceTotal.ToString("0.00"), TextFormat.Bold);
            
            // Iterate the invoice items

            foreach (var detail in model.InvoiceDetails)
            {
                if (detail.AccountTotal == 0)
                    continue;

                int detailLines = detail.InvoiceDetailLines.Count;


                // Each item fills two rows
                Row row1 = this.table.AddRow();
                row1.TopPadding = 1.5;
                //account no
                row1.Cells[0].Shading.Color = Color.Parse("LightGray");
                row1.Cells[0].VerticalAlignment = VerticalAlignment.Top;
                row1.Cells[0].MergeDown = detailLines;
                //date of service
                //row1.Cells[1].Shading.Color = Color.Parse("LightGray");
                row1.Cells[1].VerticalAlignment = VerticalAlignment.Top;                
                //patient name
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                row1.Cells[2].MergeRight = 2;
                //Account total
                row1.Cells[5].Shading.Color = Color.Parse("LightGray");
                row1.Cells[5].MergeDown = detailLines;

                //fill account
                row1.Cells[0].AddParagraph(detail.Account);
                //fill date of service
                paragraph = row1.Cells[1].AddParagraph();
                paragraph.AddText(detail.ServiceDate.ToShortDateString());
                //fill patient name
                paragraph = row1.Cells[2].AddParagraph();
                paragraph.AddFormattedText(detail.PatientName, TextFormat.Bold);


                foreach(var detailLine in detail.InvoiceDetailLines)
                {
                    Row newRow = this.table.AddRow();

                    newRow.Cells[1].AddParagraph(detailLine.Qty.ToString());
                    newRow.Cells[2].AddParagraph(detailLine.CDM.ToString());
                    newRow.Format.Font.Size = 8;

                    paragraph = newRow.Cells[3].AddParagraph();
                    paragraph.AddText(detailLine.Description);
                    if (model.ShowCpt)
                        paragraph.AddText($" ({detailLine.CPT})");
                    
                    newRow.Cells[4].AddParagraph(detailLine.Amount.ToString("0.00"));
                }

                row1.Cells[5].AddParagraph(detail.AccountTotal.ToString("0.00"));

                row1.Cells[5].VerticalAlignment = VerticalAlignment.Top;

                this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
            }

            // Add an invisible row as a space line to the table
            Row row = this.table.AddRow();
            row.Borders.Visible = false;

            // Add the total due row
            row = this.table.AddRow();
            row.Cells[0].AddParagraph("Total Due");
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(model.InvoiceTotal.ToString("0.00"));

            // Set the borders of the specified cell range
            this.table.SetEdge(5, this.table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);

            // Add the notes paragraph
            //paragraph = this.document.LastSection.AddParagraph();
            //paragraph.Format.SpaceBefore = "1cm";
            //paragraph.Format.Borders.Width = 0.75;
            //paragraph.Format.Borders.Distance = 3;
            //paragraph.Format.Borders.Color = Color.Parse("Blue");
            //paragraph.Format.Shading.Color = Color.Parse("LightGray");
            //paragraph.AddText();
        }

        private void CreateStatementPage()
        {
            CreateHeaderFooter();

            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            this.table.Borders.Color = Color.Parse("Blue");
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns

            //service date
            Column column = this.table.AddColumn("1in");
            column.Format.Alignment = ParagraphAlignment.Left;

            //reference
            column = this.table.AddColumn("1in");
            column.Format.Alignment = ParagraphAlignment.Left;

            //description
            column = this.table.AddColumn("3in");
            column.Format.Alignment = ParagraphAlignment.Left;

            //amount
            column = this.table.AddColumn("1in");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Color.Parse("LightBlue");
            //Service date
            row.Cells[0].AddParagraph("Date");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Bottom;
            //Reference
            row.Cells[1].AddParagraph("Reference");
            row.Cells[1].Format.Font.Bold = true;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Bottom;
            //description
            row.Cells[2].AddParagraph("Description");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Bottom;
            //amount
            row.Cells[3].AddParagraph("Amount");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Bottom;

            this.table.SetEdge(0, 0, this.table.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        }

        private void FillStatementContent()
        {
            // Fill address in address text frame

            Paragraph paragraph = this.addressFrame.AddParagraph();
            paragraph.AddText(model.ClientName);
            paragraph.AddLineBreak();
            paragraph.AddText(model.Address1);
            paragraph.AddLineBreak();
            paragraph.AddText($"{model.City}, {model.State} {model.ZipCode}");

            // add a balance forward line
            Row row = this.table.AddRow();
            row.TopPadding = 1.5;
            row.Cells[2].AddParagraph("Balance Forward");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[3].AddParagraph(model.BalanceForward.ToString("0.00"));
            
            // Iterate the invoice items
            foreach (var detail in model.ClientStatementDetails)
            {
                Row row1 = this.table.AddRow();
                row1.TopPadding = 1.5;
                //date
                row1.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                //reference
                row1.Cells[1].VerticalAlignment = VerticalAlignment.Top;
                //description
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                //amount
                row1.Cells[3].Format.Alignment = ParagraphAlignment.Right;

                //fill date of service
                row1.Cells[0].AddParagraph(detail.ServiceDate.ToShortDateString());
                //reference
                row1.Cells[1].AddParagraph(detail.Reference ?? string.Empty);
                //fill description
                row1.Cells[2].AddParagraph(detail.Description ?? string.Empty);
                //amount
                if (detail.Amount < 0.00)
                    row1.Cells[3].Format.Font.Color = Color.Parse("Red");
                row1.Cells[3].AddParagraph(detail.Amount.ToString("0.00"));

                this.table.SetEdge(0, this.table.Rows.Count - 1, table.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75);
            }

            // Add an invisible row as a space line to the table
            row = this.table.AddRow();
            row.Borders.Visible = false;

            // Add the total due row
            row = this.table.AddRow();
            row.Cells[0].AddParagraph("Total Due");
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 2;
            row.Cells[3].AddParagraph(model.BalanceDue.ToString("0.00"));

            // Set the borders of the specified cell range
            this.table.SetEdge(3, this.table.Rows.Count - 4, 1, 2, Edge.Box, BorderStyle.Single, 0.75);

            // Add the notes paragraph
            //paragraph = this.document.LastSection.AddParagraph();
            //paragraph.Format.SpaceBefore = "1cm";
            //paragraph.Format.Borders.Width = 0.75;
            //paragraph.Format.Borders.Distance = 3;
            //paragraph.Format.Borders.Color = Color.Parse("Blue");
            //paragraph.Format.Shading.Color = Color.Parse("LightGray");
            //paragraph.AddText();
        }


        public static void PrintPdf(string pdfFileName)
        {
            string processFilename = Microsoft.Win32.Registry.LocalMachine
                 .OpenSubKey("Software")
                 .OpenSubKey("Microsoft")
                 .OpenSubKey("Windows")
                 .OpenSubKey("CurrentVersion")
                 .OpenSubKey("App Paths")
                 .OpenSubKey("AcroRd32.exe")
                 .GetValue(String.Empty).ToString();

            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = processFilename;
            info.Arguments = String.Format("/p /h {0}", pdfFileName);
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            //(It won't be hidden anyway... thanks Adobe!)
            info.UseShellExecute = false;

            Process p = Process.Start(info);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            int counter = 0;
            while (!p.HasExited)
            {
                System.Threading.Thread.Sleep(1000);
                counter += 1;
                if (counter == 5) break;
            }
            if (!p.HasExited)
            {
                p.CloseMainWindow();
                p.Kill();
            }
        }
    }
}
