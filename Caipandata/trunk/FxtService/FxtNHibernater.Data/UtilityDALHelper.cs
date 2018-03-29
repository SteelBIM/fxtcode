using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using FxtService.Common;
using System.Reflection;
using NHibernate.Criterion;
using NHibernate;
using FxtNHibernate.DATProjectDomain.Entities;
using log4net;
using System.Collections;

/**
 * 作者: 李晓东
 * 时间: 2013.12.03
 * 摘要: 新建UtilityDALHelper类
 *       2013.12.20 新增 GetCity GetSYSArea 2个方法重载 返回集合 修改人:李晓东  
 *       2013.12.27 新增GetCityById,GetSYSAreaById,GetListDATProjectLikeByCityId,GetDATProjectByProjectNameAndCityId
 *       InsertDATProject,GetListSYSProjectMatch,GetProjectPurposeCodeByCode,GetAllProjectPurposeCodeByCode
 *       GetListSYSCODE方法 修改人:曾智磊
 *       2013.12.27 修改GetCity方法  修改人:曾智磊
 *       2013.12.27 新增GetCityTable(MSSQLDBDAL mssqldbdal, int syscityId)GetSYSArea(MSSQLDBDAL mssqldbdal, int syscityId, string areaName)方法重写 修改人:曾智磊
 *       2013.12.30 新增ProjectTableConvertToProjectModel方法 修改人:曾智磊
 *       2014.02.13 修改人:李晓东
 *                  新增GetCaseByCityIdAndProjectId
 *                      GetListProjectLikeByCityIdAndAreaId
 *                      GetProjectJoinProjectMatchByPNameOrPAddressCityId
 *       2014.02.20 修改人:李晓东
 *                  新增:GetHouseByHouse_City_Building 得到房号
 *                       GetBuildingByProject_City_Build 得到楼栋
 *       2014.06.16 修改人：贺黎亮
 *                 新增 获得指定城市列表 ADO方法
 * **/
namespace FxtNHibernater.Data
{
    public class UtilityDALHelper
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(UtilityDALHelper));


        #region 均价计算
        /// <summary>
        /// 根据sql计算维度均价
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectId"></param>
        /// <param name="areaId"></param>
        /// <param name="subAreaId"></param>
        /// <param name="cityId"></param>
        /// <param name="caseTable"></param>
        /// <param name="typeCode"></param>
        /// <param name="weight_priceBP"></param>
        /// <param name="weight_priceCJ"></param>
        /// <param name="caseMonth"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IList<DATProjectAvgPrice> GetProjectCrossPriceBySQL(MSSQLDBDAL db, int projectId, int areaId, int subAreaId, int cityId, string caseTable, int typeCode, decimal weight_priceBP, decimal weight_priceCJ, int caseMonth, string startDate, string endDate, int fxtCompanyId)
        {
            //int fxtCompanyId = 25;
            int minCaseCount = 3;
            IList<DATProjectAvgPrice> list = new List<DATProjectAvgPrice>();

            if (typeCode == 1002001)//普通住宅
            {
                #region (普通住宅)
                string sql = @" create table #table_case --创建临时表#Tmp
            (
                UnitPrice   numeric(18,2) null,
                BuildingArea  numeric(18,2) null,
                CaseTypeCode int null,
                BuildingTypeCode int null,
            );
            insert #table_case(UnitPrice,BuildingArea,CaseTypeCode,BuildingTypeCode)
            select UnitPrice,BuildingArea,CaseTypeCode,BuildingTypeCode from @dat_case  with(nolock) 
                 where Valid=1 and ProjectId=@projectId and PurposeCode in (@purposeCode) 
                       and FxtCompanyId in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid=@cityId and fxtcompanyid=@fxtcompanyId),',')) 
                       and BuildingArea>@minBuildingArea and UnitPrice>@minUnitPrice 
                       and CaseDate>= @startDate and CaseDate<=@endDate
            select 
                 code as BuildingTypeCode,8006001 as BuildingAreaType,  
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 and BuildingTypeCode=code1.code and BuildingArea<30
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) and BuildingTypeCode=code1.code and BuildingArea<30
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where  CaseTypeCode in (3001001,3001002,3001003) and BuildingTypeCode=code1.code and BuildingArea<30
                ) as CaseCount
            from  sys_code as code1 with(nolock) where id=2003
            union
            select   
                 code as BuildingTypeCode,8006002 as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 and BuildingTypeCode=code1.code and BuildingArea>=30 and BuildingArea<60 
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) and BuildingTypeCode=code1.code and BuildingTypeCode=code1.code and BuildingArea>=30 and BuildingArea<60 
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where  CaseTypeCode in (3001001,3001002,3001003) and BuildingTypeCode=code1.code and BuildingArea>=30 and BuildingArea<60 
                ) as CaseCount
            from  sys_code as code1 with(nolock) where id=2003
            union
            select 
                 code as BuildingTypeCode,8006003 as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 and BuildingTypeCode=code1.code
                       and BuildingArea>=60 and BuildingArea<90
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) and BuildingTypeCode=code1.code
                       and BuildingArea>=60 and BuildingArea<90
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003)  and BuildingTypeCode=code1.code
                       and BuildingArea>=60 and BuildingArea<90
                ) as CaseCount
            from  sys_code as code1 with(nolock) where id=2003
            union
            select 
                 code as BuildingTypeCode,8006004 as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 and BuildingTypeCode=code1.code
                       and BuildingArea>=90 and BuildingArea<=120 
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) and BuildingTypeCode=code1.code
                       and BuildingArea>=90 and BuildingArea<=120 
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003) and BuildingTypeCode=code1.code
                       and BuildingArea>=90 and BuildingArea<=120 
                ) as CaseCount
            from  sys_code as code1 with(nolock) where id=2003
            union
            select 
                 code as BuildingTypeCode,8006005 as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 and BuildingTypeCode=code1.code
                        and BuildingArea>120
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) and BuildingTypeCode=code1.code
                        and BuildingArea>120
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003) and BuildingTypeCode=code1.code
                        and BuildingArea>120
                ) as CaseCount
            from  sys_code as code1 with(nolock) where id=2003
            union 
            select  
                 null as BuildingTypeCode,code as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 
                       and BuildingArea<30 and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) 
                       and BuildingArea<30  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003)
                       and BuildingArea<30  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as CaseCount
            from  sys_code  with(nolock) where code=8006001
            union
            select 
                 null as BuildingTypeCode,code as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001
                       and BuildingArea>=30 and BuildingArea<60 and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003)
                       and BuildingArea>=30 and BuildingArea<60 and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003)
                       and BuildingArea>=30 and BuildingArea<60 and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as CaseCount
            from  sys_code with(nolock)  where code=8006002
            union
            select 
                 null as BuildingTypeCode,code as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 
                       and BuildingArea>=60 and BuildingArea<90  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) 
                       and BuildingArea>=60 and BuildingArea<90  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003) 
                       and BuildingArea>=60 and BuildingArea<90  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as CaseCount
            from  sys_code  with(nolock) where code=8006003
            union
            select 
                 null as BuildingTypeCode,code as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 
                       and BuildingArea>=90 and BuildingArea<=120  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) 
                       and BuildingArea>=90 and BuildingArea<=120  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003) 
                       and BuildingArea>=90 and BuildingArea<=120  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as CaseCount
            from  sys_code  with(nolock) where code=8006004
            union
            select  
                 null as BuildingTypeCode,8006005 as BuildingAreaType,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode=3001001 
                       and BuildingArea>120  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice1,
                (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                 where CaseTypeCode in (3001002,3001003) 
                       and BuildingArea>120  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as AvgPrice2,
                (select count(*) from #table_case 
                 where CaseTypeCode in (3001001,3001002,3001003) 
                       and BuildingArea>120  and (BuildingTypeCode is null or BuildingTypeCode=0)
                ) as CaseCount
            from  sys_code  with(nolock) where code=8006005";
            //drop table #table_case";
                sql = sql.Replace("@cityId", cityId.ToString());
                sql = sql.Replace("@fxtcompanyId", fxtCompanyId.ToString());
                sql = sql.Replace("@projectId", projectId.ToString());
                sql = sql.Replace("@purposeCode", typeCode.ToString() + ",1002002");
                sql = sql.Replace("@startDate", "'" + startDate + "'");
                sql = sql.Replace("@endDate", "'" + endDate + "'");
                sql = sql.Replace("@minUnitPrice", "100");
                sql = sql.Replace("@minBuildingArea", "1");
                sql = sql.Replace("@AreaId", areaId.ToString());
                sql = sql.Replace("@SubAreaId", subAreaId.ToString());
                sql = sql.Replace("@AvgPriceDate", "'" + Convert.ToDateTime(endDate).ToString("yyyyMM") + "'");
                sql = sql.Replace("@CreateTime", "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                sql = sql.Replace("@dat_case", caseTable);
                IList objList = db.GetCustomSQLQueryObjectList(sql);
                string avgPriceDate = Convert.ToDateTime(endDate).ToString("yyyyMM");
                DateTime createTime = DateTime.Now;
                for (int i = 0; i < objList.Count; i++)
                {
                    object[] objs = objList[i] as object[];
                    int buildingTypeCode = Convert.ToInt32(objs[0]);
                    int buildingAreaType = Convert.ToInt32(objs[1]);
                    int avgPrice1 = Convert.ToInt32(objs[2]);//报盘案例
                    int avgPrice2 = Convert.ToInt32(objs[3]);//评估案例+成交案例
                    int caseCount = Convert.ToInt32(objs[4]);
                    int avgPrice = 0;
                    if (caseCount >= minCaseCount)
                    {
                        if (avgPrice1 == 0)
                        {
                            avgPrice = avgPrice2;
                        }
                        else if (avgPrice2 == 0)
                        {
                            avgPrice = avgPrice1;
                        }
                        else
                        {
                            avgPrice = Convert.ToInt32((avgPrice1 * weight_priceBP) + (avgPrice2 * weight_priceCJ));
                        }
                    }
                    DATProjectAvgPrice objResult = new DATProjectAvgPrice
                    {
                        AreaId = areaId,
                        AvgPrice = avgPrice,
                        AvgPriceDate = avgPriceDate,
                        BuildingAreaType = buildingAreaType,
                        BuildingTypeCode = buildingTypeCode,
                        CityId = cityId,
                        CreateTime = createTime,
                        DateRange = caseMonth,
                        FxtCompanyId = fxtCompanyId,
                        Id = i,
                        JSFS = "",
                        ProjectId = projectId,
                        PurposeType = typeCode,
                        SubAreaId = subAreaId
                    };
                    list.Add(objResult);
                }
                #endregion

            }
            else if (typeCode == 1002027)//别墅
            {
                string sql = @"create table #table_case 
                    (
                        UnitPrice   numeric(18,2) null,
                        CaseTypeCode int null,
                        PurposeCode int null,
                        BuildingArea  numeric(18,2) null
                    );
                    insert #table_case(UnitPrice,CaseTypeCode,PurposeCode)
                    select UnitPrice,CaseTypeCode,PurposeCode from @dat_case  with(nolock) 
                         where Valid=1 and ProjectId=@projectId and PurposeCode in (select code from sys_code  as code1  with(nolock) where id=1002 and codename like '%别墅%') 
                               and FxtCompanyId in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid=@cityId and fxtcompanyid=@fxtcompanyId),',')) 
                               and BuildingArea>@minBuildingArea and UnitPrice>@minUnitPrice 
                               and CaseDate>= @startDate and CaseDate<=@endDate
                    select    
                         code1.code as PurposeType,
                        (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                         where CaseTypeCode=3001001 and PurposeCode=code1.code
                        ) as AvgPrice1,
                        (select sum(UnitPrice*BuildingArea)/sum(BuildingArea) from #table_case 
                         where CaseTypeCode in (3001002,3001003) and  PurposeCode=code1.code
                        ) as AvgPrice2,
                        (select count(*) from #table_case 
                         where  CaseTypeCode in (3001001,3001002,3001003)  and PurposeCode=code1.code
                        ) as CaseCount
                    from sys_code as code1  with(nolock)  where id=1002 and codename like '%别墅%'";
                    //drop table #table_case";
                sql = sql.Replace("@cityId", cityId.ToString());
                sql = sql.Replace("@fxtcompanyId", fxtCompanyId.ToString());
                sql = sql.Replace("@projectId", projectId.ToString());
                sql = sql.Replace("@startDate", "'" + startDate + "'");
                sql = sql.Replace("@endDate", "'" + endDate + "'");
                sql = sql.Replace("@minUnitPrice", "100");
                sql = sql.Replace("@minBuildingArea", "1");
                sql = sql.Replace("@dat_case", caseTable);
                IList objList = db.GetCustomSQLQueryObjectList(sql);
                string avgPriceDate = Convert.ToDateTime(endDate).ToString("yyyyMM");
                DateTime createTime = DateTime.Now;
                for (int i = 0; i < objList.Count; i++)
                {
                    object[] objs = objList[i] as object[];
                    int purposeType = Convert.ToInt32(objs[0]);
                    int avgPrice1 = Convert.ToInt32(objs[1]);//报盘案例
                    int avgPrice2 = Convert.ToInt32(objs[2]);//评估案例+成交案例
                    int caseCount = Convert.ToInt32(objs[3]);
                    int avgPrice = 0;
                    if (caseCount >= minCaseCount)
                    {
                        if (avgPrice1 == 0)
                        {
                            avgPrice = avgPrice2;
                        }
                        else if (avgPrice2 == 0)
                        {
                            avgPrice = avgPrice1;
                        }
                        else
                        {
                            avgPrice = Convert.ToInt32((avgPrice1 * weight_priceBP) + (avgPrice2 * weight_priceCJ));
                        }
                    }
                    DATProjectAvgPrice objResult = new DATProjectAvgPrice
                    {
                        AreaId = areaId,
                        AvgPrice = avgPrice,
                        AvgPriceDate = avgPriceDate,
                        BuildingAreaType = 0,
                        BuildingTypeCode = 0,
                        CityId = cityId,
                        CreateTime = createTime,
                        DateRange = caseMonth,
                        FxtCompanyId = fxtCompanyId,
                        Id = i,
                        JSFS = "",
                        ProjectId = projectId,
                        PurposeType = purposeType,
                        SubAreaId = subAreaId
                    };
                    list.Add(objResult);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取计算均价
        /// </summary>
        /// <param name="db"></param>
        /// <param name="project"></param>
        /// <param name="cityTable"></param>
        /// <param name="city"></param>
        /// <param name="typeCode"></param>
        /// <param name="date"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        static IList<DATProjectAvgPrice> GetProjectCrossPrice(MSSQLDBDAL db, DATProject project, SYSCityTable cityTable, SYSCity city, int typeCode, string date, int max,int fxtCompanyId)
        {
            int _max = max + 1;//当前往前退的月份次数
            IList<DATProjectAvgPrice> price = null;
            IList<DATProjectAvgPrice> resultPrice = new List<DATProjectAvgPrice>();
            IList<DATProjectAvgPrice> price1 = null;//上个月数据

            DateTime dt = Convert.ToDateTime(date);//传值日期
            int caseMonth = Convert.ToInt32(city.CaseMonth);//计算范围(月)
            decimal weight_priceBP = Convert.ToDecimal(city.PriceBP);//报盘案例权重
            decimal weight_priceCJ = Convert.ToDecimal(city.PriceCJ);//买卖成交+评估案例权重
            string startDate = dt.AddMonths(0 - caseMonth).ToString("yyyy-MM-dd");
            string endDate = dt.ToString("yyyy-MM-dd");
            price = GetProjectCrossPriceBySQL(db, project.ProjectId, project.AreaID, Convert.ToInt32(project.SubAreaId), city.CityId, cityTable.CaseTable, typeCode, weight_priceBP, weight_priceCJ, caseMonth, startDate, endDate,fxtCompanyId);
            for (int i = 0; i < price.Count; i++)
            {
                DATProjectAvgPrice objResult = new DATProjectAvgPrice
                {
                    AreaId = price[i].AreaId,
                    AvgPrice = price[i].AvgPrice,
                    AvgPriceDate = price[i].AvgPriceDate,
                    BuildingAreaType = price[i].BuildingAreaType,
                    BuildingTypeCode = price[i].BuildingTypeCode,
                    CityId = price[i].CityId,
                    CreateTime = price[i].CreateTime,
                    DateRange = price[i].DateRange,
                    FxtCompanyId = price[i].FxtCompanyId,
                    Id = price[i].Id,
                    JSFS = price[i].JSFS,
                    ProjectId = price[i].ProjectId,
                    PurposeType = price[i].PurposeType,
                    SubAreaId = price[i].SubAreaId
                };
                //当前纬度无值&&当前往前退的月份次数不到三个月
                if (objResult.AvgPrice <= 0 && max <= 1)
                {
                    //获取上个月数据
                    if (price1 == null)
                    {
                        price1 = GetProjectCrossPrice(db, project, cityTable, city, typeCode, Convert.ToDateTime(date).AddMonths(-1).ToString("yyyy-MM-dd"), _max, fxtCompanyId);
                    }
                    if (typeCode == 1002001)//普通住宅
                    {
                        //如果为面积段总均价
                        if (Convert.ToInt32(objResult.BuildingTypeCode) == 0)
                        {
                            goto continue_end;
                        }
                        int nowBuildingAreaType = Convert.ToInt32(objResult.BuildingAreaType);//当前面积段
                        int nowBuildingTypeCode = Convert.ToInt32(objResult.BuildingTypeCode);//当前建筑类型
                        //上个月当前维度数据
                        DATProjectAvgPrice obj2 = price1.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) == nowBuildingAreaType && Convert.ToInt32(_obj.BuildingTypeCode) == nowBuildingTypeCode).FirstOrDefault();
                        if (obj2 == null || obj2.AvgPrice <= 0)
                        {
                            goto continue_end;
                        }
                        int obj2AvgPrice = obj2.AvgPrice;//上个月当前纬度均价
                        decimal sumFloatValue = 0;
                        int countFloatValue = 0;
                        //这个月当前面积段数据List(同面积段)
                        IList<DATProjectAvgPrice> priceBuildingArea = price.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) == nowBuildingAreaType && Convert.ToInt32(_obj.BuildingTypeCode) != nowBuildingTypeCode).ToList();
                        foreach (DATProjectAvgPrice _obj2 in priceBuildingArea)
                        {
                            if (_obj2.AvgPrice > 0)
                            {
                                //上个月此维度数据
                                DATProjectAvgPrice obj3 = price1.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) == Convert.ToInt32(_obj2.BuildingAreaType) && Convert.ToInt32(_obj.BuildingTypeCode) == Convert.ToInt32(_obj2.BuildingTypeCode)).FirstOrDefault();
                                //计算与上个月同维度的浮动值
                                if (obj3 != null && obj3.AvgPrice > 0)
                                {
                                    decimal floatValue = Convert.ToDecimal(Convert.ToDecimal(_obj2.AvgPrice) / Convert.ToDecimal(obj3.AvgPrice));
                                    sumFloatValue = sumFloatValue + floatValue;
                                    countFloatValue = countFloatValue + 1;
                                }
                            }
                        }
                        //获取前纬度均价(如果成功就继续下一个循环)
                        if (countFloatValue > 0)
                        {
                            decimal nowFloatValue = Convert.ToDecimal(sumFloatValue / countFloatValue);
                            objResult.AvgPrice = Convert.ToInt32(obj2AvgPrice * nowFloatValue);
                            goto continue_end;
                        }
                        //如果同面积段下计算不出均价(则用同建筑类型)
                        sumFloatValue = 0;
                        countFloatValue = 0;
                        IList<DATProjectAvgPrice> priceBuildingTypeCode = price.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) != nowBuildingAreaType && Convert.ToInt32(_obj.BuildingTypeCode) == nowBuildingTypeCode).ToList();
                        foreach (DATProjectAvgPrice _obj2 in priceBuildingTypeCode)
                        {
                            if (_obj2.AvgPrice > 0)
                            {
                                //上个月此维度数据
                                DATProjectAvgPrice obj3 = price1.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) == Convert.ToInt32(_obj2.BuildingAreaType) && Convert.ToInt32(_obj.BuildingTypeCode) == Convert.ToInt32(_obj2.BuildingTypeCode)).FirstOrDefault();
                                //计算与上个月同维度的浮动值
                                if (obj3 != null && obj3.AvgPrice > 0)
                                {
                                    decimal floatValue = Convert.ToDecimal(Convert.ToDecimal(_obj2.AvgPrice) / Convert.ToDecimal(obj3.AvgPrice));
                                    sumFloatValue = sumFloatValue + floatValue;
                                    countFloatValue = countFloatValue + 1;
                                }
                            }
                        }
                        //获取前纬度均价
                        if (countFloatValue > 0)
                        {
                            decimal nowFloatValue = Convert.ToDecimal(sumFloatValue / countFloatValue);
                            objResult.AvgPrice = Convert.ToInt32(obj2AvgPrice * nowFloatValue);
                            objResult.JSFS = "上月同维度值*同面积段平均涨跌幅";
                            goto continue_end;
                        }
                        //如果同建筑类型下计算不出均价(则用其他维度)
                        sumFloatValue = 0;
                        countFloatValue = 0;
                        IList<DATProjectAvgPrice> priceAllCode = price.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) != nowBuildingAreaType && Convert.ToInt32(_obj.BuildingTypeCode) != nowBuildingTypeCode).ToList();
                        foreach (DATProjectAvgPrice _obj2 in priceBuildingTypeCode)
                        {
                            if (_obj2.AvgPrice > 0)
                            {
                                //上个月此维度数据
                                DATProjectAvgPrice obj3 = price1.Where(_obj => Convert.ToInt32(_obj.BuildingAreaType) == Convert.ToInt32(_obj2.BuildingAreaType) && Convert.ToInt32(_obj.BuildingTypeCode) == Convert.ToInt32(_obj2.BuildingTypeCode)).FirstOrDefault();
                                //计算与上个月同维度的浮动值
                                if (obj3 != null && obj3.AvgPrice > 0)
                                {
                                    decimal floatValue = Convert.ToDecimal(Convert.ToDecimal(_obj2.AvgPrice) / Convert.ToDecimal(obj3.AvgPrice));
                                    sumFloatValue = sumFloatValue + floatValue;
                                    countFloatValue = countFloatValue + 1;
                                }
                            }
                        }
                        //获取前纬度均价
                        if (countFloatValue > 0)
                        {
                            decimal nowFloatValue = Convert.ToDecimal(sumFloatValue / countFloatValue);
                            objResult.AvgPrice = Convert.ToInt32(obj2AvgPrice * nowFloatValue);
                            objResult.JSFS = "上月同维度值*同建筑类型平均涨跌幅";
                            goto continue_end;
                        }

                    }
                    else if (typeCode == 1002027)//别墅类型
                    {

                        int nowPurposeType = Convert.ToInt32(objResult.PurposeType);//当用途
                        //上个月当前维度数据
                        DATProjectAvgPrice obj2 = price1.Where(_obj => Convert.ToInt32(_obj.PurposeType) == nowPurposeType).FirstOrDefault();
                        if (obj2 == null || obj2.AvgPrice <= 0)
                        {
                            continue;
                        }
                        int obj2AvgPrice = obj2.AvgPrice;//上个月当前纬度均价
                        decimal sumFloatValue = 0;
                        int countFloatValue = 0;
                        IList<DATProjectAvgPrice> priceAllCode = price.Where(_obj => Convert.ToInt32(_obj.PurposeType) != nowPurposeType).ToList();
                        foreach (DATProjectAvgPrice _obj2 in priceAllCode)
                        {
                            if (_obj2.AvgPrice > 0)
                            {
                                //上个月此维度数据
                                DATProjectAvgPrice obj3 = price1.Where(_obj => Convert.ToInt32(_obj.PurposeType) == nowPurposeType).FirstOrDefault();
                                //计算与上个月同维度的浮动值
                                if (obj3 != null && obj3.AvgPrice > 0)
                                {
                                    decimal floatValue = Convert.ToDecimal(Convert.ToDecimal(_obj2.AvgPrice) / Convert.ToDecimal(obj3.AvgPrice));
                                    sumFloatValue = sumFloatValue + floatValue;
                                    countFloatValue = countFloatValue + 1;
                                }
                            }
                        }
                        //获取前纬度均价
                        if (countFloatValue > 0)
                        {
                            decimal nowFloatValue = Convert.ToDecimal(sumFloatValue / countFloatValue);
                            objResult.AvgPrice = Convert.ToInt32(obj2AvgPrice * nowFloatValue);
                            objResult.JSFS = "上月同维度值*其他用途平均涨跌幅";
                        }
                    }
                continue_end:
                    resultPrice.Add(objResult);
                }
            }
            return price;

        }
        /// <summary>
        /// 获取计算均价
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="typeCode"></param>
        /// <param name="date"></param>
        /// <param name="buildingAreaSum">是否需汇总计算面积段均价</param>
        /// <returns></returns>

        public static IList<DATProjectAvgPrice> GetProjectCrossPrice(MSSQLDBDAL db, int projectId, int cityId, int typeCode, string date, bool buildingAreaSum, int fxtCompanyId)
        {
            //获得城市
            SYSCity cityObj = GetCityById(db, cityId);
            IList<DATProjectAvgPrice> result = new List<DATProjectAvgPrice>();
            if (cityObj == null)
            {
                return new List<DATProjectAvgPrice>();
            }
            //获得案例表
            SYSCityTable vCaseTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (vCaseTable == null)
            {
                return new List<DATProjectAvgPrice>();
            }
            //属于哪个楼盘
            DATProject vProject = GetProjectByTableNameAndProjectId(db, vCaseTable.ProjectTable, projectId);
            if (vProject == null)
            {
                return new List<DATProjectAvgPrice>();
            }
            result = GetProjectCrossPrice(db, vProject, vCaseTable, cityObj, typeCode, date, 0,fxtCompanyId);

            if (typeCode == 1002001 && buildingAreaSum)//普通住宅&&需汇总计算面积段均价
            {
                //计算各面积段均价
                int[] buildingAreas = new int[] { 8006001, 8006002, 8006003, 8006004, 8006005 };
                foreach (int code in buildingAreas)
                {
                    DATProjectAvgPrice price = result.Where(obj => obj.BuildingAreaType == code && Convert.ToInt32(obj.BuildingTypeCode) == 0).FirstOrDefault();
                    DATProjectAvgPrice objResult = new DATProjectAvgPrice
                    {
                        AreaId = price.AreaId,
                        AvgPrice = price.AvgPrice,
                        AvgPriceDate = price.AvgPriceDate,
                        BuildingAreaType = price.BuildingAreaType,
                        BuildingTypeCode = price.BuildingTypeCode,
                        CityId = price.CityId,
                        CreateTime = price.CreateTime,
                        DateRange = price.DateRange,
                        FxtCompanyId = price.FxtCompanyId,
                        Id = price.Id,
                        JSFS = price.JSFS,
                        ProjectId = price.ProjectId,
                        PurposeType = price.PurposeType,
                        SubAreaId = price.SubAreaId
                    };
                    objResult.AvgPrice = 0;
                    List<DATProjectAvgPrice> list = result.Where(obj => obj.BuildingAreaType == code && obj.AvgPrice > 0).ToList();
                    int count = list.Count;
                    int pirceAvg = list.Sum(obj => obj.AvgPrice);
                    if (count > 0)
                    {
                        objResult.AvgPrice = pirceAvg / count;
                    }
                    result.Remove(price);
                    result.Add(objResult);
                }
            }
            return result;
        }
        #endregion

        #region 省份
        /// <summary>
        /// 获得省份
        /// </summary>
        /// <param name="mssqlado"></param>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public static SYSProvince GetADOProvinceById(MSSQLADODAL mssqlado, int provinceIdId)
        {
            string sql = string.Format("{0} ProvinceId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSProvince), Utility.SYSProvince), provinceIdId);

            return mssqlado.GetModel<SYSProvince>(sql);
        }
        #endregion

        #region 城市(SYSCity)

        /// <summary>
        /// 获得城市
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="syscity">城市对象</param>
        /// <returns></returns>
        public static SYSCity GetCity(MSSQLDBDAL mssqldbdal, SYSCity syscity)
        {
            return mssqldbdal.GetCustom<SYSCity>(
                    (Expression<Func<SYSCity, bool>>)(_syscity =>
                    _syscity.CityName == syscity.CityName ||
                    _syscity.Alias == syscity.CityName ||
                    _syscity.CityName == string.Format("{0}市", syscity.CityName))
                    );
        }
        /// <summary>
        /// 获得城市
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static SYSCity GetCityById(MSSQLDBDAL mssqldbdal, int cityId)
        {
            return mssqldbdal.GetCustom<SYSCity>(
                    (Expression<Func<SYSCity, bool>>)(_syscity =>
                    _syscity.CityId == cityId)
                    );
        }

        /// <summary>
        /// 获得城市
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static SYSCity GetADOCityById(MSSQLADODAL mssqlado, int cityId)
        {
            string sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), cityId);

            return mssqlado.GetModel<SYSCity>(sql);
        }

        /// <summary>
        /// 获得城市
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        public static SYSCity GetCityName(MSSQLDBDAL mssqldbdal, string cityName)
        {
            return mssqldbdal.GetCustom<SYSCity>(
                    (Expression<Func<SYSCity, bool>>)(_syscity =>
                    _syscity.CityName == cityName ||
                    _syscity.Alias == cityName ||
                    _syscity.CityName == string.Format("{0}市", cityName))
                    );
        }
        /// <summary>
        /// 获得城市列表
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public static IList<SYSCity> GetCity(MSSQLDBDAL mssqldbdal, int provinceId)
        {
            return mssqldbdal.GetListCustom<SYSCity>(
                    (Expression<Func<SYSCity, bool>>)
                    (_sysprovince =>
                        _sysprovince.ProvinceId == provinceId
                    )).ToList<SYSCity>();
        }
        /// <summary>
        /// 获得城市列表 ADO
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public static IList<SYSCity> GetADOCity(MSSQLADODAL mssqldbdal, int provinceId)
        {
            string sql = string.Format("{0} ProvinceId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), provinceId);
            return mssqldbdal.GetList<SYSCity>(sql);
        }
        /// <summary>
        /// 获得指定城市列表 ADO
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public static IList<SYSCity> GetADOCity(MSSQLADODAL mssqldbdal, string cityid)
        {
            string sql =string.Empty;
            if (string.IsNullOrEmpty(cityid))
            {
                sql = string.Format("{0} 1=1 ",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity));
            }
            else
            {
                sql = string.Format("{0} cityid in ({1}) ",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), cityid);
            }
            return mssqldbdal.GetList<SYSCity>(sql);
        }
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IList<SYSCity> GetAllCity(MSSQLDBDAL db)
        {
            return db.GetListCustom<SYSCity>().ToList<SYSCity>();
        }

        #endregion

        #region 城市对应分区(SYSCityTable)
        /// <summary>
        /// 获得城市分区楼盘表
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="syscity">城市对象</param>
        /// <returns></returns>
        public static SYSCityTable GetCityTable(MSSQLDBDAL mssqldbdal, SYSCity syscity)
        {
            string sql = string.Format("{0} CityId ={1}",
                          Utility.GetMSSQL_SQL2(typeof(SYSCityTable), Utility.SYSCityTable.Trim()),
                          syscity.CityId);
            SYSCityTable obj = mssqldbdal.GetCustomSQLQueryEntity<SYSCityTable>(sql);
            return obj;
        }
        public static SYSCityTable GetCityTableByCityName(MSSQLDBDAL db, string cityName)
        {
            string sql = string.Format("{0} CityId in (select CityId from {1} where CityName='{2}' or CityName='{2}市' or Alias='{2}' )",
                          Utility.GetMSSQL_SQL2(typeof(SYSCityTable), Utility.SYSCityTable.Trim()),
                          Utility.SYSCity,
                          cityName);
            SYSCityTable obj = db.GetCustomSQLQueryEntity<SYSCityTable>(sql);
            return obj;
        }
        /// <summary>
        /// 获得城市分区楼盘表
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="syscityId">城市Id</param>
        /// <returns></returns>
        public static SYSCityTable GetCityTable(MSSQLDBDAL mssqldbdal, int syscityId)
        {
            string sql = string.Format("{0} CityId ={1}",
                          Utility.GetMSSQL_SQL2(typeof(SYSCityTable), Utility.SYSCityTable.Trim()),
                          syscityId);
            SYSCityTable obj = mssqldbdal.GetCustomSQLQueryEntity<SYSCityTable>(sql);
            return obj;
        }
        /// <summary>
        /// 获得城市分区楼盘表
        /// </summary>
        /// <param name="mssqldbdal">MSSQLADODAL类对象</param>
        /// <param name="syscityId">城市Id</param>
        /// <returns></returns>
        public static SYSCityTable GetCityADOTable(MSSQLADODAL mssqlado, int syscityId)
        {
            string sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL2(typeof(SYSCityTable), Utility.SYSCityTable), syscityId);
            return mssqlado.GetModel<SYSCityTable>(sql);
        }
        #endregion

        #region 行政区(SYSArea)
        /// <summary>
        /// 获得城市的行政区
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="syscity">城市对象</param>
        /// <param name="areaName">行政区名</param>
        /// <returns></returns>
        public static SYSArea GetSYSArea(MSSQLDBDAL mssqldbdal, SYSCity syscity, string areaName)
        {
            if (syscity == null)
            {
                return null;
            }
            var vsysArea = mssqldbdal.GetListCustom<SYSArea>(
                    (Expression<Func<SYSArea, bool>>)
                    (sysara => sysara.CityId == syscity.CityId));
            if (areaName != null)
                return vsysArea.Where(item => item.AreaName.Contains(areaName) || areaName.Contains(item.AreaName)).FirstOrDefault();
            return null;
        }
        /// <summary>
        /// 获得城市的行政区
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="syscityId">城市Id</param>
        /// <param name="areaName">行政区名</param>
        /// <returns></returns>
        public static SYSArea GetSYSArea(MSSQLDBDAL mssqldbdal, int syscityId, string areaName)
        {
            var vsysArea = mssqldbdal.GetListCustom<SYSArea>(
                    (Expression<Func<SYSArea, bool>>)
                    (sysara => sysara.CityId == syscityId));
            if (areaName != null)
                return vsysArea.Where(item => item.AreaName.Contains(areaName) || areaName.Contains(item.AreaName)).FirstOrDefault();
            return null;
        }
        /// <summary>
        /// 获取行政区 根据ID
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static SYSArea GetSYSAreaById(MSSQLDBDAL mssqldbdal, int areaId)
        {
            return mssqldbdal.GetCustom<SYSArea>(
                    (Expression<Func<SYSArea, bool>>)(_obj =>
                    _obj.AreaId == areaId)
                    );
        }

        /// <summary>
        /// 获取行政区 根据ID ADO
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static SYSArea GetADOSYSAreaById(MSSQLADODAL mssqlado, int areaId)
        {
            string sql = string.Format("{0} AreaId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea), areaId);
            return mssqlado.GetModel<SYSArea>(sql);
        }

        /// <summary>
        /// 获得城市的行政区
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        public static List<SYSArea> GetSYSArea(MSSQLDBDAL mssqldbdal, string cityName)
        {
            var vsysCity = GetCity(mssqldbdal, new SYSCity() { CityName = cityName });
            var vsysArea1 = mssqldbdal.GetListCustom<SYSArea>(
                (Expression<Func<SYSArea, bool>>)
                    (sysara => sysara.CityId == vsysCity.CityId)
                );
            return vsysArea1.ToList<SYSArea>();
        }

        /// <summary>
        /// 获得城市行政区列表
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public static IList<SYSArea> GetSYSArea(MSSQLDBDAL mssqldbdal, int cityId)
        {            
            var vsysArea = mssqldbdal.GetListCustom<SYSArea>(
                    (Expression<Func<SYSArea, bool>>)
                    (sysara => sysara.CityId == cityId));
            return vsysArea.ToList<SYSArea>();
        }
        /// <summary>
        /// 获得城市行政区列表 ADO
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public static IList<SYSArea> GetADOSYSArea(MSSQLADODAL mssqldbdal, int cityId)
        {
            string sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea), cityId);
            var vsysArea = mssqldbdal.GetList<SYSArea>(sql);
            return vsysArea;
        }
        /// <summary>
        /// 获得指定城市行政区列表 ADO
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public static IList<SYSArea> GetADOSYSArea(MSSQLADODAL mssqldbdal, string cityId)
        {
            string sql = string.Format("{0} CityId in({1})",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea), cityId);
            var vsysArea = mssqldbdal.GetList<SYSArea>(sql);
            return vsysArea;
        }
        /// <summary>
        /// 根据多个areaId获取行政区
        /// </summary>
        /// <param name="db"></param>
        /// <param name="areaIds"></param>
        /// <returns></returns>
        public static IList<SYSArea> GetSYSAreaByAreaIds(MSSQLDBDAL db, int[] areaIds)
        {
            if (areaIds == null || areaIds.Length < 1)
            {
                return new List<SYSArea>();
            }
            //var list = db.GetListCustom<SYSArea>(
            //    (Expression<Func<SYSArea, bool>>)
            //    (p => areaIds.Contains(p.AreaId)));

            IList<SYSArea> list = db.CreateCriteria(typeof(SYSArea)).Add(Restrictions.In("AreaId", areaIds)).List<SYSArea>();


            return list;
        }

        #endregion

        #region 楼盘信息(DATProject)

        #region (查询)

        /// <summary>
        /// 根据模糊名称模糊查找楼盘信息
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="projectNameLike"></param>
        /// <param name="tableName"></param>
        /// <param name="cityId"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IList<DATProject> GetListProjectLikeByCityId(MSSQLDBDAL mssqldbdal, string projectNameLike, int cityId, int length)
        {
            //获取对应区域表obj
            SYSCityTable cityTable = UtilityDALHelper.GetCityTable(mssqldbdal, cityId);
            if (cityTable == null)
            {
                return new List<DATProject>();
            }
            string tableName = cityTable.ProjectTable;
            string sql = string.Format(new StringBuilder().Append("{0} Valid=1 and  CityID={1}")
                             .Append(" and ( ProjectName like '%{2}%' or OtherName like '%{2}%' ")
                             .Append(" or exists  (select ProjectNameId from SYS_ProjectMatch as tb2 where tb2.CityId=_tb.CityId and  _tb.ProjectId=ProjectNameId ")
                             .Append(" and NetName like '%{2}%'))").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATProject), tableName.Trim()),
                          cityId.ToString(),
                         projectNameLike);
            IList<DATProject> list = null;
            UtilityPager page = new UtilityPager(pageSize: length, pageIndex: 1);
            list = mssqldbdal.PagerList<DATProject>(page, sql).ToList<DATProject>();
            return list;
        }

        /// <summary>
        /// 根据模糊名称模糊查找楼盘信息 李晓东
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="pName">楼盘名称</param>
        /// <param name="cId">城市ID</param>
        /// <param name="aId">区域ID</param>
        /// <returns></returns>
        public static IList<DATProject> GetListProjectLikeByCityIdAndAreaId(MSSQLADODAL mssqladodal, string pName, int cId, int aId)
        {
            //获取对应区域表obj
            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(mssqladodal, cId);
            if (cityTable == null)
            {
                return new List<DATProject>();
            }
            string tableName = cityTable.ProjectTable;
            string sql = string.Format(new StringBuilder().Append("{0} Valid=1 and  CityID={1}  ")//and AreaId={3}
                             .Append(" and ( ProjectName like '%{2}%' or OtherName like '%{2}%' ")
                             .Append(" or exists  (select ProjectNameId from SYS_ProjectMatch as tb2 where tb2.CityId=_tb.CityId and  _tb.ProjectId=ProjectNameId ")
                             .Append(" and NetName like '%{2}%'))").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATProject), tableName.Trim()),
                          cId, pName);//, aId
            IList<DATProject> list = mssqladodal.GetList<DATProject>(sql);
            list = list.OrderBy(orderItem => orderItem.ProjectName).ToList<DATProject>();
            return list;
        }
        /// <summary>
        /// 根据名称和城市获取楼盘
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="projectTable"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static DATProject GetProjectByProjectNameAndCityId(MSSQLDBDAL mssqldbdal, string projectTable, int cityId, string projectName)
        {
            //string hsql = string.Format("{0} Valid=1 and CityID={1} and ProjectName ='{2}'",
            //              Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim()),
            //              cityId.ToString(),
            //             projectName);
            //IList<DATProject> list = mssqldbdal.GetCustomSQLQueryList<DATProject>(hsql).ToList<DATProject>();
            DATProject obj = GetProjectByCompanyIdAndCityIdAndProjectName(mssqldbdal, projectTable, 25, cityId, projectName);
            return obj;
        }
        /// <summary>
        /// 根据别名查找楼盘
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="projectTable"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static DATProject GetProjectByOtherNameAndCityId(MSSQLDBDAL mssqldbdal, string projectTable, int cityId, string projectName)
        {
            //string hsql = string.Format("{0} Valid=1 and  CityID={1} and OtherName ='{2}'",
            //              Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim()),
            //              cityId.ToString(),
            //             projectName);
            //IList<DATProject> list = mssqldbdal.GetCustomSQLQueryList<DATProject>(hsql).ToList<DATProject>();
            DATProject projObj = GetProjectByCompanyIdAndCityIdAndOtherName(mssqldbdal, projectTable, 25, cityId, projectName);
            return projObj;
        }

        /// <summary>
        /// 根据名称和城市获得楼盘信息(关联网络名查询)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectJoinProjectMatchByProjectNameCityId(MSSQLDBDAL db, int cityId, string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return new List<DATProject>();
            }
            SYSCityTable table = UtilityDALHelper.GetCityTable(db, cityId);
            if (table == null)
            {
                return new List<DATProject>();
            }
            string projectTable = table.ProjectTable;
            string sql = string.Format(new StringBuilder().Append("{0} Valid=1 and CityID={1} ")
                                                          .Append("and ( ProjectName ='{2}' or OtherName='{2}'")
                                                          .Append(" or exists  ( select * from SYS_ProjectMatch as tb2 where tb2.CityId=_tb.CityId  and  _tb.ProjectId=ProjectNameId  and NetName ='{2}')")
                                                         .Append(" )").ToString(),
                        Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim()),
                        cityId.ToString(),
                       projectName);
            IList<DATProject> list = db.GetCustomSQLQueryList<DATProject>(sql).ToList<DATProject>();
            return list;
        }

        /// <summary>
        /// 根据名称和城市获得楼盘信息(关联网络名查询)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectJoinProjectMatchADOByProjectNameCityId(MSSQLADODAL db, int cityId, string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return new List<DATProject>();
            }
            SYSCityTable table = UtilityDALHelper.GetCityADOTable(db, cityId);
            if (table == null)
            {
                return new List<DATProject>();
            }
            string projectTable = table.ProjectTable;
            string sql = string.Format(new StringBuilder().Append("{0} Valid=1 and CityID={1} ")
                                                          .Append("and ( ProjectName ='{2}' or OtherName='{2}'")
                                                          .Append(" or exists  ( select * from SYS_ProjectMatch as tb2 where tb2.CityId=_tb.CityId  and  _tb.ProjectId=ProjectNameId  and NetName ='{2}')")
                                                         .Append(" )").ToString(),
                        Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim()),
                        cityId.ToString(),
                       projectName);
            List<DATProject> list = db.GetList<DATProject>(sql);
            return list;
        }

        /// <summary>
        /// 根据名称、地址和城市获得楼盘信息(关联网络名查询) 李晓东
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectJoinProjectMatchByPNameOrPAddressCityId(MSSQLDBDAL db, int cityId, string pName, string pAdress)
        {
            if (string.IsNullOrEmpty(pName))
            {
                return new List<DATProject>();
            }
            SYSCityTable table = UtilityDALHelper.GetCityTable(db, cityId);
            if (table == null)
            {
                return new List<DATProject>();
            }
            string projectTable = table.ProjectTable;
            string sql = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1}")
                .Append(" and ( ProjectName like '%{2}%' or OtherName like '%{2}%' or [Address] like '%{3}%' ")
                .Append(" or exists  (select ProjectNameId from SYS_ProjectMatch as tb2 where tb2.CityId=_tb.CityId and  _tb.ProjectId=ProjectNameId ")
                .Append(" and NetName like '%{2}%'))").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim()),
                          cityId.ToString(),
                         pName, pAdress);
            IList<DATProject> list = db.GetCustomSQLQueryList<DATProject>(sql).ToList<DATProject>();
            return list;
        }

        /// <summary>
        /// 根据名称、地址和城市获得楼盘信息(关联网络名查询) 李晓东 ADO
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectJoinProjectMatchADOByPNameOrPAddressCityId(MSSQLADODAL db, int cityId, string pName, string pAdress)
        {
            if (string.IsNullOrEmpty(pName))
            {
                return new List<DATProject>();
            }
            SYSCityTable table = UtilityDALHelper.GetCityADOTable(db, cityId);
            if (table == null)
            {
                return new List<DATProject>();
            }
            string projectTable = table.ProjectTable;
            string sql = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1}")
                .Append(" and ( ProjectName like '%{2}%' or OtherName like '%{2}%' or [Address] like '%{3}%' ")
                .Append(" or exists  (select ProjectNameId from SYS_ProjectMatch as tb2 where tb2.CityId=_tb.CityId and  _tb.ProjectId=ProjectNameId ")
                .Append(" and NetName like '%{2}%'))").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim()),
                          cityId.ToString(),
                         pName, pAdress);
            IList<DATProject> list = db.GetList<DATProject>(sql);
            return list;
        }

        /// <summary>
        /// 根据城市ID and 多个楼盘Ids获取案例信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectByCityIdAndProjectIds(MSSQLDBDAL db, int cityId, int[] projectIds)
        {
            string projectIdsStr = projectIds.ConvertToString();
            if (string.IsNullOrEmpty(projectIdsStr))
            {
                return new List<DATProject>();
            }
            SYSCityTable cityTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (cityTable == null)
            {
                return new List<DATProject>();
            }
            string projectTable = cityTable.ProjectTable;
            string sql = string.Format("{0} ProjectId in ({1})", Utility.GetMSSQL_SQL(typeof(DATProject), tablename: projectTable)
                , projectIdsStr);
            IList<DATProject> list = db.GetCustomSQLQueryList<DATProject>(sql).ToList<DATProject>();
            return list;
        }

        public static DATProject GetProjectByTableNameAndProjectId(MSSQLDBDAL db, string tableName, int projectId)
        {
            string sql = string.Format("{0}  Valid=1 and ProjectId ={1}", Utility.GetMSSQL_SQL(typeof(DATProject), tablename: tableName)
                     , projectId);
            DATProject obj = db.GetCustomSQLQueryEntity<DATProject>(sql);
            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityName"></param>
        /// <param name="count">返回总个数</param>
        /// <param name="isGetCount">是否获取总个数</param>
        /// <param name="pageIndex">分页(不分页时 为0)</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectByCityName(MSSQLDBDAL db, string cityName, out int count, bool isGetCount = true, int pageIndex = 0, int pageSize = 1)
        {
            count = 0;
            SYSCityTable table = GetCityTableByCityName(db, cityName);
            if (table == null || string.IsNullOrEmpty(table.ProjectTable))
            {
                return new List<DATProject>();
            }
            string tableName = table.ProjectTable;
            IList<DATProject> list = GetProjectByTableNameAndCityId(db, tableName, table.CityId, out count, isGetCount: isGetCount, pageIndex: pageIndex, pageSize: pageSize);
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <param name="cityid"></param>
        /// <param name="count">返回总个数</param>
        /// <param name="isGetCount">是否获取总个数</param>
        /// <param name="pageIndex">分页(不分页时 为0)</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IList<DATProject> GetProjectByTableNameAndCityId(MSSQLDBDAL db, string tableName, int cityid, out int count, bool isGetCount = true, int pageIndex = 0, int pageSize = 0)
        {
            count = 0;
            if (string.IsNullOrEmpty(tableName))
            {
                return new List<DATProject>();
            }
            IList<DATProject> list = new List<DATProject>();
            string sql = string.Format("{0} Valid=1 and  CityId={1}", Utility.GetMSSQL_SQL(typeof(DATProject), tablename: tableName), cityid);
            if (pageIndex > 0)
            {
                UtilityPager page = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
                list = db.PagerList<DATProject>(page, sql).ToList();
                count = page.Count;
            }
            else
            {
                list = db.GetCustomSQLQueryList<DATProject>(sql).ToList();
            }

            return list;
        }

        /// <summary>
        /// 根据楼盘ID查询楼盘信息(根据机构权限和子表)(创建人:曾智磊,时间:2014.04.16)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static DATProject GetProjectByCompanyIdAndCityIdAndProjectId(MSSQLDBDAL db, int companyId, int cityId, int projectId)
        {

            SYSCityTable table = UtilityDALHelper.GetCityTable(db, cityId);
            if (table == null)
            {
                return null;
            }
            string projectTable = table.ProjectTable;
            string projectTableSub = table.ProjectTable + "_sub";
            string sql = string.Format(new StringBuilder()
                .Append(" {0} Valid=1 and  CityID={1} and ProjectId={2} and Projectid not in (select projectid ")
                                                  .Append("from {3} as ps with(nolock) ")
                                                  .Append("where ps.projectid={2} and ps.fxt_companyid={4} ")
                                                  .Append("and ps.cityid={1}) ").ToString(),
                                                  Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim(), keyword2: "with(nolock) "),
                          cityId.ToString(), projectId, projectTableSub, companyId);
            DATProject proj = db.GetCustomSQLQueryEntity<DATProject>(sql);
            if (proj != null)
            {
                return proj;
            }
            string sql2 = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and ProjectId={2} and Fxt_Companyid={3}").ToString(),
                Utility.GetMSSQL_SQL2(typeof(DATProjectsub), projectTableSub.Trim()),
                          cityId.ToString(), projectId, companyId);
            DATProjectsub proj2 = db.GetCustomSQLQueryEntity<DATProjectsub>(sql2);

            proj = ProjectsubConvertTo(proj2);
            return proj;
        }

        public static DATProject GetProjectByCompanyIdAndCityIdAndProjectName(MSSQLDBDAL db,string projectTable, int companyId, int cityId, string projectName)
        {
            //" in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid=@cityId and fxtcompanyid=@fxtcompanyId),',')) "
            string projectTableSub = projectTable + "_sub";
            string sql = string.Format(new StringBuilder()
                .Append(" {0} Valid=1 and  CityID={1} and ProjectName='{2}' and Projectid not in (select projectid ")
                                                  .Append("from {3} as ps with(nolock) ")
                                                  .Append("where ps.projectName='{2}' and ps.fxt_companyid in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid={1} and fxtcompanyid={4}),',')) ")
                                                  .Append("and ps.cityid={1}) ").ToString(),
                                                  Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim(),keyword:" top 1 ", keyword2: "with(nolock) "),
                          cityId.ToString(), projectName, projectTableSub, companyId);
            DATProject proj = db.GetCustomSQLQueryEntity<DATProject>(sql);
            if (proj == null)
            {
                return proj;
            }
            string sql2 = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and ProjectName='{2}' and Fxt_Companyid in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid={1} and fxtcompanyid={3}),',')) ").ToString(),
                Utility.GetMSSQL_SQL2(typeof(DATProjectsub), projectTableSub.Trim(), keyword: " top 1 "),
                          cityId.ToString(), projectName, companyId);
            DATProjectsub proj2 = db.GetCustomSQLQueryEntity<DATProjectsub>(sql2);

            proj = ProjectsubConvertTo(proj2);
            return proj;
        }
        public static DATProject GetProjectByCompanyIdAndCityIdAndOtherName(MSSQLDBDAL db, string projectTable, int companyId, int cityId, string otherName)
        {
            //" in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid=@cityId and fxtcompanyid=@fxtcompanyId),',')) "
            string projectTableSub = projectTable + "_sub";
            string sql = string.Format(new StringBuilder()
                .Append(" {0} Valid=1 and  CityID={1} and OtherName='{2}' and Projectid not in (select projectid ")
                                                  .Append("from {3} as ps with(nolock) ")
                                                  .Append("where ps.OtherName='{2}' and ps.fxt_companyid in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid={1} and fxtcompanyid={4}),',')) ")
                                                  .Append("and ps.cityid={1}) ").ToString(),
                                                  Utility.GetMSSQL_SQL(typeof(DATProject), projectTable.Trim(), keyword: " top 1 ", keyword2: "with(nolock) "),
                          cityId.ToString(), otherName, projectTableSub, companyId);
            DATProject proj = db.GetCustomSQLQueryEntity<DATProject>(sql);
            if (proj != null)
            {
                return proj;
            }
            string sql2 = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and OtherName='{2}' and Fxt_Companyid in (select value from  dbo.splittotable((select casecompanyid from dbo.privi_company_showdata with(nolock) where cityid={1} and fxtcompanyid={3}),',')) ").ToString(),
                Utility.GetMSSQL_SQL2(typeof(DATProjectsub), projectTableSub.Trim(), keyword: " top 1 "),
                          cityId.ToString(), otherName, companyId);
            DATProjectsub proj2 = db.GetCustomSQLQueryEntity<DATProjectsub>(sql2);

            proj = ProjectsubConvertTo(proj2);
            return proj;
        }
        #endregion

        #region (更新)

        public static bool InsertDATProject(MSSQLDBDAL mssqldbdal, string projectTable, DATProject project)
        {
            object obj = ProjectTableConvertToProjectModel(project, projectTable);
            object result = mssqldbdal.Create(obj);

            if (result.GetType() == typeof(int))
            {
                if (Convert.ToInt32(result) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


        #endregion

        #region 楼栋信息(DATBuilding)
        /// <summary>
        /// 根据楼栋ID查询楼栋信息(根据机构权限和子表)(创建人:曾智磊,时间:2014.04.17)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingByCompanyIdAndCityIdAndBuildingId(MSSQLDBDAL db, int companyId, int cityId, int buildingId)
        {

            SYSCityTable table = UtilityDALHelper.GetCityTable(db, cityId);
            if (table == null)
            {
                return null;
            }
            string buildingTable = table.BuildingTable;
            string buildingTableSub = table.BuildingTable + "_sub";
            string sql = string.Format(new StringBuilder()
                .Append(" {0} Valid=1 and  CityID={1} and BuildingId={2} and BuildingId not in (select BuildingId ")
                                                  .Append("from {3} as ps with(nolock) ")
                                                  .Append("where ps.BuildingId={2} and ps.fxt_companyid={4} ")
                                                  .Append("and ps.cityid={1}) ").ToString(),
                                                  Utility.GetMSSQL_SQL(typeof(DATBuilding), buildingTable.Trim(), keyword2: "with(nolock) "),
                          cityId.ToString(), buildingId, buildingTableSub, companyId);
            DATBuilding obj = db.GetCustomSQLQueryEntity<DATBuilding>(sql);
            if (obj != null)
            {
                return obj;
            }
            string sql2 = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and BuildingId={2} and Fxt_Companyid={3}").ToString(),
                Utility.GetMSSQL_SQL2(typeof(DATBuildingsub), buildingTableSub.Trim()),
                          cityId.ToString(), buildingId, companyId);
            DATBuildingsub obj2 = db.GetCustomSQLQueryEntity<DATBuildingsub>(sql2);

            obj = BuildingsubConvertTo(obj2);
            return obj;
        }
        #endregion



        #region DATHouse(房号信息)
        /// <summary>
        /// 根据房号ID查询房号信息(根据机构权限和子表)(创建人:曾智磊,时间:2014.04.17)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public static DATHouse GetHouseByCompanyIdAndCityIdAndHouseId(MSSQLDBDAL db, int companyId, int cityId, int houseId)
        {

            SYSCityTable table = UtilityDALHelper.GetCityTable(db, cityId);
            if (table == null)
            {
                return null;
            }
            string houseTable = table.HouseTable;
            string houseTableSub = table.HouseTable + "_sub";
            string sql = string.Format(new StringBuilder()
                .Append(" {0} Valid=1 and  CityID={1} and HouseId={2} and HouseId not in (select HouseId ")
                                                  .Append("from {3} as ps with(nolock) ")
                                                  .Append("where ps.HouseId={2} and ps.fxtcompanyid={4} ")
                                                  .Append("and ps.cityid={1}) ").ToString(),
                                                  Utility.GetMSSQL_SQL(typeof(DATHouse), houseTable.Trim(), keyword2: "with(nolock) "),
                          cityId.ToString(), houseId, houseTableSub, companyId);
            DATHouse obj = db.GetCustomSQLQueryEntity<DATHouse>(sql);
            if (obj != null)
            {
                return obj;
            }
            string sql2 = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and HouseId={2} and FxtCompanyid={3}").ToString(),
                Utility.GetMSSQL_SQL2(typeof(DATHousesub), houseTableSub.Trim()),
                          cityId.ToString(), houseId, companyId);
            DATHousesub obj2 = db.GetCustomSQLQueryEntity<DATHousesub>(sql2);

            obj = HousesubConvertTo(obj2);
            return obj;
        }
        #endregion

        #region 案例(DATCase)

        #region (查询)

        /// <summary>
        /// 根据楼盘、城市得到案例中的房号  李晓东
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cId">城市ID</param>
        /// <param name="pId">楼盘ID</param>
        /// <returns></returns>
        public static DATHouse GetHouseByHouse_City_Building(MSSQLADODAL db, string houseName, int cId, int bId)
        {
            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(db, cId);
            string sql = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and HouseName='{2}' and BuildingId={3}").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATHouse), cityTable.HouseTable),
                          cId, houseName, bId);
            return db.GetModel<DATHouse>(sql);
        }
        /// <summary>
        /// 根据楼盘、城市、楼宇名称(楼栋名称)得到楼宇(楼栋) 信息 李晓东
        /// </summary>
        /// <param name="pId">楼盘</param>
        /// <param name="cId">城市</param>
        /// <param name="bName">楼栋名称</param>
        /// <returns></returns>
        public static DATBuilding GetBuildingByProject_City_Build(MSSQLDBDAL db, int pId, int cId, string bName)
        {
            SYSCityTable cityTable = UtilityDALHelper.GetCityTable(db, cId);
            string sql = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and ProjectId={2} and BuildingName='{3}'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable),
                          cId, pId, bName);
            return db.GetCustomSQLQueryEntity<DATBuilding>(sql);
        }
        /// <summary>
        /// 根据楼盘、城市、楼宇名称(楼栋名称)得到楼宇(楼栋) 信息 李晓东 ADO
        /// </summary>
        /// <param name="pId">楼盘</param>
        /// <param name="cId">城市</param>
        /// <param name="bName">楼栋名称</param>
        /// <returns></returns>
        public static DATBuilding GetBuildingADOByProject_City_Build(MSSQLADODAL db, int pId, int cId, string bName)
        {
            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(db, cId);
            string sql = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and ProjectId={2} and BuildingName='{3}'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable),
                          cId, pId, bName);
            return db.GetModel<DATBuilding>(sql);
        }

        /// <summary>
        /// 根据城市ID and 多个案例ID获取案例信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        public static IList<DATCase> GetCaseByCityIdAndCaseIds(MSSQLDBDAL db, int cityId, int[] caseIds)
        {
            string caseIdsStr = caseIds.ConvertToString();
            if (string.IsNullOrEmpty(caseIdsStr))
            {
                return new List<DATCase>();
            }
            SYSCityTable cityTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (cityTable == null)
            {
                return new List<DATCase>();
            }
            string caseTable = cityTable.CaseTable;
            string sql = string.Format("{0} CaseID in ({1})", Utility.GetMSSQL_SQL(typeof(DATCase), tablename: caseTable)
                , caseIdsStr);
            IList<DATCase> list = db.GetCustomSQLQueryList<DATCase>(sql).ToList<DATCase>();
            return list;
        }

        public static DATCase GetCaseByCityIdAndCaseId(MSSQLDBDAL db, int cityId, int caseId)
        {
            SYSCityTable cityTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (cityTable == null)
            {
                return null;
            }

            string sql = string.Format("{0} CaseID  ={1}", Utility.GetMSSQL_SQL(typeof(DATCase), tablename: cityTable.CaseTable)
                , caseId);
            DATCase caseObj = db.GetCustomSQLQueryList<DATCase>(sql).FirstOrDefault();
            return caseObj;
        }
        /// <summary>
        /// 获取要修改的case对象
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public static object GetUpdateCaseByCityIdAndCaseId(MSSQLDBDAL db, int cityId, int caseId)
        {
            SYSCityTable cityTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (cityTable == null)
            {
                return null;
            }
            string sql = string.Format("{0} CaseID  ={1}", Utility.GetMSSQL_SQL(typeof(DATCase), tablename: cityTable.CaseTable)
                , caseId);
            object obj = null;
            switch (cityTable.CaseTable.Replace("dbo.", ""))
            {
                case Utility.DATCase:
                    obj = db.GetCustomSQLQueryList<DATCase>(sql).FirstOrDefault();
                    break;
                case Utility.DATCasecsj:
                    obj = db.GetCustomSQLQueryList<DATCasecsj>(sql).FirstOrDefault();
                    break;
                case Utility.DATCasehbh:
                    obj = db.GetCustomSQLQueryList<DATCasehbh>(sql).FirstOrDefault();
                    break;
                case Utility.DATCasexb:
                    obj = db.GetCustomSQLQueryList<DATCasexb>(sql).FirstOrDefault();
                    break;
                case Utility.DATCasezb:
                    obj = db.GetCustomSQLQueryList<DATCasezb>(sql).FirstOrDefault();
                    break;
                case Utility.DATCasezsj:
                    obj = db.GetCustomSQLQueryList<DATCasezsj>(sql).FirstOrDefault();
                    break;
                default:
                    break;
            }
            return obj;
        }
        public static IList<DATCase> GetCaseByCityNameAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDate(
            MSSQLDBDAL db, string cityName, int projectId, string fxtCompanyIds, int buildingTypeCode, int purposeCode, int? buildingAreaCode, DateTime startDate, DateTime endDate
            , out int count, bool isGetCount = true, int pageIndex = 0, int pageSize = 0)
        {
            count = 0;
            IList<DATCase> list = new List<DATCase>();
            SYSCity city = GetCityName(db, cityName);
            if (city == null)
            {
                return list;
            }
            list = GetCaseByCityIdAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDate(db, city.CityId, projectId, fxtCompanyIds, buildingTypeCode, purposeCode, buildingAreaCode, startDate, endDate, out count, isGetCount: isGetCount, pageIndex: pageIndex, pageSize: pageSize);
            return list;
        }
        public static IList<DATCase> GetCaseByCityIdAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDate(
            MSSQLDBDAL db, int cityId, int projectId, string fxtCompanyIds, int? buildingTypeCode, int purposeCode, int? buildingAreaCode, DateTime startDate, DateTime endDate
            , out int count, bool isGetCount = true, int pageIndex = 0, int pageSize = 0)
        {
            count = 0;
            IList<DATCase> list = new List<DATCase>();
            SYSCityTable table = GetCityTable(db, cityId);
            if (table == null)
            {
                return list;
            }
            string tableName = table.CaseTable;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(string.Format("{0} Valid=1 and CityId={1} and ProjectId={2} and FXTCompanyId in ({3}) and PurposeCode={4}",
                Utility.GetMSSQL_SQL(typeof(DATCase), tablename: tableName),
                cityId, projectId, fxtCompanyIds, purposeCode));
            if (buildingTypeCode != null)
            {
                sbSql.Append(" and BuildingTypeCode= ").Append(Convert.ToInt32(buildingTypeCode));
            }
            if (buildingAreaCode != null)
            {
                sbSql.Append(" and ").Append(BuildingAreaWhereSql(Convert.ToInt32(buildingAreaCode), "BuildingArea"));//.Append(" and BuildingArea>=").Append(minArea.ToString());
            }
            sbSql.Append(" and CaseDate>='").Append(startDate.ToString()).Append("' and CaseDate<='").Append(endDate.ToString()).Append("'");
            sbSql.Append(" order by CaseID desc");
            if (pageIndex > 0)
            {
                UtilityPager page = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
                list = db.PagerList<DATCase>(page, sbSql.ToString()).ToList();
                count = page.Count;
            }
            else
            {
                list = db.GetCustomSQLQueryList<DATCase>(sbSql.ToString()).ToList();
            }
            return list;
        }
        #endregion

        #region (更新)

        /// <summary>
        /// 根据城市+caseIds 删除案例
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        public static bool DeleteCaseByCityIdAndCaseIds(MSSQLDBDAL db, int cityId, int[] caseIds)
        {
            SYSCityTable table = GetCityTable(db, cityId);
            if (table == null)
            {
                return true;
            }
            return DeleteCaseByTableNameAndCaseIds(db, table.CaseTable, caseIds);
        }
        /// <summary>
        /// 根据城市+caseIds 删除案例
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityName"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        public static bool DeleteCaseByCityNameAndCaseIds(MSSQLDBDAL db, string cityName, int[] caseIds)
        {
            SYSCityTable table = GetCityTableByCityName(db, cityName);
            if (table == null)
            {
                return true;
            }
            return DeleteCaseByTableNameAndCaseIds(db, table.CaseTable, caseIds);
        }
        static bool DeleteCaseByTableNameAndCaseIds(MSSQLDBDAL db, string tableName, int[] caseIds)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return true;
            }
            if (caseIds == null || caseIds.Length < 1)
            {
                return true;
            }
            string sql = string.Format(" update {0} set Valid=0 where CaseID in ({1})", tableName, caseIds.ConvertToString());
            db.Update(sql);
            return true;
        }
        /// <summary>
        /// 根据projectId和指定偏高偏低范围单价+用途+案例时间区间 删除案例
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <param name="projectId"></param>
        /// <param name="purposeCode">用途</param>
        /// <param name="maxPrice"></param>
        /// <param name="minPrice"></param>
        /// <param name="startCaseDate"></param>
        /// <param name="endCaseDate"></param>
        /// <returns></returns>
        public static bool DeleteCaseByProjectIdAndPurposeCodeAndUnitPriceAndDate(MSSQLDBDAL db, string tableName, int projectId, int purposeCode, decimal maxPrice, decimal minPrice, DateTime startCaseDate, DateTime endCaseDate, ITransaction tran = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return true;
            }
            string sql = string.Format(" update {0} set Valid=0 where Valid=1 and FxtCompanyId=25 and ProjectId={1} and PurposeCode={2} and  CaseDate between '{3}' and '{4}' and (UnitPrice>{5} or UnitPrice<{6})",
                tableName, projectId, purposeCode, startCaseDate.ToString(), endCaseDate.ToString(), maxPrice, minPrice);
            db.Update(sql, transaction: tran);
            return true;
        }

        //public static bool UpdateCase(MSSQLDBDAL db, DATCase caseObj)
        //{
        //    if (caseObj == null)
        //    {
        //        return false;
        //    }
        //    int cityId = Convert.ToInt32(caseObj.CityID);
        //    SYSCityTable table = GetCityTable(db, cityId);
        //    if (table == null)
        //    {
        //        return false;
        //    }
        //    string tableName = table.CaseTable;

        //    return UpdateCaseByTableName(db, tableName, caseObj);
        //}
        static bool UpdateCaseByTableName(MSSQLDBDAL db, string tableName, DATCase caseObj)
        {
            if (string.IsNullOrEmpty(tableName) || caseObj == null)
            {
                return true;
            }
            object _case = CaseTableConvertToCaseModel(caseObj, tableName);

            db.Update(_case);
            return true;
        }
        public static bool InsertCase(MSSQLDBDAL db, DATCase caseObj)
        {
            if (caseObj == null)
            {
                return false;
            }
            int cityId = Convert.ToInt32(caseObj.CityID);
            SYSCityTable table = GetCityTable(db, cityId);
            if (table == null)
            {
                return false;
            }
            string tableName = table.CaseTable;
            return InsertCaseByTableName(db, tableName, caseObj);
        }
        static bool InsertCaseByTableName(MSSQLDBDAL db, string tableName, DATCase caseObj)
        {
            if (string.IsNullOrEmpty(tableName) || caseObj == null)
            {
                return false;
            }
            object _case = CaseTableConvertToCaseModel(caseObj, tableName);
            db.Create(_case);
            caseObj.CaseID = (_case as DATCase).CaseID;
            return true;
        }
        #endregion

        #endregion

        #region 楼盘网络名(SYSProjectMatch)
        /// <summary>
        /// 根据(房讯通建立的)网络名称 城市 获取楼盘
        /// </summary>
        /// <param name="mssqldbal"></param>
        /// <param name="netName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static IList<SYSProjectMatch> GetListSYSProjectMatch_Fxt(MSSQLDBDAL mssqldbdal, string netName, int cityId)
        {
            SYSCityTable table = GetCityTable(mssqldbdal, cityId);
            if (table == null)
            {
                return new List<SYSProjectMatch>();
            }
            string sql =
                   string.Format("{0} CityId={1} and FXTCompanyId={2} and  NetName='{3}' and ProjectNameId in (select ProjectId from {4} where Valid=1 and  CityId={1})",
                   Utility.GetMSSQL_SQL(typeof(SYSProjectMatch), Utility.SYSProjectMatch,keyword:" top 1 ", keyword2: "with(nolock) "),
                   cityId.ToString(),
                   25,
                   netName,
                   table.ProjectTable.Trim());
            IList<SYSProjectMatch> list = mssqldbdal.GetCustomSQLQueryList<SYSProjectMatch>(sql).ToList();
            return list;
        }
        /// <summary>
        /// 根据(房讯通建立的)网络名称 城市 行政区 获取楼盘
        /// </summary>
        /// <param name="mssqldbal"></param>
        /// <param name="netName"></param>
        /// <param name="cityId"></param>
        /// <param name="projectTableName"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static IList<SYSProjectMatch> GetListSYSProjectMatch_Fxt(MSSQLDBDAL mssqldbal, string netName, int cityId, string projectTableName, int areaId)
        {
            string hsql =
                   string.Format("{0} CityId={1} and FXTCompanyId={2} and NetName='{3}' and ProjectNameId in (select ProjectId from {4} where Valid=1 and  CityId={1} and AreaId={5})",
                   Utility.GetMSSQL_HSQL(typeof(SYSProjectMatch), Utility.SYSProjectMatch),
                   cityId.ToString(),
                   25,
                   netName,
                   projectTableName.Trim(),
                   areaId);
            IList<SYSProjectMatch> list = mssqldbal.HQueryPagerList<SYSProjectMatch>(hsql).ToList<SYSProjectMatch>();
            return list;
        }
        /// <summary>
        /// 根据(房讯通建立的)网络名称 城市 获取楼盘
        /// </summary>
        /// <param name="mssqldbal"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="netName"></param>
        /// <returns></returns>
        public static IList<SYSProjectMatch> GetListSYSProjectMatchByProjectIdAndCityIdAndNetName_Fxt(MSSQLDBDAL mssqldbal, int projectId, int cityId, string netName)
        {
            SYSCityTable table = GetCityTable(mssqldbal, cityId);
            if (table == null)
            {
                return new List<SYSProjectMatch>();
            }
            string sql =
                   string.Format("{0} ProjectNameId={1} and CityId={2} and FXTCompanyId={3} and NetName='{4}' and ProjectNameId in (select ProjectId from {5} where Valid=1 and  CityId={2})",
                   Utility.GetMSSQL_SQL(typeof(SYSProjectMatch), Utility.SYSProjectMatch),
                   projectId,
                   cityId.ToString(),
                   25,
                   netName,
                   table.ProjectTable.Trim());

            IList<SYSProjectMatch> list = mssqldbal.GetCustomSQLQueryList<SYSProjectMatch>(sql).ToList();
            return list.ToList<SYSProjectMatch>();
        }
        /// <summary>
        /// 根据(房讯通建立的)网络名称 获取同一城市的其他网络关联
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityId"></param>
        /// <param name="netName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public static IList<SYSProjectMatch> GetListSYSProjectMatchByCityIdAndNetName_Fxt(MSSQLDBDAL db, int cityId, string netName, int pageIndex, int pageLength)
        {
            SYSCityTable table = GetCityTable(db, cityId);
            if (table == null)
            {
                return new List<SYSProjectMatch>();
            }
            string sql =
                   string.Format("{0} CityId={1} and FXTCompanyId={2} and NetName='{3}' and ProjectNameId in (select ProjectId from {4} where Valid=1 and  CityId={1})",
                   Utility.GetMSSQL_SQL(typeof(SYSProjectMatch), Utility.SYSProjectMatch),
                   cityId.ToString(),
                   25,
                   netName,
                   table.ProjectTable.Trim());


            IList<SYSProjectMatch> list = db.PagerList<SYSProjectMatch>(new UtilityPager(pageSize: pageLength, pageIndex: pageIndex), sql).ToList();

            return list.ToList<SYSProjectMatch>();
        }

        #endregion

        #region 交叉值均价(DATProjectAvgPrice)
        /// <summary>
        /// 设置楼盘指定月份均价
        /// </summary>
        /// <param name="db"></param>
        /// <param name="avgPrice"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="avgPriceDate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        static bool SetProjectOneMothsAvgPrice(MSSQLDBDAL db, decimal avgPrice, int projectId, int cityId, int purposeCode, string avgPriceDate, ITransaction transaction = null)
        {
            DATProjectAvgPrice avgProject = db.GetCustom<DATProjectAvgPrice>((Expression<Func<DATProjectAvgPrice, bool>>)
                (tbl => tbl.ProjectId == projectId
                    && tbl.CityId == cityId
                    && tbl.FxtCompanyId == 25
                    && tbl.DateRange == 1
                    && tbl.BuildingAreaType == null
                    && tbl.BuildingTypeCode == null
                    && tbl.PurposeType == purposeCode
                    && tbl.AvgPriceDate == avgPriceDate
            ));
            if (avgProject != null)
            {
                avgProject.AvgPrice = (int)decimal.Round(avgPrice, 0);
                avgProject.CreateTime = DateTime.Now;
                db.Update(avgProject, transaction);
            }
            else
            {
                avgProject = new DATProjectAvgPrice();
                avgProject.ProjectId = projectId;
                avgProject.CityId = cityId;
                avgProject.FxtCompanyId = 25;
                avgProject.DateRange = 1;
                avgProject.BuildingAreaType = null;
                avgProject.BuildingTypeCode = null;
                avgProject.PurposeType = purposeCode;
                avgProject.AvgPriceDate = avgPriceDate;
                avgProject.AvgPrice = (int)decimal.Round(avgPrice, 0);
                avgProject.CreateTime = DateTime.Now;
                avgProject.JSFS = "根据上一个月总楼盘案例量计算,公式:(总价格/总数量)";
                db.Create(avgProject, transaction);
            }
            return true;
        }
        /// <summary>
        /// 删除楼盘指定月份单价过高和过低的案例
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityTable"></param>
        /// <param name="projectId"></param>
        /// <param name="purposeCode">当前用途(1002001普通住宅or1002027别墅)</param>
        /// <param name="date">指定要计算的月份日期(例如201203)</param>
        /// <returns></returns>
        public static bool DeleteMaxOrMinPriceCase(MSSQLDBDAL db, SYSCityTable cityTable, int projectId, int[] purposeCodes, string date)
        {
            if (cityTable == null)
            {
                return false;
            }
            string avgPriceDate = Utility.GetDateTimeMoths(date, 2, "yyyyMM");
            bool result = true;
            string sql = "";

            sql = string.Format("{0} FxtCompanyId=25 and CityId={1} and ProjectId={2} and DateRange=1 " +
                                " and PurposeType in ({3}) and BuildingAreaType is null and BuildingTypeCode is null  and  AvgPriceDate='{4}'", Utility.GetMSSQL_SQL(typeof(DATProjectAvgPrice), Utility.DATProjectAvgPrice),
                                cityTable.CityId, projectId, purposeCodes.ConvertToString(), Utility.GetDateTimeMoths(date, 1, "yyyyMM"));
            IList<DATProjectAvgPrice> avgProject = db.GetCustomSQLQueryList<DATProjectAvgPrice>(sql).ToList();
            //已经删除过 则不删除
            if (avgProject != null && avgProject.Count >= purposeCodes.Length)
            {
                return true;
            }
            using (ITransaction tx = db.BeginTransaction())
            {
                try
                {
                    foreach (int code in purposeCodes)
                    {
                        if (avgProject != null)
                        {
                            DATProjectAvgPrice avgObj = avgProject.Where(p => p.PurposeType == code).FirstOrDefault();
                            if (avgObj != null)
                            {
                                continue;
                            }
                        }
                        string startDate = Utility.GetDateTimeMoths(date, 1, "yyyy-MM-01");
                        string endDate = Utility.GetDateTimeMoths(date, 1, "yyyy-MM-dd 23:59:59");
                        string sql2 = string.Format("{0} projectId={1} and Valid=1 and FxtCompanyId=25 and PurposeCode={2} and UnitPrice>0 and CaseDate between '{3}'and '{4}'",
                            Utility.GetMSSQL_SQL_AVG("UnitPrice", cityTable.CaseTable),
                            projectId, code, startDate, endDate);//"2010-10-01"
                        object obj = db.GetCustomSQLQueryUniqueResult<object>(sql2); ;// db.GetCustomSQLQueryEntity<string>(sql);
                        decimal avgPrice = 0;
                        if (obj != null)
                        {
                            avgPrice = Convert.ToDecimal(obj);
                        }
                        decimal maxRatio = 0.25M;
                        decimal maxPrice = Convert.ToDecimal(avgPrice + (avgPrice * maxRatio));
                        decimal minPrice = Convert.ToDecimal(avgPrice - (avgPrice * maxRatio));
                        UtilityDALHelper.DeleteCaseByProjectIdAndPurposeCodeAndUnitPriceAndDate(db, cityTable.CaseTable, projectId, code, maxPrice, minPrice, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), tran: tx);
                        SetProjectOneMothsAvgPrice(db, avgPrice, projectId, cityTable.CityId, code, avgPriceDate, tx);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    result = false;
                }
            }
            return result;
        }

        #endregion

        #region SYSCode
        /// <summary>
        /// 获取别墅类型居住用途code
        /// </summary>
        /// <returns></returns>
        public static int[] GetVillaPurposeCodes()
        {
            int[] ints = new int[] { 1002005, 1002006, 1002007, 1002008, 1002027 };
            return ints;
        }

        #region 1001(土地用途)
        /// <summary>
        /// 获取土地用途(用于建楼盘)
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SYSCode GetProjectPurposeCodeByCode(MSSQLDBDAL mssqldbdal, int code)
        {
            var vsysArea = mssqldbdal.GetCustom<SYSCode>(
                       (Expression<Func<SYSCode, bool>>)
                       (obj => obj.ID == Utility.CodeID_1 && obj.Code == code));
            return vsysArea;
        }
        /// <summary>
        /// 获取土地用途(用于建楼盘)
        /// </summary>
        /// <param name="mssqldbdal"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IList<SYSCode> GetAllProjectPurposeCodeByCode(MSSQLADODAL mssqlado)
        {
            string sql = string.Format("{0} ID={1}",
                          Utility.GetMSSQL_SQL(typeof(SYSCode), Utility.SYSCode), Utility.CodeID_1);

            //var vsysArea = mssqldbdal.GetListCustom<SYSCode>(
            //           (Expression<Func<SYSCode, bool>>)
            //           (obj => obj.ID == Utility.CodeID_1));
            return mssqlado.GetList<SYSCode>(sql);
        }

        #endregion


        /// <summary>
        /// 获得集合SYSCODE
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="Id">编号ID</param>
        /// <returns></returns>
        public static IList<SYSCode> GetListSYSCODE(MSSQLDBDAL mssqldbdal, int Id)
        {
            var vsysArea = mssqldbdal.GetListCustom<SYSCode>(
                    (Expression<Func<SYSCode, bool>>)
                    (_syscode => _syscode.ID == Id));
            return vsysArea.ToList<SYSCode>();
        }

        /// <summary>
        /// 获得集合SYSCODE ADO
        /// </summary>
        /// <param name="mssqldbdal">MSSQLDBDAL类对象</param>
        /// <param name="Id">编号ID</param>
        /// <returns></returns>
        public static IList<SYSCode> GetADOListSYSCODE(MSSQLADODAL mssqlado, int Id)
        {
            string sql = string.Format("{0} ID={1}",
                          Utility.GetMSSQL_SQL(typeof(SYSCode), Utility.SYSCode),Id);
            var vsysArea = mssqlado.GetList<SYSCode>(sql);
            return vsysArea;
        }

        
        /// <summary>
        /// 根据ID(类型)和code查询
        /// </summary>
        /// <param name="db"></param>
        /// <param name="code"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static SYSCode GetSYSCodeByCodeAndID(MSSQLDBDAL db, int code, int Id)
        {
            var vsysArea = db.GetListCustom<SYSCode>(
                    (Expression<Func<SYSCode, bool>>)
                    (tbl => tbl.Code == code && tbl.ID == Id)).FirstOrDefault();
            return vsysArea;
        }

        public static SYSCode GetSYSCodeByCode(MSSQLDBDAL db, int code)
        {
            var vsysArea = db.GetListCustom<SYSCode>(
                    (Expression<Func<SYSCode, bool>>)
                    (tbl => tbl.Code == code)).FirstOrDefault();
            return vsysArea;
        }

        public static SYSCode GetADOSYSCodeByCode(MSSQLADODAL db, int code)
        {
            string sql = string.Format("{0} Code={1}",
                          Utility.GetMSSQL_SQL(typeof(SYSCode), Utility.SYSCode), code);
            return db.GetModel<SYSCode>(sql);
        }
        /// <summary>
        /// 获取别墅相关的住宅用途
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IList<SYSCode> GetPurposeTypeCodeVillaType(MSSQLDBDAL db)
        {
            var sysCode = UtilityDALHelper.GetListSYSCODE(db, Utility.CodeID_2);//(别墅)户型
            var sysCode2 = sysCode.Where(tbl => tbl.CodeName.IndexOf("别墅") >= 0).ToList();
            return sysCode2;
        }

        #endregion

        #region LNKPAppendage(配套)
        /// <summary>
        /// 根据楼盘ID和城市获取楼盘配套信息(创建人:曾智磊,时间:2014.04.16)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static IList<LNKPAppendage> GetLNKPAppendageByProjectIdAndCityId(MSSQLDBDAL db, int projectId, int cityId)
        {

            IList<LNKPAppendage> lnkpaList = db.GetListCustom<LNKPAppendage>(
               (Expression<Func<LNKPAppendage, bool>>)
               (tbl =>
                   tbl.ProjectId == projectId && tbl.CityId == cityId
               )).ToList<LNKPAppendage>();
            return lnkpaList;
        }
        #endregion

        #region LNK_P_Company(楼盘关联公司)
        /// <summary>
        /// 根据楼盘ID和城市获取楼盘关联公司(创建人:曾智磊,时间:2014.04.16)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static IList<LNKPCompany> GetLNKPCompanyByProjectIdAndCityId(MSSQLDBDAL db, int projectId, int cityId)
        {

            IList<LNKPCompany> list = db.GetListCustom<LNKPCompany>(
               (Expression<Func<LNKPCompany, bool>>)
               (tbl =>
                   tbl.LNKPCompanyPX.ProjectId == projectId && tbl.LNKPCompanyPX.CityId == cityId
               )).ToList<LNKPCompany>();
            return list;

        }

        #endregion

        #region Privi_Company(公司信息)
        /// <summary>
        /// 根据多个公司ID获取公司信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        public static IList<PriviCompany> GetPriviCompanyByCompanyIds(MSSQLDBDAL db, int[] companyIds)
        {
            if (companyIds == null || companyIds.Length < 1)
            {
                return new List<PriviCompany>();
            }
            IList<PriviCompany> list = db.CreateCriteria(typeof(PriviCompany)).Add(
           Restrictions.In("CompanyId", companyIds)
           ).List<PriviCompany>();
            return list;
        }
        #endregion

        #region (DAT_Company)

        /// <summary>
        /// 根据多个公司ID获取公司信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        public static IList<DATCompany> GetDATCompanyByCompanyIds(MSSQLDBDAL db, int[] companyIds)
        {
            if (companyIds == null || companyIds.Length < 1)
            {
                return new List<DATCompany>();
            }
            IList<DATCompany> list = db.CreateCriteria(typeof(DATCompany)).Add(
           Restrictions.In("CompanyId", companyIds)
           ).List<DATCompany>();
            return list;
        }
        #endregion

        #region LNK_P_Photo(照片信息)
        /// <summary>
        /// 获取楼盘照片个数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static int GetLNKPPhotoCount(MSSQLDBDAL db, int projectId, int cityId, int companyId)
        {

            string sql = string.Format(new StringBuilder()
                .Append(" {0} Valid=1 and  CityID={1} and ProjectId={2} and Projectid not in (select projectid ")
                                                  .Append("from {3} as ps with(nolock) ")
                                                  .Append("where ps.ProjectId={2} and ps.FxtCompanyId={4} ")
                                                  .Append("and ps.cityid={1}) ").ToString(),
                                                  Utility.GetMSSQL_SQL_COUNT(Utility.LNKPPhoto.Trim()),
                          cityId.ToString(), projectId, Utility.LNKPPhotosub, companyId);
            int phCount = Convert.ToInt32(db.GetCustomSQLQueryUniqueResult<object>(sql));
            string sql2 = string.Format(new StringBuilder()
                .Append("{0} Valid=1 and  CityID={1} and ProjectId={2} and FxtCompanyid={3}").ToString(),
                          Utility.GetMSSQL_SQL_COUNT(Utility.LNKPPhotosub.Trim()),
                          cityId.ToString(), projectId, companyId);
            int phCount2 = Convert.ToInt32(db.GetCustomSQLQueryUniqueResult<object>(sql2));

            return phCount + phCount2;
        }

        #endregion

        #region (Privi_Company_ShowData)
        public static PriviCompanyShowData GetPriviCompanyShowDataByCompanyIdAndCityId(MSSQLDBDAL db, int companyId, int cityId)
        {
            PriviCompanyShowData showData = db.GetCustom<PriviCompanyShowData>(
                (Expression<Func<PriviCompanyShowData, bool>>)
                (tbl => tbl.FxtCompanyId == 25 && tbl.CityId == cityId));
            return showData;
        }
        #endregion

        #region (DATAvgPriceDay)
        public static DATAvgPriceDay GetDATAvgPriceDayByProjectIdAndCityIdAndBuildingAreaType(MSSQLDBDAL db, int projectId, int cityId, int buildingAreaTypeCode)
        {
            string sql = "{0} ProjectId={1} and CityId={2} and BuildingAreaType={3} Order By AvgPriceDate desc";
            sql = string.Format(sql,
                Utility.GetMSSQL_SQL(typeof(DATAvgPriceDay), Utility.DATAvgPriceDay, keyword: " top 1 ", keyword2: "with(nolock) "),
                projectId, cityId, buildingAreaTypeCode);
            DATAvgPriceDay obj = db.GetCustomSQLQueryEntity<DATAvgPriceDay>(sql);
            return obj;
        }
        #endregion

        #region common

        /// <summary>
        /// 根据表名转换实体
        /// </summary>
        /// <param name="proObj"></param>
        /// <param name="projectTable"></param>
        /// <returns></returns>
        private static object ProjectTableConvertToProjectModel(DATProject proObj, string projectTable)
        {
            object obj = null;
            switch (projectTable.Replace("dbo.", ""))
            {
                case Utility.DATProject:
                    DATProject proj1 = new DATProject();
                    CopyModelData(proObj, proj1);
                    obj = proj1;
                    break;
                case Utility.DATProjectcsj:
                    DATProjectcsj proj2 = new DATProjectcsj();
                    CopyModelData(proObj, proj2);
                    obj = proj2;
                    break;
                case Utility.DATProjecthbh:
                    DATProjecthbh proj3 = new DATProjecthbh();
                    CopyModelData(proObj, proj3);
                    obj = proj3;
                    break;
                case Utility.DATProjectxb:
                    DATProjectxb proj4 = new DATProjectxb();
                    CopyModelData(proObj, proj4);
                    obj = proj4;
                    break;
                case Utility.DATProjectzb:
                    DATProjectzb proj5 = new DATProjectzb();
                    CopyModelData(proObj, proj5);
                    obj = proj5;
                    break;
                case Utility.DATProjectzsj:
                    DATProjectzsj proj6 = new DATProjectzsj();
                    CopyModelData(proObj, proj6);
                    obj = proj6;
                    break;
                default:
                    break;
            }
            return obj;

        }

        /// <summary>
        /// 根据表名转换实体
        /// </summary>
        /// <param name="caseObj"></param>
        /// <param name="caseTable"></param>
        /// <returns></returns>
        private static object CaseTableConvertToCaseModel(DATCase caseObj, string caseTable)
        {
            object obj = null;
            switch (caseTable.Replace("dbo.", ""))
            {
                case Utility.DATCase:
                    DATCase case1 = new DATCase();
                    CopyModelData(caseObj, case1);
                    obj = case1;
                    break;
                case Utility.DATCasecsj:
                    DATCasecsj case2 = new DATCasecsj();
                    CopyModelData(caseObj, case2);
                    obj = case2;
                    break;
                case Utility.DATCasehbh:
                    DATCasehbh case3 = new DATCasehbh();
                    CopyModelData(caseObj, case3);
                    obj = case3;
                    break;
                case Utility.DATCasexb:
                    DATCasexb case4 = new DATCasexb();
                    CopyModelData(caseObj, case4);
                    obj = case4;
                    break;
                case Utility.DATCasezb:
                    DATCasezb case5 = new DATCasezb();
                    CopyModelData(caseObj, case5);
                    obj = case5;
                    break;
                case Utility.DATCasezsj:
                    DATCasezsj case6 = new DATCasezsj();
                    CopyModelData(caseObj, case6);
                    obj = case6;
                    break;
                default:
                    break;
            }
            return obj;

        }

        /// <summary>
        /// copy楼盘表实体值
        /// </summary>
        /// <param name="obj1">被copy实体</param>
        /// <param name="obj2">存储copy值的实体</param>
        private static void CopyModelData(object obj1, object obj2)
        {
            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();
            PropertyInfo[] propertys1 = type1.GetProperties();
            PropertyInfo[] propertys2 = type2.GetProperties();
            for (int i = 0; i < propertys1.Length; i++)
            {
                PropertyInfo property1 = propertys1[i];
                PropertyInfo property2 = propertys2[i];
                object propertyValue = property1.GetValue(obj1, null);
                if (propertyValue != null)
                {
                    property2.SetValue(obj2, propertyValue, null);
                }

            }
        }

        /// <summary>
        /// 生成面积段查询条件
        /// </summary>
        /// <param name="buildingTypeCode"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string BuildingAreaWhereSql(int buildingTypeCode, string columnName)
        {
            string whereSql = columnName + " {0}";
            switch (buildingTypeCode)
            {
                case 8006001:
                    whereSql = string.Format(whereSql, "<30");
                    break;
                case 8006002:
                    whereSql = string.Format("{0} >={1} and {0}< {2}", columnName, "30", "60");
                    break;
                case 8006003:
                    whereSql = string.Format("{0} >={1} and {0}< {2}", columnName, "60", "90");
                    break;
                case 8006004:
                    whereSql = string.Format("{0} >={1} and {0}<= {2}", columnName, "90", "120");
                    break;
                case 8006005:
                    whereSql = string.Format(whereSql, " >120");
                    break;
                default:
                    whereSql = string.Format(whereSql, " >0");
                    break;

            }
            return whereSql;
        }
        /// <summary>
        /// 将实体类DATProjectsub转换成DATProject
        /// </summary>
        /// <param name="projSub"></param>
        /// <returns></returns>
        public static DATProject ProjectsubConvertTo(DATProjectsub projSub)
        {
            if (projSub == null)
            {
                return null;
            }
            DATProject proj = new DATProject();
            Type type1 = projSub.GetType();
            Type type2 = proj.GetType();
            PropertyInfo[] propertys1 = type1.GetProperties();
            for (int i = 0; i < propertys1.Length; i++)
            {
                PropertyInfo property1 = propertys1[i];
                PropertyInfo property2 = type2.GetProperty(property1.Name); //propertys2[i];
                if (property2 != null)
                {
                    object propertyValue = property1.GetValue(projSub, null);
                    if (propertyValue != null)
                    {
                        property2.SetValue(proj, propertyValue, null);
                    }
                }

            }
            proj.FxtCompanyId = projSub.LNKPCompanyPX.Fxt_CompanyId;
            proj.ProjectId = projSub.LNKPCompanyPX.ProjectId;
            return proj;
        }
        /// <summary>
        /// 将实体类DATBuildingsub转换成DATBuilding
        /// </summary>
        /// <param name="objSub"></param>
        /// <returns></returns>
        public static DATBuilding BuildingsubConvertTo(DATBuildingsub objSub)
        {
            if (objSub == null)
            {
                return null;
            }
            DATBuilding obj = new DATBuilding();
            Type type1 = objSub.GetType();
            Type type2 = obj.GetType();
            PropertyInfo[] propertys1 = type1.GetProperties();
            for (int i = 0; i < propertys1.Length; i++)
            {
                PropertyInfo property1 = propertys1[i];
                PropertyInfo property2 = type2.GetProperty(property1.Name); //propertys2[i];
                if (property2 != null)
                {
                    object propertyValue = property1.GetValue(objSub, null);
                    if (propertyValue != null)
                    {
                        property2.SetValue(obj, propertyValue, null);
                    }
                }

            }
            obj.FxtCompanyId = objSub.LNKBCompanyPX.Fxt_CompanyId;
            obj.BuildingId = objSub.LNKBCompanyPX.BuildingId;
            return obj;
        }

        /// <summary>
        /// 将实体类DATHousesub转换成DATHouse
        /// </summary>
        /// <param name="objSub"></param>
        /// <returns></returns>
        public static DATHouse HousesubConvertTo(DATHousesub objSub)
        {
            if (objSub == null)
            {
                return null;
            }
            DATHouse obj = new DATHouse();
            Type type1 = objSub.GetType();
            Type type2 = obj.GetType();
            PropertyInfo[] propertys1 = type1.GetProperties();
            for (int i = 0; i < propertys1.Length; i++)
            {
                PropertyInfo property1 = propertys1[i];
                PropertyInfo property2 = type2.GetProperty(property1.Name); //propertys2[i];
                if (property2 != null)
                {
                    object propertyValue = property1.GetValue(objSub, null);
                    if (propertyValue != null)
                    {
                        property2.SetValue(obj, propertyValue, null);
                    }
                }

            }
            obj.FxtCompanyId = objSub.LNKHCompanyPX.FxtCompanyId;
            obj.HouseId = objSub.LNKHCompanyPX.HouseId;
            return obj;
        }
        #endregion
    }
}
