using CAS.Entity.FxtProject;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.NHibernate.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;


namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public static class DataCenterProjectApi
    {
        public static void test()
        {
            //string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //var para = new
            //{
            //    sinfo = JsonHelp.ToJSONjss(new
            //    {
            //        functionname = "splist",
            //        appid = Common.appId,
            //        apppwd = Common.appPwd,
            //        signname = "4106DEF5-A760-4CD7-A6B2-8250420FCB18",
            //        time = nowTime, 
            //        code = Common.GetCode("4106DEF5-A760-4CD7-A6B2-8250420FCB18",nowTime,"splist")
            //    }),
            //    info = JsonHelp.ToJSONjss(new
            //    {
            //        uinfo = new { username = "admin@fxt", token = "" },
            //        appinfo = new
            //        {
            //            splatype = "win",
            //            platVer="2007",
            //            stype = "",//验证
            //            version = "4.26",
            //            vcode = "1",
            //            systypecode = Common.systypeCode.ToString(),//验证
            //            channel = "360"
            //        },
            //        funinfo = new { pageindex = 1, cityid = 6, key = "园", pagerecords=15 }
            //    })
            //};

            //HttpClient client = new HttpClient();
            //HttpResponseMessage hrm = client.PostAsJsonAsync(Common.apiUrl, para).Result;
            //string str = hrm.Content.ReadAsStringAsync().Result;
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <param name="cityId">当前城市</param>
        /// <param name="username">当前调用api的username</param>
        /// <param name="signname">当前调用api的signname</param>
        /// <param name="appList">当前用户拥有的api集合</param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectByLikeName(string key, int cityId, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { cityid = cityId, key = key };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.projectdropdownlist, para, appList);
            IList<DATProject> list = new List<DATProject>();
            if (!string.IsNullOrEmpty(result.data))
            {
                JArray _jarray = JArray.Parse(result.data);
                foreach (JToken jt in _jarray)
                {
                    JObject _jobject = (JObject)jt;
                    DATProject proj = new DATProject();
                    proj = _jobject.ToJSONjss().ParseJSONjss<DATProject>();
                    proj.BuildingNum = Convert.ToInt32(_jobject.Value<JValue>("buildingtotal").Value);
                    proj.TotalNum = Convert.ToInt32(_jobject.Value<JValue>("housetotal").Value);
                    list.Add(proj);
                }
            }
            return list;
        }
        /// <summary>
        /// 导入楼盘+楼栋+房号数据到运维中心
        /// </summary>
        /// <param name="project"></param>
        /// <param name="lnkComList"></param>
        /// <param name="lnkpaList"></param>
        /// <param name="buildingList"></param>
        /// <param name="houseList"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int ImportProjectData(Project project, List<PCompany> lnkComList, List<PAppendage> lnkpaList, List<Building> buildingList, ref List<Building> resultBuilding, List<HouseDetails> houseList, string username, string signname, List<UserCenter_Apps> appList, out string message)
        {
            message = "";
            int projectId = 0;
            JObject jobj = new JObject();
            #region (楼盘信息)
            jobj.Add(new JProperty("x", project.X));
            jobj.Add(new JProperty("y", project.Y));
            jobj.Add(new JProperty("projectname", project.ProjectName.TrimBlank().EncodeField()));
            jobj.Add(new JProperty("cityid", project.CityID));
            jobj.Add(new JProperty("areaid", project.AreaID));
            jobj.Add(new JProperty("address", project.Address.EncodeField()));
            jobj.Add(new JProperty("enddate", project.EndDate));
            jobj.Add(new JProperty("east", project.East.EncodeField()));
            jobj.Add(new JProperty("west", project.West.EncodeField()));
            jobj.Add(new JProperty("south", project.South.EncodeField()));
            jobj.Add(new JProperty("north", project.North.EncodeField()));
            //jobj.Add(new JProperty("buildingarea", project.BuildingArea));
            //jobj.Add(new JProperty("landarea", project.LandArea));
            //jobj.Add(new JProperty("cubagerate", project.CubageRate));
            //jobj.Add(new JProperty("greenrate", project.GreenRate));
            //jobj.Add(new JProperty("managerprice", project.ManagerPrice.EncodeField()));
            jobj.Add(new JProperty("parkingnumber", project.ParkingNumber));
            //jobj.Add(new JProperty("saledate", project.SaleDate));
            //jobj.Add(new JProperty("buildingdate", project.BuildingDate));
            jobj.Add(new JProperty("detail", project.Detail.EncodeField()));
            jobj.Add(new JProperty("fxtprojectid", project.FxtProjectId));
            jobj.Add(new JProperty("fxtcompanyid", project.FxtCompanyId));
            jobj.Add(new JProperty("valid", 1));
            jobj.Add(new JProperty("othername", project.OtherName));
            jobj.Add(new JProperty("subareaid", project.SubAreaId));
            jobj.Add(new JProperty("purposecode", project.PurposeCode));
            jobj.Add(new JProperty("rightcode", project.RightCode));
            jobj.Add(new JProperty("buildingnum", project.BuildingNum));
            jobj.Add(new JProperty("totalnum", project.TotalNum));
            jobj.Add(new JProperty("managerquality", project.ManagerQuality));
            //jobj.Add(new JProperty("parkingstatus", project.ParkingStatus));
            jobj.Add(new JProperty("creator", project.Creator));
            jobj.Add(new JProperty("updatedatetime", project.UpdateDateTime));
            jobj.Add(new JProperty("saveuser", project.SaveUser));
            jobj.Add(new JProperty("savedatetime", project.SaveDateTime));

            #endregion
            //楼盘关联公司
            jobj.Add(new JProperty("companylist", JArray.Parse(Common.ConvertToJson(lnkComList))));
            //楼盘配套
            jobj.Add(new JProperty("appendage", JArray.Parse(lnkpaList.ToJSONjss())));

            //楼栋+房号信息
            JArray buildingArry = new JArray();
            foreach (Building building in buildingList)
            {
                JObject buildingObj = new JObject();
                #region(楼栋信息)
                buildingObj.Add(new JProperty("buildingid", building.BuildingId));
                buildingObj.Add(new JProperty("buildingname", building.BuildingName.EncodeField()));
                //buildingObj.Add(new JProperty("doorplate", building.Doorplate.EncodeField()));
                buildingObj.Add(new JProperty("othername", building.OtherName.EncodeField()));
                buildingObj.Add(new JProperty("structurecode", building.StructureCode));
                buildingObj.Add(new JProperty("locationcode", building.LocationCode));
                //buildingObj.Add(new JProperty("averageprice", building.AveragePrice));
                buildingObj.Add(new JProperty("builddate", building.BuildDate));
                buildingObj.Add(new JProperty("iselevator", building.IsElevator));
                buildingObj.Add(new JProperty("elevatorrate", building.ElevatorRate.EncodeField()));
                //buildingObj.Add(new JProperty("pricedetail", building.PriceDetail.EncodeField()));
                buildingObj.Add(new JProperty("remark", building.Remark.EncodeField()));
                buildingObj.Add(new JProperty("fxtbuildingid", building.FxtBuildingId));
                //buildingObj.Add(new JProperty("sightcode", building.SightCode));
                buildingObj.Add(new JProperty("totalfloor", building.TotalFloor));
                buildingObj.Add(new JProperty("fxtcompanyid", building.FxtCompanyId));
                buildingObj.Add(new JProperty("cityid", building.CityID));
                buildingObj.Add(new JProperty("purposecode", building.PurposeCode));
                buildingObj.Add(new JProperty("maintenancecode", building.MaintenanceCode));
                buildingObj.Add(new JProperty("creator", building.Creator));
                buildingObj.Add(new JProperty("saveuser", building.SaveUser));
                buildingObj.Add(new JProperty("x", building.X));
                buildingObj.Add(new JProperty("y", building.Y));
                #endregion
                JArray houseArry = new JArray();
                #region (房号信息)
                List<HouseDetails> _houseList = houseList.Where(obj => obj.BuildingId == building.BuildingId).ToList();
                foreach (HouseDetails house in _houseList)
                {
                    JObject houseObj = new JObject();
                    houseObj.Add(new JProperty("fxtcompanyid", house.FxtCompanyId));
                    houseObj.Add(new JProperty("cityid", house.CityID));
                    houseObj.Add(new JProperty("unitno", (house.UnitNo+house.RoomNo) .EncodeField()));
                    houseObj.Add(new JProperty("floorno", house.FloorNo));
                    //houseObj.Add(new JProperty("endfloorno", house.EndFloorNo));
                    houseObj.Add(new JProperty("housename", house.HouseName.EncodeField()));
                    houseObj.Add(new JProperty("frontcode", house.FrontCode));
                    houseObj.Add(new JProperty("buildarea", house.BuildArea));
                    houseObj.Add(new JProperty("housetypecode", house.HouseTypeCode));
                    houseObj.Add(new JProperty("remark", house.Remark.EncodeField()));
                    houseObj.Add(new JProperty("fxthouseid", house.FxtHouseId));
                    houseObj.Add(new JProperty("sightcode", house.SightCode));
                    houseObj.Add(new JProperty("purposecode", house.PurposeCode));
                    houseObj.Add(new JProperty("vdcode", house.VDCode));
                    houseObj.Add(new JProperty("structurecode", house.StructureCode));
                    houseObj.Add(new JProperty("noisecode", house.NoiseCode));
                    houseObj.Add(new JProperty("creator", house.Creator));
                    houseObj.Add(new JProperty("saveuser", house.SaveUser));
                    houseArry.Add(houseObj);
                }
                buildingObj.Add(new JProperty("houselist", houseArry));
                #endregion
                buildingArry.Add(buildingObj);
            }
            jobj.Add(new JProperty("buildinglist", buildingArry));
            string dataJson = jobj.ToJSONjss();

            var para = new { data = dataJson };

            DataCenterResult result = Common.PostDataCenter(username, signname, Common.importprojectdata, para, appList);
            if (!string.IsNullOrEmpty(result.data))
            {
                JObject _jobj = JObject.Parse(result.data);
                projectId = _jobj.Value<int>("projectid");
                message = _jobj.Value<string>("message");
                var buildingids = _jobj["buildingids"].ToString();
                if (!buildingids.IsNullOrEmpty())
                {
                    var buildingidList = JsonHelp.ParseJSONList<Building>(buildingids);
                    resultBuilding = buildingidList;
                }
            }
            return projectId;
        }
        /// <summary>
        /// 给楼盘上传照片
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="photoTypeCode"></param>
        /// <param name="fileName"></param>
        /// <param name="photoName"></param>
        /// <param name="data"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int AddProjectPhoto(int projectId, long buildingId, int cityId, int photoTypeCode, string fileName, string photoName, byte[] data, string username, string signname, List<UserCenter_Apps> appList, out string message)
        {
            var para = new { projectid = projectId, buildingid = buildingId, cityid = cityId, phototypecode = photoTypeCode, filename = fileName, photoname = photoName };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.addprojectphoto, para, data, appList);
            string path = "";
            message = "";
            if (!string.IsNullOrEmpty(result.data))
            {
                JObject _jobj = JObject.Parse(result.data);
                path = _jobj.Value<string>("path").DecodeField();
                message = _jobj.Value<string>("message");
            }
            if (string.IsNullOrEmpty(path))
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <param name="cityId">当前城市</param>
        /// <param name="areaId">行政区</param>
        /// <param name="subAreaId">片区</param>
        /// <param name="username">当前调用api的username</param>
        /// <param name="signname">当前调用api的signname</param>
        /// <param name="appList">当前用户拥有的api集合</param>
        /// <returns></returns>
        public static IList<CAS.Entity.DBEntity.DATProject> GetProject(string key, int cityId, int areaId, int subAreaId, int pageindex, int pagerecords, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { cityid = cityId, key = key, areaid = areaId, subareaid = subAreaId, pageindex = pageindex, pagerecords = pagerecords };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.plist, para, appList);
            IList<CAS.Entity.DBEntity.DATProject> list = new List<CAS.Entity.DBEntity.DATProject>();
            if (!string.IsNullOrEmpty(result.data))
            {
                JArray _jarray = JArray.Parse(result.data);
                foreach (JToken jt in _jarray)
                {
                    JObject _jobject = (JObject)jt;
                    CAS.Entity.DBEntity.DATProject proj = new CAS.Entity.DBEntity.DATProject();
                    proj = _jobject.ToJSONjss().ParseJSONjss<CAS.Entity.DBEntity.DATProject>();
                    list.Add(proj);
                }
            }
            return list;
        }

        public static List<CAS.Entity.DBEntity.DATProject> GetProjectBuildingHouseAll(int cityId, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { cityid = cityId, projectids = "" };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.getprojectbuildinghouselist, para, appList);
            List<CAS.Entity.DBEntity.DATProject> list = new List<CAS.Entity.DBEntity.DATProject>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = JsonHelp.ParseJSONList<CAS.Entity.DBEntity.DATProject>(result.data);
            }
            return list;
        }

        public static List<CAS.Entity.DBEntity.DATProject> GetProjectBuildingHouse(int cityId, string proujectIds, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { cityid = cityId, projectids = proujectIds };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.getprojectbuildinghouselist, para, appList);
            List<CAS.Entity.DBEntity.DATProject> list = new List<CAS.Entity.DBEntity.DATProject>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = JsonHelp.ParseJSONList<CAS.Entity.DBEntity.DATProject>(result.data);
            }
            return list;
        }

        public static List<CAS.Entity.DBEntity.DATProject> GetProjectInfo(int cityId, int projectid, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { cityid = cityId, projectid = projectid };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.gpdinfo, para, appList);
            List<CAS.Entity.DBEntity.DATProject> entity = new List<CAS.Entity.DBEntity.DATProject>();
            if (!string.IsNullOrEmpty(result.data))
            {
                entity = JsonHelp.ParseJSONList<CAS.Entity.DBEntity.DATProject>(result.data);
            }
            return entity;
        }
        /// <summary>
        /// 根据楼盘ID获取楼栋、房号数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectid"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static DatProjectTotal GetProjectBuildingHouseTotal(int cityId, int projectid, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { cityid = cityId, projectid = projectid };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.buildinghousetotal, para, appList);
            DatProjectTotal entity = null;
            if (!string.IsNullOrEmpty(result.data))
            {
                entity = JsonHelp.ParseJSONjss<DatProjectTotal>(result.data);
            }
            return entity;
        }
    }
}
