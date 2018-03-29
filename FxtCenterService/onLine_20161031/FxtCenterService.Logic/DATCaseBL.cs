using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Common;
using FxtCenterService.DataAccess;
using CAS.Entity.GJBEntity;
using System.Data;

namespace FxtCenterService.Logic
{
    public class DATCaseBL
    {
        public static int Add(DATCase model)
        {
            //设置城市表
            if (!(model.cityid > 0)) return 0;
            CityTable city = CityTableDA.GetCityTable(model.cityid.Value);
            if (null == city) return 0;
            DATCaseDA.SetEntityTable<DATCase>(city.casetable);

            return DATCaseDA.Add(model);
        }
        public static int Update(DATCase model)
        {
            return DATCaseDA.Update(model);
        }
        public static int Delete(int id)
        {
            return DATCaseDA.Delete(id);
        }

        public static DATCase GetDATCaseByPK(int id)
        {
            return DATCaseDA.GetDATCaseByPK(id);
        }
        public static List<Dat_Case> GetDATCaseListByCalculate(SearchBase search, string projectname, int minBuildingArea, int maxBuildingArea, int minFloorNumber, 
            int maxFloorNumber, decimal minUnitPrice, decimal maxUnitPrice, string address, int caseTypeCode, int areaid, int subareaid,
            DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator, string structurecodename)
        {
            return DATCaseDA.GetDATCaseListByCalculate(search, projectname, minBuildingArea, maxBuildingArea, minFloorNumber, 
                maxFloorNumber, minUnitPrice, maxUnitPrice, address, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator,structurecodename);
        }

        #region 特殊客户

        public static List<Dat_Case_Dhhy> GetDATCaseListByCalculateForSpecial(SearchBase search, string projectname, int minBuildingArea, int maxBuildingArea, int minFloorNumber,
            int maxFloorNumber, decimal minUnitPrice, decimal maxUnitPrice, string address, int caseTypeCode, int areaid, int subareaid,
            DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator)
        {
            return DATCaseDA.GetDATCaseListByCalculateForSpecial(search, projectname, minBuildingArea, maxBuildingArea, minFloorNumber,
                maxFloorNumber, minUnitPrice, maxUnitPrice, address, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator);
        }


        public static List<Dat_Case_Dhhy> GetDATCaseListForSpecial(SearchBase search, string key, string projectname, int[] caseIds)
        {
            return DATCaseDA.GetDATCaseListForSpecial(search, key, projectname, caseIds);
        }

        #endregion
        /// <summary>
        /// 根据楼盘Id获取改楼盘价格案例
        /// </summary>
        /// <param name="search"></param>
        /// <param name="key"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public static List<Dat_Case> GetDATCaseList(SearchBase search, string key, string projectname, int[] caseIds)
        {
            return DATCaseDA.GetDATCaseList(search, key, projectname, caseIds);
        }

        /// <summary>
        /// 获取内行政区内指定类型案例  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <param name="purposecode"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<CAS.Entity.DBEntity.DATCase> GetAreaCase(int fxtcompanyid, int cityid, int areaid, int purposecode, DateTime startdate, DateTime enddate, int typecode)
        {
            return DATCaseDA.GetAreaCase(fxtcompanyid, cityid, areaid, purposecode, startdate, enddate, typecode);
        }

        /// <summary>
        /// 获取楼盘周边案例   caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="buildingenddate">竣工时间</param>
        /// <param name="areaid">行政区</param>
        /// <param name="surveyx">查勘X坐标</param>
        /// <param name="surveyy">查勘Y坐标</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="fivemiles">坐标定位偏差(默认5公里偏差（0.5）)</param>
        /// <returns></returns>
        public static List<Dat_AroundCase> GetProjectAroundCase(int fxtcompanyid, int cityid, int projectid, int buildingareatype, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate, int typecode, double fivemiles = 0.5)
        {
            return DATCaseDA.GetProjectAroundCase(fxtcompanyid, cityid, projectid, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate, typecode, fivemiles);
        }

        /// <summary>
        /// 周边同质楼盘价格计算  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="buildingenddate">竣工时间</param>
        /// <param name="areaid">行政区ID</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns>dataset(column:projectid,projectname,avgprice,preavgprice,changepercent,projectx,projecty)</returns>
        public static DataSet GetSameProjectCasePrice(int fxtcompanyid, int cityid, int projectid, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate, int typecode)
        {
            return DATCaseDA.GetSameProjectCasePrice(fxtcompanyid, cityid, projectid, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate, typecode);
        }

        /// <summary>
        /// 不同渠道楼盘均价获取  caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>       
        /// <param name="projectid">楼盘ID</param>
        /// <param name="purposecode">物业类型</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="daterange">计算范围</param>
        /// <returns>dataset(column:avgprice,projectid,sourcename)</returns>
        public static DataSet GetOtherChannelCasePrice(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange, int typecode)
        {
            return DATCaseDA.GetOtherChannelCasePrice(fxtcompanyid, cityid, projectid, purposecode, buildingareatype, buildingtypecode, startdate, enddate, daterange, typecode);
        }

        /// <summary>
        /// 获取楼盘案例总数 caoq 2014-3-28
        /// </summary>
        /// <param name="fxtcompanyid">公司ID</param>
        /// <param name="cityid">城市ID</param>        
        /// <param name="projectid">楼盘ID</param>
        /// <param name="purposecode">物业类型</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <returns></returns>
        public static int GetProjectCaseCount(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int typecode)
        {
            return DATCaseDA.GetProjectCaseCount(fxtcompanyid, cityid, projectid, purposecode, buildingareatype, buildingtypecode, startdate, enddate, typecode);
        }

        /// <summary>
        /// 获取单个楼盘近n个月案例总数 库晶晶20150210
        /// </summary>
        public static int GetCaseCountByProjectId_MCAS(int fxtcompanyid, int cityid, int projectid, int months, int typecode)
        {
            return DATCaseDA.GetCaseCountByProjectId_MCAS(fxtcompanyid, cityid, projectid, months, typecode);
        }
        /// <summary>
        /// 获取多个楼盘近n个月案例总数 tanql20150922
        /// </summary>
        public static DataSet GetCaseCountByProjectIds_MCAS(int fxtcompanyid, int cityid, string projectids, int months, int typecode)
        {
            return DATCaseDA.GetCaseCountByProjectIds_MCAS(fxtcompanyid, cityid, projectids, months, typecode);
        }

        /// <summary>
        /// 获取单个楼盘坐标及照片总数 库晶晶20150210
        /// </summary>
        public static DataSet GetProjectListInfo_MCAS(int fxtcompanyid, int projectid, int cityid, int typecode)
        {
            return DATCaseDA.GetProjectListInfo_MCAS(fxtcompanyid, projectid, cityid, typecode);
        }

        /// <summary>
        /// 获取住宅案例列表 库晶晶20150415
        /// </summary>
        public static DataSet GetCaseListNew(SearchBase search, int buildingtypecode, int purposecode, int casetypecode, DateTime casedatefrom, DateTime casedateto, decimal? buildingareafrom, decimal? buildingareato, string projectname, int isSource)
        {
            return DATCaseDA.GetCaseListNew(search, buildingtypecode, purposecode, casetypecode, casedatefrom, casedateto, buildingareafrom, buildingareato, projectname, isSource);
        }
        /// <summary>
        /// 获取住宅案例最高单价、最低单价、平均单价 库晶晶20150416
        /// </summary>
        public static DataSet GetCasePrice(SearchBase search, int buildingtypecode, int purposecode, int casetypecode, DateTime casedatefrom, DateTime casedateto, decimal? buildingareafrom, decimal? buildingareato, string projectname, int isSource)
        {
            return DATCaseDA.GetCasePriceModify(search, buildingtypecode, purposecode, casetypecode, casedatefrom, casedateto, buildingareafrom, buildingareato, projectname, isSource);
        }
    }

}
