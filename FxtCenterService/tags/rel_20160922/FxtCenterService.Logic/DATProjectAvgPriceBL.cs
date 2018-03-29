using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity;
using CAS.Entity.FxtDataCenter;
using CAS.Entity.FxtLog;

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
        public static List<DATProjectAvgPrice> GetProjectAvgPriceList(int fxtcompanyid, int cityid, int projectid, int[] purposetype, DateTime startdate, DateTime enddate, bool onlyhasprice, int daterange, int typecode)
        {
            string purposetypestr = (purposetype == null || purposetype.Length == 0) ? "" : string.Join(",", purposetype.Select(i => i.ToString()).ToArray());
            return DATProjectAvgPriceDA.GetProjectAvgPriceList(fxtcompanyid, cityid, projectid, purposetypestr, startdate, enddate, onlyhasprice, daterange, typecode);
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
        public static DataSet GetAvgPriceTrend(int fxtcompanyid, int cityid, int areaid, int projectid, int purposetype, int buildingareatype, int buildingtypecode, DateTime startdate, DateTime enddate, int daterange, int typecode)
        {
            return DATProjectAvgPriceDA.GetAvgPriceTrend(fxtcompanyid, cityid, areaid, projectid, purposetype, buildingareatype, buildingtypecode, startdate, enddate, daterange, typecode);
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
        public static DataSet GetMapPrice(int cityid, int fxtcompanyid, int projectid, string projectname, double surveyx, double surveyy, int purposetype, DateTime avgdate, int daterange, int typecode)
        {
            return DATProjectAvgPriceDA.GetMapPrice(cityid, fxtcompanyid, projectid, projectname, surveyx, surveyy, purposetype, avgdate, daterange, typecode);
        }
        /// <summary>
        /// MCAS自动估价:楼栋，房号
        /// </summary>
        /// <param name="CityId">城市ID</param>
        /// <param name="ProjectId">楼盘ID</param>
        /// <param name="BuildingId">楼栋ID</param>
        /// <param name="HouseId">房号ID</param>
        /// <param name="FXTCompanyId">公司ID</param>
        /// <param name="totalFloor">总楼层</param>
        /// <param name="FloorNumber">所在楼层</param>
        /// <param name="Frontcode">朝向</param>
        /// <param name="projectprice">楼盘建筑类型均价</param>
        /// <param name="buildarea">面积</param>
        /// <returns></returns>
        public static DataSet GetMCASBHAutoPrice(int cityId, int projectId, int buildingId, int houseId, int FXTCompanyId, int totalFloor, int floorNumber
            , int frontCode, double projectprice, double buildarea)
        {
            return DATProjectAvgPriceDA.GetMCASBHAutoPrice(cityId, projectId, buildingId, houseId, FXTCompanyId, totalFloor, floorNumber
            , frontCode, projectprice, buildarea);
        }
        /// <summary>
        ///  MCAS自动估价:楼盘
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId">可为0</param>
        /// <param name="HouseId">可为0</param>
        /// <param name="FXTCompanyId">评估机构ID</param>
        /// <param name="CompanyId">客户单位Id</param>
        /// <param name="UserId">账号</param>
        /// <param name="BuildingArea">建筑面积</param>
        /// <param name="StartDate">可为空</param>
        /// <param name="EndDate">可为空</param>
        /// <param name="sysTypeCode">系统code</param>
        /// <returns>Tables[0]:询价结果</returns>
        public static DataSet GetMCASProjectAutoPrice(int CityId, int ProjectId, int FxtCompanyId, string UseMonth)
        {
            return DATProjectAvgPriceDA.GetMCASProjectAutoPrice(CityId, ProjectId, FxtCompanyId, UseMonth);
        }

        /// <summary>
        /// MCAS自动估价:房号
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ProjectId"></param>
        /// <param name="BuildingId"></param>
        /// <param name="HouseId"></param>
        /// <param name="floorcount">总楼层</param>
        /// <param name="floorno">所在层</param>
        /// <param name="frontcode">朝向</param>
        /// <param name="buildingarea">面积</param>
        /// <param name="unitprice">楼盘基准房价</param>
        /// <param name="plprice">底层基准房价</param>
        /// <param name="pmprice">多层基准房价</param>
        /// <param name="psprice">小高层基准房价</param>
        /// <param name="phprice">高层基准房价</param>
        /// <param name="estimable">估价结果</param>
        /// <returns></returns>
        public static string GetMCASHouseAutoPrice(SearchBase search, int ProjectId, int BuildingId, int HouseId,
    int floorcount, int floorno, int frontcode, decimal buildingarea, int unitprice, int plprice, int pmprice, int psprice, int phprice, out int estimable,out decimal price)
        {
            // 1、根据总楼层识别建筑类型，识别方式：
            //低层是指总楼层小于等于3层的所有楼栋、多层是指总楼层为4-8层的所有楼栋、小高层是指总楼层为9-12层的所有楼栋、高层是指13层以上的所有楼栋
            // 2、选取计算房号的楼盘价格，例如：如果识别出来的总楼层是底层。则楼盘价格为底层基准方法。如果没有底层基准方法，则用楼盘基准均价。其他建筑类型以此类推

            AutoPrice autoPrice = new AutoPrice();//自动估价结果

            //if (floorcount < 1)
            //{
            //    estimable = -3;//无总楼层
            //    price = 0;
            //    return new { unitprice = 0, totalprice = 0, estimable = 0 }.ToJson();
            //}

            //如果楼盘基准房价>0，总楼层=0或者所在层=0，价格=楼盘基准房价
            if (unitprice > 0 && (floorcount == 0 || floorno == 0))
            {
                estimable = 1;
                price = unitprice;
                return new { unitprice = unitprice, totalprice = unitprice * buildingarea, estimable = 1, ishouseprice = 0 }.ToJson();
            }

            //weighttype int=0,--房号系数,1-VQ系数，0-CAS系数
            int weighttype = 0;

            DatEvalueSet de = DatEvalueSetDA.GetEvalueSetBy(25, search.CityId);//因为只设置了25的数据
            if (de != null)
            {
                weighttype = 1;
            }

            //根据建筑类型找对应的基准房价
            int projectprice = 0;
            if (floorcount <= 3)//低层
            {
                projectprice = plprice;
            }
            else if (floorcount <= 8)//多层
            {
                projectprice = pmprice;
            }
            else if (floorcount <= 12)//小高层
            {
                projectprice = psprice;
            }
            else if (floorcount > 12)//高层
            {
                projectprice = phprice;
            }
            //如果对应的建筑类型基准房价=0，则用楼盘基准房价
            if (projectprice == 0)
            {
                projectprice = unitprice;
                weighttype = 0;//没有与楼栋建筑类型对应的基准均价使用CAS系数 
            }

            //不可沽
            if (projectprice == 0)
            {
                estimable = -4;//无楼盘均价
                price = 0;
                return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();
            }

            string begindate = string.Empty;
            string enddate = string.Empty;
            int day = StringHelper.TryGetInt(DateTime.Now.ToString("dd"));
            //如果当天时间小于16
            if (day < 16)
            {
                begindate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-16 00:00:00");
                enddate = DateTime.Now.ToString("yyyy-MM-15 23:59:59");
            }
            else
            {
                begindate = DateTime.Now.ToString("yyyy-MM-16 00:00:00");
                enddate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-15 23:59:59");
            }

            DataSet ds = DATProjectAvgPriceDA.GetMCASHouseAutoPrice(search, ProjectId, BuildingId, HouseId,
             floorcount, floorno, frontcode, buildingarea, projectprice, begindate, enddate, weighttype);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //物业全称
                autoPrice.unitprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["UnitPrice"].ToString());
                autoPrice.totalprice = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["TotalPrice"].ToString());
                int flag = StringHelper.TryGetInt(ds.Tables[0].Rows[0]["flag"].ToString());
                //LogHelper.Info("房号估价:可估.FxtCompanyId:"+search.FxtCompanyId);
                //LogHelper.Info("单价:" + autoPrice.unitprice);
                //LogHelper.Info("总价:" + autoPrice.totalprice);
                estimable = flag;//1.VQ，2.VQ房号系数，3.VQ楼层差，4.CAS房号系数，5.CAS楼层差
                price = autoPrice.unitprice;
                return new { unitprice = autoPrice.unitprice, totalprice = autoPrice.totalprice, estimable = 1, ishouseprice = 1 }.ToJson();
            }
            else
            {
                //LogHelper.Info("房号估价:不可估.FxtCompanyId:"+search.FxtCompanyId);
                if (weighttype == 1)
                {
                    estimable = -5;//无楼层差系数（VQ）
                }
                else
                {
                    estimable = -6;//无楼层差系数（CAS）
                }
                price = 0;
                return new { unitprice = 0, totalprice = 0, estimable = 0, ishouseprice = 0 }.ToJson();
            }

            //房号自动估价需求

            //一、前提条件：
            //    1、楼盘可估

            //二、场景说明：
            //    1、能选择到房号，且有房号价格修正系数：
            //       房号单价=楼盘价格*房号系数
            //    2、能选择到房号，且无房号价格修正系数：
            //       房号单价=楼盘价格*楼层差
            //    3、选择不到房号，有所在楼层与总楼层：
            //        房号单价=楼盘价格*楼层差
            //    4、选择不到房号，所在楼层或总楼层，其中一个没有传送：
            //        房号单价=楼盘价格
            //    5、选择不到房号，所在楼层与总楼层都没有传送：
            //        房号单价=楼盘价格

            //优化级，依据场景顺序。

        }

        //获取行政区价格监测 kujj 20150421
        public static DataSet GetProcAreaAvgList(int fxtCompanyId, int cityId, string avgPriceDate)
        {
            return DATProjectAvgPriceDA.GetProcAreaAvgList(fxtCompanyId, cityId, avgPriceDate);
        }
        //获取片区价格监测 kujj 20150422
        public static DataSet GetProcSubAreaAvgList(int fxtCompanyId, int cityId, int areaId, string avgPriceDate)
        {
            return DATProjectAvgPriceDA.GetProcSubAreaAvgList(fxtCompanyId, cityId, areaId, avgPriceDate);
        }
        //获取楼盘价格监测 kujj 20150422
        public static DataSet GetProcProjectAvgList(int fxtCompanyId, int cityId, int areaId, int subAreaId, string avgPriceDate)
        {
            return DATProjectAvgPriceDA.GetProcProjectAvgList(fxtCompanyId, cityId, areaId, subAreaId, avgPriceDate);
        }
        //获取行政区、片区近一年均价 kujj 20150507
        public static DataSet GetAreaYearAvgList(int fxtCompanyId, int cityId, int areaId, int subAreaId)
        {
            return DATProjectAvgPriceDA.GetAreaYearAvgList(fxtCompanyId, cityId, areaId, subAreaId);
        }
        //获取行政区、片区、楼盘近一年走势 kujj 20150507
        public static DataSet GetDiffTypeAvgList(int type, int fxtCompanyId, int cityId, int areaid, int subareaid, int projectid, int buildingareatype, int buildingdatetype, int buildingtypecode, int housetype, int purposetype, int e_housetype, string begin, string end)
        {
            return DATProjectAvgPriceDA.GetDiffTypeAvgList(type, fxtCompanyId, cityId, areaid, subareaid, projectid, buildingareatype, buildingdatetype, buildingtypecode, housetype, purposetype, e_housetype, begin, end);
        }

        /// <summary>
        /// 获取楼盘建筑类型均价 kujj 20150612
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectid"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataSet GetMCASWeightProjectPrice(int cityid, int fxtcompanyid, int projectid, DateTime begindate, DateTime enddate)
        {
            return DATProjectAvgPriceDA.GetMCASWeightProjectPrice(cityid, fxtcompanyid, projectid, begindate, enddate);
        }

        /// <summary>
        /// 询价单
        /// </summary>
        /// <param name="cityid"></param>
        /// <param name="fxtcompanyid"></param>
        /// <param name="projectid"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataSet GetMCASInquiry_ForVQ(SearchBase search, int projectid, int buildingid, int houseid)
        {
            return DATProjectAvgPriceDA.GetMCASInquiry_ForVQ(search, projectid, buildingid, houseid);
        }
    }
}
