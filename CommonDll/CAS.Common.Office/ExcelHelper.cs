using System;
using System.IO;
using System.Text;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

//using cfg = System.Configuration;

namespace CAS.Common.Office
{

    #region Common Excel Enum Properties
    /// <summary>
    /// HTML，CSV，TEXT，EXCEL，XML
    /// </summary>
    enum SaveAsFileFormat
    {
        HTML,
        CSV,
        TEXT,

        EXCEL,
        XML
    }
    /// <summary>
    /// 常用颜色定义,对就Excel中颜色名
    /// </summary>
    public enum ColorIndex
    {
        无色 = -4142,
        自动 = -4105,
        黑色 = 1,
        褐色 = 53,
        橄榄 = 52,
        深绿 = 51,
        深青 = 49,
        深蓝 = 11,
        靛蓝 = 55,
        灰色80 = 56,
        深红 = 9,
        橙色 = 46,

        深黄 = 12,
        绿色 = 10,
        青色 = 14,
        蓝色 = 5,
        蓝灰 = 47,
        灰色50 = 16,
        红色 = 3,
        浅橙色 = 45,
        酸橙色 = 43,
        海绿 = 50,
        水绿色 = 42,
        浅蓝 = 41,
        紫罗兰 = 13,
        灰色40 = 48,
        粉红 = 7,
        金色 = 44,
        黄色 = 6,
        鲜绿 = 4,

        青绿 = 8,
        天蓝 = 33,
        梅红 = 54,
        灰色25 = 15,
        玫瑰红 = 38,
        茶色 = 40,
        浅黄 = 36,
        浅绿 = 35,
        浅青绿 = 34,
        淡蓝 = 37,
        淡紫 = 39,
        白色 = 2
    }
    /// <summary>
    /// 水平对齐方式
    /// </summary>
    public enum ExcelHAlign
    {
        常规 = 1,
        靠左,


        居中,
        靠右,
        填充,
        两端对齐,
        跨列居中,
        分散对齐
    }

    /// <summary>
    /// 垂直对齐方式
    /// </summary>
    public enum ExcelVAlign
    {
        靠上 = 1,
        居中,
        靠下,
        两端对齐,
        分散对齐
    }

    /// <summary>
    /// 线粗
    /// </summary>
    public enum BorderWeight
    {
        极细 = 1,
        细 = 2,
        粗 = -4138,
        极粗 = 4
    }

    /// <summary>
    /// 线样式
    /// </summary>
    public enum LineStyle
    {
        连续直线 = 1,
        短线 = -4115,
        线点相间 = 4,
        短线间两点 = 5,
        点 = -4118,
        双线 = -4119,
        无 = -4142,
        少量倾斜点 = 13
    }

    /// <summary>  
    /// 下划线方式
    /// </summary>
    public enum UnderlineStyle
    {
        无下划线 = -4142,
        双线 = -4119,
        双线充满全格 = 5,
        单线 = 2,
        单线充满全格 = 4
    }

    /// <summary>
    /// 单元格填充方式
    /// </summary>
    public enum Pattern
    {
        Automatic = -4105,
        Checker = 9,
        CrissCross = 16,
        Down = -4121,
        Gray16 = 17,
        Gray25 = -4124,
        Gray50 = -4125,
        Gray75 = -4126,
        Gray8 = 18,
        Grid = 15,
        Horizontal = -4128,
        LightDown = 13,
        LightHorizontal = 11,
        LightUp = 14,
        LightVertical = 12,
        None = -4142,
        SemiGray75 = 10,
        Solid = 1,
        Up = -4162,
        Vertical = -4166
    }
    #endregion

    public class ExcelHelper
    {
        #region 成员变量
        private string templetFile = null;
        private string outputFile = null;
        private object missing = Missing.Value;
        private DateTime beforeTime;   //Excel启动之前时间
        private DateTime afterTime;    //Excel启动之后时间
        Excel.Application app;

        Excel.Workbook workBook;
        Excel.Worksheet workSheet;
        Excel.Range range;
        Excel.Range range1;
        Excel.Range range2;
        Excel.TextBox textBox;
        private int sheetCount = 1;   //WorkSheet数量
        private string sheetPrefixName = "页";
        private double inches_1 = 0.39370078740157;//一英寸
        #endregion

        #region 公共属性
        /// <summary>
        /// WorkSheet前缀名，比如：前缀名为“页”，那么WorkSheet名称依次为“页-1，页-2...”
        /// </summary> 

        public string SheetPrefixName
        {
            set { this.sheetPrefixName = value; }
        }

        /// <summary>
        /// WorkSheet数量
        /// </summary>
        public int WorkSheetCount
        {
            get { return workBook.Sheets.Count; }
        }

        /// <summary>
        /// Excel模板文件路径
        /// </summary>
        public string TempletFilePath
        {
            set { this.templetFile = value; }
        }

        /// <summary>
        /// 输出Excel文件路径
        /// </summary>
        public string OutputFilePath
        {
            set { this.outputFile = value; }
        }
        #endregion

        #region 公共方法

        #region ExcelHelper
        /// <summary>
        /// 构造函数，将一个已有Excel工作簿作为模板，并指定输出路径 


        /// </summary>
        /// <param name="templetFilePath">Excel模板文件路径</param>
        /// <param name="outputFilePath">输出Excel文件路径</param>
        public ExcelHelper(string templetFilePath, string outputFilePath)
        {
            if (templetFilePath == null)
                throw new Exception("Excel模板文件路径不能为空！路径为:" + templetFilePath);

            if (outputFilePath == null)
                throw new Exception("输出Excel文件路径不能为空！路径为:" +outputFilePath);

            if (!File.Exists(templetFilePath))
                throw new Exception("指定路径的Excel模板文件不存在！路径为:" + templetFilePath);

            this.templetFile = templetFilePath;
            this.outputFile = outputFilePath;

            //创建一个Application对象并使其可见
            beforeTime = DateTime.Now; 
            try
            {
                app = new Excel.ApplicationClass();
                app.Visible = false;
                afterTime = DateTime.Now;
                app.DisplayAlerts = false;

                //打开模板文件，得到WorkBook对象
                workBook = app.Workbooks.Open(templetFile, missing, missing, missing, missing, missing,
                    missing, missing, missing, missing, missing, missing, missing, Type.Missing, Type.Missing);

                //得到WorkSheet对象
                workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }
        }

        /// <summary>
        /// 构造函数，打开一个已有的工作簿
        /// </summary>
        /// <param name="fileName">Excel文件名</param>
        public ExcelHelper(string fileName)
        {
            if (!File.Exists(fileName))

                throw new Exception("指定路径的Excel文件不存在！ 路径为:" + fileName);

            //创建一个Application对象并使其可见
            beforeTime = DateTime.Now;
            //see： http://msdn.microsoft.com/en-us/library/microsoft.office.interop.excel.workbooks.open%28v=office.11%29.aspx   侯湘岳
            //打开一个WorkBook 
            //修改人：rock,20150713,为了防止在打开文件的时候就出异常导致该文件被进程占用，所以加异常捕捉并关闭
            try
            {
                app = new Excel.ApplicationClass();
                app.Visible = false;
                afterTime = DateTime.Now;
                app.DisplayAlerts = false;
                workBook = app.Workbooks.Open(fileName,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,

                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //得到WorkSheet对象
                workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);
            }
            catch (Exception ex)//为防止打开文件的时候就报错而占用文件进程
            {
                Quit();
                throw;
            }

        }

        /// <summary>
        /// 构造函数，新建一个工作簿
        /// </summary>
        public ExcelHelper()
        {
            //创建一个Application对象并使其可见 
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            app.Visible = false;
            afterTime = DateTime.Now;
            app.DisplayAlerts = false;
            //新建一个WorkBook
            workBook = app.Workbooks.Add(Type.Missing);

            //得到WorkSheet对象
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);

        }
        #endregion

        public ExcelHelper(bool empty)
        {
        }

        public void Replace(string strOld, string strNew)
        {

            try
            {
                object what = strOld;　 //查找字符串
                object retxt = strNew; //替换字符串
                workSheet.Cells.Replace(what, retxt, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            catch
            { }

        }

        /// <summary>
        /// 名称获取值，返回字典
        /// </summary>
        /// <param name="dict"></param>
        public Dictionary<string, string> GetNames()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (Excel.Name name in workBook.Names)
            {
                string n = name.Name.Contains("!") ? name.Name.Split('!')[1] : name.Name;
                if (!dict.ContainsKey(n) && name.Value.IndexOf("#REF!") < 0)
                {
                    try
                    {
                        var range = name.RefersToRange;     //当取到_Order1类似值时，RefersToRange会出现异常，情况未知。
                        dict.Add(n, range.Cells.Text.ToString());
                    }
                    catch
                    {
                        
                    }
                }
                NAR(name);
            }
            return dict;
        }

        /// <summary>
        /// 名称获取值，返回字典
        /// 修改人rock:20150522,添加自动适应列宽
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="isAutoFit">在获取值时是否自动适应单元格宽度，默认false</param>
        public void GetNameValues(Dictionary<string, string> dict, bool isAutoFit=false)
        {
            foreach (Excel.Name name in workBook.Names)
            {
                string n = name.Name.Contains("!") ? name.Name.Split('!')[1] : name.Name;
                try
                {
                    //当取到_Order1类似值时，RefersToRange会出现异常，情况未知。
                    if (!dict.ContainsKey(n) && name.Value.IndexOf("#REF!") < 0)
                    {
                        if (isAutoFit)
                        {
                            name.RefersToRange.Cells.EntireColumn.AutoFit();//自动适应列框（因为在如果日期时如果列宽过窄会返回#####这样的值）
                        }
                        dict.Add(n, name.RefersToRange.Cells.Text.ToString());
                    }
                    else if (dict.ContainsKey(n) && name.Value.IndexOf("#REF!") < 0)
                    {
                        if (isAutoFit)
                        {
                            name.RefersToRange.Cells.EntireColumn.AutoFit();//自动适应列框（因为在如果日期时如果列宽过窄会返回#####这样的值）
                        }
                        dict[n] = name.RefersToRange.Cells.Text.ToString();
                    }
                }
                catch
                {

                }
                NAR(name);
            }
        }


        /// <summary>
        /// 名称设置值
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="fvalue"></param>
        public void SetName(string fname, string fvalue)
        {
            foreach (Excel.Name name in workBook.Names)
            {
                string n = name.Name.Contains("!") ? name.Name.Split('!')[1] : name.Name;
                if (n == fname && name.Value.IndexOf("#REF!") < 0)
                {
                    name.RefersToRange.Cells.Value = fvalue;
                    NAR(name);
                    return;
                }
                NAR(name);
            }
        }

        /// <summary>
        /// 二维数据写入excel kevin
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void Array2ToExcel(string[,] ss, int top, int left, int rows, int cols)
        {
            Excel.Range range = (Excel.Range)this.workSheet.Cells[top, left];
            range = range.get_Resize(rows, cols);
            range.Value2 = ss;
        }

        #region Data Export Methods

        /// <summary>
        /// 将DataTable数据写入Excel文件（自动分页）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        public void DataTableToExcel(DataTable dt, int rows, int top, int left)
        {
            int rowCount = dt.Rows.Count; //DataTable行数
            int colCount = dt.Columns.Count; //DataTable列数 

            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数
            //   StringBuilder sb;

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            for (int i = 1; i <= sheetCount; i++)
            {
                int startRow = (i - 1) * rows; //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将dt中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = dt.Rows[startRow + j][k].ToString(); 
                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {

                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;

                #region 利用Windwo粘贴板批量拷贝数据（在Web下面行不通）
                /*sb = new StringBuilder(); 




    for(int j=0;j<endRow-startRow;j++)
    {
     for(int k=0;k<colCount;k++)
     {
      sb.Append( dt.Rows[startRow + j][k].ToString() );
      sb.Append("/t");
     }

     sb.Append("/n");
    }

    System.Windows.Forms.Clipboard.SetDataObject(sb.ToString());

    range = (Excel.Range)workSheet.Cells[top,left];
    workSheet.Paste(range,false);*/
                #endregion

            }
        }

        /// <summary>
        /// 将DataTable数据写入Excel文件（自动分页）重载方法 增加起始sheetindex参数 caoq 
        /// </summary>
        /// <param name="startSheetIndex">起始sheetindex</param>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        public void DataTableToExcel(int startSheetIndex, DataTable dt, int rows, int top, int left)
        {
            if (startSheetIndex > WorkSheetCount) startSheetIndex = WorkSheetCount;
            int rowCount = dt.Rows.Count; //DataTable行数
            int colCount = dt.Columns.Count; //DataTable列数 

            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数
            //   StringBuilder sb;

            //复制sheetCount-1个WorkSheet对象
            for (int i = startSheetIndex; i < sheetCount + startSheetIndex - 1; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }
            int foridx = 0;
            for (int i = startSheetIndex; i <= sheetCount + startSheetIndex - 1; i++)
            {
                foridx++;
                int startRow = (i - startSheetIndex) * rows; //记录起始行索引
                int endRow = (i - startSheetIndex + 1) * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if ((i - startSheetIndex + 1) == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + foridx.ToString();

                //将dt中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = dt.Rows[startRow + j][k].ToString(); 
                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {

                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }
                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;
            }
        }


        /// <summary>
        /// 将DataTable数据写入Excel文件（不分页） 
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        public void DataTableToExcel(DataTable dt, int top, int left)
        {
            int rowCount = dt.Rows.Count; //DataTable行数
            int colCount = dt.Columns.Count; //DataTable列数

            //利用二维数组批量写入
            string[,] arr = new string[rowCount, colCount];

            for (int j = 0; j < rowCount; j++)
            {
                for (int k = 0; k < colCount; k++)
                {
                    arr[j, k] = dt.Rows[j][k].ToString();
                }
            }

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);
            range.Value2 = arr;

        }

        /// <summary>
        /// 将DataTable数据写入Excel文件（不分页） 
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        /// <param name="showTitle">是否显示DataTable列名</param>
        /// <param name="liststr">数据有效性下拉列表,以,分割的字符串</param> 
        public void DataTableToExcel(DataTable dt, int top, int left, bool showTitle, string liststr, Dictionary<string, string> replaceDic)
        {
            if (!showTitle)
            {
                DataTableToExcel(dt, top, left);
            }
            else
            {
                int rowCount = dt.Rows.Count + 1; //DataTable行数
                int colCount = dt.Columns.Count; //DataTable列数

                //利用二维数组批量写入
                string[,] arr = new string[rowCount, colCount];
                string temp = "";
                for (int j = 0; j < rowCount; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        if (j == 0)
                        {
                            temp = dt.Columns[k].ColumnName.ToString();
                        }
                        else
                        {
                            temp = dt.Rows[j - 1][k].ToString();
                            if (k != 0 && replaceDic != null && replaceDic.ContainsKey(temp))
                            {
                                temp = replaceDic[temp];
                            }
                        }
                        arr[j, k] = temp;
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(rowCount, colCount);
                range.Value2 = arr;
                range.Font.Size = 9;
                range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.ColorIndex = 1;
                range.Columns.AutoFit();

                if (!string.IsNullOrEmpty(liststr))
                {
                    range = workSheet.get_Range(workSheet.Cells[2, 2], workSheet.Cells[rowCount, colCount]);
                    range.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Type.Missing, liststr, Type.Missing);
                }
                NAR(range);
            }
        }

        /// <summary>
        /// 将DataTable数据写入Excel文件（不分页） 
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        /// <param name="showTitle">是否显示DataTable列名</param>
        /// <param name="liststr">数据有效性下拉列表,以,分割的字符串</param> 
        public void DataTableToExcel(DataTable dt, int top, int left, bool showTitle, string liststr)
        {
            if (!showTitle)
            {
                DataTableToExcel(dt, top, left);
            }
            else
            {
                int rowCount = dt.Rows.Count + 1; //DataTable行数
                int colCount = dt.Columns.Count; //DataTable列数

                //利用二维数组批量写入
                string[,] arr = new string[rowCount, colCount];
                string temp = "";
                for (int j = 0; j < rowCount; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        if (j == 0)
                        {
                            temp = dt.Columns[k].ColumnName.ToString();
                        }
                        else
                        {
                            temp = dt.Rows[j - 1][k].ToString();
                        }
                        arr[j, k] = temp;
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(rowCount, colCount);
                range.Value2 = arr;
                if (!string.IsNullOrEmpty(liststr))
                {
                    range = workSheet.get_Range(workSheet.Cells[2, 2], workSheet.Cells[rowCount, colCount]);
                    range.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Type.Missing, liststr, Type.Missing);
                }
            }
        }

        /// <summary>
        /// 将DataTable数据写入Excel文件（自动分页，并指定要合并的列索引）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        /// <param name="mergeColumnIndex">DataTable中要合并相同行的列索引，从0开始</param>
        public void DataTableToExcel(DataTable dt, int rows, int top, int left, int mergeColumnIndex)
        {
            int rowCount = dt.Rows.Count; //源DataTable行数
            int colCount = dt.Columns.Count; //源DataTable列数
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数
            //   StringBuilder sb;

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);


            }

            for (int i = 1; i <= sheetCount; i++)
            {
                int startRow = (i - 1) * rows; //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名 
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将dt中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = dt.Rows[startRow + j][k].ToString(); 

                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {


                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;

                //合并相同行
                this.MergeRows(workSheet, left + mergeColumnIndex, top, rows);

            }
        }


        /// <summary>
        /// 将二维数组数据写入Excel文件（自动分页）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        public void ArrayToExcel(string[,] arr, int rows, int top, int left)
        {
            int rowCount = arr.GetLength(0); //二维数组行数（一维长度） 


            int colCount = arr.GetLength(1); //二维数据列数（二维长度）
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            //将二维数组数据写入Excel 


            for (int i = sheetCount; i >= 1; i--)
            {
                int startRow = (i - 1) * rows; //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名  
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将二维数组中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = arr[startRow + j,k]; 

                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {


                        ss[j, k] = arr[startRow + j, k];
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value2 = ss;
            }

        }//end ArrayToExcel


        /// <summary> 
        /// 将二维数组数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        public void ArrayToExcel(string[,] arr, int top, int left)
        {
            int rowCount = arr.GetLength(0); //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);
            range.FormulaArray = arr;

        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        /// <param name="isFormula">填充的数据是否需要计算</param>
        public void ArrayToExcel(string[,] arr, int top, int left, bool isFormula)
        {
            int rowCount = arr.GetLength(0); //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            //注意：使用range.FormulaArray写合并的单元格会出问题
            if (isFormula)
                range.FormulaArray = arr;
            else
                range.Value2 = arr;



        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页），合并指定列的相同行
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        /// <param name="isFormula">填充的数据是否需要计算</param>
        /// <param name="mergeColumnIndex">需要合并行的列索引</param>
        public void ArrayToExcel(string[,] arr, int top, int left, bool isFormula, int mergeColumnIndex)
        {
            int rowCount = arr.GetLength(0); //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            //注意：使用range.FormulaArray写合并的单元格会出问题
            if (isFormula)
                range.FormulaArray = arr;
            else
                range.Value2 = arr;

            this.MergeRows(workSheet, mergeColumnIndex, top, rowCount);

        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        public void ArrayToExcel(int sheetIndex, string[,] arr, int top, int left)
        {


            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            // 改变当前工作表
            this.workSheet = (Excel.Worksheet)this.workBook.Sheets.get_Item(sheetIndex);

            int rowCount = arr.GetLength(0); //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度） 


            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            range.Value2 = arr;

        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（自动分页，并指定要合并的列索引）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>  
        /// <param name="mergeColumnIndex">数组的二维索引，相当于DataTable的列索引，索引从0开始</param>
        public void ArrayToExcel(string[,] arr, int rows, int top, int left, int mergeColumnIndex)
        {
            int rowCount = arr.GetLength(0); //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            //将二维数组数据写入Excel
            for (int i = sheetCount; i >= 1; i--)
            {
                int startRow = (i - 1) * rows; //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数  
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将二维数组中的数据写入WorkSheet
                for (int j = 0; j < endRow - startRow; j++)
                {

                    for (int k = 0; k < colCount; k++)
                    {
                        workSheet.Cells[top + j, left + k] = arr[startRow + j, k];
                    }
                }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = arr[startRow + j, k];
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];


                range = range.get_Resize(row, colCount);
                range.Value2 = ss;

                //合并相同行
                this.MergeRows(workSheet, left + mergeColumnIndex, top, rows);
            }

        }//end ArrayToExcel
        #endregion



        #region WorkSheet Methods

        /// <summary>
        /// 改变当前工作表
        /// </summary> 
        /// <param name="sheetIndex">工作表索引</param>
        public void ChangeCurrentWorkSheet(int sheetIndex)
        {
            //若指定工作表索引超出范围，则不改变当前工作表
            if (sheetIndex < 1)
                return;

            if (sheetIndex > workBook.Worksheets.Count && sheetIndex > 1)
            {
                Excel.Worksheet before = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex - 1);
                workBook.Worksheets.Add(Type.Missing, before, Type.Missing, Type.Missing);
                NAR(before);
            }

            this.workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);
            //显示隐藏的SHEET
            if (this.workSheet.Visible == Excel.XlSheetVisibility.xlSheetHidden)
            {
                this.workSheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;
            }
            Excel.Range cell = (Excel.Range)this.workSheet.Cells[1, 1];
            app.Goto(cell, missing);
            NAR(cell);
        }


        /// <summary>
        /// 改变当前工作表
        /// </summary> 
        /// <param name="sheetName">工作表名</param>
        public bool ChangeCurrentWorkSheet(string sheetName)
        {
            //若指定工作表索引超出范围，则不改变当前工作表
            if (string.IsNullOrEmpty(sheetName))
                return false;
            bool find = false;
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                this.workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);
                if (this.workSheet.Name == sheetName)
                {
                    //显示隐藏的SHEET
                    if (this.workSheet.Visible == Excel.XlSheetVisibility.xlSheetHidden)
                    {
                        this.workSheet.Visible = Excel.XlSheetVisibility.xlSheetVisible;
                    }
                    Excel.Range cell = (Excel.Range)this.workSheet.Cells[1, 1];
                    app.Goto(cell, missing);
                    find = true;
                    NAR(cell);
                    break;
                }
            }
            return find;
        }
        /// <summary> 

        /// 隐藏指定名称的工作表
        /// </summary>
        /// <param name="sheetName">工作表名称</param>
        public void HiddenWorkSheet(string sheetName)
        {
            try
            {
                Excel.Worksheet sheet = null;
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);
                    if (workSheet.Name == sheetName)
                        sheet = workSheet;
                }
                if (sheet != null)
                    sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                else
                {
                }
                NAR(sheet);
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// 隐藏指定索引的工作表 

        /// </summary>
        /// <param name="sheetIndex"></param>
        public void HiddenWorkSheet(int sheetIndex)
        {
            Excel.Worksheet sheet = null;
            try
            {
                if (sheetIndex <= this.workBook.Worksheets.Count)
                {
                    sheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);
                    if (sheet.Visible == Excel.XlSheetVisibility.xlSheetVisible)
                        sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    NAR(sheet);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                NAR(sheet);
            }
        }


        /// <summary>
        /// 在指定名称的工作表后面拷贝指定个数的该工作表的副本，并重命名
        /// </summary>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="sheetCount">工作表个数</param>
        public void CopyWorkSheets(string sheetName, int sheetCount)
        {
            try
            {
                Excel.Worksheet sheet = null;
                int sheetIndex = 0;




                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                        sheetIndex = workSheet.Index;


                    }
                }

                if (sheet != null)
                {
                    for (int i = sheetCount; i >= 1; i--)
                    {
                        sheet.Copy(this.missing, sheet);
                    }

                    //重命名
                    for (int i = sheetIndex; i <= sheetIndex + sheetCount; i++)
                    {
                        workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);
                        workSheet.Name = sheetName + "-" + Convert.ToString(i - sheetIndex + 1);
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为[" + sheetName + "]的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 将一个工作表拷贝到另一个工作表后面，并重命名
        /// </summary>
        /// <param name="srcSheetIndex">拷贝源工作表索引</param>
        /// <param name="aimSheetIndex">参照位置工作表索引，新工作表拷贝在该工作表后面</param>
        /// <param name="newSheetName"></param>
        public void CopyWorkSheet(int srcSheetIndex, int aimSheetIndex, string newSheetName)
        {
            if (srcSheetIndex > this.WorkSheetCount || aimSheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                Excel.Worksheet srcSheet = (Excel.Worksheet)workBook.Sheets.get_Item(srcSheetIndex);
                Excel.Worksheet aimSheet = (Excel.Worksheet)workBook.Sheets.get_Item(aimSheetIndex);

                srcSheet.Copy(this.missing, aimSheet);



                //重命名
                workSheet = (Excel.Worksheet)aimSheet.Next; //获取新拷贝的工作表
                workSheet.Name = newSheetName;
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }


        /// <summary> 

        /// 根据名称删除工作表
        /// </summary>
        /// <param name="sheetName"></param>
        public void DeleteWorkSheet(string sheetName)
        {
            try
            {
                Excel.Worksheet sheet = null;

                //找到名称位sheetName的工作表
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                    }
                }

                if (sheet != null)
                {


                    sheet.Delete();
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为[" + sheetName + "[的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 根据索引删除工作表
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void DeleteWorkSheet(int sheetIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                Excel.Worksheet sheet = null;
                sheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);

                sheet.Delete();
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 按Index得到WorkSheet
        /// </summary>
        /// <param name="index">序号</param>       
        public Excel.Worksheet GetSheet(int index)
        {
            try
            {
                if (index > workBook.Sheets.Count && index > 1)
                {
                    workBook.Worksheets.Add(Type.Missing, workBook.Worksheets.get_Item(index - 1), Type.Missing, Type.Missing);
                }
                Excel.Worksheet aimSheet = (Excel.Worksheet)workBook.Sheets.get_Item(index);
                return aimSheet;
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }
        #endregion

        #region TextBox Methods
        /// <summary> 

        /// 向指定文本框写入数据，对每个WorkSheet操作
        /// </summary>
        /// <param name="textboxName">文本框名称</param>
        /// <param name="text">要写入的文本</param>
        public void SetTextBox(string textboxName, string text)
        {
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);


                try
                {
                    textBox = (Excel.TextBox)workSheet.TextBoxes(textboxName);
                    textBox.Text = text;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("不存在ID为[" + textboxName + "[的文本框！");
                }
            }
        }

        /// <summary>
        /// 向指定文本框写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="textboxName">文本框名称</param>
        /// <param name="text">要写入的文本</param>
        public void SetTextBox(int sheetIndex, string textboxName, string text)
        {
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);


            try
            {
                textBox = (Excel.TextBox)workSheet.TextBoxes(textboxName);
                textBox.Text = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("不存在ID为[" + textboxName + "[的文本框！");
            }
        }

        /// <summary>
        /// 向文本框写入数据，对每个WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存文本框的ID和数据</param>
        public void SetTextBoxes(Hashtable ht)
        {
            if (ht.Count == 0) return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                foreach (DictionaryEntry dic in ht)
                {
                    try
                    {
                        textBox = (Excel.TextBox)workSheet.TextBoxes(dic.Key);
                        textBox.Text = dic.Value.ToString();
                    }
                    catch
                    {
                        this.KillExcelProcess();
                        throw new Exception("不存在ID为[" + dic.Key.ToString() + "[的文本框！");
                    }
                }
            }
        }

        /// <summary>
        /// 向文本框写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存文本框的ID和数据</param> 
        public void SetTextBoxes(int sheetIndex, Hashtable ht)
        {
            if (ht.Count == 0) return;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            foreach (DictionaryEntry dic in ht)
            {
                try
                {
                    textBox = (Excel.TextBox)workSheet.TextBoxes(dic.Key);
                    textBox.Text = dic.Value.ToString();
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("不存在ID为[" + dic.Key.ToString() + "[的文本框！");
                }
            }
        }
        #endregion


        #region Cell Methods
        /// <summary>
        /// 向单元格写入数据，对当前WorkSheet操作
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param> 
        /// <param name="text">要写入的文本值</param>
        public void SetCells(int rowIndex, int columnIndex, string text)
        {
            try
            {
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        ///  向单元格写入数据，对当前WorkSheet操作
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        /// <param name="fontsize">文本字体大小</param>
        public void SetCells(int rowIndex, int columnIndex, string text, int fontsize)
        {
            try
            {
                workSheet.Cells[rowIndex, columnIndex] = text;
                Excel.Range range = (Excel.Range)workSheet.Cells[rowIndex, columnIndex];
                range.Font.Size = fontsize;//设置字体大小  
                NAR(range);
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        /// 向单元格写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        public void SetCells(int sheetIndex, int rowIndex, int columnIndex, string text)
        {
            try
            {
                this.ChangeCurrentWorkSheet(sheetIndex); //改变当前工作表为指定工作表
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        /// 向单元格写入数据，对每个WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (ht.Count == 0) return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                foreach (DictionaryEntry dic in ht)
                {
                    try
                    {
                        position = dic.Key.ToString();
                        rowIndex = Convert.ToInt32(position.Split(',')[0]);



                        columnIndex = Convert.ToInt32(position.Split(',')[1]);

                        workSheet.Cells[rowIndex, columnIndex] = dic.Value;
                    }
                    catch
                    {
                        this.KillExcelProcess();
                        throw new Exception("向单元格[" + dic.Key + "]写数据出错！");

                    }
                }
            }
        }

        /// <summary>
        /// 向单元格写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(int sheetIndex, Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            if (ht.Count == 0) return;

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            foreach (DictionaryEntry dic in ht)
            {
                try
                {
                    position = dic.Key.ToString();
                    rowIndex = Convert.ToInt32(position.Split(',')[0]);
                    columnIndex = Convert.ToInt32(position.Split(',')[1]);

                    workSheet.Cells[rowIndex, columnIndex] = dic.Value;
                }

                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("向单元格[" + dic.Key + "]写数据出错！");
                }
            }
        }

        /// <summary>
        /// 设置单元格为可计算的
        /// </summary>
        /// <remarks> 


        /// 如果Excel的单元格格式设置为数字，日期或者其他类型时，需要设置这些单元格的FormulaR1C1属性，
        /// 否则写到这些单元格的数据将不会按照预先设定的格式显示
        /// </remarks>
        /// <param name="arr">保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(int sheetIndex, string[] arr)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (sheetIndex > this.WorkSheetCount)
            {

                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            if (arr.Length == 0) return;

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            for (int i = 0; i < arr.Length; i++)
            {
                try
                {
                    position = arr[i];

                    rowIndex = Convert.ToInt32(position.Split(',')[0]);
                    columnIndex = Convert.ToInt32(position.Split(',')[1]);

                    Excel.Range cell = (Excel.Range)workSheet.Cells[rowIndex, columnIndex];
                    cell.FormulaR1C1 = cell.Text;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception(string.Format("计算单元格{0}出错！", arr[i]));
                }
            }
        }

        /// <summary>
        /// 向单元格写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(string sheetName, Hashtable ht)
        {
            int rowIndex;


            int columnIndex;
            string position;
            Excel.Worksheet sheet = null;
            int sheetIndex = 0;

            if (ht.Count == 0) return;

            try
            {
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                        sheetIndex = workSheet.Index;
                    }
                }

                if (sheet != null)
                {
                    foreach (DictionaryEntry dic in ht)
                    {
                        try
                        {
                            position = dic.Key.ToString();
                            rowIndex = Convert.ToInt32(position.Split(',')[0]);
                            columnIndex = Convert.ToInt32(position.Split(',')[1]);



                            sheet.Cells[rowIndex, columnIndex] = dic.Value;
                        }
                        catch
                        {
                            this.KillExcelProcess();
                            throw new Exception("向单元格[" + dic.Key + "]写数据出错！");
                        }
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为[" + sheetName + "[的工作表不存在");
                }
            }


            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }


        /// <summary>
        /// 合并单元格，并赋值，对每个WorkSheet操作
        /// </summary>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param> 
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        public void MergeCells(int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);


                range.ClearContents(); //先把Range内容清除，合并才不会出错
                range.MergeCells = true;
                range.Value2 = text;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            }
        }

        /// <summary>
        /// 合并单元格，并赋值，对指定WorkSheet操作
        /// </summary> 
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        public void MergeCells(int sheetIndex, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
            range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents(); //先把Range内容清除，合并才不会出错
            range.MergeCells = true;

            range.Value2 = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// 合并单元格，并赋值，对指定WorkSheet操作
        /// </summary> 
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        public void MergeCellsNoFormat(int sheetIndex, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
            range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);


            range.Value2 = text;

        }
        #endregion

        #region Row Methods
        /// <summary>
        /// 将指定索引列的数据相同的行合并，对每个WorkSheet操作
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="endRowIndex">结束行索引</param> 

        public void MergeRows(int columnIndex, int beginRowIndex, int endRowIndex)
        {
            if (endRowIndex - beginRowIndex < 1)
                return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                int beginIndex = beginRowIndex;
                int count = 0;
                string text1;
                string text2;
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                for (int j = beginRowIndex; j <= endRowIndex; j++)
                {
                    range = (Excel.Range)workSheet.Cells[j, columnIndex];
                    text1 = range.Text.ToString();

                    range = (Excel.Range)workSheet.Cells[j + 1, columnIndex];
                    text2 = range.Text.ToString();


                    if (text1 == text2)
                    {
                        ++count;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            this.MergeCells(workSheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                        }

                        beginIndex = j + 1; //设置开始合并行索引
                        count = 0; //计数器清0
                    }

                }

            }
        }


        /// <summary>
        /// 将指定索引列的数据相同的行合并，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        public void MergeRows(int sheetIndex, int columnIndex, int beginRowIndex, int endRowIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            if (endRowIndex - beginRowIndex < 1)
                return;

            int beginIndex = beginRowIndex;
            int count = 0;
            string text1;
            string text2;
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            for (int j = beginRowIndex; j <= endRowIndex; j++)
            {
                range = (Excel.Range)workSheet.Cells[j, columnIndex];
                text1 = range.Text.ToString();

                range = (Excel.Range)workSheet.Cells[j + 1, columnIndex];
                text2 = range.Text.ToString();

                if (text1 == text2)
                {
                    ++count;
                }
                else
                {
                    if (count > 0)
                    {
                        this.MergeCells(workSheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                    }



                    beginIndex = j + 1; //设置开始合并行索引
                    count = 0; //计数器清0
                }

            }

        }


        /// <summary>
        /// 插行（在指定行上面插入指定数量行）
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void InsertRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 0; i < count; i++)
                    {
                        range.Insert(Excel.XlDirection.xlDown, missing);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary> 


        /// 插行（在指定WorkSheet指定行上面插入指定数量行）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void InsertRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");

            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 0; i < count; i++)
                {
                    range.Insert(Excel.XlDirection.xlDown, missing);
                }

            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制行（在指定行下面复制指定数量行）
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param> 

        public void CopyRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 1; i <= count; i++)
                    {
                        range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                        range1.Copy(range2);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();


                throw e;
            }
        }

        /// <summary>
        /// 复制行（在指定WorkSheet指定行下面复制指定数量行）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void CopyRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 1; i <= count; i++)
                {
                    range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void DeleteRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 0; i < count; i++)
                    {
                        range.Delete(Excel.XlDirection.xlDown);
                        range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void DeleteRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];



                for (int i = 0; i < count; i++)
                {
                    range.Delete(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }


        }

        #endregion

        #region Column Methods

        /// <summary>
        /// 插列（在指定列右边插入指定数量列）
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                    for (int i = 0; i < count; i++)
                    {
                        range.Insert(Excel.XlDirection.xlDown, missing);
                    }


                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 插列（在指定WorkSheet指定列右边插入指定数量列）
        /// </summary>
        /// <param name="sheetIndex"></param> 
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                for (int i = 0; i < count; i++)
                {
                    range.Insert(Excel.XlDirection.xlDown, missing);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制列（在指定列右边复制指定数量列）
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void CopyColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    //     range1 = (Excel.Range)workSheet.Columns[columnIndex,this.missing];
                    range1 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex) + "1", this.IntToLetter(columnIndex) + "10000");

                    for (int i = 1; i <= count; i++)
                    {
                        //      range2 = (Excel.Range)workSheet.Columns[this.missing,columnIndex + i];
                        range2 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex + i) + "1", this.IntToLetter(columnIndex + i) + "10000");
                        range1.Copy(range2);
                    }
                }

            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制列（在指定WorkSheet指定列右边复制指定数量列）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param> 
        /// <param name="count"></param>
        public void CopyColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                //    range1 = (Excel.Range)workSheet.Columns[Type.Missing,columnIndex];
                range1 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex) + "1", this.IntToLetter(columnIndex) + "10000");

                for (int i = 1; i <= count; i++)
                {
                    //     range2 = (Excel.Range)workSheet.Columns[Type.Missing,columnIndex + i];
                    range2 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex + i) + "1", this.IntToLetter(columnIndex + i) + "10000");
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除列
        /// </summary> 
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void DeleteColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                    for (int i = 0; i < count; i++)
                    {
                        range.Delete(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();

                throw e;
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void DeleteColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                for (int i = 0; i < count; i++)
                {
                    range.Delete(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion


        #region Range Methods

        /// <summary>
        /// 将指定范围区域拷贝到目标区域
        /// </summary>
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="startCell">要拷贝区域的开始Cell位置（比如：A10）</param>
        /// <param name="endCell">要拷贝区域的结束Cell位置（比如：F20）</param>
        /// <param name="targetCell">目标区域的开始Cell位置（比如：H10）</param>
        public void RangeCopy(int sheetIndex, string startCell, string endCell, string targetCell)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
                range1 = workSheet.get_Range(startCell, endCell);
                if (string.IsNullOrEmpty(targetCell))
                {
                    range1.Copy(missing);
                }
                else
                {
                    range2 = workSheet.get_Range(targetCell, this.missing);
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 将指定范围区域拷贝到目标区域
        /// </summary>
        /// <param name="sheetName">WorkSheet名称</param> 
        /// <param name="startCell">要拷贝区域的开始Cell位置（比如：A10）</param>
        /// <param name="endCell">要拷贝区域的结束Cell位置（比如：F20）</param>
        /// <param name="targetCell">目标区域的开始Cell位置（比如：H10）</param>
        public void RangeCopy(string sheetName, string startCell, string endCell, string targetCell)
        {
            try
            {
                string tmpSheetName = this.workSheet.Name;
                bool hasSheet = false;
                if (tmpSheetName != sheetName)
                {
                    hasSheet = ChangeCurrentWorkSheet(sheetName);
                }
                else
                {
                    hasSheet = true;
                }
                if (hasSheet)
                {
                    range1 = this.workSheet.get_Range(startCell, endCell);
                    if (string.IsNullOrEmpty(targetCell))
                    {
                        range1.Copy(missing);
                    }
                    else
                    {
                        range2 = this.workSheet.get_Range(targetCell, this.missing);
                        range1.Copy(range2);
                    }
                    if (tmpSheetName != sheetName)
                    {
                        ChangeCurrentWorkSheet(tmpSheetName);
                    }
                }
                else
                {
                    throw new Exception("名称为[" + sheetName + "[的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 自动填充
        /// </summary>
        public void RangAutoFill()
        {
            Excel.Range rng = workSheet.get_Range("B4", Type.Missing);
            rng.Value2 = "星期一 ";
            rng.AutoFill(workSheet.get_Range("B4", "B9"),
                Excel.XlAutoFillType.xlFillWeekdays);

            rng = workSheet.get_Range("C4", Type.Missing);
            rng.Value2 = "一月";
            rng.AutoFill(workSheet.get_Range("C4", "C9"),
                Excel.XlAutoFillType.xlFillMonths);

            rng = workSheet.get_Range("D4", Type.Missing);
            rng.Value2 = "1";
            rng.AutoFill(workSheet.get_Range("D4", "D9"),
                Excel.XlAutoFillType.xlFillSeries);

            rng = workSheet.get_Range("E4", Type.Missing);

            rng.Value2 = "3";
            rng = workSheet.get_Range("E5", Type.Missing);
            rng.Value2 = "6";
            rng = workSheet.get_Range("E4", "E5");
            rng.AutoFill(workSheet.get_Range("E4", "E9"),
                Excel.XlAutoFillType.xlFillSeries);

        }





        /// <summary>
        /// 设置指定单元格的值
        /// </summary>
        public void SetRangVal(int rowindex, int colindex, string val, string sheetname)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    if (workSheet.Name == sheetname)
                    {
                        workSheet.Cells[rowindex, colindex] = val;

                        range = (Excel.Range)workSheet.Cells[rowindex, colindex];
                        range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        range.Borders.ColorIndex = 1;
                        range.ColumnWidth = 14;


                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 设置CELL值
        /// </summary>
        /// <param name="rowindex"></param>
        /// <param name="colindex"></param>
        /// <param name="val"></param>
        public void SetRangVal(int rowindex, int colindex, string val)
        {
            try
            {
                this.workSheet.Cells[rowindex, colindex] = val;
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// 获取指定单元格的值
        /// </summary>
        public string GetRangVal(int rowindex, int colindex, string sheetname)
        {
            string text = "";
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    Excel.Worksheet worksheet = (Excel.Worksheet)workBook.Worksheets[n];
                    if (worksheet.Name == sheetname)
                    {
                        Excel.Range range = (Excel.Range)worksheet.Cells[rowindex, colindex];
                        text = range.Text.ToString();
                        NAR(range);
                    }
                    NAR(worksheet);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
            return text;
        }


        /// <summary>
        /// 指定sheet 插列（在指定列右边插入指定数量列）
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int columnIndex, int count, string sheetname)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    if (workSheet.Name == sheetname)
                    {
                        range = (Excel.Range)workSheet.Columns[columnIndex, this.missing];
                        for (int i = 0; i < count; i++)
                        {
                            range.Insert(Excel.XlDirection.xlDown, missing);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }



        /// <summary>
        /// 应用样式
        /// </summary>
        public void ApplyStyle()
        {
            object missingValue = Type.Missing;
            Excel.Range rng = workSheet.get_Range("B3", "L23");
            Excel.Style style;

            try
            {
                style = workBook.Styles["NewStyle"];
            }
            // Style doesn't exist yet.
            catch
            {
                style = workBook.Styles.Add("NewStyle", missingValue);
                style.Font.Name = "Verdana";
                style.Font.Size = 12;
                style.Font.Color = 255;
                style.Interior.Color = (200 << 16) | (200 << 8) | 200;
                style.Interior.Pattern = Excel.XlPattern.xlPatternSolid;
            }

            rng.Value2 = "'Style Test";
            rng.Style = "NewStyle";
            rng.Columns.AutoFit();


        }

        /// <summary>
        /// 应用样式 caoq 2014-7-1
        /// </summary>
        /// <param name="cell1">开始单元格 例：A1</param>
        /// <param name="cell2">结束单元格 例：B10</param>
        /// <param name="stylename">样式名称 （默认为NewStyle样式）</param>
        public void ApplyStyle(string cell1, string cell2, string stylename)
        {
            object missingValue = Type.Missing;
            if (string.IsNullOrEmpty(stylename)) stylename = "NewStyle";
            Excel.Range rng = workSheet.get_Range(cell1, cell2);
            Excel.Style style;
            try
            {
                style = workBook.Styles[stylename];
            }
            catch
            {
                style = workBook.Styles.Add(stylename, missingValue);
                style.Font.Name = "Verdana";
                style.Font.Size = 12;
                style.Font.Color = 255;
                style.Interior.Color = (200 << 16) | (200 << 8) | 200;
                style.Interior.Pattern = Excel.XlPattern.xlPatternSolid;
            }
            rng.Style = stylename;
            rng.Columns.AutoFit();
        }
        /// <summary>
        /// 给单元格设置边框颜色
        /// </summary>
        /// <param name="startRow">从第几行开始</param>
        /// <param name="startColumn">从第几列开始</param>
        /// <param name="endRow">到第几行结束</param>
        /// <param name="endColumn">到第几列结束</param>
        /// <param name="colorIndex"></param>
        public void SetRangeBorderColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex colorIndex)
        {
            Excel.Range rng = workSheet.get_Range(app.Cells[startRow, startColumn], app.Cells[endRow, endColumn]);
            rng.Borders.ColorIndex = colorIndex;// System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
        }
        #endregion

        #region Set Style Methods
        /// <summary>
        /// 单元格背景色及填充方式
        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="color">颜色索引</param>
        public void CellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {            
            Excel.Range range = app.get_Range(app.Cells[startRow, startColumn], app.Cells[endRow, endColumn]);
            range.Interior.ColorIndex = color;
            range.Interior.Pattern = Pattern.Solid;
        }
        /// <summary>
        /// 单元格背景色及填充方式
        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param> 


        /// <param name="endColumn">结束列</param>
        /// <param name="color">颜色索引</param>
        /// <param name="pattern">填充方式</param>
        public void CellsBackColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color, Pattern pattern)
        {
            Excel.Range range = app.get_Range(app.Cells[startRow, startColumn], app.Cells[endRow, endColumn]);
            range.Interior.ColorIndex = color;
            range.Interior.Pattern = pattern;
        }
        /// <summary> 

        /// 设置行高
        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="height">行高</param>
        public void SetRowHeight(int startRow, int endRow, int height)
        {
            //获取当前正在使用的工作表

            Excel.Worksheet worksheet = (Excel.Worksheet)app.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Rows[startRow.ToString() + ":" + endRow.ToString(), System.Type.Missing];


            range.RowHeight = height;
        }

        /// <summary>
        /// 自动调整行高
        /// </summary>
        /// <param name="columnNum">列号</param>
        public void RowAutoFit(int rowNum)
        {
            //获取当前正在使用的工作表
            Excel.Worksheet worksheet = (Excel.Worksheet)app.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Rows[rowNum.ToString() + ":" + rowNum.ToString(), System.Type.Missing];
            range.EntireColumn.AutoFit();

        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="startColumn">起始列(列对应的字母)</param>
        /// <param name="endColumn">结束列(列对应的字母)</param>
        /// <param name="width"></param>
        public void SetColumnWidth(string startColumn, string endColumn, int width)
        {
            //获取当前正在使用的工作表
            Excel.Worksheet worksheet = (Excel.Worksheet)app.ActiveSheet;


            Excel.Range range = (Excel.Range)worksheet.Columns[startColumn + ":" + endColumn, System.Type.Missing];
            range.ColumnWidth = width;
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="startColumn">起始列</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="width"></param>
        public void SetColumnWidth(int startColumn, int endColumn, int width)
        {

            string strStartColumn = GetColumnName(startColumn);
            string strEndColumn = GetColumnName(endColumn);
            //获取当前正在使用的工作表
            Excel.Worksheet worksheet = (Excel.Worksheet)app.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[strStartColumn + ":" + strEndColumn, System.Type.Missing];
            range.ColumnWidth = width;
        }

        public String GetColumnName(int num)
        {
            String[] COL_NAME =    
            {    
                    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",    
                    "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",    
                    "U", "V", "W", "X", "Y", "Z"  
            };
            int a = 0, b = num;
            String col = "";
            if (b < 26)
            {
                col = COL_NAME[b];
            }
            else
            {
                while (b >= 26)
                {
                    a = b % 26;
                    b = b / 26;
                    col = COL_NAME[a] + col;
                }
                col = COL_NAME[b - 1] + col;
            }
            return col;
        }


        /// <summary>
        /// 自动调整列宽
        /// </summary> 
        /// <param name="columnNum">列号</param>
        public void ColumnAutoFit(string column)
        {
            //获取当前正在使用的工作表
            Excel.Worksheet worksheet = (Excel.Worksheet)app.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[column + ":" + column, System.Type.Missing];
            range.EntireColumn.AutoFit();
            NAR(range);
            NAR(worksheet);
        }

        /// <summary>
        /// 自动调整列宽
        /// </summary> 



        /// <param name="columnNum">列号</param>
        public void ColumnAutoFit(int columnNum)
        {
            string strcolumnNum = GetColumnName(columnNum);
            //获取当前正在使用的工作表
            Excel.Worksheet worksheet = (Excel.Worksheet)app.ActiveSheet;
            Excel.Range range = (Excel.Range)worksheet.Columns[strcolumnNum + ":" + strcolumnNum, System.Type.Missing];
            range.EntireColumn.AutoFit();
            NAR(range);
            NAR(worksheet);
        }


        /// <summary> 


        /// 字体颜色
        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="color">颜色索引</param>
        public void FontColor(int startRow, int startColumn, int endRow, int endColumn, ColorIndex color)
        {
            Excel.Range range = app.get_Range(app.Cells[startRow, startColumn], app.Cells[endRow, endColumn]);
            range.Font.ColorIndex = color;
        }

        /// <summary>
        /// 字体样式(加粗,斜体,下划线)
        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="isBold">是否加粗</param>
        /// <param name="isItalic">是否斜体</param> 
        /// <param name="underline">下划线类型</param>
        public void FontStyle(int startRow, int startColumn, int endRow, int endColumn, bool isBold, bool isItalic, UnderlineStyle underline)
        {
            Excel.Range range = app.get_Range(app.Cells[startRow, startColumn], app.Cells[endRow, endColumn]);
            range.Font.Bold = isBold;
            range.Font.Underline = underline;
            range.Font.Italic = isItalic;
        }

        /// <summary>
        /// 单元格字体及大小 


        /// </summary>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="endRow">结束行</param>
        /// <param name="endColumn">结束列</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        public void FontNameSize(int startRow, int startColumn, int endRow, int endColumn, string fontName, int fontSize)
        {
            Excel.Range range = app.get_Range(app.Cells[startRow, startColumn], app.Cells[endRow, endColumn]);


            range.Font.Name = fontName;
            range.Font.Size = fontSize;
        }


        #endregion

        #region ExcelHelper Kit
        /// <summary>
        /// 将Excel列的字母索引值转换成整数索引值
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public int LetterToInt(string letter)
        {
            int n = 0;


            if (letter.Trim().Length == 0)
                throw new Exception("不接受空字符串！");

            if (letter.Length >= 2)
            {
                char c1 = letter.ToCharArray(0, 2)[0];
                char c2 = letter.ToCharArray(0, 2)[1];

                if (!char.IsLetter(c1) || !char.IsLetter(c2))
                {
                    throw new Exception("格式不正确，必须是字母！");
                }

                c1 = char.ToUpper(c1);
                c2 = char.ToUpper(c2);

                int i = Convert.ToInt32(c1) - 64;
                int j = Convert.ToInt32(c2) - 64;

                n = i * 26 + j;
            }

            if (letter.Length == 1)
            {
                char c1 = letter.ToCharArray()[0];


                if (!char.IsLetter(c1))
                {
                    throw new Exception("格式不正确，必须是字母！");
                }

                c1 = char.ToUpper(c1);

                n = Convert.ToInt32(c1) - 64;
            }

            if (n > 256)
                throw new Exception("索引超出范围，Excel的列索引不能超过256！");




            return n;
        }

        /// <summary>
        /// 将Excel列的整数索引值转换为字符索引值
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string IntToLetter(int n)
        {
            if (n > 256)
                throw new Exception("索引超出范围，Excel的列索引不能超过256！");

            int i = Convert.ToInt32(n / 26);
            int j = n % 26;

            char c1 = Convert.ToChar(i + 64);
            char c2 = Convert.ToChar(j + 64);

            if (n > 26)
                return c1.ToString() + c2.ToString();
            else if (n == 26)
                return "Z";
            else
                return c2.ToString();
        }

        #endregion


        #region Output File(注意：如果目标文件已存在的话会出错)
        /// <summary>
        /// 输出Excel文件并退出
        /// </summary>
        public void OutputExcelFile()
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);

            }
            catch (Exception e)
            {
                throw e;
                this.Dispose();
            }
        }

        /// <summary>
        /// 输出指定格式的文件（支持格式：HTML，CSV，TEXT，EXCEL）
        /// </summary> 

        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void OutputFile(string format)
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);



                            break;
                        }
                    //     case "XML":
                    //     {
                    //      workBook.SaveAs(outputFile,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //       Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, 

                    //       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //      break;
                    //
                    //     }
                    default:
                        {
                            workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
                this.Dispose();
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void SaveFile()
        {
            try
            {
                workBook.Save();
            }
            catch (Exception e)
            {
                throw e;
                this.Dispose();
            }
        }

        /// <summary>
        /// 另存文件
        /// </summary>
        public void SaveAsFile()
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
                this.Dispose();
            }
        }

        /// <summary>
        /// 将Excel文件另存为指定格式
        /// </summary>
        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void SaveAsFile(string format)
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    //     case "XML":
                    //     { 

                    //      workBook.SaveAs(outputFile,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //       Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //      break;
                    //     }
                    default:
                        {
                            workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
                this.Dispose();
            }
        }

        /// <summary>
        /// 另存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void SaveFile(string fileName)
        {
            try
            {
                object oformat = Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal;
                workBook.SaveAs(fileName, oformat, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                NAR(oformat);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 将Excel文件另存为指定格式
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void SaveAsFile(string fileName, string format)
        {
            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(fileName, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(fileName, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(fileName, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                    //     case "XML":
                    //     {
                    //      workBook.SaveAs(fileName,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing, 

                    //       Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //      break;
                    //     }
                    default:
                        {
                            workBook.SaveAs(fileName, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }
        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 合并单元格，并赋值，对指定WorkSheet操作
        /// </summary>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param> 
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        private void MergeCells(Excel.Worksheet sheet, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheet == null)
                return;

            range = sheet.get_Range(sheet.Cells[beginRowIndex, beginColumnIndex], sheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents(); //先把Range内容清除，合并才不会出错 
            range.MergeCells = true;
            range.Value2 = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// 将指定索引列的数据相同的行合并，对指定WorkSheet操作
        /// </summary>
        /// <param name="columnIndex">要合并的列索引</param>
        /// <param name="beginRowIndex">合并开始行索引</param>
        /// <param name="rows">要合并的行数</param> 
        private void MergeRows(Excel.Worksheet sheet, int columnIndex, int beginRowIndex, int rows)
        {
            int beginIndex = beginRowIndex;
            int count = 0;
            string text1;
            string text2;

            if (sheet == null)
                return;

            for (int j = beginRowIndex; j < beginRowIndex + rows; j++)
            {
                range1 = (Excel.Range)sheet.Cells[j, columnIndex];
                range2 = (Excel.Range)sheet.Cells[j + 1, columnIndex];
                text1 = range1.Text.ToString();
                text2 = range2.Text.ToString();

                if (text1 == text2)
                {
                    ++count;
                }
                else
                {
                    if (count > 0)
                    {
                        this.MergeCells(sheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                    }

                    beginIndex = j + 1; //设置开始合并行索引
                    count = 0; //计数器清0
                }


            }

        }

        //取批注
        public Dictionary<string, string> GetCommentsFromCurrent()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            Excel.Worksheet sheet = this.workSheet;
            for (int i = 1; i <= sheet.UsedRange.Rows.Count; i++)
            {
                for (int j = 1; j <= sheet.UsedRange.Columns.Count; j++)
                {
                    Excel.Range range = (Excel.Range)sheet.UsedRange[i, j];
                    if (range.Comment != null && !list.ContainsKey(range.Comment.Text(Missing.Value, Missing.Value, Missing.Value)))
                    {
                        list.Add(range.Comment.Text(Missing.Value, Missing.Value, Missing.Value), range.Text.ToString());
                    }
                }
            }
            return list;
        }

        //取批注
        public Dictionary<string, string> GetComments()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            for (int wc = 1; wc <= WorkSheetCount; wc++)
            {
                this.ChangeCurrentWorkSheet(wc);
                Excel.Worksheet sheet = this.workSheet;
                for (int i = 1; i <= sheet.UsedRange.Rows.Count; i++)
                {
                    for (int j = 1; j <= sheet.UsedRange.Columns.Count; j++)
                    {
                        Excel.Range range = (Excel.Range)sheet.UsedRange[i, j];
                        if (range.Comment != null)
                        {
                            list.Add(range.Comment.Text(Missing.Value, Missing.Value, Missing.Value), range.Text.ToString());
                        }
                    }
                }
            }
            return list;
        }

        //设置批注
        public void SetComments(Dictionary<string, string> dict)
        {
            Excel.Worksheet sheet = this.workSheet;
            for (int i = 1; i <= sheet.UsedRange.Rows.Count; i++)
            {
                for (int j = 1; j <= sheet.UsedRange.Columns.Count; j++)
                {
                    Excel.Range range = (Excel.Range)sheet.UsedRange[i, j];
                    if (range.Comment != null)
                    {
                        foreach (KeyValuePair<string, string> item in dict)
                        {
                            if (item.Key == range.Comment.Text(Missing.Value, Missing.Value, Missing.Value))
                            {
                                range.set_Value(missing, item.Value);
                                break;
                            }
                        }
                    }
                }
            }
        }
        //清除所有批注
        public void ClearAllComments()
        {
            for (int wc = 1; wc <= WorkSheetCount; wc++)
            {
                this.ChangeCurrentWorkSheet(wc);
                Excel.Worksheet sheet = this.workSheet;
                foreach (Excel.Comment com in sheet.Comments)
                {
                    com.Delete();
                }
            }
        }

        //清除批注
        public void ClearComments()
        {
            Excel.Worksheet sheet = this.workSheet;
            foreach (Excel.Comment com in sheet.Comments)
            {
                com.Delete();
            }
        }

        /// <summary>
        /// 清除内容
        /// </summary>
        public void ClearContents()
        {
            Excel.Worksheet sheet = this.workSheet;
            Excel.Range rowRange = sheet.Rows;
            rowRange.Delete();
        }

        //内容自适应
        public void AutoFit()
        {
            AutoFit(true, false);
        }

        /// <summary>
        /// 内容自适应
        /// </summary>
        /// <param name="rowsAutoFit"></param>
        /// <param name="columnsAutoFit">行内容自动适应，主要用于数值单元格宽度不足时出现取值为"XXXX"的情况</param>
        public void AutoFit(bool rowsAutoFit, bool columnsAutoFit)
        {
            if (!rowsAutoFit && !columnsAutoFit)
                return;
            for (int wc = 1; wc <= WorkSheetCount; wc++)
            {
                this.ChangeCurrentWorkSheet(wc);
                Excel.Worksheet sheet = this.workSheet;
                if (columnsAutoFit)
                    sheet.Columns.AutoFit();
                if (rowsAutoFit)
                    sheet.Rows.AutoFit();
            }
        }

        /// <summary>
        /// 计算WorkSheet数量
        /// </summary>
        /// <param name="rowCount">记录总行数</param>
        /// <param name="rows">每WorkSheet行数</param>
        public int GetSheetCount(int rowCount, int rows)
        {
            int n = rowCount % rows; //余数

            if (n == 0)
                return rowCount / rows;


            else
                return Convert.ToInt32(rowCount / rows) + 1;
        }


        /// <summary>
        /// 结束Excel进程
        /// </summary>
        public void KillExcelProcess()
        {
            Process[] myProcesses;
            DateTime startTime;
            myProcesses = Process.GetProcessesByName("Excel");

            //得不到Excel进程ID，暂时只能判断进程启动时间 
            foreach (Process myProcess in myProcesses)
            {
                try
                {
                    startTime = myProcess.StartTime;

                    if (startTime > beforeTime && startTime < afterTime)
                    {
                        myProcess.Kill();
                    }
                }
                catch { }
            }
        }

        public void Quit()
        {
            NAR(range);
            NAR(range1);
            NAR(range2);
            NAR(textBox);
            NAR(workSheet);
            if (workBook != null)
            {
                //这里坑爹啊，不保存的枚举值竟然保存文件了！！kevin 20131128
                //object saveOption = Microsoft.Office.Interop.Excel.XlSaveAction.xlDoNotSaveChanges;                
                workBook.Close(false, Type.Missing, Type.Missing);
                NAR(workBook);
            }
            // oWord.Application.Quit(ref missing, ref missing, ref missing); tjt
            if (app != null)
            {
                app.Workbooks.Close();
                NAR(app.Workbooks);
                app.Quit();
                NAR(app);
            }
            //释放word进程
            GC.Collect();
            //释放不了资源，强制关闭进程，这里需要处理并发的问题，留待解决 kevin 20131128
            //KillExcelProcess();
        }

        private void NAR(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch { }
            finally { o = null; }
        }


        /// <summary>
        /// 将图片插入到指定的单元格位置。
        /// 注意：图片必须是绝对物理路径
        /// </summary>
        /// <param name="RangeName">单元格名称，例如：B4</param>
        /// <param name="PicturePath">要插入图片的绝对路径。</param>
        public void InsertPicture(string RangeName, string PicturePath)
        {
            Excel.Range m_objRange = workSheet.get_Range(RangeName, Missing.Value);
            m_objRange.Select();
            Excel.Pictures pics = (Excel.Pictures)workSheet.Pictures(Missing.Value);            
            pics.Insert(PicturePath, Missing.Value);
            NAR(m_objRange);
        }

        /// <summary>
        /// 将图片插入到指定的单元格位置，并设置图片的宽度和高度。
        /// 注意：图片必须是绝对物理路径
        /// </summary>
        /// <param name="RangeName">单元格名称，例如：B4</param>
        /// <param name="PicturePath">要插入图片的绝对路径。</param>
        /// <param name="PictuteWidth">插入后，图片在Excel中显示的宽度。</param>
        /// <param name="PictureHeight">插入后，图片在Excel中显示的高度。</param>
        /// <param name="isAuto">true:自动适应单元格的宽高度，设为true时参数PictuteWidth、PictureHeight无效</param>
        public void InsertPicture(string RangeName, string PicturePath, float PictuteWidth, float PictureHeight, bool isAuto = false)
        {
            Excel.Range m_objRange = workSheet.get_Range(RangeName, Missing.Value);
            m_objRange.Select();
            float PicLeft, PicTop;
            PicLeft = Convert.ToSingle(m_objRange.Left);
            PicTop = Convert.ToSingle(m_objRange.Top);
            if (isAuto)
            {
                PictuteWidth =(float)Convert.ToDouble(m_objRange.Width);
                PictureHeight = (float)Convert.ToDouble(m_objRange.Height);
            }
            //参数含义：
            //图片路径
            //是否链接到文件
            //图片插入时是否随文档一起保存
            //图片在文档中的坐标位置（单位：points）
            //图片显示的宽度和高度（单位：points）
            //参数详细信息参见：http://msdn2.microsoft.com/zh-cn/library/aa221765(office.11).aspx
            workSheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, PictuteWidth, PictureHeight);
            NAR(m_objRange);
        }
        /// <summary>
        /// 将图片插到指定位置
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="PicturePath"></param>
        /// <param name="picLeft"></param>
        /// <param name="picTop"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InsertPicture(Excel.Worksheet worksheet, string PicturePath, float picLeft, float picTop, float width, float height)
        {
            //workBook.Application.CentimetersToPoints();
            worksheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft, picTop, width, height);
        }

        public void InsertPicture(string PicturePath, float picLeft, float picTop, float width, float height)
        {
            this.workSheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft, picTop, width, height);
        }

        /// <summary>
        /// 插入图片到指定位置（并旋转指定角度）
        /// </summary>
        /// <param name="PicturePath"></param>
        /// <param name="picLeft"></param>
        /// <param name="picTop"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="trunNumber">trunNumber小于0 向左旋转，trunNumber大于0 向右旋转</param>
        public void InsertPicture(string PicturePath, float picLeft, float picTop, float width, float height, float trunNumber)
        {
            //旋转图片需要重新计算坐标起点
            picLeft -= (width - height) / 2;
            picTop -= (height - width) / 2;
            Excel.Shape myShape = this.workSheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft, picTop, width, height);
            myShape.IncrementRotation(trunNumber); //图片旋转
        }

        /// <summary>
        /// 插入文本框
        /// </summary>
        /// <param name="picLeft"></param>
        /// <param name="picTop"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="text"></param>
        /// <param name="fontsize"></param>
        public void InsertTextBox(float picLeft, float picTop, float width, float height, string text, int fontsize)
        {
            Excel.TextBoxes texts = (Excel.TextBoxes)this.workSheet.TextBoxes(missing);
            Excel.TextBox t = texts.Add(picLeft, picTop, width, height);
            t.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            t.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            t.Border.LineStyle = LineStyle.无;
            t.Interior.Pattern = Pattern.None; //不起作用，不知原因
            t.Interior.PatternColorIndex = ColorIndex.无色;
            t.Interior.TintAndShade = 0;
            t.Font.Size = fontsize;
            t.Text = text;
            NAR(t);
            NAR(texts);
        }

        //设置sheet名
        public void SetSheetName(string sheetName)
        {
            this.workSheet.Name = sheetName;
        }

        public void PrintSet()      //打印设置 
        {
            try
            {
                //mySheet1.PageSetup.PaperSize  = mySheet1.XLPaperSize.xlPaperA4;          // 设置页面大小为A4 
                //mySheet1.PageSetup.Orientation = XlPageOrientation.xlPortrait; // 设置垂直版面 
                this.workSheet.PageSetup.HeaderMargin = 0.0;                        // 设置页眉边距 
                this.workSheet.PageSetup.FooterMargin = 0.0;                        // 设置页脚边距 
                this.workSheet.PageSetup.LeftMargin = app.InchesToPoints(0.354330708661417); // 设置左边距 
                this.workSheet.PageSetup.RightMargin = app.InchesToPoints(0.354330708661417);// 设置右边距 
                this.workSheet.PageSetup.TopMargin = app.InchesToPoints(0.393700787401575);  // 设置上边距 
                this.workSheet.PageSetup.BottomMargin = app.InchesToPoints(0.393700787401575);// 设置下边距 
                this.workSheet.PageSetup.CenterHorizontally = true;                  // 设置水平居中 
            }
            catch { }
        }

        /// <summary>
        /// 设置页边距
        /// </summary>
        /// <param name="isPaperA4">是否为A4纸张</param>
        /// <param name="topMargin">上边距(单位：cm)</param>
        /// <param name="rightMargin">右边距(单位：cm)</param>
        /// <param name="bottomMargin">下边距(单位：cm)</param>
        /// <param name="leftMargin">左边距(单位：cm)</param>
        /// <param name="headerMargin">页眉边距(单位：cm)</param>
        /// <param name="footerMargin">页脚边距(单位：cm)</param>       
        public void PrintSet(bool isPaperA4, double topMargin, double rightMargin, double bottomMargin, double leftMargin, double headerMargin, double footerMargin)
        {
            try
            {
                if (isPaperA4) // 设置页面大小为A4 
                    this.workSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                this.workSheet.PageSetup.HeaderMargin = app.InchesToPoints(headerMargin * inches_1);// 设置页眉边距 
                this.workSheet.PageSetup.FooterMargin = app.InchesToPoints(footerMargin * inches_1);// 设置页脚边距 
                this.workSheet.PageSetup.LeftMargin = app.InchesToPoints(leftMargin * inches_1);// 设置左边距 
                this.workSheet.PageSetup.RightMargin = app.InchesToPoints(rightMargin * inches_1);// 设置右边距 
                this.workSheet.PageSetup.TopMargin = app.InchesToPoints(topMargin * inches_1);// 设置上边距 
                this.workSheet.PageSetup.BottomMargin = app.InchesToPoints(bottomMargin * inches_1);// 设置下边距             
            }
            catch { }
        }

        private void Dispose()
        {
            if (app != null)
            {
                workBook.Close(null, null, null);
                app.Workbooks.Close();
                app.Quit();
            }
            if (range != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                range = null;
            }
            if (range1 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                range1 = null;
            }
            if (range2 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range2);
                range2 = null;
            }
            if (textBox != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(textBox);
                textBox = null;
            }
            if (workSheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
                workSheet = null;
            }
            if (workBook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);


                workBook = null;
            }
            if (app != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }

            GC.Collect();

            this.KillExcelProcess();

        }//end Dispose
        #endregion



    }//end class
}//end namespace
