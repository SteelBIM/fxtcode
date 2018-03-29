using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using FxtCenterService.Logic;
using CAS.Entity;
using Newtonsoft.Json.Linq;
using CAS.Entity.DBEntity;
using OpenPlatform.Framework.FlowMonitor;
using FxtOpenClient.ClientService;
using FxtCommon.Openplatform.GrpcService;
using FxtCommon.Openplatform.Data;

namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 获取商业案例列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string projectname = funinfo.Value<string>("projectname");
            decimal minBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("minbuildingarea"));
            decimal maxBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("maxbuildingarea"));
            decimal minUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("minunitprice"));
            decimal maxUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("maxunitprice"));
            //string address = funinfo.Value<string>("address");
            int caseTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("casetypecode"));

            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subareaid = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));

            DateTime startCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("startcasedate"));
            DateTime endCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("endcasedate"));

            int structurecode = StringHelper.TryGetInt(funinfo.Value<string>("structurecode"));
            string structurecodename = funinfo.Value<string>("structurecodename");
            int iselevator = StringHelper.TryGetInt(funinfo.Value<string>("iselevator"));

            if (string.IsNullOrEmpty(projectname.Trim()))   //必填
            {
                return "".ToJson();
            }
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
                CaseListBizResponse caseListBizResponse;
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
                    MinUnitPrice = Convert.ToDouble(minUnitPrice),
                    MaxUnitPrice = Convert.ToDouble(maxUnitPrice),
                    CaseTypeCode = caseTypeCode,
                    AreaId = areaid,
                    SubAreaId = subareaid,
                    StartCaseDate = startCaseDate.ToString("yyyy/MM/dd"),
                    EndCaseDate = endCaseDate.ToString("yyyy/MM/dd"),
                    StructureCode = structurecode,
                    Iselevator = iselevator,
                    StructureCodeName = string.IsNullOrEmpty(structurecodename) ? "" : structurecodename
                };

                FxtClientService.GetCaseListBiz(request, out caseListBizResponse);
                return caseListBizResponse.Cases.ToLowerJson();
            }

            var list = DatBizOfficeBL.GetCaseListBiz(search, projectname, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);

            return list.ToJson();
        }

        /// <summary>
        /// 获取商业案例列表forMCAS
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListBiz_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            int areaid = !string.IsNullOrEmpty(funinfo.Value<object>("areaid").ToString()) ? StringHelper.TryGetInt(funinfo.Value<object>("areaid").ToString()) : -1;
            int subareaid = !string.IsNullOrEmpty(funinfo.Value<object>("subareaid").ToString()) ? StringHelper.TryGetInt(funinfo.Value<object>("subareaid").ToString()) : -1;
            int casetype = !string.IsNullOrEmpty(funinfo.Value<object>("casetype").ToString()) ? StringHelper.TryGetInt(funinfo.Value<object>("casetype").ToString()) : 0;
            DateTime casestatime = StringHelper.TryGetDateTime(funinfo.Value<object>("casestatime").ToString());
            DateTime caseendtime = StringHelper.TryGetDateTime(funinfo.Value<object>("caseendtime").ToString());

            string projectname = funinfo.ToString().Contains("\"projectname\":") ? funinfo.Value<object>("projectname").ToString() : "";
            decimal? buildstaarea = funinfo.ToString().Contains("\"buildstaarea\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("buildstaarea").ToString()) : 0;
            decimal? buildendarea = funinfo.ToString().Contains("\"buildendarea\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("buildendarea").ToString()) : 0;
            decimal? pricesta = funinfo.ToString().Contains("\"pricesta\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("pricesta").ToString()) : 0;
            decimal? priceend = funinfo.ToString().Contains("\"priceend\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("priceend").ToString()) : 0;
            if (search.PageIndex == 0)
            {
                search.Page = true;
                search.PageIndex = 1;
                search.PageRecords = 15;
            }

            var list = DatBizOfficeBL.GetCaseListBiz_MCAS(search, areaid, subareaid, casetype, casestatime, caseendtime, projectname, buildstaarea, buildendarea, pricesta, priceend);
            return list.ToJson();
        }

        /// <summary>
        /// 获取商业案例详情
        /// </summary>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseInfoBiz(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string ids = funinfo.Value<object>("ids").ToString();
            if (ids == string.Empty)
            {
                return "";
            }
            int[] idArray = ids.Split(',').Select(StringHelper.TryGetInt).ToArray();

            var result = DatBizOfficeBL.GetCaseInfoBiz(search, idArray);
            if (result == null || result.Rows.Count < 1)
            {
                return "";
            }
            return result.ToJson();
        }

        /// <summary>
        /// 获取商业案例详情forMCAS
        /// </summary>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseInfoBiz_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            int id = !string.IsNullOrEmpty(funinfo.Value<object>("id").ToString()) ? StringHelper.TryGetInt(funinfo.Value<object>("id").ToString()) : -1;

            var result = DatBizOfficeBL.GetCaseInfoBiz_MCAS(search, id);
            return result.ToJson();
        }

        /// <summary>
        /// 获取办公案例列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListOffice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string projectname = funinfo.Value<string>("projectname");
            decimal minBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("minbuildingarea"));
            decimal maxBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("maxbuildingarea"));
            decimal minUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("minunitprice"));
            decimal maxUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("maxunitprice"));
            int officeType = StringHelper.TryGetInt(funinfo.Value<string>("officetype"));
            int purposeCode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));
            int caseTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("casetypecode"));

            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subareaid = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));

            DateTime startCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("startcasedate"));
            DateTime endCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("endcasedate"));

            int structurecode = StringHelper.TryGetInt(funinfo.Value<string>("structurecode"));
            string structurecodename = funinfo.Value<string>("structurecodename");
            int iselevator = StringHelper.TryGetInt(funinfo.Value<string>("iselevator"));

            if (string.IsNullOrEmpty(projectname.Trim()))   //必填
            {
                return "".ToJson();
            }
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
                CaseListOfficeResponse caseListOfficeResponse;
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
                    MinUnitPrice = Convert.ToDouble(minUnitPrice),
                    MaxUnitPrice = Convert.ToDouble(maxUnitPrice),
                    OfficeType = officeType,
                    PurposeCode = purposeCode,
                    CaseTypeCode = caseTypeCode,
                    AreaId = areaid,
                    SubAreaId = subareaid,
                    StartCaseDate = startCaseDate.ToString("yyyy/MM/dd"),
                    EndCaseDate = endCaseDate.ToString("yyyy/MM/dd"),
                    StructureCode = structurecode,
                    Iselevator = iselevator,
                    StructureCodeName = string.IsNullOrEmpty(structurecodename) ? "" : structurecodename
                };

                FxtClientService.GetCaseListOffice(request, out caseListOfficeResponse);
                return caseListOfficeResponse.Cases.ToLowerJson();
            }

            var list = DatBizOfficeBL.GetCaseListOffice(search, projectname, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, officeType, purposeCode, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);

            return list.ToJson();
        }
        /// <summary>
        /// 获取办公案例列表forMCAS
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListOffice_MCAS(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;
            //int cityid = StringHelper.TryGetInt(funinfo.Value<object>("cityid").ToString());
            int areaid = !string.IsNullOrEmpty(funinfo.Value<object>("areaid").ToString()) ? StringHelper.TryGetInt(funinfo.Value<object>("areaid").ToString()) : -1;
            int subareaid = !string.IsNullOrEmpty(funinfo.Value<object>("subareaid").ToString()) ? StringHelper.TryGetInt(funinfo.Value<object>("subareaid").ToString()) : -1;
            int casetype = StringHelper.TryGetInt(funinfo.Value<object>("casetype").ToString());
            DateTime casestatime = StringHelper.TryGetDateTime(funinfo.Value<object>("casestatime").ToString());
            DateTime caseendtime = StringHelper.TryGetDateTime(funinfo.Value<object>("caseendtime").ToString());

            string projectname = funinfo.ToString().Contains("\"projectname\":") ? funinfo.Value<object>("projectname").ToString() : "";
            decimal? buildstaarea = funinfo.ToString().Contains("\"buildstaarea\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("buildstaarea").ToString()) : 0;
            decimal? buildendarea = funinfo.ToString().Contains("\"buildendarea\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("buildendarea").ToString()) : 0;
            decimal? pricesta = funinfo.ToString().Contains("\"pricesta\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("pricesta").ToString()) : 0;
            decimal? priceend = funinfo.ToString().Contains("\"priceend\":") ? StringHelper.TryGetDecimal(funinfo.Value<object>("priceend").ToString()) : 0;
            if (search.PageIndex == 0)
            {
                search.Page = true;
                search.PageIndex = 1;
                search.PageRecords = 15;
            }

            var list = DatBizOfficeBL.GetCaseListOffice_MCAS(search, areaid, subareaid, casetype, casestatime, caseendtime, projectname, buildstaarea, buildendarea, pricesta, priceend);
            return list.ToJson();
        }

        /// <summary>
        /// 获取办公案例详情
        /// </summary>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseInfoOffice(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string ids = funinfo.Value<object>("ids").ToString();
            if (ids == string.Empty)
            {
                return "";
            }
            int[] idArray = ids.Split(',').Select(StringHelper.TryGetInt).ToArray();
            var result = DatBizOfficeBL.GetCaseInfoOffice(search, idArray);
            if (result == null || result.Rows.Count < 1)
            {
                return "";
            }
            return result.ToJson();
        }

        /// <summary>
        /// 获取土地案例列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListLand(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string landno = funinfo.Value<string>("landno");
            string landPurposeDesc = funinfo.Value<string>("landpurposedesc");
            decimal minBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("minbuildingarea"));
            decimal maxBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("maxbuildingarea"));

            int areaid = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int subareaid = StringHelper.TryGetInt(funinfo.Value<string>("subareaid"));

            decimal minUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("minunitprice"));
            decimal maxUnitPrice = StringHelper.TryGetDecimal(funinfo.Value<string>("maxunitprice"));

            DateTime startCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("startcasedate"));
            DateTime endCaseDate = StringHelper.TryGetDateTime(funinfo.Value<string>("endcasedate"));

            string landclassname = funinfo.Value<string>("landclassname");
            string landclasscode = funinfo.Value<string>("landclasscode");

            if (string.IsNullOrEmpty(landno.Trim()))   //必填
            {
                return "";
            }
            //默认只取15条记录
            if (search.PageIndex == 0)
            {
                search.Page = true;
                search.PageIndex = 1;
                search.PageRecords = 15;
            }

            //zhoub 20160504
            //使用RPC调用数据
            if (FxtClientService.IfUseRpc(company.producttypecode, company.companyid))
            {
                CaseListLandResponse caseListLandResponse;
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
                    LandNo = string.IsNullOrEmpty(landno) ? "" : landno,
                    MinBuildingArea = Convert.ToDouble(minBuildingArea),
                    MaxBuildingArea = Convert.ToDouble(maxBuildingArea),
                    MinUnitPrice = Convert.ToDouble(minUnitPrice),
                    MaxUnitPrice = Convert.ToDouble(maxUnitPrice),
                    LandPurposeDesc = string.IsNullOrEmpty(landPurposeDesc) ? "" : landPurposeDesc,
                    AreaId = areaid,
                    SubAreaId = subareaid,
                    StartCaseDate = startCaseDate.ToString("yyyy/MM/dd"),
                    EndCaseDate = endCaseDate.ToString("yyyy/MM/dd"),
                    Landclasscode = string.IsNullOrEmpty(landclasscode) ? "" : landclasscode,
                    Landclassname = string.IsNullOrEmpty(landclassname) ? "" : landclassname
                };

                FxtClientService.GetCaseListLand(request, out caseListLandResponse);
                return caseListLandResponse.Cases.ToLowerJson();
            }

            var list = DatBizOfficeBL.GetCaseListLand(search, landno, landclassname, landclasscode, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, landPurposeDesc, areaid, subareaid, startCaseDate, endCaseDate);
            return list.ToJson();
        }

        /// <summary>
        /// 获取土地案例详情
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseInfoLand(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string ids = funinfo.Value<string>("ids").ToString();
            if (ids == string.Empty)
            {
                return "";
            }
            int[] idArray = ids.Split(',').Select(StringHelper.TryGetInt).ToArray();
            var result = DatBizOfficeBL.GetCaseInfoLand(search, idArray);
            if (result == null || result.Rows.Count < 1)
            {
                return "";
            }
            return result.ToJson();
        }


        /// <summary>
        /// 获取住宅案例详情
        /// </summary>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseInfo(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string ids = funinfo.Value<object>("ids").ToString();
            string result = string.Empty;
            if (ids == string.Empty)
            {
                return result;
            }
            int[] idArray = ids.Split(',').Select(StringHelper.TryGetInt).ToArray();
            result = DatBizOfficeBL.GetCaseInfo(search, idArray);
            return result;
        }

        /// <summary>
        /// 获取工业案例列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseListIndustry(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string projectName = funinfo.Value<string>("projectname");
            decimal minBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("minbuildingarea"));
            decimal maxBuildingArea = StringHelper.TryGetDecimal(funinfo.Value<string>("maxbuildingarea"));
            int purposeCode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));
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

            if (string.IsNullOrEmpty(projectName.Trim()))   //必填
            {
                return "";
            }
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
                CaseListIndustryResponse caseListIndustryResponse;
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
                    ProjectName = string.IsNullOrEmpty(projectName) ? "" : projectName,
                    MinBuildingArea = Convert.ToDouble(minBuildingArea),
                    MaxBuildingArea = Convert.ToDouble(maxBuildingArea),
                    MinUnitPrice = Convert.ToDouble(minUnitPrice),
                    MaxUnitPrice = Convert.ToDouble(maxUnitPrice),
                    PurposeCode = purposeCode,
                    CaseTypeCode = caseTypeCode,
                    AreaId = areaid,
                    SubAreaId = subareaid,
                    StartCaseDate = startCaseDate.ToString("yyyy/MM/dd"),
                    EndCaseDate = endCaseDate.ToString("yyyy/MM/dd"),
                    StructureCode = structurecode,
                    Iselevator = iselevator,
                    StructureCodeName = string.IsNullOrEmpty(structurecodename) ? "" : structurecodename
                };

                FxtClientService.GetCaseListIndustry(request, out caseListIndustryResponse);
                return caseListIndustryResponse.Cases.ToLowerJson();
            }

            var list = DatBizOfficeBL.GetCaseListIndustry(search, projectName, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, purposeCode, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);

            return list.ToJson();
        }

        /// <summary>
        /// 获取工业案例详情
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Case)]
        public static string GetCaseInfoIndustry(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string ids = funinfo.Value<string>("ids").ToString();
            string result = string.Empty;
            if (ids == string.Empty)
            {
                return result;
            }
            int[] idArray = ids.Split(',').Select(StringHelper.TryGetInt).ToArray();
            result = DatBizOfficeBL.GetCaseInfoIndustry(search, idArray);
            return result;
        }
    }
}
