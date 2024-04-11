using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;

namespace WinFormsLibrary
{
    public class DataGridToExcel
    {
        public static void Export(DataGridView dgv, string destinationPath, string sheetName)
        {
            DataTable dt = new DataTable();

            foreach(DataGridViewColumn col in dgv.Columns)
            {
                dt.Columns.Add(col.HeaderText, col.ValueType);
            }

            foreach(DataGridViewRow row in dgv.Rows)
            {
                dt.Rows.Add();
                foreach(DataGridViewCell cell in row.Cells)
                {
                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }

            //Exporting to Excel

            string path = Path.GetDirectoryName(destinationPath);

            if(!Directory.Exists(path))
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, sheetName);
                    wb.Worksheet(1).Cells("A1:C1").Style.Fill.BackgroundColor = XLColor.DarkGreen;
                    for(int i = 1; i <= dt.Rows.Count; i++)
                    {
                        string cellRange = string.Format("A{0}:C{0}", i + 1);
                        if(i % 2 != 0)
                        {
                            wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                        else
                        {
                            wb.Worksheet(1).Cells(cellRange).Style.Fill.BackgroundColor = XLColor.White;
                        }
                    }

                    wb.Worksheet(1).Columns().AdjustToContents();
                    wb.SaveAs(destinationPath);
                }
            }
        }
    }
}
