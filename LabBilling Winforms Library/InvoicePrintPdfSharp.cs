using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using MathNet.Numerics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using BorderStyle = MigraDoc.DocumentObjectModel.BorderStyle;
using Color = MigraDoc.DocumentObjectModel.Color;
using TabAlignment = MigraDoc.DocumentObjectModel.TabAlignment;

namespace LabBilling.Core.Services;

public class InvoicePrintPdfSharp
{
    private Document _document;
    private InvoiceModel _model;
    private TextFrame _addressFrame;
    private TextFrame _invoiceFrame;
    private TextFrame _clientAddressFrame;
    private TextFrame _tableCaptionFrameLeft;
    private TextFrame _tableCaptionFrameRight;
    private Table _table;
    private Section _section;

    private readonly string _filePath;
    private readonly AppEnvironment _appEnvironment;

    public InvoicePrintPdfSharp(AppEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;

        _filePath = _appEnvironment.ApplicationParameters.InvoiceFileLocation;
        _document = new Document();
        _model = new InvoiceModel();
        _addressFrame = new TextFrame();
        _invoiceFrame = new TextFrame();
        _clientAddressFrame = new TextFrame();
        _tableCaptionFrameLeft = new TextFrame();
        _tableCaptionFrameRight = new TextFrame();
        _table = new Table();
        _section = new Section();
    }

    public string PrintInvoice(string invoiceNo)
    {
        using UnitOfWorkMain unitOfWork = new(_appEnvironment);
        InvoiceModel? invoiceModel = new();

        InvoiceHistory invoiceHistory = unitOfWork.InvoiceHistoryRepository.GetByInvoice(invoiceNo);

        string xml = invoiceHistory.InvoiceData;
        xml = xml.Replace("&#x0;", "");
        XmlSerializer serializer = new(typeof(InvoiceModel));
        StringReader rdr = new(xml);

        invoiceModel = serializer.Deserialize(rdr) as InvoiceModel;

        invoiceModel.ImageFilePath = _appEnvironment.ApplicationParameters.InvoiceLogoImagePath;

        string filename = "";
        //only print an invoice if there are invoice lines to print.
        if (invoiceModel.InvoiceDetails.Count > 0)
        {
            filename = CreateInvoicePdf(invoiceModel);
        }

        return filename;
    }

    public static string MergeFiles(IEnumerable<string> files, string outputFilename, bool duplex = false)
    {
        PdfDocument outputDocument = new PdfDocument();

        foreach (string file in files)
        {
            PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

            int count = inputDocument.PageCount;

            for (int idx = 0; idx < count; idx++)
            {
                PdfPage page = inputDocument.Pages[idx];
                outputDocument.AddPage(page);
            }
            if (count.IsOdd() && duplex)
            {
                //add a blank page
                outputDocument.AddPage();
            }
        }

        outputDocument.Save(outputFilename);

        return outputFilename;
    }

    public string CreateInvoicePdf(InvoiceModel model)
    {
        if (model.StatementType != InvoiceModel.StatementTypeEnum.Invoice)
            throw new ArgumentOutOfRangeException("InvoiceModel.StatementType", "Expected an invoice model.");

        this._model = model;

        this._document = new Document();
        this._document.Info.Title = "Invoice";
        this._document.Info.Subject = $"Invoice for {model.ClientName}";
        this._document.Info.Author = $"{model.BillingCompanyName}";

        DefineStyles();

        CreateInvoicePage();

        FillInvoiceContent();

        PdfDocumentRenderer pdfRenderer = new()
        {
            Document = _document
        };

        pdfRenderer.RenderDocument();
        string filename = $"{_filePath}\\Invoice-{model.ClientMnem}-{model.InvoiceNo}.pdf";
        pdfRenderer.PdfDocument.Save(filename);

        return filename;
    }

    public string CreateStatementPdf(InvoiceModel model)
    {
        if (model.StatementType != InvoiceModel.StatementTypeEnum.Statement)
            throw new ArgumentOutOfRangeException("InvoiceModel.StatementType", "Expected a statement type model.");

        this._model = model;

        _document = new Document();
        _document.Info.Title = "Statement";
        _document.Info.Subject = $"Statement for {model.ClientName}";
        _document.Info.Author = $"{model.BillingCompanyName}";

        DefineStyles();

        CreateStatementPage();

        FillStatementContent();

        PdfDocumentRenderer pdfRenderer = new()
        {
            Document = _document
        };

        pdfRenderer.RenderDocument();
        string filename = $"{_filePath}\\Statement-{model.ClientMnem}-{DateTime.Today:yyyyMMdd}.pdf";
        pdfRenderer.PdfDocument.Save(filename);

        return filename;
    }

    private void DefineStyles()
    {
        // Get the predefined style Normal.
        Style style = _document.Styles["Normal"];
        // Because all styles are derived from Normal, the next line changes the 
        // font of the whole document. Or, more exactly, it changes the font of
        // all styles and paragraphs that do not redefine the font.
        style.Font.Name = "Calibri";

        style = _document.Styles[StyleNames.Header];
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        style = _document.Styles[StyleNames.Footer];
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        // Create a new style called Table based on style Normal
        style = this._document.Styles.AddStyle("Table", "Normal");
        style.Font.Name = "Calibri";
        style.Font.Size = 9;

        // Create a new style called Reference based on style Normal
        style = _document.Styles.AddStyle("Reference", "Normal");
        style.ParagraphFormat.SpaceBefore = "5mm";
        style.ParagraphFormat.SpaceAfter = "5mm";
        style.ParagraphFormat.TabStops.AddTabStop("16cm", MigraDoc.DocumentObjectModel.TabAlignment.Right);
    }

    private void CreateHeaderFooter()
    {

        //section.PageSetup.DifferentFirstPageHeaderFooter = true;

        // Put a logo in the header
        MigraDoc.DocumentObjectModel.Shapes.Image image = _section.Headers.Primary.AddImage(_model.ImageFilePath);
        image.Width = "2in";
        image.LockAspectRatio = true;
        image.RelativeVertical = RelativeVertical.Line;
        image.RelativeHorizontal = RelativeHorizontal.Margin;
        image.Top = ShapePosition.Top;
        image.Left = ShapePosition.Left;
        image.WrapFormat.Style = WrapStyle.TopBottom;

        //Create the text frame for the company address
        _clientAddressFrame = _section.Headers.Primary.AddTextFrame();
        _clientAddressFrame.Width = "7.0cm";
        _clientAddressFrame.Height = "3.0cm";
        _clientAddressFrame.Left = "5.5in";
        _clientAddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
        _clientAddressFrame.RelativeVertical = RelativeVertical.Page;
        _clientAddressFrame.Top = "1.0cm";

        // Put sender in address frame
        Paragraph paragraph = _clientAddressFrame.AddParagraph();
        paragraph.AddText(_model.BillingCompanyName);
        paragraph.AddLineBreak();
        paragraph.AddText(_model.BillingCompanyAddress);
        paragraph.AddLineBreak();
        paragraph.AddText($"{_model.BillingCompanyCity}, {_model.BillingCompanyState} {_model.BillingCompanyZipCode}");
        paragraph.AddLineBreak();
        paragraph.AddText(_model.BillingCompanyPhone);

        paragraph.Format.Font.Name = "Calibri";
        paragraph.Format.Font.Size = 7;
        paragraph.Format.SpaceAfter = 3;


        // Create the text frame for the client address
        _addressFrame = _section.Headers.Primary.AddTextFrame();
        _addressFrame.Height = "2.0cm";
        _addressFrame.Width = "7.0cm";
        _addressFrame.Left = ShapePosition.Left;
        _addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
        _addressFrame.Top = "5.5cm";
        _addressFrame.RelativeVertical = RelativeVertical.Page;

        //create a text frame for the statement type, date, and page
        _tableCaptionFrameLeft = _section.Headers.Primary.AddTextFrame();
        _tableCaptionFrameLeft.Height = "1.0cm";
        _tableCaptionFrameLeft.Width = "6.0cm";
        _tableCaptionFrameLeft.Left = ShapePosition.Left;
        _tableCaptionFrameLeft.RelativeHorizontal = RelativeHorizontal.Margin;
        _tableCaptionFrameLeft.Top = "8cm";
        _tableCaptionFrameLeft.RelativeVertical = RelativeVertical.Page;

        _tableCaptionFrameRight = _section.Headers.Primary.AddTextFrame();
        _tableCaptionFrameRight.Height = "1.0cm";
        _tableCaptionFrameRight.Width = "6.0cm";
        _tableCaptionFrameRight.Left = "10.5cm";
        _tableCaptionFrameRight.RelativeHorizontal = RelativeHorizontal.Margin;
        _tableCaptionFrameRight.Top = "8cm";
        _tableCaptionFrameRight.RelativeVertical = RelativeVertical.Page;

        // Add the print date field
        paragraph = _tableCaptionFrameLeft.AddParagraph();
        //paragraph.Format.SpaceBefore = "9cm";
        paragraph.Style = "Reference";
        paragraph.AddFormattedText(_model.StatementType == InvoiceModel.StatementTypeEnum.Invoice ? $"INVOICE # {_model.InvoiceNo}" : "STATEMENT", TextFormat.Bold);
        //paragraph.AddTab();
        paragraph = _tableCaptionFrameRight.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.Format.Alignment = ParagraphAlignment.Right;
        paragraph.AddText("Page ");
        paragraph.AddPageField();
        paragraph.AddText(" of ");
        paragraph.AddNumPagesField();

        // Create footer
        paragraph = _section.Footers.Primary.AddParagraph();
        paragraph.AddText($"{_model.BillingCompanyName} · {_model.BillingCompanyAddress} · {_model.BillingCompanyCity} · {_model.BillingCompanyState} {_model.BillingCompanyZipCode} · {_model.BillingCompanyPhone}");
        paragraph.Format.Font.Size = 9;
        paragraph.Format.Alignment = ParagraphAlignment.Center;

        _section.Footers.FirstPage = _section.Footers.Primary.Clone();
    }

    private void CreateInvoicePage()
    {
        // Each MigraDoc document needs at least one section.
        _section = this._document.AddSection();
        _section.PageSetup.TopMargin = Unit.FromInch(3.75);

        CreateHeaderFooter();

        // create text frame for invoice information
        this._invoiceFrame = _section.AddTextFrame();
        this._invoiceFrame.Height = "3.0cm";
        this._invoiceFrame.Width = "2.5in";
        this._invoiceFrame.Left = "4.0in";
        this._invoiceFrame.RelativeHorizontal = RelativeHorizontal.Margin;
        this._invoiceFrame.Top = "3.1cm";
        this._invoiceFrame.RelativeVertical = RelativeVertical.Page;

        // Create the item table
        this._table = _section.AddTable();
        this._table.Style = "Table";
        this._table.Borders.Color = MigraDoc.DocumentObjectModel.Color.Parse("Blue");
        this._table.Borders.Width = 0.25;
        this._table.Borders.Left.Width = 0.5;
        this._table.Borders.Right.Width = 0.5;
        this._table.Rows.LeftIndent = 0;

        // Before you can add a row, you must define the columns
        //account
        Column column = this._table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        //quantity
        column = this._table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Center;

        //cdm
        column = this._table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        //description
        column = this._table.AddColumn("6cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        //item amount
        column = this._table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Right;

        //account total amount
        column = this._table.AddColumn("3cm");
        column.Format.Alignment = ParagraphAlignment.Right;

        // Create the header of the table
        Row row = _table.AddRow();
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

        row = _table.AddRow();
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

        this._table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
    }

    private void FillInvoiceContent()
    {
        // Fill address in address text frame

        Paragraph paragraph = this._addressFrame.AddParagraph();
        paragraph.AddText(_model.ClientName);
        paragraph.AddLineBreak();
        paragraph.AddText(_model.Address1 ?? string.Empty);
        paragraph.AddLineBreak();
        paragraph.AddText($"{_model.City}, {_model.State} {_model.ZipCode}");

        paragraph = this._invoiceFrame.AddParagraph();
        paragraph.Format.Alignment = ParagraphAlignment.Right;
        paragraph.AddText("Invoice:  ");
        paragraph.AddFormattedText(_model.InvoiceNo, TextFormat.Bold);
        paragraph.AddLineBreak();
        paragraph.AddText("Invoice Date:  ");
        paragraph.AddFormattedText(_model.InvoiceDate.ToShortDateString(), TextFormat.Bold);
        paragraph.AddLineBreak();
        paragraph.AddText("Invoice Total: ");
        paragraph.AddFormattedText(_model.InvoiceTotal.ToString("0.00"), TextFormat.Bold);

        // Iterate the invoice items

        foreach (var detail in _model.InvoiceDetails)
        {
            if (detail.AccountTotal == 0)
                continue;

            int detailLines = detail.InvoiceDetailLines.Count;


            // Each item fills two rows
            Row row1 = this._table.AddRow();
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


            foreach (var detailLine in detail.InvoiceDetailLines)
            {
                Row newRow = this._table.AddRow();

                newRow.Cells[1].AddParagraph(detailLine.Qty.ToString());
                newRow.Cells[2].AddParagraph(detailLine.CDM.ToString());
                newRow.Format.Font.Size = 8;

                paragraph = newRow.Cells[3].AddParagraph();
                paragraph.AddText(detailLine.Description);
                if (_model.ShowCpt)
                    paragraph.AddText($" ({detailLine.CPT})");

                newRow.Cells[4].AddParagraph(detailLine.Amount.ToString("0.00"));
            }

            row1.Cells[5].AddParagraph(detail.AccountTotal.ToString("0.00"));

            row1.Cells[5].VerticalAlignment = VerticalAlignment.Top;

            this._table.SetEdge(0, this._table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
        }

        // Add an invisible row as a space line to the table
        Row row = this._table.AddRow();
        row.Borders.Visible = false;

        // Add the total due row
        row = this._table.AddRow();
        row.Cells[0].AddParagraph("Total Due");
        row.Cells[0].Borders.Visible = false;
        row.Cells[0].Format.Font.Bold = true;
        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        row.Cells[0].MergeRight = 4;
        row.Cells[5].AddParagraph(_model.InvoiceTotal.ToString("0.00"));

        // Set the borders of the specified cell range
        this._table.SetEdge(5, this._table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);

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
        _section = this._document.AddSection();
        _section.PageSetup.TopMargin = Unit.FromInch(3.75);

        CreateHeaderFooter();

        // Create the item table
        this._table = _section.AddTable();
        this._table.Style = "Table";
        this._table.Borders.Color = Color.Parse("Blue");
        this._table.Borders.Width = 0.25;
        this._table.Borders.Left.Width = 0.5;
        this._table.Borders.Right.Width = 0.5;
        this._table.Rows.LeftIndent = 0;

        // Before you can add a row, you must define the columns

        //service date
        Column column = this._table.AddColumn("1in");
        column.Format.Alignment = ParagraphAlignment.Left;

        //reference
        column = this._table.AddColumn("1in");
        column.Format.Alignment = ParagraphAlignment.Left;

        //description
        column = this._table.AddColumn("3in");
        column.Format.Alignment = ParagraphAlignment.Left;

        //amount
        column = this._table.AddColumn("1in");
        column.Format.Alignment = ParagraphAlignment.Right;

        // Create the header of the table
        Row row = _table.AddRow();
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

        this._table.SetEdge(0, 0, this._table.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
    }

    private void FillStatementContent()
    {
        // Fill address in address text frame

        Paragraph paragraph = this._addressFrame.AddParagraph();
        paragraph.AddText(_model.ClientName);
        paragraph.AddLineBreak();
        paragraph.AddText(_model.Address1 ?? string.Empty);
        paragraph.AddLineBreak();
        paragraph.AddText($"{_model.City}, {_model.State} {_model.ZipCode}");

        // add a balance forward line
        Row row = this._table.AddRow();
        row.TopPadding = 1.5;
        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[0].AddParagraph(_model.BalanceForwardDate.ToShortDateString());
        row.Cells[2].AddParagraph("Balance Forward");
        row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
        row.Cells[3].AddParagraph(_model.BalanceForward.ToString("0.00"));

        // Iterate the invoice items
        foreach (var detail in _model.ClientStatementDetails)
        {
            Row row1 = this._table.AddRow();
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

            this._table.SetEdge(0, this._table.Rows.Count - 1, _table.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75);
        }

        // Add an invisible row as a space line to the table
        row = this._table.AddRow();
        row.Borders.Visible = false;

        // Add the total due row
        row = this._table.AddRow();
        row.Cells[0].AddParagraph("Total Due");
        row.Cells[0].Borders.Visible = false;
        row.Cells[0].Format.Font.Bold = true;
        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
        row.Cells[0].MergeRight = 2;
        row.Cells[3].AddParagraph(_model.BalanceDue.ToString("0.00"));

        // Set the borders of the specified cell range
        this._table.SetEdge(3, this._table.Rows.Count - 4, 1, 2, Edge.Box, BorderStyle.Single, 0.75);

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
             .GetValue(string.Empty).ToString();

        ProcessStartInfo info = new()
        {
            Verb = "print",
            FileName = processFilename,
            Arguments = String.Format("/p /h {0}", pdfFileName),
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            //(It won't be hidden anyway... thanks Adobe!)
            UseShellExecute = false
        };

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
