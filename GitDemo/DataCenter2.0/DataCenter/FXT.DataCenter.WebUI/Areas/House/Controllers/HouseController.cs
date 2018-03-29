using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System.Configuration;
using System.Text;
using System.Net;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.WebUI.Areas.House.Controllers
{
    [Authorize]
    public class HouseController : BaseController
    {
        readonly int _productTypeCode = ((int)EnumHelper.Codes.SysTypeCodeDataCenter);
        Exception _msg;
        private readonly IDropDownList _dropDownList;
        private readonly IDAT_Building _build;
        private readonly IDAT_House _house;
        private readonly ILog _log;
        private readonly IShare _share;
        public HouseController(IDAT_House house, ILog log, IDropDownList dropDownList, IDAT_Building build, IShare share)
        {
            this._house = house;
            this._log = log;
            this._dropDownList = dropDownList;
            this._build = build;
            this._share = share;
        }

        [HttpGet]
        public ActionResult Index(int TotalFloor = 0, int BuildId = 0, int fxtCompanyId = 0, int projectid = 0)
        {
            var cp = FxtUserCenterService_GetFPInfo(Passport.Current.FxtCompanyId, Passport.Current.CityId, Passport.Current.ProductTypeCode, Passport.Current.UserName, out _msg, _productTypeCode);
            if (cp == null)
            {
                return View();
            }
            //判断是否有导出权限
            //var ret = _share.IsExport(Passport.Current.CityId, Passport.Current.FxtCompanyId);
            ViewBag.IsExport = (cp.IsExportHose == 1);

            ViewBag.TotalFloor = TotalFloor;
            ViewBag.BuildingId = BuildId;
            ViewBag.FxtCompanyId = fxtCompanyId;
            ViewBag.CityId = Passport.Current.CityId;
            ViewBag.areaid = GetProjectName(projectid.ToString()).areaid;
            ViewBag.areaname = GetProjectName(projectid.ToString()).AreaName;
            ViewBag.projectName = GetProjectName(projectid.ToString()).projectname;
            ViewBag.buildName = GetBuildName(BuildId.ToString());
            ViewBag.projectId = projectid;
            ViewBag.projectPara = fxtCompanyId + "#" + projectid;
            ////判断操作权限是查看自己还是查看全部
            //var self = true;
            //int operate;
            //Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            //if (operate == (int)PermissionLevel.None)
            //{
            //    return View();
            //}
            //if (operate == (int)PermissionLevel.All) self = false;

            var houseList = _house.GetHouseListByBuildingId(Passport.Current.CityId, Passport.Current.FxtCompanyId, BuildId);
            ViewBag.houseList = houseList;
            #region 初始化
            ViewBag.pAveragePrice = "";//楼盘均价
            ViewBag.AveragePrice = "";//楼栋均价
            ViewBag.PriceDetail = "";//价格系数说明
            ViewBag.proWeight = 1.00M; //百分比
            if (houseList != null && houseList.Tables[2] != null && houseList.Tables[2].Rows.Count > 0)
            {
                ViewBag.TotalFloor = houseList.Tables[2].Rows[houseList.Tables[2].Rows.Count - 1]["FloorNo"];
                ViewBag.AveragePrice = houseList.Tables[2].Rows[0]["AveragePrice"] == null ? "" : houseList.Tables[2].Rows[0]["AveragePrice"].ToString();
                ViewBag.PriceDetail = houseList.Tables[2].Rows[0]["PriceDetail"] == null ? "" : houseList.Tables[2].Rows[0]["PriceDetail"].ToString();
                ViewBag.pAveragePrice = houseList.Tables[2].Rows[0]["pAveragePrice"] == null ? "" : houseList.Tables[2].Rows[0]["pAveragePrice"].ToString();
                //ViewBag.TotalFloor=houseList.Tables[2]["FloorNo"]
            }
            if (!string.IsNullOrEmpty(ViewBag.pAveragePrice) && !string.IsNullOrEmpty(ViewBag.AveragePrice))
            {
                ViewBag.proWeight = (Convert.ToDouble(ViewBag.AveragePrice) / Convert.ToDouble(ViewBag.pAveragePrice) * 100).ToString("F2");
            }
            #endregion
            return View();
        }
        //获取房号数量
        public int GetHouseCount(int cityId, int fxtcompanyId, int projectId)
        {
            return _house.GetHouseCount(cityId, fxtcompanyId, projectId);
        }

        public DAT_Project GetProjectName(string projectId)
        {
            return _build.GetProjectNameById(projectId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
        }
        public string GetBuildName(string buildId)
        {
            return _house.GetBuildName(buildId, Passport.Current.CityId, Passport.Current.FxtCompanyId);
        }

        //房号操作
        [HttpPost]
        public ActionResult EndityHouse(DAT_HouseEndity list)
        {
            if (list == null)
            {
                return this.Back("尚无提交数据");
            }
            #region 新增
            if (list.addHouse != null && list.addHouse.Any())
            {
                #region 权限验证
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增, SYS_Code_Dict.页面权限.新增, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有新增权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                #endregion
                try
                {
                    var failAddList = new List<string>();
                    foreach (var item in list.addHouse)
                    {
                        var house = ConvertVal(item);
                        if (self)
                        {
                            if (house.fxtcompanyid != Passport.Current.FxtCompanyId)
                            {
                                failAddList.Add(item.FxtCompanyId);
                                continue;
                            }
                        }
                        house.creator = Passport.Current.UserName;
                        house.createtime = DateTime.Now;
                        house.fxtcompanyid = Passport.Current.FxtCompanyId;
                        _house.AddHouseEndity(house);

                        // 操作日志
                        _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.新增, "", item.HouseName, "新增房号", RequestHelper.GetIP());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/House/EndityHouse", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                }
            }
            #endregion
            #region 修改
            if (list.updateHouse != null && list.updateHouse.Count() > 0)
            {
                #region 权限验证
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有修改权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                #endregion
                var failUpdateList = new List<string>();
                try
                {
                    foreach (var item in list.updateHouse)
                    {
                        var house = ConvertVal(item);
                        if (self)
                        {
                            if (house.fxtcompanyid != Passport.Current.FxtCompanyId)
                            {
                                failUpdateList.Add(item.FxtCompanyId);
                                continue;
                            }
                        }
                        //int result = _house.UpdateHouseEndity(house, Passport.Current.FxtCompanyId);
                        int result = _house.UpdateHouse(house, Passport.Current.FxtCompanyId);
                        _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.修改, house.houseid.ToString(), house.housename, "修改房号", RequestHelper.GetIP());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/House/EndityHouse", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                }
            }
            #endregion
            #region 删除
            if (list.deleteHouse != null && list.deleteHouse.Count() > 0)
            {
                #region 权限验证
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }
                #endregion
                var failDelList = new List<string>();
                try
                {
                    foreach (var item in list.deleteHouse)
                    {
                        var house = ConvertVal(item.FloorNo, item.NominalFloor, item.HouseName, item.BuildingId, item.CityID, item.FxtCompanyId);
                        if (self)
                        {
                            if (house.fxtcompanyid != Passport.Current.FxtCompanyId)
                            {
                                failDelList.Add(item.FxtCompanyId);
                                continue;
                            }
                        }
                        int result = _house.DeleteHouseEndity(house, Passport.Current.FxtCompanyId);

                        _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.删除, "", item.HouseName, "删除房号", RequestHelper.GetIP());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/House/EndityHouse", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                }
            }
            #endregion
            return Json(new { msg = "操作成功" });
        }

        //根据房号Id编辑
        [HttpGet]
        public ActionResult EndityHou(int houseId, int fxtcompanyId)
        {
            var house = _house.GetHouseInfoById(houseId, Passport.Current.FxtCompanyId, Passport.Current.CityId);
            return View(house);
        }

        //修改单个房号记录
        [HttpPost]
        public ActionResult EndityHou(DAT_House house)
        {
            try
            {
                #region 操作权限
                var self = true;
                int operate;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return base.AuthorizeWarning("对不起，您没有修改权限！");
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                if (self)
                {
                    if (house.fxtcompanyid != Passport.Current.FxtCompanyId)
                    {
                        return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                    }
                }
                #endregion

                int result = _house.UpdateHouse(house, Passport.Current.FxtCompanyId);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.修改, house.houseid.ToString(), house.housename, "修改房号", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/House/EndityHou", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return base.CloseThickboxToLoad();
        }

        //新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult AddHouse(int buildId, int floor)
        {
            var house = new DAT_House();
            house.buildingid = buildId;
            house.cityid = Passport.Current.CityId;
            house.fxtcompanyid = Passport.Current.FxtCompanyId;
            house.ListUnitNo = _house.GetUnitNo(buildId, Passport.Current.CityId, Passport.Current.FxtCompanyId).ToList();
            house.totalFloor = floor;
            return View(house);
        }
        public ActionResult AddHouse(DAT_House house)
        {
            try
            {
                _house.AddHouse(house);
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.新增, "", house.housename, "新增房号", RequestHelper.GetIP());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/House/EndityHou", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
            }
            return base.CloseThickboxToLoad();
        }

        //价格系数说明
        [HttpPost]
        public ActionResult EndtityPriceDetail(string buildId, string priceDetail, string fxtcompanyId)
        {
            int result = _build.EndtityPriceDetail(Convert.ToInt32(buildId), priceDetail, Passport.Current.CityId, Convert.ToInt32(fxtcompanyId), Passport.Current.FxtCompanyId, Passport.Current.UserName);
            if (result > 0)
                return Json(new { flag = true });
            else
                return Json(new { flag = false });
        }

        //清空房号
        public ActionResult DeleteAllHouse(int buildingid)
        {
            try
            {
                int operate;
                var self = true;
                Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.删除自己, SYS_Code_Dict.页面权限.删除全部, out operate);
                if (operate == (int)PermissionLevel.None)
                {
                    return Json(new { result = false, msg = "对不起，您没有删除权限！" });
                }
                if (operate == (int)PermissionLevel.All)
                {
                    self = false;
                }

                var houseList = _house.GetHouseInfoByBuild(buildingid, Passport.Current.CityId, Passport.Current.FxtCompanyId);
                var failDelList = new List<string>();
                foreach (var item in houseList)
                {
                    if (self)
                    {
                        if (item.fxtcompanyid != Passport.Current.FxtCompanyId)
                        {
                            failDelList.Add((item.houseid).ToString());
                            continue;
                        }
                    }
                    int result = _house.DeleteHouseEndity(item, Passport.Current.FxtCompanyId);
                }
                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.删除, buildingid.ToString(), "", "删除楼栋所有房号", RequestHelper.GetIP());

                return Json(failDelList.Any() ? new { result = true, msg = "有" + failDelList.Count + "条数据您无权限删除！" } : new { result = true, msg = "删除成功！" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("House/Project/DeleteProject", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "操作失败！" });
            }
        }

        //导出
        public ActionResult Export(int projectid, int buildingid)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.住宅数据分类.住宅基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var houseList = _house.ExportHouseList(projectid, buildingid, Passport.Current.CityId, Passport.Current.FxtCompanyId);
            if (self)
            {
                houseList = houseList.Where(t => t.creator == Passport.Current.UserName).AsQueryable();
            }

            if (houseList.Count() > 0)
            {
                #region header 信息
                System.Web.HttpContext curContext = System.Web.HttpContext.Current;
                curContext.Response.AddHeader("content-disposition",
                                                 "attachment;filename*=UTF-8''" +
                                                 System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_房号基础信息_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                curContext.Response.Charset = "UTF-8";
                #endregion
                try
                {
                    //操作日志
                    _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.房号, SYS_Code_Dict.操作.导出, "", "", "导出房号基础信息", RequestHelper.GetIP());

                    using (var ms = ExcelHandle.ListToExcel(houseList.ToList()))
                    {
                        return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("House/House/Export", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                    return this.Back("操作失败！");
                }
            }
            return base.AuthorizeWarning("对不起，没有需要导出的数据！");
        }

        //转换值 删除
        private DAT_House ConvertVal(string floorNo, string nominalFloor, string houseName, string buildingId, string cityID, string fxtCompanyId)
        {
            var house = new DAT_House();
            try
            {
                house.saveuser = Passport.Current.UserName;
                if (!string.IsNullOrEmpty(floorNo))
                {
                    house.floorno = Convert.ToInt32(floorNo);
                }
                if (!string.IsNullOrEmpty(nominalFloor))
                {
                    house.nominalfloor = nominalFloor;
                }
                if (!string.IsNullOrEmpty(buildingId))
                {
                    house.buildingid = Convert.ToInt32(buildingId);
                }
                if (!string.IsNullOrEmpty(houseName))
                {
                    house.housename = houseName;
                }
                if (!string.IsNullOrEmpty(cityID))
                {
                    house.cityid = Convert.ToInt32(cityID);
                }
                if (!string.IsNullOrEmpty(fxtCompanyId))
                {
                    house.fxtcompanyid = Convert.ToInt32(fxtCompanyId);
                }
                return house;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        //转换值 新增、修改
        private DAT_House ConvertVal(DAT_HouseOperate item)
        {
            try
            {
                var house = new DAT_House();
                house.saveuser = Passport.Current.UserName;
                if (!string.IsNullOrEmpty(item.BuildingId))
                {
                    house.buildingid = Convert.ToInt32(item.BuildingId);
                }
                if (!string.IsNullOrEmpty(item.HouseName))
                {
                    house.housename = item.HouseName;
                }
                if (!string.IsNullOrEmpty(item.BuildArea))
                {
                    house.buildarea = Convert.ToDecimal(item.BuildArea);
                }
                if (!string.IsNullOrEmpty(item.PurposeCode))
                {
                    house.purposecode = GetPurposeCode(item.PurposeCode);
                }
                if (!string.IsNullOrEmpty(item.SubHouseType))
                {
                    house.subhousetype = GetSubHouseCode(item.SubHouseType);
                }
                if (!string.IsNullOrEmpty(item.SubHouseArea))
                {
                    house.subhousearea = Convert.ToDecimal(item.SubHouseArea);
                }
                if (!string.IsNullOrEmpty(item.HouseTypeCode))
                {
                    house.housetypecode = GetHouseTypeCode(item.HouseTypeCode);
                }
                if (!string.IsNullOrEmpty(item.StructureCode))
                {
                    house.structurecode = GetStructureCode(item.StructureCode);
                }
                if (!string.IsNullOrEmpty(item.UnitPrice))
                {
                    house.unitprice = Convert.ToDecimal(item.UnitPrice);
                }
                if (!string.IsNullOrEmpty(item.Weight))
                {
                    house.weight = Convert.ToDecimal(item.Weight);
                }
                if (!string.IsNullOrEmpty(item.FrontCode))
                {
                    house.frontcode = GeteFrontCode(item.FrontCode);
                }
                if (!string.IsNullOrEmpty(item.SightCode))
                {
                    house.sightcode = GeteSightCode(item.SightCode);
                }
                if (!string.IsNullOrEmpty(item.IsEValue))
                {
                    house.isevalue = GetIsYesCode(item.IsEValue);
                }
                if (!string.IsNullOrEmpty(item.IsShowBuildingArea))
                {
                    house.isshowbuildingarea = GetIsYesCode(item.IsShowBuildingArea);
                }
                if (!string.IsNullOrEmpty(item.VDCode))
                {
                    house.VDCode = GetVDNameCode(item.VDCode);
                }
                if (!string.IsNullOrEmpty(item.FitmentCode))
                {
                    house.FitmentCode = GetFitmentCode(item.FitmentCode);
                }
                if (!string.IsNullOrEmpty(item.Cookroom))
                {
                    house.Cookroom = GetIsCookroom(item.Cookroom);
                }
                if (!string.IsNullOrEmpty(item.Balcony))
                {
                    house.Balcony = Convert.ToInt32(item.Balcony);
                }
                if (!string.IsNullOrEmpty(item.Toilet))
                {
                    house.Toilet = Convert.ToInt32(item.Toilet);
                }
                if (!string.IsNullOrEmpty(item.NoiseCode))
                {
                    house.NoiseCode = GetNoiseCode(item.NoiseCode);
                }
                if (!string.IsNullOrEmpty(item.UnitNo))
                {
                    house.unitno = item.UnitNo;
                }
                if (!string.IsNullOrEmpty(item.CityID))
                {
                    house.cityid = Convert.ToInt32(item.CityID);
                }
                if (!string.IsNullOrEmpty(item.Valid))
                {
                    house.valid = Convert.ToInt32(item.Valid);
                }
                if (!string.IsNullOrEmpty(item.FxtCompanyId))
                {
                    house.fxtcompanyid = Convert.ToInt32(item.FxtCompanyId);
                }
                if (!string.IsNullOrEmpty(item.FloorNo))
                {
                    house.floorno = Convert.ToInt32(item.FloorNo);
                }
                if (!string.IsNullOrEmpty(item.NominalFloor))
                {
                    house.nominalfloor = item.NominalFloor;
                }
                return house;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //获取用途code
        private int GetPurposeCode(string purposeName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(purposeName))
            {
                return code;
            }
            switch (purposeName)
            {
                case "普通住宅": code = 1002001; break;
                case "非普通住宅": code = 1002002; break;
                case "公寓": code = 1002003; break;
                case "酒店式公寓": code = 1002004; break;
                case "独立别墅": code = 1002005; break;
                case "联排别墅": code = 1002006; break;
                case "叠加别墅": code = 1002007; break;
                case "双拼别墅": code = 1002008; break;
                case "旅馆": code = 1002009; break;
                case "花园洋房": code = 1002010; break;
                case "老洋房": code = 1002011; break;
                case "新式里弄": code = 1002012; break;
                case "旧式里弄": code = 1002013; break;
                case "商业": code = 1002014; break;
                case "办公": code = 1002015; break;
                case "厂房": code = 1002016; break;
                case "酒店": code = 1002017; break;
                case "仓库": code = 1002018; break;
                case "车位": code = 1002019; break;
                case "综合": code = 1002020; break;
                case "商住": code = 1002021; break;
                case "其他": code = 1002022; break;
                case "经济适用房": code = 1002023; break;
                case "补差商品住房": code = 1002024; break;
                case "地下室,储藏室": code = 1002025; break;
                case "车库": code = 1002026; break;
                case "别墅": code = 1002027; break;
                default: code = -1; break;
            }
            return code;
        }

        //获取附属房类型code
        private int GetSubHouseCode(string subHouseName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(subHouseName))
            {
                return code;
            }
            switch (subHouseName)
            {
                case "地下室":
                    code = 1015001;
                    break;
                case "杂物间":
                    code = 1015002;
                    break;
                case "车库":
                    code = 1015003;
                    break;
                case "摩托车库":
                    code = 1015004;
                    break;
                case "下房":
                    code = 1015005;
                    break;
                case "储藏室":
                    code = 1015006;
                    break;
                case "阁楼":
                    code = 1015007;
                    break;
                case "厦子":
                    code = 1015008;
                    break;
                case "附房":
                    code = 1015009;
                    break;
                case "夹层":
                    code = 1015010;
                    break;
                case "地下车库":
                    code = 1015011;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取户型code
        private int GetHouseTypeCode(string houseTypeName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(houseTypeName))
            {
                return code;
            }
            switch (houseTypeName)
            {
                case "单房":
                    code = 4001001;
                    break;
                case "单身公寓":
                    code = 4001002;
                    break;
                case "一室一厅":
                    code = 4001003;
                    break;
                case "两室一厅":
                    code = 4001004;
                    break;
                case "两室两厅":
                    code = 4001005;
                    break;
                case "三室一厅":
                    code = 4001006;
                    break;
                case "三室两厅":
                    code = 4001007;
                    break;
                case "四室一厅":
                    code = 4001008;
                    break;
                case "四室两厅":
                    code = 4001009;
                    break;
                case "四室三厅":
                    code = 4001010;
                    break;
                case "五室":
                    code = 4001011;
                    break;
                case "六室":
                    code = 4001012;
                    break;
                case "七室及以上":
                    code = 4001013;
                    break;
                case "一室两厅":
                    code = 4001014;
                    break;
                case "两室零厅":
                    code = 4001015;
                    break;
                case "三室零厅":
                    code = 4001016;
                    break;
                case "四室四厅":
                    code = 4001017;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取附户型结构code
        private int GetStructureCode(string structureName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(structureName))
            {
                return code;
            }
            switch (structureName)
            {
                case "平面":
                    code = 2005001;
                    break;
                case "跃式":
                    code = 2005002;
                    break;
                case "复式":
                    code = 2005003;
                    break;
                case "错层":
                    code = 2005004;
                    break;
                case "LOFT":
                    code = 2005005;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取朝向code
        private int GeteFrontCode(string frontName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(frontName))
            {
                return code;
            }
            switch (frontName)
            {
                case "东":
                    code = 2004001;
                    break;
                case "南":
                    code = 2004002;
                    break;
                case "西":
                    code = 2004003;
                    break;
                case "北":
                    code = 2004004;
                    break;
                case "东南":
                    code = 2004005;
                    break;
                case "东北":
                    code = 2004006;
                    break;
                case "西南":
                    code = 2004007;
                    break;
                case "西北":
                    code = 2004008;
                    break;
                case "南北":
                    code = 2004009;
                    break;
                case "东西":
                    code = 2004010;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取景观code
        private int GeteSightCode(string sightName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(sightName))
            {
                return code;
            }
            switch (sightName)
            {
                case "公园景观":
                    code = 2006001;
                    break;
                case "绿地景观":
                    code = 2006002;
                    break;
                case "小区景观":
                    code = 2006003;
                    break;
                case "街景":
                    code = 2006004;
                    break;
                case "市景":
                    code = 2006005;
                    break;
                case "海景":
                    code = 2006006;
                    break;
                case "山景":
                    code = 2006007;
                    break;
                case "江景":
                    code = 2006008;
                    break;
                case "湖景":
                    code = 2006009;
                    break;
                case "无特别景观":
                    code = 2006010;
                    break;
                case "小区绿地":
                    code = 2006011;
                    break;
                case "河景":
                    code = 2006012;
                    break;
                case "有建筑物遮挡":
                    code = 2006013;
                    break;
                case "临高架桥":
                    code = 2006014;
                    break;
                case "临铁路":
                    code = 2006015;
                    break;
                case "临其他厌恶设施":
                    code = 2006016;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //可估价、面积确认
        private int GetIsYesCode(string name)
        {
            int code = -1;
            if (string.IsNullOrEmpty(name))
            {
                return code;
            }
            switch (name)
            {
                case "是":
                    code = 1;
                    break;
                case "否":
                    code = 0;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;

        }

        //有无厨房
        private int GetIsCookroom(string name)
        {
            int code = -1;
            if (string.IsNullOrEmpty(name))
            {
                return code;
            }
            switch (name)
            {
                case "有":
                    code = 1;
                    break;
                case "无":
                    code = 0;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取通风采光code
        private int GetVDNameCode(string vDName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(vDName))
            {
                return code;
            }
            switch (vDName)
            {
                case "全明通透":
                    code = 1216001;
                    break;
                case "采光欠佳":
                    code = 1216002;
                    break;
                case "通风欠佳":
                    code = 1216003;
                    break;
                case "通风采光欠佳":
                    code = 1216004;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取装修的Code
        private int GetFitmentCode(string fitmentName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(fitmentName))
            {
                return code;
            }
            switch (fitmentName)
            {
                case "豪华":
                    code = 6026001;
                    break;
                case "高档":
                    code = 6026002;
                    break;
                case "中档":
                    code = 6026003;
                    break;
                case "普通":
                    code = 6026004;
                    break;
                case "简易":
                    code = 6026005;
                    break;
                case "毛坯":
                    code = 6026006;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        //获取噪音情况的Code
        private int GetNoiseCode(string noiseCodeName)
        {
            int code = -1;
            if (string.IsNullOrEmpty(noiseCodeName))
            {
                return code;
            }
            switch (noiseCodeName)
            {
                case "安静":
                    code = 2025001;
                    break;
                case "较安静":
                    code = 2025002;
                    break;
                case "微吵":
                    code = 2025003;
                    break;
                case "较吵":
                    code = 2025004;
                    break;
                case "很吵":
                    code = 2025005;
                    break;
                default:
                    code = -1;
                    break;
            }
            return code;
        }

        #region 用户中心API
        private CompanyProduct FxtUserCenterService_GetFPInfo(int fxtcompanyid, int cityid, int producttypecode, string username, out Exception msg, int procode)
        {
            string api = ConfigurationManager.AppSettings["fxtusercenterservice"];
            string appid = ConfigurationManager.AppSettings["usercenterserviceappid"];
            string apppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
            string appkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
            string signname = ConfigurationManager.AppSettings["signname"];
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string functionname = "companythirteen";

            string[] pwdArray = { appid, apppwd, signname, time, functionname };
            string code = EncryptHelper.GetMd5(pwdArray, appkey);

            CompanyProduct cp = null;
            msg = null;
            try
            {
                if (!string.IsNullOrEmpty(api))
                {
                    var sinfo = new { appid = appid, apppwd = apppwd, signname = signname, time = time, code = code, functionname = functionname };
                    var info = new
                    {
                        uinfo = new { username, token = "" },
                        appinfo = new ApplicationInfo(procode.ToString()),
                        funinfo = new { companyid = fxtcompanyid, producttypecode = producttypecode, cityid = cityid }
                    };
                    var post = "{\"sinfo\":\"" + JSONHelper.ObjectToJSON(sinfo).Replace("\"", "'") + "\",\"info\":\"" + JSONHelper.ObjectToJSON(info).Replace("\"", "'") + "\"}";
                    var str = APIPostBack(api, post, "application/json");
                    var rtn = JSONHelper.JSONToObject<JSONHelper.ReturnData>(str);
                    if (rtn.returntype > 0)
                    {
                        cp = new CompanyProduct();
                        var returnSInfo = JSONHelper.JSONToObject<dynamic>(rtn.data.ToString())[0];
                        int ParentProductTypeCode = Convert.ToInt32(returnSInfo["parentproducttypecode"]);
                        int ParentShowDataCompanyId = Convert.ToInt32(returnSInfo["parentshowdatacompanyid"]);
                        int IsExportHose = Convert.ToInt32(returnSInfo["isexporthose"]);

                        cp.IsExportHose = IsExportHose;
                        cp.ParentProductTypeCode = ParentProductTypeCode;
                        cp.ParentShowDataCompanyId = ParentShowDataCompanyId;
                        return cp;
                    }
                    msg = new Exception(rtn.returntext.ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex;
            }
            return cp;
        }

        private string APIPostBack(string url, string post, string contentType)
        {
            byte[] postData = Encoding.UTF8.GetBytes(post);
            var client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            client.Headers.Add("ContentLength", postData.Length.ToString());
            //这里url要组装安全标记等参数
            var result = "";
            try
            {
                byte[] responseData = client.UploadData(url, "POST", postData);
                result = Encoding.UTF8.GetString(responseData);
                //找退出原因
                //LogHelper.Info(result);
            }
            catch (Exception ex)
            {
                result = JSONHelper.GetJson(null, 0, ex.Message, ex);
            }
            client.Dispose();
            return result;
        }
        #endregion

    }
}
