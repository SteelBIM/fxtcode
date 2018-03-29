using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using NPOI.HSSF.Util;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using NPOI.SS.Util;

namespace FxtDataAcquisition.Common.NPOI
{
    public class ExcelHandle : IDisposable
    {
        private readonly string _fileName; //文件名
        private IWorkbook _workbook;
        private FileStream _fs;
        private bool _disposed;

        public ExcelHandle(string fileName)
        {
            this._fileName = fileName;
            _disposed = false;
        }
        /// <summary>
        /// 导出到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static MemoryStream ListToExcel<T>(List<T> data, string sheetName = "Sheet1") where T : class
        {

            try
            {
                IWorkbook workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet(sheetName);
                CreateSheet(sheet, data, workbook);

                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms); //写入到内存流
                    ms.Flush();
                    ms.Position = 0;
                    return ms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导出到excel
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="sheetName1"></param>
        /// <param name="sheetName2"></param>
        /// <returns></returns>
        public static MemoryStream ListToExcel<T1, T2>(List<T1> data1, List<T2> data2, string sheetName1 = "Sheet1", string sheetName2 = "Sheet2")
            where T1 : class
            where T2 : class
        {

            try
            {
                IWorkbook workbook = new HSSFWorkbook();

                var sheet1 = workbook.CreateSheet(sheetName1);
                var sheet2 = workbook.CreateSheet(sheetName2);

                CreateSheet(sheet1, data1, workbook);
                CreateSheet(sheet2, data2, workbook);

                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms); //写入到内存流
                    ms.Flush();
                    ms.Position = 0;
                    return ms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream ListToExcel<T1, T2>(Dictionary<string, List<T1>> sheets1, Dictionary<string, List<T2>> sheets2 = null)
            where T1 : class
            where T2 : class
        {
            try
            {
                IWorkbook workbook = new HSSFWorkbook();

                foreach (var item in sheets1)
                {
                    var sheet = workbook.CreateSheet(item.Key);
                    CreateSheet(sheet, item.Value, workbook);
                }

                if (sheets2 != null)
                {
                    foreach (var item in sheets2)
                    {
                        var sheet = workbook.CreateSheet(item.Key);
                        CreateSheet(sheet, item.Value, workbook);
                    }
                }

                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms); //写入到内存流
                    ms.Flush();
                    ms.Position = 0;
                    return ms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        static void CreateSheet<T>(ISheet sheet, IEnumerable<T> data, IWorkbook hssfworkbook) where T : class
        {
            var count = 0;
            var columnIndex = 0;

            var type = data.GetType();
            var properties = type.GetGenericArguments()[0].GetProperties(); //获取T类型的所有属性

            var rowHeader = sheet.CreateRow(count);
            //列样式
            List<ICellStyle> cellStyles = new List<ICellStyle>();
            List<IDataFormat> formats = new List<IDataFormat>();
            foreach (var t in properties)
            {
                //sheet.AutoSizeColumn(n);//自动列宽
                var ignore = ((ExcelExportIgnoreAttribute[])t.GetCustomAttributes(typeof(ExcelExportIgnoreAttribute), false)).FirstOrDefault();
                if (ignore != null)
                {
                    //sheet.SetColumnHidden(n, true);
                    continue;
                }

                var displayName = ((DisplayNameAttribute[])t.GetCustomAttributes(typeof(DisplayNameAttribute), false)).FirstOrDefault();
                rowHeader.CreateCell(columnIndex).SetCellValue(displayName == null ? "无列名" : displayName.DisplayName);
                columnIndex++;
                cellStyles.Add(hssfworkbook.CreateCellStyle());
                formats.Add(hssfworkbook.CreateDataFormat());
            }
            count++;

            foreach (var obj in data)
            {
                var row = sheet.CreateRow(count);

                columnIndex = 0;

                foreach (var t in properties)
                {
                    var ignore = ((ExcelExportIgnoreAttribute[])t.GetCustomAttributes(typeof(ExcelExportIgnoreAttribute), false)).FirstOrDefault();
                    if (ignore != null)
                    {
                        //sheet.SetColumnHidden(j, true);
                        continue;
                    }
                    var pValue = t.GetValue(obj, null);
                    var cell = row.CreateCell(columnIndex);

                    //单元格样式
                    var cellStyleBute = ((ExcelExportIgnoreTypeAttribute[])t.GetCustomAttributes(typeof(ExcelExportIgnoreTypeAttribute), false)).FirstOrDefault();
                    if (cellStyleBute != null)
                    {

                        SetCellStyle(cellStyleBute, cell, cellStyles[columnIndex], formats[columnIndex], pValue);
                    }
                    else
                    {
                        cell.SetCellValue(pValue == null ? "" : pValue.ToString());
                    }
                    columnIndex++;
                }
                ++count;
            }
        }

        public static MemoryStream RenderToExcel(DataTable table)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet1");
                var headerRow = sheet.CreateRow(0);
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                sheet.SetColumnHidden(1, true);
                //sheet.SetColumnHidden(1, true);
                using (var ms = new MemoryStream())
                {
                    ms.Position = 0;
                    workbook.Write(ms); //写入到内存流
                    ms.Flush();
                    return ms;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream RenderToExcelHiddenColumn(string fileNamePath, List<string> columns)
        {
            try
            {
                List<int> hiddens = new List<int>();
                //try
                //{
                IWorkbook workbook = null;
                FileStream _fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
                if (fileNamePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本以上
                    workbook = new XSSFWorkbook(_fs);
                else if (fileNamePath.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    workbook = new HSSFWorkbook(_fs);

                var sheet = workbook.GetSheetAt(0);
                var headerRow = sheet.GetRow(0);

                for (int i = 0; i < headerRow.Cells.Count; i++)
                {
                    var cell = headerRow.GetCell(i);
                    if (!columns.Contains(cell.StringCellValue))
                    {
                        hiddens.Add(i);
                    }
                }

                if (hiddens.Count > 0)
                {
                    foreach (var hidden in hiddens)
                    {
                        sheet.SetColumnHidden(hidden, true);
                    }
                }

                using (var ms = new MemoryStream())
                {
                    ms.Position = 0;
                    workbook.Write(ms); //写入到内存流
                    ms.Flush();
                    return ms;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static void RenderToCsv(DataTable table, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                string data = "";

                //写出列名称
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    data += table.Columns[i].ColumnName.ToString();
                    if (i < table.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);

                //写出各行数据
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        data += (table.Rows[i][j].ToString()).Replace("\t", "");
                        if (j < table.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            var data = new DataTable();
            //try
            //{
            _fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
            if (_fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本以上
                _workbook = new XSSFWorkbook(_fs);
            else if (_fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                _workbook = new HSSFWorkbook(_fs);

            //名称管理器
            //var name = _workbook.GetName("test");
            //CellRangeAddress cellRange = CellRangeAddress.ValueOf(name.RefersToFormula); 

            ISheet sheet = null;
            sheet = sheetName != null ? _workbook.GetSheet(sheetName) : _workbook.GetSheetAt(0);
            if (sheet == null) return data;
            var firstRow = sheet.GetRow(0);
            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

            var startRow = 0;
            if (isFirstRowColumn)
            {
                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                {
                    var column = new DataColumn(firstRow.GetCell(i) != null ? firstRow.GetCell(i).ToString() : string.Empty);
                    data.Columns.Add(column);
                }
                startRow = sheet.FirstRowNum + 1;
            }
            else
            {
                startRow = sheet.FirstRowNum;
            }

            //最后一列的标号
            var rowCount = sheet.LastRowNum;
            for (var i = startRow; i <= rowCount; ++i)
            {
                var row = sheet.GetRow(i);
                if (row == null)//没有数据的行默认是null
                {
                    continue;
                }

                //if (row.Cells.Count < 2) continue;

                int count = 0;
                foreach (var rc in row.Cells)
                {
                    if (rc == null || rc.ToString() == "") count++;
                }
                if (count == row.Cells.Count)
                {
                    continue;
                }

                if (row.Cells[0] == null && row.Cells[1] == null)
                {
                    continue;
                }
                var dataRow = data.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; ++j)
                {
                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                        dataRow[j] = row.GetCell(j).ToString();
                }
                data.Rows.Add(dataRow);
            }

            return data;
            //}
            //catch (Exception ex)
            //{
            //    //throw new Exception(ex.Message);
            //    throw;
            //}
        }

        /// <summary>
        /// 创建一个Excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileNamePath">文件名绝对路径</param>
        /// <param name="folder">文件所在的文件夹绝对路径</param>
        public void CreateExcel(DataTable data, string fileNamePath, string folder = null)
        {

            var wk = new XSSFWorkbook();
            var sheet = wk.CreateSheet("Sheet1");

            //字体设置
            var font = wk.CreateFont();
            font.Color = HSSFColor.Red.Index;
            var style = wk.CreateCellStyle();
            style.SetFont(font);


            var count = 0;

            var headerRow = sheet.CreateRow(count);
            foreach (DataColumn column in data.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
            count++;
            foreach (DataRow row in data.Rows)
            {
                var datarow = sheet.CreateRow(count);

                foreach (DataColumn column in data.Columns)
                {
                    var cell = datarow.CreateCell(column.Ordinal);
                    var cellValue = row[column].ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        var array = cellValue.Split('#');
                        if (array.Length == 2)
                            cell.CellStyle = style;

                        cell.SetCellValue(array[0]);
                    }
                    else
                    {
                        cell.SetCellValue(cellValue);
                    }

                }
                ++count;
            }

            if (!string.IsNullOrWhiteSpace(folder))
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

            }

            using (var openWrite = File.OpenWrite(fileNamePath)) //打开一个xlsx文件，如果没有则自行创建，如果存在myxls.xlsx文件则在创建是不要打开该文件！
            {
                wk.Write(openWrite);   //向打开的这个xlsx文件中写入mySheet表并保存。
            }
        }

        /// <summary>
        /// 创建一个Excel
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="sheetName2"></param>
        /// <param name="fileNamePath">文件名绝对路径</param>
        /// <param name="folder">文件所在的文件夹绝对路径</param>
        /// <param name="sheetName1"></param>
        public void CreateExcel(DataTable data1, DataTable data2, string sheetName1 = "Sheet1", string sheetName2 = "Sheet2", string fileNamePath = null, string folder = null)
        {

            var wk = new XSSFWorkbook();

            //字体设置
            var font = wk.CreateFont();
            font.Color = HSSFColor.Red.Index;
            var style = wk.CreateCellStyle();
            style.SetFont(font);

            var sheet1 = wk.CreateSheet(sheetName1);
            CreateSheet4Error(sheet1, style, data1);

            var sheet2 = wk.CreateSheet(sheetName2);
            CreateSheet4Error(sheet2, style, data2);

            if (!string.IsNullOrWhiteSpace(folder))
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

            }

            using (var openWrite = File.OpenWrite(fileNamePath)) //打开一个xlsx文件，如果没有则自行创建，如果存在myxls.xlsx文件则在创建是不要打开该文件！
            {
                wk.Write(openWrite);   //向打开的这个xlsx文件中写入mySheet表并保存。
            }
        }

        /// <summary>
        /// 创建一个Excel
        /// </summary>
        /// <param name="sheets">sheet集合</param>
        /// <param name="fileNamePath">文件名绝对路径</param>
        /// <param name="folder">文件所在的文件夹绝对路径</param>
        /// <param name="sheetName1"></param>
        public void CreateExcel(Dictionary<string, DataTable> sheets, string fileNamePath = null, string folder = null)
        {

            var wk = new XSSFWorkbook();

            //字体设置
            var font = wk.CreateFont();
            font.Color = HSSFColor.Red.Index;
            var style = wk.CreateCellStyle();
            style.SetFont(font);

            foreach (var item in sheets)
            {
                var sheet = wk.CreateSheet(item.Key);
                CreateSheet4Error(sheet, style, item.Value);
            }

            if (!string.IsNullOrWhiteSpace(folder))
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

            }

            using (var openWrite = File.OpenWrite(fileNamePath)) //打开一个xlsx文件，如果没有则自行创建，如果存在myxls.xlsx文件则在创建是不要打开该文件！
            {
                wk.Write(openWrite);   //向打开的这个xlsx文件中写入mySheet表并保存。
            }
        }

        public void CreateSheet4Error(ISheet sheet, ICellStyle style, DataTable dt)
        {
            var count = 0;

            var headerRow = sheet.CreateRow(count);
            foreach (DataColumn column in dt.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
            count++;
            foreach (DataRow row in dt.Rows)
            {
                var datarow = sheet.CreateRow(count);

                foreach (DataColumn column in dt.Columns)
                {
                    var cell = datarow.CreateCell(column.Ordinal);
                    var cellValue = row[column].ToString();
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        var array = cellValue.Split('#');
                        if (array.Length == 2)
                            cell.CellStyle = style;

                        cell.SetCellValue(array[0]);
                    }
                    else
                    {
                        cell.SetCellValue(cellValue);
                    }

                }
                ++count;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (_fs != null)
                        _fs.Close();
                }

                _fs = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="cellStyleBute"></param>
        /// <param name="cell"></param>
        /// <param name="cellStyle"></param>
        /// <param name="format"></param>
        /// <param name="pValue"></param>
        private static void SetCellStyle(ExcelExportIgnoreTypeAttribute cellStyleBute, ICell cell, ICellStyle cellStyle, IDataFormat format, object pValue)
        {

            switch (cellStyleBute.ExportType)
            {
                case ExcelExportType.String:
                    break;
                case ExcelExportType.DataTime:
                    DateTime date;
                    if (DateTime.TryParse(pValue == null ? "" : pValue.ToString(), out date))
                    {
                        cell.SetCellValue(date);
                    }
                    else
                    {
                        cell.SetCellValue("");
                    }
                    cellStyle.DataFormat = format.GetFormat(cellStyleBute.Fomat);
                    cell.CellStyle = cellStyle;
                    break;
                case ExcelExportType.Numeric:
                    double number = 0;
                    if (double.TryParse(pValue == null ? "" : pValue.ToString(), out number))
                    {
                        cell.SetCellValue(number);
                    }
                    else
                    {
                        cell.SetCellValue("");
                    }
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat(cellStyleBute.Fomat);
                    cell.CellStyle = cellStyle;
                    break;
                case ExcelExportType.Money:
                    double moey = 0;
                    if (double.TryParse(pValue == null ? "" : pValue.ToString(), out moey))
                    {
                        cell.SetCellValue(moey);
                    }
                    else
                    {
                        cell.SetCellValue("");
                    }
                    cellStyle.DataFormat = format.GetFormat(cellStyleBute.Fomat);
                    cell.CellStyle = cellStyle;
                    break;
                case ExcelExportType.Percent:
                    double percent = 0;
                    if (double.TryParse(pValue == null ? "" : pValue.ToString(), out percent))
                    {
                        cell.SetCellValue(percent);
                    }
                    else
                    {
                        cell.SetCellValue("");
                    }
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat(cellStyleBute.Fomat);
                    cell.CellStyle = cellStyle;
                    break;
                case ExcelExportType.ChineseCapital:
                    cell.SetCellValue(pValue == null ? "" : pValue.ToString());
                    cellStyle.DataFormat = format.GetFormat(cellStyleBute.Fomat);
                    cell.CellStyle = cellStyle;
                    break;
                default:
                    break;
            }
        }

    }
}
