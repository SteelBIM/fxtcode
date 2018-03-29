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
using Microsoft.International.Formatters;
using System.Globalization;

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
            sheet = (Worksheet)workbook.Worksheets[0];
        }
        /// <summary>
        /// 初始化指定文件对象 sheet为默认第一个
        /// </summary>
        /// <param name="excelFilePath"></param>
        public ExcelHelper(string excelFilePath)
        {
            this.excelFilePath = excelFilePath;
            workbook = new Workbook(excelFilePath);
            sheet = (Worksheet)workbook.Worksheets[0];
        }
        /// <summary>
        /// 初始化指定文件对象
        /// </summary>
        /// <param name="excelFilePath"></param>
        public ExcelHelper(string excelFilePath,string sheetName)
        {
            this.excelFilePath = excelFilePath;
            workbook = new Workbook(excelFilePath);
            sheet = (Worksheet)workbook.Worksheets[sheetName];
            if(sheet==null)
                sheet = (Worksheet)workbook.Worksheets[0];//没有取第一个
        }
        /// <summary>
        /// 传对象
        /// </summary>
        /// <param name="book">文档对象</param>
        /// <param name="sheet">sheet对象</param>
        public ExcelHelper(Workbook book, Worksheet sheet)
        {
            if(book!=null)
                this.workbook = book;
            else
                book = new Workbook();

            if(sheet!=null)
                this.sheet = sheet;
            else
                sheet = (Worksheet)book.Worksheets[0];
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
            int rows = 1, Columns = 0,index=2;//rows记录行，columns记录列，index Wookbook记录工作表的索引
            ExcelStyle();
            if (excelHeaderArray != null && excelHeaderArray != null && excelHeaderArray.Count() > 0 && excelHeaderArray.Count() > 0)
            {
                #region 根据传入的列名和属性名导出
                AddTitleColum(excelHeaderArray, sheet);
                for (int i = 0; i < excelAttributeArray.Length; i++)//一列列导出
                {
                    rows = 1;
                    sheet = workbook.Worksheets[0];
                    index = 2;
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
                                    sheet.Cells[rows, Columns].Value = pi.GetValue(d, null) != null ? pi.GetValue(d, null).ToString() : "";
                                    sheet.Cells[rows, Columns].SetStyle(DataStyle);
                                    if (i == excelAttributeArray.Length - 1)//当循环到最后一列时给整个Excel添加样式
                                    {
                                        sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
                                        sheet.AutoFitColumns();//自动适应
                                    }
                                    rows++;

                                }
                                Columns++;
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
                        if (item.ToString().IndexOf("ExportDisplayNameAttribute") > 0)
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
                    sheet.Cells[0, Columns].Value = headerText;
                    sheet.Cells[0, Columns].SetStyle(TitleStyle);
                    foreach (var d in data)
                    {
                        sheet.Cells[rows, Columns].Value = pi.GetValue(d, null) != null ? pi.GetValue(d, null).ToString() : "";
                        sheet.Cells[rows, Columns].SetStyle(DataStyle);
                        rows++;
                    }
                    Columns++;
                }
                sheet.FreezePanes(1, 1, 1, 0);//冻结第一行
                sheet.AutoFitColumns();//自动适应
                #endregion
            }
            System.Web.HttpContext Context = System.Web.HttpContext.Current;
            Context.Response.Clear();
            Context.Response.Buffer = true;
            Context.Response.Charset = "utf-8";
            Context.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString()));
            Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            Context.Response.ContentType = "application/ms-excel";
            Context.Response.BinaryWrite(workbook.SaveToStream().ToArray());
            Context.Response.End();
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
        /// <param name="strFileName">附件路径(包含附件名)</param>
        /// <returns>返回table</returns>
        public System.Data.DataTable ReadExcel()
        {
            if (sheet != null)
            {
                Cells cells = sheet.Cells;
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            }
            else
            { return new System.Data.DataTable(); }
        }
        /// <summary>
        /// datatable转excel
        /// </summary>
        /// <param name="excelFilePath">输出文件路径</param>
        /// <param name="table">数据表</param>
        /// <param name="beginRow">开始行 从1开始</param>
        /// <param name="beginColumn">开始列 从1开始</param>
        public void DataTableToExcel(System.Data.DataTable table, int firstRow, int firstColumn)
        {
            firstRow = firstRow <= 1 ? 1 : firstRow;
            firstColumn = firstColumn <= 1 ? 1 : firstColumn;
            //删除所有行
            sheet.Cells.DeleteRows(0,sheet.Cells.Rows.Count);
            sheet.Cells.ImportDataTable(table,true, firstRow-1, firstColumn-1);
        }
        /// <summary>
        /// 获取命名管理器
        /// </summary>
        /// <param name="dicExcel"></param>
        public Dictionary<string, string> GetNames()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //获取所有名称管理器
            Range[] rangeArray = workbook.Worksheets.GetNamedRanges();
            if (rangeArray != null)
            {
                foreach (Range range in rangeArray)
                {
                    dic.Add(range.Name,range.GetCellOrNull(0,0).DisplayStringValue.Trim());
                }
            }
            return dic;
        }
        /// <summary>
        /// 设置命名管理器的值
        /// </summary>
        /// <param name="dicExcel">包含数据源的</param>
        /// <param name="result">装载数据的新容器</param>
        /// <param name="isNewDictionary">是否将数据存放到新容器返回</param>
        /// <param name="NotWriteToExcelArry">不写入Excel，只从Excel中读取的值</param>
        public void GetNameValues(Dictionary<string, string> dicExcel, Dictionary<string, string> result=null, bool isNewDictionary=false,string[] NotWriteToExcelArry=null)
        {
            Dictionary<string, string> GetData = dicExcel;
            if (isNewDictionary)
                GetData = result;
            string r1c1formula = "";//单元格自定义公式
            //获取所有名称管理器
            Range[]  rangeArray=workbook.Worksheets.GetNamedRanges();
            if (rangeArray != null)
            {
                #region 设置值
                foreach (Range range in rangeArray)
                {
                    Cell cell = range[0, 0];
                    r1c1formula = cell.R1C1Formula != null ? cell.R1C1Formula : "";
                    //如果自定义公式中包括[DBNum2][$-804]G/通用格式  就将他转化为英文的[DBNum2][$-804]General aspose.Cells的CalculateFormula才支持
                    if (r1c1formula.Contains("G/通用格式"))
                    {
                        cell.R1C1Formula = r1c1formula.Replace("G/通用格式", "General");
                    }
                    //else if (r1c1formula.Contains("G/通用格式"))
                    //{
                    //    cell.R1C1Formula = r1c1formula.Replace("G/通用格式", "General");
                    //}
                    if ((NotWriteToExcelArry==null || (NotWriteToExcelArry != null && !NotWriteToExcelArry.Contains(range.Name))) && dicExcel.ContainsKey(range.Name))
                    {
                        Style style = cell.GetStyle();
                        //写入值
                        if (!string.IsNullOrEmpty(dicExcel[range.Name]))
                        {
                            PutExcelValue(cell, style, dicExcel[range.Name]);
                        }
                    }
                }
                #endregion 
                //计算引用值
                workbook.CalculateFormula();
                #region 取值
                rangeArray = workbook.Worksheets.GetNamedRanges();
                foreach (Range range in rangeArray)
                {
                    Cell cell = range[0, 0];
                    Style style = cell.GetStyle();
                    string custom = style.CultureCustom != null ? style.CultureCustom.ToLower() : "";
                    r1c1formula = cell.R1C1Formula != null ? cell.R1C1Formula.ToLower() : "";

                    //string formula = string.Format("=Text({0},\"{1}\")", cell.Name, custom);
                    //object testO = sheet.CalculateFormula(formula);
                    if (custom.IndexOf("年") > 0 && custom.IndexOf("dbnum1") > 0)
                    {
                        AsposeHelper.AddItem(GetData, range.Name, AsposeHelper.ConvertDate(cell.StringValue));
                    }
                    else if (custom.IndexOf("dbnum1") > 0)
                    {
                        decimal d = 0;
                        if (decimal.TryParse(cell.StringValue, out d))
                            AsposeHelper.AddItem(GetData, range.Name, InternationalNumericFormatter.FormatWithCulture("Ln", Convert.ToDecimal(cell.StringValue), null, new CultureInfo("zh-CHS")));
                    }
                    else if (custom.IndexOf("dbnum2") > 0 || r1c1formula.IndexOf("dbnum2") > 0)
                    {
                        decimal d = 0;
                        if (decimal.TryParse(cell.StringValue, out d))
                            AsposeHelper.AddItem(GetData, range.Name, InternationalNumericFormatter.FormatWithCulture("L", Convert.ToDecimal(cell.StringValue), null, new CultureInfo("zh-CHS")));
                    }
                    else
                    {
                        AsposeHelper.AddItem(GetData, range.Name, cell.DisplayStringValue.Trim());
                    }

                    if (r1c1formula.Contains("General"))
                    { r1c1formula.Replace("General", "G/通用格式"); }
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
        private void PutExcelValue(Cell cell, Style style, string value)
        {
            if ((cell.Type == CellValueType.IsNumeric || cell.NumberCategoryType == NumberCategoryType.Number) && AsposeHelper.TryGetDecimal(value))
                cell.PutValue(Convert.ToDecimal(value));
            else if ((style.IsDateTime || cell.Type == CellValueType.IsDateTime || cell.NumberCategoryType == NumberCategoryType.Date) && AsposeHelper.TryGetDateTime(value))
                cell.PutValue(Convert.ToDateTime(value));
            else if (cell.Type == CellValueType.IsBool && AsposeHelper.TryGetbool(value))
                cell.PutValue(Convert.ToBoolean(value));
            else if (cell.Type == CellValueType.IsNull || cell.Type == CellValueType.IsString)
                cell.PutValue(value);
        }

        /// <summary>
        /// 获取excel中的图片
        /// </summary>
        /// <returns></returns>
        public List<byte[]> GetPictureData()
        {
            List<byte[]> imgData = new List<byte[]>();
            //读取excel中图片
            PictureCollection pictures = sheet.Pictures;
            foreach (Picture pic in pictures)
            {
                imgData.Add(pic.Data);
            }
            return imgData;
        }
        #endregion

        #region 将图片生成到Excel中
        /// <summary>
        /// 将图片生成到Excel中
        /// 修改 :潘锦发 0160317  将根目录获取文件方式改成 OSS方式
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="dicExcelImg">字典</param>
        public void SetPictureToExcel(string FilePath,Dictionary<string, string> dicExcelImg)
        {
            if (dicExcelImg.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dicExcelImg)//由于图片的字典数据相对所有的名称管理器来说比较少所以遍历字段
                {
                    if (kv.Value != null && kv.Value != "" && File.Exists(HttpContext.Current.Server.MapPath(FilePath + Path.GetFileName(kv.Value))))
                    {
                        Range rang = workbook.Worksheets.GetRangeByName(kv.Key);//获取指定区域
                        if (rang != null)
                        {
                            int LastRow = rang.FirstRow + rang.RowCount, LastColumn = rang.FirstColumn + rang.ColumnCount;//获取区域的最后的行和列
                            sheet.Pictures.Add(rang.FirstRow, rang.FirstColumn, LastRow, LastColumn, HttpContext.Current.Server.MapPath(FilePath + Path.GetFileName(kv.Value)));//插入图片
                        }
                    }
                }
            }
        }
        #endregion 
    }
}
