using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.Reflection;
using System.Web;
using System.IO;
using CAS.Entity.AttributeHelper;
using Aspose.Cells.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CAS.Common.Aspose
{
    public class ExcelHelper
    {
        #region 初始化Workbook、Worksheet
        public Workbook workbook;
        public Worksheet sheet;
        Style TitleStyle, DataStyle;
        private string excelFilePath;
        /// <summary>
        /// 初始化 没有指定文件
        /// </summary>
        public ExcelHelper()
        {
            workbook = new Workbook();
            sheet = workbook.Worksheets[0];
        }
        /// <summary>
        /// 初始化指定文件对象 sheet为默认第一个
        /// </summary>
        /// <param name="excelFilePath"></param>
        public ExcelHelper(string excelFilePath)
        {
            this.excelFilePath = excelFilePath;
            workbook = new Workbook(excelFilePath);
            sheet = workbook.Worksheets[0];
        }

        /// <summary>
        /// 初始化指定文件对象
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <param name="sheetName"></param>
        public ExcelHelper(string excelFilePath, string sheetName)
        {
            this.excelFilePath = excelFilePath;
            workbook = new Workbook(excelFilePath);
            sheet = workbook.Worksheets[sheetName];
            if (sheet == null)
                sheet = workbook.Worksheets[0];//没有取第一个
        }
        /// <summary>
        /// 传对象
        /// </summary>
        /// <param name="book">文档对象</param>
        /// <param name="sheet">sheet对象</param>
        public ExcelHelper(Workbook book, Worksheet sheet)
        {
            if (book != null)
                this.workbook = book;
            else
                book = new Workbook();

            if (sheet != null)
                this.sheet = sheet;
            else
                sheet = book.Worksheets[0];
        }
        #endregion

        #region  数据导出
        /// <summary>
        /// IEnumerable集合导出Excel
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="data">数据源</param>
        /// <param name="exportFileName">导出文件名称</param>
        /// <param name="excelHeaderArray">导出文件标题列</param>
        /// <param name="excelAttributeArray">导出文件标题列对应的对象属性</param>
        public void Export<T>(IEnumerable<T> data, string exportFileName, string[] excelHeaderArray, string[] excelAttributeArray)
        {
            PropertyInfo[] piT = typeof(T).GetProperties();
            int rows, columns = 0;//rows记录行，columns记录列，index Wookbook记录工作表的索引
            ExcelStyle();
            if (excelHeaderArray != null && excelAttributeArray != null && excelHeaderArray.Any() && excelAttributeArray.Any())
            {
                #region 根据传入的列名和属性名导出
                AddTitleColum(excelHeaderArray, sheet);
                for (int i = 0; i < excelAttributeArray.Length; i++)//一列列导出
                {
                    rows = 1;
                    sheet = workbook.Worksheets[0];
                    int index = 2;//rows记录行，columns记录列，index Wookbook记录工作表的索引
                    foreach (var pi in piT)
                    {
                        if (excelHeaderArray.Length >= i)
                        {
                            if (excelAttributeArray[i] == pi.Name)
                            {
                                foreach (var d in data)
                                {
                                    if (rows % 60001 == 0)//每六万条分一页
                                    {
                                        if (workbook.Worksheets["Sheet" + index] == null)//判断当前工作表是否存在
                                        {
                                            sheet = workbook.Worksheets.Add("Sheet" + index);
                                            AddTitleColum(excelHeaderArray, sheet);//新增工作表的同时将列插入
                                        }
                                        else
                                            sheet = workbook.Worksheets["Sheet" + index];
                                        index++;
                                        rows = 1;
                                    }
                                    sheet.Cells[rows, columns].Value = pi.GetValue(d, null) != null ? pi.GetValue(d, null).ToString() : "";
                                    sheet.Cells[rows, columns].SetStyle(DataStyle);
                                    if (i == excelAttributeArray.Length - 1)//当循环到最后一列时给整个Excel添加样式
                                    {
                                        sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
                                        sheet.AutoFitColumns();//自动适应
                                    }
                                    rows++;

                                }
                                columns++;
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 根据实体中自定义属性作为列名
                foreach (var pi in piT)
                {
                    string headerText = null;
                    rows = 1;
                    object[] attributes = pi.GetCustomAttributes(false);
                    foreach (object item in attributes)
                    {
                        if (item.ToString().IndexOf("ExportDisplayNameAttribute", StringComparison.Ordinal) > 0)
                        {
                            ExportDisplayNameAttribute atts = (ExportDisplayNameAttribute)item;
                            headerText = atts.Name;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(headerText))
                    {
                        headerText = pi.Name;
                    }
                    sheet.Cells[0, columns].Value = headerText;
                    sheet.Cells[0, columns].SetStyle(TitleStyle);
                    foreach (var d in data)
                    {
                        sheet.Cells[rows, columns].Value = pi.GetValue(d, null) != null ? pi.GetValue(d, null).ToString() : "";
                        sheet.Cells[rows, columns].SetStyle(DataStyle);
                        rows++;
                    }
                    columns++;
                }
                sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
                sheet.AutoFitColumns();//自动适应
                #endregion
            }
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.Charset = "utf-8";
            context.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString()));
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "application/ms-excel";
            context.Response.BinaryWrite(workbook.SaveToStream().ToArray());
            context.Response.End();
        }
        /// <summary>
        ///  导出Excel 指定保存的url
        ///  kingfer 20161117
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="data">数据源</param>
        /// <param name="exportFileName">导出文件名称</param>
        /// <param name="excelHeaderArray">导出文件标题列</param>
        /// <param name="excelAttributeArray">导出文件标题列对应的对象属性</param>
        /// <param name="SavePath">要保存的地址(虚拟路径)</param>
        public int Export<T>(IEnumerable<T> data, string exportFileName, string[] excelHeaderArray, string[] excelAttributeArray, string SavePath)
        {
            int result = 0;
            try
            {
                PropertyInfo[] piT = typeof(T).GetProperties();
                int rows, columns = 0;//rows记录行，columns记录列，index Wookbook记录工作表的索引
                ExcelStyle();
                if (excelHeaderArray != null && excelAttributeArray != null && excelHeaderArray.Any() && excelAttributeArray.Any())
                {
                    #region 根据传入的列名和属性名导出
                    AddTitleColum(excelHeaderArray, sheet);
                    for (int i = 0; i < excelAttributeArray.Length; i++)//一列列导出
                    {
                        rows = 1;
                        sheet = workbook.Worksheets[0];
                        int index = 2;//rows记录行，columns记录列，index Wookbook记录工作表的索引
                        foreach (var pi in piT)
                        {
                            if (excelHeaderArray.Length >= i)
                            {
                                if (excelAttributeArray[i] == pi.Name)
                                {
                                    foreach (var d in data)
                                    {
                                        if (rows % 60001 == 0)//每六万条分一页
                                        {
                                            if (workbook.Worksheets["Sheet" + index] == null)//判断当前工作表是否存在
                                            {
                                                sheet = workbook.Worksheets.Add("Sheet" + index);
                                                AddTitleColum(excelHeaderArray, sheet);//新增工作表的同时将列插入
                                            }
                                            else
                                                sheet = workbook.Worksheets["Sheet" + index];
                                            index++;
                                            rows = 1;
                                        }
                                        sheet.Cells[rows, columns].Value = pi.GetValue(d, null) != null ? pi.GetValue(d, null).ToString() : "";
                                        sheet.Cells[rows, columns].SetStyle(DataStyle);
                                        if (i == excelAttributeArray.Length - 1)//当循环到最后一列时给整个Excel添加样式
                                        {
                                            sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
                                            sheet.AutoFitColumns();//自动适应
                                        }
                                        rows++;

                                    }
                                    columns++;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 根据实体中自定义属性作为列名
                    foreach (var pi in piT)
                    {
                        string headerText = null;
                        rows = 1;
                        object[] attributes = pi.GetCustomAttributes(false);
                        foreach (object item in attributes)
                        {
                            if (item.ToString().IndexOf("ExportDisplayNameAttribute", StringComparison.Ordinal) > 0)
                            {
                                ExportDisplayNameAttribute atts = (ExportDisplayNameAttribute)item;
                                headerText = atts.Name;
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(headerText))
                        {
                            headerText = pi.Name;
                        }
                        sheet.Cells[0, columns].Value = headerText;
                        sheet.Cells[0, columns].SetStyle(TitleStyle);
                        foreach (var d in data)
                        {
                            sheet.Cells[rows, columns].Value = pi.GetValue(d, null) != null ? pi.GetValue(d, null).ToString() : "";
                            sheet.Cells[rows, columns].SetStyle(DataStyle);
                            rows++;
                        }
                        columns++;
                    }
                    sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
                    sheet.AutoFitColumns();//自动适应
                    #endregion
                }
                using (MemoryStream ms = workbook.SaveToStream())
                {
                    using (FileStream fs = new FileStream(SavePath, FileMode.Create, FileAccess.Write))
                    {
                        int readResult;
                        while ((readResult = ms.ReadByte()) != -1)
                        {
                            fs.WriteByte((Byte)readResult);
                        }
                    }
                }
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                AsposeHelper.Info(ex.Message);
            }
            return result;
        }
        #region 设置导出单元列
        /// <summary>
        /// 设置导出单元列
        /// </summary>
        /// <param name="excelHeaderArray">列名数组</param>
        /// <param name="sheet">需要设置的工作表对象</param>
        public void AddTitleColum(string[] excelHeaderArray, Worksheet sheet)
        {
            for (int i = 0; i < excelHeaderArray.Length; i++)
            {
                sheet.Cells[0, i].Value = excelHeaderArray[i];
                sheet.Cells[0, i].SetStyle(TitleStyle);
            }
        }

        /// <summary>
        /// 设置导出单元列
        /// </summary>
        /// <param name="excelHeaderArray">DataTable列名集合</param>
        /// <param name="sheet">需要设置的工作表对象</param>
        public void AddTitleColum(System.Data.DataColumnCollection excelHeaderArray, Worksheet sheet)
        {
            for (int i = 0; i < excelHeaderArray.Count; i++)
            {
                sheet.Cells[0, i].Value = excelHeaderArray[i].ColumnName;
                sheet.Cells[0, i].SetStyle(TitleStyle);
            }
        }
        #endregion
        /// <summary>
        /// DataTable导出Excel
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="exportFileName">导出文件名称</param>
        public void Export(System.Data.DataTable data, string exportFileName)
        {
            Export(data, exportFileName, null, null);
        }
        /// <summary>
        /// 需要合并行的导出 目前主要用于系统中的自定义导出
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="exportFileName">导出文件名称</param>
        /// <param name="mergeRow">合并单元格用到的数组，标识要合并多少列</param>
        /// <param name="mergeId">根据哪一列分隔每个业务的标识</param>
        /// <param name="savePath">生成的文件要保存的路径，传了值的话则不已流输出，传了此参数时exportFileName参数就无用了</param>
        public void Export(System.Data.DataTable data, string exportFileName, List<int[]> mergeRow, int? mergeId, string savePath = null)
        {
            Cells cells = sheet.Cells;//单元格 
            int Rows = data.Rows.Count, Columns = data.Columns.Count;
            ExcelStyle();
            #region 将表格中的数据导入Excel
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sheet.Cells[0, j].Value = data.Columns[j].ColumnName;
                    sheet.Cells[0, j].SetStyle(TitleStyle);
                    sheet.Cells[i + 1, j].Value = data.Rows[i][j];
                    sheet.Cells[i + 1, j].SetStyle(DataStyle);
                }
            }
            #endregion
            #region 对Excel进行处理
            //判断data导出有没有数据
            if (Rows > 0 && data.Rows[0][1].ToString() != "没有找到查询条件相匹配的数据。")
            {
                if (null != mergeRow && 0 < mergeRow.Count && mergeId != null)
                {
                    //合并单元格
                    for (int i = 0; i < mergeRow.Count; i++)
                    {
                        MergeRow(Rows, mergeRow[i][0], mergeRow[i][1], mergeId);
                    }
                    ////合并单元格
                    //for (int j = 0; j <= mergeRow[0][1]; j++)
                    //{
                    //    MergeRow(Rows, j, 1, 1, separatorGuid);
                    //}
                }
            }
            else
                cells.Merge(1, 1, 1, Columns - 1);//合并单元格
            if (mergeId != null)
            {
                /*删除合并单元格时用于标示是否为同一业务数据的列*/
                cells.DeleteColumn(Convert.ToInt32(mergeId));
                /* 导出bug 循环删除行太慢
                //删除加有separatorGuid标识的行
                for (int i = 0; i < Rows; i++)
                {
                    if (cells[i, 0].Value != null && cells[i, 0].Value.ToString() == separatorGuid)
                    {
                        cells.DeleteRow(i);
                    }
                }*/
            }
            #endregion
            sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
            sheet.AutoFitColumns();
            if (!string.IsNullOrEmpty(savePath))
            {

                workbook.Save(savePath);
            }
            else
            {
                System.Web.HttpContext Context = System.Web.HttpContext.Current;
                Context.Response.Clear();
                Context.Response.Buffer = true;
                Context.Response.Charset = "utf-8";
                Context.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString()));
                Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                Context.Response.ContentType = "application/ms-excel";
                Context.Response.BinaryWrite(workbook.SaveToStream().ToArray());
                //Context.Response.End();//使用此方法会中止请求进程导致异常抛出 潘锦发20160322
                Context.ApplicationInstance.CompleteRequest();
            }
        }
        ///// <summary>
        ///// 合并单元格  递归次数多之后报错
        ///// </summary>
        ///// <param name="sheet"></param>
        ///// <param name="rows"></param>
        ///// <param name="column"></param>
        ///// <param name="i"></param>
        ///// <param name="fristrow"></param>
        ///// <param name="separatorGuid"></param>
        //public void MergeRow(int rows, int column, int i, int fristrow, string separatorGuid)
        //{
        //    Cells cells = sheet.Cells;//单元格 
        //    if (fristrow < rows && i < rows)
        //    {
        //        if (cells[fristrow + i, column].Value != null && cells[fristrow + i, column].Value.ToString() == separatorGuid)
        //        {
        //            //cells.DeleteRow(fristrow + i);
        //            fristrow = fristrow + i + 1;
        //            MergeRow(rows, column, 1, fristrow, separatorGuid);
        //        }
        //        else if (cells[fristrow, column].Value == cells[fristrow + i, column].Value)
        //        {
        //            cells.Merge(fristrow, column, i + 1, 1);//合并单元格 
        //            i++;
        //            MergeRow(rows, column, i, fristrow, separatorGuid);
        //        }
        //    }
        //}
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="rows">总行数(不包括标题头部)</param>
        /// <param name="Scolumn">要合并的起始列</param>
        /// <param name="Ecolumn">要合并的结束列</param>
        /// <param name="mergeId">根据哪一列分隔每个业务的标识</param>
        public void MergeRow(int rows, int Scolumn, int Ecolumn, int? mergeId)
        {
            for (int column = Scolumn; column <= Ecolumn; column++)
            {
                //如果当前列是要分隔每个业务的标识列则不进行合并判断
                if (mergeId == column)
                {
                    continue;
                }
                int fristrow = 0//起始行
                    , mergerow = 1;//需要合并的行数
                for (int row = 0; row < rows; row++)
                {
                    Cells cells = sheet.Cells;//单元格 
                    /*导出bug
                    if (cells[row, column].Value != null && cells[row, column].Value.ToString() == separatorGuid)
                    {
                        fristrow = row + 1;
                        mergerow = 1;
                    }
                    else if (cells[fristrow, column].Value == cells[fristrow + mergerow, column].Value)
                    {
                        cells.Merge(fristrow, column, mergerow+1, 1);//合并单元格 
                        mergerow++;
                    }
                    */
                    /*当前行与下一行不属于同一条业务*/
                    if (cells[row, Convert.ToInt32(mergeId)].Value.ToString() != cells[row + 1, Convert.ToInt32(mergeId)].Value.ToString())
                    {
                        fristrow = row + 1;
                        mergerow = 1;
                    }/*当前行当前列值==下一行当前列值&&当前行与下一行属于同一条业务*/
                    else if (cells[fristrow, column].Value == cells[fristrow + mergerow, column].Value && cells[fristrow, Convert.ToInt32(mergeId)].Value == cells[fristrow + mergerow, Convert.ToInt32(mergeId)].Value)
                    {
                        cells.Merge(fristrow, column, mergerow + 1, 1);//合并单元格 
                        mergerow++;
                    }
                }
            }
        }
        #endregion

        #region 样式设置
        /// <summary>
        /// excel样式
        /// </summary>
        public void ExcelStyle()
        {
            #region 标题列样式
            TitleStyle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            TitleStyle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            TitleStyle.Font.Name = "宋体";//文字字体 
            TitleStyle.Font.Size = 11;//文字大小 
            TitleStyle.Font.IsBold = true;//粗体
            TitleStyle.ForegroundColor = System.Drawing.Color.WhiteSmoke;
            TitleStyle.Pattern = BackgroundType.Solid;
            TitleStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            TitleStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            TitleStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            TitleStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            #endregion
            #region 数据列样式
            DataStyle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            DataStyle.Font.Name = "宋体";//文字字体 
            DataStyle.IsTextWrapped = true;//单元格内容自动换行 
            //DataStyle.Custom = "yyyy-mm-dd";或者 DataStyle.Number = 1; //单元格格式 
            DataStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            DataStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            DataStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            DataStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            #endregion
        }
        #endregion

        #region 数据读取

        /// <summary>
        /// 读取excel数据
        /// </summary>
        /// <param name="sheetName">要获取的sheet名称，默认第一个</param>
        /// <returns>返回datatable</returns>
        public System.Data.DataTable ReadExcel()
        {
            return ReadExcel(null);
        }

        /// <summary>
        /// 读取excel数据
        /// </summary>
        /// <param name="sheetName">要获取的sheet名称，默认第一个</param>
        /// <returns>返回datatable</returns>
        public System.Data.DataTable ReadExcel(string sheetName = null)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                if (sheet != null)
                {
                    Cells cells = sheet.Cells;
                    return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
                }
                else
                { return new System.Data.DataTable(); }
            }
            else
            {
                Worksheet _sheet = workbook.Worksheets[sheetName];
                if (_sheet != null)
                {
                    Cells cells = _sheet.Cells;
                    return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
                }
                else
                { return new System.Data.DataTable(); }
            }
        }

        /// <summary>
        /// datatable转excel
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="firstRow">开始行 从1开始</param>
        /// <param name="firstColumn">开始列 从1开始</param>
        public void DataTableToExcel(System.Data.DataTable table, int firstRow, int firstColumn)
        {
            firstRow = firstRow <= 1 ? 1 : firstRow;
            firstColumn = firstColumn <= 1 ? 1 : firstColumn;
            //删除所有行
            sheet.Cells.DeleteRows(0, sheet.Cells.Rows.Count);
            sheet.Cells.ImportDataTable(table, true, firstRow - 1, firstColumn - 1);
        }

        /// <summary>
        /// 获取命名管理器
        /// </summary>
        public Dictionary<string, string> GetNames()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //获取所有名称管理器
            Range[] rangeArray = workbook.Worksheets.GetNamedRanges();
            if (rangeArray != null)
            {
                foreach (Range range in rangeArray)
                {
                    dic.Add(range.Name, range.GetCellOrNull(0, 0).DisplayStringValue.Trim());
                }
            }
            return dic;
        }

        /// <summary>
        /// 设置命名管理器的值
        /// </summary>
        /// <param name="dicExcel">包含数据源的</param>
        /// <param name="result">装载数据的新容器</param>
        /// <param name="isNewDictionary">是否将数据存放到新容器返回税费用的</param>
        /// <param name="notWriteToExcelArry">不写入Excel，只从Excel中读取的值</param>
        public void GetNameValues(Dictionary<string, string> dicExcel, Dictionary<string, string> result = null,
            bool isNewDictionary = false, string[] notWriteToExcelArry = null)
        {
            //获取所有名称管理器
            Range[] rangeArray = workbook.Worksheets.GetNamedRanges();
            if (rangeArray != null)
            {
                #region 设置值

                string formula = ""; //单元格自定义公式
                foreach (Range range in rangeArray)
                {
                    try
                    {
                        Cell cell = range[0, 0];
                        string name = "";
                        name = range.Name.Contains("!") ? range.Name.Split('!')[1] : range.Name;
                        //if (name == "土地使用权证" || name == "不动产权证号" || name == "应补地价" || name == "出让金说明")
                        //    name = name;
                        formula = cell.Formula ?? "";
                        //如果自定义公式中包括[DBNum2][$-804]G/通用格式  就将他转化为英文的[DBNum2][$-804]General aspose.Cells的CalculateFormula才支持
                        if (formula.ToLower().Contains("g/通用格式"))
                        {
                            formula = formula.ToLower().Replace("g/通用格式", "General");
                        }
                        if (formula.ToLower().Contains("\"[dbnum2]\""))
                        {
                            formula = formula.ToLower().Replace("[dbnum2]", "[DBNum2][$-804]General");
                        }
                        //中诚新的的测算表中的这个=IF(MAX(比181:比183)/MIN(比181:比183)<1.2,ROUND((比181+比182+比183)/3,0),\"比准价格之间相差20%\这个公式在aspose中被认为是无效的  目前不知道啥原因；又发现河源市鑫安中的这个公式=MIN(IF(AS10:AS23>AV12,AS10:AS23))--2016-12-22
                        //目前发现包含MAX和MIN这两个函数时会出现问题  --2016-12-22 
                        if (formula != "" && !formula.ToLower().Contains("max(") && !formula.ToLower().Contains("min("))//&&formula!="=IF(MAX(比181:比183)/MIN(比181:比183)<1.2,ROUND((比181+比182+比183)/3,0),\"比准价格之间相差20%\")"
                            cell.Formula = formula.Replace("rmb", "RMB");
                        //else if (r1c1formula.Contains("G/通用格式"))
                        //{
                        //    cell.Formula = r1c1formula.Replace("G/通用格式", "General");
                        //}

                        if ((notWriteToExcelArry == null || !notWriteToExcelArry.Contains(range.Name)) && dicExcel.ContainsKey(name))
                        {
                            Style style = cell.GetStyle();
                            //写入值
                            //if (!string.IsNullOrEmpty(dicExcel[name]))
                            //{
                            PutExcelValue(cell, style, dicExcel[name], isNewDictionary);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        AsposeHelper.Info(ex + "名称管理器名称：" + range.Name);
                    }
                }

                #endregion

                //计算引用值
                workbook.CalculateFormula(true);

                #region 处理特殊值

                foreach (Range range in rangeArray)
                {
                    try
                    {
                        Cell cell = range[0, 0];
                        //Style style = cell.GetStyle();
                        //string custom = style.CultureCustom != null ? style.CultureCustom.ToLower() : "";

                        //string name = "";
                        //if (range.Name.Contains("!"))
                        //    name = range.Name.Split('!')[1] ?? range.Name;
                        //else
                        //    name = range.Name;
                        //if (name == "出让单价" || name == "比182" || name == "出让总价" || name == "出让金说明")
                        //    name = name;
                        if (cell.Formula != null)
                        {
                            //对于公式“=TEXT(B63,"[dbnum1]")”无法计算的和公式中既有[DBNum1]又有[DBNum2]的做特殊处理
                            if (cell.Formula.ToLower().Contains("=text(") || (cell.Formula.ToLower().Contains("[dbnum2]") && cell.Formula.ToLower().Contains("[dbnum1]")) || (cell.Formula.ToLower().Contains("[dbnum2]") && cell.Formula.ToLower().Contains("value(")) || (cell.Formula.ToLower().Contains("[dbnum1]") && cell.Formula.ToLower().Contains("value("))||cell.Formula.ToLower().Contains("[dbnum1];d[dbnum1]"))
                            {
                                #region 对于这样的公式处理：“大写：人民币"&TEXT(净值万元大写,"[DBNum2]")&"元整。",""”

                                //Regex regex = new Regex(@"(&)[^&&]+(&)");
                                //foreach (Match m in regex.Matches(cell.Formula))
                                //{
                                //    string key = m.Value.Replace("&", "");
                                //    if (key.Contains("(") && key.Contains(")") && key.Contains(","))
                                //        //对于这样的公式处理：“大写：人民币"&TEXT(净值万元大写,"[DBNum2]")&"元整。",""”
                                //        key = key.Split('(')[1].Split(',')[0];
                                //    if (key.Contains("大写") && workbook.Worksheets.GetRangeByName(key) != null)
                                //    {
                                //        ExcelDataToDic(dicExcel, result, workbook.Worksheets.GetRangeByName(key),
                                //            isNewDictionary);
                                //        if (dicExcel.Keys.Contains(key))
                                //            cell.Formula =
                                //                cell.Formula.Replace("\"" + m.Value + "\"", dicExcel[key])
                                //                    .Replace("rmb", "RMB");
                                //    }
                                //}
                                FormulaText(cell, dicExcel, result, isNewDictionary);

                                #endregion
                            }
                            else if (cell.Formula.ToUpper().Contains("NUMBERSTRING"))
                            {
                                FormulaNumberstring(cell, dicExcel, result, isNewDictionary);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AsposeHelper.Info(ex + "名称管理器名称：" + range.Name);
                    }
                }

                #endregion

                //计算引用值
                workbook.CalculateFormula(true);
                #region 取值

                rangeArray = workbook.Worksheets.GetNamedRanges();
                foreach (Range range in rangeArray)
                {
                    try
                    {
                        //string name = range.Name.Contains("!") ? range.Name.Split('!')[1] : range.Name;
                        //if (name == "总价转换" || name == "土地面积" || name == "土地使用权证描述" || name == "出让金说明")
                        //    name = name;
                        ExcelDataToDic(dicExcel, result, range, isNewDictionary);
                    }
                    catch (Exception ex)
                    {
                        AsposeHelper.Info(ex + "名称管理器名称：" + range.Name);
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// 值填入excel
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="style"></param>
        /// <param name="value"></param>
        /// <param name="isNewDictionary"></param>
        private void PutExcelValue(Cell cell, Style style, string value,bool isNewDictionary)
        {
            if ((cell.Type == CellValueType.IsNumeric || cell.NumberCategoryType == NumberCategoryType.Number ||
                 cell.NumberCategoryType == NumberCategoryType.Scientific ||
                 cell.NumberCategoryType == NumberCategoryType.General ||
                 cell.NumberCategoryType == NumberCategoryType.Fraction) &&
                (AsposeHelper.TryGetDecimal(value) || value == ""))
            {
                if (isNewDictionary)//如果是税费计算
                {
                    if (value != "")//税费计算中不能带入""
                        cell.PutValue(AsposeHelper.TryGetDecimalValue(value));
                }
                //else if (value == "")//等于空的话可能导致某些计算公式计算不了
                //    cell.PutValue(value);
                else{
                    if (value != "")
                        cell.PutValue(AsposeHelper.TryGetDecimalValue(value));//如果这个地方不做转换的话会导致Excel中的数字类型变成文本类型 
                }
                //if (value != "")//税费计算中不能带入""
                //    cell.PutValue(AsposeHelper.TryGetDecimalValue(value));
                //else if (cell.StringValue != "" && !AsposeHelper.TryGetDecimal(cell.StringValue))
                //    cell.PutValue(value);
            }
            else if ((style.IsDateTime || cell.Type == CellValueType.IsDateTime || cell.NumberCategoryType == NumberCategoryType.Date) && AsposeHelper.TryGetDateTime(value))
                cell.PutValue(Convert.ToDateTime(value));
            else if (cell.Type == CellValueType.IsBool && AsposeHelper.TryGetbool(value))
                cell.PutValue(Convert.ToBoolean(value));
            else if (cell.Type == CellValueType.IsNull || cell.Type == CellValueType.IsString)
            {
                cell.PutValue(value);
            }
            else
            {
                cell.PutValue(value);
            }
        }

        /// <summary>
        /// 获取excel中的图片
        /// </summary>
        /// <returns></returns>
        public List<byte[]> GetPictureData()
        {
            //读取excel中图片
            PictureCollection pictures = sheet.Pictures;
            return pictures.Select(pic => pic.Data).ToList();
        }
        /// <summary>
        /// 读取excel指定工作表中图片
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public List<byte[]> GetPictureData(string sheetName)
        {
            List<byte[]> imgData = new List<byte[]>();
            if (workbook.Worksheets[sheetName] != null)
            {
                //读取excel中图片
                sheet = workbook.Worksheets[sheetName];
                PictureCollection pictures = sheet.Pictures;
                imgData.AddRange(pictures.Select(pic => pic.Data));
            }
            return imgData;
        }
        #endregion

        #region 将图片生成到Excel中
        /// <summary>
        /// 将图片生成到Excel中
        /// 修改 :潘锦发 0160317  将根目录获取文件方式改成 OSS方式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="dicExcelImg">字典</param>
        public void SetPictureToExcel(string filePath, Dictionary<string, string> dicExcelImg)
        {
            if (dicExcelImg.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dicExcelImg)//由于图片的字典数据相对所有的名称管理器来说比较少所以遍历字段
                {
                    if (!string.IsNullOrEmpty(kv.Value) && File.Exists(HttpContext.Current.Server.MapPath(filePath + Path.GetFileName(kv.Value))))
                    {
                        Range rang = workbook.Worksheets.GetRangeByName(kv.Key);//获取指定区域
                        if (rang != null)
                        {
                            int lastRow = rang.FirstRow + rang.RowCount, lastColumn = rang.FirstColumn + rang.ColumnCount;//获取区域的最后的行和列
                            sheet.Pictures.Add(rang.FirstRow, rang.FirstColumn, lastRow, lastColumn, HttpContext.Current.Server.MapPath(filePath + Path.GetFileName(kv.Value)));//插入图片
                        }
                    }
                }
            }
        }
        #endregion

        #region 将图片生成到Excel中
        /// <summary>
        /// 将图片生成到Excel中
        /// 修改 :潘锦发 20160317  将根目录获取文件方式改成 OSS方式
        /// 重载去掉路径参数
        /// </summary>
        /// <param name="dicExcelImg">字典(包含路径)</param>
        public void SetPictureToExcel(Dictionary<string, string> dicExcelImg)
        {
            if (dicExcelImg.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dicExcelImg)//由于图片的字典数据相对所有的名称管理器来说比较少所以遍历字段
                {
                    if (!string.IsNullOrEmpty(kv.Value) && File.Exists(HttpContext.Current.Server.MapPath(kv.Value)) && !kv.Key.Contains("可比实例"))
                    {
                        Range rang = workbook.Worksheets.GetRangeByName(kv.Key);//获取指定区域
                        if (rang != null)
                        {
                            int lastRow = rang.FirstRow + rang.RowCount, lastColumn = rang.FirstColumn + rang.ColumnCount;//获取区域的最后的行和列
                            sheet.Pictures.Add(rang.FirstRow, rang.FirstColumn, lastRow, lastColumn, HttpContext.Current.Server.MapPath(kv.Value));//插入图片

                        }
                    }
                }
            }
        }
        #endregion

        #region 将图片生成到Excel中
        /// <summary>
        /// 将图片生成到Excel中
        /// 重载添加工作表的参数
        /// </summary>
        /// <param name="dicExcelImg">字典(包含路径)</param>
        public void SetPictureToExcel(Dictionary<string, string> dicExcelImg, string sheetName)
        {
            if ((Worksheet)workbook.Worksheets[sheetName] != null)
            {
                sheet = (Worksheet)workbook.Worksheets[sheetName];
                string name = sheetName.Replace("照片", "_");
                if (dicExcelImg.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kv in dicExcelImg)//由于图片的字典数据相对所有的名称管理器来说比较少所以遍历字段
                    {
                        if (!string.IsNullOrEmpty(kv.Value) && File.Exists(HttpContext.Current.Server.MapPath(kv.Value)) && kv.Key.Contains(name))
                        {
                            Range rang = workbook.Worksheets.GetRangeByName(kv.Key);//获取指定区域
                            if (rang != null)
                            {
                                int lastRow = rang.FirstRow + rang.RowCount, lastColumn = rang.FirstColumn + rang.ColumnCount;//获取区域的最后的行和列
                                sheet.Pictures.Add(rang.FirstRow, rang.FirstColumn, lastRow, lastColumn, HttpContext.Current.Server.MapPath(kv.Value));//插入图片
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Excel动态生成表格统一处理

        /// <summary>
        /// Excel动态生成表格统一处理 张磊 20151122
        /// </summary>
        /// <param name="multipleDic"></param>
        /// <param name="tableName">名称管理器名称</param>
        public void AutoFillTable(List<Dictionary<string, string>> multipleDic, string tableName)
        {
            int index = 0;
            Range range = workbook.Worksheets.GetRangeByName(tableName);
            if (range != null && multipleDic != null)
            {
                int a = range.FirstColumn, b = range.FirstRow, count = range.ColumnCount;
                string[] arr = new string[count];
                for (int i = 0; i < count; i++)
                {
                    if (multipleDic.Count <= 0)
                    {
                        if (sheet.Cells[b, a + i].Value.ToString().Contains("{") && sheet.Cells[b, a + i].Value.ToString().Contains("}"))
                            sheet.Cells[b, a + i].Value = "";
                    }
                    else
                    {
                        if (sheet.Cells[b, a + i] != null && sheet.Cells[b, a + i].Value != null)
                            arr[i] = sheet.Cells[b, a + i].Value.ToString();
                        else
                            arr[i] = "";
                    }
                }
                foreach (Dictionary<string, string> multipleObject in multipleDic)
                {
                    sheet.Cells.InsertRow(b + 2 + index);
                    sheet.Cells.CopyRow(sheet.Cells, b + 1 + index, b + 2 + index);
                    sheet.Cells.CopyRow(sheet.Cells, b + index, b + 1 + index);//改为复制整行
                    #region 复制指定区域 会将下一行的合并单元格的样式弄坏
                    // worksheet.Cells.InsertRow(b + 1 + index);
                    //Aspose.Cells.Range rangecopy = worksheet.Cells.CreateRange(b + 1 + index, a, b + 1 + index, a + count - 1);
                    //rangecopy.Name = tableName + index.ToString();
                    //if (index != 0)
                    //    rangecopy.Copy(workbook.Worksheets.GetRangeByName(tableName + index.ToString()));
                    //else
                    //    rangecopy.Copy(workbook.Worksheets.GetRangeByName(tableName));
                    #endregion

                    for (int i = 0; i < count; i++)
                    {
                        // sheet.Cells[b + 1 + index, a + i].Value = arr[i].Replace("}", index + 1 + "}");
                        string key = arr[i].Replace("}", "").Replace("{", "");
                        sheet.Cells[b + 1 + index, a + i].Value = multipleObject.ContainsKey(key) ? multipleObject[key] : "";
                    }
                    index++;
                }
                //替换委估对象行以外的数据
                foreach (KeyValuePair<string, string> kv in multipleDic[0])
                {
                    sheet.Replace("{" + kv.Key + "}", kv.Value);
                }
                ////替换委估对象行以外的数据
                //Cells cells = sheet.Cells;
                //for (int i = 0; i < cells.MaxDataRow + 1; i++)
                //{
                //    for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                //    {
                //        if (cells[i, j].StringValue.Contains("{") && cells[i, j].StringValue.Contains("}"))
                //        {

                //            string key = cells[i, j].StringValue.Replace("}", "").Replace("{", "");
                //            sheet.Cells[i, j].Value = multipleDic[0].ContainsKey(key)
                //                ? multipleDic[0][key]
                //                : sheet.Cells[b + 1 + index, a + i].Value;
                //        }
                //    }
                //}
                if (index != 0)
                    sheet.Cells.DeleteRow(b);
            }
        }
        #endregion

        #region 操作Worksheet
        /// <summary>
        /// 对excel的行删除
        /// </summary>
        /// <param name="rowIndex">要删除哪一行</param>
        /// <param name="sheetName">要删除行的sheet名称,默认第一个</param>
        /// <returns></returns>
        public void DeleteRow(int rowIndex, string sheetName = null)
        {
            (string.IsNullOrEmpty(sheetName) ? sheet : workbook.Worksheets[sheetName]).Cells.DeleteRow(rowIndex);
        }
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="style">样式类</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="sheetName">要删除行的sheet名称,默认第一个</param>
        public void SetSheetStyle(Style style, int row, int column, string sheetName = null)
        {
            (string.IsNullOrEmpty(sheetName) ? sheet : workbook.Worksheets[sheetName]).Cells[row, column].SetStyle(style);
        }
        /// <summary>
        /// 删除一个sheet
        /// </summary>
        /// <param name="sheetName"></param>
        public void DelSheet(string sheetName)
        {
            workbook.Worksheets.RemoveAt(sheetName);
        }
        #endregion

        #region Excel的值转换值到字典

        private void ExcelDataToDic(Dictionary<string, string> dicExcel, Dictionary<string, string> result, Range range,
            bool isNewDictionary)
        {
            Cell cell = range[0, 0];
            Style style = cell.GetStyle();
            string custom = style.CultureCustom != null ? style.CultureCustom.ToLower() : "";
            string formula = cell.Formula != null ? cell.Formula.ToLower() : "";

            // 判断该名称管理器是否是全局的  不是则进行分隔取出正确的名字
            string name = range.Name.Contains("!") ? range.Name.Split('!')[1] : range.Name;

            if ((custom.IndexOf("年", StringComparison.Ordinal) >= 0 && cell.StringValue != "") || custom == "[$-f800]dddd\\,\\ mmmm\\ dd\\,\\ yyyy")
            {
                AsposeHelper.AddItem(isNewDictionary ? result : dicExcel, name,
                    custom.IndexOf("dbnum1", StringComparison.Ordinal) >= 0
                        ? AsposeHelper.ConvertMoreDate(cell.StringValue, "dbnum1")
                        : custom.IndexOf("yyyy", StringComparison.Ordinal) >= 0 && custom.IndexOf("mm", StringComparison.Ordinal) >= 0 && custom != "[$-f800]dddd\\,\\ mmmm\\ dd\\,\\ yyyy" ? AsposeHelper.ConvertMoreDate(cell.StringValue, "mm") : AsposeHelper.ConvertMoreDate(cell.StringValue));
            }
            else if (custom.IndexOf("dbnum1", StringComparison.Ordinal) >= 0 || formula.IndexOf("dbnum1", StringComparison.Ordinal) >= 0 && formula.IndexOf("dbnum2", StringComparison.Ordinal) < 0 && formula.IndexOf("value(", StringComparison.Ordinal) < 0)//当只有dbnum1时则将该单元格中的所有数组转换为小写
            {
                AsposeHelper.AddItem(isNewDictionary ? result : dicExcel, name, AsposeHelper.MoreFormatWithCulture(cell.StringValue, 1));
            }
            else if (custom.IndexOf("dbnum2", StringComparison.Ordinal) >= 0 || formula.IndexOf("dbnum2", StringComparison.Ordinal) >= 0 && formula.IndexOf("dbnum1", StringComparison.Ordinal) < 0 && formula.IndexOf("value(", StringComparison.Ordinal) < 0)//当只有dbnum2时则将该单元格中的所有数组转换为大写
            {
                string svalue="";
                //特殊处理桐乡中正公司的收费附件中的“=TEXT(应收金额*100,"[dbnum2]"&RIGHT("0亿0仟0佰0拾0万0仟0佰0拾0元0角0分",LEN(ABS(INT(应收金额)))*2+4)&";￥负[dbnum2]"&RIGHT("0亿0仟0佰0拾0万0仟0佰0拾0元0角0分",LEN(ABS(INT(应收金额)))*2+4))”公司
                if (formula.IndexOf("\"[dbnum2][$-804]general\"&right(\"0亿0仟0佰0拾0万0仟0佰0拾0元0角0分\"", StringComparison.Ordinal) >= 0)
                {
                    Regex regex1 = new Regex(@"[0-9]{1,}");
                    foreach (Match m in regex1.Matches(cell.StringValue))
                    {
                        svalue = m.Value.Substring(0, m.Value.Length - 1);
                        break;
                    }
                    AsposeHelper.AddItem(isNewDictionary ? result : dicExcel, name, AsposeHelper.MoreCmycurD(svalue=="0"?cell.StringValue.Replace("00", "0"):cell.StringValue.Replace(svalue, "")));
                }
                else
                {
                    AsposeHelper.AddItem(isNewDictionary ? result : dicExcel, name,
                        AsposeHelper.MoreCmycurD(cell.StringValue));
                }
                //InternationalNumericFormatter.FormatWithCulture("L", Convert.ToDecimal(cell.StringValue), null, new CultureInfo("zh-CHS")));
            }
            else
            {
                AsposeHelper.AddItem(isNewDictionary ? result : dicExcel, name, AsposeHelper.TryGetDecimal(cell.DisplayStringValue) ? cell.DisplayStringValue.Trim() : cell.DisplayStringValue);
            }
        }

        #endregion

        #region 处理aspose中无法转换的NUMBERSTRING公式

        /// <summary>
        /// 处理aspose中无法转换的NUMBERSTRING公式
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="dicExcel">包含数据源的</param>
        /// <param name="result">装载数据的新容器</param>
        /// <param name="isNewDictionary">是否装入新容器</param>
        private void FormulaNumberstring(Cell cell, Dictionary<string, string> dicExcel, Dictionary<string, string> result, bool isNewDictionary)
        {
            #region 对于这样的公式处理：NUMBERSTRING(室,3)

            Regex regex1 = new Regex(@"(NUMBERSTRING\()[^NUMBERSTRING\(\)]+(\))");
            foreach (Match m in regex1.Matches(cell.Formula.ToUpper()))
            {
                if (!m.Value.Contains(",") || !m.Value.Contains(")") || !m.Value.Contains("("))
                    continue;
                int num = AsposeHelper.TryGetInt(m.Value.Split(',')[1].Split(')')[0]);
                string key = m.Value.Split(',')[0].Split('(')[1];
                string beforeValue = RangeNameOrCellIndexByGetValue(key, dicExcel, result, isNewDictionary);
                if (beforeValue == null) continue;
                string value = "";
                switch (num)
                {
                    case 1:
                        value = beforeValue != "" ? AsposeHelper.FormatWithCulture(beforeValue) : "〇";//Excel中的〇是这样的○
                        break;
                    case 2:
                        value = beforeValue != "" ? AsposeHelper.CmycurD(beforeValue) : "零";
                        break;
                    case 3:
                        value = beforeValue != "" ? AsposeHelper.SplitFormatWithCulture(beforeValue) : "〇";
                        break;
                }
                ReplaceFormula(cell, m.Value, value);
            }

            #endregion
        }
        #endregion

        #region 处理aspose中无法转换的TEXT公式

        /// <summary>
        /// 处理aspose中无法转换的TEXT公式
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="dicExcel">包含数据源的</param>
        /// <param name="result">装载数据的新容器</param>
        /// <param name="isNewDictionary">是否装入新容器</param>
        private void FormulaText(Cell cell, Dictionary<string, string> dicExcel, Dictionary<string, string> result, bool isNewDictionary)
        {

            #region 对于这样的公式处理：TEXT(评估厂房总幢数,"[dbnum1]")

            Regex regex1 = new Regex(@"(TEXT\()[^TEXT\(\)]+(\))");
            foreach (Match m in regex1.Matches(cell.Formula))
            {
                if (!m.Value.Contains(",") || !m.Value.Contains(")") || !m.Value.Contains("(")) continue;
                string num = m.Value.Split(',')[1].Split(')')[0];
                string key = m.Value.Split(',')[0].Split('(')[1];
                string beforeValue = RangeNameOrCellIndexByGetValue(key, dicExcel, result, isNewDictionary);
                if (beforeValue == null) continue;
                string value = "";
                switch (num.ToLower())
                {
                    case "\"[dbnum2][$-804]general\"":
                    case "\"[dbnum2]\"":
                        value = beforeValue != "" ? AsposeHelper.CmycurD(beforeValue) : "零";
                        break;
                    case "\"[>20][dbnum1];d[dbnum1]\""://中诚信德的楼层情况1字段
                    case "\"[dbnum1][$-804]general\"":
                    case "\"[dbnum1]\"":
                        value = beforeValue != "" ? AsposeHelper.FormatWithCulture(beforeValue) : "〇";
                        break;
                    case "\"yyyy年m月d日\"":
                        value = beforeValue != "" ? AsposeHelper.ConvertDate(beforeValue, "m") : "";
                        break;
                    case "\"yyyy年mm月dd日\"":
                        value = beforeValue != "" ? AsposeHelper.ConvertDate(beforeValue, "mm") : "";
                        break;
                    case "\"[dbnum1]e年m月d日\"":
                        value = beforeValue != "" ? AsposeHelper.ConvertDate(beforeValue, "dbnum1") : "";
                        break;
                }
                ReplaceFormula(cell, m.Value, value);
            }

            #endregion
        }

        #endregion

        #region 根据excel中的名称管理器的名称或者excel中的单元格索引来获取值
        /// <summary>
        /// 根据excel中的名称管理器的名称或者excel中的单元格索引来获取值
        /// </summary>
        /// <param name="key">单元格</param>
        /// <param name="dicExcel">包含数据源的</param>
        /// <param name="result">装载数据的新容器</param>
        /// <param name="isNewDictionary">是否装入新容器</param>
        /// <returns></returns>
        private string RangeNameOrCellIndexByGetValue(string key, Dictionary<string, string> dicExcel, Dictionary<string, string> result, bool isNewDictionary)
        {
            string beforeValue = "", name = key;
            int value = 0;
            if (key.Contains("*")) //=NUMBERSTRING(装修净值万元*10000,2)
            {
                value = AsposeHelper.TryGetInt(key.Split('*')[1]);
                name = key.Split('*')[0];
            }
            else if (key.Contains("+"))
            {
                value = AsposeHelper.TryGetInt(key.Split('+')[1]);
                name = key.Split('+')[0];
            }
            if (workbook.Worksheets.GetRangeByName(name) != null)//如果公式里面放的是名称管理器的名称
            {
                ExcelDataToDic(dicExcel, result, workbook.Worksheets.GetRangeByName(name), isNewDictionary);
                if (isNewDictionary)
                {
                    if (result.Keys.Contains(name))
                        beforeValue = result[name];
                }
                else
                {
                    if (dicExcel.Keys.Contains(name))
                        beforeValue = dicExcel[name];
                }
            }
            else
                beforeValue = sheet.Cells[name].Value != null ? sheet.Cells[name].Value.ToString() : ""; //如果公式里面放的是B12这样的索引则获取该索引的值
            if (value != 0 && key.Contains("*"))
                beforeValue = (AsposeHelper.TryGetDouble(beforeValue) * value).ToString(CultureInfo.InvariantCulture);
            else if (value != 0 && key.Contains("+"))
                beforeValue = (AsposeHelper.TryGetDouble(beforeValue) + value).ToString(CultureInfo.InvariantCulture);
            return beforeValue;
        }

        #endregion

        #region 根据一定的规律将Formula替换为转换后的值

        /// <summary>
        /// 根据一定的规律将Formula替换为转换后的值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="oldValue">旧公式</param>
        /// <param name="newValue">新值</param>
        private void ReplaceFormula(Cell cell, string oldValue, string newValue)
        {
            int fristindex = cell.Formula.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);
            int lastindex = cell.Formula.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase) + oldValue.Length;
            string replacevalue = oldValue;
            //向前匹配
            for (int i = fristindex - 1; i >= 0; i--)
            {
                if (cell.Formula[i].ToString() == "," || cell.Formula[i].ToString() == "(")//当遇到，终止向前查找
                {
                    break;
                }
                if (cell.Formula[i].ToString() == ")")//当遇到)终止向前查找
                {
                    if (replacevalue.Substring(0, 1) == "&")//如果前面有&则替换掉
                        replacevalue = replacevalue.Substring(1, replacevalue.Length - 1);
                    break;
                }
                if (cell.Formula[i].ToString() == "\"")//遇到引号则终止并且将引号拼接入
                {
                    replacevalue = cell.Formula[i] + replacevalue;
                    break;
                }
                replacevalue = cell.Formula[i] + replacevalue;
            }
            //向后匹配
            for (int i = lastindex; i <= cell.Formula.Length - 1; i++)
            {
                if (cell.Formula[i].ToString() == "," || cell.Formula[i].ToString() == ")")//当遇到，和）时终止向后查找
                {
                    break;
                }
                if (cell.Formula[i].ToString() == "\"")//遇到引号则终止并且将引号拼接入
                {
                    replacevalue = replacevalue + cell.Formula[i];
                    break;
                }
                replacevalue = replacevalue + cell.Formula[i];
            }
            if (replacevalue.IndexOf("=", StringComparison.OrdinalIgnoreCase) == 0 && replacevalue.Substring(replacevalue.Length - 1, 1) != "\"")//当有等号且最后不为引号就直接将值替换掉
                cell.Formula = cell.Formula.Replace(replacevalue, "\"" + newValue + "\"");
            else if (replacevalue.Substring(0, 1) == "\"" && replacevalue.Substring(replacevalue.Length - 1, 1) == "\"")//当前面后面都是引号时则直接替换
                cell.Formula = cell.Formula.Replace(replacevalue, newValue);
            else if (replacevalue.Substring(0, 1) == "\"")//当只有后面为引号是则替换后在后面也添加引号
                cell.Formula = cell.Formula.Replace(replacevalue, newValue + "\"");
            else if (replacevalue.Substring(replacevalue.Length - 1, 1) == "\"")//当只有前面为引号是则替换后在前面也添加引号
                cell.Formula = cell.Formula.Replace(replacevalue, "\"" + newValue);
            else
                cell.Formula = cell.Formula.Replace(replacevalue, "\"" + newValue + "\"");
        }

        #endregion

    }
}
