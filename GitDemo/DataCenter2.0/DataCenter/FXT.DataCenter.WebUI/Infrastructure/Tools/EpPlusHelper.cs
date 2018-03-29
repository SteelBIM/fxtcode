using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using OfficeOpenXml;

namespace FXT.DataCenter.WebUI.Tools
{
    public class EpPlusHelper
    {
        public static void ListToExcel<T>(ExcelPackage package, List<T> data, string sheetName = "Sheet1")
            where T : class
        {
            try
            {
                    var rowIndex = 1;
                    var columnIndex = 1;

                    var worksheet = package.Workbook.Worksheets.Add(sheetName);

                    var type = data.GetType();
                    var properties = type.GetGenericArguments()[0].GetProperties();

                    foreach (var t in properties)
                    {

                        var ignore = ((ExcelExportIgnoreAttribute[])t.GetCustomAttributes(typeof(ExcelExportIgnoreAttribute), false)).FirstOrDefault();

                        if (ignore != null) continue;

                        var displayName = ((DisplayNameAttribute[])t.GetCustomAttributes(typeof(DisplayNameAttribute), false)).FirstOrDefault();

                        worksheet.Cells[rowIndex, columnIndex].Value = displayName == null ? "无列名" : displayName.DisplayName;

                        columnIndex++;
                    }

                    rowIndex = 2;

                    foreach (var obj in data)
                    {
                        columnIndex = 1;

                        foreach (var t in properties)
                        {
                            var ignore = ((ExcelExportIgnoreAttribute[])t.GetCustomAttributes(typeof(ExcelExportIgnoreAttribute), false)).FirstOrDefault();
                            if (ignore != null) continue;

                            var pValue = t.GetValue(obj, null);

                            worksheet.Cells[rowIndex, columnIndex].Value = pValue == null ? "" : pValue.ToString();

                            columnIndex++;
                        }
                        rowIndex++;
                    }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}