using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseExport
{
    class ExcelHelper
    {

        public static void Export(DataTable dt, string excelFullPath)
        {
            using (ExcelPackage excelpkg = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = excelpkg.Workbook.Worksheets.Add(dt.TableName??"Sheet1");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                //File.WriteAllBytes(excelFullPath, excelpkg.GetAsByteArray());
                using (FileStream fs = File.OpenWrite(excelFullPath))
                {
                    excelpkg.SaveAs(fs);
                }
            }


        }

    }
}
