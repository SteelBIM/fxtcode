using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.SynchronousStudy.Web.GeneralTools
{
    public partial class ExportExcelTools : Page
    {
        public int BookId;
        public string BookName;
        protected void Page_Load(object sender, EventArgs e)
        {
            BookId = Convert.ToInt32(Request.QueryString["BookID"]);

            DataBind(BookId);
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
                        ICellStyle locked = book.CreateCellStyle();
                        locked.IsLocked = true;//设置该单元格为锁定
                        cell.CellStyle = locked;
                        //保护表单，password为解锁密码
                        sheet.ProtectSheet("password");
                        if (j > 7)
                        {
                            cell.CellStyle.IsLocked = false;
                        }
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

        protected void rpModule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "down")
            {
                string[] arg = e.CommandArgument.ToString().Split(',');
                string sql = string.Format(@"SELECT    ms.ModuleID ,
                                                        ms.ModuleName ,
                                                        ms.FirstTitleID ,
                                                        ms.SecondTitleID ,
                                                        mc.FirstTitle ,
                                                        mc.SecondTitle ,
                                                        mc.BookID ,
                                                        mc.TeachingNaterialName
                                              FROM      dbo.TB_ModuleSort AS ms
                                                        LEFT JOIN dbo.TB_ModuleConfiguration AS mc ON ms.BookID = mc.BookID
                                                      AND ( ms.SuperiorID = mc.SecondTitleID
                                                            OR ms.SuperiorID = mc.FirstTitileID
                                                          )
                                              WHERE     mc.BookID={0} AND ms.ModuleID = {1};", BookId, arg[1]);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql).Tables[0];

                if (dt.Rows.Count <= 0)
                {
                    ClientScript.RegisterStartupScript(GetType(), "tishi", "<script type=\"text/javascript\">alert('数据不存在！');</script>");
                }

                DataTable dtList = new DataTable();
                dtList.TableName = "Sheet1";
                dtList.Columns.Add("书籍ID");
                dtList.Columns.Add("书籍名称");
                dtList.Columns.Add("一级标题ID");
                dtList.Columns.Add("一级标题");
                dtList.Columns.Add("二级标题ID");
                dtList.Columns.Add("二级标题");
                dtList.Columns.Add("一级模块ID");
                dtList.Columns.Add("一级模块");
                dtList.Columns.Add("模块地址");
                dtList.Columns.Add("MD5值");

                var moduleid = "";
                var module = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtList.NewRow();
                    dr["书籍ID"] = dt.Rows[i]["BookID"].ToString();
                    dr["书籍名称"] = dt.Rows[i]["TeachingNaterialName"].ToString();
                    dr["一级标题ID"] = dt.Rows[i]["FirstTitleID"].ToString();
                    dr["一级标题"] = dt.Rows[i]["FirstTitle"].ToString();
                    dr["二级标题ID"] = dt.Rows[i]["SecondTitleID"].ToString();
                    dr["二级标题"] = dt.Rows[i]["SecondTitle"].ToString();
                    dr["一级模块ID"] = dt.Rows[i]["ModuleID"].ToString(); ;
                    dr["一级模块"] = dt.Rows[i]["ModuleName"].ToString();
                    dtList.Rows.Add(dr);
                }
                //string path = Server.MapPath("~/Upload/ResourcesExcel/");//"C:\\TeamCity\\buildAgent\\work\\88580d4c6fbacf3\\Kingsun.SynchronousStudy.Web\\Upload\\ResourcesExcel\\" + dt.Rows[0]["TeachingNaterialName"] + ".xls";
                string fileName = dt.Rows[0]["TeachingNaterialName"].ToString() + dt.Rows[0]["ModuleName"] + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
                WriteExcel(dtList, fileName);
            }
        }
    }
}