using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

namespace FxtCenterService.Logic
{
    public class DATProjectAvgPriceBL
    {
        /// <summary>
        /// 建筑类型及面积段分类均价表
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid"></param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="purposetype">楼盘类型（普通住宅：1002001）</param>
        /// <param name="startdate">案例日期</param>
        /// <param name="enddate">案例日期</param>
        /// <param name="onlyhasprice">是否只获取均价大于0的数据</param>
        /// <param name="daterange">计算范围</param>
        /// <returns></returns>
        public static List<DATProjectAvgPrice> GetProjectAvgPriceList(int fxtcompanyid, int cityid, int projectid, int[] purposetype, DateTime startdate, DateTime enddate, bool onlyhasprice, int daterange)
        {
            string purposetypestr = (purposetype == null || purposetype.Length == 0) ? "" : string.Join(",", purposetype.Select(i => i.ToString()).ToArray());
            return DATProjectAvgPriceDA.GetProjectAvgPriceList(fxtcompanyid, cityid, projectid, purposetypestr, startdate, enddate, onlyhasprice, daterange);
        }

        /// <summary>
        /// 均价走势图 caoq 2014-1-20
        /// </summary>
        /// <param name="fxtcompanyid"></param>
        /// <param name="cityid">城市ID</param>
        /// <param name="areaid">行政区ID</param>
        /// <param name="purposetype">楼盘类型（普通住宅：1002001）</param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingareatype">面积段CODE</param>
        /// <param name="buildingtypecode">建筑类型Code</param>
        /// <param name="startdate">起始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="daterange">计算范围</param>
        /// <returns>avgtype=1 楼盘,avgtype=2 行政区,avgtype=3 城市</returns>
        public static DataSet GetAvgPriceTrend(int fxtcompanyid, int cityid, int areaid, int projectid, int purposetype, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange)
        {
            return DATProjectAvgPriceDA.GetAvgPriceTrend(fxtcompanyid, cityid, areaid, projectid, purposetype, buildingareatype, buildingtypecode, startdate, enddate, daterange);
        }

        /// <summary>
        /// 周边楼盘价格、环比涨跌幅
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectid"></param>
        /// <param name="projectname"></param>
        /// <param name="surveyx"></param>
        /// <param name="surveyy"></param>
        /// <param name="purposetype"></param>
        /// <param name="avgdate">均价日期</param>
        /// <param name="daterange">计算范围</param>
        /// <returns>dataset(column:projectid,projectname,avgprice,preavgprice,changepercent,projectx,projecty)</returns>
        public static DataSet GetMapPrice(int cityid, int fxtcompanyid, int projectid, string projectname, double surveyx, double surveyy, int purposetype, DateTime avgdate, int daterange)
        {
            return DATProjectAvgPriceDA.GetMapPrice(cityid, fxtcompanyid, projectid, projectname, surveyx, surveyy, purposetype, avgdate, daterange);
        }
    }
}
