using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Kingsun.SynchronousStudy.Common;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.SynchronousStudy.BLL.Management;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.Web.UserManagement
{
    public partial class UserStatistics : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        public string url = WebConfigurationManager.AppSettings["FZUUMS_Relation2"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVersion();

                DataBind();
            }
        }

        private void BindVersion()
        {
            string sql = @"SELECT [VersionID]
                                  ,[VersionName]
                              FROM [TB_APPManagement]";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            ddlVersion.DataSource = ds.Tables[0];
            ddlVersion.DataValueField = ds.Tables[0].Columns[0].ColumnName;
            ddlVersion.DataTextField = ds.Tables[0].Columns[1].ColumnName;
            ddlVersion.DataBind();
            ddlVersion.Items.Insert(0, new ListItem("请选择", "0"));
        }

        public void DataBind()
        {

            UserStatisticsBLL userStaBLL = new UserStatisticsBLL();

            string where = " 1=1 ";
            if (txtTelePhone.Text != "")
            {
                where += " AND b.TelePhone like '%" + txtTelePhone.Text + "%' ";
            }
            if (ddlVersion.SelectedValue != "0")
            {
                where += " AND c.VersionName like '%" + ddlVersion.SelectedItem.Text + "%' ";
            }
            if (txtUseTime.Text != "")
            {
                if (ddlTime.SelectedValue == "0")
                {
                    where += " AND a.UseTime >= '" + txtUseTime.Text + "' ";
                }
                else if (ddlTime.SelectedValue == "1")
                {
                    where += " AND a.UseTime >= '" + ParseInt(txtUseTime.Text) * 60 + "' ";
                }
                else if (ddlTime.SelectedValue == "2")
                {
                    where += " AND a.UseTime >= '" + ParseInt(txtUseTime.Text) * 3600 + "' ";
                }

            }
           
            if (txtStartDate.Value != "" && txtEndDate.Value != "")
            {
                where += "  AND b.CreateTime >= '" + txtStartDate.Value + "' AND b.CreateTime<='" + txtEndDate.Value + "' ";
            }
            else if (txtStartDate.Value != "")
            {
                where += "   AND b.CreateTime >= '" + txtStartDate.Value + "' ";
            }
            else if (txtEndDate.Value != "")
            {
                where += "  AND b.CreateTime<='" + txtEndDate.Value + "' ";
            }

            if (txtLoginStart.Value != "" && txtLoginEnd.Value != "")
            {
                where += "  AND a.LoginTime >= '" + txtLoginStart.Value + "' AND a.LoginTime<='" + txtLoginEnd.Value + "' ";
            }
            else if (txtStartDate.Value != "")
            {
                where += "   AND a.LoginTime >= '" + txtLoginStart.Value + "' ";
            }
            else if (txtEndDate.Value != "")
            {
                where += "  AND a.LoginTime<='" + txtLoginEnd.Value + "' ";
            }
            var list = userStaBLL.GetUserStatistic(where);

            List<UserStatisticsModel> reList=new List<UserStatisticsModel>();
            List<StudentCount> stucount=new List<StudentCount>();
            if (txtClassID.Text != "")
            {
                var classlist=classBLL.GetUserClassRelationByNum(txtClassID.Text, out stucount);
                reList = (from a in list
                              join b in classlist on a.UserID equals b.UserID into userid
                              from b in userid.DefaultIfEmpty()
                              select new UserStatisticsModel
                              {
                                  ID = a.ID,
                                  CreateTime = a.CreateTime,
                                  LoginTime = a.LoginTime,
                                  NickName = a.NickName,
                                  Number = a.Number,
                                  TelePhone = a.TelePhone,
                                  UserID = a.UserID,
                                  UserName = a.UserName,
                                  UseTime = a.UseTime,
                                  VersionName = a.VersionName,
                                  ClassShortID = b == null ? "" : b.ClassNum.ToString()
                              }).ToList<UserStatisticsModel>();
            }
            else
            {
               list.ForEach(a =>
               {
                   var classinfo = userBLL.GetClassInfoByUserID(a.UserID);
                   if (classinfo != null)
                   {
                       a.ClassShortID = classinfo.ClassNum;
                   }
                   else
                   {
                       a.ClassShortID = "";
                   }
               });
                reList = list;
            }
           

            int recordCount = 0;
            recordCount = reList.Count;
            reList = QueryByPage(AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, reList);

            AspNetPager1.RecordCount = recordCount;

            rpStatistics.DataSource = reList;
            rpStatistics.DataBind();
        }

        protected List<UserStatisticsModel> QueryByPage(int PageSize, int CurPage, IList<UserStatisticsModel> objs)
        {
            var query = from cms_contents in objs select cms_contents;
            return query.Take(PageSize * CurPage).Skip(PageSize * (CurPage - 1)).ToList();
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            DataBind();
        }


        public string TimeConversion(string ss)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(ss));
            string str = (ts.Days < 0 ? 0 : ts.Days) + "天" + (ts.Hours < 0 ? 0 : ts.Hours) + "时" + (ts.Minutes < 0 ? 0 : ts.Minutes) + "分" + (ts.Seconds < 0 ? 0 : ts.Seconds) + "秒";
            return str;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            AspNetPager1.CurrentPageIndex = 1;
            DataBind();
        }

        protected void rpStatistics_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Serach")
            {
                Response.Redirect("../UserManagement/UserStatisticsInfo.aspx?UserId=" + e.CommandArgument, true);
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

        public class UserClass
        {
            //public string IsPublic { get; set; }
            //public string CreateDate { get; set; }
            //public string IsRemove { get; set; }
            //public string ClassName { get; set; }
            //public string SchoolID { get; set; }
            //public string ID { get; set; }
            //public string GradeID { get; set; }
            //public string SubjectID { get; set; }
            public string ClassNum { get; set; }
            public string CreateID { get; set; }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            List<UserStatisticsModel> list = new List<UserStatisticsModel>();
            UserStatisticsBLL userStaBLL = new UserStatisticsBLL();
            List<StudentCount> stc = new List<StudentCount>();
            string sql = string.Format(@"SELECT a.ID ,
                                                a.UserID ,
                                                b.UserName ,
                                                b.NickName ,
                                                b.TelePhone ,
                                                c.VersionName ,
                                                b.CreateTime ,
                                                a.Number ,
                                                a.UseTime ,
                                                a.LoginTime ,
                                                d.ClassShortID ,
                                                ( SELECT    MAX(VersionNumber)
                                                  FROM      TB_ApplicationVersion
                                                  WHERE     VersionID = c.VersionID
                                                            AND State = 1
                                                ) AS VersionNumber
                                        FROM    TB_UserStatistics a
                                                LEFT JOIN ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b ON b.UserID = a.UserID
                                                LEFT JOIN dbo.TB_APPManagement c ON c.ID = a.AppID where b.IsUser=1;");
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "tishi", "<script type=\"text/javascript\">alert('数据不存在！');</script>");
                return;
            }   else 
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserStatisticsModel userSt = new UserStatisticsModel();
                    userSt.ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                    userSt.UserID = Convert.ToInt32(dt.Rows[i]["UserID"].ToString());
                    userSt.UserName = dt.Rows[i]["UserName"].ToString();
                    userSt.NickName = dt.Rows[i]["NickName"].ToString();
                    userSt.TelePhone = dt.Rows[i]["TelePhone"].ToString();
                    userSt.VersionName = dt.Rows[i]["VersionName"].ToString();
                    userSt.Number = Convert.ToInt32(dt.Rows[i]["Number"].ToString());
                    userSt.UseTime = Convert.ToInt32(dt.Rows[i]["UseTime"].ToString());
                    if (dt.Rows[i]["CreateTime"] is DBNull)
                    {
                        userSt.CreateTime = null;
                    }
                    else
                    {
                        userSt.CreateTime = DateTime.Parse(dt.Rows[i]["CreateTime"].ToString());
                    }
                    userSt.VersionNumber = Convert.ToInt32(dt.Rows[i]["VersionNumber"].ToString());
                    userSt.LoginTime = DateTime.Parse(dt.Rows[i]["LoginTime"].ToString());
                    list.Add(userSt);
                }
            }
            list.ForEach(a =>
            {
                var classinfo = userBLL.GetClassInfoByUserID(a.UserID);
                if (classinfo != null)
                {
                    a.ClassShortID = classinfo.ClassNum;
                }
                else
                {
                    a.ClassShortID = "";
                }
            });
            DataTable dtList = GetExcelSheetWordGame(list);
            string fileName = "用户信息" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            WriteExcel(dtList, fileName);
        }

        /// <summary>
        /// 玩单词（目录信息）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetExcelSheetWordGame(List<UserStatisticsModel> dt)
        {
            DataTable dtList = new DataTable();
            dtList.TableName = "用户信息";
            //dtList.Columns.Add("书籍ID");
            //dtList.Columns.Add("书籍名称");
            dtList.Columns.Add("昵称");
            dtList.Columns.Add("手机号");
            dtList.Columns.Add("产品版本");
            dtList.Columns.Add("App版本");
            dtList.Columns.Add("注册时间");
            dtList.Columns.Add("已使用天数");
            dtList.Columns.Add("使用时长");
            dtList.Columns.Add("加入的学校班级");
            dtList.Columns.Add("最后登陆时间");

            var moduleid = "";
            var module = "";
            int count = 0;

            dt.ForEach(a =>
            {
                DataRow dr = dtList.NewRow();
                dr["昵称"] = a.NickName;
                dr["手机号"] = a.TelePhone;
                dr["产品版本"] = a.VersionName;
                dr["App版本"] = a.VersionNumber;
                dr["注册时间"] = a.CreateTime;
                dr["已使用天数"] = a.Number;
                dr["使用时长"] = a.UseTime;
                dr["加入的学校班级"] = a.ClassShortID;
                dr["最后登陆时间"] = a.LoginTime;
                dtList.Rows.Add(dr);

            });

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
            dtList.TableName = "用户信息";
            //dtList.Columns.Add("书籍ID");
            //dtList.Columns.Add("书籍名称");
            dtList.Columns.Add("昵称");
            dtList.Columns.Add("手机号");
            dtList.Columns.Add("产品版本");
            dtList.Columns.Add("App版本");
            dtList.Columns.Add("注册时间");
            dtList.Columns.Add("已使用天数");
            dtList.Columns.Add("使用时长");
            dtList.Columns.Add("加入的学校班级");
            dtList.Columns.Add("最后登陆时间");

            var moduleid = "";
            var module = "";
            int count = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtList.NewRow();
                dr["昵称"] = dt.Rows[i]["NickName"].ToString();
                dr["手机号"] = dt.Rows[i]["TelePhone"].ToString();
                dr["产品版本"] = dt.Rows[i]["VersionName"].ToString();
                dr["App版本"] = dt.Rows[i]["VersionNumber"].ToString();
                dr["注册时间"] = dt.Rows[i]["CreateTime"].ToString();
                dr["已使用天数"] = dt.Rows[i]["Number"].ToString();
                dr["使用时长"] = TimeConversion(dt.Rows[i]["UseTime"].ToString());
                dr["加入的学校班级"] = dt.Rows[i]["ClassShortID"].ToString();
                dr["最后登陆时间"] = dt.Rows[i]["LoginTime"].ToString();
                dtList.Rows.Add(dr);

            }

            return dtList;
        }

        /// <summary>
        /// 生成excel并下载到本地
        /// </summary>
        /// <param name="dtList">第一个excel工作薄</param>
        /// <param name="dtSheet">第二个excel工作薄</param>
        /// <param name="name">excel名称</param>
        public static void WriteExcel(DataTable dtList, string name)
        {
            if (null != dtList && dtList.Rows.Count > 0)
            {
                HSSFWorkbook book = new HSSFWorkbook();
                ISheet sheet = book.CreateSheet(dtList.TableName);

                //sheet.SetColumnWidth(0, 30 * 256);
                //sheet.SetColumnWidth(3, 30 * 256);
                //sheet.SetColumnWidth(5, 30 * 256);
                //sheet.SetColumnWidth(7, 30 * 256);

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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sql = "SELECT UserID FROM ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo] where IsUser=1";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string userID = ds.Tables[0].Rows[0][0].ToString();
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userID));
                if (user != null) 
                {
                    user.ClassSchList.ForEach(a => 
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                    });
                }
            }
           
        }

    }
}