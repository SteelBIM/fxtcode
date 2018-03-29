using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxtCollateralManager.Common;
using FxtCollateralManager.Common.FxtAPI;
using Newtonsoft.Json.Linq;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using System.Reflection;
using FxtNHibernate.DATProjectDomain.Entities;
using FxtNHibernate.FxtDataUserDomain.Entities;
using CAS.Common.MVC4;

namespace FxtCollateralManager.Web.Controllers
{
    public class CollateralController : BaseController
    {
        //
        // GET: /Collateral/

        public ActionResult Index(int uploadfileid=0)
        {
            ViewData["uploadfileid"] = uploadfileid;
            return View();
        }
        #region 省市县区、楼盘用途加载
        //省份
        [HttpPost]
        public ActionResult Province()
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                  "D", _GetProvince, "");
                return Json(result);
            }
        }

        //城市
        [HttpPost]
        public ActionResult City(int id)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("provinceId", id);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetCity, Utils.Serialize(_obj));
                return Json(result);
            }
        }

        //行政区
        [HttpPost]
        public ActionResult Area(int id)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("cityId", id);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetArea, Utils.Serialize(_obj));
                return Json(result);
            }
        }

        //楼盘用途
        [HttpPost]
        public ActionResult ProjectPurposeCode()
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetAllProjectPurposeCode, "");
                JObject jobject = new JObject();
                jobject.Add(new JProperty("data", result));
                return Json(ResultServerJson(Utils.Serialize(jobject)));
            }
        }

        //一次性获得省市区
        [HttpPost]
        public ActionResult ProvinceCityArea(int pid, int cid, int aid)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                string ProvinceName = string.Empty,
                    CityName = string.Empty,
                    AreaName = string.Empty;
                //省份
                JObject _obj = new JObject();
                _obj.Add("provinceId", pid);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetProvinceById, Utils.Serialize(_obj));                
                SYSProvince sysp = Utils.Deserialize<SYSProvince>(result.ToString());
                if (sysp != null)
                    ProvinceName = sysp.ProvinceName;
                //城市
                _obj = new JObject();
                _obj.Add("cityId", cid);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetCityByCityId, Utils.Serialize(_obj));
                SYSCity sysc = Utils.Deserialize<SYSCity>(result.ToString());
                if (sysc != null)
                    CityName = sysc.CityName;
                //行政区
                _obj = new JObject();
                _obj.Add("areaId", aid);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetAreaByAreaId, Utils.Serialize(_obj));
                SYSArea sysa = Utils.Deserialize<SYSArea>(result.ToString());
                if (sysa != null)
                    AreaName = sysa.AreaName;
                return Json(new
                {
                    PName = ProvinceName,
                    CName = CityName,
                    AName = AreaName
                });
            }
        }

        #endregion

        #region 临时楼盘 楼栋 房号添加
        //贷后 添加临时楼盘
        [HttpPost]
        public ActionResult CreateDATAProject(DataProject data)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("dataProject", Utils.Serialize(data));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C", _DATAProjectAdd, Utils.Serialize(_obj));
                return Json(ResultServerJson(result.ToString()));
            }
        }
        //贷后添加临时楼栋
        [HttpPost]
        public ActionResult CreateDATABuilding(DataBuilding data)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("dataBuilding", Utils.Serialize(data));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C", _DATABuildingAdd, Utils.Serialize(_obj));
                return Json(ResultServerJson(result.ToString()));
            }
        }

        //贷后添加临时房号
        [HttpPost]
        public ActionResult CreateDATAHouse(DataHouse data)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("dataHouse", Utils.Serialize(data));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C", _DATAHouseAdd, Utils.Serialize(_obj));
                return Json(ResultServerJson(result.ToString()));
            }
        }
        #endregion

        #region 标准化楼盘新增、获取
        //标准化押品新增
        [HttpPost]
        public ActionResult CreateDATCollateral(DataCollateral data)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                if (data.Id.Equals(0))
                {
                    JObject _obj = new JObject();
                    _obj.Add("dataCollateral", Utils.Serialize(data));
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C", _DATACollateralAdd, Utils.Serialize(_obj));
                }
                else
                {
                    JObject _obj = new JObject();
                    _obj.Add("dataCollateral", Utils.Serialize(data));
                    result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C", _DATADataCollateralUpdate, Utils.Serialize(_obj));
                }
                return Json(ResultServerJson(result.ToString()));
            }
        }
        //标准化押品修改
        [HttpPost]
        public ActionResult UpdateDATCollateral(DataCollateral data)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("dataCollateral", Utils.Serialize(data));
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C",
                    _DATADataCollateralUpdate, Utils.Serialize(_obj));
                return Json(ResultServerJson(result.ToString()));
            }
        }
        //标准化押品列表
        [HttpPost]
        public ActionResult GetCollateral(int pageIndex, int pageSize, string orderName, string orderType
            , int uploadfileid=0)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("pageSize", pageSize);
                _obj.Add("pageIndex", pageIndex);
                _obj.Add("orderProperty", orderName);
                _obj.Add("orderType", orderType);
                _obj.Add("cityarrid", CookieHelper.Get("cityarrid"));
                _obj.Add("itemarrid", CookieHelper.Get("itemarrid"));
                _obj.Add("uploadfileid", uploadfileid);
                _obj.Add("customerid", Public.LoginInfo.CustomerId);
                _obj.Add("customertype", Public.LoginInfo.CustomerType);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C",
                    _DATAGetDataCollateral, Utils.Serialize(_obj));
                List<DataCollaterals> listC = Utils.Deserialize<List<DataCollaterals>>(Utils.GetJObjectValue(result, "data"));
                if (listC != null)
                {
                    return Json(ResultServerJson(Utils.Serialize(GetDataCollaterals(listC))));
                }
                return Json(result);
            }
        }

        [HttpPost]
        //获得相关某个文件的押品列表
        public ActionResult GetDataCollateralByFileId(int fileId, int pageSize, int pageIndex)
        {
            JObject _obj = new JObject();
            _obj.Add("fileId", fileId);
            _obj.Add("pageSize", pageSize);
            _obj.Add("pageIndex", pageIndex);
            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C",
                _DATAGetDataCollateralByFileId, Utils.Serialize(_obj));
                List<DataCollaterals> listC = Utils.Deserialize<List<DataCollaterals>>(Utils.GetJObjectValue(result, "data"));
                return Json(ResultServerJson(Utils.Serialize(GetDataCollaterals(listC))));
            }
        }

        //修改某个文件当前完成页数
        [HttpPost]
        public ActionResult SetFilePageIndex(int fileId, int pageIndex)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                SysUploadFile uploadFile = new SysUploadFile()
                {
                    Id = fileId,
                    PageIndex = pageIndex
                };
                JObject send = new JObject();
                send.Add("model", Serialize(uploadFile));
                send.Add("otype", "U");
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "A", "Uploads", Utils.Serialize(send));
                return Json(ResultServerJson("", (bool)result));
            }
        }

        /// <summary>
        /// 处理已标准化列表中的相关列信息
        /// </summary>
        /// <param name="list">标准化集合对象</param>
        /// <returns></returns>
        private JObject GetDataCollaterals(List<DataCollaterals> list)
        {
            string[] filter = { "recordcount", "CustomPrimaryKeyIdentify", "IsSetCustomerFields" };
            foreach (var item in list)
            {
                if (!item.ProjectId.Equals(0))
                    item.ProjectNameMatch = true;
                if (item.BuildingId != null && !item.BuildingId.Value.Equals(0))
                    item.BuildingNumberMatch = true;
                if (item.RoomId != null && !item.RoomId.Value.Equals(0))
                    item.RoomNumberMatch = true;
            }
            JObject objs = JObject.Parse(result.ToString());
            objs["data"] = Utils.Serialize(list);
            return objs;
        }
        //获得自定义列值
        [HttpPost]
        public ActionResult GetCutsomColumnVal(int cid, int pid, string cname)
        {

            JObject _obj = new JObject();
            _obj.Add("cId", cid);
            _obj.Add("projectId", pid);
            _obj.Add("columnName", cname);

            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C",
                _DATGetCustomColumnsValue, Utils.Serialize(_obj));
                return Json(result);
            }
        }
        //保存自定义列值
        [HttpPost]
        public ActionResult SetCutsomColumnVal(int cid, int type, string data)
        {
            JObject _obj = new JObject();
            _obj.Add("data", data);
            _obj.Add("cid", cid);
            _obj.Add("type", type);

            using (FxtAPIClient client = new FxtAPIClient())
            {
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey), "C",
                _DATUpdateCustomColumnsValue, Utils.Serialize(_obj));
                return Json(result);
            }
        }
        #endregion

        #region 搜索

        //楼盘模糊搜索
        [HttpPost]
        public ActionResult Search(string q, int cityId, int areaId)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("cId", cityId);
                _obj.Add("aId", areaId);
                _obj.Add("pName", q);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetProjectByCityIDAndLikePrjName, Utils.Serialize(_obj));

                return Json(result.ToString());
            }
        }

        //楼栋模糊搜索
        [HttpPost]
        public ActionResult SearchBuilding(string q, int cityId, int pId)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("cId", cityId);
                _obj.Add("pId", pId);
                _obj.Add("bName", q);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetBuildingByProjectIdCityIDAndLikeBuildingName, Utils.Serialize(_obj));

                return Json(result.ToString());
            }
        }

        //房号模糊搜索
        [HttpPost]
        public ActionResult SearchHouse(string q, int cityId, int bId)
        {
            using (FxtAPIClient client = new FxtAPIClient())
            {
                JObject _obj = new JObject();
                _obj.Add("cId", cityId);
                _obj.Add("bId", bId);
                _obj.Add("hName", q);
                result = client.Entrance(Utils.CommonKey, Utils.GetWcfCode(Utils.CommonKey),
                 "D", _GetHouseByBuildingIdCityIDAndLikeHouseName, Utils.Serialize(_obj));

                return Json(result.ToString());
            }
        }
        #endregion
    }
}
