using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Kingsun.SynchronousStudy.Web.Handler
{
    /// <summary>
    /// ModuleData 的摘要说明
    /// </summary>
    public class ModuleData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string methodData = context.Request["methodData"] ?? "";
            switch (methodData)
            {
                case "SaveModule"://保存模块
                    SaveModule(context);
                    break;
                case "DeleteModule"://删除模块
                    DeleteModule(context);
                    break;
                case "AddModuleConfig"://保存模块配置
                    AddModuleConfig(context);
                    break;
                case "GetModModularList"://通过BookId获取mod数据库里教科书数据
                    GetModModularList(context);
                    break;
                case "GetModularList"://通过BookId获取mod数据库里教科书数据
                    GetModularList(context);
                    break;
                case "ModularToJson"://获取模块信息并返回Json格式的数据
                    ModularToJson(context);
                    break;
                case "GetModuleSort"://根据书籍单元获取模块排序信息
                    GetModuleSort(context);
                    break;
                case "UpdateModuleSort"://更新模块排序
                    UpdateModuleSort(context);
                    break;
                case "GetBookTitle":
                    GetBookTitle(context); //获取书本一二级标题
                    break;
                case "GetCompletedModule":
                    GetCompletedModule(context); //获取书本配置完成模块列表
                    break;
                case "GetUserStatistics":
                    GetUserStatistics(context); //
                    break;

                default:
                    context.Response.Write("{\"Result\":\"false\",\"msg\":\"\",\"data\":\"\"}");
                    break;
            }
        }


        /// <summary>
        /// 获取GetUserStatistics表数据
        /// </summary>
        /// <param name="context"></param>
        private void GetUserStatistics(HttpContext context)
        {
            string strResult = "";
            string sortname = context.Request["sortname"] ?? "";
            string sortvalue = context.Request["sortvalue"] ?? "";

            if (string.IsNullOrEmpty(sortname))
            {
                sortname = "UserName";
            }
            if (string.IsNullOrEmpty(sortvalue))
            {
                sortvalue = "ASC";
            }


            string sql = string.Format(@"SELECT a.*,b.BookID,c.TeachingNaterialName FROM TB_UserStatistics a LEFT JOIN ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b ON a.UserID=b.UserID LEFT JOIN dbo.TB_CurriculumManage c ON b.BookID=c.BookID where  AND IsUser=1 ORDER BY {0} {1}", sortname, sortvalue);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            List<UserStatistics> user = new List<UserStatistics>();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                user = DataSetToIList<UserStatistics>(ds, 0);
            }

            if (ds != null)
            {
                var obj = new { rows = user, total = ds.Tables[0].Rows.Count };

                context.Response.Write(JsonHelper.EncodeJson(obj));
            }
            context.Response.End();
        }


        /// <summary>
        /// 保存模块
        /// </summary>
        /// <param name="context"></param>
        private void SaveModule(HttpContext context)
        {
            string strResult;
            string modularName = context.Request["ModularName"] ?? "";//子节点名称
            string superiorId = context.Request["SuperiorID"] ?? "";//父节点id
            string parentId = context.Request["ParentID"] ?? "";
            string level = context.Request["Level"] ?? "";//父节点位置 
            string rootLevel = context.Request["rootLevel"] ?? "";//子节点位置
            string parentModularId = context.Request["ParentModularID"] ?? "";
            string parentName = context.Request["ParentName"] ?? "";//父节点名称
            string modularId = context.Request["ModularID"] ?? "";//子节点id
            int iEnquiryStatus;
            if (string.IsNullOrEmpty(modularName) || string.IsNullOrEmpty(superiorId))
            {
                strResult = "{\"Result\":\"false\",\"msg\":\"参数传递错误\",\"data\":\"\"}";
            }
            else if (int.TryParse(modularId, out iEnquiryStatus))
            {
                string sql;
                string strSql;
                if (int.TryParse(parentId, out iEnquiryStatus))
                {
                    //查询父节点是否存在于数据库
                    strSql = "SELECT COUNT(1) FROM [TB_ModularManage] WHERE ModularID=" + parentModularId + " And State=1";
                    if (Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strSql)) > 0)
                    {
                        strSql = "SELECT id FROM [TB_ModularManage] WHERE  State=1 AND ModularID=" + modularId;
                        int d = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strSql));
                        if (d > 0)
                        {
                            sql =
                                string.Format("UPDATE TB_ModuleSort SET ModuleName ='{0}' WHERE ModuleID={4};" +
                                              "UPDATE [TB_ModularManage] SET ModularID={4},[ModularName] ='{0}',[SuperiorID] ={1}  ,[State] ={2},Level='{5}' WHERE ID={3} ",
                                    modularName, superiorId, 1, d, modularId, level);
                        }
                        else
                        {
                            sql =
                                string.Format(
                                    " INSERT INTO dbo.TB_ModularManage (ModularID,ModularName ,SuperiorID ,State,Level)VALUES  ( '{0}' , '{1}' , {2},1,'{3}')",
                                    modularId, modularName, Convert.ToInt32(superiorId), level);
                        }
                    }
                    else
                    {
                        sql = string.Format(" INSERT INTO dbo.TB_ModularManage ( ModularID,ModularName ,SuperiorID ,State,Level)VALUES  ( '{0}' , '{1}' , {2},1 ,'{6}');" +
                                            "INSERT INTO dbo.TB_ModularManage ( ModularID,ModularName ,SuperiorID ,State,Level)VALUES  ( '{3}' , '{4}',{5},1, '{7}')",
                                            parentModularId, parentName, parentId, modularId, modularName, Convert.ToInt32(superiorId), rootLevel, level);
                    }
                }
                else
                {
                    strSql = "SELECT id FROM [TB_ModularManage] WHERE ModularID=" + modularId + " AND State=1";
                    int d = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, strSql));
                    if (d > 0)
                    {
                        sql = string.Format("UPDATE TB_ModuleSort SET ModuleName ='{0}' WHERE ModuleID={4};" +
                                            "UPDATE [TB_ModularManage] SET ModularID={4},[ModularName] ='{0}',[SuperiorID] ={1},[State] ={2},Level='{5}' WHERE ID={3} ", modularName, superiorId, 1, d, modularId, level);
                    }
                    else
                    {
                        sql = string.Format(" INSERT INTO dbo.TB_ModularManage (ModularID,ModularName ,SuperiorID ,State,Level)VALUES  ( '{0}' , '{1}' , {2},{3},'{4}')", modularId, modularName, Convert.ToInt32(superiorId), 1, level);
                    }
                }


                int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                strResult = i > 0 ? "{\"Result\":\"true\",\"msg\":\"修改成功\",\"data\":\"\"}" : "{\"Result\":\"false\",\"msg\":\"修改失败，请重试\",\"data\":\"\"}";
            }
            else
            {
                strResult = "{\"Result\":\"false\",\"msg\":\"参数传递错误\",\"data\":\"\"}";
            }
            context.Response.Write(strResult);
            context.Response.End();
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="context"></param>
        private void DeleteModule(HttpContext context)
        {
            string strResult;
            string modularId = context.Request["Mid"] ?? "";
            int iEnquiryStatus;

            if (string.IsNullOrEmpty(modularId))
            {
            }
            if (int.TryParse(modularId, out iEnquiryStatus))
            {
                string sql = string.Format("SELECT COUNT(1) FROM  TB_ModuleSort WHERE ModuleID={0}", modularId);
                if (Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
                {
                    strResult = "{\"Result\":\"false\",\"msg\":\"删除失败，失败原因：该模块已在教材中使用！\",\"data\":\"\"}";
                }
                else
                {
                    sql = string.Format("SELECT COUNT(1) FROM dbo.TB_ModularManage WHERE ModularID={0}", modularId);
                    if (Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
                    {
                        string strSql = string.Format("UPDATE [TB_ModularManage] SET [State] =0 WHERE ModularID={0}",
                            modularId);

                        int i = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);
                        strResult = i > 0 ? "{\"Result\":\"true\",\"msg\":\"删除成功\",\"data\":\"\"}" : "{\"Result\":\"false\",\"msg\":\"删除失败\",\"data\":\"\"}";
                    }
                    else
                    {
                        strResult = "{\"Result\":\"false\",\"msg\":\"删除失败，失败原因：数据不存在！\",\"data\":\"\"}";
                    }
                }

            }
            else
            {
                strResult = "{\"Result\":\"false\",\"msg\":\"参数传递错误\",\"data\":\"\"}";
            }
            context.Response.Write(strResult);
            context.Response.End();
        }


        /// <summary>
        /// 通过BookId获取mod数据库里教科书数据
        /// </summary>
        /// <param name="context"></param>
        private void GetModModularList(HttpContext context)
        {
            string bookid = context.Request["bookid"] ?? "";
            string modUrl = context.Request["ModUrl"] ?? "";

            JavaScriptSerializer js = new JavaScriptSerializer();
            StreamReader responseReader = null;
            List<Data> listS = new List<Data>();
            try
            {
                //ashx Url
                // string getGscUserUrl = "http://183.47.42.221:8027/GetCatalogByBookId.sun";
                //加入参数，用于更新请求
                string urlHandler = modUrl + "GetCatalogByBookId.sun?t[BookId]=" + bookid;
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlHandler);
                webRequest.Timeout = 3000;//3秒超时
                //调用ashx，并取值
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string currentUserGulid = responseReader.ReadToEnd();

                Data[] bookdata = js.Deserialize<Data[]>(currentUserGulid.Trim());
                listS = new List<Data>(bookdata);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex);
                context.Response.End();
            }
            finally
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                }
            }
            context.Response.Write(ModToJson(listS));
            context.Response.End();
        }

        /// <summary>
        /// 通过BookId获取mod数据库里教科书数据
        /// </summary>
        /// <param name="context"></param>
        private void GetModularList(HttpContext context)
        {
            string bookid = context.Request["bookid"] ?? "";
            string modUrl = context.Request["ModUrl"] ?? "";

            JavaScriptSerializer js = new JavaScriptSerializer();
            StreamReader responseReader = null;
            List<Data> listS = new List<Data>();

            string sql = @"SELECT COUNT(1) FROM dbo.TB_ModuleSort WHERE BookID=" + bookid;
            if (Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0 && !string.IsNullOrEmpty(bookid))
            {
                sql = @"SELECT DISTINCT(FirstTitileID),FirstTitle FROM dbo.TB_ModuleConfiguration WHERE BookID=" + bookid + " AND State=0 ";
                DataSet dsList = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                sql = @"SELECT FirstTitileID,FirstTitle,SecondTitleID,SecondTitle FROM dbo.TB_ModuleConfiguration WHERE BookID=" + bookid + " AND State=0 ";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //list = DataSetToIList<TB_ModuleConfiguration>(ds, 0);
                    Data[] bookdata = js.Deserialize<Data[]>(GetModuleToJson(DataSetToIList<TB_ModuleConfiguration>(ds, 0), DataSetToIList<TB_ModuleConfiguration>(dsList, 0)));
                    listS = new List<Data>(bookdata);
                }

            }
            else
            {
                try
                {
                    ////ashx Url
                    //// string getGscUserUrl = "http://183.47.42.221:8027/GetCatalogByBookId.sun";
                    ////加入参数，用于更新请求
                    //string urlHandler = modUrl + "GetCatalogByBookId.sun?t[BookId]=" + bookid;
                    //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlHandler);
                    //webRequest.Timeout = 3000;//3秒超时
                    ////调用ashx，并取值
                    //responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                    //string currentUserGulid = responseReader.ReadToEnd();

                    //Data[] bookdata = js.Deserialize<Data[]>(currentUserGulid.Trim());
                    //listS = new List<Data>(bookdata);
                    sql = @"SELECT DISTINCT(FirstTitileID),FirstTitle FROM dbo.TB_ModuleConfiguration WHERE BookID=" + bookid + " AND State=0 ";
                    DataSet dsList = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    sql = @"SELECT FirstTitileID,FirstTitle,SecondTitleID,SecondTitle FROM dbo.TB_ModuleConfiguration WHERE BookID=" + bookid + " AND State=0 ";
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        //list = DataSetToIList<TB_ModuleConfiguration>(ds, 0);
                        Data[] bookdata = js.Deserialize<Data[]>(GetModuleToJson(DataSetToIList<TB_ModuleConfiguration>(ds, 0), DataSetToIList<TB_ModuleConfiguration>(dsList, 0)));
                        listS = new List<Data>(bookdata);
                    }


                }
                catch (Exception ex)
                {
                    context.Response.Write(ex);
                    context.Response.End();
                }
                finally
                {
                    if (responseReader != null)
                    {
                        responseReader.Close();
                        responseReader.Dispose();
                    }
                }
            }

            //string ssc = ToJson(listS);
            context.Response.Write(ToJson(listS));
            context.Response.End();
        }

        /// <summary>
        /// 获取模块信息并返回Json格式的数据
        /// </summary>
        /// <param name="context"></param>
        public void ModularToJson(HttpContext context)
        {
            string parentid = context.Request["parentid"] ?? "";
            string isnull = context.Request["isnull"] ?? "";

            ModularManageBLL modularManageBll = new ModularManageBLL();
            List<TB_ModularManage> data = modularManageBll.GetModularList();
            StringBuilder json = new StringBuilder();
            int sum = 0;
            int count = 0;
            json.Append("[");
            foreach (var item in data)
            {
                if (item.SuperiorID == 0)
                {
                    json.Append("{\"id\":\"" + parentid + "_" + item.ID + "\",\"ModularID\":" + item.ModularID + ",\"ModularName\":\"" + String2Json(item.ModularName));
                    json.Append("\",\"SuperiorID\":" + item.SuperiorID + ",\"sort\":\"" + count++ + "\",\"Level\":\"" + item.Level + "\",\"children\":[");
                    foreach (var m in data.Where(m => m.SuperiorID == item.ModularID))
                    {
                        json.Append("{\"id\":\"" + parentid + "_" + m.ID + "\",\"ModularID\":" + m.ModularID + ",\"ModularName\":\"" + String2Json(m.ModularName));
                        json.Append("\",\"SuperiorID\":" + m.SuperiorID + ",\"Level\":\"" + m.Level + "\",\"isNull\":\"" + isnull + "\"},");
                        sum++;
                    }
                    if (sum == 0)
                    {
                        json.Append("],\"isNull\":\"" + isnull + "\"");
                    }
                    else
                    {
                        json.Remove(json.Length - 1, 1);
                        json.Append("],\"state\":\"closed\",\"isNull\":\"" + isnull + "\"");
                    }
                    json.Append("},");
                }
                sum = 0;
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            context.Response.Write(json.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 将从Mod获取的数据转换一级标题以及二级标题为json数据格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(List<Data> dt)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            if (dt.Count <= 0) return "";
            foreach (Data t in dt)
            {
                json.Append("{\"id\":\"" + t.Id + "\",\"ModularName\":\"" + String2Json(t.Title) + "\",\"state\":\"closed\"");
                if (t.Children != null && t.Children.Length > 0)
                {
                    json.Append(",\"children\":[");
                    foreach (Children t1 in t.Children)
                    {
                        json.Append("{\"id\":\"" + t1.Id + "\",\"ModularName\":\"" + String2Json(t1.Title) + "\",\"state\":\"closed\",\"isNull\":\"false\"},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("],\"isNull\":\"false\"},");
                }
                else
                {
                    json.Append(",\"isNull\":\"true\"},");
                }
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }


        // <summary>
        /// 将从Mod获取的数据转换一级标题以及二级标题为json数据格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ModToJson(List<Data> dt)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            if (dt.Count <= 0) return "";
            foreach (Data t in dt)
            {
                if (t.Children == null)
                    json.Append("{\"id\":\"" + t.Id + "\",\"ModularName\":\"" + String2Json(t.Title) + "\"");
                else 
                json.Append("{\"id\":\"" + t.Id + "\",\"ModularName\":\"" + String2Json(t.Title) + "\",\"state\":\"closed\"");
                if (t.Children != null && t.Children.Length > 0)
                {
                    json.Append(",\"children\":[");
                    foreach (Children t1 in t.Children)
                    {
                        json.Append("{\"id\":\"" + t1.Id + "\",\"ModularName\":\"" + String2Json(t1.Title) + "\",\"isNull\":\"false\"},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("],\"isNull\":\"false\"},");
                }
                else
                {
                    json.Append(",\"isNull\":\"true\"},");
                }
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }

        /// <summary>
        /// 转换一级标题以及二级标题为json数据格式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string GetModuleToJson(List<TB_ModuleConfiguration> dt, List<TB_ModuleConfiguration> ds)
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
        /// 添加配置模块
        /// </summary>
        /// <param name="context"></param>
        private void AddModuleConfig(HttpContext context)
        {
            string strResult;
            string teachingNaterialName = context.Request["TeachingNaterialName"] ?? "";
            string module = context.Request["module"] ?? "";
            string bookid = context.Request["BookId"] ?? "";
            string type = context.Request["type"] ?? "";
            string modUrl = WebConfigurationManager.AppSettings["modUrl"];

            JavaScriptSerializer js = new JavaScriptSerializer();
            Module md = new Module();
            List<Module> team = js.Deserialize<List<Module>>(module);
            StreamReader responseReader = null;
            //team.Add(new Module("1","1","1","1","1","1","1","1"));
            List<Data> listS = new List<Data>();
            try
            {
                //ashx Url
                // string getGscUserUrl = "http://183.47.42.221:8027/GetCatalogByBookId.sun";
                //加入参数，用于更新请求
                string urlHandler = modUrl + "GETALLCATALOGBYBOOKID.sun?t[BookId]=" + bookid;
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlHandler);
                webRequest.Timeout = 3000;//3秒超时
                //调用ashx，并取值
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string currentUserGulid = responseReader.ReadToEnd();

                Data[] bookdata = js.Deserialize<Data[]>(currentUserGulid.Trim());
                listS = new List<Data>(bookdata);
                foreach (var item in listS)
                {
                    if (item.Children != null)
                    {
                        if (item.isRemove == 1)
                        {
                            md = new Module
                            {
                                Id = item.Id.ToString(),
                                Modularname = item.Title,
                                State = item.isRemove
                            };
                            team.Add(md);
                        }
                        foreach (var sc in item.Children.Where(s => s.isRemove == 1))
                        {
                            md = new Module
                            {
                                Id = sc.Id.ToString(),
                                Modularname = sc.Title,
                                State = sc.isRemove
                            };
                            team.Add(md);
                        }
                    }
                    else if (item.Children == null && item.isRemove == 1)
                    {
                        md = new Module
                        {
                            Id = item.Id.ToString(),
                            Modularname = item.Title,
                            State = item.isRemove
                        };
                        team.Add(md);
                    }

                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex);
                context.Response.End();
            }
            finally
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("BookID");
            dt.Columns.Add("TeachingNaterialName");
            dt.Columns.Add("FirstTitileID");
            dt.Columns.Add("FirstTitle");
            dt.Columns.Add("SecondTitleID");
            dt.Columns.Add("SecondTitle");
            dt.Columns.Add("CreateDate");
            dt.Columns.Add("State");

            DataTable dtList = new DataTable();
            dtList.Columns.Add("ID");
            dtList.Columns.Add("ModuleID");
            dtList.Columns.Add("ModuleName");
            dtList.Columns.Add("SuperiorID");
            dtList.Columns.Add("Sort");
            dtList.Columns.Add("BookID");
            dtList.Columns.Add("FirstTitleID");
            dtList.Columns.Add("SecondTitleID");

            int first = 0;
            string firstname = "";
            int second = 0;
            foreach (var info in team)
            {
                DataRow dr = dt.NewRow();
                DataRow drlist = dtList.NewRow();

                if (info.Level == "1" && info.isNull == "false")
                {
                    first = Convert.ToInt32(info.Id);
                    firstname = info.Modularname;
                }
                else if (info.Level == "1" && info.isNull == "true")
                {
                    first = Convert.ToInt32(info.Id);
                    firstname = info.Modularname;
                    dr["BookID"] = bookid;
                    dr["TeachingNaterialName"] = teachingNaterialName;
                    dr["FirstTitileID"] = first;
                    dr["FirstTitle"] = firstname;
                    dr["CreateDate"] = DateTime.Now;
                    dr["State"] = info.State;
                    dt.Rows.Add(dr);
                }
                else if (info.Level == "2" && info.isNull == "false")
                {
                    second = Convert.ToInt32(info.Id);
                    dr["BookID"] = bookid;
                    dr["TeachingNaterialName"] = teachingNaterialName;
                    dr["FirstTitileID"] = first;
                    dr["FirstTitle"] = firstname;
                    dr["SecondTitleID"] = info.Id;
                    dr["SecondTitle"] = info.Modularname;
                    dr["CreateDate"] = DateTime.Now;
                    dr["State"] = info.State;
                    dt.Rows.Add(dr);
                }
                else if (info.Level == "2" && info.isNull == "true")
                {
                    second = 0;
                    drlist["ModuleID"] = info.Mouduleid;
                    drlist["ModuleName"] = info.Modularname;
                    drlist["SuperiorID"] = info.Parentid;
                    drlist["Sort"] = info.Sortid == "undefined" ? "0" : info.Sortid;
                    drlist["BookID"] = bookid;
                    drlist["FirstTitleID"] = first;
                    dtList.Rows.Add(drlist);
                }
                else if (info.Level == "3" || info.Level == "4")
                {
                    drlist["ModuleID"] = info.Mouduleid;
                    drlist["ModuleName"] = info.Modularname;
                    drlist["SuperiorID"] = info.Parentid;
                    drlist["Sort"] = info.Sortid == "undefined" ? "0" : info.Sortid;
                    drlist["BookID"] = bookid;
                    drlist["FirstTitleID"] = first;
                    drlist["SecondTitleID"] = second;
                    dtList.Rows.Add(drlist);
                }
                else if (info.State == 1)
                {
                    first = Convert.ToInt32(info.Id);
                    firstname = info.Modularname;
                    dr["BookID"] = bookid;
                    dr["TeachingNaterialName"] = teachingNaterialName;
                    dr["FirstTitileID"] = first;
                    dr["FirstTitle"] = firstname;
                    dr["CreateDate"] = DateTime.Now;
                    dr["State"] = info.State;
                    dt.Rows.Add(dr);
                }
                //first = 0;
                //second = 0;
            }

            SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };
            SqlBulkCopy sbc1 = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dtList.Rows.Count,
            };

            string strSql = "DELETE FROM dbo.[TB_ModuleSort] WHERE [BookID]=" + Convert.ToInt32(bookid);
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);
            if (type == "saveModule")
            {
                strSql = "DELETE FROM dbo.[TB_ModuleConfiguration] WHERE [BookID]=" + Convert.ToInt32(bookid);
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);
            }

            try
            {
                if (type == "saveModule")
                {
                    sbc.DestinationTableName = "TB_ModuleConfiguration";
                    sbc.WriteToServer(dt);
                }
                sbc1.DestinationTableName = "TB_ModuleSort";
                sbc1.WriteToServer(dtList);
            }
            catch (Exception ex)
            {
                strResult = "{\"Result\":\"false\",\"msg\":\"参数传递错误:" + ex.Message + "\",\"data\":\"\"}";
            }
            if (sbc.NotifyAfter <= 0 || sbc1.NotifyAfter <= 0)
            {
                strResult = "{\"Result\":\"true\",\"msg\":\"插入" + sbc.NotifyAfter + "条数据\",\"data\":\"\"}";
            }
            else
            {
                strResult = "{\"Result\":\"true\",\"msg\":\"保存成功\",\"data\":\"\"}";
            }


            context.Response.Write(strResult);
            context.Response.End();
        }

        /// <summary>
        /// 获取单元下模块排序信息
        /// </summary>
        /// <param name="context"></param>
        private void GetModuleSort(HttpContext context)
        {
            string bookid = context.Request["bookid"] ?? "";
            string firstid = context.Request["firstid"] ?? "";
            string secondid = context.Request["secondid"] ?? "";

            string where = " 1=1 ";

            if (firstid == "undefined")
            {
                where += string.Format(" AND BookID = '{0}' AND FirstTitleID = '{1}' AND SuperiorID='{1}' ORDER BY Sort ", bookid, secondid);
            }
            else
            {
                where += string.Format(" AND BookID={0} AND FirstTitleID={1} AND SecondTitleID={2} AND SuperiorID={1} or SuperiorID={2} ORDER BY Sort ", bookid, firstid, secondid);
            }

            string sql = string.Format(@"SELECT [ID],[ModuleID],[ModuleName],[SuperiorID],[Sort],[BookID],[FirstTitleID],[SecondTitleID]
                                         FROM [TB_ModuleSort] WHERE {0} ", where);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

            StringBuilder json = new StringBuilder();
            json.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                json.Append("{\"ID\":\"" + ds.Tables[0].Rows[i]["ID"] + "\",\"ModuleID\":\"" + ds.Tables[0].Rows[i]["ModuleID"] + "\",\"ModuleName\":\"" + String2Json(ds.Tables[0].Rows[i]["ModuleName"].ToString()));
                json.Append("\",\"SuperiorID\":\"" + ds.Tables[0].Rows[i]["SuperiorID"] + "\",\"Sort\":\"" + ds.Tables[0].Rows[i]["Sort"] + "\",\"BookID\":\"" + ds.Tables[0].Rows[i]["BookID"]);
                json.Append("\",\"FirstTitleID\":\"" + ds.Tables[0].Rows[i]["FirstTitleID"] + "\",\"SecondTitleID\":\"" + ds.Tables[0].Rows[i]["SecondTitleID"] + "\",\"count\":\"" + ds.Tables[0].Rows.Count + "\"},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            context.Response.Write(json);
            context.Response.End();
        }

        /// <summary>
        /// 批量更新模块排序
        /// </summary>
        /// <param name="context"></param>
        private void UpdateModuleSort(HttpContext context)
        {
            string strResult;
            string data = context.Request["data"] ?? "";
            string firstid = context.Request["firstid"] ?? "0";
            string secondid = context.Request["secondid"];
            string bookid = context.Request["bookid"] ?? "0";

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<TB_ModuleSort> team = js.Deserialize<List<TB_ModuleSort>>(data);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("ModuleName");
            dt.Columns.Add("SuperiorID");
            dt.Columns.Add("Sort");
            dt.Columns.Add("BookID");
            dt.Columns.Add("FirstTitleID");
            dt.Columns.Add("SecondTitleID");

            foreach (var info in team)
            {
                DataRow drlist = dt.NewRow();
                drlist["ID"] = info.ID;
                drlist["ModuleID"] = info.ModuleID;
                drlist["ModuleName"] = info.ModuleName;
                drlist["SuperiorID"] = info.SuperiorID;
                drlist["Sort"] = info.Sort;
                drlist["BookID"] = bookid;
                drlist["FirstTitleID"] = firstid;
                drlist["SecondTitleID"] = secondid == "" ? null : secondid;
                dt.Rows.Add(drlist);
            }

            SqlConnection sqlCon = new SqlConnection(SqlHelper.ConnectionString);
            sqlCon.Open();
            SqlTransaction sqlTran = sqlCon.BeginTransaction();
            SqlBulkCopy sbc = new SqlBulkCopy(sqlCon, SqlBulkCopyOptions.Default, sqlTran)
            {
                BulkCopyTimeout = 5000,
                NotifyAfter = dt.Rows.Count,
            };
            string strSql = string.Empty;
            if (secondid == "")
            {
                strSql = string.Format(@"DELETE FROM dbo.[TB_ModuleSort] WHERE BookID='{0}' AND FirstTitleID='{1}'
                                          AND SuperiorID='{1}'", Convert.ToInt32(bookid), Convert.ToInt32(firstid));
            }
            else
            {
                strSql = string.Format(@"DELETE FROM dbo.[TB_ModuleSort] WHERE BookID={0} AND FirstTitleID={1}
                                         AND SecondTitleID={2} AND SuperiorID={2}", Convert.ToInt32(bookid), Convert.ToInt32(firstid), Convert.ToInt32(secondid));
            }

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

            try
            {
                sbc.DestinationTableName = "TB_ModuleSort";
                sbc.WriteToServer(dt); //此处报错
                sqlTran.Commit();

            }
            catch (Exception)
            {
                sqlTran.Rollback();
                throw;
            }
            finally
            {
                sbc.Close();
                sqlCon.Close();
            }

            if (sbc.NotifyAfter <= 0)
            {
                strResult = "{\"Result\":\"false\",\"msg\":\"保存失败\",\"data\":\"\"}";
            }
            else
            {
                strResult = "{\"Result\":\"true\",\"msg\":\"保存成功\",\"data\":\"\"}";
            }

            context.Response.Write(strResult);
            context.Response.End();

        }

        /// <summary>
        /// 获取书本一二级模块目录
        /// </summary>
        /// <param name="context"></param>
        private void GetBookTitle(HttpContext context)
        {
            ModuleConfigurationBLL moduleConfigurationBll = new ModuleConfigurationBLL();
            string bookid = context.Request["BookID"] ?? "";
           // string type = context.Request["type"] ?? "";
            string where = "BookID = " + bookid + " AND State = 0 ";
            IList<TB_ModuleConfiguration> moduleList = moduleConfigurationBll.GetModuleList(where);
            if (moduleList != null)
            {
                StringBuilder json = new StringBuilder();
                json.Append("[");
                string firstTitle = "";
                foreach (var item in moduleList)
                {
                    if (firstTitle.IndexOf(item.FirstTitle, StringComparison.Ordinal) == -1)
                    {
                        firstTitle += item.FirstTitle;
                        string itemFirstTitle = item.FirstTitle;
                        //if (item.SecondTitleID == null)
                        //    json.Append("{\"id\":\"" + item.FirstTitileID + "\",\"ModularName\":\"" + String2Json(item.FirstTitle) + "\",\"children\":[");
                        //else 
                        json.Append("{\"id\":\"" + item.FirstTitileID + "\",\"ModularName\":\"" + String2Json(item.FirstTitle) + "\",\"state\":\"closed\",\"children\":[");

                        
                        if (item.SecondTitleID != null && item.SecondTitleID != 0)
                        {
                            foreach (var module in moduleList)
                            {
                                if (module.FirstTitle == itemFirstTitle && module.SecondTitleID != null && module.SecondTitleID != 0)
                                {
                                    json.Append("{\"id\":\"" + module.SecondTitleID + "\",\"ModularName\":\"" + String2Json(module.SecondTitle) + "\",\"state\":\"closed\"},");
                                    //if(string.IsNullOrEmpty(type))
                                    
                                    //else
                                    //    json.Append("{\"id\":\"" + module.SecondTitleID + "\",\"ModularName\":\"" + String2Json(module.SecondTitle) + "\",");
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
                json.Remove(json.Length - 1, 1);
                json.Append("]");
                context.Response.Write(json);
                context.Response.End();
            }
            else
            {
                context.Response.Write(JsonHelper.EncodeJson(moduleList));
                context.Response.End();
            }
        }

        /// <summary>
        /// 获取书本配置过模块
        /// </summary>
        /// <param name="context"></param>
        private void GetCompletedModule(HttpContext context)
        {
            ModularSortBLL modularSortBLL = new ModularSortBLL();
            string bookid = context.Request["BookID"] ?? "";
            string where;
            where = "BookID = " + bookid;
            IList<TB_ModuleSort> moduleList = modularSortBLL.GetModuleList(where);
            if (moduleList != null)
            {
                context.Response.Write(JsonHelper.EncodeJson(new { obj = moduleList, result = true }));
                context.Response.End();
            }
            else
            {
                context.Response.Write(JsonHelper.EncodeJson(new { result = false }));
                context.Response.End();
            }
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

        public class Module
        {
            public string Id { get; set; }
            public string Level { get; set; }
            public string Sortid { get; set; }
            public string Mouduleid { get; set; }
            public string Modularname { get; set; }
            public string Parentid { get; set; }
            public string isNull { get; set; }
            public int State { get; set; }
        }

        public class Data
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int isRemove { get; set; }
            public Children[] Children { get; set; }
        }

        public class Children
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int isRemove { get; set; }
        }

        public class Modular
        {
            public int Id { get; set; }
            public int? ModularId { get; set; }
            public string ModularName { get; set; }
            public int? SuperiorId { get; set; }
            public string Level { get; set; }
            public ModularChildren[] Children { get; set; }
        }

        public class UserStatistics
        {
            public int ID { get; set; }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Telephone { get; set; }
            public DateTime RegistrationTime { get; set; }
            public DateTime LastLoginTime { get; set; }
            public int StartTimes { get; set; }
            public int LoginStatus { get; set; }
            public DateTime CreateTime { get; set; }
            public int BookID { get; set; }
            public string TeachingNaterialName { get; set; }
        }

        public class ModularChildren
        {
            public int Id { get; set; }
            public int? ModularId { get; set; }
            public string ModularName { get; set; }
            public int? SuperiorId { get; set; }
            public string Level { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}