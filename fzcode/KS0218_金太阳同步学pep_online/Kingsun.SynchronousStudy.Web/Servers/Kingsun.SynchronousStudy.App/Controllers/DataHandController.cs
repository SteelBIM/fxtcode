using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Common.Model;
using log4net;
using System.Text.RegularExpressions;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class DataHandController : ApiController
    {

        CourseBLL coursebll = new CourseBLL();
        ModuleConfigurationBLL moduleconfig = new ModuleConfigurationBLL();
        VersionChangeBLL versionbll = new VersionChangeBLL();
        ModularSortBLL modularSortBLL = new ModularSortBLL();
        APPManagementBLL appmangementbll = new APPManagementBLL();
        DataHand datahand = new DataHand();
        string modUrl = WebConfigurationManager.AppSettings["modUrl"];
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string RJTBX = System.Configuration.ConfigurationManager.ConnectionStrings["RJTBX"].ConnectionString;

        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <param name="request">返回版本id和版本名</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetEditionList()
        {
            try
            {
                var edList = new ArrayList();
                edList = coursebll.GetEdition();
                if (edList.Count != 0)
                {
                    return ObjectToJson.GetResult(edList);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在版本信息");
        }

        /// <summary>
        /// 根君配置查看当前用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCourseListAgo([FromBody]KingRequest request)
        {
            APPManagement submitData = JsonHelper.DecodeJson<APPManagement>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.AppID == null && !string.IsNullOrEmpty(submitData.AppID))
            {
                return ObjectToJson.GetErrorResult("当前版本信息不能为空");
            }
            try
            {
                TB_APPManagement app = appmangementbll.GetAPPManagement(submitData.AppID);
                if (app == null)
                {
                    return ObjectToJson.GetErrorResult("不存在版本信息");
                }
                PayActionController pay = new PayActionController();
                string where = "StageID=2 and SubjectID=3 and EditionID=" + app.VersionID + " and State =1 order by GradeID,BreelID";

                string sql = string.Format(@"SELECT * FROM TB_CurriculumManage WHERE {0}", where);
                DataSet ds = pay.SelectOrderSql(submitData.AppID, sql);
                List<TB_CurriculumManage> courselist = JsonHelper.DataSetToIList<TB_CurriculumManage>(ds, 0); //coursebll.GetCourseList(where) as List<TB_CurriculumManage>;

                if (courselist == null)
                {
                    return ObjectToJson.GetErrorResult("不存在版本信息");
                }
                string wheres = " State=0 ";

                string strsql = string.Format(@"SELECT * FROM TB_ModuleConfiguration WHERE  State=0");
                DataSet dsMC = pay.SelectOrderSql(submitData.AppID, strsql);

                List<TB_ModuleConfiguration> newversionlist = JsonHelper.DataSetToIList<TB_ModuleConfiguration>(dsMC, 0);//moduleconfig.GetModuleList(wheres) as List<TB_ModuleConfiguration>;// versionbll.GetModuleByWhere(wheres) as List<TB_VersionChange>;
                //去重
                List<TB_ModuleConfiguration> versionlist = newversionlist.Where((x, i) => newversionlist.FindIndex(z => z.BookID == x.BookID) == i).ToList();
                List<TB_CurriculumManage> courselistes = new List<TB_CurriculumManage>();
                foreach (TB_CurriculumManage curricu in courselist)
                {
                    foreach (TB_ModuleConfiguration verchange in versionlist)
                    {
                        if (curricu.BookID == verchange.BookID)
                        {
                            if (!courselistes.Contains(curricu))
                            {
                                courselistes.Add(curricu);
                            }
                        }
                    }

                }
                if (courselist != null)
                {
                    return ObjectToJson.GetResult(courselistes);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在课程信息");

        }

        /// <summary>
        /// 获取激活码是否显示
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetActivateState([FromBody]KingRequest request)
        {
            AppUpDate application = JsonHelper.DecodeJson<AppUpDate>(request.Data);
            if (application == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (application.AppID == null)
            {
                return ObjectToJson.GetErrorResult("AppID版本号为空");
            }
            if (application.VersionNumber == null)
            {
                return ObjectToJson.GetErrorResult("版本参数为空");
            }
            int versionId = coursebll.GetVersionID(application.AppID);
            if (versionId != 0)
            {
                string sql = string.Format(@"SELECT isEnabled FROM  [TB_ApplicationVersion] WHERE VersionID='{0}' AND VersionNumber='{1}' AND VersionType =1 AND State=1 ORDER BY CreateDate DESC", versionId, application.VersionNumber);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        object obj = new
                        {
                            isEnabled = ds.Tables[0].Rows[0]["isEnabled"].ToString()
                        };
                        return ObjectToJson.GetResult(obj);
                    }
                    else
                    {
                        object obj = new
                        {
                            isEnabled = 0
                        };
                        return ObjectToJson.GetResult(obj);
                    }
                }
                else
                {
                    object obj = new
                    {
                        isEnabled = 0
                    };
                    return ObjectToJson.GetResult(obj);
                }
            }
            return ObjectToJson.GetErrorResult("查询错误");
        }

        /// <summary>
        /// 根君配置查看当前用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCourseList([FromBody]KingRequest request)
        {
            APPManagement submitData = JsonHelper.DecodeJson<APPManagement>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.AppID == null && !string.IsNullOrEmpty(submitData.AppID))
            {
                return ObjectToJson.GetErrorResult("当前版本信息不能为空");
            }
            try
            {
                //TB_APPManagement app = appmangementbll.GetAPPManagement(submitData.AppID);
                string errMsg;
                var courselistes = coursebll.GetCourseListMethod(submitData.AppID, out errMsg);
                if (errMsg != "")
                {
                    return ObjectToJson.GetErrorResult(errMsg);
                }

                if (courselistes != null)
                {
                    return ObjectToJson.GetResult(courselistes);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在课程信息");

        }




        /// <summary>
        /// 根君配置查看当前用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCourseListtest()
        {
            APPManagement submitData = new APPManagement();//JsonHelper.DecodeJson<APPManagement>(request.data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            //if (submitData.AppID == null && !string.IsNullOrEmpty(submitData.AppID))
            //{
            //    return ObjectToJson.GetErrorResult("当前版本信息不能为空");
            //}
            submitData.AppID = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";
            try
            {
                TB_APPManagement app = appmangementbll.GetAPPManagement(submitData.AppID);
                if (app == null)
                {
                    return ObjectToJson.GetErrorResult("不存在版本信息");
                }

                string where = "StageID=2 and SubjectID=3 and EditionID=" + app.VersionID + " and State =1 order by GradeID,BreelID";
                List<TB_CurriculumManage> courselist = coursebll.GetCourseList(where) as List<TB_CurriculumManage>;
                if (courselist == null)
                {
                    return ObjectToJson.GetErrorResult("不存在版本信息");
                }
                string wheres = " State=0 ";

                List<TB_ModuleConfiguration> newversionlist = moduleconfig.GetModuleList(wheres) as List<TB_ModuleConfiguration>;// versionbll.GetModuleByWhere(wheres) as List<TB_VersionChange>;
                //去重
                List<TB_ModuleConfiguration> versionlist = newversionlist.Where((x, i) => newversionlist.FindIndex(z => z.BookID == x.BookID) == i).ToList();
                List<TB_CurriculumManage> courselistes = new List<TB_CurriculumManage>();
                foreach (TB_CurriculumManage curricu in courselist)
                {
                    foreach (TB_ModuleConfiguration verchange in versionlist)
                    {
                        if (curricu.BookID == verchange.BookID)
                        {
                            if (!courselistes.Contains(curricu))
                            {
                                courselistes.Add(curricu);
                            }
                        }
                    }

                }
                if (courselist != null)
                {
                    return ObjectToJson.GetResult(courselistes);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在课程信息");

        }

        /// <summary>
        /// 获取课程下目录信息列表 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCatalogueList([FromBody] KingRequest request)
        {
            TB_ModuleConfiguration submitData = JsonHelper.DecodeJson<TB_ModuleConfiguration>(request.Data);
            StreamReader responseReader = null;
            List<Data> listS = new List<Data>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书本课程为空");
            }

            try
            {
                //ashx Url
                // string getGscUserUrl = "http://183.47.42.221:8027/GetCatalogByBookId.sun";
                //加入参数，用于更新请求
                string urlHandler = modUrl + "GetCatalogByBookId.sun?t[BookId]=" + submitData.BookID;
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlHandler);
                webRequest.Timeout = 3000;//3秒超时
                //调用ashx，并取值
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string currentUserGulid = responseReader.ReadToEnd();

                Data[] bookdata = js.Deserialize<Data[]>(currentUserGulid.Trim());
                listS = new List<Data>(bookdata);
                // List<info> infos = Dal.GetInfos();  
                DataTable dt = new DataTable();
                dt.Columns.Add("BookID");
                dt.Columns.Add("FirstTitileID");
                dt.Columns.Add("FirstTitle");
                dt.Columns.Add("SecondTitleID");
                dt.Columns.Add("SecondTitle");

                foreach (var info in listS)
                {
                    DataRow dr = dt.NewRow();
                    dr["BookID"] = submitData.BookID;
                    dr["FirstTitileID"] = info.Id;
                    dr["FirstTitle"] = info.Title;
                    if (info.Children.Length > 0)
                    {
                        foreach (var children in info.Children)
                        {
                            dr["SecondTitleID"] = children.Id;
                            dr["SecondTitle"] = children.Title;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return ObjectToJson.GetErrorResult(ex.Message);

            }
            finally
            {
                if (responseReader != null)
                {
                    responseReader.Close();
                    responseReader.Dispose();
                }
            }
            try
            {
                string where = "BookID=" + submitData.BookID + " AND State=0 order by ID";
                List<TB_ModuleConfiguration> courseconfiglist = moduleconfig.GetModuleList(where) as List<TB_ModuleConfiguration>;
                if (courseconfiglist != null)
                {
                    return ObjectToJson.GetResult(courseconfiglist);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在课程信息");
        }

        /// <summary>
        /// 获取课程下目录信息列表 (测试)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCatalogueListTest()
        {
            var g = Guid.NewGuid();
            TB_ModuleConfiguration submitData = new TB_ModuleConfiguration();//JsonHelper.DecodeJson<TB_ModuleConfiguration>(request.data);
            StreamReader responseReader = null;
            List<Data> listS = new List<Data>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            submitData.BookID = 170;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书本课程为空");
            }


            string where = "BookID=" + submitData.BookID + " And State=0 order by ID";
            List<TB_ModuleConfiguration> courseconfiglist = moduleconfig.GetModuleList(where) as List<TB_ModuleConfiguration>;
            if (courseconfiglist != null)
            {
                return ObjectToJson.GetResult(courseconfiglist);
            }
            else
            {
                return ObjectToJson.GetErrorResult("不存在课程信息");
            }
        }

        /// <summary>
        /// 获取课程下目录信息列表（新加） 如果有用户信息 就把书籍信息同步到用户信息表 △
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCatalogueLists([FromBody] KingRequest request)
        {
            CatalogueandUser submitData = JsonHelper.DecodeJson<CatalogueandUser>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID < 0)
            {
                return ObjectToJson.GetErrorResult("书本课程为空");
            }
            DataSet ds = new DataSet();
            try
            {
                if (submitData.UserID != null) //更新用户信息
                {
                    bool Success = datahand.UpdateUserID(submitData.UserID, submitData.BookID);
                    if (!Success)
                    {
                        return ObjectToJson.GetErrorResult("当前书籍信息与用户信息没有同步成功");
                    }
                }

                string where = " a.BookID=" + submitData.BookID + " AND a.State=0 ";
                where += "and b.Column_8 is null";

                string sql = string.Format(@"SELECT  a.ID
                                                      ,a.[BookID]
                                                      ,a.[TeachingNaterialName]
                                                      ,a.[FirstTitileID]
                                                      ,a.[FirstTitle]
                                                      ,a.[SecondTitleID]
                                                      ,a.[SecondTitle]
                                                      ,a.State      
                                                      ,a.[CreateDate]
                                                      ,b.EndingPage
                                                      ,b.StartingPage
                                                FROM    dbo.TB_ModuleConfiguration a
                                                        LEFT JOIN dbo.TB_CatalogPage b ON a.BookID = b.BookID
                                                AND a.FirstTitileID = b.FirstTitleID
                                                AND ((b.SecondTitleID IS NULL AND a.SecondTitleID IS NULL) OR a.SecondTitleID = b.SecondTitleID) WHERE {0};", where);

                ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            List<TB_ModuleConfiguration> courseconfiglist = DataSetToIList<TB_ModuleConfiguration>(ds, 0);
            if (courseconfiglist != null)
            {
                return ObjectToJson.ListToJson(courseconfiglist);
            }
            else
            {
                return ObjectToJson.GetErrorResult("不存在课程信息");
            }
        }

        /// <summary>
        /// 获取课程下目录信息列表（新加） 如果有用户信息 就把书籍信息同步到用户信息表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetCatalogueListsTest()
        {
            CatalogueandUser submitData = new CatalogueandUser(); //JsonHelper.DecodeJson<CatalogueandUser>(request.data);
            submitData.BookID = 169;
            DataSet ds = new DataSet();
            try
            {
                if (submitData.UserID != null) //更新用户信息
                {
                    bool Success = datahand.UpdateUserID(submitData.UserID, submitData.BookID);
                    if (!Success)
                    {
                        return ObjectToJson.GetErrorResult("当前书籍信息与用户信息没有同步成功");
                    }
                }

                string where = " a.BookID=" + submitData.BookID + " AND a.State=0 ";
                if (submitData.IsYX == null || submitData.IsYX == 0)
                {
                    where += "and b.Column_8 is null";
                }
                else
                {
                    where += "and b.Column_8 is not null"; //优学目录导入时,会给Column_8字段赋值1
                }

                string sql = string.Format(@"SELECT  a.ID
                                                      ,a.[BookID]
                                                      ,a.[TeachingNaterialName]
                                                      ,a.[FirstTitileID]
                                                      ,a.[FirstTitle]
                                                      ,a.[SecondTitleID]
                                                      ,a.[SecondTitle]
                                                      ,a.[CreateDate]
                                                      ,a.State      
                                                      ,b.EndingPage
                                                      ,b.StartingPage
                                              FROM    dbo.TB_ModuleConfiguration a
                                              LEFT JOIN dbo.TB_CatalogPage b ON a.BookID = b.BookID
                                              AND a.FirstTitileID = b.FirstTitleID
                                              AND ((b.SecondTitleID IS NULL AND a.SecondTitleID IS NULL) OR  a.SecondTitleID = b.SecondTitleID) WHERE {0};", where);
                ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);                //List<TB_ModuleConfiguration> courseconfiglist = moduleconfig.GetModuleList(where) as List<TB_ModuleConfiguration>;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            List<TB_ModuleConfiguration> courseconfiglist = DataSetToIList<TB_ModuleConfiguration>(ds, 0);
            if (courseconfiglist != null)
            {
                return ObjectToJson.ListToJson(courseconfiglist);
            }
            else
            {
                return ObjectToJson.GetErrorResult("不存在课程信息");
            }
        }


        /// <summary>
        ///根据书本一级标题id二级标题id获取配置模块信息 返回带下载地址的一些信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetModuleConfigurationByFirstIDSecondID([FromBody] KingRequest request)
        {
            string where = "";
            module submitData = JsonHelper.DecodeJson<module>(request.Data);

            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书本课程为空");
            }
            if (string.IsNullOrEmpty(submitData.IsYX))
            {
                submitData.IsYX = "0";
            }
            else
            {
                submitData.IsYX = "1";
            }
            try
            {
                where = "BookID=" + submitData.BookID;
                if (submitData.FirstTitleID != null) //传第一级模块标识
                {
                    where += " and FirstTitleID =" + submitData.FirstTitleID;
                    if (submitData.SecondTitleID != null)
                    {
                        where += " and SecondTitleID =" + submitData.SecondTitleID;
                    }

                    List<TB_ModuleSort> modularlist = modularSortBLL.GetModuleList(where) as List<TB_ModuleSort>;

                    if (modularlist != null)
                    {
                        FilterForNotYX(modularlist, submitData.IsYX);
                    }

                    List<object> obj = null;
                    //IsYX(0代表2.8.0之前的版本，1代表2.8.0版本)
                    if (submitData.IsYX == "0")
                    {
                        #region
                        //人教
                        string sql = string.Format(@"SELECT  [ID]
                                                              ,[ModuleID]
                                                              ,[ModuleName]
                                                              ,[SuperiorID]
                                                              ,[Sort]
                                                              ,[BookID]
                                                              ,[FirstTitleID]
                                                              ,[SecondTitleID]
                                                              ,[CreateTime]
                                                          FROM [TB_ModuleSort] where {0}", where);
                        DataSet ds = SqlHelper.ExecuteDataset(RJTBX, CommandType.Text, sql);
                        List<tbmodulesort> ms = JsonHelper.DataSetToIList<tbmodulesort>(ds, 0);

                        #endregion

                        where += " and State=1 order by ID";

                        #region
                        //人教
                        string strSql = string.Format(@"SELECT [ID]
                                                              ,[ModuleID]
                                                              ,[BooKID]
                                                              ,[TeachingNaterialName]
                                                              ,[ModuleName]
                                                              ,[FirstTitleID]
                                                              ,[FirstTitle]
                                                              ,[SecondTitleID]
                                                              ,[SecondTitle]
                                                              ,[ModuleAddress]
                                                              ,[MD5]
                                                              ,[IncrementalPacketAddress]
                                                              ,[IncrementalPacketMD5]
                                                              ,[ModuleVersion]
                                                              ,[UpdateDescription]
                                                              ,[State]
                                                              ,[IsUpdate]
                                                              ,[CreateDate]
                                                          FROM [dbo].[TB_VersionChange] where {0}", where);
                        DataSet dsv = SqlHelper.ExecuteDataset(RJTBX, CommandType.Text, strSql);
                        List<versionchange> vc = JsonHelper.DataSetToIList<versionchange>(dsv, 0);

                        #endregion
                        obj = datahand.GetRJTBXVersionChanges(ms, vc);

                    }
                    else
                    {
                        where += " and State=1 order by ID";
                    }
                    List<TB_VersionChange> newversionlist = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                    List<object> objlist = datahand.GetModuleSortVersionChanges(modularlist, newversionlist);
                    //Log4Net.LogHelper.Info("GetModuleConfigurationByFirstIDSecondID接口objlist的值" + JsonHelper.EncodeJson(objlist));
                    if (objlist != null)
                    {
                        if (obj != null)
                        {
                            objlist.InsertRange(0, obj);
                            return ObjectToJson.GetResult(objlist);
                        }
                        else
                        {
                            return ObjectToJson.GetResult(objlist);

                        }
                    }

                }
                else //只传bookid 默认返回 第一模块 第一单元
                {
                    List<TB_ModuleSort> modularlists = datahand.GetModuleSort(submitData.BookID.ToString());
                    where += " and State=1 order by ID";

                    if (modularlists != null)
                    {
                        FilterForNotYX(modularlists, submitData.IsYX);
                    }

                    List<TB_VersionChange> newversionlist = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                    List<object> objlist = datahand.GetModuleSortVersionChanges(modularlists, newversionlist);

                    if (objlist != null)
                    {
                        return ObjectToJson.GetResult(objlist);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "GetModuleConfigurationByFirstIDSecondID接口错误");
            }
            return ObjectToJson.GetErrorResult("不存在课程信息");
        }

        /// <summary>
        /// 过滤到优学2的模块，替换优学1的名称
        /// </summary>
        /// <param name="modularlist"></param>
        private void FilterForNotYX(List<TB_ModuleSort> modularlist, string IsYx)
        {
            List<TB_ModuleSort> needRemoveModularlist = new List<TB_ModuleSort>();
            foreach (var item in modularlist)
            {
                if (Regex.IsMatch(item.ModuleName, @"^YX1_"))
                {
                    if (IsYx == "0")
                    {
                        needRemoveModularlist.Add(item);
                    }
                    else
                    {
                        item.ModuleName = item.ModuleName.Remove(0, 4);
                    }
                }
                if (Regex.IsMatch(item.ModuleName, @"^YX2_"))
                {
                    needRemoveModularlist.Add(item);
                }
                if (Regex.IsMatch(item.ModuleName, @"^YX3_"))
                {
                    needRemoveModularlist.Add(item);
                }
            }
            foreach (var item in needRemoveModularlist)
            {
                modularlist.Remove(item);
            }
        }

        /// <summary>
        /// 筛选出优学2的模块
        /// </summary>
        /// <param name="modularlist"></param>
        private void FilterForYX(List<TB_ModuleSort> modularlist)
        {
            List<TB_ModuleSort> needRemoveModularlist = new List<TB_ModuleSort>();
            foreach (var item in modularlist)
            {
                if (!Regex.IsMatch(item.ModuleName, @"^YX2_"))
                {
                    needRemoveModularlist.Add(item);
                }
                else
                {
                    item.ModuleName = item.ModuleName.Remove(0, 4);
                }
            }
            foreach (var item in needRemoveModularlist)
            {
                modularlist.Remove(item);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetModuleConfigurationByFirstIDSecondIDtest(int bookID, int FirstTitleID, int SecondTitleID, string AppID)
        {
            string where = "";
            module submitData = new module();//JsonHelper.DecodeJson<TB_ModuleSort>(request.Data);
            submitData.BookID = bookID;
            submitData.FirstTitleID = FirstTitleID;
            submitData.SecondTitleID = SecondTitleID;
            submitData.AppID = AppID;
            if (string.IsNullOrEmpty(submitData.IsYX))
            {
                submitData.IsYX = "0";
            }
            else
            {
                submitData.IsYX = "1";
            }


            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书本课程为空");
            }
            try
            {
                where = "BookID=" + submitData.BookID;
                if (submitData.FirstTitleID != null) //传第一级模块标识
                {
                    where += " and FirstTitleID =" + submitData.FirstTitleID;
                    if (submitData.SecondTitleID != null)
                    {
                        where += " and SecondTitleID =" + submitData.SecondTitleID;
                    }

                    List<TB_ModuleSort> modularlist = modularSortBLL.GetModuleList(where) as List<TB_ModuleSort>;
                    if (modularlist != null)
                    {
                        FilterForNotYX(modularlist, submitData.IsYX);
                    }

                    List<object> obj = null;
                    if (submitData.IsYX == "0")
                    {
                        #region 注释获取人教模块


                        #region

                        //人教
                        string sql = string.Format(@"SELECT  [ID]
                                                              ,[ModuleID]
                                                              ,[ModuleName]
                                                              ,[SuperiorID]
                                                              ,[Sort]
                                                              ,[BookID]
                                                              ,[FirstTitleID]
                                                              ,[SecondTitleID]
                                                              ,[CreateTime]
                                                          FROM [TB_ModuleSort] where {0}", where);
                        DataSet ds = SqlHelper.ExecuteDataset(RJTBX, CommandType.Text, sql);
                        List<tbmodulesort> ms = JsonHelper.DataSetToIList<tbmodulesort>(ds, 0);

                        #endregion

                        where += " and State=1 order by ID";

                        #region

                        //人教
                        string strSql = string.Format(@"SELECT [ID]
                                                              ,[ModuleID]
                                                              ,[BooKID]
                                                              ,[TeachingNaterialName]
                                                              ,[ModuleName]
                                                              ,[FirstTitleID]
                                                              ,[FirstTitle]
                                                              ,[SecondTitleID]
                                                              ,[SecondTitle]
                                                              ,[ModuleAddress]
                                                              ,[MD5]
                                                              ,[IncrementalPacketAddress]
                                                              ,[IncrementalPacketMD5]
                                                              ,[ModuleVersion]
                                                              ,[UpdateDescription]
                                                              ,[State]
                                                              ,[IsUpdate]
                                                              ,[CreateDate]
                                                          FROM [dbo].[TB_VersionChange] where {0}", where);
                        DataSet dsv = SqlHelper.ExecuteDataset(RJTBX, CommandType.Text, strSql);
                        List<versionchange> vc = JsonHelper.DataSetToIList<versionchange>(dsv, 0);

                        #endregion

                        obj = datahand.GetRJTBXVersionChanges(ms, vc);

                        #endregion
                    }
                    else
                    {
                        where += " and State=1 order by ID";
                    }
                    List<TB_VersionChange> newversionlist = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                    List<object> objlist = datahand.GetModuleSortVersionChanges(modularlist, newversionlist);

                    if (objlist != null)
                    {
                        if (obj != null)
                        {
                            objlist.InsertRange(0, obj);
                            return ObjectToJson.GetResult(objlist);
                        }
                        else
                        {
                            return ObjectToJson.GetResult(objlist);

                        }
                    }

                }
                else //只传bookid 默认返回 第一模块 第一单元
                {
                    List<TB_ModuleSort> modularlists = datahand.GetModuleSort(submitData.BookID.ToString());
                    where += " and State=1 order by ID";

                    if (modularlists != null)
                    {
                        FilterForNotYX(modularlists, submitData.IsYX);
                    }

                    List<TB_VersionChange> newversionlist = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                    List<object> objlist = datahand.GetModuleSortVersionChanges(modularlists, newversionlist);

                    if (objlist != null)
                    {
                        return ObjectToJson.GetResult(objlist);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "获取模块信息错误：bookID：" + bookID + ";FirstTitleID:" + FirstTitleID + ";SecondTitleID:" + SecondTitleID + ";AppID:" + AppID);
            }
            return ObjectToJson.GetErrorResult("不存在课程信息");
        }

        /// <summary>
        /// 获取模块资源最新版本 △
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetModularLatestVersion([FromBody] KingRequest request)
        {
            VW_VersionChange submitData = JsonHelper.DecodeJson<VW_VersionChange>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.ModuleID == null)
            {
                return ObjectToJson.GetErrorResult("模块资源信息为空");
            }
            if (submitData.BooKID == null)
            {
                return ObjectToJson.GetErrorResult("书本信息为空");
            }
            if (submitData.FirstTitleID == null)
            {
                return ObjectToJson.GetErrorResult("资源信息为空");
            }
            string where = "ModuleID=" + submitData.ModuleID + " and BooKID=" + submitData.BooKID + " and FirstTitleID=" + submitData.FirstTitleID + " and State=1";
            if (submitData.SecondTitleID != null)
            {
                where += " and SecondTitleID= " + submitData.SecondTitleID;
            }
            where += " order by CreateDate DESC";
            try
            {
                if (submitData.IsYX == null || submitData.IsYX == 0)
                {
                    TB_VersionChange newversion = versionbll.GetNewVersionChange(where);
                    if (newversion != null)
                    {
                        if (string.IsNullOrEmpty(newversion.MD5))
                        {
                            return ObjectToJson.GetErrorResult("不存在模块资源信息");
                        }
                        else
                        {
                            object obj = new { Success = true, data = newversion, Message = "", UpdateType = 1 };
                            return KingsunResponse.toJson(obj);
                        }
                    }
                }
                else
                {
                    TB_VersionChange_YX newversion_YX = versionbll.GetNewVersionChange_YX(where);
                    if (newversion_YX != null)
                    {
                        if (string.IsNullOrEmpty(newversion_YX.MD5))
                        {
                            return ObjectToJson.GetErrorResult("不存在模块资源信息");
                        }
                        else
                        {
                            object obj = new { Success = true, data = newversion_YX, Message = "", UpdateType = 1 };
                            return KingsunResponse.toJson(obj);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在模块资源信息");
        }

        /// <summary>
        /// 获取模块资源版本更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetModularUpdate([FromBody] KingRequest request)
        {
            VW_VersionChange submitData = JsonHelper.DecodeJson<VW_VersionChange>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.ModuleID == null)
            {
                return ObjectToJson.GetErrorResult("模块资源信息为空");
            }
            if (submitData.BooKID == null)
            {
                return ObjectToJson.GetErrorResult("书本信息为空");
            }
            if (submitData.FirstTitleID == null)
            {
                return ObjectToJson.GetErrorResult("资源信息为空");
            }
            string where = "ModuleID=" + submitData.ModuleID + " and BooKID=" + submitData.BooKID + " and FirstTitleID=" + submitData.FirstTitleID + " and State=1";
            if (submitData.SecondTitleID != null)
            {
                where += " and SecondTitleID= " + submitData.SecondTitleID;
            }
            where += " order by CreateDate DESC";
            try
            {
                if (submitData.IsYX == null || submitData.IsYX == 0)
                {
                    TB_VersionChange newversion = versionbll.GetNewVersionChange(where);
                    if (newversion != null)
                    {
                        string version = "v0.0.0";
                        if (submitData.ModuleVersion != null)
                        {
                            version = submitData.ModuleVersion;
                        }
                        Version v1 = new Version(newversion.ModuleVersion.ToString().Remove(0, 1));
                        if (v1 != null)
                        {
                            Version v2 = new Version(version.Remove(0, 1));
                            if (v1 == v2 || v1 < v2)
                            {
                                return ObjectToJson.GetErrorResult("当前版本已是最新！");
                            }
                            return ObjectToJson.GetResult(newversion);
                        }
                    }
                }
                else
                {
                    TB_VersionChange_YX newversion_YX = versionbll.GetNewVersionChange_YX(where);
                    if (newversion_YX != null)
                    {
                        string version = "v0.0.0";
                        if (submitData.ModuleVersion != null)
                        {
                            version = submitData.ModuleVersion;
                        }
                        Version v1 = new Version(newversion_YX.ModuleVersion.ToString().Remove(0, 1));
                        if (v1 != null)
                        {
                            Version v2 = new Version(version.Remove(0, 1));
                            if (v1 == v2 || v1 < v2)
                            {
                                return ObjectToJson.GetErrorResult("当前版本已是最新！");
                            }
                            return ObjectToJson.GetResult(newversion_YX);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("不存在该版本信息");
        }

        /// <summary>
        /// 添加课程历史记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddBookHistorys([FromBody] KingRequest request)
        {
            TB_UserBookHistory submitData = JsonHelper.DecodeJson<TB_UserBookHistory>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.UserID == null)
            {
                return ObjectToJson.GetErrorResult("用户不能为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书籍不能为空");
            }
            try
            {
                if (coursebll.AddBookHistorys(submitData)) //内部判断是否已经添加过
                {
                    return ObjectToJson.GetResult(submitData);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("已经添加过");
        }

        /// <summary>
        /// 获取某本书的点读的所有资源下载地址 且都是最高版本 △
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetPointReadAssetsWithBookID([FromBody] KingRequest request)
        {
            //TB_VersionChange submitData = JsonHelper.DecodeJson<TB_VersionChange>(request.data);
            VW_UserBookHistory submitData = JsonHelper.DecodeJson<VW_UserBookHistory>(request.Data);

            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书籍不能为空");
            }
            try
            {
                string where = "BooKID=" + submitData.BookID + " and ModuleName='点读' and State=1  order by FirstTitleID,SecondTitleID";

                if (submitData.IsYX == null || submitData.IsYX == 0)
                {
                    List<TB_VersionChange> listverchange = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                    //比较版本
                    if (listverchange != null)
                    {
                        List<TB_VersionChange> newversionlistsort = (from c in listverchange
                                                                     orderby c.ID descending //ascending  //倒序
                                                                     select c).ToList();
                        List<TB_VersionChange> listverchanges = new List<TB_VersionChange>();

                        foreach (TB_VersionChange verchange in listverchange)
                        {
                            foreach (TB_VersionChange newsr in newversionlistsort)
                            {
                                if (verchange.FirstTitleID == newsr.FirstTitleID && verchange.SecondTitleID == newsr.SecondTitleID)
                                {
                                    Version v1 = new Version(verchange.ModuleVersion.ToString().Remove(0, 1));
                                    if (v1 != null)
                                    {
                                        Version v2 = new Version(newsr.ModuleVersion.ToString().Remove(0, 1));
                                        if (v1 == v2 || v1 < v2)
                                        {
                                            // return ObjectToJson.GetErrorResult("当前版本已是最新！");
                                            listverchanges.Add(verchange);
                                        }

                                    }
                                }
                            }
                        }

                        if (listverchanges != null)
                        {
                            List<TB_VersionChange> versionlist = listverchanges.Where((x, i) => listverchanges.FindIndex(z => z.BooKID == x.BooKID && z.FirstTitleID == x.FirstTitleID && z.SecondTitleID == x.SecondTitleID) == i).ToList();

                            return ObjectToJson.GetResult(versionlist);
                        }
                    }
                }
                else
                {
                    List<TB_VersionChange_YX> listverchange_YX = versionbll.GetModuleByWhere_YX(where) as List<TB_VersionChange_YX>;

                    //比较版本
                    if (listverchange_YX != null)
                    {
                        List<TB_VersionChange_YX> newversionlistsort = (from c in listverchange_YX
                                                                        orderby c.ID descending //ascending  //倒序
                                                                        select c).ToList();
                        List<TB_VersionChange_YX> listverchanges = new List<TB_VersionChange_YX>();

                        foreach (TB_VersionChange_YX verchange in listverchange_YX)
                        {
                            foreach (TB_VersionChange_YX newsr in newversionlistsort)
                            {
                                if (verchange.FirstTitleID == newsr.FirstTitleID && verchange.SecondTitleID == newsr.SecondTitleID)
                                {
                                    Version v1 = new Version(verchange.ModuleVersion.ToString().Remove(0, 1));
                                    if (v1 != null)
                                    {
                                        Version v2 = new Version(newsr.ModuleVersion.ToString().Remove(0, 1));
                                        if (v1 == v2 || v1 < v2)
                                        {
                                            // return ObjectToJson.GetErrorResult("当前版本已是最新！");
                                            listverchanges.Add(verchange);
                                        }

                                    }
                                }
                            }
                        }

                        if (listverchanges != null)
                        {
                            List<TB_VersionChange_YX> versionlist = listverchanges.Where((x, i) => listverchanges.FindIndex(z => z.BooKID == x.BooKID && z.FirstTitleID == x.FirstTitleID && z.SecondTitleID == x.SecondTitleID) == i).ToList();

                            return ObjectToJson.GetResult(versionlist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("数据库不存在数据");
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="dd"></param>
        /// <param name="ddd"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDecidePhoneCode()
        {
            string where = "";
            TB_ModuleSort submitData = new TB_ModuleSort();//JsonHelper.DecodeJson<TB_ModuleSort>(request.data);
            submitData.BookID = 211;
            submitData.FirstTitleID = 277970;
            submitData.SecondTitleID = 277971;

            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.BookID == null)
            {
                return ObjectToJson.GetErrorResult("书本课程为空");
            }
            where = "BookID=" + submitData.BookID;
            if (submitData.FirstTitleID != null) //传第一级模块标识
            {
                where += " and FirstTitleID =" + submitData.FirstTitleID;
                if (submitData.SecondTitleID != null)
                {
                    where += " and SecondTitleID =" + submitData.SecondTitleID;
                }

                string sql = @"SELECT a.[ID]
                              ,[ModuleID]
                              ,[ModuleName]
                              ,a.[SuperiorID]
                              ,[Sort]
                              ,[BookID]
                              ,[FirstTitleID]
                              ,[SecondTitleID]
                              ,[CreateTime]
                              ,b.ActiveState
                          FROM [TB_ModuleSort] a LEFT JOIN dbo.TB_ModularManage b ON a.ModuleID=b.ModularID WHERE " + where + " OR " + where + " And ActiveState=1";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<TB_ModuleSort> modularlist = DataSetToIList<TB_ModuleSort>(ds, 0);

                //List<TB_ModuleSort> modularlist = modularSortBLL.GetModuleList(where) as List<TB_ModuleSort>;
                where += " and State=1 order by ID";
                List<TB_VersionChange> newversionlist = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                // List<VersionChangeModuleSort> objlist = datahand.GetModuleSortVersionChange(modularlist, newversionlist);
                List<object> objlist = datahand.GetModuleSortVersionChanges(modularlist, newversionlist);
                if (objlist != null)
                {
                    return ObjectToJson.GetResult(objlist);
                }
                else
                {
                    return ObjectToJson.GetErrorResult("不存在课程信息");
                }
            }
            else //只传bookid 默认返回 第一模块 第一单元
            {
                List<TB_ModuleSort> modularlists = datahand.GetModuleSort(submitData.BookID.ToString());
                where += " and State=1 order by ID";
                List<TB_VersionChange> newversionlist = versionbll.GetModuleByWhere(where) as List<TB_VersionChange>;
                //   List<VersionChangeModuleSort> objlist = datahand.GetModuleSortVersionChange(modularlists, newversionlist);
                List<object> objlist = datahand.GetModuleSortVersionChanges(modularlists, newversionlist);

                if (objlist != null)
                {
                    return ObjectToJson.GetResult(objlist);
                }
                else
                {
                    return ObjectToJson.GetErrorResult("不存在课程信息");
                }
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




        public class module
        {
            public int? BookID { get; set; }
            public int? FirstTitleID { get; set; }
            public int? SecondTitleID { get; set; }
            public string AppID { get; set; }

            public string IsYX { get; set; }
        }

        public class Data
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public Children[] Children { get; set; }
        }

        public class Children
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
    }

}
