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
        public static string GetSearchProjectListByKey(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            if (search.PageIndex == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
                search.Page = true;
            }
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            string key = funinfo.Value<string>("key");
            return DatProjectBL.GetSearchProjectListByKey(search, company.companyid, search.CityId, key);
        }
        /// <summary>
        /// 获取楼盘详细信息
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetProjectDetailsByProjectid(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            return DatProjectBL.GetProjectInfoDetailsByProjectid(search, company.companyid, search.CityId, projectid);
        }
        /// <summary>
        /// 获取自动估价楼盘详细信息
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetProjectSimpleDetails(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            return DatProjectBL.GetProjectDetailsByProjectid(search, search.FxtCompanyId, search.CityId, projectid);
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetProjectDropDownList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            int items = 15;
            List<Dictionary<string, object>> list = DatProjectBL.GetProjectDropDownList(search, key, items);
            return list.ToJson();
        }
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="funinfo">功能参数</param>
        /// <param name="company">公司</param>
        /// <returns></returns>
        public static string GetProjectList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            string key = funinfo.Value<string>("key");
            search.OrderBy = "ProjectId";
            int buildingTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));//主建筑物类型
            int purposecode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));//用途
            if (search.PageRecords == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
            }
            List<DATProject> plist = DatProjectBL.GetDATProjectList(search, key, search.AreaId, buildingTypeCode, purposecode);
            return plist.ToJson();
        }

        /// <summary>
        /// 获取楼盘详细数据
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectInfoById(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DATProject dat = DatProjectBL.GetProjectInfoById(search.CityId, projectid, search.FxtCompanyId);
            return dat.ToJson();
        }
        /// <summary>
        /// 获取楼盘图片
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectPhotoById(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            List<LNKPPhoto> photo = DatProjectBL.GetProjectPhotoById(search.CityId, projectid, search.FxtCompanyId);
            return photo.ToJson();
        }
        /// <summary>
        /// 价格走势
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetDATAvgPriceMonthList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            search.DateBegin = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("begindate"), DateTime.Now.AddMonths(-12));
            search.DateEnd = StringHelper.TryGetDateTimeFormat(funinfo.Value<string>("enddate"), DateTime.Now);
            search.CityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DataSet ds = DATAvgPriceMonthBL.GetDATAvgPriceMonthList(search, projectid);
            return returnFlashJson(ds, 0, 0);
        }
        /// <summary>
        /// 楼盘案例
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetProjectCase(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            search.DateBegin = funinfo.Value<string>("begindate");
            search.DateEnd = funinfo.Value<string>("enddate");
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DataTable dt = DatProjectBL.GetProjectCase(search, projectid, search.FxtCompanyId, search.CityId);
            return dt.ToJson();
        }


        /// <summary>
        /// 获取楼栋列表 
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetBuildingListByPid(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
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
        public static string GetBuildingBaseInfoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int avgprice = StringHelper.TryGetInt(funinfo.Value<string>("avgprice"));
            if (string.IsNullOrEmpty(search.OrderBy))
                search.OrderBy = "BuildingId";
            DataSet ds = DatBuildingBL.GetBuildingBaseInfoList(search, projectId, avgprice);
            return ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取楼栋下拉列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetAutoBuildingInfoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
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
                projectid = o.projectid
            });
            return buildingList.ToJson();
        }

        /// <summary>
        /// 获取楼栋单元列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetHouseUnitList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            DataSet ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "unitno", search.FxtCompanyId);
            return ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取楼栋楼层列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetHouseFloorList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            DataSet ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "floorno", search.FxtCompanyId);
            return ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取楼层列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetHouseNoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");
            key = HttpUtility.UrlDecode(key);
            DataSet ds = DatHouseBL.GetAutoFloorNoList(search, buildingId, key);
            return ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 获取房号下拉列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetHouseDropDownList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            DataSet ds = DatHouseBL.GetHouseDropDownList(search, buildingId, floorNo);
            return ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetAutoHouseList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            string key = funinfo.Value<string>("key");
            List<DATHouse> list = DatHouseBL.GetAutoHouseListList(search, buildingId, floorNo, key);
            var houselist = list.Select(o => new
            {
                houseid = o.houseid,
                housename = o.housename,
                buildarea = o.buildarea,
                isevalue = o.isevalue,
                weight = o.weight,
                recordcount = o.recordcount
            });
            return houselist.ToJson();
        }

        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCaseList(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
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
            List<Dat_Case> list = null;
            if (string.IsNullOrEmpty(projectname.Trim()))   //必填
            {
                list = new List<Dat_Case>();
                result = list.ToJson();
            }
            else
            {
                //默认只取15条记录
                if (search.PageRecords == 0)
                {
                    search.PageRecords = 15;
                    search.PageIndex = 1;
                }
                //敦化特殊处理
                if (company.companycode.ToLower() == "dhhy")
                {
                    var listResult = DATCaseBL.GetDATCaseListByCalculateForSpecial(search, projectname, minBuildingArea, maxBuildingArea, minFloorNumber, maxFloorNumber, address);
                    result = listResult.ToJson();
                }
                else
                {
                    list = DATCaseBL.GetDATCaseListByCalculate(search, projectname, minBuildingArea, maxBuildingArea, minFloorNumber, maxFloorNumber, address);
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
        public static string GetProjectCaseCount(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
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

            int cnt = DATCaseBL.GetProjectCaseCount(search.FxtCompanyId, search.CityId, projectid, purposetypecode, buildingareatype, buildingtypecode, startdate, enddate);
            var casecount = new { casecount = cnt };
            return casecount.ToJson();
        }
        /// <summary>
        /// 根据案例ID获取案例列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetCaseListByCaseIds(JObject funinfo, UserCheck company)
        {
            string result = string.Empty;
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid == 365 || company.companyid == 218 ? 25 : company.companyid;
            string projectname = funinfo.Value<string>("projectname");
            string caseIds = funinfo.Value<string>("caseids");
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
            company.companyid = 365;
            string result = "{{\"projectid\":{0},\"message\":\"{1}\"}}";
            string data = funinfo.Value<string>("data");
            //获取楼盘对象JObject
            JObject jobj = JObject.Parse(data);
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
            DATProject project = null;
            if (projectId == 0)
            {
                List<DATProject> _list = DatProjectBL.GetProjectInfoByNames(cityId, areaId, company.companyid, searchNames);
                project = _list == null || _list.Count() < 1 ? null : _list[0];
            }
            else
            {
                project = DatProjectBL.GetProjectInfoByProjectId(cityId, company.companyid, projectId);
                if (project == null)
                {
                    projectId = 0;
                    List<DATProject> _list = DatProjectBL.GetProjectInfoByNames(cityId, areaId, company.companyid, searchNames);
                    project = _list == null || _list.Count() < 1 ? null : _list[0];
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
                if (key.ToLower().Equals("projectid"))
                {
                    continue;
                }
                //如果字段为备注
                if (key.ToLower().Equals("detail") && project != null)
                {
                    string detail = Convert.ToString(_jobj.Value.Value<JValue>().Value);
                    if (detail != null)
                    {
                        nowProject.detail = (project.detail == null ? "" : project.detail) + detail;
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
            nowProject.projectid = 0;
            nowProject.fxt_companyid = company.companyid;
            nowProject.fxtcompanyid = company.companyid;
            nowProject.cityid = cityId;
            nowProject.valid = 1;
            //如果此楼盘是新增数据
            if (projectId == 0)
            {
                projectIsAdd = true;
                //********新增楼盘实体*********//
                IList<DATProject> _projList = DatProjectBL.GetProjectInfoByNames(cityId, 0, company.companyid, new string[] { projectName });
                if (_projList != null && _projList.Count() > 0)
                {
                    nowProject.projectname = projectName + "(" + area.areaname + ")";
                }
                #region (楼盘数据插入)
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
                nowProject.projectid = projectId;
                int _projResult = 0;
                //不是自己的信息&&自己不是25(插入子表)
                if (project.fxtcompanyid != company.companyid && company.companyid != DatProjectBL.FXTCOMPANYID)
                {
                    _projResult = DatProjectBL.AddSub(nowProject, tableInfo.ProjectTable);
                }
                else//是自己的||自己是25
                {
                    nowProject.SetAvailableFields(upProjColumn.ToArray());//设置要更新的字段
                    nowProject.fxt_companyid = project.fxt_companyid;
                    nowProject.fxtcompanyid = project.fxtcompanyid;
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
            foreach (LNKPAppendage obj in lnkpaList)
            {
                obj.projectid = projectId;
                obj.cityid = cityId;
                LNKPAppendage existsObj = existsLnkpaList.Where(_obj => _obj.projectid == projectId && _obj.cityid == cityId && _obj.appendagecode == obj.appendagecode).FirstOrDefault();
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
                        if (key.ToLower().Equals("buildingid"))
                        {
                            continue;
                        }
                        var property = nowBuilding.GetType().GetProperties()
                                 .Where(pInfo => pInfo.Name.ToLower().Equals(key.ToLower())).FirstOrDefault();
                        if (property == null)
                        {
                            continue;
                        }
                        if (!key.ToLower().Contains("companyid") && !key.ToLower().Contains("totalfloor"))
                        {
                            upBuilColumn.Add(key.ToLower());
                        }
                        object _value = ImportProjectAndBuildingAndHouse_valueType(property.PropertyType, _prop.Value.Value<JValue>().Value, true);
                        property.SetValue(nowBuilding, _value, null);
                    }
                    #endregion
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
                            building = DatBuildingBL.GetBuildingByName(cityId, projectId, company.companyid, nowBuilding.buildingname);
                        }
                        else
                        {
                            building = DatBuildingBL.GetBuildingById(cityId, projectId, company.companyid, buildingId);
                            if (building == null)
                            {
                                buildingId = 0;
                                building = DatBuildingBL.GetBuildingByName(cityId, projectId, company.companyid, nowBuilding.buildingname);
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
                        buildingId = DatBuildingBL.Add(nowBuilding, tableInfo.BuildingTable);
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
                            _builResult = DatBuildingBL.AddSub(nowBuilding, tableInfo.BuildingTable);
                        }
                        else//是自己的||自己是25
                        {
                            nowBuilding.fxt_companyid = building.fxt_companyid;
                            nowBuilding.fxtcompanyid = building.fxtcompanyid;
                            nowBuilding.SetAvailableFields(upBuilColumn.ToArray());//设置要更新的字段
                            //在主表
                            if (building.fxt_companyid == 0)
                            {
                                _builResult = DatBuildingBL.Update(nowBuilding, tableInfo.BuildingTable);
                            }
                            else//在子表
                            {
                                _builResult = DatBuildingBL.UpdateSub(nowBuilding, tableInfo.BuildingTable);
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
                        nowHouse.houseid = 0;
                        nowHouse.buildingid = buildingId;
                        nowHouse.cityid = cityId;
                        nowHouse.fxtcompanyid = company.companyid;
                        nowHouse.valid = 1;
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
                                house = DatHouseBL.GetHouseByName(cityId, buildingId, company.companyid, nowHouse.housename);
                            }
                            else
                            {
                                house = DatHouseBL.GetHouseById(cityId, buildingId, company.companyid, houseId);
                                if (house == null)
                                {
                                    houseId = 0;
                                    house = DatHouseBL.GetHouseByName(cityId, buildingId, company.companyid, nowHouse.housename);
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
                                _houseResult = DatHouseBL.AddSub(nowHouse, tableInfo.HouseTable);
                            }
                            else//是自己的||自己是25
                            {
                                nowHouse.fxtcompanyid = house.fxtcompanyid;
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
                }
            }
            #endregion

            return string.Format(result, projectId, "");
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
                    value = Convert.ToDecimal(value);
                    break;
                case "Int32":
                    value = Convert.ToInt32(value);
                    break;
                case "Int64":
                    value = Convert.ToInt64(value);
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
        public static string AddProjectPhoto(System.IO.Stream stream, JObject funinfo, UserCheck company)
        {


            string result = "{{\"path\":\"{0}\",\"message\":\"{1}\"}}";
            int projectId = 0;//funinfo.Value<int>("projectid");
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
            if (!Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(dirPath)))
            {
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(dirPath));
            }
            string _fileName = dirPath + "/" + WebCommon.GetRndString(20) + "_" + fileName;
            using (FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath(_fileName), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //偏移指针
                fs.Seek(0, SeekOrigin.Begin);
                long ByteLength = WebOperationContext.Current.IncomingRequest.ContentLength;
                byte[] fileContent = new byte[ByteLength];
                stream.Read(fileContent, 0, fileContent.Length);
                fs.Write(fileContent, 0, fileContent.Length);
                fs.Flush();
            }
            LNKPPhoto addObj = new LNKPPhoto
            {
                cityid = cityId,
                fxtcompanyid = company.companyid,
                path = ".." + _fileName,
                photodate = photoDate,
                photoname = photoName,
                phototypecode = photoTypeCode,
                projectid = projectId,
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
            int systypecode = StringHelper.TryGetInt(funinfo.Value<string>("systypecode"));
            int houseId = StringHelper.TryGetInt(funinfo.Value<string>("houseid"));// 0;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));// 0;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));// 0;
            int cityId = StringHelper.TryGetInt(funinfo.Value<string>("cityid"));//  1;
            int fxtCompanyId = StringHelper.TryGetInt(funinfo.Value<string>("fxtcompanyid"));//  25;//要通过哪个公司的companyId计算，传参数
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
                    LogHelper.Info(avgstr);
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
                    price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                    price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                    price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                    project = DatProjectBL.GetProjectInfoByProjectId(cityId, fxtCompanyId, projectId);
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
                    LogHelper.Info(avgstr);
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
                        price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                        price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                        price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                        price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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
                        price = avgPrice.avgprice;
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
                        price = (avgPrice.avgprice * (coefficient == 0 ? 1 : coefficient));
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

    }
}
