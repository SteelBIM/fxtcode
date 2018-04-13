using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Model;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.App.Filter;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class ApplicationVersionController : ApiController
    {
        UserManagement appVersion = new UserManagement();
        APPManagementBLL appmangementbll = new APPManagementBLL();
        /// <summary>
        /// 版本更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage GetNewVersions([FromBody]KingRequest request)
        //{
        //    TB_ApplicationVersion application = JsonHelper.DecodeJson<TB_ApplicationVersion>(request.data);
        //    if (application == null)
        //    {
        //        ObjectToJson.GetErrorResult("当前信息为空");
        //    }
        //    if (application.VersionType == null)
        //    {
        //        ObjectToJson.GetErrorResult("不确定设备类型");
        //    }
        //    DataSet ds = appVersion.GetAppVersion(application.VersionType.ToString());
        //    if (ds != null)
        //    {
        //        Version v1 = new Version(ds.Tables[0].Rows[0]["VersionNumber"].ToString().Remove(0, 1));
        //        if (v1 != null)
        //        {
        //            string version = "v0.0.0";
        //            if (application.VersionNumber != null)
        //            {
        //                version = application.VersionNumber;
        //            }
        //            Version v2 = new Version(version.Remove(0, 1));
        //            if (v1 == v2 || v1 < v2)
        //            {
        //                return ObjectToJson.GetResult("","当前版本已是最新！");
        //            }
        //            else
        //            {
        //                List<TB_ApplicationVersion> listApp = appVersion.GetDataSet(ds);
        //                object obj = new
        //                {
        //                    rows = listApp
        //                };
        //                return ObjectToJson.GetResult(obj);
        //            }
        //        }
        //        else
        //        {
        //            return ObjectToJson.GetErrorResult("不存在该版本信息");
        //        }
        //    }
        //    else 
        //    {
        //        return ObjectToJson.GetErrorResult("不存在该版本信息");
        //    }         
        //}


        [HttpPost]
        public HttpResponseMessage GetNewVersions([FromBody]KingRequest request)
        {
            AppUpDate application = JsonHelper.DecodeJson<AppUpDate>(request.Data);
            if (application == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (application.AppID == null)
            {
                return GetErrorResult("AppID版本号为空");
            }
            if (application.VersionNumber == null)
            {
                return GetErrorResult("版本号为空");
            }
            //int type = GetVersionType(application.Type);
            //if (string.IsNullOrEmpty(application.Type))
            //{
            //    application.Type = "0";
            //}
            int type = GetVersionType(application.Type);
            if (type == 0)
            {
                return GetErrorResult("版本类型参数错误");
            }
            // DataSet ds = appVersion.GetAppVersion(application.VersionType.ToString());
            TB_APPManagement app = appmangementbll.GetAPPManagement(application.AppID);
            if (app != null)
            {
                //DataSet ds = appVersion.GetAppVersion(app.VersionID.ToString(), type);
                //if (ds == null || ds.Tables.Count < 1)
                //{
                //    return GetErrorResult("系统不存版本更新");
                //}
                //if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0) //判断里面的书籍为空
                //{
                //    return GetErrorResult("系统不存版本更新");
                //}
                string sql = "";
                if (type == 1)
                {
                    sql = string.Format(@"SELECT * FROM  [TB_ApplicationVersion] WHERE VersionID='{0}' AND VersionType ='{1}' AND State=1 ORDER BY CreateDate DESC", app.VersionID, type);
                }
                else
                {
                    sql = string.Format(@"SELECT * FROM  [TB_ApplicationVersion] WHERE VersionID='{0}' AND VersionType ='{1}' AND State=1 ORDER BY CreateDate DESC", app.VersionID, type);
                }
                AppSetting.SetValidUserUsageNumber(application.UserId, application.AppID, app.VersionID.ToString(), application.DownloadChannel);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Rows[0]["VersionDescription"] = ds.Tables[0].Rows[0]["VersionDescription"].ToString().Replace("<br>", "\n").Replace("&nbsp;"," ");
                    Version v1 = new Version(ds.Tables[0].Rows[0]["VersionNumber"].ToString().Remove(0, 1));
                    if (v1 != null)
                    {
                        string version = "v0.0.0";
                        if (application.VersionNumber != null)
                        {
                            version = application.VersionNumber;
                        }

                        Version v2 = new Version(version.Remove(0, 1));
                        if (v1 == v2 || v1 < v2)
                        {
                            return GetResult("", "当前版本已是最新！");
                        }
                        else
                        {
                            List<TB_ApplicationVersion> listApp = appVersion.GetDataSet(ds);
                            object obj = new
                            {
                                rows = listApp
                            };
                            return GetResult(obj);
                        }
                    }
                    else
                    {
                        return GetErrorResult("系统不存该版本");
                    }

                }
            }
            return GetErrorResult("系统不存该版本");
        }

        [HttpGet]
        public HttpResponseMessage GetNewVersionsTest()
        {
            AppUpDate application = new AppUpDate(); //JsonHelper.DecodeJson<AppUpDate>(request.data);
            application.AppID = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";
            application.VersionNumber = "V 3.3.0";
            if (application == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (application.AppID == null)
            {
                return GetErrorResult("AppID版本号为空");
            }
            if (application.VersionNumber == null)
            {
                return GetErrorResult("版本号为空");
            }
            int type = GetVersionType(application.Type);
            if (type == 0)
            {
                return GetErrorResult("版本类型参数错误");
            }
            // DataSet ds = appVersion.GetAppVersion(application.VersionType.ToString());
            TB_APPManagement app = appmangementbll.GetAPPManagement(application.AppID);
            if (app != null)
            {
                //DataSet ds = appVersion.GetAppVersion(app.VersionID.ToString(), type);
                //if (ds == null || ds.Tables.Count < 1)
                //{
                //    return GetErrorResult("系统不存版本更新");
                //}
                //if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0) //判断里面的书籍为空
                //{
                //    return GetErrorResult("系统不存版本更新");
                //}、
                string sql = string.Format(@"SELECT * FROM  [TB_ApplicationVersion] WHERE VersionID='{0}' AND State=1 ORDER BY CreateDate DESC", app.VersionID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Rows[0]["VersionDescription"] = ds.Tables[0].Rows[0]["VersionDescription"].ToString().Replace("<br>", "\n");
                    Version v1 = new Version(ds.Tables[0].Rows[0]["VersionNumber"].ToString().Remove(0, 1));
                    if (v1 != null)
                    {
                        string version = "v0.0.0";
                        if (application.VersionNumber != null)
                        {
                            version = application.VersionNumber;
                        }

                        Version v2 = new Version(version.Remove(0, 1));
                        if (v1 == v2 || v1 < v2)
                        {
                            return GetResult("", "当前版本已是最新！");
                        }
                        else
                        {
                            List<TB_ApplicationVersion> listApp = appVersion.GetDataSet(ds);

                            object obj = new
                            {
                                rows = listApp
                            };
                            return GetResult(obj);
                        }
                    }
                    else
                    {
                        return GetErrorResult("系统不存该版本");
                    }

                }
            }
            return GetErrorResult("系统不存该版本");
        }

        /// <summary>
        /// 问题反馈
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        public HttpResponseMessage AddUserFeedback([FromBody]KingRequest request)
        {
            TB_UserFeedback application = JsonHelper.DecodeJson<TB_UserFeedback>(request.Data);
            UserFeedBackBLL userfeed = new UserFeedBackBLL();
            if (userfeed.InsertFeedBackInfo(application))
            {
                return ObjectToJson.GetResult(application);
            }
            else
            {
                return ObjectToJson.GetErrorResult("反馈信息插入失败");
            }
        }


        [HttpGet]
        public HttpResponseMessage GetNewVersion()
        {
            AppUpDate application = new AppUpDate();// JsonHelper.DecodeJson<AppUpDate>(request.data);
            application.AppID = "9426808e-da8e-488c-9827-b082c19b62a7";
            application.VersionNumber = "V1.0.0";
            if (application == null)
            {
                return GetErrorResult("当前信息为空");
            }
            if (application.AppID == null)
            {
                return GetErrorResult("AppID版本号为空");
            }
            if (application.VersionNumber == null)
            {
                return GetErrorResult("版本号为空");
            }

            // DataSet ds = appVersion.GetAppVersion(application.VersionType.ToString());
            TB_APPManagement app = appmangementbll.GetAPPManagement(application.AppID);
            if (app != null)
            {
                DataSet ds = appVersion.GetAppVersion(app.VersionID.ToString(), 2);
                if (ds == null || ds.Tables.Count < 1)
                {
                    return GetErrorResult("系统不存版本更新");
                }
                if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 0) //判断里面的书籍为空
                {
                    return GetErrorResult("系统不存版本更新");
                }

                Version v1 = new Version(ds.Tables[0].Rows[0]["VersionNumber"].ToString().Remove(0, 1));
                if (v1 != null)
                {
                    string version = "v0.0.0";
                    if (application.VersionNumber != null)
                    {
                        version = application.VersionNumber;
                    }

                    Version v2 = new Version(version.Remove(0, 1));
                    if (v1 == v2 || v1 < v2)
                    {
                        return ObjectToJson.GetResult("", "当前版本已是最新！");
                    }
                    else
                    {
                        List<TB_ApplicationVersion> listApp = appVersion.GetDataSet(ds);
                        object obj = new
                        {
                            rows = listApp
                        };
                        return GetResult(obj);
                    }
                }
                else
                {
                    return GetErrorResult("系统不存该版本");
                }
            }
            else
            {
                return GetErrorResult("系统不存该版本");
            }
        }

        private HttpResponseMessage GetErrorResult(string message)
        {
            object obj = new { Success = false, data = "", Message = message };
            return KingsunResponse.toJson(obj);
        }

        private HttpResponseMessage GetResult(object Data, string message = "")
        {
            object obj = new { Success = true, data = Data, Message = message };
            return KingsunResponse.toJson(obj);
        }

        //获取版本类型参数
        private int GetVersionType(string type)
        {
            int versionType = 2;
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    versionType = Convert.ToInt32(type);
                }
                return versionType;
            }
            catch (Exception)
            {
                return versionType = 0;
            }
        }



    }
}
