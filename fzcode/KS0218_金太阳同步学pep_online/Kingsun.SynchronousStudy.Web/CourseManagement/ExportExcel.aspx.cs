using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.Web.Handler;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.CourseManagement
{
    public partial class ExportExcel : Page
    {
        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int Bookid;
        public string BookName;
        public string Type;
        protected void Page_Load(object sender, EventArgs e)
        {
            Bookid = ParseInt(Request.QueryString["BookID"]);
            Type = Request.QueryString["Type"];
            if (!IsPostBack)
            {
                if (Bookid > 0)
                {
                    BookName = GetBookName(Bookid);
                    lbBookName.Text = BookName;
                    if (Type != "catalog")
                    {
                        DataBind(Bookid);
                        btnAdd.Visible = false;
                    }
                }
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

        private string GetBookName(int bookid)
        {
            string bookname = "";
            string sql = @"SELECT top 1 [TeachingNaterialName] FROM [TB_ModuleConfiguration] WHERE BookID=" + bookid;
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
                string[] arg = e.CommandArgument.ToString().Split(',');
                string sql = string.Format(@"SELECT  mc.BookID ,
                                                    mc.TeachingNaterialName ,
                                                    mc.FirstTitileID ,
                                                    mc.FirstTitle ,
                                                    mc.SecondTitleID ,
                                                    mc.SecondTitle ,
                                                    ms.ModuleID ,
                                                    ms.ModuleName ,
                                                    ms.SuperiorID 
                                            FROM    dbo.TB_ModuleConfiguration AS mc
                                                    RIGHT JOIN dbo.TB_ModuleSort AS ms ON ms.BookID = mc.BookID
                                                                                            AND ( ms.SecondTitleID = mc.SecondTitleID
                                                                                                OR ( ms.FirstTitleID = mc.FirstTitileID
                                                                                                        AND (ms.SecondTitleID IS NULL OR ms.SecondTitleID=0)
                                                                                                    )
                                                                                                )
                                            WHERE   mc.BookID = {0}
                                                    AND (ms.ModuleID = {1} OR ms.SuperiorID={1})
                                            ORDER BY mc.FirstTitileID ASC;", Bookid, arg[1]);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];

                if (dt.Rows.Count <= 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "tishi", "<script type=\"text/javascript\">alert('数据不存在！');</script>");
                }
                DataTable dtList = new DataTable();
                DataTable dtSheet = new DataTable();
                switch (dt.Rows[0]["ModuleName"].ToString())
                {
                    case "趣配音":
                        dtList = GetExcelSheetOne(dt);
                        dtSheet = GetExcelSheetTwo(dt);
                        break;
                    case "说说看":
                        dtList = GetExcelSheetHearResources(dt);
                        break;
                    case "玩单词":
                        dtList = GetExcelSheetWordGame(dt);
                        break;
                    case "练习册":
                        dtList = GetExcelExerciseBook(dt);
                        break;
                    case "YX2_课本剧":
                        dtList = GetExcelSheetOne(dt);
                        dtSheet = GetExcelSheetTwo(dt);
                        break;
                    case "期末测评卷":
                        ExportExcelTemp("同步学期末考试卷导入模板.xlsx");
                        return;
                    default:
                        break;
                }

                //DataTable dtList = GetExcelSheetOne(dt);
                //DataTable dtSheet = GetExcelSheetTwo(dt);

                // string path = Server.MapPath("../Upload/Excel/"); //"C:\\TeamCity\\buildAgent\\work\\88580d4c6fbacf3\\Kingsun.SynchronousStudy.Web\\Upload\\Excel\\";
                string fileName = dt.Rows[0]["TeachingNaterialName"].ToString() + dt.Rows[0]["ModuleName"] + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WriteExcel(dtList, dtSheet, fileName);
            }
        }

        /// <summary>
        /// 导出Excel模板
        /// </summary>
        /// <param name="filename"></param>
        private void ExportExcelTemp(string filename)
        {
            var context = HttpContext.Current;
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", context.Server.UrlEncode(filename)));
            context.Response.Clear();
            context.Response.WriteFile(Server.MapPath("~/Upload/Excel/" + filename));
            context.Response.End();
        }

        /// <summary>
        /// 趣配音（总视频信息）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetExcelSheetOne(DataTable dt)
        {
            DataTable dtList = new DataTable();
            dtList.TableName = "总视频信息";
            //dtList.Columns.Add("书籍ID");
            //dtList.Columns.Add("书籍名称");
            dtList.Columns.Add("一级标题ID");
            dtList.Columns.Add("一级标题");
            dtList.Columns.Add("二级标题ID");
            dtList.Columns.Add("二级标题");
            dtList.Columns.Add("一级模块ID");
            dtList.Columns.Add("一级模块");
            dtList.Columns.Add("二级模块ID");
            dtList.Columns.Add("二级模块");
            dtList.Columns.Add("序号");
            dtList.Columns.Add("视频标题");
            dtList.Columns.Add("静音视频");
            dtList.Columns.Add("完整视频");
            dtList.Columns.Add("背景音频");
            dtList.Columns.Add("视频封图");
            dtList.Columns.Add("视频简介");
            dtList.Columns.Add("难易程度");

            var moduleid = "";
            var module = "";
            int count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["SuperiorID"].ToString() != dt.Rows[i]["FirstTitileID"].ToString() && dt.Rows[i]["SuperiorID"].ToString() != dt.Rows[i]["SecondTitleID"].ToString())
                {
                    count++;
                }
            }

            if (count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtList.NewRow();
                    if (dt.Rows[i]["SuperiorID"].ToString() == dt.Rows[i]["FirstTitileID"].ToString()
                        || dt.Rows[i]["SuperiorID"].ToString() == dt.Rows[i]["SecondTitleID"].ToString())
                    {
                        moduleid = dt.Rows[i]["ModuleID"].ToString();
                        module = dt.Rows[i]["ModuleName"].ToString();
                    }
                    else switch (dt.Rows[i]["ModuleName"].ToString())
                        {
                            case "课内配音":
                                for (int j = 0; j < 3; j++)
                                {
                                    dr = dtList.NewRow();
                                    //dr["书籍ID"] = dt.Rows[i]["BookID"];
                                    //dr["书籍名称"] = dt.Rows[i]["TeachingNaterialName"];
                                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"];
                                    dr["一级标题"] = dt.Rows[i]["FirstTitle"];
                                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"];
                                    dr["二级标题"] = dt.Rows[i]["SecondTitle"];
                                    dr["一级模块ID"] = moduleid;
                                    dr["一级模块"] = module;
                                    dr["二级模块ID"] = dt.Rows[i]["ModuleID"];
                                    dr["二级模块"] = dt.Rows[i]["ModuleName"];
                                    dtList.Rows.Add(dr);
                                }
                                break;
                            case "电影配音":
                                for (int j = 0; j < 2; j++)
                                {
                                    dr = dtList.NewRow();
                                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"];
                                    dr["一级标题"] = dt.Rows[i]["FirstTitle"];
                                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"];
                                    dr["二级标题"] = dt.Rows[i]["SecondTitle"];
                                    dr["一级模块ID"] = moduleid;
                                    dr["一级模块"] = module;
                                    dr["二级模块ID"] = dt.Rows[i]["ModuleID"];
                                    dr["二级模块"] = dt.Rows[i]["ModuleName"];
                                    dtList.Rows.Add(dr);
                                }
                                break;
                            default:
                                dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"];
                                dr["一级标题"] = dt.Rows[i]["FirstTitle"];
                                dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"];
                                dr["二级标题"] = dt.Rows[i]["SecondTitle"];
                                dr["一级模块ID"] = moduleid;
                                dr["一级模块"] = module;
                                dr["二级模块ID"] = dt.Rows[i]["ModuleID"];
                                dr["二级模块"] = dt.Rows[i]["ModuleName"];
                                dtList.Rows.Add(dr);
                                break;
                        }
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtList.NewRow();
                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"].ToString();
                    dr["一级标题"] = dt.Rows[i]["FirstTitle"].ToString();
                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"].ToString();
                    dr["二级标题"] = dt.Rows[i]["SecondTitle"].ToString();
                    dr["一级模块ID"] = dt.Rows[i]["ModuleID"].ToString();
                    dr["一级模块"] = dt.Rows[i]["ModuleName"].ToString();
                    dtList.Rows.Add(dr);
                }
            }

            return dtList;
        }

        /// <summary>
        /// 趣配音（分视频信息）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetExcelSheetTwo(DataTable dt)
        {
            DataTable dtList = new DataTable();
            dtList.TableName = "分视频信息";
            dtList.Columns.Add("序号");
            dtList.Columns.Add("分视频序号");
            dtList.Columns.Add("分视频文本");
            dtList.Columns.Add("起始时间");
            dtList.Columns.Add("结束时间");
            return dtList;
        }

        /// <summary>
        /// 说说看(总视频信息)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetExcelSheetHearResources(DataTable dt)
        {
            DataTable dtList = new DataTable();
            dtList.TableName = "总视频信息";
            //dtList.Columns.Add("书籍ID");
            //dtList.Columns.Add("书籍名称");
            dtList.Columns.Add("一级标题ID");
            dtList.Columns.Add("一级标题");
            dtList.Columns.Add("二级标题ID");
            dtList.Columns.Add("二级标题");
            dtList.Columns.Add("一级模块ID");
            dtList.Columns.Add("一级模块");
            dtList.Columns.Add("二级模块ID");
            dtList.Columns.Add("二级模块");
            dtList.Columns.Add("二级模块英文标题");
            dtList.Columns.Add("序号");
            dtList.Columns.Add("子序号");
            dtList.Columns.Add("角色名");
            dtList.Columns.Add("文本");
            dtList.Columns.Add("音频");
            dtList.Columns.Add("图片");

            var moduleid = "";
            var module = "";
            int count = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["SuperiorID"].ToString() == dt.Rows[i]["FirstTitileID"].ToString()
                   || dt.Rows[i]["SuperiorID"].ToString() == dt.Rows[i]["SecondTitleID"].ToString())
                {
                    moduleid = dt.Rows[i]["ModuleID"].ToString();
                    module = dt.Rows[i]["ModuleName"].ToString();
                }
                else
                {
                    DataRow dr = dtList.NewRow();
                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"].ToString();
                    dr["一级标题"] = dt.Rows[i]["FirstTitle"].ToString();
                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"].ToString();
                    dr["二级标题"] = dt.Rows[i]["SecondTitle"].ToString();
                    dr["一级模块ID"] = moduleid;
                    dr["一级模块"] = module;
                    dr["二级模块ID"] = dt.Rows[i]["ModuleID"].ToString();
                    dr["二级模块"] = dt.Rows[i]["ModuleName"].ToString();
                    dtList.Rows.Add(dr);
                }

            }

            return dtList;
        }

        /// <summary>
        /// 玩单词（目录信息）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetExcelSheetWordGame(DataTable dt)
        {
            DataTable dtList = new DataTable();
            dtList.TableName = "目录信息";
            //dtList.Columns.Add("书籍ID");
            //dtList.Columns.Add("书籍名称");
            dtList.Columns.Add("一级标题ID");
            dtList.Columns.Add("一级标题");
            dtList.Columns.Add("二级标题ID");
            dtList.Columns.Add("二级标题");
            dtList.Columns.Add("一级模块ID");
            dtList.Columns.Add("一级模块");
            dtList.Columns.Add("二级模块ID");
            dtList.Columns.Add("二级模块");
            dtList.Columns.Add("序号");
            dtList.Columns.Add("单词");
            dtList.Columns.Add("字母音频");
            dtList.Columns.Add("单词音频（女声）");
            dtList.Columns.Add("单词音频（和声）");
            dtList.Columns.Add("单词图片");
            dtList.Columns.Add("单词释义图片");
            dtList.Columns.Add("单词例句");
            dtList.Columns.Add("例句音频");

            var moduleid = "";
            var module = "";
            int count = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["SuperiorID"].ToString() == dt.Rows[i]["FirstTitileID"].ToString()
                   || dt.Rows[i]["SuperiorID"].ToString() == dt.Rows[i]["SecondTitleID"].ToString())
                {
                    moduleid = dt.Rows[i]["ModuleID"].ToString();
                    module = dt.Rows[i]["ModuleName"].ToString();
                }
                else
                {
                    DataRow dr = dtList.NewRow();
                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"].ToString();
                    dr["一级标题"] = dt.Rows[i]["FirstTitle"].ToString();
                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"].ToString();
                    dr["二级标题"] = dt.Rows[i]["SecondTitle"].ToString();
                    dr["一级模块ID"] = moduleid;
                    dr["一级模块"] = module;
                    dr["二级模块ID"] = dt.Rows[i]["ModuleID"].ToString();
                    dr["二级模块"] = dt.Rows[i]["ModuleName"].ToString();
                    dtList.Rows.Add(dr);
                }

            }

            return dtList;
        }

        /// <summary>
        /// 练习册
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetExcelExerciseBook(DataTable dt)
        {
            DataTable dtList = new DataTable();
            dtList.TableName = "目录信息";
            dtList.Columns.Add("一级标题ID");
            dtList.Columns.Add("一级标题");
            dtList.Columns.Add("二级标题ID");
            dtList.Columns.Add("二级标题");
            dtList.Columns.Add("页码");
            dtList.Columns.Add("二级模块");
            dtList.Columns.Add("三级模块");
            dtList.Columns.Add("音频");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[i]["SecondTitleID"].ToString()))
                {
                    DataRow dr = dtList.NewRow();
                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"].ToString();
                    dr["一级标题"] = dt.Rows[i]["FirstTitle"].ToString();
                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"].ToString();
                    dr["二级标题"] = dt.Rows[i]["SecondTitle"].ToString();
                    dtList.Rows.Add(dr);
                }
                else
                {
                    DataRow dr = dtList.NewRow();
                    dr["一级标题ID"] = dt.Rows[i]["FirstTitileID"].ToString();
                    dr["一级标题"] = dt.Rows[i]["FirstTitle"].ToString();
                    dtList.Rows.Add(dr);
                }
            }

            return dtList;
        }

        /// <summary>
        /// 生成excel并下载到本地
        /// </summary>
        /// <param name="dtList">第一个excel工作薄</param>
        /// <param name="dtSheet">第二个excel工作薄</param>
        /// <param name="name">excel名称</param>
        private static void WriteExcel(DataTable dtList, DataTable dtSheet, string name)
        {
            if (null != dtList && dtList.Rows.Count > 0)
            {
                HSSFWorkbook book = new HSSFWorkbook();
                ISheet sheet = book.CreateSheet(dtList.TableName);

                sheet.SetColumnWidth(1, 30 * 256);
                sheet.SetColumnWidth(3, 30 * 256);
                sheet.SetColumnWidth(5, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);

                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dtList.Columns.Count; i++)
                {
                    //row.CreateCell(i).SetCellValue(Convert.ToString(dt.Columns[i].ColumnName));
                    row.CreateCell(i).SetCellValue(dtList.Columns[i].ColumnName);
                }

                for (int i = 0; i < dtList.Rows.Count; i++)
                {
                    IRow row2 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dtList.Columns.Count; j++)
                    {
                        //row2.CreateCell(i).SetCellValue(Convert.ToString(dt.Rows[i][j]));
                        ICell cell = row2.CreateCell(j);
                        cell.SetCellValue(Convert.ToString(dtList.Rows[i][j]));
                        ICellStyle locked = book.CreateCellStyle();
                        locked.IsLocked = false; //设置该单元格是否锁定，true为锁定
                        //cell.CellStyle = locked;
                        ////保护表单，password为解锁密码
                        //sheet.ProtectSheet("password");
                        //if (j > 7)
                        //{
                        //    cell.CellStyle.IsLocked = false;
                        //}
                    }
                }

                if (dtSheet.TableName != "")
                {
                    ISheet sheet1 = book.CreateSheet(dtSheet.TableName);
                    IRow row1 = sheet1.CreateRow(0);

                    for (int i = 0; i < dtSheet.Columns.Count; i++)
                    {
                        row1.CreateCell(i).SetCellValue(dtSheet.Columns[i].ColumnName);
                    }
                    for (int i = 0; i < dtSheet.Rows.Count; i++)
                    {
                        IRow row3 = sheet1.CreateRow(i + 1);
                        for (int j = 0; j < dtSheet.Columns.Count; j++)
                        {
                            row3.CreateCell(j).SetCellValue(Convert.ToString(dtSheet.Rows[i][j]));
                        }
                    }
                }

                var context = HttpContext.Current;
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", context.Server.UrlEncode(name)));
                context.Response.Clear();

                MemoryStream file = new MemoryStream();
                book.Write(file);
                context.Response.BinaryWrite(file.GetBuffer());
                context.Response.End();

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ExportCatalog();
        }

        /// <summary>
        /// 下载目录页码模板
        /// </summary>
        /// <param name="BookId"></param>
        private void ExportCatalog()
        {
            BookName = GetBookName(Bookid);
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<ModuleData.Data> listS = new List<ModuleData.Data>();

            try
            {
                string sql = "";
                sql = @"SELECT DISTINCT(FirstTitileID),FirstTitle FROM dbo.TB_ModuleConfiguration WHERE BookID=" + Bookid + " AND State=0 ";
                DataSet dsList = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                sql = @"SELECT FirstTitileID,FirstTitle,SecondTitleID,SecondTitle FROM dbo.TB_ModuleConfiguration WHERE BookID=" + Bookid + " AND State=0 ";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //list = DataSetToIList<TB_ModuleConfiguration>(ds, 0);
                    ModuleData.Data[] bookdata = js.Deserialize<ModuleData.Data[]>(GetModuleToJson(DataSetToIList<TB_ModuleConfiguration>(ds, 0), DataSetToIList<TB_ModuleConfiguration>(dsList, 0)));
                    listS = new List<ModuleData.Data>(bookdata);
                }

                DataTable dtSheet = new DataTable();
                DataTable dtList = new DataTable();
                dtList.TableName = "目录信息";
                dtList.Columns.Add("一级标题ID");
                dtList.Columns.Add("一级标题");
                dtList.Columns.Add("二级标题ID");
                dtList.Columns.Add("二级标题");
                dtList.Columns.Add("起始页码");
                dtList.Columns.Add("终止页码");

                foreach (var item in listS)
                {
                    if (item.Children != null && item.Children.Length > 0)
                    {
                        foreach (var children in item.Children)
                        {
                            DataRow dr = dtList.NewRow();
                            dr["一级标题ID"] = item.Id.ToString();
                            dr["一级标题"] = item.Title;
                            dr["二级标题ID"] = children.Id.ToString();
                            dr["二级标题"] = children.Title;
                            dtList.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dtList.NewRow();
                        dr["一级标题ID"] = item.Id.ToString();
                        dr["一级标题"] = item.Title;
                        dtList.Rows.Add(dr);
                    }
                }
                string fileName = BookName + "" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WriteExcel(dtList, dtSheet, fileName);
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
        private static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
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
        /// 转换一级标题以及二级标题为json数据格式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private static string GetModuleToJson(List<TB_ModuleConfiguration> dt, List<TB_ModuleConfiguration> ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            if (dt.Count <= 0) return "";
            foreach (TB_ModuleConfiguration t in ds)
            {
                json.Append("{\"id\":\"" + t.FirstTitileID + "\",\"title\":\"" + String2Json(t.FirstTitle) + "\",\"children\":[");
                //json.Append("{\"id\":\"" + t.FirstTitileID + "\",\"ModularName\":\"" + String2Json(t.FirstTitle) + "\",\"state\":\"closed\",\"children\":[");
                foreach (var m in dt.Where(m => m.FirstTitileID == t.FirstTitileID && m.SecondTitleID != null && m.SecondTitleID != 0))
                {
                    //json.Append("{\"id\":\"" + m.SecondTitleID + "\",\"ModularName\":\"" + String2Json(m.SecondTitle) + "\",\"state\":\"closed\"},");
                    json.Append("{\"id\":\"" + m.SecondTitleID + "\",\"title\":\"" + String2Json(m.SecondTitle) + "\"},");
                }
                if (dt.Count(m => m.FirstTitileID == t.FirstTitileID && m.SecondTitleID != null && m.SecondTitleID != 0) <= 0)
                {
                    json.Append("[");
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(s)) return "";

            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '+':
                        sb.Append("\\n"); break;
                    //case 'null':
                    //    sb.Append("\\n"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
    }
}