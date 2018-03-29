using System;
using System.Collections.Generic;
using System.Linq;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;
using System.Data;
using CAS.Entity.GJBEntity;
using System.Web;
using System.IO;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using OpenPlatform.Framework.FlowMonitor;
using Newtonsoft.Json;
using FxtCommon.Openplatform.Data;
using FxtCommon.Openplatform.GrpcService;
using FxtOpenClient.ClientService;
using CAS.Entity.FxtProject;
using FxtCenterService.Common;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SqlClient;
using FxtCenterService.DataAccess;
using System.Threading.Tasks;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 获取楼盘列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetSearchProjectListByKey(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
                search.Page = true;
            }
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string key = funinfo.Value<string>("key");
            bool isSearchInBuildingDoorplate = false;
            if (funinfo.Property("searchbuildingdoorplate") != null)
            {
                bool.TryParse(funinfo.Property("searchbuildingdoorplate").Value.ToString(), out isSearchInBuildingDoorplate);
            }
            var list = DatProjectBL.GetSearchProjectListByKey(search, company.parentshowdatacompanyid, search.CityId, key, isSearchInBuildingDoorplate);
            return list.ToJson();
        }
        /// <summary>
        /// 获取楼盘详细信息
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectDetailsByProjectid(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));


            //added by: dpc
            //使用RPC调用数据
            //try
            //{
            //    if (FxtClientService.IfUseRpc())
            //    {
            //        var request = new ProjectRequestParam()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                CompanyId = search.FxtCompanyId,
            //                BEncryptId = false,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            ProjectId = funinfo.Value<string>("projectid")
            //        };


            //        ProjectDetailsResponse response;

            //        FxtClientService.GetProjectDetailsByProjectid(request, out response);
            //        if (!response.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(response.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼盘详细信息成功");
            //        }

            //        return response.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}





            return DatProjectBL.GetProjectInfoDetailsByProjectid(search, company.parentshowdatacompanyid, search.CityId, projectid);
        }
        /// <summary>
        /// 获取自动估价楼盘详细信息
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectSimpleDetails(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            return DatProjectBL.GetProjectDetailsByProjectid(search, search.FxtCompanyId, search.CityId, projectid);
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectDropDownList(JObject funinfo, UserCheck company)
        {

            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string key = funinfo.Value<string>("key");

            //try
            //{
            //    if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //    {
            //        ProjectDropdownListResponse projectResponse;
            //        var requestParam = new ProjectRequestParam()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                CompanyId = search.FxtCompanyId,
            //                BEncryptId = false,
            //                SysTypeCode = search.SysTypeCode,
            //                ProductTypeCode = company.producttypecode
            //            },
            //            AreaId = search.AreaId,
            //            SearchKey = key

            //        };
            //        FxtClientService.GetProjectDropDownList(requestParam, out projectResponse);
            //        if (!projectResponse.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(projectResponse.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼盘列表成功");
            //        }

            //        return projectResponse.Projects.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}

            key = HttpUtility.UrlDecode(key);
            int items = 15;
            List<Dictionary<string, object>> list = DatProjectBL.GetProjectDropDownList(search, key, items);
            return list.ToJson();
        }
        /// <summary>
        /// 获取楼盘下拉列表MCAS
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectDropDownList_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.AreaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            search.SysTypeCode = company.parentproducttypecode; //1003036;
            int items = StringHelper.TryGetInt(funinfo.Value<string>("items"));
            items = items > 0 ? items : 15;
            string key = funinfo.Value<string>("key");
            string buildingname = funinfo.Value<string>("buildingname");
            int priceorderby = StringHelper.TryGetInt(funinfo.Value<string>("priceorderby"));
            string serialno = funinfo.Value<string>("serialno");
            key = HttpUtility.UrlDecode(key);
            Stopwatch swHuiDu = new Stopwatch();
            swHuiDu.Start();
            //使用RPC调用数据 20160816tring.IsNullOrEmpty(buildingname)
            //if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //{
            //    swHuiDu.Stop();
            //    int huiDuMill = Convert.ToInt32(swHuiDu.Elapsed.TotalMilliseconds);
            //    Stopwatch swRequest = new Stopwatch();
            //    swRequest.Start();
            //    ProjectDropdownListResponse projectDropdownListResponse;
            //    var request = new ProjectRequestParam()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            Page = true,
            //            PageIndex = search.PageIndex,
            //            PageRecords = items,
            //            CityId = search.CityId,
            //            OrderBy = string.IsNullOrEmpty(search.OrderBy) ? "" : search.OrderBy,
            //            CompanyId = search.FxtCompanyId,
            //            SysTypeCode = search.SysTypeCode,
            //            ProductTypeCode = company.producttypecode,
            //            ParentShowDataCompanyId = company.companyid    //查询公司ID
            //        },
            //        AreaId = search.AreaId,
            //        SearchKey = string.IsNullOrEmpty(key) ? "" : key,
            //        Buildingname = string.IsNullOrEmpty(buildingname) ? "" : buildingname,
            //        Serialno = string.IsNullOrEmpty(serialno) ? "" : serialno
            //    };

            //    FxtClientService.GetProjectDropDownListMcas(request, out projectDropdownListResponse);
            //    swRequest.Stop();
            //    int requestMill = Convert.ToInt32(swRequest.Elapsed.TotalMilliseconds);
            //    //日志记录
            //    Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
            //    {
            //        sqltime = projectDropdownListResponse.ResponseMsg.SqlMill,
            //        functionname = "projectdropdownlistmcas",
            //        code = serialno,
            //        time = "灰度发布" + (projectDropdownListResponse.ResponseMsg.Success ? "成功" : "失败") + "(判" + huiDuMill + ",请" + requestMill + ")",
            //        addtime = DateTime.Now,
            //        sqlconnetiontime = projectDropdownListResponse.ResponseMsg.ConnetionMill,
            //        sqlexecutetime = projectDropdownListResponse.ResponseMsg.ExecuteMill
            //    }));

            //    return projectDropdownListResponse.Projects.ToLowerJson();
            //}
            List<Dictionary<string, object>> list = DatProjectBL.GetProjectDropDownList_MCAS(search, key, buildingname, items, serialno, company.producttypecode, company.companyid, priceorderby);

            return list.ToJson();
        }


        /// <summary>
        /// 获取楼盘简易下拉列表MCAS(招行sdk专用,只返回楼盘表数据)
        /// zhoub 20161102
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectDropDownList_MCAS_SDK(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.AreaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            search.SysTypeCode = company.parentproducttypecode; //1003036;
            int items = StringHelper.TryGetInt(funinfo.Value<string>("items"));
            items = items > 0 ? items : 15;
            string key = funinfo.Value<string>("key");
            string buildingname = funinfo.Value<string>("buildingname");
            int priceorderby = StringHelper.TryGetInt(funinfo.Value<string>("priceorderby"));
            string serialno = funinfo.Value<string>("serialno");
            key = HttpUtility.UrlDecode(key);
            Stopwatch swHuiDu = new Stopwatch();
            swHuiDu.Start();
            //使用RPC调用数据 20160816tring.IsNullOrEmpty(buildingname)
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                swHuiDu.Stop();
                int huiDuMill = Convert.ToInt32(swHuiDu.Elapsed.TotalMilliseconds);
                Stopwatch swRequest = new Stopwatch();
                swRequest.Start();
                ProjectDropdownListSdkResponse projectDropdownListResponse;
                var request = new ProjectSdkRequestParam()
                {
                    SerachParam = new SearchParam()
                    {
                        Page = true,
                        PageIndex = search.PageIndex,
                        PageRecords = items,
                        CityId = search.CityId,
                        OrderBy = string.IsNullOrEmpty(search.OrderBy) ? "" : search.OrderBy,
                        CompanyId = search.FxtCompanyId,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //查询公司ID
                    },
                    AreaId = search.AreaId,
                    SearchKey = string.IsNullOrEmpty(key) ? "" : key,
                    Buildingname = string.IsNullOrEmpty(buildingname) ? "" : buildingname,
                    Serialno = string.IsNullOrEmpty(serialno) ? "" : serialno
                };

                FxtClientService.GetProjectDropDownListMcasSdk(request, out projectDropdownListResponse);
                swRequest.Stop();
                int requestMill = Convert.ToInt32(swRequest.Elapsed.TotalMilliseconds);
                //日志记录
                Task.Run(() => ExecuteTimeLogDA.Add09(new ExecuteTimeLog()
                {
                    sqltime = projectDropdownListResponse.ResponseMsg.SqlMill,
                    functionname = "projectdropdownlistmcassdk",
                    code = serialno,
                    time = "灰度发布" + (projectDropdownListResponse.ResponseMsg.Success ? "成功" : "失败") + "(判" + huiDuMill + ",请" + requestMill + ")",
                    addtime = DateTime.Now,
                    sqlconnetiontime = projectDropdownListResponse.ResponseMsg.ConnetionMill,
                    sqlexecutetime = projectDropdownListResponse.ResponseMsg.ExecuteMill
                }));

                return projectDropdownListResponse.Projects.ToLowerJson();
            }
            List<Dictionary<string, object>> list = DatProjectBL.GetProjectDropDownList_MCAS_SDK(search, key, buildingname, items, serialno, company.producttypecode, company.companyid, priceorderby);

            return list.ToJson();
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string key = funinfo.Value<string>("key");
            search.OrderBy = "ProjectId";
            int buildingTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));//主建筑物类型
            int purposecode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));//用途
            search.AreaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));//
            search.SubAreaId = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));//
            if (search.PageRecords == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
            }
            List<DATProject> plist = DatProjectBL.GetDATProjectList(search, key, search.AreaId, search.SubAreaId, buildingTypeCode, purposecode);
            return plist.ToJson();
        }

        /// <summary>
        /// 获取楼盘详细数据
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectInfoById(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));

            //1003003 24
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                ProjectInfoByIdResponse projectInfoResponse;
                var requestParam = new ProjectRequestParam()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CityId = search.CityId,
                        CompanyId = search.FxtCompanyId,
                        BEncryptId = false,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    ProjectId = funinfo.Value<string>("projectid")
                };
                FxtClientService.GetProjectInfoById(requestParam, out projectInfoResponse);
                return projectInfoResponse.ToLowerJson();
            }

            DATProject dat = DatProjectBL.GetProjectInfoById(search.CityId, projectid, search.FxtCompanyId, search.SysTypeCode);
            return dat.ToJson();
        }
        /// <summary>
        /// 获取楼盘图片
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectPhotoById(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));


            //added by: dpc
            //使用RPC调用数据
            //try
            //{
            //    if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //    {
            //        var request = new ProjectRequestParam()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                CityId = search.CityId,
            //                CompanyId = company.companyid,
            //                BEncryptId = false,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            ProjectId = funinfo.Value<string>("projectid")
            //        };


            //        ProjectPhotoByIdResponse photoResponse;

            //        FxtClientService.GetProjectPhotoById(request, out photoResponse);
            //        if (!photoResponse.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(photoResponse.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼盘照片列表成功");
            //        }

            //        return photoResponse.Photos.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}

            List<LNKPPhoto> photo = DatProjectBL.GetProjectPhotoById(search.CityId, projectid, search.FxtCompanyId, search.SysTypeCode);
            return photo.ToJson();
        }
        /// <summary>
        /// 获取楼盘图片forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectPhotoById_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingid = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            List<LNKPPhoto> photo = DatProjectBL.GetProjectPhotoById_MCAS(search.CityId, projectid, buildingid, search.FxtCompanyId, search.SysTypeCode);
            return photo.ToJson();
        }
        /// <summary>
        /// 价格走势
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetDATAvgPriceMonthList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.DateBegin = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("begindate"), DateTime.Now.AddMonths(-12));
            search.DateEnd = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("enddate"), DateTime.Now);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));



            //added by: dpc
            //使用RPC调用数据
            //try
            //{
            //    if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //    {
            //        AvgPriceMonthListResponse response;
            //        var requestParam = new ProjectRequestParam()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                CompanyId = search.FxtCompanyId,
            //                BEncryptId = false,
            //            },
            //            ProjectId = funinfo.Value<string>("projectid"),
            //            DateBegin = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("begindate"), DateTime.Now.AddMonths(-12)),
            //            DateEnd = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("enddate"), DateTime.Now)

            //        };
            //        FxtClientService.GetAvgPriceMonth(requestParam, out response);
            //        if (!response.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(response.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼盘价格成功");
            //        }



            //        return response.PriceResponses.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}



            DataSet ds = DATAvgPriceMonthBL.GetDATAvgPriceMonthList(search, projectid);

            if (ds == null || ds.Tables.Count <= 0)
            {
                return "";
            }
            return returnFlashJson(ds, 0, 0);
        }
        //NoFlash版
        [OverflowAttrbute(ApiType.Project)]
        public static string GetDATAvgPriceMonthList_NoFlash(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.DateBegin = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("begindate"), DateTime.Now.AddMonths(-12));
            search.DateEnd = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("enddate"), DateTime.Now);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DataSet ds = DATAvgPriceMonthBL.GetDATAvgPriceMonthList(search, projectid);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 楼盘案例
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectCase(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            search.DateBegin = funinfo.Value<string>("begindate");
            search.DateEnd = funinfo.Value<string>("enddate");
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));


            //added by: dpc
            //使用RPC调用数据
            //try
            //{
            //    if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //    {
            //        ProjectCaseListResponse response;
            //        var requestParam = new CaseRequest()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                CompanyId = company.companyid,
            //                BEncryptId = false,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            ProjectId = funinfo.Value<string>("projectid"),
            //            BeginDate = funinfo.Value<string>("begindate"),
            //            EndDate = funinfo.Value<string>("enddate")

            //        };
            //        FxtClientService.GetProjectCase(requestParam, out response);
            //        if (!response.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(response.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼盘案例成功");
            //        }



            //        return response.CaseResponses.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}



            DataTable dt = DatProjectBL.GetProjectCase(search, projectid, search.FxtCompanyId, search.CityId);
            return dt == null || dt.Rows.Count < 1 ? "" : dt.ToJson();
        }

        /// <summary>
        /// 楼盘案例forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectCase_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            search.DateBegin = funinfo.Value<string>("begindate");
            search.DateEnd = funinfo.Value<string>("enddate");
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DataTable dt = DatProjectBL.GetProjectCase_MCAS(search, projectid, search.FxtCompanyId, search.CityId);
            return dt == null || dt.Rows.Count < 1 ? "" : dt.ToJson();
        }


        /// <summary>
        /// 获取楼栋列表 
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingListByPid(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string key = funinfo.Value<string>("key");
            List<DATBuilding> list = DatBuildingBL.GetDATBuildingList(search, projectId, key);
            return list.ToJson();
        }
        /// <summary>
        /// 获取楼栋下拉列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingBaseInfoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int avgprice = StringHelper.TryGetInt(funinfo.Value<string>("avgprice"));

            //added by: dpc
            //使用RPC调用数据
            //try
            //{
            //    if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            //    {
            //        BuildingDropdownListResponse buildingResponse;
            //        var requestParam = new BuildingRequestParam()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                CompanyId = company.companyid,
            //                BEncryptId = false,
            //                SysTypeCode = search.SysTypeCode,
            //                ProductTypeCode = company.producttypecode
            //            },
            //            ProjectId = projectId.ToString(),
            //            AvgPrice = avgprice,

            //        };
            //        FxtClientService.GetBuildingDropdownList(requestParam, out buildingResponse);
            //        if (!buildingResponse.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(buildingResponse.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼栋列表成功");
            //        }



            //        return buildingResponse.Buildings.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}



            List<DATBuildingOrderBy> ds = DatBuildingBL.GetBuildingBaseInfoList(search, projectId, avgprice);
            List<DATBuildingOrderBy> result = new List<DATBuildingOrderBy>();
            result = OrderByHelper.OrderBy<DATBuildingOrderBy>(ds, "buildingname");
            return result.ToJson();
        }

        /// <summary>
        /// 获取楼栋下拉列表forCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingBaseInfoListList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int avgprice = StringHelper.TryGetInt(funinfo.Value<string>("avgprice"));

            //added by: dpc
            //使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                BuildingDropdownListResponse buildingResponse;
                var requestParam = new BuildingRequestParam()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CityId = search.CityId,
                        CompanyId = company.companyid,
                        BEncryptId = false,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    ProjectId = projectId.ToString(),
                    AvgPrice = avgprice,

                };
                FxtClientService.GetBuildingDropdownList(requestParam, out buildingResponse);
                return buildingResponse.Buildings.ToLowerJson();
            }

            DataSet ds = DatBuildingBL.GetBuildingBaseInfoListList(search, projectId, avgprice);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 获取楼栋下拉列表forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingBaseInfoList_MCAS(JObject funinfo, UserCheck company)
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int avgprice = StringHelper.TryGetInt(funinfo.Value<string>("avgprice"));
            string key = funinfo.Value<string>("key");
            string serialno = funinfo.Value<string>("serialno");
            Stopwatch swHuiDu = new Stopwatch();
            swHuiDu.Start();

            //zhoub 20160815 使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                swHuiDu.Stop();
                int huiDuMill = Convert.ToInt32(swHuiDu.Elapsed.TotalMilliseconds);
                Stopwatch swRequest = new Stopwatch();
                swRequest.Start();

                BuildingDropdownListResponse buildingResponse;
                var requestParam = new BuildingRequestParam()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CityId = search.CityId,
                        BEncryptId = true,
                        CompanyId = search.FxtCompanyId,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    ProjectId = projectId.ToString(),
                    AvgPrice = avgprice,
                    Key = string.IsNullOrEmpty(key) ? "" : key,
                    Serialno = string.IsNullOrEmpty(serialno) ? "" : serialno
                };
                FxtClientService.GetBuildingDropdownListMcas(requestParam, out buildingResponse);
                swRequest.Stop();
                int requestMill = Convert.ToInt32(swRequest.Elapsed.TotalMilliseconds);
                //日志记录
                new Task(DatProjectDA.AddExecuteTimeLog, new ExecuteTimeLog()
                {
                    sqltime = buildingResponse.ResponseMsg.SqlMill,
                    functionname = "buildingbaseinfolistmcas",
                    addtime = DateTime.Now,
                    code = serialno,
                    time = "灰度发布" + (buildingResponse.ResponseMsg.Success ? "成功" : "失败") + "(判" + huiDuMill + ",请" + requestMill + ")",
                    sqlconnetiontime = buildingResponse.ResponseMsg.ConnetionMill,
                    sqlexecutetime = buildingResponse.ResponseMsg.ExecuteMill
                }).Start();

                return buildingResponse.Buildings.ToLowerJson();
            }
            List<DATBuildingOrderBy> ds = DatBuildingBL.GetBuildingBaseInfoList_MCAS(search, projectId, avgprice, key, serialno, company.producttypecode, company.companyid);
            return ds.OrderBy(obj => obj.buildingname, new FxtCenterService.Common.CNComparer()).ToList().ToJson();
        }
        /// <summary>
        /// 获取楼栋下拉列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetAutoBuildingInfoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string key = funinfo.Value<string>("key");
            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
                search.Page = true;
            }
            List<DATBuilding> list = DatBuildingBL.GetAutoBuildingInfoList(search, projectId, key);
            var buildingList = list.Select(o => new
            {
                buildingid = o.buildingid,
                buildingname = o.buildingname,
                isevalue = o.isevalue,
                weight = o.weight,
                recordcount = o.recordcount,
                totalfloor = o.totalfloor,
                unitsnumber = o.unitsnumber,
                totalnumber = o.totalnumber,
                projectid = o.projectid,
                doorplate = o.doorplate
            });
            return buildingList.ToJson();
        }

        /// <summary>
        /// 获取楼栋单元列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseUnitList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            DataSet ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "unitno", search.FxtCompanyId, search.SysTypeCode);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取楼栋楼层列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseFloorList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));


            DataSet ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "floorno", search.FxtCompanyId, search.SysTypeCode);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取楼层列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseNoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            string key = funinfo.Value<string>("key");
            string dailyReportFullPath = GetFilePath();

            //added by: dpc
            //使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                var request = new HouseRequest()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CompanyId = search.FxtCompanyId,
                        CityId = search.CityId,
                        SysTypeCode = search.SysTypeCode,
                        BEncryptId = false,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    BuildingId = buildingId.ToString(),
                    FloorNo = key
                };

                FloorHouseNoListResponse floorHouseResponse;
                FxtClientService.GetFloorHouseNoList(request, out floorHouseResponse);
                return floorHouseResponse.FloorHouses.Select(x => new { floorno = x.FloorNo, housecnt = x.HouseNoCount }).ToList().ToJson();
            }
            key = HttpUtility.UrlDecode(key);
            DataSet ds = DatHouseBL.GetAutoFloorNoList(search, buildingId, key);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 获取楼层列表forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseNoList_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");
            string serialno = funinfo.Value<string>("serialno");
            Stopwatch swHuiDu = new Stopwatch();
            swHuiDu.Start();

            //使用RPC调用数据 zhoub 20160816
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                LogHelper.Info("1");
                swHuiDu.Stop();
                int huiDuMill = Convert.ToInt32(swHuiDu.Elapsed.TotalMilliseconds);
                Stopwatch swRequest = new Stopwatch();
                swRequest.Start();

                var request = new HouseRequest()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CompanyId = search.FxtCompanyId,
                        CityId = search.CityId,
                        SysTypeCode = search.SysTypeCode,
                        BEncryptId = true,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    BuildingId = buildingId.ToString(),
                    Key = string.IsNullOrEmpty(key) ? "" : key,
                    Serialno = string.IsNullOrEmpty(serialno) ? "" : serialno
                };

                FloorHouseNoListResponse floorHouseResponse;
                FxtClientService.GetFloorHouseNoListMcas(request, out floorHouseResponse);
                swRequest.Stop();
                int requestMill = Convert.ToInt32(swRequest.Elapsed.TotalMilliseconds);
                //日志记录
                new Task(DatProjectDA.AddExecuteTimeLog, new ExecuteTimeLog()
                {
                    sqltime = floorHouseResponse.ResponseMsg.SqlMill,
                    functionname = "housefloorlistmcas",
                    addtime = DateTime.Now,
                    code = serialno,
                    time = "灰度发布" + (floorHouseResponse.ResponseMsg.Success ? "成功" : "失败") + "(判" + huiDuMill + ",请" + requestMill + ")",
                    sqlconnetiontime = floorHouseResponse.ResponseMsg.ConnetionMill,
                    sqlexecutetime = floorHouseResponse.ResponseMsg.ExecuteMill
                }).Start();

                return floorHouseResponse.FloorHouses.Select(x => new { floorno = x.FloorNo, housecnt = x.HouseNoCount }).ToList().ToJson();

            }

            key = HttpUtility.UrlDecode(key);
            DataSet ds = DatHouseBL.GetAutoFloorNoList_MCAS(search, buildingId, key, company.producttypecode, company.companyid, serialno);
            if ( ds == null || ds.Tables.Count<=0)
            {
                return "";
            }
            else
            {
                #region 排序操作
                DataView dv;
                DataTable dtCopy;
                dv = ds.Tables[0].DefaultView;
                dv.Sort = "FloorNo asc";
                dtCopy = dv.ToTable();
                #endregion
                return dtCopy.ToJson();
            }
        }

        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetHouseDropDownList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));

            //added by: dpc
            //使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                var request = new HouseRequest()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CityId = search.CityId,
                        CompanyId = search.FxtCompanyId,
                        BEncryptId = false,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    BuildingId = funinfo.Value<string>("buildingid"),
                    FloorNo = funinfo.Value<string>("floorno")
                };


                HouseDropdownListResponse houseResponse;

                FxtClientService.GetHouseDropdownList(request, out houseResponse);

                return houseResponse.Houses.Select(x => new
                {
                    houseid = x.HouseId,
                    housename = x.HouseName,
                    unitno = x.UnitNo,
                    buildarea = x.BuildArea,
                    isevalue = x.IsEvalue,
                    subhousetype = x.SubHouseType,
                    subhousearea = x.SubHouseArea,
                    floorno = x.FloorNo,
                    frontcode = x.FrontCode,
                    sightcode = x.SightCode,
                    purposecode = x.PurposeCode,
                }).ToList().ToJson();
            }

            List<DATHouseOrderBy> result = new List<DATHouseOrderBy>();
            result = OrderByHelper.OrderBy<DATHouseOrderBy>(DatHouseBL.GetHouseDropDownList(search, buildingId, floorNo), "housename");
            return result.ToJson();
        }

        /// <summary>
        /// 获取房号下拉列表forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetHouseDropDownList_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            string key = funinfo.Value<string>("key");
            string serialno = funinfo.Value<string>("serialno");
            Stopwatch swHuiDu = new Stopwatch();
            swHuiDu.Start();

            // 使用RPC调用数据 zhoub 20160816
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                swHuiDu.Stop();
                int huiDuMill = Convert.ToInt32(swHuiDu.Elapsed.TotalMilliseconds);
                Stopwatch swRequest = new Stopwatch();
                swRequest.Start();
                var request = new HouseRequest()
                {
                    SerachParam = new SearchParam()
                    {
                        PageIndex = 0,
                        PageRecords = 15,
                        CityId = search.CityId,
                        CompanyId = search.FxtCompanyId,
                        BEncryptId = true,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    BuildingId = buildingId.ToString(),
                    FloorNo = floorNo.ToString(),
                    Key = string.IsNullOrEmpty(key) ? "" : key,
                    Serialno = string.IsNullOrEmpty(serialno) ? "" : serialno
                };

                HouseDropdownListResponse houseResponse;

                FxtClientService.GetHouseDropdownListMcas(request, out houseResponse);
                swRequest.Stop();
                int requestMill = Convert.ToInt32(swRequest.Elapsed.TotalMilliseconds);

                //日志记录
                new Task(DatProjectDA.AddExecuteTimeLog, new ExecuteTimeLog()
                {
                    sqltime = houseResponse.ResponseMsg.SqlMill,
                    functionname = "housedropdownlistmcas",
                    addtime = DateTime.Now,
                    code = serialno,
                    time = "灰度发布" + (houseResponse.ResponseMsg.Success ? "成功" : "失败") + "(判" + huiDuMill + ",请" + requestMill + ")",
                    sqlconnetiontime = houseResponse.ResponseMsg.ConnetionMill,
                    sqlexecutetime = houseResponse.ResponseMsg.ExecuteMill
                }).Start();

                List<DATHouseOrderBy> list = houseResponse.Houses.Select(x => new DATHouseOrderBy
                {
                    houseid = Convert.ToInt32(x.HouseId),
                    housename = x.HouseName,
                    unitno = x.UnitNo,
                    buildarea = Convert.ToDecimal(x.BuildArea),
                    isevalue = x.IsEvalue,
                    subhousetype = x.SubHouseType,
                    subhousearea = Convert.ToDecimal(x.SubHouseArea),
                    floorno = x.FloorNo,
                    frontcode = x.FrontCode,
                    sightcode = x.SightCode,
                    purposecode = x.PurposeCode
                }).ToList<DATHouseOrderBy>();

                if (string.IsNullOrEmpty(key))
                {
                    return OrderByHelper.OrderBy<DATHouseOrderBy>(list, "housename").ToJson();
                }
                else
                {
                    return list.ToJson();
                }
            }
            List<DATHouseOrderBy> result = DatHouseBL.GetHouseDropDownList_MCAS(search, buildingId, floorNo, key, serialno, company.producttypecode, company.companyid);
            return result.OrderBy(obj => obj.housename, new FxtCenterService.Common.CNComparer()).ToList().ToJson();
        }
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetAutoHouseList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            //int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            string fn = funinfo.Value<string>("floorno");
            int? floorNo = null;
            if (!string.IsNullOrEmpty(fn))
            {
                floorNo = StringHelper.TryGetInt(fn);
            }
            string key = funinfo.Value<string>("key");
            List<DATHouse> list = DatHouseBL.GetAutoHouseListList(search, buildingId, floorNo, key);
            var houselist = list.Select(o => new
            {
                houseid = o.houseid,
                housename = o.housename,
                buildarea = o.buildarea,
                isevalue = o.isevalue,
                weight = o.weight,
                builddate = o.builddate,
                purposecode = o.purposecode,
                purposecodename = o.purposecodename,
                housetypecode = o.housetypecode,
                housetypecodename = o.housetypecodename,
                recordcount = o.recordcount,
                unitno = o.unitno,
                subhousetype = o.subhousetype,
                subhousearea = o.subhousearea,
                housepropertynumber = o.propertynumber
            });
            return houselist.ToJson();
        }

        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseList(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string projectname = funinfo.Value<string>("projectname");
            int minBuildingArea = StringHelper.TryGetInt(
                                    funinfo.Value<string>("minbuildingarea"));
            int maxBuildingArea = StringHelper.TryGetInt(
                                    funinfo.Value<string>("maxbuildingarea"));
            int minFloorNumber = StringHelper.TryGetInt(
                                    funinfo.Value<string>("minfloornumber"));
            int maxFloorNumber = StringHelper.TryGetInt(
                                    funinfo.Value<string>("maxfloornumber"));
            string address = funinfo.Value<string>("address");

            int caseTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("casetypecode"));

            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subareaid = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));

            decimal minUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("minunitprice"));
            decimal maxUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("maxunitprice"));

            DateTime startCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("startcasedate"));
            DateTime endCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("endcasedate"));

            int structurecode = StringHelper.TryGetInt(funinfo.Value<string>("structurecode"));
            string structurecodename = funinfo.Value<string>("structurecodename");
            int iselevator = StringHelper.TryGetInt(funinfo.Value<string>("iselevator"));

            List<Dat_Case> list = null;
            if (string.IsNullOrEmpty(projectname.Trim()))   //必填
            {
                list = new List<Dat_Case>();
                result = list.ToJson();
            }
            else
            {
                //默认只取15条记录
                if (search.PageIndex == 0)
                {
                    search.Page = true;
                    search.PageIndex = 1;
                    search.PageRecords = 15;
                }

                //zhoub 20160510
                //使用RPC调用数据
                if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
                {
                    CaseListResponse caseListResponse;
                    var request = new CaseRequest()
                    {
                        SerachParam = new SearchParam()
                        {
                            Page = search.Page,
                            PageIndex = search.PageIndex,
                            PageRecords = search.PageRecords,
                            CityId = search.CityId,
                            OrderBy = string.IsNullOrEmpty(search.OrderBy) ? "" : search.OrderBy,
                            CompanyId = search.FxtCompanyId,
                            SysTypeCode = search.SysTypeCode,
                            ProductTypeCode = company.producttypecode,
                            ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                        },
                        ProjectName = string.IsNullOrEmpty(projectname) ? "" : projectname,
                        MinBuildingArea = Convert.ToDouble(minBuildingArea),
                        MaxBuildingArea = Convert.ToDouble(maxBuildingArea),
                        MinFloorNumber = minFloorNumber,
                        MaxFloorNumber = maxFloorNumber,
                        MinUnitPrice = Convert.ToDouble(minUnitPrice),
                        MaxUnitPrice = Convert.ToDouble(maxUnitPrice),
                        Address = (string.IsNullOrEmpty(address) ? "" : address),
                        CaseTypeCode = caseTypeCode,
                        AreaId = areaid,
                        SubAreaId = subareaid,
                        CompanyCode = company.companycode,
                        StartCaseDate = startCaseDate.ToString("yyyy/MM/dd"),
                        EndCaseDate = endCaseDate.ToString("yyyy/MM/dd"),
                        StructureCode = structurecode,
                        Iselevator = iselevator,
                        StructureCodeName = string.IsNullOrEmpty(structurecodename) ? "" : structurecodename
                    };

                    FxtClientService.GetCaseList(request, out caseListResponse);
                    return caseListResponse.Cases.ToLowerJson();
                }


                //敦化特殊处理
                if (company.companycode.ToLower() == "dhhy")
                {
                    var listResult = DATCaseBL.GetDATCaseListByCalculateForSpecial(search, projectname, minBuildingArea, maxBuildingArea, minFloorNumber, maxFloorNumber, minUnitPrice, maxUnitPrice, address, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator);
                    result = listResult.ToJson();
                }
                else
                {
                    list = DATCaseBL.GetDATCaseListByCalculate(search, projectname, minBuildingArea, maxBuildingArea, minFloorNumber, maxFloorNumber, minUnitPrice, maxUnitPrice, address, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);
                    result = list.ToJson();
                }
            }
            return result;
        }
        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetProjectCaseCount(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string purpose = funinfo.Value<string>("purpose");//物业类型
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//物业面积                    
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));//总楼层数

            string begindate = funinfo.Value<string>("startdate");
            string lastdate = funinfo.Value<string>("enddate");
            if (string.IsNullOrEmpty(begindate))
                begindate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(lastdate))
                lastdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime startdate = StringHelper.TryGetDateTime(begindate);
            DateTime enddate = StringHelper.TryGetDateTime(lastdate);

            int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积类型
            int buildingtypecode = CodeHelper.GetBuildingTypeCode(totalfloor);//楼栋类型
            int purposetypecode = 0;
            if (purpose != "普通住宅") purposetypecode = 1002027;
            else
            {
                SYSCode code = SYSCodeBL.GetCode(1002, purpose);
                purposetypecode = (code != null && code.code != null) ? code.code.Value : 0;//物业类型 
            }

            int cnt = DATCaseBL.GetProjectCaseCount(search.FxtCompanyId, search.CityId, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate, search.SysTypeCode);
            var casecount = new { casecount = cnt };
            return casecount.ToJson();
        }
        /// <summary>
        /// 根据案例ID获取案例列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListByCaseIds(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string projectname = funinfo.Value<string>("projectname");
            string caseIds = funinfo.Value<string>("caseids");

            //zhoub 20160510
            //使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                CaseListByCaseIdsResponse caseListByCaseIdsResponse;
                var request = new CaseRequest()
                {
                    SerachParam = new SearchParam()
                    {
                        Page = false,
                        CityId = search.CityId,
                        OrderBy = string.IsNullOrEmpty(search.OrderBy) ? "" : search.OrderBy,
                        CompanyId = search.FxtCompanyId,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    ProjectName = string.IsNullOrEmpty(projectname) ? "" : projectname,
                    CaseIds = caseIds,
                    CompanyCode = company.companycode
                };

                FxtClientService.GetCaseListByCaseIds(request, out caseListByCaseIdsResponse);
                return caseListByCaseIdsResponse.Cases.ToLowerJson();
            }


            if (string.IsNullOrEmpty(caseIds))
            {
                if (company.companycode == "dhhy")
                {
                    GetDATCaseListForSpecial(search, projectname, out result);
                }
                else
                {
                    GetDATCaseList(search, projectname, out result);
                }
            }
            else
            {
                int[] CaseIdArray = caseIds.Split(',').Select(StringHelper.TryGetInt).ToArray();
                if (company.companycode == "dhhy")
                {
                    List<Dat_Case_Dhhy> list = DATCaseBL.GetDATCaseListForSpecial(search, "", string.Empty, CaseIdArray);
                    result = list.ToJson();
                }
                else
                {
                    List<Dat_Case> list = DATCaseBL.GetDATCaseList(search, "", string.Empty, CaseIdArray);
                    result = list.ToJson();

                }
            }
            return result;
        }

        /// <summary>
        /// 新增案例 caoq 2014-4-28 
        /// (招行 机构回价价格、预评价格入库)
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string AddCaseInfo(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;

            string casestr = funinfo.Value<string>("caselist");
            List<DATCase> caselist = (!string.IsNullOrEmpty(casestr)) ? JSONHelper.JSONStringToList<DATCase>(casestr) : null;

            int actionResult = 0;
            //循环添加案例，因案例量不大，先循环添加
            foreach (DATCase item in caselist)
            {
                //物业类型
                SYSCode code = SYSCodeBL.GetCode(1002, item.purpose);
                int purposetype = (code != null && code.code != null) ? code.code.Value : 0;//物业类型 

                item.fxtcompanyid = search.FxtCompanyId;
                item.companyid = search.FxtCompanyId;
                item.purposecode = purposetype;
                if (DATCaseBL.Add(item) > 0)
                    actionResult++;
            }
            //目前数据中心返回值returntype均为1
            return actionResult.ToString();
        }
        /// <summary>
        /// 导入楼盘,楼栋,房号信息
        /// ("无纸化住宅物业信息采集系统"入库调用)
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string ImportProjectAndBuildingAndHouse(JObject funinfo, UserCheck company)
        {
            try
            {
                //BaseBL.BeginBaseDATransaction();
                //company.companyid = 365;
                int typecode = company.producttypecode == 1003036 ? 1003036 : 1003002;
                string result = "{{\"projectid\":{0},\"message\":\"{1}\",\"buildingids\":{2}}}";
                string data = funinfo.Value<string>("data");

                //获取楼盘对象JObject
                JObject jobj = JObject.Parse(data);
                List<object> buildings = new List<object>();
                int projectId = 0;
                int areaId = 0;
                int cityId = 0;
                string projectName = "";

                //获取楼盘城市
                if (!string.IsNullOrEmpty(jobj.Value<string>("cityid")) || jobj.Value<string>("cityid") != "null")
                {
                    cityId = Convert.ToInt32(jobj.Value<string>("cityid"));
                }
                //获取楼盘行政区
                if (!string.IsNullOrEmpty(jobj.Value<string>("areaid")) || jobj.Value<string>("areaid") != "null")
                {
                    areaId = Convert.ToInt32(jobj.Value<string>("areaid"));
                }
                //获取楼盘名称
                projectName = HttpUtility.UrlDecode(Convert.ToString(jobj.Value<string>("projectname"))).Replace("%20", "+"); ;

                //*************************验证数据***************************************//
                if (cityId < 1)
                {
                    return string.Format(result, 0, "请填写楼盘城市");
                }
                if (areaId < 1)
                {
                    return string.Format(result, 0, "请填写楼盘行政区");
                }
                SYSArea area = SYSAreaBL.GetSYSArea(areaId);
                if (area == null)
                {
                    return string.Format(result, 0, "请填写楼盘行政区名称");
                }
                if (string.IsNullOrEmpty(projectName))
                {
                    return string.Format(result, 0, "请填写楼盘名称");
                }
                string[] searchNames = new string[] { projectName, projectName + "(" + area.areaname + ")" };
                CityTable tableInfo = CityTableBL.GetCityTable(cityId);
                if (tableInfo == null)
                {
                    return string.Format(result, 0, "城市不存在");
                }
                //如果传过来数据库对应的projectId
                if (!string.IsNullOrEmpty(jobj.Value<string>("fxtprojectid")) || jobj.Value<string>("fxtprojectid") != "null")
                {
                    projectId = Convert.ToInt32(jobj.Value<string>("fxtprojectid"));
                }
                //没传过来数据库对应的projectId,则自己查询
                DATProject project = new DATProject();
                if (projectId == 0)
                {
                    List<DATProject> _list = DatProjectBL.GetProjectInfoByNames(cityId, areaId, company.companyid, searchNames, typecode);
                    project = _list == null || _list.Count() < 1 ? new DATProject() : _list[0];
                }
                else
                {
                    project = DatProjectBL.GetProjectInfoByProjectId(cityId, company.companyid, projectId, typecode);
                    if (project == null)
                    {
                        projectId = 0;
                        List<DATProject> _list = DatProjectBL.GetProjectInfoByNames(cityId, areaId, company.companyid, searchNames, typecode);
                        project = _list == null || _list.Count() < 1 ? new DATProject() : _list[0];
                    }
                }
                if (project != null)
                {
                    projectId = project.projectid;
                }
                bool projectIsAdd = false;
                #region 楼盘数据操作
                DATProject nowProject = new DATProject();
                List<string> upProjColumn = new List<string>();
                foreach (var _jobj in jobj)
                {
                    string key = _jobj.Key;
                    if (key.ToLower().Equals("projectid") || key.ToLower().Equals("buildinglist"))
                    {
                        continue;
                    }

                    //如果字段为备注
                    if (key.ToLower().Equals("detail") && project != null)
                    {
                        string detail = Convert.ToString(_jobj.Value.Value<JValue>().Value).Replace("%20", "+");
                        detail = HttpUtility.UrlDecode(detail);
                        if (detail != null)
                        {
                            nowProject.detail = (project.detail == null ? "" : project.detail) + detail;
                            upProjColumn.Add(key.ToLower());//添加数据表需要修改的列
                        }
                        continue;
                    }
                    //获取实体属性对象
                    var property = nowProject.GetType().GetProperties().Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                    if (property == null)
                    {
                        continue;
                    }
                    if (!key.ToLower().Contains("companyid"))// && !key.ToLower().Contains("totalnum") && !key.ToLower().Contains("buildingnum"))
                    {
                        upProjColumn.Add(key.ToLower());//添加数据表需要修改的列
                    }
                    object _value = ImportProjectAndBuildingAndHouse_valueType(property.PropertyType, _jobj.Value.Value<JValue>().Value, isDecode: true);
                    property.SetValue(nowProject, _value, null);
                }
                nowProject.iscomplete = null;
                nowProject.isevalue = null;
                nowProject.projectid = 0;
                nowProject.fxt_companyid = company.companyid;
                nowProject.fxtcompanyid = company.companyid;
                nowProject.cityid = cityId;
                nowProject.valid = 1;
                LogHelper.Info("开始添加楼盘");

                //如果此楼盘是新增数据
                if (projectId == 0)
                {
                    projectIsAdd = true;
                    //********新增楼盘实体*********//
                    IList<DATProject> _projList = DatProjectBL.GetProjectInfoByNames(cityId, areaId, company.companyid, new string[] { projectName }, typecode);
                    if (_projList != null && _projList.Count() > 0)
                    {
                        return string.Format(result, 0, "新增楼盘失败，楼盘名称已存在");
                        //nowProject.projectname = projectName + "(" + area.areaname + ")";
                    }
                    #region (楼盘数据插入)
                    nowProject.createtime = DateTime.Now;
                    projectId = DatProjectBL.Add(nowProject, tableInfo.ProjectTable);
                    if (projectId < 1)
                    {
                        return string.Format(result, 0, "新增楼盘失败");
                    }
                    #endregion
                }
                else
                {
                    //********修改楼盘实体***********//
                    #region(楼盘数据修改)
                    IList<DATProject> _projList = DatProjectBL.GetProjectInfoByNames(cityId, areaId, company.companyid, new string[] { projectName }, typecode);
                    if (_projList != null && _projList.Count() > 0 && _projList.FirstOrDefault().projectid != projectId)
                    {
                        return string.Format(result, 0, "修改楼盘失败，楼盘名称已存在");
                    }
                    nowProject.projectid = projectId;
                    int _projResult = 0;
                    //不是自己的信息&&自己不是25(插入子表)
                    if (project.fxtcompanyid != company.companyid && company.companyid != DatProjectBL.FXTCOMPANYID)
                    {
                        nowProject.updatedatetime = DateTime.Now;
                        nowProject.savedatetime = DateTime.Now;
                        _projResult = DatProjectBL.AddSub(nowProject, tableInfo.ProjectTable);
                    }
                    else//是自己的||自己是25
                    {
                        upProjColumn.Remove("creator");
                        nowProject.SetAvailableFields(upProjColumn.ToArray());//设置要更新的字段
                        nowProject.fxt_companyid = project.fxt_companyid;
                        nowProject.fxtcompanyid = project.fxtcompanyid;
                        nowProject.createtime = DateTime.Now;
                        //在主表
                        if (project.fxt_companyid == 0)
                        {
                            _projResult = DatProjectBL.Update(nowProject, tableInfo.ProjectTable);
                        }
                        else//在子表
                        {
                            _projResult = DatProjectBL.UpdateSub(nowProject, tableInfo.ProjectTable);
                        }
                    }
                    if (_projResult < 1)
                    {
                        return string.Format(result, 0, "修改楼盘失败");
                    }
                    #endregion
                }
                #endregion

                #region (设置楼盘关联公司)
                //*******新增楼盘关联公司********//
                List<LNKPCompany> lnkComList = LNKPCompanyBL.GetLNKPCompanyByProjId(projectId, cityId);
                JArray companyJobj = jobj["companylist"] as JArray;
                foreach (var com in companyJobj)
                {
                    string companyName = com.Value<string>("companyname");
                    if (string.IsNullOrEmpty(companyName))
                    {
                        continue;
                    }
                    int companyTypeCode = com.Value<int>("companytype");
                    //如果公司名称不存在则新增
                    bool companyIsAdd = false;
                    DATCompany existsCom = DATCompanyBL.GetByName(companyName);
                    if (existsCom == null)
                    {
                        companyIsAdd = true;
                        existsCom = new DATCompany();
                        existsCom.chinesename = companyName;
                        existsCom.companytypecode = companyTypeCode;
                        existsCom.cityid = cityId;
                        existsCom.valid = 1;
                        existsCom.createdate = DateTime.Now;
                        existsCom.companyid = company.companyid;
                        int companyId = DATCompanyBL.Add(existsCom);
                        if (companyId < 1)
                        {
                            return string.Format(result, 0, "新增公司失败,公司名:" + companyName);
                        }
                        existsCom.companyid = companyId;
                    }
                    LNKPCompany lnkCom = new LNKPCompany { cityid = cityId, companyid = existsCom.companyid, companytype = companyTypeCode, projectid = projectId };
                    LNKPCompany uplnkCom = lnkComList.Where(obj => obj.companyid == existsCom.companyid && obj.projectid == projectId && obj.cityid == cityId && obj.companytype == companyTypeCode).FirstOrDefault();
                    if (projectIsAdd || companyIsAdd || uplnkCom == null)//楼盘为新增||公司为新增--(新增信息)
                    {
                        if (LNKPCompanyBL.Add(lnkCom) < 1)
                        {
                            return string.Format(result, 0, "新增楼盘关联公司失败");
                        }
                    }
                    else
                    {
                        if (LNKPCompanyBL.Update(lnkCom) < 1)
                        {
                            return string.Format(result, 0, "修改楼盘关联公司失败");
                        }

                    }
                }
                #endregion
                #region (设置楼盘配套)
                //*******新增配套信息*********//
                JArray appendageJobj = jobj["appendage"] as JArray;
                List<LNKPAppendage> lnkpaList = JSONHelper.JSONStringToList<LNKPAppendage>(appendageJobj.ToJson());
                List<LNKPAppendage> existsLnkpaList = LNKPAppendageBL.GetPAppendageByProjectId(projectId, cityId);//获取数据库已经存在的配套信息
                List<DatSchool> existsSchoolList = DatSchoolBL.GetSchoolByCityId(cityId);//获取数据库已经存在的信息
                List<DatPeitao> existsPeitaoList = DatPeitaoBL.GetPeitaoByCityId(cityId);//获取数据库已经存在的配套信息
                List<CAS.Entity.SurveyDBEntity.SYSCode> lpCodes = SYSCodeBL.GetSYSCodeList(2008);//LNKPAppendage表配套类型
                List<CAS.Entity.SurveyDBEntity.SYSCode> ptCodes = SYSCodeBL.GetSYSCodeList(1055);//peitao表配套类型
                if (lnkpaList != null && lnkpaList.Count > 0)
                {
                    foreach (LNKPAppendage obj in lnkpaList)
                    {
                        var lpcode = lpCodes.Where(m => m.code == obj.appendagecode).FirstOrDefault();
                        if (lpcode.code == 2008006)
                        {
                            //存入school表（学校）
                            DatSchool school = new DatSchool();
                            school.schoolname = obj.p_aname;
                            school.cityid = cityId;
                            school.areaid = areaId;
                            school.address = obj.address;
                            school.x = obj.x;
                            school.y = obj.y;
                            school.valid = 1;
                            school.typecode = 0;
                            DatSchool existsSchool = existsSchoolList == null ? null : existsSchoolList.Where(m => m.cityid == cityId && m.schoolname == obj.p_aname && m.valid == 1).FirstOrDefault();
                            if (existsSchool == null)
                            {
                                //新增
                                obj.schoolid = DatSchoolBL.Add(school);
                            }
                            else
                            {
                                school.id = existsSchool.id;
                                DatSchoolBL.Update(school);
                                obj.schoolid = school.id;
                            }
                        }
                        else
                        {
                            //配套类型相同，存入peitao表
                            var ptcode = ptCodes == null ? null : ptCodes.Where(m => m.codename == lpcode.codename).FirstOrDefault();
                            if (ptcode != null)
                            {
                                //存入peitao表
                                DatPeitao peitao = new DatPeitao();
                                peitao.PeiTaoName = obj.p_aname;
                                peitao.cityid = cityId;
                                peitao.areaid = areaId;
                                peitao.Address = obj.address;
                                peitao.x = obj.x;
                                peitao.y = obj.y;
                                peitao.valid = 1;
                                DatPeitao existsSchool = existsPeitaoList == null ? null : existsPeitaoList.Where(m => m.cityid == cityId && m.PeiTaoName == obj.p_aname && m.valid == 1).FirstOrDefault();
                                if (existsSchool == null)
                                {
                                    //新增
                                    obj.peitaoid = DatPeitaoBL.Add(peitao);
                                }
                                else
                                {
                                    peitao.id = existsSchool.id;
                                    DatPeitaoBL.Update(peitao);
                                    obj.peitaoid = peitao.id;
                                }
                            }
                        }

                        obj.projectid = projectId;
                        obj.cityid = cityId;
                        LNKPAppendage existsObj = existsLnkpaList == null ? null : existsLnkpaList.Where(_obj => _obj.projectid == projectId && _obj.cityid == cityId && _obj.appendagecode == obj.appendagecode && _obj.distance == obj.distance).FirstOrDefault();
                        if (projectIsAdd || existsObj == null)
                        {
                            if (LNKPAppendageBL.Add(obj) < 1)
                            {
                                return string.Format(result, 0, "新增楼盘配套失败");
                            }
                        }
                        else
                        {
                            obj.id = existsObj.id;
                            if (LNKPAppendageBL.Update(obj) < 1)
                            {
                                return string.Format(result, 0, "修改楼盘配套失败");
                            }
                        }
                    }

                }
                #endregion

                #region (楼栋+房号数据更新)
                JArray buildinglistJobj = jobj["buildinglist"] as JArray;
                foreach (var arry in buildinglistJobj)
                {
                    JObject buiJobj = arry as JObject;
                    if (buiJobj != null)
                    {
                        List<string> upBuilColumn = new List<string>();
                        DATBuilding nowBuilding = new DATBuilding();
                        #region(封装实体)
                        foreach (var _prop in buiJobj)
                        {
                            string key = _prop.Key;
                            if (key.ToLower().Equals("buildingid") || key.ToLower().Equals("houselist"))
                            {
                                continue;
                            }
                            var property = nowBuilding.GetType().GetProperties().Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                            if (property == null)
                            {
                                continue;
                            }
                            if (!key.ToLower().Contains("companyid"))
                            {
                                upBuilColumn.Add(key.ToLower());
                            }
                            object _value = ImportProjectAndBuildingAndHouse_valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                            property.SetValue(nowBuilding, _value, null);
                        }
                        #endregion
                        if (nowBuilding.x.HasValue && nowBuilding.x == 0)
                        {
                            nowBuilding.x = null;
                        }
                        if (nowBuilding.y.HasValue && nowBuilding.y == 0)
                        {
                            nowBuilding.y = null;
                        }
                        nowBuilding.buildingid = 0;
                        nowBuilding.cityid = cityId;
                        nowBuilding.fxtcompanyid = company.companyid;
                        nowBuilding.fxt_companyid = company.companyid;
                        nowBuilding.projectid = projectId;
                        nowBuilding.valid = 1;
                        if (string.IsNullOrEmpty(nowBuilding.buildingname))
                        {
                            return string.Format(result, 0, "新增楼栋失败,楼栋名不能为空");
                        }
                        int buildingId = 0;
                        //如果传过来数据库对应的buildingId
                        if (!string.IsNullOrEmpty(jobj.Value<string>("fxtbuildingid")) || jobj.Value<string>("fxtbuildingid") != "null")
                        {
                            buildingId = Convert.ToInt32(jobj.Value<string>("fxtbuildingid"));
                        }
                        //从数据库获取存在的楼栋信息
                        DATBuilding building = null;
                        if (!projectIsAdd)//楼盘不是新增的
                        {
                            if (buildingId == 0)//没有传buildingId过来(查询楼栋)
                            {
                                building = DatBuildingBL.GetBuildingByName(cityId, projectId, company.companyid, nowBuilding.buildingname, typecode);
                            }
                            else
                            {
                                building = DatBuildingBL.GetBuildingById(cityId, projectId, company.companyid, buildingId);
                                if (building == null)
                                {
                                    buildingId = 0;
                                    building = DatBuildingBL.GetBuildingByName(cityId, projectId, company.companyid, nowBuilding.buildingname, typecode);
                                }
                            }
                            if (building != null)
                            {
                                buildingId = building.buildingid;
                            }
                        }
                        else
                        {
                            buildingId = 0;
                        }
                        //开始对楼栋进行数据库更新
                        bool buildingIsAdd = false;
                        if (buildingId == 0)//数据库不存在(新增)
                        {
                            buildingIsAdd = true;
                            nowBuilding.createtime = DateTime.Now;
                            buildingId = DatBuildingBL.Add(nowBuilding, tableInfo.BuildingTable);
                            //添加返回楼栋id
                            int bid = buiJobj.Value<int>("buildingid");
                            buildings.Add(new { fxtbuildingid = buildingId, buildingid = bid });
                            if (buildingId < 1)
                            {
                                return string.Format(result, 0, "新增楼栋失败");
                            }
                        }
                        else//数据库存在(修改)
                        {
                            #region(楼盘数据修改)
                            nowBuilding.buildingid = buildingId;
                            int _builResult = 0;
                            //不是自己的信息&&自己不是25(插入子表)
                            if (building.fxtcompanyid != company.companyid && company.companyid != DatProjectBL.FXTCOMPANYID)
                            {
                                nowBuilding.createtime = DateTime.Now;
                                _builResult = DatBuildingBL.AddSub(nowBuilding, tableInfo.BuildingTable);
                                //添加返回楼栋id
                                int bid = buiJobj.Value<int>("buildingid");
                                buildings.Add(new { fxtbuildingid = _builResult, buildingid = bid });
                            }
                            else//是自己的||自己是25
                            {
                                upBuilColumn.Remove("creator");
                                nowBuilding.fxt_companyid = building.fxt_companyid;
                                nowBuilding.fxtcompanyid = building.fxtcompanyid;
                                nowBuilding.savedatetime = DateTime.Now;
                                nowBuilding.SetAvailableFields(upBuilColumn.ToArray());//设置要更新的字段
                                //在主表
                                if (building.fxt_companyid == 0)
                                {
                                    DatBuildingBL.Update(nowBuilding, tableInfo.BuildingTable);
                                    _builResult = nowBuilding.buildingid;
                                    //添加返回楼栋id
                                    int bid = buiJobj.Value<int>("buildingid");
                                    buildings.Add(new { fxtbuildingid = _builResult, buildingid = bid });
                                }
                                else//在子表
                                {
                                    DatBuildingBL.UpdateSub(nowBuilding, tableInfo.BuildingTable);
                                    _builResult = nowBuilding.buildingid;
                                    //添加返回楼栋id
                                    int bid = buiJobj.Value<int>("buildingid");
                                    buildings.Add(new { fxtbuildingid = _builResult, buildingid = bid });
                                }
                            }
                            if (_builResult < 1)
                            {
                                return string.Format(result, 0, "修改楼栋失败");
                            }
                            #endregion
                        }
                        if (buildingId < 1)
                        {
                            return string.Format(result, 0, "新增楼栋失败,楼栋名:" + nowBuilding.buildingname);
                        }
                        //LogHelper.Info("楼栋");

                        //*********新增房号*********//
                        #region (房号数据更新)
                        JArray houseArray = buiJobj["houselist"] as JArray;
                        foreach (var array in houseArray)
                        {
                            JObject houseJobj = (JObject)array;
                            List<string> upHouseColumn = new List<string>();
                            DATHouse nowHouse = new DATHouse();
                            foreach (var _prop in houseJobj)
                            {
                                string key = _prop.Key;
                                if (key.ToLower().Equals("houseid"))
                                {
                                    continue;
                                }

                                var property = nowHouse.GetType().GetProperties().Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                                if (property == null)
                                {
                                    continue;
                                }
                                if (!key.ToLower().Contains("companyid"))
                                {
                                    upHouseColumn.Add(key.ToLower());
                                }
                                object _value = ImportProjectAndBuildingAndHouse_valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                                property.SetValue(nowHouse, _value, null);
                            }
                            if (nowHouse.buildarea.HasValue && nowHouse.buildarea == 0)
                            {
                                nowHouse.buildarea = null;
                            }
                            nowHouse.houseid = 0;
                            nowHouse.buildingid = buildingId;
                            nowHouse.cityid = cityId;
                            //nowHouse.fxtcompanyid = company.companyid;
                            nowHouse.valid = 1;
                            //nowHouse.isevalue = 1;
                            //nowHouse.isshowbuildingarea = null;

                            if (string.IsNullOrEmpty(nowHouse.housename))
                            {
                                return string.Format(result, 0, "新增房号失败,房号名不能为空");
                            }

                            int houseId = 0;
                            //如果传过来数据库对应的buildingId
                            if (!string.IsNullOrEmpty(jobj.Value<string>("fxthouseid")) || jobj.Value<string>("fxthouseid") != "null")
                            {
                                houseId = Convert.ToInt32(jobj.Value<string>("fxthouseid"));
                            }
                            //从数据库获取存在的楼栋信息
                            DATHouse house = null;
                            if (!buildingIsAdd)//楼栋不是新增的
                            {
                                if (houseId == 0)//没有传houseId过来(查询房号)
                                {
                                    house = DatHouseBL.GetHouseByName(cityId, buildingId, company.companyid, nowHouse.housename, typecode);
                                }
                                else
                                {
                                    house = DatHouseBL.GetHouseById(cityId, buildingId, company.companyid, houseId);
                                    if (house == null)
                                    {
                                        houseId = 0;
                                        house = DatHouseBL.GetHouseByName(cityId, buildingId, company.companyid, nowHouse.housename, typecode);
                                    }
                                }
                                if (house != null)
                                {
                                    houseId = house.houseid;
                                }
                            }
                            else
                            {
                                houseId = 0;
                            }

                            //开始对房号进行数据库更新
                            bool houseIsAdd = false;
                            if (houseId == 0)//数据库不存在(新增)
                            {
                                houseIsAdd = true;
                                nowHouse.createtime = DateTime.Now;
                                houseId = DatHouseBL.Add(nowHouse, tableInfo.HouseTable);
                                if (houseId < 1)
                                {
                                    return string.Format(result, 0, "新增房号失败");
                                }
                            }
                            else//数据库存在(修改)
                            {
                                #region(楼盘数据修改)
                                nowHouse.houseid = houseId;
                                int _houseResult = 0;
                                //不是自己的信息&&自己不是25(插入子表)
                                if (house.fxtcompanyid != company.companyid && company.companyid != DatProjectBL.FXTCOMPANYID)
                                {
                                    nowHouse.createtime = DateTime.Now;
                                    _houseResult = DatHouseBL.AddSub(nowHouse, tableInfo.HouseTable);
                                }
                                else//是自己的||自己是25
                                {
                                    upHouseColumn.Remove("creator");
                                    nowHouse.fxtcompanyid = house.fxtcompanyid;
                                    nowHouse.savedatetime = DateTime.Now;
                                    nowHouse.SetAvailableFields(upHouseColumn.ToArray());//设置要更新的字段
                                    //在主表
                                    if (house.remark == "1")
                                    {
                                        _houseResult = DatHouseBL.Update(nowHouse, tableInfo.HouseTable);
                                    }
                                    else//在子表
                                    {
                                        _houseResult = DatHouseBL.UpdateSub(nowHouse, tableInfo.HouseTable);
                                    }
                                }
                                if (_houseResult < 1)
                                {
                                    return string.Format(result, 0, "修改房号失败");
                                }
                                #endregion
                            }
                            if (houseId < 1)
                            {
                                return string.Format(result, 0, "新增房号失败,房号名:" + nowHouse.housename);
                            }
                        }
                        #endregion
                        LogHelper.Info("房号");

                    }
                }
                #endregion

                //BaseBL.CommitBaseDATransaction();

                return string.Format(result, projectId, "", buildings.ToJson());


            }
            catch (Exception ex)
            {
                //BaseBL.RollbackBaseDATransaction();
                //LogHelper.Error(ex);
                throw ex;
            }
        }
        static object ImportProjectAndBuildingAndHouse_valueType(Type t, object value, bool isDecode = false)
        {
            string strName = t.Name;
            bool existsNull = false;
            if (t.Name == "Nullable`1")
            {
                existsNull = true;
                strName = t.GetGenericArguments()[0].Name;
            }
            if (string.IsNullOrEmpty(Convert.ToString(value)))
            {
                if (existsNull)
                {
                    return null;
                }
                else
                {
                    value = null;
                }
            }

            if (isDecode)
            {
                value = HttpUtility.UrlDecode(Convert.ToString(value)).Replace("%20", "+");
            }
            switch (strName.Trim())
            {
                case "Decimal":
                    decimal d = decimal.Zero;
                    decimal.TryParse(value.ToString(), out d);
                    value = d;
                    break;
                case "Int32":
                    int i = 0;
                    int.TryParse(value.ToString(), out i);
                    value = i;
                    break;
                case "Int64":
                    long i2 = 0;
                    long.TryParse(value.ToString(), out i2);
                    value = i2;
                    break;
                case "Float":
                    value = float.Parse(Convert.ToString(value));
                    break;
                case "DateTime":
                    value = Convert.ToDateTime(value);
                    break;
                case "Double":
                    value = Convert.ToDouble(value);
                    break;
                case "Bool":
                    value = Convert.ToBoolean(value);
                    break;
                case "String":
                    value = Convert.ToString(value);
                    break;
                case "Array":
                    value = (Array)value;
                    break;
                default:
                    value = value as object;
                    break;
            }
            return value;
        }
        /// <summary>
        /// 给楼盘上传照片
        /// ("无纸化住宅物业信息采集系统"入库调用)
        /// 创建人:曾智磊,日期:2014-07-07
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string AddProjectPhoto(System.IO.Stream stream, JObject funinfo, UserCheck company)
        {


            string result = "{{\"path\":\"{0}\",\"message\":\"{1}\"}}";
            int projectId = 0;//funinfo.Value<int>("projectid");
            int buildingId = Convert.ToInt32(funinfo.Value<string>("buildingid"));
            int photoTypeCode = 0;// funinfo.Value<int>("phototypecode");
            string photoName = "";
            string fileName = "";
            int cityId = 0;
            DateTime photoDate = DateTime.Now;
            //获取楼盘
            if (!string.IsNullOrEmpty(funinfo.Value<string>("projectid")) || funinfo.Value<string>("projectid") != "null")
            {
                projectId = Convert.ToInt32(funinfo.Value<string>("projectid"));
            }
            //获取城市
            if (!string.IsNullOrEmpty(funinfo.Value<string>("cityid")) || funinfo.Value<string>("cityid") != "null")
            {
                cityId = Convert.ToInt32(funinfo.Value<string>("cityid"));
            }
            //获取照片类型
            if (!string.IsNullOrEmpty(funinfo.Value<string>("phototypecode")) || funinfo.Value<string>("phototypecode") != "null")
            {
                photoTypeCode = Convert.ToInt32(funinfo.Value<string>("phototypecode"));
            }
            //获取照片文件名
            if (!string.IsNullOrEmpty(funinfo.Value<string>("filename")) || funinfo.Value<string>("filename") != "null")
            {
                fileName = funinfo.Value<string>("filename");
            }
            //照片名称
            photoName = funinfo.Value<string>("photoname");
            //*************************验证数据***************************************//
            if (cityId < 1)
            {
                return string.Format(result, "", "请填写楼盘城市");
            }
            if (projectId < 1)
            {
                return string.Format(result, "", "请选择楼盘");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Format(result, "", "未获取到文件名");
            }
            if (photoTypeCode < 1)
            {
                photoTypeCode = DatProjectBL.PHOTOTYPECODE_9;
            }
            string dirPath = WebCommon.GetConfigSetting("ProjectPicPath") + "/" + cityId + "/" + projectId;
            //if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(dirPath)))
            //{
            //    Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(dirPath));
            //}
            string _fileName = dirPath + "/" + WebCommon.GetRndString(20) + "_" + fileName;
            //using (FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath(_fileName), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //{
            //    //偏移指针
            //    fs.Seek(0, SeekOrigin.Begin);
            //    long ByteLength = WebOperationContext.Current.IncomingRequest.ContentLength;
            //    byte[] fileContent = new byte[ByteLength];
            //    stream.Read(fileContent, 0, fileContent.Length);
            //    fs.Write(fileContent, 0, fileContent.Length);
            //    fs.Flush();
            //}

            LogHelper.Info(_fileName);
            var a = WebCommon.GetConfigSetting("OssUpload");
            LogHelper.Info(a);

            var r = OssHelp.UpFileAsync(stream, _fileName);

            LNKPPhoto addObj = new LNKPPhoto
            {
                cityid = cityId,
                fxtcompanyid = company.companyid,
                path = _fileName,
                photodate = photoDate,
                photoname = photoName,
                phototypecode = photoTypeCode,
                projectid = projectId,
                buildingid = buildingId,
                valid = 1
            };
            if (DatProjectBL.AddPhoto(addObj) <= 0)
            {
                return string.Format(result, "", "照片信息插入数据库异常");
            }
            return string.Format(result, HttpUtility.UrlEncode(_fileName).Replace("+", "%20"), "成功");


        }


        /// <summary>
        /// 获取押品复估价格
        /// 创建人:曾智磊，2014-08-06
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCollateralReassessment(JObject funinfo, UserCheck company)
        {
            int typecode = company.producttypecode == 1003036 ? 1003036 : 1003002;
            ReassessmentPrice resultObj = new ReassessmentPrice();
            //string result = "{{\"price\":{0},\"pricetype\":{1},\"projecteprice\":{2},\"buildingeprice\":{3},\"remark\":\"{4}\",\"nowareaprice\":{5},\"lastareaprice\":{6}}}";
            //楼层需进行初次判断
            //////////需要输出的值
            decimal price = 0;
            int pricetype = 0;//1:自动估价值，2:现复估值,0：估不出值
            decimal projecteprice = 0;
            decimal buildingeprice = 0;
            decimal nowareaprice = 0;//当月行政区均价
            decimal lastareaprice = 0;//上个月行政区均价
            ///////////各传过来的参数
            List<DATProjectAvgPrice> avgCountList = new List<DATProjectAvgPrice>();
            //int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            int systypecode = company.parentproducttypecode;
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));// 0;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));// 0;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));// 0;
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));//  1;
            //int fxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));//  25;//要通过哪个公司的companyId计算，传参数
            int fxtCompanyId = company.parentshowdatacompanyid;
            double buildingArea = StringHelper.TryGetDouble(funinfo.Value<string>("buildingarea"));//0;//房号建筑面积
            string date = funinfo.Value<string>("date");// "2014-05";//传过来的月份
            string floornoStr = funinfo.Value<string>("floornostr");// "1";//传过来的可能是字符型
            int isfirst = StringHelper.TryGetInt(funinfo.Value<string>("isfirst"));// 1;//押品是否为首次复估，1::是 0:不是
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));//0;//行政区ID
            ////////////功能变量
            string lastdate = Convert.ToDateTime(date).AddMonths(-1).ToString("yyyy-MM");//上个月份
            List<DATProjectAvgPrice> avgpricelist = new List<DATProjectAvgPrice>();//楼盘建筑类型及面积段分类均价
            DATBuilding building = null;
            DATHouse house = null;
            int purposetype = 1002001;//普通住宅
            decimal areaCoefficient = 0;//面积修正系数
            decimal floorCoefficient = 0;//楼层系数
            decimal frontCoefficient = 0;//朝向系数
            decimal sightCoefficient = 0;//景观系数
            decimal coefficient = 0;//总修正系数
            string wcfcurdate = DateTime.Now.ToString("yyyy-MM-dd"), avgstr = "";
            var wcfpara = new { projectId = projectId, cityId = cityId, codeType = purposetype, date = date, fxtCompanyId = fxtCompanyId };
            int buildingareatype = CodeHelper.GetBuildingAreaType(buildingArea);//面积段类型

            //如果关联到房号
            if (houseId > 0)
            {
                //验证用途
                house = DatHouseBL.GetHouseById(cityId, buildingId, fxtCompanyId, houseId);
                if (house == null)//找不到房号
                {
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = 0,
                        remark = "查询房号-根据ID未找到匹配的房号，需进入人工复估",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();
                }
                if (house.purposecode != purposetype)//不为普通住宅
                {
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = house.purposecode,
                        remark = "查询房号-主用途不为普通住宅，需进入人工复估",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();
                }
                DateTime nowDate = Convert.ToDateTime(date);
                string autoStartTime = nowDate.AddMonths(-3).ToString("yyyy-MM-01");//前三个月月初
                string autoEndTime = StringHelper.TryGetDateTime(nowDate.ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd");//上个月月末
                DataSet ds = DatHouseBL.GetEValueByProjectId(cityId, projectId, buildingId, houseId, fxtCompanyId, 1, company.companyid, company.username, buildingArea, autoStartTime, autoEndTime, 0, 0, systypecode, 0, 0, 0, 0);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    price = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["avgPrice"].ToString());
                    projecteprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["EPrice"].ToString());
                    buildingeprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["BEPrice"].ToString());
                }
                //自动估出价格（则返回结果）
                #region
                if (price > 100)
                {
                    pricetype = 1;
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = house.purposecode,
                        remark = "自动估价结果",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();//string.Format(result, price, pricetype, projecteprice, buildingeprice, "自动估价结果", nowareaprice, lastareaprice);
                }
                #endregion
                price = 0;
                //无自动估价(调用wcf接口)
                #region
                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                object objprice = proprice.Entrance(wcfcurdate, DataCenterCommon.GetCode(wcfcurdate), "D", "CrossByFxtCompanyId", JSONHelper.ObjectToJSON(wcfpara));
                proprice.Abort();
                if (objprice != null)
                {
                    avgstr = objprice.ToString();
                    //LogHelper.Info(avgstr);
                }
                if (!string.IsNullOrEmpty(avgstr))
                {
                    avgpricelist = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                }
                #endregion
                //计算楼盘均价
                avgCountList = avgpricelist.Where(obj => obj.avgprice > 0).ToList();
                projecteprice = avgCountList.Count() <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                building = DatBuildingBL.GetBuildingById(cityId, projectId, fxtCompanyId, buildingId);
                house = DatHouseBL.GetHouseById(cityId, buildingId, fxtCompanyId, houseId);
                int buildingtypecode = building == null ? 0 : Convert.ToInt32(building.buildingtypecode);//建筑类型
                buildingeprice = building == null ? 0 : projecteprice * (building.weight == 0 ? 1 : building.weight);//计算楼栋均价
                GetReassessmentCoefficient(cityId, purposetype, buildingareatype, building, house, floornoStr,
                    out areaCoefficient, out floorCoefficient, out sightCoefficient, out frontCoefficient);//获取各修正系数
                #region (根据各维度复估)
                //无自动估价值时（面积段&建筑类型,维度价格进行修正-楼层差*，朝向差，景观差）     
                #region
                DATProjectAvgPrice avgPrice = avgpricelist.Where(obj => obj.buildingareatype == buildingareatype && obj.buildingtypecode == buildingtypecode).FirstOrDefault();
                price = 0;
                if (avgPrice != null && avgPrice.avgprice > 0)
                {
                    coefficient = GetReassessmentCoefficientSum(new decimal[] { floorCoefficient, frontCoefficient, sightCoefficient });
                    price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
                    pricetype = 2;
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = house.purposecode,
                        remark = "匹配到房号-面积段&建筑类型维度价格*(楼层差，朝向差，景观差)",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到房号-面积段&建筑类型维度价格*(楼层差，朝向差，景观差)", nowareaprice, lastareaprice);
                }
                #endregion
                //无上面维度价格时（建筑类型，维度均价进行修正-面积差*，楼层差*，朝向差，景观差）
                #region
                avgCountList = avgpricelist.Where(obj => obj.avgprice > 0 && obj.buildingtypecode == buildingtypecode).ToList();
                decimal _avgPrice = avgCountList.Count <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                price = 0;
                if (_avgPrice > 0)
                {
                    coefficient = GetReassessmentCoefficientSum(new decimal[] { areaCoefficient, floorCoefficient, frontCoefficient, sightCoefficient });
                    price = (_avgPrice * (coefficient == 0 ? 1 : coefficient));
                    pricetype = 2;
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = house.purposecode,
                        remark = "匹配到房号-建筑类型维度均价*(面积差，楼层差，朝向差，景观差)",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到房号-建筑类型维度均价*(面积差，楼层差，朝向差，景观差)", nowareaprice, lastareaprice);
                }
                #endregion
                //无上面维度价格时（面积段，维度均价进行修正-楼层差*，朝向差，景观差）
                #region
                avgCountList = avgpricelist.Where(obj => obj.avgprice > 0 && obj.buildingareatype == buildingareatype).ToList();
                _avgPrice = avgCountList.Count <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                price = 0;
                if (_avgPrice > 0)
                {
                    coefficient = GetReassessmentCoefficientSum(new decimal[] { floorCoefficient, frontCoefficient, sightCoefficient });
                    price = (_avgPrice * (coefficient == 0 ? 1 : coefficient));
                    pricetype = 2;
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = house.purposecode,
                        remark = "匹配到房号-面积段维度均价*(楼层差，朝向差，景观差)",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到房号-面积段维度均价*(楼层差，朝向差，景观差)", nowareaprice, lastareaprice);
                }
                #endregion
                //无上面维度价格时（楼盘均价进行修正-面积差*，楼层差*，朝向差，景观差）
                #region
                price = 0;
                if (projecteprice > 0)
                {
                    coefficient = GetReassessmentCoefficientSum(new decimal[] { areaCoefficient, floorCoefficient, frontCoefficient, sightCoefficient });
                    price = (projecteprice * (coefficient == 0 ? 1 : coefficient));
                    pricetype = 2;
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = house.purposecode,
                        remark = "匹配到房号-楼盘均价*(面积差，楼层差，朝向差，景观差)",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到房号-楼盘均价*(面积差，楼层差，朝向差，景观差)", nowareaprice, lastareaprice);
                }
                #endregion
                #endregion
                pricetype = 0;
                //如果押品是第一次复估则返回本月行政区均价和上个月行政区均价
                if (isfirst != 1)
                {
                    nowareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, date);
                    lastareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, lastdate);
                }
                resultObj = new ReassessmentPrice
                {
                    price = price,
                    pricetype = pricetype,
                    projecteprice = projecteprice,
                    buildingeprice = buildingeprice,
                    purposecode = house.purposecode,
                    remark = "匹配到房号-无楼盘均价，需进入人工复估",
                    nowareaprice = nowareaprice,
                    lastareaprice = lastareaprice
                };
                return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配房号-无楼盘均价，需进入人工复估", nowareaprice, lastareaprice);
            }
            else
            {
                DATProject project = null;
                #region(验证用途)
                if (buildingId > 0)
                {
                    building = DatBuildingBL.GetBuildingById(cityId, projectId, fxtCompanyId, buildingId);//建筑类型
                    if (building == null)//找不到楼栋
                    {
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = 0,
                            remark = "查询楼栋-根据ID未找到匹配的楼栋，需进入人工复估",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();
                    }
                    if (building.purposecode != purposetype)//不为普通住宅
                    {
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = Convert.ToInt32(building.purposecode),
                            remark = "查询楼栋-主用途不为普通住宅，需进入人工复估",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();
                    }
                }
                else if (projectId > 0)
                {
                    project = DatProjectBL.GetProjectInfoByProjectId(cityId, fxtCompanyId, projectId, typecode);
                    if (project == null)//找不到楼盘
                    {
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = 0,
                            remark = "查询楼盘-根据ID未找到匹配的楼盘，需进入人工复估",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();
                    }
                    if (project.purposecode != 1001001)//楼盘主用途不为(居住)
                    {
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = project.purposecode,
                            remark = "查询楼盘-主用途不为普通住宅，需进入人工复估",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();
                    }
                }
                #endregion
                //无自动估价(调用wcf接口)
                #region
                wcf_projectavgprice.FxtAPIClient proprice = new wcf_projectavgprice.FxtAPIClient();
                object objprice = proprice.Entrance(wcfcurdate, DataCenterCommon.GetCode(wcfcurdate), "D", "CrossByFxtCompanyId", JSONHelper.ObjectToJSON(wcfpara));
                proprice.Abort();
                if (objprice != null)
                {
                    avgstr = objprice.ToString();
                    //LogHelper.Info(avgstr);
                }
                if (!string.IsNullOrEmpty(avgstr))
                {
                    avgpricelist = JSONHelper.JSONStringToList<DATProjectAvgPrice>(avgstr);
                }
                #endregion
                pricetype = 2;//设置为现复估值
                //计算楼盘均价
                avgCountList = avgpricelist.Where(obj => obj.avgprice > 0).ToList();
                projecteprice = avgCountList.Count <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                if (buildingId > 0)//关联到楼栋
                {
                    #region (关联到楼栋)
                    buildingeprice = building == null ? 0 : projecteprice * (building.weight == 0 ? 1 : building.weight);//计算楼栋均价
                    int buildingtypecode = building == null ? 0 : Convert.ToInt32(building.buildingtypecode);//建筑类型
                    GetReassessmentCoefficient(cityId, purposetype, buildingareatype, building, null, floornoStr,
                        out areaCoefficient, out floorCoefficient, out sightCoefficient, out frontCoefficient);//获取各修正系数
                    #region (各维度复估)
                    //无自动估价值时（面积段&建筑类型,维度价格进行修正-楼层差）     
                    #region
                    DATProjectAvgPrice avgPrice = avgpricelist.Where(obj => obj.buildingareatype == buildingareatype && obj.buildingtypecode == buildingtypecode).FirstOrDefault();
                    price = 0;
                    if (avgPrice != null && avgPrice.avgprice > 0)
                    {
                        coefficient = GetReassessmentCoefficientSum(new decimal[] { floorCoefficient });
                        price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = Convert.ToInt32(building.purposecode),
                            remark = "匹配到楼栋-面积段&建筑类型维度价格*(楼层差)",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼栋-面积段&建筑类型维度价格*(楼层差)", nowareaprice, lastareaprice);
                    }
                    #endregion
                    //无上面维度价格时（建筑类型，维度均价进行修正-面积差*，楼层差*）
                    #region
                    avgCountList = avgpricelist.Where(obj => obj.avgprice > 0 && obj.buildingtypecode == buildingtypecode).ToList();
                    decimal _avgPrice = avgCountList.Count <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                    price = 0;
                    if (_avgPrice > 0)
                    {
                        coefficient = GetReassessmentCoefficientSum(new decimal[] { areaCoefficient, floorCoefficient });
                        price = (_avgPrice * (coefficient == 0 ? 1 : coefficient));
                        pricetype = 2;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = Convert.ToInt32(building.purposecode),
                            remark = "匹配到楼栋-建筑类型维度均价*(面积差，楼层差)",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼栋-建筑类型维度均价*(面积差，楼层差)", nowareaprice, lastareaprice);
                    }
                    #endregion
                    //无上面维度价格时（面积段，维度均价进行修正-楼层差*）
                    #region
                    avgCountList = avgpricelist.Where(obj => obj.avgprice > 0 && obj.buildingareatype == buildingareatype).ToList();
                    _avgPrice = avgpricelist.Count <= 0 ? 0 : Convert.ToDecimal(avgpricelist.Average(obj => obj.avgprice));
                    price = 0;
                    if (_avgPrice > 0)
                    {
                        coefficient = GetReassessmentCoefficientSum(new decimal[] { floorCoefficient });
                        price = (_avgPrice * (coefficient == 0 ? 1 : coefficient));
                        pricetype = 2;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = Convert.ToInt32(building.purposecode),
                            remark = "匹配到楼栋-面积段维度均价*(楼层差)",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼栋-面积段维度均价*(楼层差)", nowareaprice, lastareaprice);
                    }
                    #endregion
                    //无上面维度价格时（楼盘均价进行修正-面积差*，楼层差*）
                    #region
                    price = 0;
                    if (projecteprice > 0)
                    {
                        coefficient = GetReassessmentCoefficientSum(new decimal[] { areaCoefficient, floorCoefficient });
                        price = (projecteprice * (coefficient == 0 ? 1 : coefficient));
                        pricetype = 2;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = Convert.ToInt32(building.purposecode),
                            remark = "匹配到楼栋-楼盘均价*(面积差，楼层差)",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼栋-楼盘均价*(面积差，楼层差)", nowareaprice, lastareaprice);
                    }
                    #endregion
                    #endregion

                    #endregion
                    pricetype = 0;
                    //如果押品是第一次复估则返回本月行政区均价和上个月行政区均价
                    if (isfirst != 1)
                    {
                        nowareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, date);
                        lastareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, lastdate);
                    }
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = Convert.ToInt32(building.purposecode),
                        remark = "匹配到楼栋-无楼盘均价，需进入人工复估",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼栋-无楼盘均价，需进入人工复估", nowareaprice, lastareaprice);
                }
                else if (projectId > 0)//关联到楼盘
                {
                    #region (关联到楼盘)
                    int buildingtypecode = 0;
                    if (StringHelper.TryGetInt(floornoStr) > 18)
                    {
                        buildingtypecode = 2003004;
                    }
                    GetReassessmentCoefficient(cityId, purposetype, buildingareatype, null, null, null,
                    out areaCoefficient, out floorCoefficient, out sightCoefficient, out frontCoefficient);//获取各修正系数
                    #region(各维度复估)
                    //无自动估价值时（面积段&建筑类型,维度价格-）     
                    #region
                    DATProjectAvgPrice avgPrice = avgpricelist.Where(obj => obj.buildingareatype == buildingareatype && obj.buildingtypecode == buildingtypecode && buildingtypecode > 0).FirstOrDefault();
                    price = 0;
                    if (avgPrice != null && avgPrice.avgprice > 0)
                    {
                        price = avgPrice.avgprice;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = project.purposecode,
                            remark = "匹配到楼盘-面积段&建筑类型维度价格",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼盘-面积段&建筑类型维度价格", nowareaprice, lastareaprice);
                    }
                    #endregion
                    //无上面维度价格时（建筑类型，维度均价进行修正-面积差）
                    #region
                    avgCountList = avgpricelist.Where(obj => obj.avgprice > 0 && obj.buildingtypecode == buildingtypecode && buildingtypecode > 0).ToList();
                    decimal _avgPrice = avgCountList.Count <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                    price = 0;
                    if (_avgPrice > 0)
                    {
                        coefficient = GetReassessmentCoefficientSum(new decimal[] { areaCoefficient });
                        price = (_avgPrice * (coefficient == 0 ? 1 : coefficient));
                        pricetype = 2;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = project.purposecode,
                            remark = "匹配到楼盘-建筑类型维度均价*(面积差)",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼盘-建筑类型维度均价*(面积差)", nowareaprice, lastareaprice);
                    }
                    #endregion
                    //无上面维度价格时（面积段，维度均价进行修正-楼层差*）
                    #region
                    avgCountList = avgpricelist.Where(obj => obj.avgprice > 0 && obj.buildingareatype == buildingareatype).ToList();
                    _avgPrice = avgCountList.Count <= 0 ? 0 : Convert.ToDecimal(avgCountList.Average(obj => obj.avgprice));
                    price = 0;
                    if (_avgPrice > 0)
                    {
                        price = _avgPrice;
                        pricetype = 2;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = project.purposecode,
                            remark = "匹配到楼盘-面积段维度均价",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼盘-面积段维度均价", nowareaprice, lastareaprice);
                    }
                    #endregion
                    //无上面维度价格时（楼盘均价进行修正-面积差*）
                    #region
                    price = 0;
                    if (projecteprice > 0)
                    {
                        coefficient = GetReassessmentCoefficientSum(new decimal[] { areaCoefficient });
                        price = (projecteprice * (coefficient == 0 ? 1 : coefficient));
                        pricetype = 2;
                        resultObj = new ReassessmentPrice
                        {
                            price = price,
                            pricetype = pricetype,
                            projecteprice = projecteprice,
                            buildingeprice = buildingeprice,
                            purposecode = project.purposecode,
                            remark = "匹配到楼盘-楼盘均价*(面积差)",
                            nowareaprice = nowareaprice,
                            lastareaprice = lastareaprice
                        };
                        return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼盘-楼盘均价*(面积差)", nowareaprice, lastareaprice);
                    }
                    #endregion
                    #endregion
                    #endregion
                    pricetype = 0;
                    //如果押品是第一次复估则返回本月行政区均价和上个月行政区均价
                    if (isfirst != 1)
                    {
                        nowareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, date);
                        lastareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, lastdate);
                    }
                    resultObj = new ReassessmentPrice
                    {
                        price = price,
                        pricetype = pricetype,
                        projecteprice = projecteprice,
                        buildingeprice = buildingeprice,
                        purposecode = project.purposecode,
                        remark = "匹配到楼盘-无楼盘均价，需进入人工复估",
                        nowareaprice = nowareaprice,
                        lastareaprice = lastareaprice
                    };
                    return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配到楼盘-无楼盘均价，需进入人工复估", nowareaprice, lastareaprice);
                }

            }
            //如果押品是第一次复估则返回本月行政区均价和上个月行政区均价
            if (isfirst != 1)
            {
                nowareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, date);
                lastareaprice = DATAvgPriceMonthBL.GetAreaAvgPriceTrend(cityId, areaId, lastdate);
            }
            resultObj = new ReassessmentPrice
            {
                price = price,
                pricetype = pricetype,
                projecteprice = projecteprice,
                buildingeprice = buildingeprice,
                purposecode = purposetype,
                remark = "匹配不到楼盘-，需进入人工复估",
                nowareaprice = nowareaprice,
                lastareaprice = lastareaprice
            };
            return resultObj.ToJson();// string.Format(result, price, pricetype, projecteprice, buildingeprice, "匹配不到楼盘-，需进入人工复估", nowareaprice, lastareaprice);
        }
        /// <summary>
        /// 获取押品复估各修正系数
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="purposetype">用途</param>
        /// <param name="buildingareatype">面积段</param>
        /// <param name="building">楼栋信息，可为null</param>
        /// <param name="house">房号信息，可为null</param>
        /// <param name="floornoStr">押品所在层（如果此参数为null或非数字，则会根据houe参数进行判断）</param>
        /// <param name="areaCoefficient">面积差修正系数</param>
        /// <param name="floorCoefficient">楼层差修正系数</param>
        /// <param name="sightCoefficient">景观差修正系数</param>
        /// <param name="frontCoefficient">朝向差修正系数</param>
        private static void GetReassessmentCoefficient(int cityId, int purposetype, int buildingareatype, DATBuilding building, DATHouse house, string floornoStr,
            out decimal areaCoefficient, out decimal floorCoefficient, out decimal sightCoefficient, out decimal frontCoefficient)
        {
            areaCoefficient = 0;
            floorCoefficient = 0;
            sightCoefficient = 0;
            frontCoefficient = 0;
            int _floorno = 0;
            int _sightcode = 0;
            int _frontcode = 0;
            int _totalfloor = 0;
            //获取系数信息
            if (!string.IsNullOrEmpty(floornoStr) && StringHelper.TryGetInt(floornoStr) != 0)//Regex.IsMatch(floornoStr, @"^\d+$"))
            {
                _floorno = StringHelper.TryGetInt(floornoStr);
            }
            else if (house != null)
            {
                _floorno = Convert.ToInt32(house.floorno);
            }
            if (building != null)
            {
                _totalfloor = Convert.ToInt32(building.totalfloor);
            }
            if (house != null)
            {
                _sightcode = Convert.ToInt32(house.sightcode);
                _frontcode = Convert.ToInt32(house.frontcode);
            }
            List<SysCodePrice> codepricelist = SysCodePriceBL.GetCodePriceList(cityId, purposetype, _totalfloor,
                _floorno, 0, 0, _sightcode, _frontcode, buildingareatype);
            //如果城市没修正系数，则默认取昆明
            if (codepricelist == null || codepricelist.Count < 1)
            {
                cityId = 157;
                codepricelist = SysCodePriceBL.GetCodePriceList(cityId, purposetype, _totalfloor,
                _floorno, 0, 0, _sightcode, _frontcode, buildingareatype);
            }
            areaCoefficient = codepricelist.Where(obj => obj.code == buildingareatype).Count() > 0 ? codepricelist.Where(obj => obj.code == buildingareatype && buildingareatype > 0).FirstOrDefault().price : 0;
            floorCoefficient = codepricelist.Where(obj => obj.code == _floorno).Count() > 0 ? codepricelist.Where(obj => obj.code == _floorno).FirstOrDefault().price : 0;
            sightCoefficient = codepricelist.Where(obj => obj.code == _sightcode).Count() > 0 ? codepricelist.Where(obj => obj.code == _sightcode).FirstOrDefault().price : 0;
            frontCoefficient = codepricelist.Where(obj => obj.code == _frontcode).Count() > 0 ? codepricelist.Where(obj => obj.code == _frontcode).FirstOrDefault().price : 0;
        }
        /// <summary>
        /// 根据各细分修正系数获取总修正系数
        /// </summary>
        /// <param name="coefficients"></param>
        /// <returns></returns>
        private static decimal GetReassessmentCoefficientSum(decimal[] coefficients)
        {
            decimal coefficient = 0;
            if (coefficients != null && coefficients.Length > 0)
            {
                for (int i = 0; i < coefficients.Length; i++)
                {
                    if (coefficients[i] == 0)
                    {
                        continue;
                    }
                    if (i == 0)
                    {
                        coefficient = 1 + coefficients[i];
                    }
                    else
                    {
                        coefficient = coefficient * (1 + coefficients[i]);
                    }
                }
            }
            return coefficient;
        }

        [OverflowAttrbute(ApiType.Case)]
        private static void GetDATCaseList(SearchBase search, string projectname, out string result)
        {

            List<Dat_Case> list = DATCaseBL.GetDATCaseList(search, "", projectname, null);
            decimal? oneStageUnitprice = list.Where(s => s.buildingarea < 60).ToList().Average(s => s.unitprice);
            decimal? twoStageUnitprice = list.Where(s => s.buildingarea >= 60 && s.buildingarea < 90).ToList().Average(s => s.unitprice);
            decimal? threeStageUnitprice = list.Where(s => s.buildingarea >= 90 && s.buildingarea < 120).ToList().Average(s => s.unitprice);
            decimal? forStageUnitprice = list.Where(s => s.buildingarea >= 120 && s.buildingarea < 144).ToList().Average(s => s.unitprice);
            decimal? fiveStageUnitprice = list.Where(s => s.buildingarea >= 144).ToList().Average(s => s.unitprice);
            if (list.Count > 0)
            {
                list[0].oneStageUnitprice = oneStageUnitprice == null ? 0 : (decimal)oneStageUnitprice;
                list[0].twoStageUnitprice = twoStageUnitprice == null ? 0 : (decimal)twoStageUnitprice;
                list[0].threeStageUnitprice = threeStageUnitprice == null ? 0 : (decimal)threeStageUnitprice;
                list[0].forStageUnitprice = forStageUnitprice == null ? 0 : (decimal)forStageUnitprice;
                list[0].fiveStageUnitprice = fiveStageUnitprice == null ? 0 : (decimal)fiveStageUnitprice;
            }
            result = list.ToJson();
        }
        [OverflowAttrbute(ApiType.Case)]
        private static void GetDATCaseListForSpecial(SearchBase search, string projectname, out string result)
        {

            List<Dat_Case_Dhhy> list = DATCaseBL.GetDATCaseListForSpecial(search, "", projectname, null);
            decimal? oneStageUnitprice = list.Where(s => s.buildingarea < 60).ToList().Average(s => s.unitprice);
            decimal? twoStageUnitprice = list.Where(s => s.buildingarea >= 60 && s.buildingarea < 90).ToList().Average(s => s.unitprice);
            decimal? threeStageUnitprice = list.Where(s => s.buildingarea >= 90 && s.buildingarea < 120).ToList().Average(s => s.unitprice);
            decimal? forStageUnitprice = list.Where(s => s.buildingarea >= 120 && s.buildingarea < 144).ToList().Average(s => s.unitprice);
            decimal? fiveStageUnitprice = list.Where(s => s.buildingarea >= 144).ToList().Average(s => s.unitprice);
            if (list.Count > 0)
            {
                list[0].oneStageUnitprice = oneStageUnitprice == null ? 0 : (decimal)oneStageUnitprice;
                list[0].twoStageUnitprice = twoStageUnitprice == null ? 0 : (decimal)twoStageUnitprice;
                list[0].threeStageUnitprice = threeStageUnitprice == null ? 0 : (decimal)threeStageUnitprice;
                list[0].forStageUnitprice = forStageUnitprice == null ? 0 : (decimal)forStageUnitprice;
                list[0].fiveStageUnitprice = fiveStageUnitprice == null ? 0 : (decimal)fiveStageUnitprice;
            }
            result = list.ToJson();
        }
        private static string returnFlashJson(DataSet ds, int dateType, int flashType)
        {
            int max = 0;
            int min = 0;
            string title = "";
            string labels = "";
            string values = "";
            string yy = ""; //11年
            string yyyy = "";//2012年
            string format = "";

            switch (dateType)
            {
                case 1://week
                    format = "周";
                    break;
                case 2://Quarter
                    format = "季";
                    break;
                case 3://year
                    format = "年"; break;
                case 0:
                    format = "月";
                    break;
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                //图形数据
                if (dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int price = Convert.ToInt32(dt.Rows[i]["avgprice"]);
                        string week = "";
                        DateTime time = Convert.ToDateTime(dt.Rows[i]["avgpricedate"]);
                        yy = time.ToString("yyyy-MM-dd").Substring(2, 2);
                        yyyy = time.ToString("yyyy-MM-dd").Substring(0, 4);
                        if (dateType != 3)
                        {
                            week = time.ToString("yyyy-MM-dd").Substring(5, 2);
                            labels += string.Format("\"{0}-{1}\",", yy, week);
                        }
                        else
                        {
                            labels += string.Format("\"{0}\",", yy, week);
                        }

                        //if (i == 0)
                        //    title += date.ToString("yyyy-MM") + " 至 ";
                        //else if (i == dt.Rows.Count - 1)
                        //    title += date.ToString("yyyy-MM");

                        if (flashType == 0)
                        {
                            if (price > max) max = price;
                            if (min == 0) min = price;
                            else if (price < min) min = price;
                            if (dateType != 3)
                            {
                                values += string.Format("{{\"value\": {0},\"tip\": \"{1}年{2}{3}<br>#val#元/平方米\"}},", price, yyyy, week, format);
                            }
                            else
                            {
                                values += string.Format("{{\"value\": {0},\"tip\": \"{1}年<br>#val#元/平方米\"}},", price, yyyy);
                            }

                        }
                        else
                        {
                            int index = Convert.ToInt32(dt.Rows[i]["Exponential"]);
                            if (index > max) max = index;
                            if (min == 0) min = index;
                            else if (index < min) min = index;
                            values += string.Format("{{\"value\": {0},\"tip\": \"{1}年{2}{3}<br>指数：#val#\"}},", index, yyyy, week, format);
                        }

                        //CasePrice.Append('"' + dt.Rows[i]["CaseDate"].ToString() + ";" + dt.Rows[i]["UnitPrice"].ToString() + '"' + ",");
                        //values:鼠标滑过提示；title时间区间轴：201211至201212，labels:x轴；
                    }
                    labels = labels.Remove(labels.Length - 1);
                    values = values.Remove(values.Length - 1);
                }
            }//GetTrendZS
            if (flashType == 0)
            { return TrendHelper.GetTrend(title, labels, min, max, values); }
            else { return TrendHelper.GetTrendZS(title, labels, min, max, values); }

        }

        /// <summary>
        /// 获取单个楼盘近1、3、6个月案例总数 库晶晶20150210
        /// </summary>
        public class CaseCountList
        {
            public int months { get; set; }
            public int casecount { get; set; }
        }
        public static string GetCaseCountByProjectId_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;// StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;//1003036;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));

            var casecountlist = new List<CaseCountList>();
            casecountlist.Add(new CaseCountList
            {
                months = 1,
                casecount = DATCaseBL.GetCaseCountByProjectId_MCAS(search.FxtCompanyId, search.CityId, projectid, 1, search.SysTypeCode),
            });
            casecountlist.Add(new CaseCountList
            {
                months = 3,
                casecount = DATCaseBL.GetCaseCountByProjectId_MCAS(search.FxtCompanyId, search.CityId, projectid, 3, search.SysTypeCode),
            });
            casecountlist.Add(new CaseCountList
            {
                months = 6,
                casecount = DATCaseBL.GetCaseCountByProjectId_MCAS(search.FxtCompanyId, search.CityId, projectid, 6, search.SysTypeCode),
            });
            return casecountlist.ToJson();
        }
        /// <summary>
        /// 获取多个楼盘近n个月案例总数 tanql20150924
        /// </summary>
        public static string GetCaseCountByProjectIds_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            string projectids = funinfo.Value<string>("projectids");
            int months = StringHelper.TryGetInt(funinfo.Value<string>("months"));

            var value = DATCaseBL.GetCaseCountByProjectIds_MCAS(search.FxtCompanyId, search.CityId, projectids, months, search.SysTypeCode);
            return (value == null || value.Tables.Count <= 0 ? "" : value.Tables[0].ToJson());
        }

        /// <summary>
        /// 获取单个楼盘坐标及照片总数 库晶晶20150210
        /// </summary>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectListInfo_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string data = funinfo.Value<string>("data").Replace("\"", "");

            DataSet value = new DataSet();
            var reg = new Regex("projectid.(?<projectid>\\d*),cityid.(?<cityid>\\d*)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection ms = reg.Matches(data);
            foreach (Match m in ms)
            {
                int projectid = StringHelper.TryGetInt(m.Groups["projectid"].Value);
                int cityid = StringHelper.TryGetInt(m.Groups["cityid"].Value);
                value.Merge(DATCaseBL.GetProjectListInfo_MCAS(search.FxtCompanyId, projectid, cityid, search.SysTypeCode));
            }
            return (value == null || value.Tables.Count <= 0 ? "" : value.Tables[0].ToJson());
        }

        //城市区域均价、环比、同比
        public static string GetAvgPriceList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            DateTime datefrom = StringHelper.TryGetDateTime(funinfo.Value<string>("datefrom"));
            DateTime dateto = StringHelper.TryGetDateTime(funinfo.Value<string>("dateto"));

            DataSet ds = DATAvgPriceMonthBL.GetAvgPriceList(search, datefrom, dateto);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }

        //获取住宅案例列表 库晶晶20150415
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListNew(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            search.AreaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int issource = StringHelper.TryGetInt(funinfo.Value<string>("issource"));
            int buildingtypecode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));
            int purposecode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));
            int casetypecode = StringHelper.TryGetInt(funinfo.Value<string>("casetypecode"));
            DateTime casedatefrom = StringHelper.TryGetDateTime(funinfo.Value<string>("casedatefrom"));
            DateTime casedateto = StringHelper.TryGetDateTime(funinfo.Value<string>("casedateto"));
            decimal? buildingareafrom = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingareafrom"));
            decimal? buildingareato = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingareato"));
            string projectname = funinfo.Value<string>("projectname");
            if (search.PageIndex == 0)
            {
                search.Page = true;
                search.PageIndex = 1;
                search.PageRecords = 15;
            }
            DataSet ds = DATCaseBL.GetCaseListNew(search, buildingtypecode, purposecode, casetypecode, casedatefrom, casedateto, buildingareafrom, buildingareato, projectname, issource);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }
        //获取住宅案例最高单价、最低单价、平均单价 库晶晶20150416
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCasePrice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            search.AreaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int cityid = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            if (cityid < 1) return "请传入cityid";
            int issource = StringHelper.TryGetInt(funinfo.Value<string>("issource"));
            int buildingtypecode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));
            int purposecode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));
            int casetypecode = StringHelper.TryGetInt(funinfo.Value<string>("casetypecode"));
            DateTime casedatefrom = StringHelper.TryGetDateTime(funinfo.Value<string>("casedatefrom"));
            DateTime casedateto = StringHelper.TryGetDateTime(funinfo.Value<string>("casedateto"));
            decimal? buildingareafrom = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingareafrom"));
            decimal? buildingareato = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingareato"));
            string projectname = funinfo.Value<string>("projectname");
            if (search.PageIndex == 0)
            {
                search.Page = true;
                search.PageIndex = 1;
                search.PageRecords = 15;
            }
            DataSet ds = DATCaseBL.GetCasePrice(search, buildingtypecode, purposecode, casetypecode, casedatefrom, casedateto, buildingareafrom, buildingareato, projectname, issource);
            return (ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson());
        }
        //获取项目配套 库晶晶20150521
        [OverflowAttrbute(ApiType.Project)]
        public static string GetPAppendageByProjectId(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));

            //zhoub 20160510
            //使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                PAppendageByProjectIdListResponse pAppendageByProjectIdResponse;
                var request = new ProjectRequestParam()
                {
                    SerachParam = new SearchParam()
                    {
                        Page = search.Page,
                        PageIndex = search.PageIndex,
                        PageRecords = search.PageRecords,
                        CityId = search.CityId,
                        OrderBy = string.IsNullOrEmpty(search.OrderBy) ? "" : search.OrderBy,
                        CompanyId = search.FxtCompanyId,
                        SysTypeCode = search.SysTypeCode,
                        ProductTypeCode = company.producttypecode,
                        ParentShowDataCompanyId = company.companyid    //当前操作公司ID
                    },
                    ProjectId = projectId.ToString()
                };

                FxtClientService.GetPAppendageByProjectId(request, out pAppendageByProjectIdResponse);
                return pAppendageByProjectIdResponse.Project.ToLowerJson();
            }

            List<LNKPAppendage> existsLnkpaList = LNKPAppendageBL.GetPAppendageByProjectId(projectId, search.CityId);
            return existsLnkpaList.ToJson();
        }

        //获取楼盘附属房屋信息forMCAS kujj20150714
        [OverflowAttrbute(ApiType.Project)]
        public static string GetMCASProjectSubHouse(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int cityid = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            long buildingid = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            long houseid = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            DataTable dt = DatProjectBL.GetMCASProjectSubHouse(projectid, buildingid, houseid, cityid, search.FxtCompanyId, search.SysTypeCode);
            return dt == null || dt.Rows.Count <= 0 ? "" : dt.ToJson();
        }

        //获取楼盘附属房屋价格 tanql20150908
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectSubHouse(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DataTable dt = DatProjectBL.GetProjectSubHouse(projectid, search);
            return dt == null || dt.Rows.Count <= 0 ? "" : dt.ToJson();
        }

        //获取装修单价列表 tanql20150909
        [OverflowAttrbute(ApiType.Project)]
        public static string GetFitmentPriceList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            DataTable dt = DatProjectBL.GetFitmentPriceList(search);
            return dt == null || dt.Rows.Count <= 0 ? "" : dt.ToJson();
        }

        //获取楼盘详细(包含codeName) tanql20150911
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectDetailInfo(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DataSet ds = DatProjectBL.GetProjectDetailInfo(projectId, search);

            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
            //DATProject project = DatProjectBL.GetProjectDetailInfo(projectId, search);
            //return project == null ? "" : project.ToJson();
        }

        //获取楼栋详细 tanql20150911
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingDetailInfo(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            DataSet ds = DatBuildingBL.GetBuildingDetailInfo(buildingId, search);

            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        //获取房号详细 tanql20150911
        [OverflowAttrbute(ApiType.House)]
        public static string GetHouseDetailInfo(JObject funinfo, UserCheck company)
        {

            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            search.FxtCompanyId = company.parentshowdatacompanyid; ;
            int houseid = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            DataSet ds = DatHouseBL.GetHouseDetailInfo(houseid, search);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();

        }

        /// <summary>
        /// 根据多个楼盘ID获取楼盘楼栋房号信息
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectBuildingHouseByProjectIds(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string projectIds = funinfo.Value<string>("projectids");
            List<DATProject> ds = DatProjectBL.GetProjectBuildingHouseByProjectIds(projectIds, search);
            return ds.ToJson();
        }

        /// <summary>
        /// 根据楼盘ID获取楼栋、房号数量
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectBuildingHouseTotal(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DatProjectTotal ds = DatProjectBL.GetProjectBuildingHouseTotal(projectId, search);
            return ds.ToJson();
        }

        /// <summary>
        /// 获取楼盘数量
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectCountByCityId(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;

            int count = DatProjectBL.GetProjectCountByCityId(search);
            return new { projectcount = count }.ToJson();
        }

        /// <summary>
        /// 获取日志文件存放路径
        /// zhoub 20160627
        /// </summary>
        /// <returns></returns>
        private static string GetFilePath()
        {
            var reportDirectory = string.Format("~/Log/huidu/{0}/", DateTime.Now.ToString("yyyy-MM"));

            reportDirectory = System.Web.Hosting.HostingEnvironment.MapPath(reportDirectory);

            if (!System.IO.Directory.Exists(reportDirectory))
            {
                System.IO.Directory.CreateDirectory(reportDirectory);
            }
            return string.Format("{0}report_{1}.log", reportDirectory, DateTime.Now.Day);
        }

        public static string GetProjectMatchList(JObject funinfo, UserCheck company)
        {
            SearchBase searchBase = DataCenterCommon.InitSearBase(funinfo);
            searchBase.FxtCompanyId = company.parentshowdatacompanyid;
            searchBase.SysTypeCode = company.parentproducttypecode;
            searchBase.OrderBy = string.IsNullOrEmpty(searchBase.OrderBy) ? "id" : searchBase.OrderBy;
            string netName = funinfo.Value<string>("netname");
            return ProjectMatchBL.GetProjectMatchList(searchBase, netName).ToJson();
        }
    }
}
