using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.IO;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// Excel相关操作类
    /// </summary>
    public class ExcelHelper
    {
        private static IWorkbook _workbook;
        private ExcelHelper() { }

        #region 根据Excel文件名(包含所在的路径)获得DataTable

        /// <summary>
        /// 根据Excel文件名(包含所在的路径)获得DataTable
        /// 2003
        /// </summary>
        /// <param name="strExcelFilePath">Excel文件名(包含所在的路径)</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable GetDataTable(string strExcelFilePath)
        {
            return GetDataTable(strExcelFilePath, 0);
        }

        /// <summary>
        /// 根据Excel文件名(包含所在的路径)获得DataTable
        /// </summary>
        /// <param name="strExcelFilePath">Excel文件名(包含所在的路径)</param>
        /// 2003
        /// <param name="TableIndex"></param>
        /// <returns>返回的DataTable</returns>
        public static DataTable GetDataTable(string strExcelFilePath, int TableIndex)
        {
            DataTable dtResuilt = new DataTable();
            string TableName = GetTableName(strExcelFilePath, TableIndex);
            string strConn = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=" + strExcelFilePath.Trim() + ";Extended Properties=\"Excel 8.0;IMEX=1\"";
            using (OleDbDataAdapter cmd = new OleDbDataAdapter("SELECT * FROM [" + TableName + "]", strConn))
            {
                cmd.Fill(dtResuilt);
                return dtResuilt;
            }
        }

        #endregion

        #region 取得Sheet名称

        /// <summary>
        /// 取得Sheet名称
        /// 2003
        /// </summary>
        /// <param name="strExcelFilePath">Excel文件名(包含所在的路径)</param>
        /// <param name="TableIndex">页签索引</param>
        /// <returns>Sheet名称</returns>
        public static string GetTableName(string strExcelFilePath, int TableIndex)
        {
            string strConn = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=" + strExcelFilePath.Trim() + "; Extended Properties=\"Excel 8.0;IMEX=1\"";

            using (OleDbConnection ExcelConnection = new OleDbConnection(strConn))
            {
                try
                {
                    ExcelConnection.Open();
                    return ExcelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" }).Rows[TableIndex][2].ToString();

                }
                catch (OleDbException ex)
                {
                    throw ex;
                }
            }
        }

        #endregion


        #region 导出为Excel

        public static void DataTableToExcel(DataTable dt, String FileName)
        {
            if (dt.Rows.Count > 0)
            {
                string fileName = "预置文件名";//可以加个saveFileDialog文件保存控件，saveFileDialog1.FileName

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                object ms = Type.Missing;
                Microsoft.Office.Interop.Excel.Workbook wk = excel.Workbooks.Add(ms);
                Microsoft.Office.Interop.Excel.Worksheet ws = wk.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                    }
                }
                try
                {
                    wk.SaveAs(fileName, ms, ms, ms, ms, ms, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, ms, ms, ms, ms, ms);
                    //MessageBox.Show(page,"信息导出成功！");

                }
                catch (Exception)
                {
                    // MessageBox.Show(page, "信息导出失败！");
                }
                excel.Quit();

            }
        }

        private static void dtExport_RowCreated(object sender, GridViewRowEventArgs e)
        {
            foreach (TableCell tableCell in e.Row.Cells)
            {
                tableCell.Attributes.Add("style", "vnd.ms-excel.numberformat:@;");
            }
        }

        /// <summary> 
        /// 将一组对象导出成EXCEL 
        /// </summary> 
        /// <typeparam name="T">要导出对象的类型</typeparam> 
        /// <param name="objList">一组对象</param> 
        /// <param name="FileName">导出后的文件名</param> 
        /// <param name="columnInfo">列名信息</param> 
        public static string ExExcel<T>(List<T> objList, Dictionary<string, string> columnInfo)
        {
            if (columnInfo.Count == 0) { return ""; }
            if (objList.Count == 0) { return ""; }
            //生成EXCEL的HTML 
            string excelStr = "";

            Type myType = objList[0].GetType();
            //根据反射从传递进来的属性名信息得到要显示的属性 
            List<PropertyInfo> myPro = new List<PropertyInfo>();
            foreach (string cName in columnInfo.Keys)
            {
                PropertyInfo p = myType.GetProperty(cName);
                if (p != null)
                {
                    myPro.Add(p);
                    excelStr += columnInfo[cName] + "\t";
                }
            }
            //如果没有找到可用的属性则结束 
            if (myPro.Count == 0) { return ""; }
            excelStr += "\n";

            foreach (T obj in objList)
            {
                foreach (PropertyInfo p in myPro)
                {
                    excelStr += p.GetValue(obj, null) + "\t";
                }
                excelStr += "\n";
            }
            //输出EXCEL 
            return excelStr;
        }
        #endregion

        #region Excel导入DataTable

        /// <summary>
        /// 获取Excel文件数据表列表
        /// 2003  2007以上
        /// </summary>z
        public static ArrayList GetExcelTables(string ExcelFileName)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;';Data Source=" + ExcelFileName))
                {
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //获取数据表个数
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (TablesList.IndexOf(tablename) < 0)
                        {
                            TablesList.Add(tablename);
                        }
                    }
                }
            }
            return TablesList;
        }

        /// <summary>
        /// 将Excel文件导出至DataTable(第一行作为表头)
        /// 2003  2007以上
        /// </summary>
        /// <param name="ExcelFilePath">Excel文件路径</param>
        /// <param name="TableName">数据表名，如果数据表名错误，默认为第一个数据表名</param>
        public static DataTable ExportExcelToDt(string ExcelFilePath, string TableName)
        {
            if (!File.Exists(ExcelFilePath))
            {
                throw new Exception("Excel文件不存在！");
            }

            //如果数据表名不存在，则数据表名为Excel文件的第一个数据表
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables(ExcelFilePath);

            if (!TableList.Contains(TableName))
            {
                TableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFilePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;';");
            OleDbCommand cmd = new OleDbCommand("select * from [" + TableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// 获取Excel文件指定数据表的数据列表
        /// 2003
        /// </summary>
        /// <param name="ExcelFileName">Excel文件名</param>
        /// <param name="TableName">数据表名</param>
        public static ArrayList GetExcelTableColumns(string ExcelFileName, string TableName)
        {
            DataTable dt = new DataTable();
            ArrayList ColsList = new ArrayList();
            if (File.Exists(ExcelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + ExcelFileName))
                {
                    conn.Open();
                    dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, TableName, null });

                    //获取列个数
                    int colcount = dt.Rows.Count;
                    for (int i = 0; i < colcount; i++)
                    {
                        string colname = dt.Rows[i]["Column_Name"].ToString().Trim();
                        ColsList.Add(colname);
                    }
                }
            }
            return ColsList;
        }
        #endregion

        #region 使用NPOI导入excel

        /// <summary>
        /// 使用npoi导入excel
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int ImportExcelByNPOI(string connectionString, DataTable dt, string tableName)
        {
            SqlBulkCopy sbc = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };

            try
            {
                sbc.DestinationTableName = tableName;
                sbc.WriteToServer(dt); //此处报错
                return sbc.NotifyAfter;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        /// <summary>
        /// 获取完整路径加文件名
        /// </summary>
        /// <param name="strName">文件名称（带后缀）</param>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="ofile"></param>
        /// <returns></returns>
        public static string GetNewFileName(string strName, string filePath, HttpPostedFileBase ofile)
        {
            string newFileName = string.Empty;
            if (strName != "") //如果文件名存在
            {
                //bool fileOk = false;
                //int i = strName.LastIndexOf(".", StringComparison.Ordinal); //获取。的索引顺序号，在这里。代表文件名字与后缀的间隔
                //string kzm = strName.Substring(i);
                //获取文件扩展名的另一种方法 string fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                //string juedui = Server.MapPath("~\\Upload\\Excel\\");
                //设置文件保存的本地目录绝对路径，对于路径中的字符“＼”在字符串中必须以“＼＼”表示，因为“＼”为特殊字符。或者可以使用上一行的给路径前面加上＠
                newFileName = filePath + strName;//+ kzm;

                //String[] allowedExtensions = { ".xls", ".xlsx", ".csv" };
                //foreach (string t in allowedExtensions)
                //{
                //    if (kzm == t)
                //    {
                //        fileOk = true;
                //    }
                //}
                //if (fileOk)
                //{
                try
                {
                    // 判定该路径是否存在
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);
                    ofile.SaveAs(newFileName); //将文件存储到服务器上
                }
                catch (Exception ex)
                {
                    return StringRep(ex.Message);
                }
                //}
            }
            return newFileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string StringRep(string name)
        {
            string str = name.Replace("'", "\'");
            return str;
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="fileName"></param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTableByNPOI(string sheetName, string fileName, bool isFirstRowColumn)
        {
            DataTable data = new DataTable();
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath(fileName));
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                    _workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    _workbook = new HSSFWorkbook(fs);

                ISheet sheet;
                if (sheetName != null)
                {
                    sheet = _workbook.GetSheet(sheetName) ?? _workbook.GetSheetAt(0);
                }
                else
                {
                    sheet = _workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    int startRow;
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Cells[0].ToString().Trim() == "") continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        #endregion
    }
}
