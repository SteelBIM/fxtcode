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
using CAS.Entity.BaseDAModels;
using OpenPlatform.Framework.FlowMonitor;
using CAS.Entity.FxtDataCenter;
//using FxtCommon.Openplatform.Data;
//using FxtOpenClient.ClientService;
//using FxtCommon.Openplatform;
//using FxtCommon.Openplatform.GrpcService;
using CAS.Entity.FxtLog;
using System.Diagnostics;


namespace FxtCenterService.Actualize
{
    public partial class DataController
    {
        /// <summary>
        /// 获取楼盘下拉列表MCAS
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetProjectDropDownList_MCAS_ForVQ(JObject funinfo, UserCheck company)
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


            //added by: dpc
            //使用RPC调用数据

            //try
            //{
            //    if (FxtClientService.IfUseRpc())
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
            //                BEncryptId = true,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            AreaId = search.AreaId,
            //            SearchKey = key

            //        };
            //        FxtClientService.GetProjectDropDownListVQ(requestParam, out projectResponse);
            //        if(!projectResponse.ResponseMsg.Success)
            //        {
            //            LogHelper.Info(projectResponse.ResponseMsg.Msg);
            //        }
            //        else
            //        {
            //            LogHelper.Info("获取楼盘列表成功");
            //        }



            //        return projectResponse.ToLowerJson();
            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}

            string serialno = funinfo.Value<string>("serialno");

            key = HttpUtility.UrlDecode(key);
            List<Dictionary<string, object>> list = DatProjectBL.GetProjectDropDownList_MCAS(search, key, buildingname, items, serialno, company.producttypecode, company.companyid);

            foreach (var itemDict in list)
            {
                if (itemDict.ContainsKey("projectid") && itemDict["projectid"] != null)
                {
                    itemDict["projectid"] = EncryptHelper.ProjectIdEncrypt(itemDict["projectid"].ToString());
                }
            }

            return list.ToJson();
        }

        /// <summary>
        /// 获取楼栋下拉列表forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetBuildingBaseInfoList_MCAS_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;//1003036;
            int projectId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("projectid"));
            int avgprice = StringHelper.TryGetInt(funinfo.Value<string>("avgprice"));
            string key = funinfo.Value<string>("key");

            //try
            //{
            //    //added by: dpc
            //    //使用RPC调用数据
            //    if (FxtClientService.IfUseRpc())
            //    {
            //        BuildingDropdownListResponse buildingResponse;
            //        var requestParam = new BuildingRequestParam()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                BEncryptId = true,
            //                CompanyId = search.FxtCompanyId,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            ProjectId = funinfo.Value<string>("projectid"),
            //            AvgPrice = avgprice,

            //        };
            //        FxtClientService.GetBuildingDropdownListVQ(requestParam, out buildingResponse);
            //        if(!buildingResponse.ResponseMsg.Success)
            //        {
            //            LogHelper.Error(buildingResponse.ResponseMsg.Msg);
            //        }


            //        List<DATBuildingOrderBy> _ds = new List<DATBuildingOrderBy>();
            //        foreach (var building in buildingResponse.Buildings)
            //        {
            //            DATBuildingOrderBy data = new DATBuildingOrderBy();
            //            data.buildingid = int.Parse(building.BuildingId);
            //            data.buildingname = building.BuildingName;
            //            data.buildingtypecode = building.BuildingTypeCode;
            //            data.floortotal = (int)building.FloorTotal;
            //            data.isevalue = building.IsEvalue;
            //            data.purposecode = building.PurposeCode;
            //            data.weight = Convert.ToDecimal(building.Weight);
            //            data.totalbuildarea = Convert.ToDecimal(building.TotalBuildArea);
            //            data.housetotal = (int)building.HouseTotal;
            //            data.codename = building.CodeName;
            //            data.averageprice = building.AveragePrice;

            //        }

            //        var _result = OrderByHelper.OrderBy<DATBuildingOrderBy>(_ds, "buildingname");

            //        List<DATBuildingForVQOrderBy> _resultVQ = new List<DATBuildingForVQOrderBy>(_result.Count);

            //        foreach (var itemBuilding in _result)
            //        {
            //            DATBuildingForVQOrderBy datVq = new DATBuildingForVQOrderBy(itemBuilding);
            //            datVq.buildingid = EncryptHelper.ProjectIdEncrypt(datVq.buildingid);
            //            _resultVQ.Add(datVq);
            //        }

            //        return _resultVQ.ToJson();

            //        //return buildingResponse.Buildings.ToLowerJson();
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Info(ex.ToString());
            //    return "";
            //}
            string serialno = funinfo.Value<string>("serialno");

            List<DATBuildingOrderBy> ds = DatBuildingBL.GetBuildingBaseInfoList_MCAS(search, projectId, avgprice, key, serialno, company.producttypecode, company.companyid);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<DATBuildingOrderBy> result = string.IsNullOrEmpty(key) ? OrderByHelper.OrderBy<DATBuildingOrderBy>(ds, "buildingname") : ds;
            List<DATBuildingForVQOrderBy> resultVQ = new List<DATBuildingForVQOrderBy>(result.Count);
            foreach (var itemBuilding in result)
            {
                DATBuildingForVQOrderBy datVq = new DATBuildingForVQOrderBy(itemBuilding);
                datVq.buildingid = EncryptHelper.ProjectIdEncrypt(datVq.buildingid);
                resultVQ.Add(datVq);
            }
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            //LogHelper.Info("楼栋名称加密时间：" + ts2.TotalMilliseconds + "ms.Guild:" + serialno);
            return resultVQ.ToJson();
        }

        /// <summary>
        /// 获取楼层列表forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Building)]
        public static string GetHouseNoList_MCAS_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;//1003036;
            int buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");


            //added by: dpc
            //使用RPC调用数据

            //try
            //{
            //    if (FxtClientService.IfUseRpc())
            //    {
            //        var request = new HouseRequest()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CompanyId = search.FxtCompanyId,
            //                CityId = search.CityId,
            //                BEncryptId = true,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            BuildingId = funinfo.Value<string>("buildingid"),
            //            FloorNo = key,

            //        };


            //        FloorHouseNoListResponse floorHouseResponse;
            //        FxtClientService.GetFloorHouseNoListVQ(request, out floorHouseResponse);

            //        return floorHouseResponse.FloorHouses.Select(x => new { floorno = x.FloorNo, housecnt = x.HouseNoCount }).ToList().ToJson();
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Error(ex.ToString());
            //    return "";
            //}




            key = HttpUtility.UrlDecode(key);
            DataSet ds = DatHouseBL.GetAutoFloorNoList_MCAS(search, buildingId, key, company.producttypecode, company.companyid);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 获取房号下拉列表forMCAS
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetHouseDropDownList_MCAS_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            search.SysTypeCode = company.parentproducttypecode;//1003036;
            int buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            string key = funinfo.Value<string>("key");

            //added by: dpc
            //使用RPC调用数据
            //try
            //{
            //    if (FxtClientService.IfUseRpc())
            //    {
            //        var request = new HouseRequest()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CityId = search.CityId,
            //                CompanyId = search.FxtCompanyId,
            //                BEncryptId = true,
            //                SysTypeCode = search.SysTypeCode
            //            },
            //            BuildingId = funinfo.Value<string>("buildingid"),
            //            FloorNo = floorNo.ToString()
            //        };


            //        HouseDropdownListResponse houseResponse;

            //        FxtClientService.GetHouseDropdownListVQ(request, out houseResponse);

            //        return houseResponse.Houses.Select(x => new
            //        {
            //            houseid = x.HouseId,
            //            housename = x.HouseName,
            //            unitno = x.UnitNo,
            //            buildarea = x.BuildArea,
            //            isevalue = x.IsEvalue,
            //            subhousetype = x.SubHouseType,
            //            subhousearea = x.SubHouseArea,
            //            floorno = x.FloorNo,
            //            frontcode = x.FrontCode,
            //            sightcode = x.SightCode,
            //            purposecode = x.PurposeCode,

            //        }).ToList().ToJson();
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Error(ex.ToString());
            //    return "";
            //}

            string serialno = funinfo.Value<string>("serialno");


            List<DATHouseOrderBy> result = string.IsNullOrEmpty(key) ? OrderByHelper.OrderBy<DATHouseOrderBy>(DatHouseBL.GetHouseDropDownList_MCAS(search, buildingId, floorNo, key, serialno, company.producttypecode, company.companyid), "housename") : DatHouseBL.GetHouseDropDownList_MCAS(search, buildingId, floorNo, key, serialno, company.producttypecode, company.companyid);
            List<DATHouseForVQOrderBy> resultVQ = new List<DATHouseForVQOrderBy>(result.Count);
            foreach (var itemHouse in result)
            {
                DATHouseForVQOrderBy itemVq = new DATHouseForVQOrderBy(itemHouse);
                itemVq.houseid = EncryptHelper.ProjectIdEncrypt(itemVq.houseid);
                resultVQ.Add(itemVq);
            }
            return resultVQ.ToJson();
        }

        /// <summary>
        ///  MCAS自动估价:楼盘
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="sysTypeCode">系统code</param>
        /// <returns>Tables[0]:询价结果</returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetMCASProjectAutoPrice_ForVQ(JObject funinfo, UserCheck company)
        {
            //added by: dpc, 2015-12-30
            //使用RPC调用数据

            //try
            //{
            //    if (FxtClientService.IfUseRpc())
            //    {
            //        var _projectId = funinfo.Value<string>("projectid");
            //        int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            //        int fxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid")) == 0 ? 25 : StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            //        var request = new PriceRequest()
            //        {
            //            SerachParam = new SearchParam()
            //            {
            //                PageIndex = 0,
            //                PageRecords = 15,
            //                CompanyId = fxtCompanyId,
            //                CityId = cityId,
            //                SysTypeCode = company.producttypecode,
            //                BEncryptId = true
            //            },
            //            ProjectId = _projectId,
            //        };

            //        FxtCommon.Openplatform.GrpcService.AutoPrice projectPrice;
            //        FxtClientService.GetProjectPriceVQ(request, out projectPrice);

            //        if (!projectPrice.ResponseMsg.Success)
            //        {
            //            LogHelper.Error(projectPrice.ResponseMsg.Msg);
            //        }

            //        return projectPrice.ToLowerJson();

            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    LogHelper.Error(ex.ToString());
            //    return "";
            //}





            string pid = funinfo.Value<string>("projectid");
            int projectId = EncryptHelper.ProjectIdDecrypt(pid);
            string json = funinfo.ToJson();
            json = json.Replace(pid, projectId.ToString());
            funinfo = JObject.Parse(json);




            //return AutoPrice(funinfo, company, projectId);
            return DataController.GetMCASProjectAutoPrice(funinfo, company);

        }

        /// <summary>
        ///  MCAS自动估价:楼盘
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="sysTypeCode">系统code</param>
        /// <returns>Tables[0]:询价结果</returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetMCASProjectAutoPriceExpress_ForVQ(JObject funinfo, UserCheck company)
        {

            //added by: dpc, 2015-12-31
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc())
            //{
            //    var projectId = funinfo.Value<string>("projectid");
            //    int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            //    int fxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid")) == 0 ? 25 : StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            //    var request = new PriceRequest()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            PageIndex = 0,
            //            PageRecords = 15,
            //            CompanyId = fxtCompanyId,
            //            CityId = cityId,
            //            SysTypeCode = company.producttypecode,
            //            BEncryptId = false
            //        },
            //        ProjectId = projectId,
            //    };

            //    FxtCommon.Openplatform.GrpcService.AutoPrice projectPrice;
            //    FxtClientService.GetProjectPrice(request, out projectPrice);

            //    return projectPrice.ToLowerJson();
            //}

            //int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            return DataController.GetMCASProjectAutoPrice(funinfo, company);

            //return AutoPrice(funinfo, company, projectId);
        }


        /// <summary>
        /// 询价单
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetMCASInquiry_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            //search.FxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            //search.SysTypeCode = company.producttypecode;//1003036;
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;

            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseid = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));






            //int projectId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("projectid"));
            //int buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            //int houseid = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("houseid"));
            DataSet ds = DATProjectAvgPriceBL.GetMCASInquiry_ForVQ(search, projectId, buildingId, houseid);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        ///  MCAS自动估价:房号
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="BuildingArea">面积</param>
        /// <param name="floorcount">总楼层</param>
        /// <param name="floorno">所在层</param>
        /// <param name="FrontCode">朝向</param>
        /// <param name="unitprice">楼盘基准房价</param>
        /// <param name="plprice">底层基准房价</param>
        /// <param name="pmprice">多层基准房价</param>
        /// <param name="psprice">小高层基准房价</param>
        /// <param name="phprice">高层基准房价</param>
        /// <returns>Tables[0]:询价结果</returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetMCASHouseAutoPrice_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.SysTypeCode = company.parentproducttypecode;
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));

            int projectId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("projectid"));

            int buildingId = 0;
            if (!string.IsNullOrEmpty(funinfo.Value<string>("buildingid")))
            {
                buildingId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("buildingid"));
            }
            int houseId = 0;
            if (!string.IsNullOrEmpty(funinfo.Value<string>("houseid")))
            {
                houseId = EncryptHelper.ProjectIdDecrypt(funinfo.Value<string>("houseid"));
            }
            //int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            //int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            //int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int floorcount = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));
            int floorno = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));
            int frontcode = StringHelper.TryGetInt(funinfo.Value<string>("frontcode"));
            decimal buildingarea = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingarea"));
            int unitprice = StringHelper.TryGetInt(funinfo.Value<string>("unitprice"));
            int fxtcompanyid = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            int plprice = StringHelper.TryGetInt(funinfo.Value<string>("plprice"));
            int pmprice = StringHelper.TryGetInt(funinfo.Value<string>("pmprice"));
            int psprice = StringHelper.TryGetInt(funinfo.Value<string>("psprice"));
            int phprice = StringHelper.TryGetInt(funinfo.Value<string>("phprice"));

            //added by: dpc, 2015-12-30
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc())
            //{

            //    var request = new PriceRequest()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            PageIndex = 0,
            //            PageRecords = 15,
            //            CompanyId = search.FxtCompanyId,
            //            CityId = search.CityId,
            //            SysTypeCode = company.producttypecode,
            //            BEncryptId = true
            //        },
            //        HouseId = funinfo.Value<string>("houseid"),
            //        ProjectId = funinfo.Value<string>("projectid"),
            //        BuildingId = funinfo.Value<string>("buildingid"),

            //        Totalfloor = floorcount,
            //        Floornumber = floorno,
            //        Frontcode = frontcode,
            //        Buildingarea = (double)buildingarea,


            //        Unitprice = unitprice,
            //        Plprice = plprice,
            //        Pmprice = pmprice,
            //        Psprice = psprice,
            //        Phprice = phprice,

            //        WeightType = 1
            //    };

            //    FxtCommon.Openplatform.GrpcService.AutoPrice housePrice;
            //    FxtClientService.GetHousePriceVQ(request, out housePrice);


            //    if(housePrice == null)
            //    {
            //        return "";
            //    }

            //    return new { unitprice = housePrice.Unitprice, totalprice = housePrice.Totalprice, estimable = housePrice.Estimable?1:0 }.ToJson();
            //}





            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();//自动估价结果
            int estimable = 1;
            decimal price = 0;
            var rt = DATProjectAvgPriceBL.GetMCASHouseAutoPrice(search, projectId, buildingId, houseId, floorcount, floorno, frontcode, buildingarea
                , unitprice, plprice, pmprice, psprice, phprice, out estimable, out price);

            var log = new AutoPriceLog();//估价记录
            log.AddTime = DateTime.Now;
            log.AutoType = 2;
            log.CityId = search.CityId;
            log.Estimable = estimable;
            log.FxtCompanyId = fxtcompanyid;
            log.ProductTypeCode = company.producttypecode;
            log.ProjectId = projectId;
            log.BuildingArea = buildingarea;
            log.BuildingId = buildingId;
            log.HouseId = houseId;
            log.TotalFloor = floorcount;
            log.FloorNo = floorno;
            log.FrontCode = frontcode;
            log.UnitPrice = price;
            AutoPriceLogBL.Add(log);

            return rt;
        }

        /// <summary>
        ///  MCAS自动估价:房号
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="BuildingArea">面积</param>
        /// <param name="floorcount">总楼层</param>
        /// <param name="floorno">所在层</param>
        /// <param name="FrontCode">朝向</param>
        /// <param name="unitprice">楼盘基准房价</param>
        /// <param name="plprice">底层基准房价</param>
        /// <param name="pmprice">多层基准房价</param>
        /// <param name="psprice">小高层基准房价</param>
        /// <param name="phprice">高层基准房价</param>
        /// <returns>Tables[0]:询价结果</returns>
        [OverflowAttrbute(ApiType.House)]
        public static string GetMCASHouseAutoPriceExpress_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.SysTypeCode = company.parentproducttypecode;
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));

            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int floorcount = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));
            int floorno = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));
            int frontcode = StringHelper.TryGetInt(funinfo.Value<string>("frontcode"));
            decimal buildingarea = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingarea"));
            int unitprice = StringHelper.TryGetInt(funinfo.Value<string>("unitprice"));
            int fxtcompanyid = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            int plprice = StringHelper.TryGetInt(funinfo.Value<string>("plprice"));
            int pmprice = StringHelper.TryGetInt(funinfo.Value<string>("pmprice"));
            int psprice = StringHelper.TryGetInt(funinfo.Value<string>("psprice"));
            int phprice = StringHelper.TryGetInt(funinfo.Value<string>("phprice"));

            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();//自动估价结果
            int estimable = 1;
            decimal price = 0;
            var rt = DATProjectAvgPriceBL.GetMCASHouseAutoPrice(search, projectId, buildingId, houseId, floorcount, floorno, frontcode, buildingarea
                , unitprice, plprice, pmprice, psprice, phprice, out estimable, out price);

            var log = new AutoPriceLog();//估价记录
            log.AddTime = DateTime.Now;
            log.AutoType = 2;
            log.CityId = search.CityId;
            log.Estimable = estimable;
            log.FxtCompanyId = fxtcompanyid;
            log.ProductTypeCode = company.producttypecode;
            log.ProjectId = projectId;
            log.BuildingArea = buildingarea;
            log.BuildingId = buildingId;
            log.HouseId = houseId;
            log.TotalFloor = floorcount;
            log.FloorNo = floorno;
            log.FrontCode = frontcode;
            log.UnitPrice = price;
            AutoPriceLogBL.Add(log);

            return rt;
        }

        /// <summary>
        ///  MCAS自动估价:楼盘、房号估价
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="floorno">所在层</param>
        /// <param name="FrontCode">朝向</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="sysTypeCode">系统code</param>
        /// <returns>Tables[0]:询价结果</returns>
        [OverflowAttrbute(ApiType.Project)]
        public static string GetMCASProjectHouseAutoPrice_ForVQ(JObject funinfo, UserCheck company)
        {
            //LogHelper.Info("楼盘、房号估价");

            SearchBase search = DataCenterCommon.InitSearBase(funinfo);

            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int floorno = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));
            int frontcode = StringHelper.TryGetInt(funinfo.Value<string>("frontcode"));
            int fxtCompanyIdLog = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            decimal buildingarea = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingarea"));
            int fxtCompanyId = company.parentshowdatacompanyid;
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            string useMonth = DateTime.Now.ToString("yyyy-MM") + "-01";
            //string useMonth = "2015-01-01";

            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();//自动估价结果
            #region 估价记录
            int estimable = 1;
            decimal unitPrice = 0;
            var log = new AutoPriceLog();//估价记录
            log.AddTime = DateTime.Now;
            log.AutoType = 3;
            log.CityId = search.CityId;
            log.Estimable = estimable;
            log.FxtCompanyId = fxtCompanyIdLog;
            log.ProductTypeCode = company.producttypecode;
            log.ProjectId = projectId;
            log.BuildingArea = buildingarea;
            log.BuildingId = buildingId;
            log.HouseId = houseId;
            log.TotalFloor = totalfloor;
            log.FloorNo = floorno;
            log.FrontCode = frontcode;
            #endregion

            DataSet ds = DATProjectAvgPriceBL.GetMCASProjectAutoPrice(cityId, projectId, fxtCompanyId, useMonth);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region 新的
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//楼盘均价
                autoPrice.caseavg = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//
                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//
                autoPrice.plprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["LowLayerPrice"].ToString());//
                autoPrice.pmprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MultiLayerPrice"].ToString());//;
                autoPrice.psprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SmallHighLayerPrice"].ToString());//;;
                autoPrice.phprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HighLayerPrice"].ToString());//;;
                autoPrice.psvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SingleVillaPrice"].ToString());//;;
                autoPrice.ppvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["PlatoonVillaPrice"].ToString());//;;
                autoPrice.pmbhprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MoveBackHousePrice"].ToString());//;;
                autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseMinPrice"].ToString());//
                autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseMaxPrice"].ToString());//
                autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseCount"].ToString());//
                autoPrice.startdate = ds.Tables[0].Rows[0]["StartDate"].ToString();//
                autoPrice.enddate = ds.Tables[0].Rows[0]["EndDate"].ToString();//

                int IsJzHousePrice = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["IsJzHousePrice"].ToString());//
                if (autoPrice.unitprice <= 0)
                {
                    //不可估
                    autoPrice.estimable = 0;
                    autoPrice.unitprice = 0;

                    if (IsJzHousePrice == 1)
                    {
                        log.Estimable = -1;
                    }
                    else
                    {
                        log.Estimable = -2;
                    }
                }
                else
                {
                    //可估
                    autoPrice.estimable = 1;
                    if (IsJzHousePrice == 1)
                    {
                        log.Estimable = 6;
                    }
                    else
                    {
                        log.Estimable = 7;
                    }
                    //LogHelper.Info("可估：" + autoPrice.unitprice);
                }

                #endregion
            }

            if (totalfloor < 1)
            {
                //LogHelper.Info("无楼层：");
                DataSet building = DatBuildingBL.GetBuildingDetailInfo(buildingId, search);
                if (building != null && building.Tables.Count > 0 && building.Tables[0].Rows.Count > 0)
                {
                    totalfloor = StringHelper.TryGetInt(building.Tables[0].Rows[0]["TotalFloor"].ToString());
                    log.TotalFloor = totalfloor;
                }
                //LogHelper.Info("根据BuildingId也查楼层：totalfloor=" + totalfloor + ",BuildingId=" + buildingId + ",CityId=" + search.CityId
                //    + ",FxtCompanyId=" + search.FxtCompanyId + ",TypeCode=" + search.SysTypeCode);

            }
            if (autoPrice.estimable == 1 && totalfloor < 1)
            {
                log.UnitPrice = autoPrice.unitprice;
                AutoPriceLogBL.Add(log);
                //LogHelper.Info("autoPrice.estimable == " + autoPrice.estimable + " && totalfloor == " + totalfloor);
                return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.unitprice * (decimal)buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
            }
            //var house = DatHouseBL.GetHouseById(cityId, buildingId, fxtCompanyId, houseId) ?? new DATHouse();

            if (autoPrice.estimable == 1)
            {
                //float buildarea = (float)(house.buildarea.HasValue ? house.buildarea : 0);
                var rt = DATProjectAvgPriceBL.GetMCASHouseAutoPrice(search, projectId, buildingId, houseId, totalfloor,
                    floorno, frontcode, buildingarea, Convert.ToInt32(autoPrice.unitprice),
                    Convert.ToInt32(autoPrice.plprice), Convert.ToInt32(autoPrice.pmprice), Convert.ToInt32(autoPrice.psprice), Convert.ToInt32(autoPrice.phprice), out estimable, out unitPrice);

                log.Estimable = estimable;
                log.UnitPrice = unitPrice;
                AutoPriceLogBL.Add(log);
                return rt;
            }
            else
            {
                AutoPriceLogBL.Add(log);
                return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();//ishouseprice=0楼盘价格，1房号价格
                //return autoPrice.ToJson();
            }

        }

        [OverflowAttrbute(ApiType.Project)]
        public static string GetMCASProjectHouseNameAutoPrice_ForVQ(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);

            string projectName = funinfo.Value<string>("projectname");
            string buildingName = funinfo.Value<string>("buildingname");
            string houseName = funinfo.Value<string>("housename");
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int floorno = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));
            int frontcode = StringHelper.TryGetInt(funinfo.Value<string>("frontcode"));
            int fxtCompanyIdLog = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));
            string serialno = funinfo.Value<string>("serialno");

            decimal buildingarea = StringHelper.TryGetDecimal(funinfo.Value<string>("buildingarea"));
            int fxtCompanyId = company.parentshowdatacompanyid;
            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            search.AreaId = areaId;

            string useMonth = DateTime.Now.ToString("yyyy-MM") + "-01";

            CAS.Entity.AutoPrice autoPrice = new CAS.Entity.AutoPrice();//自动估价结果

            DATProject project = DataController.GetProject(search, projectName, serialno);

            if (project == null)
            {
                project = DataController.GetProject(search, projectName, serialno);
            }


            if (project == null)
            {
                return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();//ishouseprice=0楼盘价格，1房号价格
            }

            #region 估价记录
            int estimable = 1;
            decimal unitPrice = 0;
            var log = new AutoPriceLog();//估价记录
            log.AddTime = DateTime.Now;
            log.AutoType = 3;
            log.CityId = search.CityId;
            log.Estimable = estimable;
            log.FxtCompanyId = fxtCompanyIdLog;
            log.ProductTypeCode = company.producttypecode;
            log.ProjectId = project.projectid;
            log.BuildingArea = buildingarea;
            //log.BuildingId = buildingId;
            //log.HouseId = houseId;
            log.TotalFloor = totalfloor;
            log.FloorNo = floorno;
            log.FrontCode = frontcode;
            #endregion


            DataSet ds = DATProjectAvgPriceBL.GetMCASProjectAutoPrice(cityId, project.projectid, fxtCompanyId, useMonth);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region 新的
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//楼盘均价
                autoPrice.caseavg = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//
                autoPrice.avgprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["ProjectAvgPrice"].ToString());//
                autoPrice.plprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["LowLayerPrice"].ToString());//
                autoPrice.pmprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MultiLayerPrice"].ToString());//;
                autoPrice.psprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SmallHighLayerPrice"].ToString());//;;
                autoPrice.phprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["HighLayerPrice"].ToString());//;;
                autoPrice.psvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["SingleVillaPrice"].ToString());//;;
                autoPrice.ppvprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["PlatoonVillaPrice"].ToString());//;;
                autoPrice.pmbhprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["MoveBackHousePrice"].ToString());//;;
                autoPrice.casemax = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseMinPrice"].ToString());//
                autoPrice.casemin = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseMaxPrice"].ToString());//
                autoPrice.casecount = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["CaseCount"].ToString());//
                autoPrice.startdate = ds.Tables[0].Rows[0]["StartDate"].ToString();//
                autoPrice.enddate = ds.Tables[0].Rows[0]["EndDate"].ToString();//

                int IsJzHousePrice = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["IsJzHousePrice"].ToString());//
                if (autoPrice.unitprice <= 0)
                {
                    //不可估
                    autoPrice.estimable = 0;
                    autoPrice.unitprice = 0;

                    if (IsJzHousePrice == 1)
                    {
                        log.Estimable = -1;
                    }
                    else
                    {
                        log.Estimable = -2;
                    }
                }
                else
                {
                    //可估
                    autoPrice.estimable = 1;
                    if (IsJzHousePrice == 1)
                    {
                        log.Estimable = 6;
                    }
                    else
                    {
                        log.Estimable = 7;
                    }
                }

                #endregion
            }

            //无楼栋名称
            if (string.IsNullOrEmpty(buildingName))
            {
                //楼盘可估，返回楼盘价格
                if (autoPrice.estimable == 1)
                {
                    log.UnitPrice = autoPrice.unitprice;
                    AutoPriceLogBL.Add(log);
                    return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.unitprice * (decimal)buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
                }
                else
                {
                    return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();//ishouseprice=0楼盘价格，1房号价格
                }
            }

            DATBuilding building = DatBuildingBL.GetBuildingByName(cityId, project.projectid, fxtCompanyId, buildingName, search.SysTypeCode);

            if (building == null)
            {
                if (autoPrice.estimable == 1)
                {
                    log.UnitPrice = autoPrice.unitprice;
                    AutoPriceLogBL.Add(log);
                    return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.unitprice * (decimal)buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
                }
                else
                {
                    return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();//ishouseprice=0楼盘价格，1房号价格
                }
            }else if ( totalfloor < 1)
            {
                totalfloor = building.totalfloor ?? 0;
                log.TotalFloor = totalfloor;
            }

            //楼盘可估，无总楼层
            if (autoPrice.estimable == 1 && totalfloor < 1)
            {
                log.UnitPrice = autoPrice.unitprice;
                AutoPriceLogBL.Add(log);
                return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.unitprice * (decimal)buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
            }

            if (autoPrice.estimable == 1 && !string.IsNullOrEmpty(houseName))
            {
                DATHouse house = DatHouseBL.GetHouseByName(cityId, building.buildingid, fxtCompanyId, houseName, search.SysTypeCode);

                if (house == null)
                {
                    if (autoPrice.estimable == 1)
                    {
                        log.UnitPrice = autoPrice.unitprice;
                        AutoPriceLogBL.Add(log);
                        return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.unitprice * (decimal)buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
                    }
                    else
                    {
                        return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();//ishouseprice=0楼盘价格，1房号价格
                    }
                }
                else if (floorno < 1)
                {
                    floorno = house.floorno;
                }

                var rt = DATProjectAvgPriceBL.GetMCASHouseAutoPrice(search, project.projectid, building.buildingid, house.houseid, totalfloor,
                    floorno, frontcode, buildingarea, Convert.ToInt32(autoPrice.unitprice),
                    Convert.ToInt32(autoPrice.plprice), Convert.ToInt32(autoPrice.pmprice), Convert.ToInt32(autoPrice.psprice), Convert.ToInt32(autoPrice.phprice), out estimable, out unitPrice);

                log.Estimable = estimable;
                log.UnitPrice = unitPrice;
                AutoPriceLogBL.Add(log);
                return rt;
            }
            else
            {
                if (autoPrice.estimable == 1)
                {
                    log.UnitPrice = autoPrice.unitprice;
                    AutoPriceLogBL.Add(log);
                    return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.unitprice * (decimal)buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
                }
                else
                {
                    return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();//ishouseprice=0楼盘价格，1房号价格
                }
            }

        }


        /// <summary>
        /// VQ押品复估
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCollateralReassessmentForVQ(JObject funinfo, UserCheck company)
        {
            //added by: dpc, 2015-12-31
            //使用RPC调用数据
            //if (FxtClientService.IfUseRpc())
            //{
            //    var _projectId = funinfo.Value<string>("projectid");
            //    int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            //    int fxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid")) == 0 ? 25 : StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));

            //    var request = new PriceRequest()
            //    {
            //        SerachParam = new SearchParam()
            //        {
            //            PageIndex = 0,
            //            PageRecords = 15,
            //            CompanyId = fxtCompanyId,
            //            CityId = cityId,
            //            SysTypeCode = company.producttypecode,
            //            BEncryptId = false
            //        },
            //        ProjectId = _projectId,
            //    };

            //    FxtCommon.Openplatform.GrpcService.AutoPrice projectPrice;
            //    FxtClientService.GetProjectPrice(request, out projectPrice);

            //    return projectPrice.ToLowerJson();
            //}


            SearchBase search = DataCenterCommon.InitSearBase(funinfo);

            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int areaId = StringHelper.TryGetInt(funinfo.Value<string>("areaid"));
            int floorno = StringHelper.TryGetInt(funinfo.Value<string>("floornumber"));
            int totalfloor = StringHelper.TryGetInt(funinfo.Value<string>("totalfloor"));
            int frontcode = StringHelper.TryGetInt(funinfo.Value<string>("frontcode"));
            //float buildingarea = StringHelper.TryGetFloat(funinfo.Value<string>("buildingarea"));
            int distance = StringHelper.TryGetInt(funinfo.Value<string>("distance"));
            decimal x = StringHelper.TryGetDecimal(funinfo.Value<string>("x"));
            decimal y = StringHelper.TryGetDecimal(funinfo.Value<string>("y"));
            DateTime rsd = StringHelper.TryGetDateTime(funinfo.Value<string>("relstartdate"));//关联楼盘估价日期1
            DateTime red = StringHelper.TryGetDateTime(funinfo.Value<string>("relenddate"));//关联楼盘估价日期2
            int fxtCompanyIdLog = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));
            int showlog = StringHelper.TryGetInt(funinfo.Value<string>("showlog"));//显示日志;

            search.FxtCompanyId = company.parentshowdatacompanyid;
            search.SysTypeCode = company.parentproducttypecode;
            search.CityId = cityId;
            search.AreaId = areaId;

            string relstartdate = new DateTime(rsd.Year, rsd.Month, 1).ToString("yyyyMMdd");
            string relenddate = new DateTime(red.Year, red.Month, 1).ToString("yyyyMMdd");

            return DatProjectBL.GetCollateralReassessmentForVQ(search, projectId, buildingId, houseId, floorno, totalfloor, frontcode, relstartdate, relenddate, distance, x, y, fxtCompanyIdLog, company.producttypecode, showlog);
        }

        private static string AutoPrice(JObject funinfo, UserCheck company, int projectId)
        {

            SearchBase search = DataCenterCommon.InitSearBase(funinfo);

            //通过案例计算楼盘价格，底层、多层、小高层、高层的价格写死为0
            return DataController.GetMCASProjectAutoPrice(funinfo, company);
        }

        private static DATProject GetProject(SearchBase search, string projectName, string serialno)
        {
            SearchBase pSearch = new SearchBase()
            {
                AreaId = search.AreaId,
                CityId = search.CityId,
                FxtCompanyId = search.FxtCompanyId,
                SysTypeCode = search.SysTypeCode
            };
            DATProject project = DataController.SerchProject(pSearch, projectName, serialno);

            //if (project == null && projectName.Contains("小区"))
            //{
            //    string pname = projectName.Replace("小区", "");
            //    SearchBase newSearch = new SearchBase()
            //    {
            //        AreaId = search.AreaId,
            //        CityId = search.CityId,
            //        FxtCompanyId = search.FxtCompanyId,
            //        SysTypeCode = search.SysTypeCode
            //    };
            //    project = DataController.SerchProject(newSearch, pname, guid);
            //}

            //if (project == null && projectName.Contains("小区"))
            //{
            //    string pname = projectName.Substring(0, projectName.IndexOf("小区") + 2);
            //    SearchBase newSearch = new SearchBase()
            //    {
            //        AreaId = search.AreaId,
            //        CityId = search.CityId,
            //        FxtCompanyId = search.FxtCompanyId,
            //        SysTypeCode = search.SysTypeCode
            //    };
            //    project = DataController.SerchProject(newSearch, pname, guid);
            //}

            //if (project == null && projectName.Contains("花园"))
            //{
            //    string pname = projectName.Substring(0, projectName.IndexOf("花园") + 2);
            //    SearchBase newSearch = new SearchBase()
            //    {
            //        AreaId = search.AreaId,
            //        CityId = search.CityId,
            //        FxtCompanyId = search.FxtCompanyId,
            //        SysTypeCode = search.SysTypeCode
            //    };
            //    project = DataController.SerchProject(newSearch, pname, guid);
            //}

            //if (project == null && projectName.Contains("公寓"))
            //{
            //    string pname = projectName.Substring(0, projectName.IndexOf("公寓") + 2);
            //    SearchBase newSearch = new SearchBase()
            //    {
            //        AreaId = search.AreaId,
            //        CityId = search.CityId,
            //        FxtCompanyId = search.FxtCompanyId,
            //        SysTypeCode = search.SysTypeCode
            //    };
            //    project = DataController.SerchProject(newSearch, pname, guid);
            //}

            return project;
        }

        private static DATProject SerchProject(SearchBase search, string projectName, string serialno)
        {
            var projects = DatProjectBL.GetProjectDropDownList_MCAS2(search, projectName, "", 10, serialno);

            //if (projects == null || projects.Count == 0)
            //{
            //    search.AreaId = 0;
            //    projects = DatProjectBL.GetProjectDropDownList_MCAS2(search, projectName, "", 10, guid);
            //}

            if (projects != null && projects.Count > 1)
            {
                return null;
            }

            return (projects == null) || projects.Count == 0 ? null : projects[0];
        }

    }

    [Serializable]
    public class DATBuildingForVQOrderBy : DATBuildingOrderBy
    {
        public new string buildingid { get; set; }

        public DATBuildingForVQOrderBy(DATBuildingOrderBy building)
        {
            this.buildingid = building.buildingid.ToString();
            floortotal = building.floortotal;
            housetotal = building.housetotal;
            buildingname = building.buildingname;
            builddate = building.builddate;
            averageprice = building.averageprice;
            isevalue = building.isevalue;
            weight = building.weight;
            totalbuildarea = building.totalbuildarea;
            codename = building.codename;
            purposecode = building.purposecode;
            buildingtypecode = building.buildingtypecode;
            ob_othername = building.ob_othername;
            ob_startnum = building.ob_startnum;
            ob_starletter = building.ob_starletter;
            ob_text = building.ob_text;
            ob_number = building.ob_number;
        }
    }

    [Serializable]
    public class DATHouseForVQOrderBy : DATHouseOrderBy
    {
        public new string houseid { get; set; }

        public DATHouseForVQOrderBy(DATHouseOrderBy house)
        {
            this.houseid = house.houseid.ToString();
            housename = house.housename;
            unitno = house.unitno;
            buildarea = house.buildarea;
            isevalue = house.isevalue;
            subhousetype = house.subhousetype;
            subhousearea = house.subhousearea;
            floorno = house.floorno;
            frontcode = house.frontcode;
            sightcode = house.sightcode;
            purposecode = house.purposecode;
            rownum = house.rownum;
            recordcount = house.recordcount;
            ob_othername = house.ob_othername;
            ob_startnum = house.ob_startnum;
            ob_starletter = house.ob_starletter;
            ob_text = house.ob_text;
            ob_number = house.ob_number;

        }
    }

}
