namespace FxtDataAcquisition.Web.Controllers
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;

    using FxtDataAcquisition.Common;
    using FxtDataAcquisition.Web.Common;
    using FxtDataAcquisition.Application.Services;
    using FxtDataAcquisition.Application.Interfaces;
    using FxtDataAcquisition.Domain.DTO;
    using FxtDataAcquisition.Domain.Models;
    using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

    using log4net;
    using AutoMapper;

    /// <summary>
    /// 房号
    /// </summary>
    public class HouseController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HouseController));
        public HouseController(IAdminService unitOfWork)
            : base(unitOfWork)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 房号详细列表
        /// </summary>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.ACTION, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult HouseDetails(int buildingId, long allotId, int fxtcompanyId, int cityId, LoginUser userInfo)
        {
            var houseList = _unitOfWork.HouseDetailsRepository.Get(m => m.BuildingId == buildingId).OrderBy(m => m.UnitNo).ThenBy(m => m.FloorNo).ToList();
            var houseTypeCodeList = _unitOfWork.CodeService.HouseTypeCodeManager();//户型
            var frontCodeList = _unitOfWork.CodeService.HouseFrontCodeManager();//朝向
            var sightCodeList = _unitOfWork.CodeService.HouseSightCodeManager();//景观
            var noiseCodeList = _unitOfWork.CodeService.NoiseManager();//噪音情况
            var purposeCodeList = _unitOfWork.CodeService.HousePurposeCodeManager();//用途
            var structureCodeList = _unitOfWork.CodeService.StructureCodeManager();//户型结构
            var vdCodeList = _unitOfWork.CodeService.VDCodeManager();//通风采光
            var subHouseList = _unitOfWork.CodeService.HouseSubHouseTypeManager();//附属房屋类型
            var fitmentCodeList = _unitOfWork.CodeService.HouseFitmentCodeTypeManager();//装修

            List<HouseDetailsDto> hDtos = new List<HouseDetailsDto>();
            houseList.ForEach((o) =>
            {
                var hDto = Mapper.Map<HouseDetails, HouseDetailsDto>(o);
                var h = houseTypeCodeList.Where(m => m.Code == hDto.HouseTypeCode).FirstOrDefault();
                var f = frontCodeList.Where(m => m.Code == hDto.FrontCode).FirstOrDefault();
                var s = sightCodeList.Where(m => m.Code == hDto.SightCode).FirstOrDefault();
                var n = noiseCodeList.Where(m => m.Code == hDto.NoiseCode).FirstOrDefault();
                var p = purposeCodeList.Where(m => m.Code == hDto.PurposeCode).FirstOrDefault();
                var st = structureCodeList.Where(m => m.Code == hDto.StructureCode).FirstOrDefault();
                var v = vdCodeList.Where(m => m.Code == hDto.VDCode).FirstOrDefault();
                var sub = subHouseList.Where(m => m.Code == hDto.SubHouseType).FirstOrDefault();
                var fc = fitmentCodeList.Where(m => m.Code == hDto.FitmentCode).FirstOrDefault();
                hDto.HouseTypeCodeName = h == null ? "" : h.CodeName;
                hDto.FrontCodeName = f == null ? "" : f.CodeName;
                hDto.SightCodeName = s == null ? "" : s.CodeName;
                hDto.NoiseCodeName = n == null ? "" : n.CodeName;
                hDto.PurposeCodeName = p == null ? "" : p.CodeName;
                hDto.StructureCodeName = st == null ? "" : st.CodeName;
                hDto.VDCodeName = v == null ? "" : v.CodeName;
                hDto.SubHouseTypeName = sub == null ? "" : sub.CodeName;
                hDto.FitmentCodeName = fc == null ? "" : fc.CodeName;

                if (hDto.IsShowBuildingArea == 1)
                {
                    hDto.IsShowBuildingAreaName = "是";
                }
                else if (hDto.IsShowBuildingArea == 0)
                {
                    hDto.IsShowBuildingAreaName = "否";
                }
                else
                {
                    hDto.IsShowBuildingAreaName = "";
                }

                if (hDto.Cookroom == 1)
                {
                    hDto.CookroomName = "有";
                }
                else if (hDto.Cookroom == 0)
                {
                    hDto.CookroomName = "无";
                }
                else
                {
                    hDto.CookroomName = "";
                }
                hDtos.Add(hDto);
            });

            var unit = houseList.GroupBy(m => new { m.UnitNo, m.RoomNo }) .Select(m => new HouseDetailsDto() { UnitNo = m.Key.UnitNo, RoomNo = m.Key.RoomNo }) .OrderBy(m => m.UnitNo).ThenBy(m => m.RoomNo).ToList();

            var floorNo = houseList.OrderBy(m => m.FloorNo).GroupBy(m => m.FloorNo).Select(m => m.Key).ToList();

            ViewBag.houseList = hDtos;

            ViewBag.unit = unit;

            ViewBag.floorNo = floorNo;

            ViewBag.floorNoCount = floorNo.Count();

            ViewBag.fxtCompanyId = fxtcompanyId;

            ViewBag.buildingId = buildingId;

            ViewBag.cityId = cityId;

            ViewBag.allotId = allotId;

            ViewBag.IsInsert = _unitOfWork.AllotFlowRepository.GetById(allotId).StateCode == SYSCodeManager.STATECODE_10;//已入库

            return View();
        }

        /// <summary>
        /// 生成房号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult CreateHouseDetails(int buildingId)
        {
            AjaxResult result = new AjaxResult("生成房号详细成功！");
            try
            {
                var houses = _unitOfWork.HouseRepository.Get(m => m.BuildingId == buildingId);

                if (houses != null && houses.Count() > 0)
                {
                    _unitOfWork.HouseDetailsRepository.Delete(m => m.BuildingId == buildingId);

                    foreach (var house in houses)
                    {
                        if (house.EndFloorNo >= house.FloorNo)
                        {
                            string unitNo = _unitOfWork.HouseService.GetUnitNoByUnitNoStr(house.UnitNo);

                            string houseNo = _unitOfWork.HouseService.GetHouseNoByUnitNoStr(house.UnitNo);

                            for (int i = house.FloorNo; i <= house.EndFloorNo; i++)
                            {
                                List<int> cs = new List<int>(){

                                SYSCodeManager.HOUSEPURPOSECODE_5,SYSCodeManager.HOUSEPURPOSECODE_6,SYSCodeManager.HOUSEPURPOSECODE_8,SYSCodeManager.HOUSEPURPOSECODE_27
                            };

                                HouseDetails detail = new HouseDetails();
                                detail.HouseId = house.HouseId;
                                detail.BuildingId = house.BuildingId;
                                detail.FrontCode = house.FrontCode;
                                detail.HouseName = house.HouseName;
                                detail.HouseTypeCode = house.HouseTypeCode;
                                detail.NoiseCode = house.NoiseCode;
                                detail.PurposeCode = house.PurposeCode;
                                detail.SightCode = house.SightCode;
                                detail.StructureCode = house.StructureCode;
                                detail.VDCode = house.VDCode;
                                detail.BuildArea = house.BuildArea;
                                detail.Remark = house.Remark;
                                detail.FloorNo = i;
                                detail.NominalFloor = i.ToString();
                                detail.UnitNo = unitNo;
                                detail.RoomNo = houseNo;

                                if (house.PurposeCode.HasValue && cs.Contains(house.PurposeCode.Value))
                                {
                                    detail.HouseName = unitNo + houseNo;
                                }
                                else
                                {
                                    if (i < 0)
                                    {
                                        detail.HouseName = "-" + unitNo + -i + houseNo;
                                    }
                                    else
                                    {
                                        detail.HouseName = unitNo + i + houseNo;
                                    }
                                }
                                if (detail.BuildArea.HasValue && detail.BuildArea == 0)
                                {
                                    detail.BuildArea = null;
                                }
                                if (i != 0)
                                {
                                    _unitOfWork.HouseDetailsRepository.Insert(detail);
                                }
                            }
                        }
                    }
                    _unitOfWork.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                result.Result = false;

                result.Message = "同一楼栋下不能有相同房号！";
            }
            return AjaxJson(result);
        }

        /// <summary>
        /// 生成房号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult CreateHouseDetailsByHouseId(int houseId)
        {
            AjaxResult result = new AjaxResult("生成房号详细成功！");
            try
            {
                var house = _unitOfWork.HouseRepository.GetById(houseId);

                if (house != null)
                {
                    _unitOfWork.HouseDetailsRepository.Delete(m => m.HouseId == houseId);

                    if (house.EndFloorNo >= house.FloorNo)
                    {
                        string unitNo = _unitOfWork.HouseService.GetUnitNoByUnitNoStr(house.UnitNo);
                        string houseNo = _unitOfWork.HouseService.GetHouseNoByUnitNoStr(house.UnitNo);
                        for (int i = house.FloorNo; i <= house.EndFloorNo; i++)
                        {
                            List<int> cs = new List<int>(){
                                SYSCodeManager.HOUSEPURPOSECODE_5,SYSCodeManager.HOUSEPURPOSECODE_6,SYSCodeManager.HOUSEPURPOSECODE_8,SYSCodeManager.HOUSEPURPOSECODE_27
                            };

                            HouseDetails detail = new HouseDetails();
                            detail.BuildingId = house.BuildingId;
                            detail.FrontCode = house.FrontCode;
                            detail.HouseName = house.HouseName;
                            detail.HouseTypeCode = house.HouseTypeCode;
                            detail.NoiseCode = house.NoiseCode;
                            detail.PurposeCode = house.PurposeCode;
                            detail.SightCode = house.SightCode;
                            detail.StructureCode = house.StructureCode;
                            detail.VDCode = house.VDCode;
                            detail.HouseId = houseId;
                            detail.BuildArea = house.BuildArea;
                            detail.Remark = house.Remark;
                            detail.FloorNo = i;
                            detail.NominalFloor = i.ToString();
                            detail.UnitNo = unitNo;
                            detail.RoomNo = houseNo;
                            if (house.PurposeCode.HasValue && cs.Contains(house.PurposeCode.Value))
                            {
                                detail.HouseName = unitNo + houseNo;
                            }
                            else
                            {
                                if (i < 0)
                                {
                                    detail.HouseName = "-" + unitNo + -i + houseNo;
                                }
                                else
                                {
                                    detail.HouseName = unitNo + i + houseNo;
                                }
                            }

                            if (detail.BuildArea.HasValue && detail.BuildArea == 0)
                            {
                                detail.BuildArea = null;
                            }
                            if (i != 0)
                            {
                                _unitOfWork.HouseDetailsRepository.Insert(detail);
                            }
                        }
                    }
                    _unitOfWork.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                result.Result = false;
                result.Message = "同一楼栋下不能有相同房号！";
            }
            return AjaxJson(result);
        }

        /// <summary>
        /// 新增房号
        /// </summary>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.ACTION, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult AddHouseDetails(HouseDetails house, long allotId, int fxtcompanyId, int cityId, LoginUser loginUser)
        {

            //户型结构
            ViewBag.StructureCode = _unitOfWork.CodeService.StructureCodeManager();
            //通风采光
            ViewBag.VDCode = _unitOfWork.CodeService.VDCodeManager();
            //噪音情况
            ViewBag.Noise = _unitOfWork.CodeService.NoiseManager();
            //用途
            ViewBag.PurposeCode = _unitOfWork.CodeService.HousePurposeCodeManager();

            if (house.Id > 0)
            {
                return View(_unitOfWork.HouseDetailsRepository.GetById(house.Id));
            }
            else
            {
                return View(house);

            }
        }
        /// <summary>
        /// 保存房号
        /// </summary>
        /// <returns></returns>
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult SaveHouseDetails(HouseDetails house, LoginUser userInfo)
        {

            AjaxResult result = new AjaxResult("添加房号成功！");
            if (house.UnitNo == null) house.UnitNo = "";
            if (house.Id < 1)
            {
                var exists = _unitOfWork.HouseDetailsRepository.Get(m => m.HouseName == house.HouseName && m.BuildingId == house.BuildingId).FirstOrDefault();
                if (exists != null)
                {
                    result.Message = "房号已存在";
                    result.Result = false;
                    return AjaxJson(result);
                }

                var fno = _unitOfWork.HouseDetailsRepository.Get(m => m.UnitNo == house.UnitNo && m.RoomNo == house.RoomNo && m.FloorNo == house.FloorNo
    && m.BuildingId == house.BuildingId).FirstOrDefault();
                if (fno != null)
                {
                    result.Message = "楼层已存在";
                    result.Result = false;
                    return AjaxJson(result);
                }

                house.Creator = userInfo.UserName;
                house.SaveUser = userInfo.UserName;
                house.SaveDateTime = DateTime.Now;
                house.HouseId = 0;
                house.Valid = 1;
                _unitOfWork.HouseDetailsRepository.Insert(house);
            }
            else
            {
                var exists = _unitOfWork.HouseDetailsRepository.Get(m => m.HouseName == house.HouseName
            && m.BuildingId == house.BuildingId && m.Id != house.Id).FirstOrDefault();
                if (exists != null)
                {
                    result.Message = "房号已存在";
                    result.Result = false;
                    return AjaxJson(result);
                }

                var fno = _unitOfWork.HouseDetailsRepository.Get(m => m.UnitNo == house.UnitNo && m.RoomNo == house.RoomNo && m.FloorNo == house.FloorNo
                    && m.BuildingId == house.BuildingId && m.Id != house.Id).FirstOrDefault();
                if (fno != null)
                {
                    result.Message = "楼层已存在";
                    result.Result = false;
                    return AjaxJson(result);
                }

                house.SaveUser = userInfo.UserName;
                house.SaveDateTime = DateTime.Now;
                result.Message = "修改成功！";
            }
            _unitOfWork.Commit();
            return AjaxJson(result);
        }

        //房号操作
        [HttpPost]
        [Common.AuthorizeFilterAttribute(NowRequestType = RequestType.AJAX, NowFunctionPageUrl = WebCommon.Url_AllotFlowInfo_AllotFlowManager, OrNowFunctionCodes = new int[] { SYSCodeManager.FunOperCode_1, SYSCodeManager.FunOperCode_2, SYSCodeManager.FunOperCode_3 })]
        public ActionResult EndityHouse(HouseEndity list, LoginUser userInfo)
        {
            if (list == null)
            {
                return this.Back("尚无提交数据");
            }
            #region 新增
            if (list.addHouse != null && list.addHouse.Any())
            {
                try
                {
                    foreach (var item in list.addHouse)
                    {
                        var house = ConvertVal(item, userInfo.UserName);
                        if (house.UnitNo == null)
                        {
                            house.UnitNo = "";
                        }
                        house.Creator = userInfo.UserName;
                        _unitOfWork.HouseDetailsRepository.Insert(house);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("House/EndityHouse/insert", ex);
                }
            }
            #endregion
            #region 修改
            if (list.updateHouse != null && list.updateHouse.Count() > 0)
            {
                var failUpdateList = new List<string>();
                try
                {
                    foreach (var item in list.updateHouse)
                    {
                        var house = ConvertVal(item, userInfo.UserName);
                        var upHouse = _unitOfWork.HouseDetailsRepository.Get(m => m.BuildingId == house.BuildingId && m.HouseName == house.HouseName).FirstOrDefault();
                        if (upHouse != null)
                        {
                            upHouse.BuildArea = house.BuildArea;
                            upHouse.FloorNo = house.FloorNo;
                            upHouse.FrontCode = house.FrontCode;
                            upHouse.HouseName = house.HouseName;
                            upHouse.HouseTypeCode = house.HouseTypeCode;
                            upHouse.NoiseCode = house.NoiseCode;
                            upHouse.PurposeCode = house.PurposeCode;
                            upHouse.Remark = house.Remark;
                            upHouse.SightCode = house.SightCode;
                            upHouse.StructureCode = house.StructureCode;
                            upHouse.UnitNo = house.UnitNo;
                            upHouse.RoomNo = house.RoomNo;
                            upHouse.VDCode = house.VDCode;
                            upHouse.SubHouseType = house.SubHouseType;
                            upHouse.SubHouseArea = house.SubHouseArea;
                            upHouse.IsShowBuildingArea = house.IsShowBuildingArea;
                            upHouse.FitmentCode = house.FitmentCode;
                            upHouse.Cookroom = house.Cookroom;
                            upHouse.Balcony = house.Balcony;
                            upHouse.Toilet = house.Toilet;
                            if (house.UnitNo == null)
                            {
                                upHouse.UnitNo = "";
                            }
                            _unitOfWork.HouseDetailsRepository.Update(upHouse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("House/EndityHouse/update", ex);
                }
            }
            #endregion
            #region 删除
            if (list.deleteHouse != null && list.deleteHouse.Count() > 0)
            {
                var failDelList = new List<string>();
                try
                {
                    foreach (var item in list.deleteHouse)
                    {
                        var house = ConvertVal(item.FloorNo, item.NominalFloor, item.HouseName, item.BuildingId, item.CityID, item.FxtCompanyId, userInfo.UserName);
                        var upHouse = _unitOfWork.HouseDetailsRepository.Get(m => m.BuildingId == house.BuildingId && m.HouseName == house.HouseName
                                        && m.FloorNo == house.FloorNo).FirstOrDefault();
                        if (upHouse != null)
                        {
                            _unitOfWork.HouseDetailsRepository.Delete(upHouse.Id);
                        }
                    }
                }
                catch (Exception ex)
                {

                    log.Error("House/EndityHouse/delete", ex);
                }
            }
            #endregion
            _unitOfWork.Commit();
            return Json(new { msg = "操作成功" });
        }

        /// <summary>
        ///  警告并且历史返回
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ContentResult Back(string notice = null)
        {
            var content = new StringBuilder("<script>");
            if (!string.IsNullOrEmpty(notice))
                content.AppendFormat("alert('{0}');", notice);
            content.Append("history.go(-1)</script>");
            return this.Content(content.ToString());
        }

        #region private

        //转换值 删除
        private HouseDetails ConvertVal(string floorNo, string nominalFloor, string houseName, string buildingId, string cityID, string fxtCompanyId, string userName)
        {
            var house = new HouseDetails();
            try
            {
                house.Creator = userName;
                if (!string.IsNullOrEmpty(floorNo))
                {
                    house.FloorNo = Convert.ToInt32(floorNo);
                }
                if (!string.IsNullOrEmpty(nominalFloor))
                {
                    house.NominalFloor = nominalFloor;
                }
                if (!string.IsNullOrEmpty(buildingId))
                {
                    house.BuildingId = Convert.ToInt32(buildingId);
                }
                if (!string.IsNullOrEmpty(houseName))
                {
                    house.HouseName = houseName;
                }
                if (!string.IsNullOrEmpty(cityID))
                {
                    house.CityID = Convert.ToInt32(cityID);
                }
                if (!string.IsNullOrEmpty(fxtCompanyId))
                {
                    house.FxtCompanyId = Convert.ToInt32(fxtCompanyId);
                }
                return house;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        //转换值 新增、修改
        private HouseDetails ConvertVal(HouseOperate item, string userName)
        {
            try
            {
                var house = new HouseDetails();
                house.SaveUser = userName;
                if (!string.IsNullOrEmpty(item.BuildingId))
                {
                    house.BuildingId = Convert.ToInt32(item.BuildingId);
                }
                if (!string.IsNullOrEmpty(item.HouseName))
                {
                    house.HouseName = item.HouseName;
                }
                if (!string.IsNullOrEmpty(item.BuildArea))
                {
                    house.BuildArea = Convert.ToDecimal(item.BuildArea);
                }
                if (!string.IsNullOrEmpty(item.PurposeCode))
                {
                    house.PurposeCode = GetPurposeCode(item.PurposeCode);
                }
                if (!string.IsNullOrEmpty(item.SubHouseType))
                {
                    house.SubHouseType = GetSubHouseCode(item.SubHouseType);
                }
                if (!string.IsNullOrEmpty(item.SubHouseArea))
                {
                    house.SubHouseArea = Convert.ToDecimal(item.SubHouseArea);
                }
                if (!string.IsNullOrEmpty(item.HouseTypeCode))
                {
                    house.HouseTypeCode = GetHouseTypeCode(item.HouseTypeCode);
                }
                if (!string.IsNullOrEmpty(item.StructureCode))
                {
                    house.StructureCode = GetStructureCode(item.StructureCode);
                }
                if (!string.IsNullOrEmpty(item.UnitPrice))
                {
                    house.UnitPrice = Convert.ToDecimal(item.UnitPrice);
                }
                if (!string.IsNullOrEmpty(item.Weight))
                {
                    house.Weight = Convert.ToDecimal(item.Weight);
                }
                if (!string.IsNullOrEmpty(item.FrontCode))
                {
                    house.FrontCode = GeteFrontCode(item.FrontCode);
                }
                if (!string.IsNullOrEmpty(item.SightCode))
                {
                    house.SightCode = GeteSightCode(item.SightCode);
                }
                if (!string.IsNullOrEmpty(item.IsShowBuildingArea))
                {
                    house.IsShowBuildingArea = GetIsYesCode(item.IsShowBuildingArea);
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
                if (!string.IsNullOrEmpty(item.UnitNo))
                {
                    house.UnitNo = item.UnitNo;
                }
                if (!string.IsNullOrEmpty(item.RoomNo))
                {
                    house.RoomNo = item.RoomNo;
                }
                if (!string.IsNullOrEmpty(item.CityID))
                {
                    house.CityID = Convert.ToInt32(item.CityID);
                }
                if (!string.IsNullOrEmpty(item.Valid))
                {
                    house.Valid = Convert.ToInt32(item.Valid);
                }
                if (!string.IsNullOrEmpty(item.FxtCompanyId))
                {
                    house.FxtHouseId = Convert.ToInt32(item.FxtCompanyId);
                }
                if (!string.IsNullOrEmpty(item.FloorNo))
                {
                    house.FloorNo = Convert.ToInt32(item.FloorNo);
                }
                if (!string.IsNullOrEmpty(item.NominalFloor))
                {
                    house.NominalFloor = item.NominalFloor;
                }
                if (!string.IsNullOrEmpty(item.NoiseCode))
                {
                    house.NoiseCode = GetNoiseCode(item.NoiseCode);
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

        //可估价、面积确认
        private short GetIsYesCode(string name)
        {
            short code = -1;
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

        //获取噪音情况c的Code
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
        #endregion
    }
}