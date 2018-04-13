using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System.Text;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Kingsun.SynchronousStudy.Web.CourseManagement
{
    public partial class CourseDetails : Page
    {
        readonly ModuleConfigurationBLL _moduleConfigurationBll = new ModuleConfigurationBLL();
        readonly ModularSortBLL _modularSortBll = new ModularSortBLL();
        readonly VersionChangeBLL _versionChangeBll = new VersionChangeBLL();
        readonly CourseBLL _courseBll = new CourseBLL();

        public int BookId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["BookID"]))
            {
                BookId = Convert.ToInt32(Request.QueryString["BookID"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }

            if (!IsPostBack)
            {
                BindModular();
            }
        }

        private void BindModular()
        {
            string sql = @"SELECT DISTINCT
                                [ModuleID] ,
                                [ModuleName]
                        FROM    [TB_ModuleSort]
                        WHERE   SuperiorID = FirstTitleID
                                OR SuperiorID = SecondTitleID";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            ddlModuleName.DataSource = ds.Tables[0];
            ddlModuleName.DataValueField = ds.Tables[0].Columns[0].ColumnName;
            ddlModuleName.DataTextField = ds.Tables[0].Columns[1].ColumnName;
            ddlModuleName.DataBind();
            ddlModuleName.Items.Insert(0, new ListItem("请选择", "0"));
        }

        private void InitAction(string action)
        {
            switch (action)
            {
                case "getbooktitle":
                    int bookId = Convert.ToInt32(Request.QueryString["BookID"]);
                    string where;
                    where = "BookID = " + bookId + " AND State=0";
                    IList<TB_ModuleConfiguration> moduleList = _moduleConfigurationBll.GetModuleList(where);
                    StringBuilder json = new StringBuilder();
                    json.Append("[");
                    string firstTitle = "";
                    if (moduleList != null && moduleList.Count > 0)
                    {
                        foreach (var item in moduleList)
                        {
                            if (firstTitle.IndexOf(item.FirstTitle, StringComparison.Ordinal) == -1)
                            {
                                firstTitle += item.FirstTitle;
                                string itemFirstTitle = item.FirstTitle;
                                json.Append("{\"id\":\"" + item.FirstTitileID + "\",\"ModularName\":\"" + String2Json(item.FirstTitle) + "\",\"state\":\"closed\",\"children\":[");
                                if (item.SecondTitleID != null && item.SecondTitleID != 0)
                                {
                                    foreach (var module in moduleList)
                                    {
                                        if (module.FirstTitle == itemFirstTitle)
                                        {
                                            json.Append("{\"id\":\"" + module.SecondTitleID + "\",\"ModularName\":\"" + String2Json(module.SecondTitle) + "\",\"state\":\"closed\"},");
                                        }
                                    }
                                    json.Remove(json.Length - 1, 1);
                                    json.Append("],\"isNull\":\"false\"},");
                                }
                                else
                                {
                                    json.Append("[");
                                    json.Remove(json.Length - 1, 1);
                                    json.Append("],\"isNull\":\"true\"},");
                                }

                            }
                        }
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    Response.Write(json.ToString());
                    Response.End();
                    break;
                case "getbookname":
                    bookId = Convert.ToInt32(Request.Form["BookID"]);
                    where = "BookID = " + bookId + " and State = 1";
                    IList<TB_CurriculumManage> courseList = _courseBll.GetCourseByCondition(where);
                    TB_CurriculumManage courseInfo = new TB_CurriculumManage();

                    if (courseList != null && courseList.Count > 0)
                    {
                        courseInfo = courseList[0];
                    }
                    Response.Write(JsonHelper.EncodeJson(new { obj = courseInfo }));
                    Response.End();
                    break;
                case "getmodulardetails":
                    bookId = Convert.ToInt32(Request.QueryString["BookID"]);
                    int firstTitleId = Convert.ToInt32(Request.QueryString["firstTitleID"]);
                    int secondTitleId = Convert.ToInt32(Request.QueryString["secondTitleID"]);
                    var isnull = "false";
                    if (secondTitleId == 0)
                    {
                        where = "BookID = " + bookId + " and FirstTitleID = " + firstTitleId + " order by Sort ";
                        secondTitleId = firstTitleId;
                        isnull = "true";
                    }
                    else
                    {
                        where = "BookID = " + bookId + " and FirstTitleID = " + firstTitleId + " and SecondTitleID = " + secondTitleId + " order by Sort ";
                    }

                    IList<TB_ModuleSort> data = _modularSortBll.GetModuleList(where);
                    json = new StringBuilder();
                    int sum = 0;
                    int count = 0;
                    json.Append("[");
                    foreach (var item in data)
                    {
                        if (item.SuperiorID == secondTitleId)
                        {
                            json.Append("{\"id\":\"" + secondTitleId + "_" + item.ID + "\",\"ModularID\":" + item.ModuleID + ",\"ModularName\":\"" + String2Json(item.ModuleName));
                            json.Append("\",\"SuperiorID\":" + item.SuperiorID + ",\"sort\":\"" + count++ + "\",\"isNull\":\"" + isnull + "\",\"children\":[");
                            foreach (var m in data)
                            {
                                if (m.SuperiorID == item.ModuleID)
                                {
                                    json.Append("{\"id\":\"" + secondTitleId + "_" + m.ID + "\",\"ModularID\":" + m.ModuleID + ",\"ModularName\":\"" + String2Json(m.ModuleName));
                                    json.Append("\",\"SuperiorID\":\"" + m.SuperiorID + "\",\"isNull\":\"" + isnull + "\"},");
                                    sum++;
                                }
                            }
                            if (sum == 0)
                            {
                                json.Append("]");
                            }
                            else
                            {
                                json.Remove(json.Length - 1, 1);
                                json.Append("],\"state\":\"closed\"");
                            }
                            json.Append("},");
                        }
                        sum = 0;
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    Response.Write(json.ToString());
                    Response.End();
                    break;
                case "getversionnumber":
                    int modularId = Convert.ToInt32(Request.Form["ModularID"]);
                    firstTitleId = Convert.ToInt32(Request.Form["FirstTitleID"]);
                    secondTitleId = Convert.ToInt32(Request.Form["SecondTitleID"]);
                    where = " FirstTitleID = " + firstTitleId;
                    if (secondTitleId != 0)
                    {
                        where += " and SecondTitleID = " + secondTitleId;
                    }
                    where += " and ModuleID = " + modularId + " and State = 1 ORDER BY CreateDate DESC";
                    IList<TB_VersionChange> list = _versionChangeBll.GetModuleByID(where);
                    if (list != null && list.Count > 0)
                    {
                        TB_VersionChange moduleDetails = list[0];
                        Response.Write(JsonHelper.EncodeJson(new { obj = moduleDetails }));
                    }
                    else
                    {
                        Response.Write(JsonHelper.EncodeJson(null));
                    }
                    Response.End();
                    break;
                case "save":
                    string courseCover = Request.Form["CourseCover"];
                    string CreateDate = Request.Form["CreateDate"];
                    int BookID = Int32.Parse(Request.Form["BookID"]);
                    TB_CurriculumManage course = new TB_CurriculumManage();
                    where = "BookID = " + BookID + " and State = 1";
                    courseList = _courseBll.GetCourseByCondition(where);
                    if (courseList != null && courseList.Count > 0)
                    {
                        course = courseList[0];
                    }
                    if (courseCover!="")
                    {
                        course.CourseCover = courseCover;
                    } 
                    course.CreateDate = DateTime.Parse(CreateDate);
                    if (_courseBll.UpdateCourse(course))
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
                case "changestate":
                    modularId = Convert.ToInt32(Request.Form["ModularID"]);
                    firstTitleId = Convert.ToInt32(Request.Form["FirstTitleID"]);
                    secondTitleId = Convert.ToInt32(Request.Form["SecondTitleID"]);
                    where = " FirstTitleID = " + firstTitleId;
                    where += " and SecondTitleID = " + secondTitleId;
                    where += " and ModuleID = " + modularId + " and State = 1 ORDER BY CreateDate DESC";
                    list = _versionChangeBll.GetModuleByID(where);
                    if (list != null && list.Count > 0)
                    {
                        TB_VersionChange moduleDetails = list[0];
                        moduleDetails.State = false;
                        bool result = _versionChangeBll.UpdateModuleInfo(moduleDetails);
                        Response.Write(JsonHelper.EncodeJson(new { obj = result }));
                    }
                    else
                    {
                        Response.Write(JsonHelper.EncodeJson(new { obj = false }));
                    }
                    Response.End();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
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
                    //case '+':
                    //    sb.Append("\\n"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ddlModuleName.SelectedValue == "0")
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('请选择模块名称！');</script>");
                return;
            }
            string sql = string.Format(@" UPDATE dbo.TB_VersionChange SET ModuleAddress='{0}{1}'+ModuleAddress   WHERE BooKID IN ({2}) AND ModuleName = '{3}'", ddlAddress.SelectedItem.Text, txtFile.Text, BookId, ddlModuleName.SelectedItem.Text);
            int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            if (i > 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('修改成功！');</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('修改失败！');</script>");
            }
        }
    }
}