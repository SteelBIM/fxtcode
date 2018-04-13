using Kingsun.SpokenBroadcas.BLL;
using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.SpokenBroadcasManagement
{
    public partial class UserAppointCount : System.Web.UI.Page
    {
        CourseBLL courseBll = new CourseBLL();
        UserBLL userBll = new UserBLL();
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string Action = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActionInit();
            }
        }
        private void ActionInit()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
                {
                    Action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写 
                }
                else
                {
                    return;
                }
                switch (Action)
                {
                    case "query":
                        query();
                        break;
                    case "excel":
                        excel();
                        break;
                    case "getuserinfobytelephone":
                        getUserInfoByTelePhone();
                        break;
                    case "getcourseperiodtimesearch":
                        getCoursePeriodTimeSearch();
                        break;
                    case "handadduserappointinfo":
                        HandAddUserAppointInfo();
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            Response.End();
        }
        /// <summary>
        /// 手动添加预约用户信息
        /// </summary>
        public void HandAddUserAppointInfo()
        {
            try
            {
                string UserID = Request["UserID"];
                string UserName = Request["UserName"];
                string TrueName = Request["TrueName"];
                string TelePhone = Request["TelePhone"];
                string CoursePeriodTimeID = Request["CoursePeriodTimeID"];
                DataTable dt = courseBll.getCoursePeriodTimeSearch("c.ID in(" + CoursePeriodTimeID + ")").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    int msg_success = 0, msg_fail = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        bool flag = courseBll.AddUserAppointInfo(UserID, UserName, TrueName, TelePhone, row["CoursePeriodTimeID"].ToString(), DateTime.Now.ToString(), row["CourseName"].ToString(), row["CoursePeriodName"].ToString(), row["StartTime"].ToString(), row["EndTime"].ToString(),"0");
                        if (flag)
                        {
                            msg_success++;
                        }
                        else
                        {
                            msg_fail++;
                        }
                    }
                    string msg = string.Format(@"预约成功，总共添加：{0} 条数据，成功：{1} 条，失败：{2} 条。", dt.Rows.Count, msg_success, msg_fail);
                    Response.Write("{'state':1,'msg':'" + msg + "'}");
                }
                else
                {
                    Response.Write("'state':0,'msg':'预约失败，未找到该课程:"+CoursePeriodTimeID+"'");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("'state':0,'msg':'预约失败，操作异常'");
            }
        }
        /// <summary>
        /// 获取课时列表
        /// </summary>
        public void getCoursePeriodTimeSearch()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                IList<selectCoursePeriodModel> listModel = new List<selectCoursePeriodModel>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = listModel, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                }
                int pageindex = int.Parse(Request.Form["page"].ToString());
                int pagesize = int.Parse(Request.Form["rows"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = " 1=1";
                }
                DataSet set = courseBll.getCoursePeriodTimeSearch(strWhere);
                listModel = DataSetToIList<selectCoursePeriodModel>(set, 0);
                if (listModel == null)
                {
                    listModel = new List<selectCoursePeriodModel>();
                }
                else
                {
                    totalcount = listModel.Count;
                    List<selectCoursePeriodModel> removelist = new List<selectCoursePeriodModel>();
                    for (int i = 0; i < listModel.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(listModel[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            listModel.Remove(removelist[i]);
                        }
                    }
                }
                var obj = new { rows = listModel, total = totalcount };
                Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        public void getUserInfoByTelePhone()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                IList<Tb_UserInfo> listModel = new List<Tb_UserInfo>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = listModel, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                }
                int pageindex = int.Parse(Request.Form["page"].ToString());
                int pagesize = int.Parse(Request.Form["rows"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = " 1=1";
                }
                listModel = userBll.GetUserInfo(strWhere);
                if (listModel == null)
                {
                    listModel = new List<Tb_UserInfo>();
                }
                else
                {
                    totalcount = listModel.Count;
                    List<Tb_UserInfo> removelist = new List<Tb_UserInfo>();
                    for (int i = 0; i < listModel.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(listModel[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            listModel.Remove(removelist[i]);
                        }
                    }
                }
                var obj = new { rows = listModel, total = totalcount };
                Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        public void excel()
        {
            try
            {
                string strWhere = "";
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = "and 1=1";
                }
                DataTable dt = courseBll.GetUserAppointCount(strWhere).Tables[0];
                dt.Columns[0].ColumnName = "用户ID";
                dt.Columns[1].ColumnName = "用户名";
                dt.Columns[2].ColumnName = "真实姓名";
                dt.Columns[3].ColumnName = "联系方式";
                dt.Columns[4].ColumnName = "预约课程ID";
                dt.Columns[5].ColumnName = "预约课程名称";
                dt.Columns[6].ColumnName = "预约课时ID";
                dt.Columns[7].ColumnName = "预约课时名称";
                dt.Columns[8].ColumnName = "预约时间";
                dt.Columns[9].ColumnName = "上课时间";
                dt.Columns[10].ColumnName = "预约价格";
                MemoryStream s = dt.ToExcel() as MemoryStream;
                if (s != null)
                {
                    byte[] excel = s.ToArray();
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=预约用户列表.xlsx"));
                    Response.AddHeader("Content-Length", excel.Length.ToString());
                    Response.BinaryWrite(excel);
                    s.Close();
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                List<UserAppointCountModel> listModel = new List<UserAppointCountModel>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = listModel, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                }
                int pageindex = int.Parse(Request.Form["page"].ToString());
                int pagesize = int.Parse(Request.Form["rows"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = "and 1=1";
                }
                DataSet set = courseBll.GetUserAppointCount(strWhere);
                listModel = DataSetToIList<UserAppointCountModel>(set, 0);
                if (listModel == null)
                {
                    listModel = new List<UserAppointCountModel>();
                }
                else
                {
                    totalcount = listModel.Count;
                    List<UserAppointCountModel> removelist = new List<UserAppointCountModel>();
                    for (int i = 0; i < listModel.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(listModel[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            listModel.Remove(removelist[i]);
                        }
                    }
                }
                var obj = new { rows = listModel, total = totalcount };
                Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
        /// <summary>
        /// 导入预约用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string newFileName = string.Empty;
                string strName = FileUpload1.PostedFile.FileName; //使用fileupload控件获取上传文件的文件名
                if (string.IsNullOrEmpty(strName))
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi",
                        "<script type=\"text/javascript\">alert('请选择文件！');</script>");
                    return;
                }
                newFileName = GetNewFileName(strName, newFileName);
                DataTable dt = new DataTable();
                try
                {
                    dt = ExcelToDataTable("预约用户信息", newFileName, true);
                    if (dt.Rows.Count <= 0)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                            "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
                        return;
                    }
                    else
                    {
                        string UserID = ""; string CoursePeriodTimeID = "";
                        int msg_success = 0, msg_fail = 0, msg_null = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            UserID = row["用户ID"].ToString();
                            CoursePeriodTimeID = row["课时时间ID"].ToString();
                            if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(CoursePeriodTimeID))
                            {
                                msg_null++;
                            }
                            else
                            {
                                bool flag = courseBll.AddUserAppointInfo(UserID, row["用户名"].ToString(), row["真实姓名"].ToString(), row["联系方式"].ToString(), row["课时时间ID"].ToString(), row["预约时间"].ToString(), row["课程名称"].ToString(), row["课时名称"].ToString(), row["课时开始时间"].ToString(), row["课时结束时间"].ToString(), row["价格"].ToString());
                                if (flag)
                                {
                                    msg_success++;
                                }
                                else
                                {
                                    msg_fail++;
                                }
                            }
                        }
                        string msg = string.Format(@"总共导入：{0} 条数据，成功：{1} 条，失败：{2} 条，用户ID或课时时间ID不存在的：{3} 条。", dt.Rows.Count, msg_success, msg_fail, msg_null);
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                       "<script type=\"text/javascript\">alert('" + msg + "');</script>");
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi",
                        "<script type=\"text/javascript\">alert('导入：" + ex.Message + "');</script>");
                    log.Error("error", ex);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                      "<script type=\"text/javascript\">alert('导入：" + ex.Message + "');</script>");
                log.Error("error", ex);
            }
        }
        private IWorkbook _workbook;
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
                log.Error("error", ex);
                return null;
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
                newFileName = juedui + strName + kzm;
                if (FileUpload1.HasFile) //验证 FileUpload 控件确实包含文件
                {
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int j = 0; j < allowedExtensions.Length; j++)
                    {
                        if (kzm == allowedExtensions[j])
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
                        FileUpload1.PostedFile.SaveAs(newFileName); //将图片存储到服务器上
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi",
                            "<script type=\"text/javascript\">alert('" + ex.Message + "');</script>");
                    }
                }
            }
            return newFileName;
        }
    }
    public class selectCoursePeriodModel
    {
        public int CoursePeriodTimeID { get; set; }
        public string CourseName { get; set; }
        public string Groups { get; set; }
        public string CoursePeriodName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal NewPrice { get; set; }
    }

    /// <summary>
    /// 预约用户统计实体
    /// </summary>
    class UserAppointCountModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string TelePhone { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int CoursePeriodID { get; set; }
        public string CoursePeriodName { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public string StartTime { get; set; }
        ///// <summary>
        ///// 结束时间
        ///// </summary>
        //public DateTime EndTime { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal NewPrice { get; set; }
    }
}