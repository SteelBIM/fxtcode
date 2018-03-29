using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.Common;
using FXT.DataCenter.Infrastructure.Common.CommonWeb;
using FXT.DataCenter.Infrastructure.Common.Dictionary;
using FXT.DataCenter.WebUI.Infrastructure.WebSecurity;
using FXT.DataCenter.WebUI.Tools;
using OfficeOpenXml;
using Webdiyer.WebControls.Mvc;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.WebUI.Areas.Business.Controllers
{
    [Authorize]
    public class BusinessCircleController : BaseController
    {
        private readonly IBusinessCircle _businessCircle;
        private readonly IBusinessStreet _businessStreet;
        private readonly ILog _log;
        private readonly IDropDownList _dropDownList;

        public BusinessCircleController(ILog log, IBusinessCircle businessCircle, IDropDownList dropDownList, IBusinessStreet businessStreet)
        {
            this._businessStreet = businessStreet;
            this._businessCircle = businessCircle;
            this._log = log;
            this._dropDownList = dropDownList;
        }

        public ActionResult Index(string name, int? pageIndex)
        {

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.查看自己, SYS_Code_Dict.页面权限.查看全部, out operate);
            if (operate == (int)PermissionLevel.None) return View();
            if (operate == (int)PermissionLevel.All) self = false;

            var model = new SYS_SubArea_Biz
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                SubAreaName = name
            };
            var pageSize = 30;
            int totalCount;
            var result = _businessCircle.GetSubAreaBiz(model, pageIndex ?? 1, pageSize, out totalCount, self);
            var viewModel = new PagedList<SYS_SubArea_Biz>(result, pageIndex ?? 1, pageSize, totalCount);

            return View(viewModel);
        }

        // 商圈中的商业街数
        public JsonResult BusinessStreetCount(int subAreaId)
        {
            var result = _businessStreet.GetProjectCountsBySubAreaId(subAreaId, Passport.Current.CityId,
                Passport.Current.FxtCompanyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        //删除
        [HttpPost]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.删除自己)]
        public ActionResult Delete(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                //return this.Back("请选择要删除的数据！");
                return Json(new { result = false, msg = "删除失败！" });
            }

            try
            {
                var failList = new List<string>();
                foreach (var item in ids)
                {
                    var array = item.Split('#');
                    
                  
                        if (int.Parse(array[0]) != Passport.Current.FxtCompanyId)
                        {
                            failList.Add(array[1]);
                            continue;
                        }
                   
                    _businessCircle.DeleteSubAreaBiz(int.Parse(array[1]));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商圈, SYS_Code_Dict.操作.删除, "", "", "删除商圈", RequestHelper.GetIP());

                return Json(failList.Any() ? new { result = true, msg = "有" + failList.Count + "条数据您无权限删除！" } : new { result = true, msg = "" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCircle/Delete", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return Json(new { result = false, msg = "删除失败！" });
            }

        }

        [NonAction]
        private void BindViewData(int areaId, int typeCode)
        {
            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text", areaId));
            ViewData.Add("TypeCode", new SelectList(GetDictById(SYS_Code_Dict._商圈等级), "Value", "Text", typeCode));
        }

        // 新增
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.新增)]
        public ActionResult Create()
        {
            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            BindViewData(-1, -1);
            return View("Edit");
        }

        [HttpPost]
        public JsonResult IsExistSubAreaBiz(int areaId,int subAreaId,string subAreaName)
        {
            return Json(_businessCircle.IsExistSubAreaBiz(areaId, Passport.Current.FxtCompanyId, subAreaId,
                subAreaName));
        }

        //新增保存
        [HttpPost]
        public ActionResult Create(SYS_SubArea_Biz subAreaBiz)
        {
            try
            {
              
                subAreaBiz.CityId = Passport.Current.CityId;
                subAreaBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                subAreaBiz.CreateDate = DateTime.Now;
                subAreaBiz.Creators = Passport.Current.ID;

                var subAreaId = _businessCircle.AddSubAreaBiz(subAreaBiz);

                var subAreaBizCoordinateList = new List<SYS_SubArea_Biz_Coordinate>();
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                if (!string.IsNullOrWhiteSpace(subAreaBiz.LngOrLat))
                {
                    var xyList = subAreaBiz.LngOrLat.Split('|');
                    if (xyList.Length > 0)
                    {
                        xyList.ToList().ForEach(m => subAreaBizCoordinateList.Add(
                            new SYS_SubArea_Biz_Coordinate
                            {
                                SubAreaId = subAreaId,
                                AreaId = subAreaBiz.AreaId,
                                CityID = cityId,
                                FxtCompanyID = fxtCompanyId,
                                X = Convert.ToDecimal(m.Split(',')[0]),
                                Y = Convert.ToDecimal(m.Split(',')[1])
                            }));

                    }
                    subAreaBizCoordinateList.ForEach(p => _businessCircle.AddSubAreaBizCoordinate(p));
                }

                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商圈, SYS_Code_Dict.操作.新增, "", "", "新增商圈", RequestHelper.GetIP());

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCircle/Create", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }

        //编辑
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var splitArray = id.Split('#');

            var city = _dropDownList.GetCityByCityId(Passport.Current.CityId).FirstOrDefault();
            ViewBag.CityX = city.X > 0 ? city.X : (decimal)114.065643;  //默认深圳坐标
            ViewBag.CityY = city.Y > 0 ? city.Y : (decimal)22.549525; //默认深圳坐标

            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.修改自己, SYS_Code_Dict.页面权限.修改全部, out operate);
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
                if (int.Parse(splitArray[0]) != Passport.Current.FxtCompanyId)
                {
                    return base.AuthorizeWarning("对不起，该条数据您没有修改权限！");
                }
            }

            var result = _businessCircle.GetSubAreaBizById(int.Parse(splitArray[1]));
            BindViewData(result.AreaId, result.TypeCode ?? -1);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(SYS_SubArea_Biz subAreaBiz)
        {
            try
            {

                subAreaBiz.CityId = Passport.Current.CityId;
                subAreaBiz.FxtCompanyId = Passport.Current.FxtCompanyId;
                subAreaBiz.SaveDate = DateTime.Now;
                subAreaBiz.SaveUser = Passport.Current.ID;

                _businessCircle.UpdateSubAreaBiz(subAreaBiz);

                var subAreaBizCoordinateList = new List<SYS_SubArea_Biz_Coordinate>();
                var cityId = Passport.Current.CityId;
                var fxtCompanyId = Passport.Current.FxtCompanyId;
                if (!string.IsNullOrWhiteSpace(subAreaBiz.LngOrLat))
                {
                    var xyList = subAreaBiz.LngOrLat.Split('|');
                    if (xyList.Length > 0)
                    {
                        xyList.ToList().ForEach(m => subAreaBizCoordinateList.Add(
                            new SYS_SubArea_Biz_Coordinate
                            {
                                SubAreaId = subAreaBiz.SubAreaId,
                                AreaId = subAreaBiz.AreaId,
                                CityID = cityId,
                                FxtCompanyID = fxtCompanyId,
                                X = Convert.ToDecimal(m.Split(',')[0]),
                                Y = Convert.ToDecimal(m.Split(',')[1])
                            }));

                    }

                    var list = _businessCircle.GetSubAreaBizCoordinate(subAreaBiz.SubAreaId, subAreaBiz.AreaId, Passport.Current.FxtCompanyId);
                    if (list == 0) subAreaBizCoordinateList.ForEach(p => _businessCircle.AddSubAreaBizCoordinate(p));
                    else subAreaBizCoordinateList.ForEach(p => _businessCircle.UpdateSubAreaBizCoordinate(p));

                }



                _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商圈, SYS_Code_Dict.操作.修改, "", "", "修改商圈", RequestHelper.GetIP());

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("Business/BusinessCircle/Edit", RequestHelper.GetIP(), Passport.Current.ID, Passport.Current.CityId, Passport.Current.FxtCompanyId, ex);
                return base.Back("操作失败！");
            }

        }

        //导出
        [HttpGet]
        public ActionResult Export(string name)
        {
            var self = true;
            int operate;
            Permission.Check(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.导出自己, SYS_Code_Dict.页面权限.导出全部, out operate);
            if (operate == (int)PermissionLevel.None)
            {
                return base.AuthorizeWarning("对不起，您没有导出权限！");
            }
            if (operate == (int)PermissionLevel.All)
            {
                self = false;
            }

            var model = new SYS_SubArea_Biz
            {
                CityId = Passport.Current.CityId,
                FxtCompanyId = Passport.Current.FxtCompanyId,
                SubAreaName = name
            };

            int totalCount;
            var result = _businessCircle.GetSubAreaBiz(model, 1, int.MaxValue, out totalCount, self);

          
            //插入日志
            _log.InsertLog(Passport.Current.CityId, Passport.Current.FxtCompanyId, Passport.Current.ID, Passport.Current.ID, SYS_Code_Dict.功能模块.商圈, SYS_Code_Dict.操作.导出, "", "", "导出商圈", RequestHelper.GetIP());

            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             System.Web.HttpUtility.UrlEncode(Passport.Current.CityName + "_商业_商圈数据_", System.Text.Encoding.GetEncoding("UTF-8")) + (DateTime.Now).ToString("yyyyMMdd") + ".xlsx");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";
            using (var ms = ExcelHandle.ListToExcel(result.ToList()))
            {
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-excel");
            }
        }

        //统计
        [HttpGet]
        [DataCenterActionFilter(SYS_Code_Dict.商业数据分类.商业基础数据, SYS_Code_Dict.页面权限.统计)]
        public ActionResult Statistic(int? areaId)
        {
            ViewData.Add("AreaId", new SelectList(GetAreaName(), "Value", "Text"));
            var statistic = _businessCircle.GetSubAreaBizStatistic(areaId ?? -1, Passport.Current.FxtCompanyId,
                 Passport.Current.CityId);
            var result = statistic.ToPagedList(1, 30);
            return View(result);
        }

        #region 帮助程序

        //excel 导出头部信息
        readonly Action<string> _excelExportHeader = m =>
        {
            var curContext = System.Web.HttpContext.Current;
            curContext.Response.AddHeader("content-disposition",
                                             "attachment;filename*=UTF-8''" +
                                             HttpUtility.UrlEncode(m, System.Text.Encoding.GetEncoding("UTF-8")) + DateTime.Now.ToShortDateString() + ".xls");
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            curContext.Response.Charset = "UTF-8";

        };


        //行政区列表
        [NonAction]
        private IEnumerable<SelectListItem> GetAreaName()
        {
            var area = _dropDownList.GetAreaName(Passport.Current.CityId);
            var areaResult = new List<SelectListItem>();
            area.ToList().ForEach(m =>
                areaResult.Add(
                new SelectListItem { Value = m.AreaId.ToString(), Text = m.AreaName }
                ));
            areaResult.Insert(0, new SelectListItem { Value = "-1", Text = "全市" });
            //this.ViewBag.areaResult = areaResult;
            return areaResult;
        }

        //根据ID查找相应值
        [NonAction]
        private IEnumerable<SelectListItem> GetDictById(int id)
        {
            var casetype = _dropDownList.GetDictById(id);
            var casetypeResult = new List<SelectListItem>();
            casetype.ToList().ForEach(m =>
                casetypeResult.Add(
                new SelectListItem { Value = m.Code.ToString(), Text = m.CodeName }
                ));
            casetypeResult.Insert(0, new SelectListItem { Value = "-1", Text = "--请选择--" });
            return casetypeResult;
        }

        #endregion

    }
}
