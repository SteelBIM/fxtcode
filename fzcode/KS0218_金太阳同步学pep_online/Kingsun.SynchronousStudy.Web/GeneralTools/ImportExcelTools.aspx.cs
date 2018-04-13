using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.GeneralTools
{
    public partial class ImportExcelTools : System.Web.UI.Page
    {
        public int Bookid;
        public string BookName;
        private IWorkbook _workbook;

        protected void Page_Load(object sender, EventArgs e)
        {
            Bookid = Convert.ToInt32(Request.QueryString["BookID"]);
            BookName = GetBookName(Convert.ToInt32(Bookid));
            lbBookName.Text = BookName;
            DataBind(Bookid);
        }

        private string GetBookName(int bookid)
        {
            string sql = @"SELECT top 1 [TeachingNaterialName] FROM [TB_ModuleConfiguration] WHERE BookID=" + bookid;
            string bookname = "";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bookname = ds.Tables[0].Rows[0][0].ToString();
            }

            return bookname;
        }

        private void DataBind(int bookid)
        {
            string sql = string.Format(@"SELECT  *
                            FROM    [TB_ModuleSort]
                            WHERE   ID IN ( SELECT  MAX(ID)
                                            FROM    [TB_ModuleSort]
                                            WHERE   BookID = {0}
                                                    AND ( SuperiorID = FirstTitleID
                                                          OR SuperiorID = SecondTitleID
                                                        )
                                            GROUP BY ModuleID );", bookid);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
            rpModule.DataSource = dt;
            rpModule.DataBind();
        }

        protected void rpModule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "down")
            {

                string newFileName = string.Empty;
                string strName = FileUpload1.PostedFile.FileName;//使用fileupload控件获取上传文件的文件名
                if (string.IsNullOrEmpty(strName))
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('请选择文件！');</script>");
                    return;
                }
                newFileName = GetNewFileName(strName, newFileName);
                if (string.IsNullOrEmpty(newFileName))
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('" + newFileName + "！');</script>");
                    return;
                }

                //string path = Server.MapPath("~/Upload/Excel/");
                //"C:\\TeamCity\\buildAgent\\work\\88580d4c6fbacf3\\Kingsun.SynchronousStudy.Web\\Upload\\ResourcesExcel\\" + GetBookName(Convert.ToInt32(Bookid)) + ".xls";
                // string fileName = path + GetBookName(Convert.ToInt32(Bookid)) + ".xls";
                DataTable dt = ExcelToDataTable("Sheet1", newFileName, true);

                DataTable dtList = new DataTable();
                dtList.Columns.Add("ID");
                dtList.Columns.Add("ModuleID");
                dtList.Columns.Add("BooKID");
                dtList.Columns.Add("TeachingNaterialName");
                dtList.Columns.Add("ModuleName");
                dtList.Columns.Add("FirstTitleID");
                dtList.Columns.Add("FirstTitle");
                dtList.Columns.Add("SecondTitleID");
                dtList.Columns.Add("SecondTitle");
                dtList.Columns.Add("ModuleAddress");
                dtList.Columns.Add("MD5");


                if (dt.Rows.Count <= 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow drlist = dtList.NewRow();
                    drlist["BooKID"] = ParseInt(Bookid);
                    drlist["TeachingNaterialName"] = BookName;
                    drlist["FirstTitleID"] = dt.Rows[i]["一级标题ID"].ToString();
                    drlist["FirstTitle"] = dt.Rows[i]["一级标题"];
                    if (dt.Rows[i]["二级标题ID"].ToString().Trim() != "")
                    {
                        drlist["SecondTitleID"] = dt.Rows[i]["二级标题ID"].ToString();
                    }
                    drlist["SecondTitle"] = dt.Rows[i]["二级标题"];
                    drlist["ModuleID"] = dt.Rows[i]["一级模块ID"];
                    drlist["ModuleName"] = dt.Rows[i]["一级模块"];
                    if (dt.Rows[i]["模块地址"].ToString().Trim() != "")
                    {
                        drlist["ModuleAddress"] = dt.Rows[i]["模块地址"].ToString().Trim();
                    }
                    if (dt.Rows[i]["MD5值"].ToString().Trim() != "")
                    {
                        drlist["MD5"] = dt.Rows[i]["MD5值"].ToString().Trim();
                    }

                    dtList.Rows.Add(drlist);
                }


                SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
                {
                    BulkCopyTimeout = 5000,
                    NotifyAfter = dtList.Rows.Count,
                };
                if (dt.Rows[0]["一级模块"].ToString().Contains("YX"))
                {
                    string strSql = "DELETE FROM dbo.[TB_VersionChange_YX] WHERE BookID=" + Convert.ToInt32(Bookid) + " AND ModuleID='" + dt.Rows[0]["一级模块ID"] + "'";
                    SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

                    try
                    {
                        sbc.DestinationTableName = "TB_VersionChange_YX";
                        sbc.WriteToServer(dtList); //此处报错
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
                    }

                    if (sbc.NotifyAfter <= 0)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('导入成功！');</script>");
                        //File.Delete(newFileName);
                    }
                }
                else
                {

                    string strSql = "DELETE FROM dbo.[TB_VersionChange] WHERE BookID=" + Convert.ToInt32(Bookid) + " AND ModuleID='" + dt.Rows[0]["一级模块ID"] + "'";
                    SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

                    try
                    {
                        sbc.DestinationTableName = "TB_VersionChange";
                        sbc.WriteToServer(dtList); //此处报错
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
                    }

                    if (sbc.NotifyAfter <= 0)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('导入成功！');</script>");
                        //File.Delete(newFileName);
                    }
                }

            }
        }

        private string GetNewFileName(string strName, string newFileName)
        {
            if (strName != "") //如果文件名存在
            {
                bool fileOk = false;
                int i = strName.LastIndexOf(".", StringComparison.Ordinal); //获取。的索引顺序号，在这里。代表文件名字与后缀的间隔
                string kzm = strName.Substring(i);
                //获取文件扩展名的另一种方法 string fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                string juedui = Server.MapPath("~\\Upload\\Excel\\");
                //设置文件保存的本地目录绝对路径，对于路径中的字符“＼”在字符串中必须以“＼＼”表示，因为“＼”为特殊字符。或者可以使用上一行的给路径前面加上＠
                newFileName = juedui + strName;
                if (FileUpload1.HasFile) //验证 FileUpload 控件确实包含文件
                {
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    foreach (string t in allowedExtensions)
                    {
                        if (kzm == t)
                        {
                            fileOk = true;
                        }
                    }
                }
                if (fileOk)
                {
                    try
                    {
                        // 判定该路径是否存在
                        if (!Directory.Exists(juedui))
                            Directory.CreateDirectory(juedui);
                        FileUpload1.PostedFile.SaveAs(newFileName); //将文件存储到服务器上
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                            "<script type=\"text/javascript\">alert('" + StringRep(ex.Message) + "');</script>");
                    }
                }
            }
            return newFileName;
        }

        public string StringRep(string name)
        {
            string str = name.Replace("'", "\'");
            return str;
        }

        /// <summary>
        /// 转换int型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = -1;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="fileName"></param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, string fileName, bool isFirstRowColumn)
        {
            DataTable data = new DataTable();
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
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
    }
}