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

namespace Kingsun.SynchronousStudy.Web.ModuleManagement
{
    public partial class ModuleConfigList : System.Web.UI.Page
    {
        ModuleConfigImplement implete = new ModuleConfigImplement();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
                return;
            }
        }
        private void InitAction(string action)
        {
            switch (action)
            {
                case "querylist":
                    KingRequest request = new KingRequest();
                    request.Function = "QueryModuleConfigList";
                    string pageindex = Request.Form["page"];
                    string pagesize = Request.Form["rows"];
                    string where = "";
                    if (Request.QueryString["queryStr"] == null || Request.QueryString["queryStr"].ToString()=="0")
                    {
                        where = "1=1";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                    }
                    var obj = new { PageIndex = pageindex, PageSize = pagesize, Where = where };
                    request.Data = JsonHelper.EncodeJson(obj);
                    KingResponse response = implete.ProcessRequest(request);
                    if (response.Success)
                    {
                        Response.Write(JsonHelper.EncodeJson(response.Data));
                        Response.End();
                    }
                    else
                    {
                        Response.Write(response.ErrorMsg);
                        Response.End();
                    }
                    break;
                case "updatacatalog":
                    string bookid = Request.Form["BookID"];
                    string bookname = Request.Form["BookName"];
                    UpdateCatalog(bookid, bookname);
                    break;
                case "DeleteModule":
                default:
                    break;
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
    }
}