using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.PSO;
using Kingsun.SynchronousStudy.Web.Handler;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Kingsun.SynchronousStudy.Web.CourseManagement
{
    public partial class CourseManageMent : System.Web.UI.Page
    {
        string Action = "";
        public string menuList = "";
        public ClientUserinfo UserInfo = new ClientUserinfo();
        CourseBLL couserbll = new CourseBLL();
        ModuleConfigurationBLL moduleConfigurationBLL = new ModuleConfigurationBLL();
        private IWorkbook _workbook;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo = CheckLogin.Check(HttpContext.Current, ref menuList);
            if (!IsPostBack)
            {
                ActionInit();
            }
        }

        private void ActionInit()
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
                    string strWhere = "";

                    int? bookID = 0;
                    int totalcount = 0;
                    IList<TB_CurriculumManage> courseList = new List<TB_CurriculumManage>();
                    if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                    {
                        var obj1 = new { rows = courseList, total = totalcount };
                        Response.Write(JsonHelper.EncodeJson(obj1));
                        Response.End();
                    }
                    int pageindex = int.Parse(Request.Form["page"].ToString());
                    int pagesize = int.Parse(Request.Form["rows"].ToString());
                    if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        strWhere = Request.QueryString["queryStr"];
                    }
                    strWhere = string.IsNullOrEmpty(strWhere) ? " 1=1 and State = 1 ORDER BY CreateDate DESC" : strWhere;
                    courseList = couserbll.GetCourseList(strWhere);
                    if (courseList == null)
                    {
                        courseList = new List<TB_CurriculumManage>();
                    }
                    else
                    {
                        totalcount = courseList.Count;
                        IList<TB_CurriculumManage> removelist = new List<TB_CurriculumManage>();
                        for (int i = 0; i < courseList.Count; i++)
                        {
                            if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                            {
                                removelist.Add(courseList[i]);
                            }
                        }
                        if (removelist != null && removelist.Count > 0)
                        {
                            for (int i = 0; i < removelist.Count; i++)
                            {
                                courseList.Remove(removelist[i]);
                            }
                        }
                        //判断书本是否注册模块
                        for (int i = 0; i < courseList.Count; i++)
                        {
                            bookID = courseList[i].BookID;
                            strWhere = " BookID = '" + bookID + "' And State=0";
                            IList<TB_ModuleConfiguration> moduleList = moduleConfigurationBLL.GetModuleByID(strWhere);
                            if (moduleList == null || moduleList.Count == 0)
                            {
                                courseList[i].State = false;
                            }
                            else
                            {
                                courseList[i].State = true;
                            }
                        }
                    }

                    var obj = new { rows = courseList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "save":
                    string EducationLevel = Request.Form["EducationLevel"];
                    int stageID = Convert.ToInt32(Request.Form["StageID"]);
                    string CourseCategory = Request.Form["CourseCategory"];
                    int subjectID = Convert.ToInt32(Request.Form["SubjectID"]);
                    string TextbookVersion = Request.Form["TextbookVersion"];
                    int editionID = Convert.ToInt32(Request.Form["EditionID"]);
                    string JuniorGrade = Request.Form["JuniorGrade"];
                    int gradeID = Convert.ToInt32(Request.Form["GradeID"]);
                    string TeachingBooks = Request.Form["TeachingBooks"];
                    int breelID = Convert.ToInt32(Request.Form["BreelID"]);
                    string CourseCover = Request.Form["CourseCover"];
                    string UserName = Request.Form["UserName"];
                    string CreateDate = Request.Form["CreateDate"];
                    int BookID = Int32.Parse(Request.Form["BookID"]);
                    TB_CurriculumManage course = new TB_CurriculumManage();
                    course.TeachingNaterialName = EducationLevel + CourseCategory + TextbookVersion + JuniorGrade + TeachingBooks;
                    course.EducationLevel = EducationLevel;
                    course.StageID = stageID;
                    course.CourseCategory = CourseCategory;
                    course.SubjectID = subjectID;
                    course.TextbookVersion = TextbookVersion;
                    course.EditionID = editionID;
                    course.JuniorGrade = JuniorGrade;
                    course.GradeID = gradeID;
                    course.TeachingBooks = TeachingBooks;
                    course.BreelID = breelID;
                    course.CourseCover = CourseCover;
                    course.UserName = UserName;
                    course.State = true;
                    course.CreateDate = DateTime.Parse(CreateDate);
                    course.BookID = BookID;
                    if (couserbll.AddCourse(course))
                    {
                        Response.Write(JsonHelper.EncodeJson(new { result = true }));
                        Response.End();
                    }
                    else
                    {
                        Response.Write(JsonHelper.EncodeJson(new { result = false }));
                        Response.End();
                    }
                    break;
                case "delete":
                    string courseid = Request.Form["CourseID"];
                    TB_CurriculumManage cor = couserbll.GetCourseByID(Int32.Parse(courseid));
                    cor.State = false;
                    if (couserbll.UpdateCourse(cor))
                    {
                        Response.Write(JsonHelper.EncodeJson(new { result = true }));
                        Response.End();
                    }
                    else
                    {
                        Response.Write(JsonHelper.EncodeJson(new { result = false }));
                        Response.End();
                    }
                    break;
                case "updatacatalog":
                    string bookid = Request.Form["BookID"];
                    string bookname = Request.Form["BookName"];
                    UpdateCatalog(bookid, bookname);
                    break;
                case "ossuploadimg":
                    OssUploadImg();
                    break;
                default:
                    Response.Write("");
                    Response.End();
                    break;
            }
        }
        public void OssUploadImg()
        {
            try
            {
                HttpFileCollection files = Request.Files;
                if (files.Count > 0)
                {
                    Response.Write("ok");
                }
                else
                {
                    Response.Write("0");
                } 
            }
            catch (Exception ex)
            {
                Response.Write("{'msg':'0'}");
            }
        }
        private void UpdateCatalog(string bookid, string bookname)
        {
            string modUrl = WebConfigurationManager.AppSettings["modUrl"];
            StreamReader responseReader = null;
            List<ModuleData.Data> listS = new List<ModuleData.Data>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            DataTable dt = new DataTable();

            try
            {
                //ashx Url
                // string getGscUserUrl = "http://183.47.42.221:8027/GetCatalogByBookId.sun";
                //加入参数，用于更新请求
                string urlHandler = modUrl + "GETALLCATALOGBYBOOKID.sun?t[BookId]=" + bookid;
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlHandler);
                webRequest.Timeout = 3000; //3秒超时
                //调用ashx，并取值
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string currentUserGulid = responseReader.ReadToEnd();

                ModuleData.Data[] bookdata = js.Deserialize<ModuleData.Data[]>(currentUserGulid.Trim());
                listS = new List<ModuleData.Data>(bookdata);
                // List<info> infos = Dal.GetInfos();  
                dt.Columns.Add("ID");
                dt.Columns.Add("BookID");
                dt.Columns.Add("TeachingNaterialName");
                dt.Columns.Add("FirstTitileID");
                dt.Columns.Add("FirstTitle");
                dt.Columns.Add("SecondTitleID");
                dt.Columns.Add("SecondTitle");
                dt.Columns.Add("CreateDate");
                dt.Columns.Add("State");

                foreach (var info in listS)
                {
                    if (info.Children != null && info.Children.Length > 0)
                    {
                        foreach (var children in info.Children)
                        {
                            DataRow dr = dt.NewRow();
                            dr["BookID"] = ParseInt(bookid);
                            dr["TeachingNaterialName"] = bookname;
                            dr["FirstTitileID"] = info.Id;
                            dr["FirstTitle"] = info.Title;
                            dr["SecondTitleID"] = children.Id;
                            dr["SecondTitle"] = children.Title;
                            dr["State"] = children.isRemove;
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        dr["BookID"] = ParseInt(bookid);
                        dr["TeachingNaterialName"] = bookname;
                        dr["FirstTitileID"] = info.Id;
                        dr["FirstTitle"] = info.Title;
                        dr["State"] = info.isRemove;
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(JsonHelper.EncodeJson(new { result = false }));
                Response.End();
            }
            finally
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                }
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };

            string strSql = "DELETE FROM dbo.[TB_ModuleConfiguration] WHERE BookID=" + Convert.ToInt32(bookid);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "TB_ModuleConfiguration";
                sbc.WriteToServer(dt); //此处报错
            }
            catch (Exception ex)
            {
                Response.Write(JsonHelper.EncodeJson(new { result = false }));
                Response.End();

            }


            if (sbc.NotifyAfter <= 0)
            {
                Response.Write(JsonHelper.EncodeJson(new { result = false }));
                Response.End();
            }
            else
            {
                Response.Write(JsonHelper.EncodeJson(new { result = true }));
                Response.End();
                //File.Delete(newFileName);
            }

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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ImportVideoDetails();
        }

        /// <summary>
        /// 导入活动资源
        /// </summary>
        private void ImportVideoDetails()
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
            DataTable dtSheet2 = new DataTable();
            try
            {
                dt = ExcelToDataTable("总视频信息", newFileName, true);
                dtSheet2 = ExcelToDataTable("分视频信息", newFileName, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('导入：" + ex.Message + "');</script>");
                throw ex;
            }
            //string path = Server.MapPath("~/Upload/Excel/");//"C:\\TeamCity\\buildAgent\\work\\88580d4c6fbacf3\\Kingsun.SynchronousStudy.Web\\Upload\\Excel\\" + GetBookName(Convert.ToInt32(Bookid)) + ".xls";
            //string fileName = FileUpload1.FileName;// GetBookName(Convert.ToInt32(Bookid)) + ".xls";


            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("BookName");
            dtList.Columns.Add("FirstTitleID");
            dtList.Columns.Add("FirstTitle");
            dtList.Columns.Add("SecondTitleID");
            dtList.Columns.Add("SecondTitle");
            dtList.Columns.Add("FirstModularID");
            dtList.Columns.Add("FirstModular");
            dtList.Columns.Add("SecondModularID");
            dtList.Columns.Add("SecondModular");
            dtList.Columns.Add("VideoTitle");
            dtList.Columns.Add("VideoNumber");
            dtList.Columns.Add("MuteVideo");
            dtList.Columns.Add("CompleteVideo");
            //dtList.Columns.Add("VideoTime");
            dtList.Columns.Add("BackgroundAudio");
            dtList.Columns.Add("VideoCover");
            dtList.Columns.Add("VideoDesc");
            dtList.Columns.Add("VideoDifficulty");
            dtList.Columns.Add("State");
            dtList.Columns.Add("CreateTime");
            dtList.Columns.Add("VideoType");

            DataTable dtDialogue = new DataTable();
            dtDialogue.Columns.Add("ID");
            dtDialogue.Columns.Add("VideoID");
            dtDialogue.Columns.Add("BookID");
            dtDialogue.Columns.Add("FirstTitleID");
            dtDialogue.Columns.Add("SecondTitleID");
            dtDialogue.Columns.Add("FirstModularID");
            dtDialogue.Columns.Add("SecondModularID");
            dtDialogue.Columns.Add("DialogueText");
            dtDialogue.Columns.Add("DialogueNumber");
            dtDialogue.Columns.Add("StartTime");
            dtDialogue.Columns.Add("EndTime");
            dtDialogue.Columns.Add("CreateTime");
            dtDialogue.Columns.Add("ActiveID");

            if (dt.Rows.Count <= 0 || dtSheet2.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["VideoNumber"] = ParseInt(dt.Rows[i]["序号"]);
                drlist["VideoTitle"] = dt.Rows[i]["视频标题"];
                drlist["MuteVideo"] = dt.Rows[i]["静音视频"];
                drlist["CompleteVideo"] = dt.Rows[i]["完整视频"];
                //drlist["VideoTime"] = ParseInt(dt.Rows[i]["视频时长"]);
                drlist["BackgroundAudio"] = dt.Rows[i]["背景音频"];
                drlist["VideoCover"] = dt.Rows[i]["视频封图"];
                drlist["VideoDesc"] = dt.Rows[i]["视频简介"];
                drlist["VideoDifficulty"] = ParseInt(dt.Rows[i]["难易程度"]);
                drlist["State"] = "1";
                drlist["VideoType"] = "1";
                dtList.Rows.Add(drlist);
            }

            for (int i = 0; i < dtSheet2.Rows.Count; i++)
            {
                DataRow drDialogue = dtDialogue.NewRow();
                //drDialogue["FirstTitleID"] = Convert.ToInt32((string)dt.Rows[i]["一级标题ID"] == "" ? "0" : dt.Rows[i]["一级标题ID"]);
                //drDialogue["SecondTitleID"] = Convert.ToInt32((string)dt.Rows[i]["二级标题ID"] == "" ? "0" : dt.Rows[i]["二级标题ID"]);
                //drDialogue["FirstModularID"] = Convert.ToInt32((string)dt.Rows[i]["一级模块ID"] == "" ? "0" : dt.Rows[i]["一级模块ID"]);
                //drDialogue["SecondModularID"] = Convert.ToInt32((string)dt.Rows[i]["二级模块ID"] == "" ? "0" : dt.Rows[i]["二级模块ID"]);
                drDialogue["DialogueText"] = dtSheet2.Rows[i]["分视频文本"].ToString();
                drDialogue["DialogueNumber"] = ParseInt(dtSheet2.Rows[i]["分视频序号"]);
                if (dtSheet2.Rows[i]["起始时间"].ToString() != "")
                {
                    if (IsDate(dtSheet2.Rows[i]["起始时间"].ToString()))
                    {
                        drDialogue["StartTime"] = dtSheet2.Rows[i]["起始时间"];
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('序号为" + dtSheet2.Rows[i]["序号"] + "，分视频序号为：" + dtSheet2.Rows[i]["分视频序号"] + "的开始时间格式错误！');</script>");
                        return;
                    }

                }
                if (dtSheet2.Rows[i]["结束时间"].ToString() != "")
                {
                    if (IsDate(dtSheet2.Rows[i]["结束时间"].ToString()))
                    {
                        drDialogue["EndTime"] = dtSheet2.Rows[i]["结束时间"];
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('序号为" + dtSheet2.Rows[i]["序号"] + "，分视频序号为：" + dtSheet2.Rows[i]["分视频序号"] + "的结束时间格式错误！');</script>");
                        return;
                    }
                }
                drDialogue["ActiveID"] = dtSheet2.Rows[i]["序号"];
                dtDialogue.Rows.Add(drDialogue);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };
            SqlBulkCopy sbc1 = new SqlBulkCopy(SqlHelper.GetConnectionString("TB_VideoDialogue"), SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };

            string strSql = "DELETE FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE VideoType =1";
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);
            strSql = "DELETE FROM [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] WHERE  ActiveID IS NOT NULL";
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "[FZ_InterestDubbing].[dbo].[TB_VideoDetails]";
                sbc.WriteToServer(dtList); //此处报错
                sbc1.DestinationTableName = "TB_VideoDialogue";
                sbc1.WriteToServer(dtDialogue); //此处报错
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
            }

            if (sbc.NotifyAfter <= 0 || sbc1.NotifyAfter <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('导入成功！');</script>");
                File.Delete(newFileName);
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
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 判断是否属于时间格式
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = string.Format(@"UPDATE [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] SET ActiveID=(SELECT TOP 1 ID FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE VideoType =1 AND VideoNumber=1) WHERE ActiveID=1;
  UPDATE [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] SET ActiveID=(SELECT TOP 1 ID FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE VideoType =1 AND VideoNumber=2) WHERE ActiveID=2;
  UPDATE [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] SET ActiveID=(SELECT TOP 1 ID FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE VideoType =1 AND VideoNumber=3) WHERE ActiveID=3;");
                if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi",
                     "<script type=\"text/javascript\">alert('ActiveID更新成功！');</script>");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                                    "<script type=\"text/javascript\">alert('错误信息：" + ex.Message + "！');</script>");
            }
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            ImportBookResource();
        }

        /// <summary>
        /// 导入书籍资源
        /// </summary>
        private void ImportBookResource()
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
                dt = ExcelToDataTable("目录信息", newFileName, true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('导入：" + ex.Message + "');</script>");
                throw ex;
            }

            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("ResourceUrl");
            dtList.Columns.Add("CreateDate");

            if (dt.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('Excel表数据为空或Excel是打开状态！');</script>");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow drlist = dtList.NewRow();
                drlist["BookID"] = ParseInt(dt.Rows[i]["书籍ID"]);
                drlist["ResourceUrl"] = dt.Rows[i]["资源地址"];
                dtList.Rows.Add(drlist);
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };

            string strSql = "DELETE FROM [FZ_HearResources].[dbo].[TB_BookResource]";
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "[FZ_HearResources].[dbo].TB_BookResource";
                sbc.WriteToServer(dtList); //此处报错
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('错误：" + ex.Message + "');</script>");
            }

            if (sbc.NotifyAfter <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('插入" + sbc.NotifyAfter + "条数据！');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi",
                    "<script type=\"text/javascript\">alert('导入成功！');</script>");
                File.Delete(newFileName);
            }
        }
    }
}