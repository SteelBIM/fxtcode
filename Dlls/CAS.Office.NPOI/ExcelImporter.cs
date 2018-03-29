using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading;
using System.Globalization;

namespace CAS.Office.NPOI
{ 
    /// <summary>
    /// byte 2014-12-05
    /// excel导入
    /// </summary>
    public class ExcelImporter
    {
        /// <summary>
        /// Use NPOI to read an XLS or XLSX file into a datatable
        /// </summary>
        /// <param name="openTargetFile"></param>
        /// <returns></returns>
        public static DataTable ReadExcelFileToDataTable(string openTargetFile)
        {
            ISheet sheet = null;
            using (FileStream input = File.OpenRead(openTargetFile))
            {
                if (openTargetFile.EndsWith(".xlsx"))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(input);
                    sheet = workbook.GetSheetAt(0);
                }
                else
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(input);
                    sheet = workbook.GetSheetAt(0);
                }
            }
            DataTable dataTable = ConvertISheetToDataTable(sheet);
            return dataTable;
        }

        /// <summary>
        /// Read the contents an NPOI ISheet into a new DataTable object
        /// </summary>
        /// <param name="sheet">ISheet to read</param>
        /// <returns>DataTable populated with the contents of the ISheet</returns>
        private static DataTable ConvertISheetToDataTable(ISheet sheet)
        {
            // Temporarily set the thread culture to avoid conversion issues
            // http://stackoverflow.com/questions/15040567/c-xlsx-date-cell-import-to-datatable-by-npoi-2-0
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                DataTable table = new DataTable();

                int rowCount = sheet.LastRowNum;
                int colCount = 0;

                IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
                foreach (ICell cell in headerRow.Cells)
                {
                    string columnName = cell.ToString();
                    DataColumn column = new DataColumn(columnName);
                    table.Columns.Add(column);
                    colCount++;
                }

                for (int rowNum = (sheet.FirstRowNum) + 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow row = sheet.GetRow(rowNum);
                    DataRow dataRow = table.NewRow();
                    int cellNumber = 0;
                    for (int colNum = 0; colNum < colCount; colNum++)
                    {
                        ICell cell = row.GetCell(colNum);
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.Boolean:
                                    dataRow[cellNumber] = cell.BooleanCellValue;
                                    break;
                                case CellType.Numeric:
                                case CellType.Formula:
                                    dataRow[cellNumber] = cell.NumericCellValue;
                                    break;
                                default:
                                    dataRow[cellNumber] = cell.StringCellValue;
                                    break;
                            }
                        }
                        cellNumber++;
                    }
                    table.Rows.Add(dataRow);
                }
                return table;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = prevCulture;
            }

        }
    }
}
