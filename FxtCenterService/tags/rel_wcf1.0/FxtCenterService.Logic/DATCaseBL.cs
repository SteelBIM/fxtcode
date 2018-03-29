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
        public static List<Dat_Case> GetDATCaseListByCalculate(SearchBase search, string projectname)
        {
            return DATCaseDA.GetDATCaseListByCalculate(search, projectname);
        }

        #region 特殊客户

        public static List<Dat_Case_Dhhy> GetDATCaseListByCalculateForSpecial(SearchBase search, string projectname)
        {
            return DATCaseDA.GetDATCaseListByCalculateForSpecial(search, projectname);
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
        public static List<CAS.Entity.DBEntity.DATCase> GetAreaCase(int fxtcompanyid, int cityid, int areaid, int purposecode, DateTime startdate, DateTime enddate)
        {
            return DATCaseDA.GetAreaCase(fxtcompanyid, cityid, areaid, purposecode,startdate, enddate);
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
        /// <returns></returns>
        public static List<Dat_AroundCase> GetProjectAroundCase(int fxtcompanyid, int cityid, int projectid, int buildingareatype, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate)
        {
            return DATCaseDA.GetProjectAroundCase(fxtcompanyid, cityid, projectid, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate);
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
        /// <returns>dataset(column:avgprice,projectid,projectname)</returns>
        public static DataSet GetSameProjectCasePrice(int fxtcompanyid, int cityid, int projectid, int buildingareatype, int buildingtypecode, DateTime buildingenddate, int areaid, double surveyx, double surveyy, DateTime startdate, DateTime enddate)
        {
            return DATCaseDA.GetSameProjectCasePrice(fxtcompanyid, cityid, projectid, buildingareatype, buildingtypecode, buildingenddate, areaid, surveyx, surveyy, startdate, enddate);
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
        /// <returns>dataset(column:avgprice,ygjavgprice,projectid,sourcename)</returns>
        public static DataSet GetOtherChannelCasePrice(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange)
        {
            return DATCaseDA.GetOtherChannelCasePrice(fxtcompanyid, cityid, projectid, purposecode, buildingareatype, buildingtypecode, startdate, enddate, daterange);
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
        public static int GetProjectCaseCount(int fxtcompanyid, int cityid, int projectid, int purposecode, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate)
        {
            return DATCaseDA.GetProjectCaseCount(fxtcompanyid, cityid, projectid, purposecode, buildingareatype, buildingtypecode, startdate, enddate);
        }
    }

}
