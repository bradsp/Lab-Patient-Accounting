using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using LabBilling.Core.Models;
using Table = iText.Layout.Element.Table;
using VerticalAlignment = iText.Layout.Properties.VerticalAlignment;

namespace LabBilling.Core
{

    /// <summary>
    /// Used to create and generate client invoices
    /// </summary>
    public static class InvoicePrint
    {

        /// <summary>
        /// Generates a PDF invoice
        /// </summary>
        /// <param name="model"></param>
        /// <param name="outFilePath">Include full path and filename of generated pdf.</param>
        public static void CreatePDF(InvoiceModel model, string outFilePath)
        {
            using (PdfWriter writer = new PdfWriter(outFilePath))
            {

                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                Paragraph newline = new Paragraph(new Text("\n"));

                Table headerTable = new Table(2, false)
                    .SetBorder(Border.NO_BORDER)
                    .UseAllAvailableWidth();

                if (model.ImageFilePath != null && File.Exists(model.ImageFilePath))
                {
                    Image img = new Image(ImageDataFactory
                        .Create(model.ImageFilePath))
                        .SetTextAlignment(TextAlignment.LEFT)
                        .ScaleToFit(200, 60)
                        .SetAutoScale(false);

                    headerTable.AddCell(new Cell(1, 1)
                        .SetBackgroundColor(ColorConstants.WHITE)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.TOP)
                        .SetBorder(Border.NO_BORDER)
                        .SetBorderBottom(new SolidBorder(ColorConstants.BLUE, 1))
                        .Add(img));
                }
                else
                {
                    headerTable.AddCell(new Cell(1, 1)
                        .SetBackgroundColor(ColorConstants.WHITE)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.TOP)
                        .SetBorder(Border.NO_BORDER)
                        .SetBorderBottom(new SolidBorder(ColorConstants.BLUE, 1)));
                }

                headerTable.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetVerticalAlignment(VerticalAlignment.TOP)
                    .SetBorder(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(ColorConstants.BLUE, 1))
                    .Add(new Paragraph(model.BillingCompanyName).SetFontSize(8))
                    .Add(new Paragraph(model.BillingCompanyAddress).SetFontSize(8))
                    .Add(new Paragraph(string.Format("{0} {1} {2}", model.BillingCompanyCity, model.BillingCompanyState, model.BillingCompanyZipCode)).SetFontSize(8))
                    .Add(new Paragraph(model.BillingCompanyPhone).SetFontSize(8)));

                headerTable.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetVerticalAlignment(VerticalAlignment.TOP)
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph(model.ClientName).SetFontSize(9))
                    .Add(new Paragraph(model.Address1).SetFontSize(9))
                    .Add(new Paragraph(model.Address2 ?? "").SetFontSize(9))
                    .Add(new Paragraph(string.Format("{0} {1} {2}", model.City, model.State, model.ZipCode)).SetFontSize(9)));

                Table invoiceInfo = null;
                if (model.StatementType == InvoiceModel.StatementTypeEnum.Invoice)
                    invoiceInfo = InvoiceInfoHeader(model);
                else if (model.StatementType == InvoiceModel.StatementTypeEnum.Statement)
                    invoiceInfo = StatementInfoHeader(model);

                headerTable.AddCell(invoiceInfo)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(new SolidBorder(ColorConstants.WHITE, 2));

                document.Add(headerTable);

                Border detailBorder = new SolidBorder(ColorConstants.WHITE, 2);
                Color tableHeaderColor = WebColors.GetRGBColor("RoyalBlue");

                Table table = null;

                if (model.StatementType == InvoiceModel.StatementTypeEnum.Invoice)
                    table = InvoiceTableHeader(tableHeaderColor, detailBorder);
                else if (model.StatementType == InvoiceModel.StatementTypeEnum.Statement)
                    table = StatementTableHeader(tableHeaderColor, detailBorder);
                                
                // add balance foward line
                //if (model.StatementType == InvoiceModel.StatementTypeEnum.Statement)
                //{
                //    table.AddCell(new Cell(1, 5)
                //    .SetBackgroundColor(ColorConstants.WHITE)
                //    .SetBorder(detailBorder));

                //    table.AddCell(new Cell(1, 1)
                //        .SetBorder(detailBorder)
                //        .SetBackgroundColor(WebColors.GetRGBColor("RoyalBlue"))
                //        .SetFontColor(ColorConstants.WHITE)
                //        .Add(new Paragraph("Balance Forward").SetFontSize(10).SetBold()));

                //    table.AddCell(new Cell(1, 2)
                //        .SetBorder(detailBorder)
                //        .SetBackgroundColor(WebColors.GetRGBColor("RoyalBlue"))
                //        .SetFontColor(ColorConstants.WHITE)
                //        .SetTextAlignment(TextAlignment.RIGHT)
                //        .Add(new Paragraph(model.BalanceForward.ToString("0.00")).SetFontSize(11)));
                //}

                if (model.StatementType == InvoiceModel.StatementTypeEnum.Invoice)
                {
                    InvoiceDetailLines(model, table, detailBorder);
                }

                if(model.StatementType == InvoiceModel.StatementTypeEnum.Statement)
                {
                    StatementDetailLines(model, table, detailBorder);
                }
                // todo: footer
                InvoiceFooter(model, table, detailBorder);

                document.Add(table);

                // Page numbers
                //int n = pdf.GetNumberOfPages();
                //for (int i = 1; i <= n; i++)
                //{
                //    document.ShowTextAligned(new Paragraph(String
                //       .Format("Page" + i + " of " + n)),
                //       559, 10, i, TextAlignment.RIGHT,
                //       VerticalAlignment.BOTTOM, 0);
                //}

                document.Close();
            }
        }


        private static Table InvoiceInfoHeader(InvoiceModel model)
        {
            Table invoiceInfo = new Table(2, false)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .UseAllAvailableWidth();
            invoiceInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph("Invoice:")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBold()
                .SetFontSize(9)));
            invoiceInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph(model.InvoiceNo)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(9)));
            invoiceInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph("Invoice Date:")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBold()
                .SetFontSize(9)));
            invoiceInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph(model.InvoiceDate.ToShortDateString())
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(9)));
            invoiceInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph("Invoice Total:")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBold()
                .SetFontSize(9)));
            invoiceInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph(model.InvoiceTotal.ToString("0.00"))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(9)));

            return invoiceInfo;
        }

        private static Table StatementInfoHeader(InvoiceModel model)
        {
            Table statementInfo = new Table(2, false)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .UseAllAvailableWidth();

            //statementInfo.AddCell(new Cell(1, 1)
            //    .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
            //    .Add(new Paragraph("Invoice:")
            //    .SetTextAlignment(TextAlignment.RIGHT)
            //    .SetBold()
            //    .SetFontSize(9)));
            //statementInfo.AddCell(new Cell(1, 1)
            //    .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
            //    .Add(new Paragraph(model.InvoiceNo)
            //    .SetTextAlignment(TextAlignment.RIGHT)
            //    .SetFontSize(9)));
            statementInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph("Statement Date:")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBold()
                .SetFontSize(9)));
            statementInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph(model.InvoiceDate.ToShortDateString())
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(9)));
            statementInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph("Balance Due:")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBold()
                .SetFontSize(9)));
            statementInfo.AddCell(new Cell(1, 1)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 2))
                .Add(new Paragraph(model.BalanceDue.ToString("0.00"))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(9)));

            return statementInfo;
        }

        private static Table InvoiceTableHeader(Color tableHeaderColor, Border detailBorder)
        {
            Table table = new Table(8, false)
                .UseAllAvailableWidth();
            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Account").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Patient Name").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Service Date").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Charge Code").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("CPT Code").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Description").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Qty").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Amount").SetFontSize(10)));

            return table;
        }

        private static Table StatementTableHeader(Color tableHeaderColor, Border detailBorder)
        {
            Table table = new Table(8, false)
                .UseAllAvailableWidth();

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Date").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Ref").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Description").SetFontSize(10)));

            table.AddCell(new Cell(1, 1)
                .SetBackgroundColor(tableHeaderColor)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontColor(ColorConstants.WHITE)
                .SetBorder(detailBorder)
                .Add(new Paragraph("Amount").SetFontSize(10)));

            return table;
        }

        private static void InvoiceDetailLines(InvoiceModel model, Table table, Border detailBorder)
        {
            int detailFontSize = 8;

            Color bkgColor1 = WebColors.GetRGBColor("LightCyan");
            Color bkgColor2 = WebColors.GetRGBColor("SkyBlue");
            Color currentRow = bkgColor2;

            foreach (var detail in model.InvoiceDetails)
            {
                if (currentRow == bkgColor1)
                    currentRow = bkgColor2;
                else currentRow = bkgColor1;
                // Table
                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.Account ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.PatientName ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.ServiceDate?.ToShortDateString() ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.CDM ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.CPT ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.Description ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.Qty.ToString() ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.Amount.ToString("0.00") ?? "").SetFontSize(detailFontSize)));

            }

        }

        private static void StatementDetailLines(InvoiceModel model, Table table, Border detailBorder)
        {
            int detailFontSize = 8;

            Color bkgColor1 = WebColors.GetRGBColor("LightCyan");
            Color bkgColor2 = WebColors.GetRGBColor("SkyBlue");
            Color currentRow = bkgColor2;

            foreach (var detail in model.ClientStatementDetails)
            {
                if (currentRow == bkgColor1)
                    currentRow = bkgColor2;
                else currentRow = bkgColor1;
                // Table
                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.ServiceDate.ToShortDateString()).SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph("").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.Description ?? "").SetFontSize(detailFontSize)));

                table.AddCell(new Cell(1, 1)
                    .SetBackgroundColor(currentRow)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(detailBorder)
                    .Add(new Paragraph(detail.Amount.ToString("0.00") ?? "").SetFontSize(detailFontSize)));

            }

        }

        private static void InvoiceFooter(InvoiceModel model, Table table, Border detailBorder)
        {
            table.AddCell(new Cell(1, 5)
                .SetBackgroundColor(ColorConstants.WHITE)
                .SetBorder(detailBorder));

            table.AddCell(new Cell(1, 1)
                .SetBorder(detailBorder)
                .SetBackgroundColor(WebColors.GetRGBColor("RoyalBlue"))
                .SetFontColor(ColorConstants.WHITE)
                .Add(new Paragraph("Total").SetFontSize(10).SetBold()));

            table.AddCell(new Cell(1, 2)
                .SetBorder(detailBorder)
                .SetBackgroundColor(WebColors.GetRGBColor("RoyalBlue"))
                .SetFontColor(ColorConstants.WHITE)
                .SetTextAlignment(TextAlignment.RIGHT)
                .Add(new Paragraph(model.InvoiceTotal.ToString("0.00")).SetFontSize(11)));

            table.AddCell(new Cell(1, 5)
                .SetBackgroundColor(ColorConstants.WHITE)
                .SetBorder(detailBorder));

            if (model.StatementType == InvoiceModel.StatementTypeEnum.Statement)
            {
                table.AddCell(new Cell(1, 1)
                    .SetBorder(detailBorder)
                    .SetBackgroundColor(WebColors.GetRGBColor("RoyalBlue"))
                    .SetFontColor(ColorConstants.WHITE)
                    .Add(new Paragraph("Balance Due").SetFontSize(10).SetBold()));

                table.AddCell(new Cell(1, 2)
                    .SetBorder(detailBorder)
                    .SetBackgroundColor(WebColors.GetRGBColor("RoyalBlue"))
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph(model.BalanceDue.ToString("0.00")).SetFontSize(11)));
            }

        }

    }


}
