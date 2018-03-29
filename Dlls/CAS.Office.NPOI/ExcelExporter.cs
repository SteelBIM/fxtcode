using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Threading;
using System.Globalization;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HPSF;
using System.Reflection;
using CAS.Entity.AttributeHelper;


namespace CAS.Office.NPOI
{
    /// <summary>
    /// byte 2014-12-05
    /// excel导出
    /// </summary>
    public class ExcelExporter
    {
        private HSSFWorkbook hssfWorkBook;

        public ExcelExporter()
        {
            hssfWorkBook = new HSSFWorkbook();
            InitializeWorkbook();
        }

        public void Export<T>(string saveTargetFile, IList<T> source, string[] availableFields) where T : class
        {
            Export<T>(saveTargetFile, source, availableFields, "Sheet1");
        }

        public void Export<T>(string saveTargetFile, IList<T> source, string[] availableFields, string sheetName) where T : class
        {
            #region 参数验证
            if (string.IsNullOrWhiteSpace(saveTargetFile))
            {
                throw new ArgumentNullException("saveTargetFile");
            }
            if (null == source)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new ArgumentNullException("sheetName");
            }
            #endregion
            if (0 == source.Count)
                return;
            Type type = source.GetType();
            PropertyInfo[] infos = type.GetGenericArguments()[0].GetProperties();
            availableFields = availableFields.Select(q => q.ToLower()).ToArray();
            int exportFieldCount = availableFields.Length;
            Dictionary<string, PropertyInfo> fieldNameMapping = new Dictionary<string, PropertyInfo>();
            Dictionary<string, string> columnsNameMapping = GetColumnsName(infos, availableFields, ref fieldNameMapping);
            var sheet = (HSSFSheet)hssfWorkBook.CreateSheet(sheetName);
            ApplyDetailCell<T>(source, sheet, availableFields, fieldNameMapping, columnsNameMapping);
            //格式化当前sheet，用于数据total计算
            sheet.ForceFormulaRecalculation = true;
            using (FileStream fs = new FileStream(saveTargetFile, FileMode.Create))
            {
                hssfWorkBook.Write(fs);
                fs.Flush();
            }
            sheet = null;
        }

        /// <summary>
        /// 使用格式应用明细单元格
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="sheet">当前单元格</param>
        private void ApplyDetailCell<T>(
            IList<T> source, ISheet sheet, string[] availableFields, Dictionary<string, PropertyInfo> fieldNameMapping, Dictionary<string, string> columnsNameMapping) where T : class
        {
            if (source == null || source.Count <= 0)
            {
                return;
            }
            int columnIndex;
            sheet.InsertRow(source.Count + 1, availableFields.Length);
            columnIndex = 0;
            //标头
            foreach (var columns in columnsNameMapping)
            {
                var tempRow = sheet.GetRow(0);
                var tempCell = tempRow.GetCell(columnIndex);
                tempCell.SetCellValue(columns.Value);
                tempCell.CellStyle.Alignment = HorizontalAlignment.Center;
                columnIndex++;
            }
            //填充数据
            for (int rowIndex = 1; rowIndex < source.Count + 1; rowIndex++)
            {
                columnIndex = 0;
                foreach (string field in availableFields)
                {
                    var tempRow = sheet.GetRow(rowIndex);
                    var tempCell = tempRow.GetCell(columnIndex);
                    PropertyInfo p = fieldNameMapping[field];
                    SetCellValue<T>(tempCell, p.GetValue(source[rowIndex - 1], null));
                    columnIndex++;
                }
            }
            columnIndex = 0;
            //标头
            foreach (var columns in columnsNameMapping)
            {
                sheet.AutoSizeColumn(columnIndex);
                columnIndex++;
            }
        }
        /// <summary>
        /// 设置单元格值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cell"></param>
        /// <param name="data"></param>
        private void SetCellValue<T>(ICell cell, object val)
        {
            if (cell == null)
            {
                return;
            }
            if (val != null)
            {
                var valType = val.GetType();

                switch (valType.ToString())
                {
                    case "System.String"://字符串类型
                        cell.SetCellValue(Convert.ToString(val));
                        break;
                    case "System.DateTime"://日期类型
                        cell.SetCellValue((DateTime)val);
                        ICellStyle cellStyle = hssfWorkBook.CreateCellStyle();
                        IDataFormat format = hssfWorkBook.CreateDataFormat();   
                        cellStyle.DataFormat = format.GetFormat("yyyy-m-d");                        
                        cell.CellStyle = cellStyle;                        
                        break;
                    case "System.Boolean"://布尔型
                        cell.SetCellValue((bool)val);
                        break;
                    case "System.Int16"://整型
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                    case "System.Decimal"://浮点型
                    case "System.Double":
                        cell.SetCellValue(Convert.ToDouble(val));
                        break;
                    case "System.DBNull"://空值处理
                        cell.SetCellValue("");
                        break;
                    default:
                        cell.SetCellValue("");
                        break;
                }
            }
            else
            {
                cell.SetCellValue("");
            }
        }
        private Dictionary<string, string> GetColumnsName(PropertyInfo[] infos, string[] availableFields, ref Dictionary<string, PropertyInfo> fieldNameMapping)
        {
            int exportFieldCount = availableFields.Length;
            Dictionary<string, string> columnsName = new Dictionary<string, string>();
            foreach (string field in availableFields)
            {
                foreach (PropertyInfo info in infos)
                {
                    if (field.ToLower() == info.Name.ToLower())
                    {
                        if (0 < info.GetCustomAttributes(typeof(ExportDisplayNameAttribute), false).Length)
                        {
                            ExportDisplayNameAttribute[] atts = (ExportDisplayNameAttribute[])info.GetCustomAttributes(typeof(ExportDisplayNameAttribute), false);
                            ExportDisplayNameAttribute[] nameAtt = atts.Where(t => !string.IsNullOrEmpty(t.Name)).ToArray<ExportDisplayNameAttribute>();
                            string displayName = nameAtt.Length > 0 ? nameAtt[0].Name : info.Name;
                            columnsName.Add(info.Name.ToLower(), displayName);
                        }
                        else
                        {
                            columnsName.Add(info.Name.ToLower(), "未指定名称");
                        }
                        fieldNameMapping.Add(info.Name.ToLower(), info);
                    }
                }
            }
            return columnsName;
        }
        private void InitializeWorkbook()
        {
            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "http://www.yungujia.com/";                        
            hssfWorkBook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "http://www.yungujia.com/";
            si.Title = "云估价";                        
            hssfWorkBook.SummaryInformation = si;
        }


    }
}
