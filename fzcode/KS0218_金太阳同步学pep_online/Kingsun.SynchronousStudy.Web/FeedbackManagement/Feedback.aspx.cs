using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web.FeedbackManagement
{
    public partial class Feedback : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        public UserFeedBackBLL UserFeedBackBll = new UserFeedBackBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            switch (action)
            {
                case "queryfeedbacklist":
                    TB_UserFeedback userFeedback = new TB_UserFeedback();
                    int totalcount = 0;
                    IList<TB_UserFeedback> feedBacklist = new List<TB_UserFeedback>();
                    if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                    {
                        var obj1 = new { rows = feedBacklist, total = totalcount };
                        Response.Write(JsonHelper.EncodeJson(obj1));
                        Response.End();
                    }
                    int pageindex = int.Parse(Request.Form["page"]);
                    int pagesize = int.Parse(Request.Form["rows"]);
                    string where;
                    if (string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        where = "1=1 ORDER BY CreateDate DESC";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"];
                    }
                    feedBacklist = UserFeedBackBll.GetFeedBackList(where);
                    if (feedBacklist == null)
                    {
                        feedBacklist = new List<TB_UserFeedback>();
                    }
                    else
                    {
                        for (int i = 0; i < feedBacklist.Count; i++)
                        {
                            if (feedBacklist[i].FeedbackLevel == 0)
                            {
                                feedBacklist.Remove(feedBacklist[i]);
                                if (i != 0)
                                {
                                    i--;
                                }
                                else
                                {
                                    i = 0;
                                }
                            }
                        }
                        totalcount = feedBacklist.Count;
                        IList<TB_UserFeedback> removelist = new List<TB_UserFeedback>();
                        for (int i = 0; i < feedBacklist.Count; i++)
                        {
                            if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                            {
                                removelist.Add(feedBacklist[i]);
                            }
                        }
                        if (removelist.Count > 0)
                        {
                            for (int i = 0; i < removelist.Count; i++)
                            {
                                feedBacklist.Remove(removelist[i]);
                            }
                        }
                    }
                    var obj = new { rows = feedBacklist, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "update":
                    string feedBackId = Request.Form["FeedBackID"];
                    string feedbackLevel = Request.Form["FeedbackLevel"];
                    string problemDescription = Request.Form["ProblemDescription"];
                    where = " ID = '" + feedBackId + "'";
                    if (Utils.filterSql(problemDescription))
                    {
                        Response.Write("有SQL攻击嫌疑，请停止操作。");
                        Response.End();
                    }
                    feedBacklist = UserFeedBackBll.GetFeedBackByID(where);
                    userFeedback = feedBacklist[0];
                    userFeedback.FeedbackLevel = Convert.ToInt32(feedbackLevel);
                    userFeedback.ProblemDescription = problemDescription;
                    bool result = UserFeedBackBll.UpdateFeedBackInfo(userFeedback);
                    Response.Write(JsonHelper.EncodeJson(new { result = result }));
                    Response.End();
                    break;
                default:
                    break;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<BookInfo> listS = new List<BookInfo>();
            try
            {
                //string s = Service.GetBookData("", "", "", "", "").Trim();
                listS = js.Deserialize<List<BookInfo>>(userBLL.GetBookData("", "", "", "", "").Trim());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            if (listS.Count <= 0)
            {
                ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('数据不存在！');</script>");
            }

            DataTable dtList = new DataTable();
            dtList.TableName = "Sheet1";
            dtList.Columns.Add("书籍ID");
            dtList.Columns.Add("书籍名称");
            dtList.Columns.Add("资源地址");

            foreach (var item in listS)
            {
                DataRow dr = dtList.NewRow();
                dr["书籍ID"] = item.ID;
                dr["书籍名称"] = item.BooKName;
                dtList.Rows.Add(dr);
            }
            //string path = Server.MapPath("~/Upload/ResourcesExcel/");//"C:\\TeamCity\\buildAgent\\work\\88580d4c6fbacf3\\Kingsun.SynchronousStudy.Web\\Upload\\ResourcesExcel\\" + dt.Rows[0]["TeachingNaterialName"] + ".xls";
            string fileName = "书籍表" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            WriteExcel(dtList, fileName);
        }

        public static void WriteExcel(DataTable dt, string fileName)
        {
            if (null != dt && dt.Rows.Count > 0)
            {
                HSSFWorkbook book = new HSSFWorkbook();
                ISheet sheet = book.CreateSheet(dt.TableName);

                sheet.SetColumnWidth(1, 30 * 256);
                sheet.SetColumnWidth(3, 30 * 256);
                sheet.SetColumnWidth(5, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);
                sheet.SetColumnWidth(8, 60 * 256);
                sheet.SetColumnWidth(9, 40 * 256);

                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row2 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row2.CreateCell(j);
                        cell.SetCellValue(Convert.ToString(dt.Rows[i][j]));
                       // ICellStyle locked = book.CreateCellStyle();
                        //locked.IsLocked = true;//设置该单元格为锁定
                        //cell.CellStyle = locked;
                        //保护表单，password为解锁密码
                       // sheet.ProtectSheet("password");
                        //if (j > 7)
                        //{
                        //    cell.CellStyle.IsLocked = false;
                        //}
                    }
                }

                var context = HttpContext.Current;
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", context.Server.UrlEncode(fileName)));
                context.Response.Clear();

                MemoryStream file = new MemoryStream();
                book.Write(file);
                context.Response.BinaryWrite(file.GetBuffer());
                context.Response.End();
            }
        }


        public class BookInfo
        {
            public string ID { get; set; }
            public string BooKName { get; set; }
        }
    }
}