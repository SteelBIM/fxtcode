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
            search.FxtCompanyId = company.companyid;
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
            search.FxtCompanyId = company.companyid;
            int projectid =StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
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
            search.FxtCompanyId = company.companyid;
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
            search.FxtCompanyId = company.companyid;
            string key = funinfo.Value<string>("key");
            int items = 15;
            List<Dictionary<string, object>> list= DatProjectBL.GetProjectDropDownList(search, key, items);
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
            search.FxtCompanyId = company.companyid;
            string key = funinfo.Value<string>("key");
            search.OrderBy = "ProjectId";
            int buildingTypeCode = StringHelper.TryGetInt(funinfo.Value<string>("buildingtypecode"));//主建筑物类型
            int purposecode = StringHelper.TryGetInt(funinfo.Value<string>("purposecode"));//用途
            if (search.PageRecords == 0)
            {
                search.PageIndex = 1;
                search.PageRecords = 15;
            }
            List<DATProject> plist = DatProjectBL.GetDATProjectList(search, key,search.AreaId, buildingTypeCode, purposecode);
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
            search.FxtCompanyId = company.companyid;
            int projectid = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            DATProject dat = DatProjectBL.GetProjectInfoById(search.CityId, projectid,search.FxtCompanyId);
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
            search.FxtCompanyId = company.companyid;
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
            search.FxtCompanyId = company.companyid;
            search.DateBegin = funinfo.Value<string>("begindate");
            search.DateEnd = funinfo.Value<string>("enddate");
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
            search.FxtCompanyId = company.companyid;
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
            search.FxtCompanyId = company.companyid;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string key= funinfo.Value<string>("key");
            List<DATBuilding> list=DatBuildingBL.GetDATBuildingList(search, projectId, key);
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
            search.FxtCompanyId = company.companyid;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            int avgprice =StringHelper.TryGetInt(funinfo.Value<string>("avgprice"));
            search.OrderBy = "BuildingId";
            DataSet ds= DatBuildingBL.GetBuildingBaseInfoList(search, projectId, avgprice);
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
            search.FxtCompanyId = company.companyid;
            int projectId = StringHelper.TryGetInt(funinfo.Value<string>("projectid"));
            string key = funinfo.Value<string>("key");
            List<DATBuilding> list = DatBuildingBL.GetAutoBuildingInfoList(search, projectId, key);
            var buildingList = list.Select(o => new
            {
                buildingid = o.buildingid,
                buildingname = o.buildingname,
                isevalue = o.isevalue,
                weight = o.weight,
                recordcount = o.recordcount,
                totalfloor = o.totalfloor
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
            search.FxtCompanyId = company.companyid;
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
            search.FxtCompanyId = company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            DataSet ds = DatHouseBL.GetHouseFileListWithSub(search.CityId, buildingId, "floorno", search.FxtCompanyId);
            return ds.Tables[0].ToJson();
        }
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="funinfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public static string GetHouseNoList(JObject funinfo, UserCheck company)
        {
            SearchBase search = DataCenterCommon.InitSearBase(funinfo);
            search.FxtCompanyId = company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            string key = funinfo.Value<string>("key");
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
            search.FxtCompanyId = company.companyid;
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
            search.FxtCompanyId = company.companyid;
            int buildingId = StringHelper.TryGetInt(funinfo.Value<string>("buildingid"));
            int floorNo = StringHelper.TryGetInt(funinfo.Value<string>("floorno"));
            string key = funinfo.Value<string>("key");
            List<DATHouse> list= DatHouseBL.GetAutoHouseListList(search, buildingId, floorNo,key);
            var houselist = list.Select(o => new {
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
            search.FxtCompanyId = company.companyid;
            string projectname = funinfo.Value<string>("projectname");
            List<Dat_Case> list = null;
            if (string.IsNullOrEmpty(projectname.Trim()))
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
                    var listResult = DATCaseBL.GetDATCaseListByCalculateForSpecial(search, projectname);
                    result = listResult.ToJson();
                }
                else
                {
                    list = DATCaseBL.GetDATCaseListByCalculate(search, projectname);
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
            search.FxtCompanyId = company.companyid;
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
            var casecount=new {casecount=cnt};
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
            search.FxtCompanyId = company.companyid;
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
                    result =list.ToJson();

                }
            }
            return result;
        }

        private static void GetDATCaseList(SearchBase search,string projectname, out string result)
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
        private static void GetDATCaseListForSpecial(SearchBase search,string projectname, out string result)
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
            result =list.ToJson();
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
