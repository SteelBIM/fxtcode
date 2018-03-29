using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtService.Contract.APIInterface;
using FxtNHibernater.Data;
using System.Linq.Expressions;
using FxtService.Common;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using FxtNHibernate.DTODomain.DATProjectDTO;
using FxtNHibernate.DATProjectDomain.Entities;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtCommonLibrary.LibraryUtils;
using System.ServiceModel;
using log4net;

/**
 * 作者: 李晓东
 * 时间: 2013.12.03
 * 摘要: 新建Wcf (具体化)Actualize FxtAPI(实现接口IFxtAPI)
 *       2013.12.18 针对 Cross 的完善处理并把参数 string crossType更换成 int codeType 修改人:李晓东
 *       2013.12.19 新实现了接口中的 GetProvince省份 GetCity城市 GetArea行政区 方法 修改人:李晓东
 *       2013.12.20 修改GetCity城市 GetArea行政区方法 修改人:李晓东
 *       2013.12.20 新增GetAreaByCityName方法(根据城市名称获取行政区) 修改人:曾智磊
 *       2013.12.30 新增GetProjectNameLikeByCityNameAndPrjName,InsertProject,InsertSYSProjectMatch,InsertSYSProjectMatchList,
 *       GetAllProjectPurposeCode,GetCityByCityName,GetProvinceById方法(服务),GetCaseByCityIdAndCaseIds(服务),
 *       GetProjectByCityIdAndProjectIds(服务),GetCaseIdJoinProjectNameByCityIdAndCaseIds(服务),修改人:曾智磊
 *       2013.12.30 新增CheckSYSProjectMatch方法(内部方法) 修改人:曾智磊
 *       2013.12.30 新增实体FxtApi_Result类 修改人:曾智磊
 *       2014.01.23 修改人:李晓东
 *                  新增:GetCityListByCityName 满足检索城市是否有多个相同
 *                       GetProvinceByName 根据省名称查找省编号
 *                       GetAreaListByAraeName 根据区域名称得到相关所有区域
 *       2014.02.07 修改人:李晓东
 *                  新增GetProvinceByCityId 根据城市联动拿到省份
 *                      GetCityByAreaId  根据行政区联动拿到城市
 *       2014.02.13 修改人:李晓东
 *                  新增:GetProjectByCityIDAndLikePrjName
 *                        GetProjectJoinPMatchByPNameOrPAddressCityId
 *       2014.02.20 修改人:李晓东
 *                  新增:Entrance API入口
 *       2014.02.20 修改人:曾智磊
 *                  新增DeleteCaseByCityIdAndCaseIds()方法
 *                  新增:GetHouseByHouse_City_Building 得到房号
 *                       GetBuildingByProject_City_Build 得到楼栋
 *       2014.02.26 修改人:李晓东
 *                  新增:GetCityByCityId,GetAreaByAreaId
 *                       GetBuildingByProjectIdCityIDAndLikeBuildingName 检索楼栋
 *       2014.02.27 修改人:李晓东
 *                  新增:GetHouseByBuildingIdCityIDAndLikeHouseName 检索房号
 *       2014.03.07 修改人:曾智磊
 *                  修改Entrance方法的逻辑
 *       2014.03.14 修改人:曾智磊
 *                  新增GetPriviCompanyShowDataByCompanyIdAndCityId 得到可查询数据的company
 *       2014.03.27 修改人:曾智磊
 *                  修改Cross()计算起止时间为21号-20号
 *                  新增方法CrossProjectByCodeType()获取楼盘均价
 *       2014.04.02 修改人:李晓东
 *                  修改:1.GetProjectJoinPMatchByPNameOrPAddressCityId修改对临时库的支持
 *                       2.GetBuildingByProject_City_Build 修改对临时库的支持
 *                       3.GetHouseByHouse_City_Building 修改对临时库的支持
 *                      
 *       2014.05.27 修改人:李晓东
 *                  修改:GetBuildingByProject_City_Build改成ADO类型
 *       2014.06.25 修改人:李晓东
 *                  修改:GetProjectByCityIDAndLikePrjName、GetHouseByBuildingIdCityIDAndLikeHouseName、GetBuildingByProjectIdCityIDAndLikeBuildingName改成ADO类型
 * **/
namespace FxtService.Service.APIActualize
{
    [FxtService.Service.ServiceBehavior]
    public class FxtAPI : IFxtAPI
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(FxtAPI));

        #region 楼盘交叉值运算
        /// <summary>
        /// 交叉信息,根据机构fxtcompanyId(数据结构默认25)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="codeType">用途:(普通住宅code,别墅code)</param>
        /// <param name="date"></param>
        /// <param name="fxtCompanyId">要以哪个机构的数据</param>
        /// <returns></returns>
        public string CrossByFxtCompanyId(int projectId, int cityId, int codeType, string date,int fxtCompanyId)        
        {
            MSSQLDBDAL _mssqlDBDAL = new MSSQLDBDAL();
            try
            {
                IList<DATProjectAvgPrice> list1 = UtilityDALHelper.GetProjectCrossPrice(_mssqlDBDAL, projectId, cityId, codeType, date, true, fxtCompanyId);
                _mssqlDBDAL.Close();
                return JsonConvert.SerializeObject(list1);
            }
            catch (Exception ex)
            {
                _mssqlDBDAL.Close();
                log.Error(string.Format("Cross(int projectId={0}, int cityId={1}, int codeType={2}, string date={3},int fxtCompanyId={4})", projectId, cityId, codeType, date == null ? "null" : date,fxtCompanyId), ex);
                return Utility.GetJson(0, "系统异常", ex.Message);
            }
        }
        /// <summary>
        /// 交叉信息(数据结构默认25)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="codeType">用途:(普通住宅code,别墅code)</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string Cross(int projectId, int cityId, int codeType, string date)
        {
            MSSQLDBDAL _mssqlDBDAL = new MSSQLDBDAL();
            try
            {
                IList<DATProjectAvgPrice> list1 = UtilityDALHelper.GetProjectCrossPrice(_mssqlDBDAL, projectId, cityId, codeType, date, true,25);
                _mssqlDBDAL.Close();
                return JsonConvert.SerializeObject(list1);
                string startDate = string.IsNullOrEmpty(date) ?
                    Utility.GetDateTimeMoths(null, -3, "yyyy-MM-21") :
                    Utility.GetDateTimeMoths(date, -3, "yyyy-MM-21");
                //Utility.GetDateTimeMoths(null, -2, "yyyy-MM-01") :
                //Utility.GetDateTimeMoths(date, -2, "yyyy-MM-01");

                string endDate = string.IsNullOrEmpty(date) ?
                    Utility.GetDateTimeMoths() :
                    Utility.GetDateTimeMoths(date, 0, "yyyy-MM-20 23:59:59"); //Utility.GetDateTimeMoths(date, 0, "yyyy-MM-dd 23:59:59");
                IList<DATProjectAvgPrice> ExistsAvgPrice = new List<DATProjectAvgPrice>();
                string avgPricSql = "";
                IList<SYSCode> purposeTypeVillaList = new List<SYSCode>();//别墅类型用途
                int[] purposeCodes = new[] { 0 };
                IList<SYSCode> vListBuildingArea = new List<SYSCode>();//面积
                IList<SYSCode> buildingTypeList = new List<SYSCode>();//建筑类型
                int[] buildingTypeCodes = new[] { 0 };
                if (codeType.Equals(1002001))//普通住宅
                {
                    //面积分类
                    vListBuildingArea = _mssqlDBDAL.GetListCustom<SYSCode>(
                    (Expression<Func<SYSCode, bool>>)(syscode => syscode.ID == 8006))
                    .ToList<SYSCode>();
                    //建筑类型
                    buildingTypeList = _mssqlDBDAL.GetListCustom<SYSCode>(
                       (Expression<Func<SYSCode, bool>>)(syscode => syscode.ID == 2003))
                       .ToList<SYSCode>();
                    List<int> intList = new List<int>();
                    foreach (var item in buildingTypeList)
                    {
                        intList.Add(item.Code);
                    }
                    purposeCodes = new[] { codeType };
                    buildingTypeCodes = intList.ToArray();
                    //查询均价
                    avgPricSql = string.Format("{0} FxtCompanyId=25 and CityId={1} and ProjectId={2} and DateRange=3 " +
                                        " and PurposeType={3} and BuildingTypeCode in ({4}) and  AvgPriceDate='{5}'", Utility.GetMSSQL_SQL(typeof(DATProjectAvgPrice), Utility.DATProjectAvgPrice),
                                        cityId, projectId, codeType, buildingTypeCodes.ConvertToString(), Utility.GetDateTimeMoths(date, 1, "yyyyMM"));

                }
                else if (codeType.Equals(1002027))//别墅
                {
                    //别墅类型用途
                    purposeTypeVillaList = UtilityDALHelper.GetPurposeTypeCodeVillaType(_mssqlDBDAL);
                    List<int> intList = new List<int>();
                    foreach (var item in purposeTypeVillaList)
                    {
                        intList.Add(item.Code);
                    }
                    purposeCodes = intList.ToArray();
                    //查询均价

                    avgPricSql = string.Format("{0} FxtCompanyId=25 and CityId={1} and ProjectId={2} and DateRange=3 " +
                                        " and PurposeType in ({3})  and  AvgPriceDate='{4}'", Utility.GetMSSQL_SQL(typeof(DATProjectAvgPrice), Utility.DATProjectAvgPrice),
                                        cityId, projectId, purposeCodes.ConvertToString(), Utility.GetDateTimeMoths(date, 1, "yyyyMM"));
                    ExistsAvgPrice = new List<DATProjectAvgPrice>();
                }
                else
                {
                    _mssqlDBDAL.Close();
                    return JsonConvert.SerializeObject(ExistsAvgPrice);
                }
                ExistsAvgPrice = _mssqlDBDAL.GetCustomSQLQueryList<DATProjectAvgPrice>(avgPricSql).ToList();
                //检查指定月份是否已有均价,1.已有直接返回,2.无则计算
                if (ExistsAvgPrice != null && ExistsAvgPrice.Count() > 0)
                {
                    _mssqlDBDAL.Close();
                    return JsonConvert.SerializeObject(ExistsAvgPrice);
                }
                //获得城市
                var vCaseTable = UtilityDALHelper.GetCityTable(_mssqlDBDAL, cityId);
                if (vCaseTable == null)
                {
                    _mssqlDBDAL.Close();
                    return JsonConvert.SerializeObject(ExistsAvgPrice);
                }
                //属于哪个楼盘
                var vProject = _mssqlDBDAL.GetSQLCustom<DATProject>(string.Format("{0} ProjectId={1}",
                    Utility.GetMSSQL_SQL(typeof(DATProject), vCaseTable.ProjectTable), projectId));
                if (vProject == null)
                {
                    _mssqlDBDAL.Close();
                    return JsonConvert.SerializeObject(ExistsAvgPrice);
                }
                //删除期间价格过高过低的案例
                DeleteMothsLengthMaxOrMinPriceCase(_mssqlDBDAL, vCaseTable, projectId, purposeCodes, date, 3);
                //面积段
                var vSysCode = UtilityDALHelper.GetListSYSCODE(_mssqlDBDAL, 8006);

                List<DATProjectAvgPrice> _listObject = new List<DATProjectAvgPrice>();
                IList<SYSCode> vListSysCode = null;
                if (vCaseTable != null)
                {
                    PriviCompanyShowData showData = _mssqlDBDAL.GetCustom<PriviCompanyShowData>(
                        (Expression<Func<PriviCompanyShowData, bool>>)
                        (tbl => tbl.FxtCompanyId == 25 && tbl.CityId == cityId));
                    string dataCompanyIds = "25";
                    if (showData != null && !string.IsNullOrEmpty(showData.CaseCompanyId))
                    {
                        dataCompanyIds = showData.CaseCompanyId;
                    }
                    //某个案例中所属楼盘中的所有案例信息
                    string sqlFormat = "{0} Valid=1 and FxtCompanyId in (" + dataCompanyIds + ") and ProjectId={1}{2} and CaseDate between '{3}' and '{4}'";
                    string hsqlFormat = "{0} Valid=1 and FxtCompanyId in (" + dataCompanyIds + ") and ProjectId={1}  and CaseDate between '{2}' and '{3}'",
                        hsqlCross = string.Empty;
                    string hprojectsql = string.Format(hsqlFormat, Utility.GetMSSQL_HSQL(typeof(DATCase), vCaseTable.CaseTable),
                        projectId, startDate, endDate);
                    sqlFormat = string.Format(sqlFormat, "{0}", projectId, "{1}", startDate, endDate);
                    vListSysCode = new List<SYSCode>();//层次段

                    StringBuilder codeSB = new StringBuilder();

                    var codeTypeExists = _mssqlDBDAL.GetCustom<SYSCode>(
                        (Expression<Func<SYSCode, bool>>)
                        (syscode => syscode.Code == codeType));//判断类型是否有
                    if (codeTypeExists == null)
                    {
                        _mssqlDBDAL.Close();
                        return JsonConvert.SerializeObject(ExistsAvgPrice);
                    }
                    else if (codeType.Equals(1002001))//普通住宅
                    {
                        vListSysCode = buildingTypeList;//建筑类型
                        hsqlCross = string.Format("and PurposeCode={0} and BuildingTypeCode in ({1})",
                            codeType, buildingTypeCodes.ConvertToString());
                    }
                    else if (codeType.Equals(1002027))//别墅
                    {
                        vListSysCode = purposeTypeVillaList; //UtilityDALHelper.GetListSYSCODE(_mssqlDBDAL, Utility.CodeID_2);//(别墅)户型

                        hsqlCross = string.Format(" and PurposeCode in ({0})",
                            purposeCodes.ConvertToString());

                    }
                    hprojectsql = string.Format("{0} {1}", hprojectsql, hsqlCross);
                    UtilityPager utilityPager = new UtilityPager(100);

                    //int pagerCount = 0, pageIndex = 1;

                    //while (pagerCount >= pageIndex || pagerCount == 0)//一页一页检索信息
                    //{
                    //    utilityPager.PageIndex = pageIndex;

                    //    if (pageIndex > 1)
                    //    {
                    //        utilityPager.IsGetCount = false;
                    //    }
                    //    var datProject = _mssqlDBDAL.HQueryPagerList<DATCase>(utilityPager, hprojectsql).ToList();
                    //    if (pagerCount == 0)
                    //    {
                    //        pagerCount = (utilityPager.Count + utilityPager.PageSize - 1) / utilityPager.PageSize;
                    //        if (pagerCount == 0) pagerCount = pageIndex;//准备跳出
                    //    }
                    //    if (datProject != null)//数据是否为空,否则退出 && datProject.Count() > 0
                    //    {
                    //        #region 计算
                    //        //计算当前相应均价
                    //        PageCross(datProject, _listObject, codeType, vListSysCode, vListBuildingArea);          

                    //        #endregion
                    //    }
                    //    else break;
                    //    pageIndex++;
                    //}
                    SqlComputeAvgPrice(_mssqlDBDAL, sqlFormat, vCaseTable, _listObject, codeType, vListSysCode, vListBuildingArea);
                    //将当月计算后的均价信息进行筛选插入数据库
                    log.Debug(string.Format("执行方法---SetCross(_mssqlDBDAL, _listObject={0}, cityId={1}, projectId={2}, vProject={3}, date={4}, true)"
                        , _listObject != null ? _listObject.Count.ToString() : "null", cityId, projectId, vProject == null ? "null" : vProject.ProjectName, date == null ? "null" : date)
                        );
                    SetCross(_mssqlDBDAL, _listObject, cityId, projectId, vProject, date, true);
                    #region (注释内容)
                    //foreach (var dAvgPrice in _listObject)
                    //{
                    //    //当某类型层次的所有案例信息不足3条时按上月同类型层次来计算本月均价
                    //    if (dAvgPrice.Id < 3)
                    //    {

                    //        var lessThanThree = _mssqlDBDAL
                    //            .GetListCustom<DATProjectAvgPrice>(
                    //            (Expression<Func<DATProjectAvgPrice, bool>>)
                    //            (avgPrice =>
                    //                avgPrice.AvgPrice != 0 &&
                    //                avgPrice.CityId == cityId &&
                    //                avgPrice.ProjectId == projectId &&
                    //                avgPrice.AvgPriceDate ==
                    //                Utility.GetDateTimeMoths(date, 0, "yyyyMM"))//上月均价的月份
                    //             );//得到上个月的均价信息

                    //        if (lessThanThree != null && lessThanThree.Count() > 0)//上个月中的值已存储
                    //        {
                    //            //上月对应层次类型信息
                    //            var avgPrice = lessThanThree.Where(litem =>
                    //                 litem.PurposeType.Equals(dAvgPrice.PurposeType) &&
                    //                 litem.BuildingTypeCode.Equals(dAvgPrice.BuildingTypeCode) &&
                    //                 litem.BuildingAreaType.Equals(dAvgPrice.BuildingAreaType))
                    //                 .FirstOrDefault();
                    //            Func<DATProjectAvgPrice, bool> fAvgPrice = litem =>
                    //                 !litem.PurposeType.Equals(dAvgPrice.PurposeType) &&
                    //                 !litem.BuildingTypeCode.Equals(dAvgPrice.BuildingTypeCode) &&
                    //                 !litem.BuildingAreaType.Equals(dAvgPrice.BuildingAreaType);
                    //            //上月总合
                    //            var previousMonths = lessThanThree.Where(fAvgPrice);

                    //            //当月总合
                    //            var currentMonths = _listObject.Where(fAvgPrice);

                    //            //当月
                    //            decimal currentSum = currentMonths.Sum(citem =>
                    //                citem.AvgPrice);
                    //            decimal currentCount = currentMonths.Count();
                    //            decimal currentDivide = 0;
                    //            if (currentCount != 0)//当月总量不能为零
                    //                currentDivide = decimal.Divide(currentSum, currentCount);
                    //            //上月
                    //            decimal previousSum = previousMonths.Sum(pitem =>
                    //                pitem.AvgPrice);
                    //            decimal previousCount = previousMonths.Count();
                    //            decimal previousDivide = 0;
                    //            if (previousCount != 0)//上月总量不能为零
                    //                previousDivide = decimal.Divide(previousSum, previousCount);

                    //            decimal cpDivide = 0;
                    //            if (previousDivide != 0)//当月除以上月,上月所计算出的值不能为零
                    //                cpDivide = decimal.Divide(currentDivide, previousDivide) - 1;

                    //            dAvgPrice.AvgPrice =
                    //                avgPrice.AvgPrice * (int)Decimal.Round(cpDivide, 0);
                    //            dAvgPrice.JSFS =
                    //                string.Format("根据当月<3条案例计算,公式:{0},涨跌幅:({1}/{2})-1",
                    //                "(上月同层次类型*涨跌幅)",
                    //                "(当月其他层次类型已存在均价/当月其他层次类型已存在均价总合)",
                    //                "(上月其他层次类型已存在均价/上月其他层次类型已存在均价总合)");
                    //        }
                    //        else
                    //        {
                    //            dAvgPrice.JSFS = "<3条案例,第一次计算,无公式";
                    //        }

                    //    }
                    //    else
                    //    {
                    //        decimal avgPrice = 0;
                    //        if (dAvgPrice.Id > 0)
                    //            avgPrice = Decimal.Round(decimal.Divide(dAvgPrice.AvgPrice, dAvgPrice.Id), 0);
                    //        dAvgPrice.AvgPrice = (int)avgPrice;
                    //        dAvgPrice.JSFS = "根据当月>=3条案例计算,公式:(总单价/总数量)";
                    //    }
                    //    dAvgPrice.CityId = cityId;
                    //    dAvgPrice.FxtCompanyId = 25;
                    //    dAvgPrice.AreaId = vArea == null ? 0 : vArea.AreaID;
                    //    dAvgPrice.SubAreaId = vArea == null ? 0 : vArea.SubAreaId.Value;
                    //    dAvgPrice.ProjectId = projectId;
                    //    dAvgPrice.AvgPriceDate = Utility.GetDateTimeMoths(date, 1, "yyyyMM");
                    //    dAvgPrice.CreateTime = DateTime.Now;
                    //    _mssqlDBDAL.Create(dAvgPrice);
                    //}
                    #endregion
                }
                _mssqlDBDAL.Close();
                return JsonConvert.SerializeObject(_listObject);
            }
            catch (Exception ex)
            {
                _mssqlDBDAL.Close();
                log.Error(string.Format("Cross(int projectId={0}, int cityId={1}, int codeType={2}, string date={3})", projectId, cityId, codeType, date == null ? "null" : date), ex);
                return Utility.GetJson(0, "系统异常", ex.Message);
            }
        }
        /// <summary>
        /// 重置交叉值计算(根据条件更新,注意:此方法用于原交叉值已经存在的情况下)(up)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="purposeTypeCode"></param>
        /// <param name="buildingTypeCode">如果purposeTypeCode为别墅相关类型时则为0</param>
        /// <param name="buildingAreaType">如果purposeTypeCode为别墅相关类型时则为0</param>
        /// <param name="date"></param>
        /// <returns>DATProjectAvgPrice 结构json字符串</returns>
        public string ResetCrossBy(int projectId, int cityId, int purposeTypeCode, int buildingTypeCode, int buildingAreaType, string date)
        {

            string avgPriceDate = Utility.GetDateTimeMoths(date, 1, "yyyyMM");
            string startDate = string.IsNullOrEmpty(date) ?
                    Utility.GetDateTimeMoths(null, -3, "yyyy-MM-21") :
                    Utility.GetDateTimeMoths(date, -3, "yyyy-MM-21");
            //Utility.GetDateTimeMoths(null, -2, "yyyy-MM-01") :
            //Utility.GetDateTimeMoths(date, -2, "yyyy-MM-01");

            string endDate = string.IsNullOrEmpty(date) ?
                Utility.GetDateTimeMoths() :
                Utility.GetDateTimeMoths(date, 0, "yyyy-MM-20 23:59:59"); //Utility.GetDateTimeMoths(date, 0, "yyyy-MM-dd 23:59:59");

            MSSQLDBDAL _mssqlDBDAL = new MSSQLDBDAL();
            int codeType = 0;
            Expression<Func<DATProjectAvgPrice, bool>> fAvgPrice = null;
            if (purposeTypeCode.Equals(1002001))//普通住宅
            {
                fAvgPrice = avgPrice => avgPrice.FxtCompanyId == 25 && avgPrice.CityId == cityId &&
                                avgPrice.ProjectId == projectId && avgPrice.DateRange == 3 && avgPrice.PurposeType == purposeTypeCode &&
                                avgPrice.BuildingTypeCode == buildingTypeCode && avgPrice.BuildingAreaType == buildingAreaType &&
                                avgPrice.AvgPriceDate == avgPriceDate;
            }
            else
            {
                fAvgPrice = avgPrice => avgPrice.FxtCompanyId == 25 && avgPrice.CityId == cityId &&
                                               avgPrice.ProjectId == projectId && avgPrice.DateRange == 3 && avgPrice.PurposeType == purposeTypeCode &&
                                               avgPrice.AvgPriceDate == avgPriceDate;
            }
            var ExistsAvgPrice = _mssqlDBDAL.GetCustom<DATProjectAvgPrice>(
                    (Expression<Func<DATProjectAvgPrice, bool>>)
                            (fAvgPrice)
                    );
            if (ExistsAvgPrice == null)
            {
                ExistsAvgPrice = new DATProjectAvgPrice().EncodeField();
                _mssqlDBDAL.Close();
                return JsonConvert.SerializeObject(ExistsAvgPrice);
            }
            IList<SYSCode> vListSysCode = new List<SYSCode>();//建筑类型list(用于普通住宅) or 用途list(用于别墅)
            IList<SYSCode> buildingAreaTypeList = new List<SYSCode>();
            List<DATProjectAvgPrice> _listObject = new List<DATProjectAvgPrice>();
            ExistsAvgPrice.AvgPrice = 0;
            _listObject.Add(ExistsAvgPrice);
            //获得城市
            var vCaseTable = UtilityDALHelper.GetCityTable(_mssqlDBDAL, cityId);
            var vProject = _mssqlDBDAL.GetSQLCustom<DATProject>(string.Format("{0} ProjectId={1}",
                Utility.GetMSSQL_SQL(typeof(DATProject), vCaseTable.ProjectTable), projectId));
            if (vCaseTable != null)
            {
                PriviCompanyShowData showData = _mssqlDBDAL.GetCustom<PriviCompanyShowData>(
                    (Expression<Func<PriviCompanyShowData, bool>>)
                    (tbl => tbl.FxtCompanyId == 25 && tbl.CityId == cityId));
                string dataCompanyIds = "25";
                if (showData != null && !string.IsNullOrEmpty(showData.CaseCompanyId))
                {
                    dataCompanyIds = showData.CaseCompanyId;
                }
                //某个案例中所属楼盘中的所有案例信息
                string sqlFormat = "{0} Valid=1 and FxtCompanyId in (" + dataCompanyIds + ") and ProjectId={1} {2} and CaseDate between '{3}' and '{4}'";
                string hsqlFormat = "{0} Valid=1 and FxtCompanyId in (" + dataCompanyIds + ") and ProjectId={1}  and CaseDate between '{2}' and '{3}'",
                    hsqlCross = string.Empty;
                string hprojectsql = string.Format(hsqlFormat, Utility.GetMSSQL_HSQL(typeof(DATCase), vCaseTable.CaseTable),
                    projectId, "{0}", startDate, endDate);
                sqlFormat = string.Format(sqlFormat, "{0}", projectId, "{1}", startDate, endDate);
                StringBuilder codeSB = new StringBuilder();

                var codeTypeExists = _mssqlDBDAL.GetCustom<SYSCode>(
                    (Expression<Func<SYSCode, bool>>)
                    (syscode => syscode.Code == purposeTypeCode));//判断类型是否有
                if (codeTypeExists == null)
                {
                    _mssqlDBDAL.Close();
                    return JsonConvert.SerializeObject(ExistsAvgPrice);
                }
                else if (purposeTypeCode.Equals(1002001))//普通住宅
                {
                    codeType = 1002001;
                    var code1 = UtilityDALHelper.GetSYSCodeByCode(_mssqlDBDAL, buildingTypeCode);
                    var code2 = UtilityDALHelper.GetSYSCodeByCode(_mssqlDBDAL, buildingAreaType);
                    vListSysCode.Add(code1); //设置为建筑类型
                    buildingAreaTypeList.Add(code2);
                    hsqlCross = string.Format("and PurposeCode={0} and BuildingTypeCode ={1} and {2}",
                        purposeTypeCode, buildingTypeCode, UtilityDALHelper.BuildingAreaWhereSql(buildingAreaType, "BuildingArea"));
                }
                else//别墅类型
                {
                    codeType = 1002027;
                    var code1 = UtilityDALHelper.GetSYSCodeByCode(_mssqlDBDAL, purposeTypeCode);
                    vListSysCode.Add(code1);//设置为用途
                    hsqlCross = string.Format(" and PurposeCode={0}", purposeTypeCode);

                }
                hprojectsql = string.Format(hprojectsql, hsqlCross);
                //UtilityPager utilityPager = new UtilityPager(100);
                //int pagerCount = 0, pageIndex = 1;
                //while (pagerCount >= pageIndex || pagerCount == 0)//一页一页检索信息
                //{
                //    utilityPager.PageIndex = pageIndex;
                //    if (pageIndex > 1)
                //    {
                //        utilityPager.IsGetCount = false;
                //    }
                //    var datProject = _mssqlDBDAL.HQueryPagerList<DATCase>(utilityPager, hprojectsql).ToList();
                //    if (pagerCount == 0)
                //    {
                //        pagerCount = (utilityPager.Count + utilityPager.PageSize - 1) / utilityPager.PageSize;
                //        if (pagerCount == 0) pagerCount = pageIndex;//准备跳出
                //    }
                //    if (datProject != null)//数据是否为空,否则退出 && datProject.Count() > 0
                //    {
                //        #region 计算
                //        //计算当前相应均价
                //        PageCross(datProject, _listObject, codeType, vListSysCode, buildingAreaTypeList);

                //        #endregion
                //    }
                //    else break;
                //    pageIndex++;
                //}

                SqlComputeAvgPrice(_mssqlDBDAL, sqlFormat, vCaseTable, _listObject, codeType, vListSysCode, buildingAreaTypeList);
                //将当月计算后的均价信息进行筛选插入数据库
                SetCross(_mssqlDBDAL, _listObject, cityId, projectId, vProject, date, true, isInsert: 0);
            }
            _mssqlDBDAL.Close();
            return JsonConvert.SerializeObject(_listObject[0]);

        }
        /// <summary>
        /// 获取指定楼盘and用途均价（数据机构默认25）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="codeType"></param>
        /// <param name="date">{type:1,message:"",data:均价,count:0}</param>
        /// <returns></returns>
        public string CrossProjectByCodeType(int projectId, int cityId, int codeType, string date)
        {
            //string json = Cross(projectId, cityId, codeType, date);
            //List<DATProjectAvgPrice> list = JsonConvert.DeserializeObject<List<DATProjectAvgPrice>>(json);
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATProjectAvgPrice> list = UtilityDALHelper.GetProjectCrossPrice(db, projectId, cityId, codeType, date, false,25);
            db.Close();
            int sumPrice2 = list.Sum(tbl => tbl.AvgPrice);
            int sumCount2 = list.Where(tbl => tbl.AvgPrice > 0).Count();
            int villaAvgPrice = sumCount2 == 0 ? 0 : sumPrice2 / sumCount2;
            return Utility.GetJson(1, "", data: villaAvgPrice);
        }

        /// <summary>
        /// 将当月计算好的均价筛选插入数据库
        /// </summary>
        /// <param name="db"></param>
        /// <param name="_listObject">当前要插入的计算值</param>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="project"></param>
        /// <param name="date"></param>
        /// <param name="isSql">true:sql直接算出的均价,false:程序逻辑计算出的均价</param>
        /// <param name="isInsert">1:插入,0;为修改</param>
        void SetCross(MSSQLDBDAL db, List<DATProjectAvgPrice> _listObject, int cityId, int projectId, DATProject project, string date, bool isSql, int isInsert = 1)
        {
            log.Debug(string.Format("进入方法---SetCross(_mssqlDBDAL, _listObject={0}, cityId={1}, projectId={2}, vProject={3}, date={4}, true)"
                          , _listObject != null ? _listObject.Count.ToString() : "null", cityId, projectId, project == null ? "null" : project.ProjectName, date == null ? "null" : date)
                          );
            IList<DATProjectAvgPrice> lessThanThree = new List<DATProjectAvgPrice>();
            foreach (var dAvgPrice in _listObject)
            {
                //update_rock
                if (!StringHelp.IsInteger(dAvgPrice.JSFS))
                {
                    dAvgPrice.JSFS = "0";
                }
                int avgCount = Convert.ToInt32(dAvgPrice.JSFS);
                //当某类型层次的所有案例信息不足3条时按上月同类型层次来计算本月均价
                //if (dAvgPrice.Id < 3)update_rock
                if (avgCount < 3)
                {
                    if (lessThanThree == null || lessThanThree.Count < 1)
                    {
                        lessThanThree = db
                           .GetListCustom<DATProjectAvgPrice>(
                           (Expression<Func<DATProjectAvgPrice, bool>>)
                           (avgPrice =>
                               avgPrice.FxtCompanyId == 25 &&
                               avgPrice.DateRange == 3 &&
                               avgPrice.CityId == cityId &&
                               avgPrice.ProjectId == projectId &&
                               avgPrice.AvgPriceDate ==
                               Utility.GetDateTimeMoths(date, 0, "yyyyMM"))//上月均价的月份
                            );//得到上个月的均价信息
                    }
                    if (lessThanThree != null && lessThanThree.Count() > 0)//上个月中的值已存储
                    {
                        //上月对应层次类型信息
                        var avgPrice = lessThanThree.Where(litem =>
                             litem.PurposeType.Equals(dAvgPrice.PurposeType) &&
                             litem.BuildingTypeCode.Equals(dAvgPrice.BuildingTypeCode) &&
                             litem.BuildingAreaType.Equals(dAvgPrice.BuildingAreaType))
                             .FirstOrDefault();
                        Func<DATProjectAvgPrice, bool> fAvgPrice = litem =>
                             !litem.PurposeType.Equals(dAvgPrice.PurposeType) &&
                             !litem.BuildingTypeCode.Equals(dAvgPrice.BuildingTypeCode) &&
                             !litem.BuildingAreaType.Equals(dAvgPrice.BuildingAreaType);
                        //上月总合
                        var previousMonths = lessThanThree.Where(fAvgPrice);

                        //当月总合
                        var currentMonths = _listObject.Where(fAvgPrice);

                        //当月
                        decimal currentSum = currentMonths.Sum(citem =>
                            citem.AvgPrice);
                        decimal currentCount = currentMonths.Count();
                        decimal currentDivide = 0;
                        if (currentCount != 0)//当月总量不能为零
                            currentDivide = Decimal.Divide(currentSum, currentCount);
                        //上月
                        decimal previousSum = previousMonths.Sum(pitem =>
                            pitem.AvgPrice);
                        decimal previousCount = previousMonths.Count();
                        decimal previousDivide = 0;
                        if (previousCount != 0)//上月总量不能为零
                            previousDivide = Decimal.Divide(previousSum, previousCount);

                        decimal cpDivide = 0;
                        if (previousDivide != 0)//当月除以上月,上月所计算出的值不能为零
                            cpDivide = decimal.Divide(currentDivide, previousDivide) - 1;
                        int intval = (int)Decimal.Round(cpDivide, 0);
                        dAvgPrice.AvgPrice = avgPrice.AvgPrice * intval;
                        dAvgPrice.JSFS =
                            string.Format("根据当月<3条案例计算,公式:{0},涨跌幅:({1}/{2})-1",
                            "(上月同层次类型*涨跌幅)",
                            "(当月其他层次类型已存在均价/当月其他层次类型已存在均价总合)",
                            "(上月其他层次类型已存在均价/上月其他层次类型已存在均价总合)");
                    }
                    else
                    {
                        dAvgPrice.JSFS = "<3条案例,第一次计算,无公式";
                    }

                }
                else
                {
                    decimal avgPrice = 0;
                    //总个数大于0并且有
                    if (avgCount > 0)//if (dAvgPrice.Id > 0)update_rock
                    {
                        avgPrice = dAvgPrice.AvgPrice;
                        //程序逻辑计算出来的不为sql计算值
                        if (!isSql)
                        {
                            avgPrice = Decimal.Round(Decimal.Divide(dAvgPrice.AvgPrice, avgCount), 0);
                        }
                    }
                    dAvgPrice.AvgPrice = (int)avgPrice;
                    dAvgPrice.JSFS = "根据当月>=3条案例计算,公式:(总单价/总数量)";
                }
                dAvgPrice.CityId = cityId;
                dAvgPrice.FxtCompanyId = 25;
                dAvgPrice.AreaId = project == null ? 0 : project.AreaID;
                dAvgPrice.SubAreaId = project == null ? 0 : Convert.ToInt32(project.SubAreaId);
                dAvgPrice.ProjectId = projectId;
                dAvgPrice.AvgPriceDate = Utility.GetDateTimeMoths(date, 1, "yyyyMM");
                dAvgPrice.CreateTime = DateTime.Now;
                dAvgPrice.DateRange = 3;
                if (isInsert == 1)
                {
                    db.Create(dAvgPrice);
                }
                else
                {
                    db.Update(dAvgPrice);
                }
            }
        }
        /// <summary>
        /// 计算当前页的案例均价
        /// </summary>
        /// <param name="nowPageCaseList">当前案例列表</param>
        /// <param name="nowAvgPriceList">用于存储均价</param>
        /// <param name="codeType">当前用途(1002001普通住宅or1002027别墅)</param>
        /// <param name="vListSysCode">codeType=1002001时:建筑类型list,codeType=1002027时:别墅类用途List</param>
        /// <param name="vListBuildingArea">codeType=1002001时:面积段list,codeType=1002027时:null</param>
        void PageCross(IList<DATCase> nowPageCaseList, List<DATProjectAvgPrice> nowAvgPriceList, int codeType, IList<SYSCode> vListSysCode, IList<SYSCode> vListBuildingArea)
        {
            if (codeType.Equals(1002001))//普通住宅
            {
                foreach (var item in vListSysCode)//建筑类型
                {
                    DATProjectAvgPrice datProjectAvgPrice = null;
                    foreach (var buildingArea in vListBuildingArea)//面积段
                    {
                        datProjectAvgPrice = new DATProjectAvgPrice();
                        datProjectAvgPrice.PurposeType = codeType;
                        datProjectAvgPrice.BuildingTypeCode = item.Code;
                        datProjectAvgPrice.BuildingAreaType = buildingArea.Code;

                        var Residence = nowPageCaseList.Where(ditem =>
                            ditem.BuildingTypeCode.Equals(item.Code) &&
                            BuildingArea(buildingArea.Code,
                            ditem.BuildingArea.Value)).ToList();//普通住宅
                        SetPriceAndCount(nowAvgPriceList, Residence, datProjectAvgPrice);
                    }
                }
            }
            else if (codeType.Equals(1002027))//别墅类型
            {
                foreach (var item in vListSysCode)//用途
                {
                    DATProjectAvgPrice datProjectAvgPrice = new DATProjectAvgPrice();
                    datProjectAvgPrice.PurposeType = item.Code;

                    var Residence = nowPageCaseList.Where(ditem =>
                        ditem.PurposeCode.Equals(item.Code));//别墅
                    SetPriceAndCount(nowAvgPriceList, Residence, datProjectAvgPrice);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sqlFormat">主查询条件SQL</param>
        /// <param name="cityTable"></param>
        /// <param name="nowAvgPriceList"></param>
        /// <param name="codeType"></param>
        /// <param name="vListSysCode">codeType=普通住宅时为建筑类型Code集合,codeType=别墅时为别墅用途Code集合</param>
        /// <param name="vListBuildingArea">面积段,codeType=别墅时为null</param>
        void SqlComputeAvgPrice(MSSQLDBDAL db, string sqlFormat, SYSCityTable cityTable, List<DATProjectAvgPrice> nowAvgPriceList, int codeType, IList<SYSCode> vListSysCode, IList<SYSCode> vListBuildingArea)
        {
            //string dataCompanyIds = "";
            //string hsqlFormat = "{0} Valid=1 and FxtCompanyId in (" + dataCompanyIds + ") and ProjectId={1} and CaseDate between '{2}' and '{3}'";

            //string sql2 = string.Format("{0} projectId={1} and Valid=1 and FxtCompanyId=25 and PurposeCode={2} and UnitPrice>0 and CaseDate between '{3}'and '{4}'",
            //               Utility.GetMSSQL_SQL_AVG("UnitPrice", cityTable.CaseTable),
            //               projectId, code, startDate, endDate);//"2010-10-01"
            //object obj = db.GetCustomSQLQueryUniqueResult<object>(sql2);


            if (codeType.Equals(1002001))//普通住宅
            {
                foreach (var item in vListSysCode)//建筑类型
                {
                    DATProjectAvgPrice datProjectAvgPrice = null;
                    foreach (var buildingArea in vListBuildingArea)//面积段
                    {
                        StringBuilder sbSql = new StringBuilder()
                         .Append(" and PurposeCode=").Append(1002001)
                         .Append(" and BuildingTypeCode=").Append(item.Code)
                         .Append(" and ").Append(UtilityDALHelper.BuildingAreaWhereSql(Convert.ToInt32(buildingArea.Code), "BuildingArea"));
                        string sql = string.Format(sqlFormat, Utility.GetMSSQL_SQL_AVG("UnitPrice", cityTable.CaseTable), sbSql.ToString());
                        object obj = db.GetCustomSQLQueryUniqueResult<object>(sql);
                        int avgPrice = 0;
                        if (obj != null)
                        {
                            avgPrice = Convert.ToInt32(obj);
                        }
                        sql = string.Format(sqlFormat, Utility.GetMSSQL_SQL_COUNT(cityTable.CaseTable), sbSql.ToString());
                        object obj2 = db.GetCustomSQLQueryUniqueResult<object>(sql);
                        int count = Convert.ToInt32(obj2);
                        datProjectAvgPrice = new DATProjectAvgPrice();
                        datProjectAvgPrice.PurposeType = codeType;
                        datProjectAvgPrice.BuildingTypeCode = item.Code;
                        datProjectAvgPrice.BuildingAreaType = buildingArea.Code;
                        datProjectAvgPrice.AvgPrice = avgPrice;
                        datProjectAvgPrice.JSFS = count.ToString();
                        AddList(nowAvgPriceList, datProjectAvgPrice);
                    }
                }
            }
            else if (codeType.Equals(1002027))//别墅类型
            {
                foreach (var item in vListSysCode)//用途
                {
                    DATProjectAvgPrice datProjectAvgPrice = new DATProjectAvgPrice();
                    datProjectAvgPrice.PurposeType = item.Code;

                    StringBuilder sbSql = new StringBuilder()
                     .Append(" and PurposeCode=").Append(item.Code);
                    string sql = string.Format(sqlFormat, Utility.GetMSSQL_SQL_AVG("UnitPrice", cityTable.CaseTable), sbSql.ToString());
                    object obj = db.GetCustomSQLQueryUniqueResult<object>(sql);
                    int avgPrice = 0;
                    if (obj != null)
                    {
                        avgPrice = Convert.ToInt32(obj);
                    }
                    sql = string.Format(sqlFormat, Utility.GetMSSQL_SQL_COUNT(cityTable.CaseTable), sbSql.ToString());
                    object obj2 = db.GetCustomSQLQueryUniqueResult<object>(sql);
                    int count = Convert.ToInt32(obj2);
                    datProjectAvgPrice.AvgPrice = avgPrice;
                    datProjectAvgPrice.JSFS = count.ToString();
                    AddList(nowAvgPriceList, datProjectAvgPrice);

                    //var Residence = nowPageCaseList.Where(ditem =>
                    //    ditem.PurposeCode.Equals(item.Code));//别墅
                    //SetPriceAndCount(nowAvgPriceList, Residence, datProjectAvgPrice);
                }
            }
        }

        /// <summary>
        /// 面积段判断
        /// </summary>
        /// <param name="code">面积段Code</param>
        /// <param name="buildingArea">面积值</param>
        /// <returns></returns>
        bool BuildingArea(int buildingTypeCode, decimal buildingArea)
        {
            bool flag = false;
            switch (buildingTypeCode)
            {
                case 8006001:
                    if (buildingArea < 30)
                        flag = true;
                    break;
                case 8006002:
                    if (buildingArea >= 30 && buildingArea < 60)
                        flag = true;
                    break;
                case 8006003:
                    if (buildingArea >= 60 && buildingArea < 90)
                        flag = true;
                    break;
                case 8006004:
                    if (buildingArea >= 90 && buildingArea <= 120)
                        flag = true;
                    break;
                case 8006005:
                    if (buildingArea > 120)
                        flag = true;
                    break;
            }
            return flag;
        }

        /// <summary>
        /// 把对应相关条件的信息存储起来,如果已存在则更新
        /// </summary>
        /// <param name="_listAvgPrice">存储集合</param>
        /// <param name="code">类型Code</param>
        /// <param name="datProjectAvgPrice">存储对象</param>
        void AddList(List<DATProjectAvgPrice> _listAvgPrice,
            DATProjectAvgPrice datProjectAvgPrice)
        {
            IEnumerable<DATProjectAvgPrice> vExists = null;
            int index = 0;
            if (datProjectAvgPrice.PurposeType.Equals(1002001))//普通住宅
            {
                vExists = _listAvgPrice
                    .Where(lpAvgPrice =>
                        lpAvgPrice.BuildingTypeCode.Equals(datProjectAvgPrice.BuildingTypeCode) &&
                                        lpAvgPrice.BuildingAreaType
                                        .Equals(datProjectAvgPrice.BuildingAreaType));
                //检索已存在的信息哪个下标中
                index = _listAvgPrice.FindIndex(
                    delegate(DATProjectAvgPrice pAvgPrice)
                    {
                        return pAvgPrice.PurposeType.Equals(datProjectAvgPrice.PurposeType) &&
                               pAvgPrice.BuildingAreaType.Equals(datProjectAvgPrice.BuildingAreaType);
                    });
            }
            else if (datProjectAvgPrice.PurposeType.Equals(1002027) ||//别墅
                datProjectAvgPrice.PurposeType.Equals(1002005) ||//独立别墅
                datProjectAvgPrice.PurposeType.Equals(1002006) ||//联排别墅
                datProjectAvgPrice.PurposeType.Equals(1002007) ||//叠加别墅
                datProjectAvgPrice.PurposeType.Equals(1002008))//双拼别墅
            {
                vExists = _listAvgPrice
                    .Where(lpAvgPrice =>
                        lpAvgPrice.PurposeType.Equals(datProjectAvgPrice.PurposeType));
                //检索已存在的信息哪个下标中
                index = _listAvgPrice.FindIndex(
                    delegate(DATProjectAvgPrice pAvgPrice)
                    {
                        return pAvgPrice.PurposeType.Equals(datProjectAvgPrice.PurposeType);
                    });
            }
            if (vExists != null && vExists.Any())
            {

                int zhi = (int)decimal.Add(_listAvgPrice[index].AvgPrice,
                    Decimal.Round(datProjectAvgPrice.AvgPrice, 0));
                //单价总合
                _listAvgPrice[index].AvgPrice = zhi;
                //数量总合update_rock
                //_listAvgPrice[index].Id = (int)Decimal.Add(_listAvgPrice[index].Id, datProjectAvgPrice.Id);
                if (!StringHelp.IsInteger(_listAvgPrice[index].JSFS))
                {
                    _listAvgPrice[index].JSFS = "0";
                }
                _listAvgPrice[index].JSFS = ((int)Decimal.Add(Convert.ToInt32(_listAvgPrice[index].JSFS), Convert.ToInt32(datProjectAvgPrice.JSFS))).ToString();
            }
            else
            {
                _listAvgPrice.Add(datProjectAvgPrice);
            }
        }

        /// <summary>
        /// 设置价格和数量
        /// </summary>
        /// <param name="_listObject">存储集合</param>
        /// <param name="Residence">根据条件得到的集合对象</param>
        /// <param name="datProjectAvgPrice">存储对象</param>
        /// <param name="code">标识值</param>
        void SetPriceAndCount(List<DATProjectAvgPrice> _listObject,
            IEnumerable<DATCase> Residence,
            DATProjectAvgPrice datProjectAvgPrice)
        {
            //总价
            datProjectAvgPrice.AvgPrice = Residence != null ?
                (int)Decimal.Round(Residence.Sum(sitem => sitem.UnitPrice.Value), 0) : 0;
            //总量update_rock
            //datProjectAvgPrice.Id = Residence != null ? Residence.Count() : 0;
            datProjectAvgPrice.JSFS = Residence != null ? Residence.Count().ToString() : "0";
            AddList(_listObject, datProjectAvgPrice);
        }
        /// <summary>
        /// 删除楼盘指定月份数单价过高和过低的案例
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cityTable"></param>
        /// <param name="projectId"></param>
        /// <param name="purposeCodes">用途</param>
        /// <param name="date">当前月份日期</param>
        /// <param name="mothsLength">要删除的月数</param>
        void DeleteMothsLengthMaxOrMinPriceCase(MSSQLDBDAL db, SYSCityTable cityTable, int projectId, int[] purposeCodes, string date, int mothsLength)
        {
            for (int i = 0; i < mothsLength; i++)
            {
                string _date = Utility.GetDateTimeMoths(date, 0 - i, "yyyy-MM");
                UtilityDALHelper.DeleteMaxOrMinPriceCase(db, cityTable, projectId, purposeCodes, _date);
            }
        }

        #endregion


        #region 省份(SYS_Province)

        //省份
        public string GetProvince()
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            var province = _mssql.GetList<SYSProvince>(
                string.Format("{0} 1=1", Utility.GetMSSQL_SQL(typeof(SYSProvince), Utility.SYSProvince)));

            if (province.Count > 0)
                return Utility.GetJson(1, "", province);
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 根据名称获取省份
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <returns></returns>
        public string GetProvinceByName(string provinceName)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            string sql = string.Format("{0}  ProvinceName='{1}' and Alias='{1}'",
                          Utility.GetMSSQL_SQL(typeof(SYSProvince), Utility.SYSProvince), provinceName);
            SYSProvince province = db.GetModel<SYSProvince>(sql);
            return JsonConvert.SerializeObject(province);
        }

        /// <summary>
        /// 根据ID获取省份
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public string GetProvinceById(int provinceId)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            SYSProvince province = db.GetCustom<SYSProvince>(
                (Expression<Func<SYSProvince, bool>>)(obj =>
                    obj.ProvinceId == provinceId));
            db.Close();
            return JsonConvert.SerializeObject(province);
        }

        /// <summary>
        /// 根据ID获取省份(类型ADO)
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public string GetProvinceADOById(int provinceId)
        {
            MSSQLADODAL adodb = new MSSQLADODAL();
            string sql = string.Format("{0} ProvinceId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSProvince), Utility.SYSProvince), provinceId);
            SYSProvince province = adodb.GetModel<SYSProvince>(sql);
            return JsonConvert.SerializeObject(province);
        }
        #endregion

        #region 城市(SYS_City)

        /// <summary>
        /// 根据省份获得城市列表
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public string GetCity(int provinceId)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            var city = UtilityDALHelper.GetADOCity(_mssql, provinceId);
            if (city.Count > 0)
                return Utility.GetJson(1, "", city);
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 根据城市名称获取城市
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        public string GetCityByCityName(string cityName)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            SYSCity city = UtilityDALHelper.GetCity(db, new SYSCity { CityName = cityName });
            db.Close();
            return JsonConvert.SerializeObject(city);
        }

        /// <summary>
        /// 根据城市名称获取城市列表
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        public string GetCityListByCityName(string cityName)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            string sql = string.Format("{0} CityName='{1}' or Alias='{1}'",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), cityName);

            List<SYSCity> city = db.GetList<SYSCity>(sql);
            return JsonConvert.SerializeObject(city);
        }
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public string GetAllCity()
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<SYSCity> list = UtilityDALHelper.GetAllCity(db);
            db.Close();
            return JsonConvert.SerializeObject(list);

        }

        /// <summary>
        /// 根据城市ID获得省份
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public string GetProvinceByCityId(int cityId)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            string sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), cityId);

            SYSCity sysCity = db.GetModel<SYSCity>(sql);

            sql = string.Format("{0} ProvinceId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSProvince), Utility.SYSProvince), sysCity.ProvinceId);

            SYSProvince sysProvince = db.GetModel<SYSProvince>(sql);

            return JsonConvert.SerializeObject(sysProvince);
        }

        /// <summary>
        /// 根据城市ID获得城市
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public string GetCityByCityId(int cityId)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            string sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), cityId);
            SYSCity sysCity = db.GetModel<SYSCity>(sql);

            return JsonConvert.SerializeObject(sysCity);
        }

        /// <summary>
        /// 根据城市ID获得城市(类型ADO)
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public string GetCityADOByCityId(int cityId)
        {
            MSSQLADODAL adodb = new MSSQLADODAL();
            string sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), cityId);
            SYSCity sysCity = adodb.GetModel<SYSCity>(sql);
            return JsonConvert.SerializeObject(sysCity);
        }
        #endregion

        #region 行政区(SYS_Area)

        //行政区
        public string GetArea(int cityId)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            var area = UtilityDALHelper.GetADOSYSArea(_mssql, cityId);
            if (area.Count > 0)
                return Utility.GetJson(1, "", area);
            return Utility.GetJson(0, "");
        }
        //行政区
        public string GetAreaByCityName(string cityName)
        {
            MSSQLDBDAL _mssql = new MSSQLDBDAL();
            var area = UtilityDALHelper.GetSYSArea(_mssql, cityName);
            _mssql.Close();
            return JsonConvert.SerializeObject(area);
        }

        /// <summary>
        /// 根据区域名称得到相关所有区域
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <returns></returns>
        public string GetAreaListByAraeName(string areaName)
        {
            MSSQLADODAL adodb = new MSSQLADODAL();
            string sql = string.Format("{0} AreaName='{1}' or AreaName='{2}' or AreaName='{3}'or AreaName='{4}'",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea),
                areaName, string.Format("{0}区", areaName),
                string.Format("{0}县", areaName),
                string.Format("{0}市", areaName));

            var area = adodb.GetList<SYSArea>(sql);

            return JsonConvert.SerializeObject(area);
        }

        /// <summary>
        /// 根据行政区获得城市
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public string GetCityByAreaId(int areaId)
        {
            MSSQLADODAL adodb = new MSSQLADODAL();
            string sql = string.Format("{0} AreaId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea), areaId);
            SYSArea sysArea = adodb.GetModel<SYSArea>(sql);

            sql = string.Format("{0} CityId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSCity), Utility.SYSCity), sysArea.CityId);
            SYSCity sysCity = adodb.GetModel<SYSCity>(sql);

            return JsonConvert.SerializeObject(sysCity);
        }
        /// <summary>
        /// 更加多个areaId获取行政区
        /// </summary>
        /// <param name="areaIds"></param>
        /// <returns></returns>
        public string GetSYSAreaByAreaIds(string areaIds)
        {
            if (string.IsNullOrEmpty(areaIds))
            {
                return "";
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            string[] _areaIds = areaIds.Split(',');
            IList<SYSArea> areaList = UtilityDALHelper.GetSYSAreaByAreaIds(db, _areaIds.ConvertToInts());
            db.Close();
            return JsonConvert.SerializeObject(areaList);
        }

        /// <summary>
        /// 根据行政区ID获得行政区
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        public string GetAreaByAreaId(int areaId)
        {
            MSSQLADODAL adodb = new MSSQLADODAL();
            string sql = string.Format("{0} AreaId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea), areaId);
            SYSArea sysArea = adodb.GetModel<SYSArea>(sql);

            return JsonConvert.SerializeObject(sysArea);
        }

        /// <summary>
        /// 根据行政区ID获得行政区(类型ADO)
        /// </summary>
        /// <param name="areaId">行政区ID</param>
        /// <returns></returns>
        public string GetAreaADOByAreaId(int areaId)
        {
            MSSQLADODAL adodb = new MSSQLADODAL();
            string sql = string.Format("{0} AreaId={1}",
                Utility.GetMSSQL_SQL(typeof(SYSArea), Utility.SYSArea), areaId);
            SYSArea sysArea = adodb.GetModel<SYSArea>(sql);
            return JsonConvert.SerializeObject(sysArea);
        }

        #endregion

        #region 楼盘(DAT_Project)

        /// <summary>
        /// 根据城市名称+楼盘名称 模糊检索出楼盘信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="area"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public string GetProjectByCityNameAndLikePrjName(string cityName, string projectName, int length)
        {
            MSSQLDBDAL _mssql = new MSSQLDBDAL();
            int cityId = 0;
            //获取城市
            SYSCity cityObj = UtilityDALHelper.GetCity(_mssql, new SYSCity() { CityName = cityName });
            if (cityObj == null)
            {
                return "";
            }
            cityId = cityObj.CityId;
            IList<DATProject> list = UtilityDALHelper.GetListProjectLikeByCityId(_mssql, projectName, cityId, length);
            _mssql.Close();
            list = list.EncodeField<DATProject>();
            return JsonConvert.SerializeObject(list); ;
        }

        /// <summary>
        /// 根据城市ID+楼盘名称 模糊检索出楼盘信息   李晓东
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="projectName">楼盘名称</param>
        /// <returns></returns>
        public string GetProjectByCityIDAndLikePrjName(int cId, int aId, string pName)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            IList<DATProject> list = UtilityDALHelper.GetListProjectLikeByCityIdAndAreaId(_mssql, pName, cId, aId);
            List<JObject> listobj = new List<JObject>();
            string sql = string.Empty;
            foreach (var item in list)
            {
                JObject _jobj = new JObject();
                sql = string.Format("{0} ProjectNameId={1}", Utility.GetMSSQL_SQL(typeof(SYSProjectMatch), Utility.SYSProjectMatch)
                    , item.ProjectId);
                var clist = _mssql.GetModel<SYSProjectMatch>(sql);
                _jobj.Add("Project", Utils.Serialize(item));
                _jobj.Add("Children", Utils.Serialize(clist));
                _jobj.Add("MatchStatus", 0);//运维中心库匹配
                listobj.Add(_jobj);
            }

            _mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            sql = string.Format(new StringBuilder().Append("{0} CityID={1}  ")//and AreaId={2}
                             .Append(" and ProjectName like '%{2}%'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DataProject), Utility.loan_DataProject),
                          cId, pName);//, aId
            IList<DataProject> listDataProject = _mssql.GetList<DataProject>(sql);
            listDataProject = listDataProject.OrderBy(orderItem => orderItem.ProjectName).ToList();
            foreach (var item in listDataProject)
            {
                JObject _jobj = new JObject();
                _jobj.Add("Project", Utils.Serialize(item));
                _jobj.Add("Children", "");
                _jobj.Add("MatchStatus", 1);//贷后临时库匹配
                listobj.Add(_jobj);
            }
            return JsonConvert.SerializeObject(listobj);
        }

        /// <summary>
        /// 根据城市ID、楼盘ID、楼栋名称得到楼栋 李晓东
        /// </summary>
        /// <param name="cId">城市ID</param>
        /// <param name="pId">楼盘ID</param>
        /// <param name="bName">楼栋名称</param>
        /// <returns></returns>
        public string GetBuildingByProjectIdCityIDAndLikeBuildingName(int cId, int pId, string bName)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            string sql = string.Empty;
            List<JObject> listobj = new List<JObject>();
            //获取对应区域表obj
            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
            if (cityTable != null)
            {
                sql = string.Format(new StringBuilder().Append("{0} Valid=1 and CityID={1} and ProjectId={2}  ")//and AreaId={2}
                             .Append(" and BuildingName like '%{3}%'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable),
                          cId, pId, bName);
                var building = _mssql.GetList<DATBuilding>(sql);//楼栋
                foreach (var item in building)
                {
                    JObject obj = new JObject();
                    obj.Add("Building", Utils.Serialize(item));
                    obj.Add("MatchStatus", 0);//运维中心库匹配
                    listobj.Add(obj);
                }
            }
            _mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            sql = string.Format(new StringBuilder().Append("{0} CityID={1} and ProjectId={2}  ")//and AreaId={2}
                             .Append(" and BuildingName like '%{3}%'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DataBuilding), Utility.loan_DataBuilding),
                          cId, pId, bName);
            var loanbuilding = _mssql.GetList<DataBuilding>(sql);//楼栋
            foreach (var item in loanbuilding)
            {
                JObject obj = new JObject();
                obj.Add("Building", Utils.Serialize(item));
                obj.Add("MatchStatus", 1);//贷后临时库匹配
                listobj.Add(obj);
            }
            return JsonConvert.SerializeObject(listobj);
        }

        /// <summary>
        /// 根据城市ID、楼栋ID、房号名称等到房号
        /// </summary>
        /// <param name="cId">城市ID</param>
        /// <param name="bId">楼栋ID</param>
        /// <param name="hName">房号名称</param>
        /// <returns></returns>
        public string GetHouseByBuildingIdCityIDAndLikeHouseName(int cId, int bId, string hName)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            string sql = string.Empty;
            List<JObject> listobj = new List<JObject>();
            //获取对应区域表obj
            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
            if (cityTable != null)
            {
                sql = string.Format(new StringBuilder().Append("{0} Valid=1 and CityID={1} and BuildingId={2}  ")//and AreaId={2}
                             .Append(" and HouseName like '%{3}%'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DATHouse), cityTable.HouseTable),
                          cId, bId, hName);
                var building = _mssql.GetList<DATHouse>(sql);//房号
                foreach (var item in building)
                {
                    JObject obj = new JObject();
                    obj.Add("House", Utils.Serialize(item));
                    obj.Add("MatchStatus", 0);//运维中心库匹配
                    listobj.Add(obj);
                }
            }
            _mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            sql = string.Format(new StringBuilder().Append("{0} CityID={1} and BuildingId={2}  ")//and AreaId={2}
                             .Append(" and HouseName like '%{3}%'").ToString(),
                          Utility.GetMSSQL_SQL(typeof(DataHouse), Utility.loan_DataHouse),
                          cId, bId, hName);
            var loanbuilding = _mssql.GetList<DataHouse>(sql);//房号
            foreach (var item in loanbuilding)
            {
                JObject obj = new JObject();
                obj.Add("House", Utils.Serialize(item));
                obj.Add("MatchStatus", 1);//贷后临时库匹配
                listobj.Add(obj);
            }
            return JsonConvert.SerializeObject(listobj);
        }

        /// <summary>
        /// 根据地址、楼盘名称查找信息   李晓东
        /// </summary>
        /// <param name="pName">楼盘名称</param>
        /// <param name="pAddress">楼盘地址</param>
        /// <param name="cId">城市</param>
        /// <returns></returns>
        public string GetProjectJoinPMatchByPNameOrPAddressCityId(string pName, string pAddress, int cId)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATProject> list = UtilityDALHelper
                .GetProjectJoinProjectMatchByPNameOrPAddressCityId(db, cId, pName, pAddress);
            db.Close();
            if (list.Count() == 0)
            {
                db = new MSSQLDBDAL(Utility.DBFxtLoan);
                string sql = string.Format(new StringBuilder()
                      .Append("{0}  CityID={1} and [Address] like '%{2}%' ").ToString(),
                      Utility.GetMSSQL_SQL(typeof(DataProject), Utility.loan_DataProject),
                      cId, pAddress);
                var listData = db.GetCustomSQLQueryList<DataProject>(sql);
                db.Close();
                return JsonConvert.SerializeObject(listData.EncodeField());
            }
            else
            {
                return JsonConvert.SerializeObject(list.EncodeField());
            }
        }

        /// <summary>
        /// 根据地址、楼盘名称查找信息   李晓东 ADO
        /// </summary>
        /// <param name="pName">楼盘名称</param>
        /// <param name="pAddress">楼盘地址</param>
        /// <param name="cId">城市</param>
        /// <returns></returns>
        public string GetProjectJoinPMatchADOByPNameOrPAddressCityId(string pName, string pAddress, int cId)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            IList<DATProject> list = UtilityDALHelper
                .GetProjectJoinProjectMatchADOByPNameOrPAddressCityId(db, cId, pName, pAddress);

            if (list.Count() == 0)
            {
                db = new MSSQLADODAL(Utility.DBFxtLoan);
                string sql = string.Format(new StringBuilder()
                      .Append("{0}  CityID={1} and [Address] like '%{2}%' ").ToString(),
                      Utility.GetMSSQL_SQL(typeof(DataProject), Utility.loan_DataProject),
                      cId, pAddress);
                var listData = db.GetList<DataProject>(sql);

                return JsonConvert.SerializeObject(listData.EncodeField());
            }
            else
            {
                return JsonConvert.SerializeObject(list.EncodeField());
            }
        }

        public string GetProjectViewByCityNameAndLikePrjName(string cityName, string projectName, int length)
        {
            MSSQLDBDAL _mssql = new MSSQLDBDAL();
            int cityId = 0;
            //获取城市
            SYSCity cityObj = UtilityDALHelper.GetCity(_mssql, new SYSCity() { CityName = cityName });
            if (cityObj == null)
            {
                return "";
            }
            cityId = cityObj.CityId;
            IList<DATProject> list = UtilityDALHelper.GetListProjectLikeByCityId(_mssql, projectName, cityId, length);
            List<int> intList = new List<int>();
            foreach (DATProject proj in list)
            {
                if (intList.Where(p => p == proj.AreaID).Count() < 1)
                {
                    intList.Add(proj.AreaID);
                }
            }
            IList<SYSArea> areaList = UtilityDALHelper.GetSYSAreaByAreaIds(_mssql, intList.ToArray());
            IList<SYSCity> citylist = new List<SYSCity>();
            citylist.Add(cityObj);
            IList<DATProjectView> viewlist = DATProjectView.ConvertToList(list, citylist, areaList);
            _mssql.Close();
            list = list.EncodeField<DATProject>();
            return JsonConvert.SerializeObject(viewlist); ;
        }
        /// <summary>
        /// 根据城市ID and 多个楼盘Ids获取案例信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        public string GetProjectByCityIdAndProjectIds(int cityId, string projectIds)
        {
            if (string.IsNullOrEmpty(projectIds))
            {
                return "";
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATProject> list = UtilityDALHelper.GetProjectByCityIdAndProjectIds(db, cityId, projectIds.Split(',').ConvertToInts());
            db.Close();
            list = list.EncodeField();
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 根据名称成城市ID,获取楼盘信息(关联网络名查询)
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public string GetProjectJoinProjectMatchByProjectNameCityId(string projectName, int cityId)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return "";
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATProject> list = UtilityDALHelper.GetProjectJoinProjectMatchByProjectNameCityId(db, cityId, projectName);
            db.Close();
            list = list.EncodeField();
            string json = JsonConvert.SerializeObject(list);
            return json;
        }
        /// <summary>
        /// 根据名称成城市ID,获取楼盘信息(关联网络名查询) ADO
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public string GetProjectJoinProjectMatchADOByProjectNameCityId(string projectName, int cityId)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                return "";
            }
            MSSQLADODAL db = new MSSQLADODAL();
            IList<DATProject> list = UtilityDALHelper.GetProjectJoinProjectMatchADOByProjectNameCityId(db, cityId, projectName);

            list = list.EncodeField();
            string json = JsonConvert.SerializeObject(list);
            return json;
        }
        /// <summary>
        /// 根据楼盘名称+城市名获取楼盘信息
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="cityName"></param>
        /// <returns>DATProject结构json字符串(带UrlEncode编码)</returns>
        public string GetProjectByProjectNameAndCityName(string projectName, string cityName)
        {
            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(cityName))
            {
                return "";
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            SYSCityTable table = UtilityDALHelper.GetCityTableByCityName(db, cityName);
            if (table == null)
            {
                db.Close();
                return "";
            }
            DATProject obj = UtilityDALHelper.GetProjectByProjectNameAndCityId(db, table.ProjectTable, table.CityId, projectName);
            db.Close();
            if (obj == null)
            {
                return "";
            }
            DATProject project = obj;
            string json = JsonConvert.SerializeObject(project);
            return json;
        }
        /// <summary>
        /// 获取楼盘 根据城市名称
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount">是否获取总个数</param>
        /// <returns></returns>
        public string GetProjectByCityNameAndPage(string cityName, int pageIndex, int pageSize, int isGetCount)
        {

            if (string.IsNullOrEmpty(cityName) || string.IsNullOrEmpty(cityName))
            {
                return "";
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            int count = 0;
            IList<DATProject> list = UtilityDALHelper.GetProjectByCityName(db, cityName, out count, isGetCount: isGetCount == 0 ? false : true, pageIndex: pageIndex, pageSize: pageSize);
            db.Close();

            if (list == null || list.Count < 1)
            {
                return "";
            }
            list = list.EncodeField();
            FxtApi_ResultPageList apiResult = new FxtApi_ResultPageList(count, list);
            string json = JsonConvert.SerializeObject(apiResult);
            return json;
        }
        /// <summary>
        /// 根据楼盘ID查询楼盘信息(根据机构权限和子表)(创建人:曾智磊,时间:2014.04.176)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GetProjectDetailByProjectIdAndCityIdAndCompanyId(int projectId, int cityId, int companyId)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            var resultData = new
            {
                project = "null",
                appendage = "[]",
                pcompany = "[]",
                phcount = 0
            };
            try
            {
                DATProject project = UtilityDALHelper.GetProjectByCompanyIdAndCityIdAndProjectId(db, companyId, cityId, projectId);
                if (project == null)
                {
                    return Utility.GetJson(1, "", JsonConvert.SerializeObject(resultData), 0);
                    db.Close();
                }
                IList<LNKPAppendage> lnkpaList = UtilityDALHelper.GetLNKPAppendageByProjectIdAndCityId(db, projectId, cityId);
                IList<LNKPCompany> lnkpcList = UtilityDALHelper.GetLNKPCompanyByProjectIdAndCityId(db, projectId, cityId);
                List<int> companyIds = new List<int>();
                foreach (LNKPCompany com in lnkpcList)
                {
                    companyIds.Add(com.LNKPCompanyPX.CompanyId);
                }
                IList<DATCompany> comList = UtilityDALHelper.GetDATCompanyByCompanyIds(db, companyIds.ToArray());
                int phCount = UtilityDALHelper.GetLNKPPhotoCount(db, projectId, cityId, companyId);
                List<JObject> jobList = new List<JObject>();
                foreach (LNKPCompany com in lnkpcList)
                {
                    JObject job = JObject.Parse(JsonConvert.SerializeObject(com));
                    string companyName = "";
                    DATCompany comObj = comList.Where(tbl => tbl.CompanyId == com.LNKPCompanyPX.CompanyId).FirstOrDefault();
                    if (comObj != null)
                    {
                        companyName = comObj.ChineseName;
                    }
                    job.Add(new JProperty("CompanyName", companyName));
                    jobList.Add(job);

                }
                var resultData2 = new
                {
                    project = JsonConvert.SerializeObject(project),
                    appendage = JsonConvert.SerializeObject(lnkpaList),
                    pcompany = JsonConvert.SerializeObject(jobList),
                    phcount = phCount
                };
                db.Close();
                return Utility.GetJson(1, "成功", JsonConvert.SerializeObject(resultData2), 0);
            }
            catch (Exception ex)
            {
                log.Error("获取正式库楼盘详细信息异常", ex);
            }
            db.Close();
            return Utility.GetJson(0, "失败", "系统异常", 0);

        }

        /// <summary>
        /// 新建楼盘
        /// </summary>
        /// <param name="projectName">楼盘名称</param>
        /// <param name="cityId"></param>
        /// <param name="areaId">行政区ID</param>
        /// <param name="purposeCode">住用途ID</param>
        /// <param name="address">楼盘地址</param>
        /// <returns></returns>
        public string InsertProject(string projectName, int cityId, int areaId, int purposeCode, string address)
        {
            FxtApi_Result result = new FxtApi_Result(1, "");
            MSSQLDBDAL _mssql = new MSSQLDBDAL();
            SYSCity city = UtilityDALHelper.GetCityById(_mssql, cityId);
            if (city == null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("填写的城市不存在"));
                _mssql.Close();
                return JsonConvert.SerializeObject(result);
            }
            SYSArea area = UtilityDALHelper.GetSYSAreaById(_mssql, areaId);
            if (area == null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("填写的行政区不存在"));
                _mssql.Close();
                return JsonConvert.SerializeObject(result);
            }
            SYSCode code = UtilityDALHelper.GetProjectPurposeCodeByCode(_mssql, purposeCode);
            if (area == null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("填写的住用途不存在"));
                _mssql.Close();
                return JsonConvert.SerializeObject(result); ;
            }
            SYSCityTable table = UtilityDALHelper.GetCityTable(_mssql, cityId);
            DATProject projObj = UtilityDALHelper.GetProjectByProjectNameAndCityId(_mssql, table.ProjectTable, cityId, projectName);
            if (projObj != null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("楼盘名称已存在"));
                _mssql.Close();
                return JsonConvert.SerializeObject(result);
            }
            projObj = UtilityDALHelper.GetProjectByOtherNameAndCityId(_mssql, table.ProjectTable, cityId, projectName);
            if (projObj != null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField(string.Format("楼盘名称已存在,为楼盘\"{0}\"的别名", projObj.ProjectName)));
                _mssql.Close();
                return JsonConvert.SerializeObject(result);
            }
            DATProject project = new DATProject { ProjectName = projectName, CityID = cityId, AreaID = areaId, Address = address, PurposeCode = purposeCode, Valid = 1, FxtCompanyId = 25 };
            log.Debug(string.Format("新增楼盘:table{0},projectName:{1}", table.ProjectTable, project.ProjectName));
            if (!UtilityDALHelper.InsertDATProject(_mssql, table.ProjectTable, project))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("系统异常"));
            }
            _mssql.Close();
            return JsonConvert.SerializeObject(result); ;
        }


        #endregion


        #region 楼栋信息(DATBuilding)
        /// <summary>
        /// 根据楼栋ID查询楼栋信息(根据机构权限和子表)(创建人:曾智磊,时间:2014.04.17)
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GetBuildingDetailByBuildingIdAndCityIdAndCompanyId(int buildingId, int cityId, int companyId)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            DATBuilding obj = UtilityDALHelper.GetBuildingByCompanyIdAndCityIdAndBuildingId(db, companyId, cityId, buildingId);
            db.Close();
            if (obj == null)
            {
                return Utility.GetJson(1, "", null, 0);
            }
            return Utility.GetJson(1, "成功", JsonConvert.SerializeObject(obj), 0);
        }
        #endregion
        #region DATHouse(房号信息)
        /// <summary>
        /// 根据房号ID查询房号信息(根据机构权限和子表)(创建人:曾智磊,时间:2014.04.17)
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string GetHouseDetailByHouseIdAndCityIdAndCompanyId(int houseId, int cityId, int companyId)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            DATHouse obj = UtilityDALHelper.GetHouseByCompanyIdAndCityIdAndHouseId(db, companyId, cityId, houseId);
            db.Close();
            if (obj == null)
            {
                return Utility.GetJson(1, "", null, 0);
            }
            return Utility.GetJson(1, "成功", JsonConvert.SerializeObject(obj), 0);
        }
        #endregion

        #region 楼盘网络名(SYS_ProjectMatch)
        /// <summary>
        /// 新增网络关系
        /// </summary>
        /// <param name="netName"></param>
        /// <param name="projectName"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public string InsertSYSProjectMatch(string netName, string projectName, int projectId, int cityId, string ip, string validate)
        {
            FxtApi_Result result = new FxtApi_Result(1, "");
            MSSQLDBDAL _mssql = new MSSQLDBDAL();
            SYSProjectMatch obj = new SYSProjectMatch { CityId = cityId, NetName = netName, ProjectNameId = projectId, ProjectName = projectName };
            string _message = "";
            if (!CheckSYSProjectMatch(_mssql, obj, out _message))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField(_message));
                _mssql.Close();
                return JsonConvert.SerializeObject(result);
            }
            _mssql.Create(obj);
            _mssql.Close();
            return JsonConvert.SerializeObject(result); ;
        }
        /// <summary>
        /// 新增网络关系插入多个
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="ip"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        public string InsertSYSProjectMatchList(string jsonData)
        {

            FxtApi_Result result = new FxtApi_Result(1, "", "null");
            if (string.IsNullOrEmpty(jsonData))
            {
                return JsonConvert.SerializeObject(result); ;
            }
            MSSQLDBDAL _mssql = new MSSQLDBDAL();
            JArray jarray = JArray.Parse(jsonData);
            IList<SYSProjectMatch> list = new List<SYSProjectMatch>();
            int i = 0;
            string _message = "";
            while (i < jarray.Count)
            {
                JObject jobject = (JObject)jarray[i];
                int projectId = jobject["ProjectNameId"].Value<int>();
                string netName = jobject["NetName"].Value<string>().DecodeField();
                string projectName = jobject["ProjectName"].Value<string>().DecodeField();
                int cityId = jobject["CityId"].Value<int>();
                SYSProjectMatch obj = new SYSProjectMatch { CityId = cityId, NetName = netName, ProjectNameId = projectId, ProjectName = projectName };
                obj = obj.DecodeField<SYSProjectMatch>();
                if (CheckSYSProjectMatch(_mssql, obj, out _message))
                {
                    list.Add(obj);
                }
                i++;
            }
            if (list != null && list.Count > 0)
            {
                _mssql.Create<SYSProjectMatch>(list);
            }
            _mssql.Close();
            list = list.EncodeField<SYSProjectMatch>();
            string listJson = JsonConvert.SerializeObject(list);
            result = new FxtApi_Result(1, "", listJson);
            return JsonConvert.SerializeObject(result);
        }
        #endregion

        #region 案例(DAT_Case)

        #region(查询)

        /// <summary>
        /// 根据房号、城市、楼栋得到房号(物业)中的房号 李晓东
        /// </summary>
        /// <param name="houseName">房号</param>
        /// <param name="cId">城市</param>
        /// <param name="bId">楼栋(楼宇)</param>
        /// <returns></returns>
        public string GetHouseByHouse_City_Building(string houseName, int cId, int bId)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            DATHouse dat_Case = UtilityDALHelper.GetHouseByHouse_City_Building(db, houseName, cId, bId);
            if (dat_Case == null)
            {
                db = new MSSQLADODAL(Utility.DBFxtLoan);
                string sql = string.Format("{0} BuildingId={1} and CityID={2} and HouseName='{3}'",
                          Utility.GetMSSQL_SQL(typeof(DataHouse), Utility.loan_DataHouse),
                          bId, cId, houseName);
                var data_Case = db.GetModel<DataHouse>(sql);
                return JsonConvert.SerializeObject(data_Case);
            }
            else
            {
                return JsonConvert.SerializeObject(dat_Case);
            }
        }

        /// <summary>
        /// 根据楼盘、城市、楼宇名称(楼栋名称)得到楼宇(楼栋) 信息 李晓东
        /// </summary>
        /// <param name="pId">楼盘</param>
        /// <param name="cId">城市</param>
        /// <param name="bName">楼栋名称</param>
        /// <returns></returns>
        public string GetBuildingByProject_City_Build(int pId, int cId, string bName)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            DATBuilding dat_Case = UtilityDALHelper.GetBuildingADOByProject_City_Build(db, pId, cId, bName);
            if (dat_Case == null)
            {
                db = new MSSQLADODAL(Utility.DBFxtLoan);

                string sql = string.Format("{0} CityID={1} and ProjectId={2} and BuildingName='{3}'",
                          Utility.GetMSSQL_SQL(typeof(DataBuilding), Utility.loan_DataBuilding),
                          cId, pId, bName);

                var data_Case = db.GetModel<DataBuilding>(sql);

                return JsonConvert.SerializeObject(data_Case);
            }
            else
            {
                return JsonConvert.SerializeObject(dat_Case);
            }
        }

        /// <summary>
        /// 根据城市ID and 多个案例ID获取案例信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        public string GetCaseByCityIdAndCaseIds(int cityId, string caseIds)
        {
            if (string.IsNullOrEmpty(caseIds))
            {
                return "";
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATCase> list = UtilityDALHelper.GetCaseByCityIdAndCaseIds(db, cityId, caseIds.Split(',').ConvertToInts());
            db.Close();
            list = list.EncodeField();
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 获取案例信息
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseId"></param>
        /// <returns>DATCase类型json格式,带UrlEncode编码</returns>
        public string GetCaseByCityIdAndCaseId(int cityId, int caseId)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            DATCase _case = UtilityDALHelper.GetCaseByCityIdAndCaseId(db, cityId, caseId);
            db.Close();
            _case = _case.EncodeField();
            return JsonConvert.SerializeObject(_case);
        }
        /// <summary>
        /// 根据城市ID and 多个楼盘Ids获取案例Id+楼盘名
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns>json:[{CaseId:CaseId,ProjectName:"ProjectName"},{}...]字符串属性带UrlEncode编码</returns>
        public string GetCaseIdJoinProjectNameByCityIdAndCaseIds(int cityId, string caseIds)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATCase> caseList = UtilityDALHelper.GetCaseByCityIdAndCaseIds(db, cityId, caseIds.Split(',').ConvertToInts());
            if (caseList == null || caseList.Count < 1)
            {
                db.Close();
                return "";
            }
            List<int> projectIds = new List<int>();
            foreach (DATCase _case in caseList)
            {
                int projId = projectIds.Find(delegate(int _id) { return _case.ProjectId == _id; });
                if (projId < 1)
                {
                    projectIds.Add(_case.ProjectId);
                }
            }
            IList<DATProject> projectList = UtilityDALHelper.GetProjectByCityIdAndProjectIds(db, cityId, projectIds.ToArray());
            db.Close();
            if (projectList == null)
            {
                return "";
            }
            List<JObject> jObjList = new List<JObject>();
            foreach (DATCase _case in caseList)
            {
                int caseId = _case.CaseID;
                string projectName = "";
                DATProject project = projectList.Where(p => p.ProjectId == _case.ProjectId).FirstOrDefault();
                if (project != null)
                {
                    projectName = project.ProjectName;
                }
                JObject jObj = new JObject();
                jObj.Add(new JProperty("CaseId", caseId));
                jObj.Add(new JProperty("ProjectName", projectName.EncodeField()));
                jObjList.Add(jObj);
            }
            return JsonConvert.SerializeObject(jObjList);
        }
        /// <summary>
        /// 根据城市名称 and楼盘Id and建筑类型Code and建筑面积区间 and案例时间区间获取案例信息(companyId=25的showdata查询范围)
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID,房讯通为25,</param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="purposeCode">用途</param>
        /// <param name="buildingAreaCode">面积段code</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isGetCount"></param>
        /// <returns></returns>
        public string GetCaseByCityIdAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDateAndPage(int cityId, int projectId, string fxtCompanyIds, int? buildingTypeCode, int purposeCode, int? buildingAreaCode, string startDate, string endDate, int pageIndex, int pageSize, int isGetCount)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            int count = 0;
            decimal? _minArea = null;
            decimal? _maxArea = null;
            DateTime _startDate = DateTime.Now;
            DateTime _endDate = DateTime.Now;
            if (!string.IsNullOrEmpty(startDate))
            {
                _startDate = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                _endDate = Convert.ToDateTime(endDate);
            }
            IList<DATCase> list = UtilityDALHelper.GetCaseByCityIdAndProjectIdAndPurposeCodeAndBuildingTypeCodeAndAreaTypeAndDate(db, cityId, projectId, fxtCompanyIds, buildingTypeCode, purposeCode, buildingAreaCode, _startDate, _endDate, out count, isGetCount: isGetCount == 0 ? false : true, pageIndex: pageIndex, pageSize: pageSize);
            db.Close();

            if (list == null || list.Count < 1)
            {
                return "";
            }
            list = list.EncodeField();
            FxtApi_ResultPageList apiResult = new FxtApi_ResultPageList(count, list);
            string json = JsonConvert.SerializeObject(apiResult);
            return json;
        }
        /// <summary>
        /// 获取指定日期楼盘下案例个数(别墅,普通住宅的个数)(companyId=25的showdata查询范围)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID</param>
        /// <param name="dates">逗号分隔的日期数组(例如:"2012-03,2012-04")</param>
        /// <returns>json:[{ ProjectId:1, CityId:1, Date:"2012-03",PurposePublicCount:0, PurposeVillaCount:0},{}...]</returns>
        public string GetCaseCountJoinProjectJoinPurposeTypeByCityIdAndProjectIdAndDates(int cityId, int projectId, string fxtCompanyIds, string dates)
        {

            List<JObject> objList = new List<JObject>();
            string[] _dates = dates.Split(',');
            int code = 1002001;//普通住宅
            int[] code2 = new int[] { 0 };//别墅类型
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<SYSCode> sysCodeList = UtilityDALHelper.GetPurposeTypeCodeVillaType(db);
            List<int> intList = new List<int>();
            foreach (var sysCode in sysCodeList)
            {
                intList.Add(sysCode.Code);
            }
            if (intList != null && intList.Count > 0)
            {
                code2 = intList.ToArray();
            }
            SYSCityTable vCaseTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (vCaseTable == null)
            {
                db.Close();
                return "";
            }
            string sql = string.Format("{0} Valid=1 and FxtCompanyId in (" + fxtCompanyIds + ") and ProjectId={1} and {2} ", Utility.GetMSSQL_SQL(typeof(DATCase), tablename: vCaseTable.CaseTable), projectId, "{0}");
            foreach (string _date in _dates)
            {
                JObject jObj = new JObject();
                jObj.Add(new JProperty("ProjectId", projectId));
                jObj.Add(new JProperty("CityId", cityId));
                jObj.Add(new JProperty("Date", _date));

                string startDate = string.IsNullOrEmpty(_date) ? Utility.GetDateTimeMoths(null, -2, "yyyy-MM-01") : Utility.GetDateTimeMoths(_date, -2, "yyyy-MM-01");
                string endDate = string.IsNullOrEmpty(_date) ? Utility.GetDateTimeMoths() : Utility.GetDateTimeMoths(_date, 0, "yyyy-MM-dd 23:59:59");
                string _dateWhereHsql = string.Format("CaseDate between '{0}' and '{1}'", startDate, endDate);
                //查询普通住宅案例数量
                string _hsql2 = string.Format(sql, string.Format(" PurposeCode={0} and {1} ", code, _dateWhereHsql));
                int count = db.GetCustomSQLQueryList<DATCase>(_hsql2).Count();
                jObj.Add(new JProperty("PurposePublicCount", count));
                //查询别墅类型案例数量
                _hsql2 = string.Format(sql, string.Format(" PurposeCode in ({0}) and {1} ", code2.ConvertToString(), _dateWhereHsql));
                int count2 = db.GetCustomSQLQueryList<DATCase>(_hsql2).Count();
                jObj.Add(new JProperty("PurposeVillaCount", count2));
                objList.Add(jObj);
            }
            db.Close();
            string json = JsonConvert.SerializeObject(objList);
            return json;
        }
        /// <summary>
        /// 获取指定日期楼盘下普通住宅的案例个数(各建筑面积,建筑类型下的案例个数)(companyId=25的showdata查询范围)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID</param>
        /// <param name="date">日期(例如:"2012-03")</param>
        /// <returns>json:[{ ProjectId:1, CityId:1, Date:"2012-03", BuildingTypeCode:1, BuildingAreaTypeCode:1, Count:0, },{}...]</returns>
        public string GetCaseCountJoinProjectJoinBuildingTypeJoinAreaTypeByPublicPurposeAndCityIdAndProjectIdAndDate(int cityId, int projectId, string fxtCompanyIds, string date)
        {
            List<JObject> objList = new List<JObject>();
            int code = 1002001;//普通住宅
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<SYSCode> sysCodeList = UtilityDALHelper.GetListSYSCODE(db, Utility.CodeID_5);//建筑类型
            IList<SYSCode> sysCodeList2 = UtilityDALHelper.GetListSYSCODE(db, Utility.CodeID_10);//面积段
            SYSCityTable vCaseTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (vCaseTable == null)
            {
                db.Close();
                return "";
            }
            string startDate = string.IsNullOrEmpty(date) ? Utility.GetDateTimeMoths(null, -2, "yyyy-MM-01") : Utility.GetDateTimeMoths(date, -2, "yyyy-MM-01");
            string endDate = string.IsNullOrEmpty(date) ? Utility.GetDateTimeMoths() : Utility.GetDateTimeMoths(date, 0, "yyyy-MM-dd 23:59:59");
            string sql = string.Format("{0} Valid=1 and FxtCompanyId in (" + fxtCompanyIds + ") and ProjectId={1} and PurposeCode={2} and {3} and CaseDate between '{4}' and '{5}' ", Utility.GetMSSQL_SQL(typeof(DATCase), tablename: vCaseTable.CaseTable), projectId, code, "{0}", startDate, endDate);
            foreach (SYSCode _code in sysCodeList)
            {
                foreach (SYSCode _code2 in sysCodeList2)
                {
                    JObject jObj = new JObject();
                    jObj.Add(new JProperty("ProjectId", projectId));
                    jObj.Add(new JProperty("CityId", cityId));
                    jObj.Add(new JProperty("Date", date));
                    jObj.Add(new JProperty("BuildingTypeCode", _code.Code));
                    jObj.Add(new JProperty("BuildingAreaTypeCode", _code2.Code));
                    string _sql = string.Format(" BuildingTypeCode = {0} and {1}", _code.Code, BuildingAreaWhereSQL(_code2.Code));
                    _sql = string.Format(sql, _sql);
                    int count = db.GetCustomSQLQueryList<DATCase>(_sql).Count();
                    jObj.Add(new JProperty("Count", count));
                    objList.Add(jObj);
                }
            }
            db.Close();
            string json = JsonConvert.SerializeObject(objList);
            return json;
        }
        /// <summary>
        /// 获取指定日期楼盘下各别墅用途的案例个数(companyId=25的showdata查询范围)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyIds">由逗号分隔的多个公司ID</param>
        /// <param name="date">日期(例如:"2012-03")</param>
        /// <returns>json:[{ ProjectId:1, CityId:1, Date:"2012-03", PurposeTypeCode:1, Count:0, },{}...]</returns>
        public string GetCaseCountJoinProjectJoinPurposeTypeByVillaPurposeAndCityIdAndProjectIdAndDate(int cityId, int projectId, string fxtCompanyIds, string date)
        {
            List<JObject> objList = new List<JObject>();
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<SYSCode> sysCodeList = UtilityDALHelper.GetPurposeTypeCodeVillaType(db);//别墅类型用途
            SYSCityTable vCaseTable = UtilityDALHelper.GetCityTable(db, cityId);
            if (vCaseTable == null)
            {
                db.Close();
                return "";
            }
            string startDate = string.IsNullOrEmpty(date) ? Utility.GetDateTimeMoths(null, -2, "yyyy-MM-01") : Utility.GetDateTimeMoths(date, -2, "yyyy-MM-01");
            string endDate = string.IsNullOrEmpty(date) ? Utility.GetDateTimeMoths() : Utility.GetDateTimeMoths(date, 0, "yyyy-MM-dd 23:59:59");
            string sql = string.Format("{0} Valid=1 and FxtCompanyId in (" + fxtCompanyIds + ") and ProjectId={1} and PurposeCode={2}  and CaseDate between '{3}' and '{4}' ", Utility.GetMSSQL_SQL(typeof(DATCase), tablename: vCaseTable.CaseTable), projectId, "{0}", startDate, endDate);
            foreach (SYSCode _code in sysCodeList)
            {
                JObject jObj = new JObject();
                jObj.Add(new JProperty("ProjectId", projectId));
                jObj.Add(new JProperty("CityId", cityId));
                jObj.Add(new JProperty("Date", date));
                jObj.Add(new JProperty("PurposeTypeCode", _code.Code));
                string _sql = string.Format(sql, _code.Code);
                int count = db.GetCustomSQLQueryList<DATCase>(_sql).Count();
                jObj.Add(new JProperty("Count", count));
                objList.Add(jObj);
            }
            db.Close();
            string json = JsonConvert.SerializeObject(objList);
            return json;
        }
        string BuildingAreaWhereSQL(int code)
        {
            string sqlWhere = " 1=2 ";
            switch (code)
            {
                case 8006001:
                    sqlWhere = " BuildingArea <30 ";
                    break;
                case 8006002:
                    sqlWhere = " BuildingArea >=30 and BuildingArea<=60";
                    break;
                case 8006003:
                    sqlWhere = " BuildingArea >60 and BuildingArea<=90";
                    break;
                case 8006004:
                    sqlWhere = " BuildingArea >90 and BuildingArea <= 120";
                    break;
                case 8006005:
                    sqlWhere = " BuildingArea >120 ";
                    break;
            }
            return sqlWhere;
        }
        #endregion

        #region (更新)
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        public string DeleteCaseByCityNameAndCaseIds(string cityName, string caseIds)
        {
            FxtApi_Result result = new FxtApi_Result(1, "");
            if (string.IsNullOrEmpty(cityName))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("城市不能为空"));
                return JsonConvert.SerializeObject(result);
            }
            if (string.IsNullOrEmpty(caseIds))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("请选择要修改的案例"));
                return JsonConvert.SerializeObject(result);
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            if (!UtilityDALHelper.DeleteCaseByCityNameAndCaseIds(db, cityName, caseIds.Split(',').ConvertToInts()))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("系统异常"));
            }
            db.Close();
            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 根据多个caseId删除案例(方法创建时间:2014-2-20,创建人:曾智磊)
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseIds"></param>
        /// <returns></returns>
        public string DeleteCaseByCityIdAndCaseIds(int cityId, string caseIds)
        {
            string result = Utility.GetJson(1, "");
            if (string.IsNullOrEmpty(caseIds))
            {
                result = Utility.GetJson(0, StringHelp.EncodeField("请选择要修改的案例"));
                return result;
            }
            MSSQLDBDAL db = new MSSQLDBDAL();
            if (!UtilityDALHelper.DeleteCaseByCityIdAndCaseIds(db, cityId, caseIds.Split(',').ConvertToInts()))
            {
                result = Utility.GetJson(0, StringHelp.EncodeField("系统异常"));
            }
            db.Close();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="houseNo"></param>
        /// <param name="caseDate"></param>
        /// <param name="purposeCode"></param>
        /// <param name="buildingArea"></param>
        /// <param name="unitPrice"></param>
        /// <param name="totalPrice"></param>
        /// <param name="caseTypeCode"></param>
        /// <param name="structureCode"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="floorNumber"></param>
        /// <param name="totalFloor"></param>
        /// <param name="houseTypeCode"></param>
        /// <param name="frontCode"></param>
        /// <param name="moneyUnitCode"></param>
        /// <param name="remark"></param>
        /// <param name="areaId"></param>
        /// <param name="buildingDate"></param>
        /// <param name="fitmentCode"></param>
        /// <param name="subHouse"></param>
        /// <param name="peiTao"></param>
        /// <param name="createUser"></param>
        /// <param name="sourceName"></param>
        /// <param name="sourceLink"></param>
        /// <param name="sourcePhone"></param>
        /// <param name="ip"></param>
        /// <param name="validate"></param>
        /// <returns>{"Result":"","Message":"","Detail":""} ,属性: Result={1:成功|0:失败 }; Message=失败时返回的消息, (带UrlEncode编码); Detail=新增成功的DATCase的json字符串(带UrlEncode编码)</returns>
        public string InsertCase(int cityId, int projectId, int? buildingId, string houseNo, DateTime caseDate, int? purposeCode,
            decimal? buildingArea, decimal? unitPrice, decimal? totalPrice, int? caseTypeCode, int? structureCode, int? buildingTypeCode,
            int? floorNumber, int? totalFloor, int? houseTypeCode, int? frontCode, int? moneyUnitCode, string remark, int? areaId,
            string buildingDate, int? fitmentCode, string subHouse, string peiTao, string createUser, string sourceName, string sourceLink, string sourcePhone)
        {
            DATCase caseObj = new DATCase();
            caseObj.CityID = cityId;
            caseObj.ProjectId = projectId;
            caseObj.BuildingId = buildingId;
            caseObj.HouseNo = houseNo;
            caseObj.CaseDate = caseDate;
            caseObj.PurposeCode = purposeCode;
            caseObj.BuildingArea = buildingArea;
            caseObj.UnitPrice = unitPrice;
            caseObj.TotalPrice = totalPrice;
            caseObj.CaseTypeCode = caseTypeCode;
            caseObj.StructureCode = structureCode;
            caseObj.BuildingTypeCode = buildingTypeCode;
            caseObj.FloorNumber = floorNumber;
            caseObj.TotalFloor = totalFloor;
            caseObj.HouseTypeCode = houseTypeCode;
            caseObj.FrontCode = frontCode;
            caseObj.MoneyUnitCode = moneyUnitCode;
            caseObj.Remark = remark;
            caseObj.AreaId = areaId;
            caseObj.BuildingDate = buildingDate;
            caseObj.FitmentCode = fitmentCode;
            caseObj.SubHouse = subHouse;
            caseObj.PeiTao = peiTao;
            caseObj.Creator = createUser;
            caseObj.SourceName = sourceName;
            caseObj.SourceLink = sourceLink;
            caseObj.SourcePhone = sourcePhone;
            caseObj.Valid = 1;
            caseObj.FXTCompanyId = 25;
            caseObj.CompanyId = 25;

            FxtApi_Result result = new FxtApi_Result(1, "");
            MSSQLDBDAL db = new MSSQLDBDAL();
            string message = "";
            if (!CheckDATCase(db, caseObj, out message))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField(message));
                db.Close();
                return JsonConvert.SerializeObject(result);
            }
            if (!UtilityDALHelper.InsertCase(db, caseObj))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("系统异常或城市对应表名不存在"));
            }
            db.Close();
            result.Detail = JsonConvert.SerializeObject(caseObj.EncodeField());
            return JsonConvert.SerializeObject(result);

        }
        /// <summary>
        /// 修改案例
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="caseJson">DATCase实体json字符串(带UrlEncode编码)</param>
        /// <returns></returns>
        public string UpdateCase(int cityId, string caseJson)
        {
            FxtApi_Result result = new FxtApi_Result(1, "");
            if (string.IsNullOrEmpty(caseJson))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("请填写案例信息"));
                return JsonConvert.SerializeObject(result);
            }
            JObject jObject = JObject.Parse(caseJson);
            if (jObject == null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("请填写案例信息"));
                return JsonConvert.SerializeObject(result);
            }
            int caseId = jObject["CaseID"].Value<int>();
            MSSQLDBDAL db = new MSSQLDBDAL();
            object caseObj = UtilityDALHelper.GetUpdateCaseByCityIdAndCaseId(db, cityId, caseId);
            if (caseObj == null)
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("此信息不存在或已被删除"));
                db.Close();
                return JsonConvert.SerializeObject(result);
            }
            foreach (var _jobj in jObject)
            {
                string key = _jobj.Key;
                if (key.Equals("CaseID"))
                {
                    continue;
                }
                var propertyObj = caseObj.GetType()
                         .GetProperties()
                         .Where(pInfo =>
                             pInfo.Name.Equals(key)).FirstOrDefault();
                if (propertyObj == null)
                {
                    continue;
                }
                object propertyValue = _jobj.Value.Value<JValue>().Value;
                propertyValue = Utility.valueType(propertyObj.PropertyType, propertyValue);
                //解码
                if (propertyObj.PropertyType.FullName == "System.String")
                {
                    propertyValue = Convert.ToString(propertyValue).DecodeField();
                }
                propertyObj.SetValue(caseObj, propertyValue, null);
            }
            //db.Update(caseObj);
            if (!db.Update(caseObj))//if (!UtilityDALHelper.UpdateCase(db, caseObj))
            {
                result = new FxtApi_Result(0, StringHelp.EncodeField("系统异常或城市对应表名不存在"));
            }
            db.Close();
            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #endregion

        #region 基础数据(SYS_Code)
        /// <summary>
        /// 获取用于楼盘的所有用途
        /// </summary>
        /// <returns></returns>
        public string GetAllProjectPurposeCode()
        {
            MSSQLADODAL db = new MSSQLADODAL();
            IList<SYSCode> list = UtilityDALHelper.GetAllProjectPurposeCodeByCode(db);
            list = list.EncodeField<SYSCode>();
            string jsonStr = JsonConvert.SerializeObject(list);
            return jsonStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSYSCodeByID(int id)
        {
            MSSQLADODAL db = new MSSQLADODAL();
            IList<SYSCode> list = UtilityDALHelper.GetADOListSYSCODE(db, id);
            list = list.EncodeField<SYSCode>();
            string jsonStr = JsonConvert.SerializeObject(list);
            return jsonStr;
        }
        /// <summary>
        /// 获取别墅相关的住宅用途
        /// </summary>
        /// <returns></returns>
        public string GetPurposeTypeCodeVillaType()
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<SYSCode> list = UtilityDALHelper.GetPurposeTypeCodeVillaType(db);
            db.Close();
            list = list.EncodeField<SYSCode>();
            string jsonStr = JsonConvert.SerializeObject(list);
            return jsonStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetSYSCodeByCode(int code)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            SYSCode _code = UtilityDALHelper.GetSYSCodeByCode(db, code);
            db.Close();
            string jsonStr = JsonConvert.SerializeObject(_code.EncodeField());
            return jsonStr;
        }


        #endregion

        #region 楼盘均价(DAT_ProjectAvgPrice)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectIds">逗号分隔的多个projectId</param>
        /// <param name="cityId"></param>
        /// <param name="dateRange">计算范围</param>
        /// <param name="avgPriceDates">逗号分隔的多个avgPriceDate</param>
        /// <returns>IList DATProjectAvgPrice  结构json字符串</returns>
        public string GetProjectAvgPriceByProjectIdsAndCityIdAndDateRangeAndAvgPriceDates(string projectIds, int cityId, int dateRange, string avgPriceDates)
        {
            IList<DATProjectAvgPrice> list = new List<DATProjectAvgPrice>();
            int[] _projectIds = string.IsNullOrEmpty(projectIds) ? null : projectIds.Split(',').ConvertToInts();
            string[] _dates = string.IsNullOrEmpty(avgPriceDates) ? null : avgPriceDates.Split(',');
            if (_projectIds == null || _projectIds.Length < 1 || _dates == null || _dates.Length < 1)
            {
                return JsonConvert.SerializeObject(list);
            }
            for (int i = 0; i < _dates.Length; i++)
            {
                _dates[i] = "'" + _dates[i] + "'";
            }
            string sql = string.Format("{0} FxtCompanyId=25 and CityId={1} and ProjectId in ({2}) and DateRange={3} " +
                    " and  AvgPriceDate in ({4})", Utility.GetMSSQL_SQL(typeof(DATProjectAvgPrice), Utility.DATProjectAvgPrice),
                    cityId, _projectIds.ConvertToString(), dateRange, _dates.ConvertToString());

            MSSQLDBDAL db = new MSSQLDBDAL();
            list = db.GetCustomSQLQueryList<DATProjectAvgPrice>(sql).ToList();
            db.Close();
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 查询DATProjectAvgPrice,根据用途,建筑类型,面积段,日期计算区间
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="purposeCode"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="buildingAreaCode"></param>
        /// <param name="date"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public string GetProjectAvgPriceByProjectIdAndCityIdAndBy(int projectId, int cityId, int purposeCode, int? buildingTypeCode, int? buildingAreaCode, string date, int dateRange)
        {
            MSSQLDBDAL db = new MSSQLDBDAL();
            IList<DATProjectAvgPrice> list = db.GetListCustom<DATProjectAvgPrice>(
                    (Expression<Func<DATProjectAvgPrice, bool>>)
                            (p =>
                                p.FxtCompanyId == 25
                                && p.CityId == cityId
                                && p.ProjectId == projectId
                                && p.DateRange == dateRange
                                && p.PurposeType == purposeCode
                                && p.BuildingTypeCode == buildingTypeCode
                                && p.BuildingAreaType == buildingAreaCode
                                && p.AvgPriceDate == Utility.GetDateTimeMoths(date, 1, "yyyyMM"))
                    ).ToList();
            db.Close();
            string listJson = JsonConvert.SerializeObject(list);
            return Utility.GetJson(1, "", listJson);
        }

        #endregion

        #region (Privi_Company_ShowData)
        /// <summary>
        /// 更加fxtCompanyId获取能获取到数据的企业
        /// </summary>
        /// <param name="fxtCompanyId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public string GetPriviCompanyShowDataByCompanyIdAndCityId(int fxtCompanyId, int cityId)
        {

            MSSQLDBDAL db = new MSSQLDBDAL();
            PriviCompanyShowData obj = UtilityDALHelper.GetPriviCompanyShowDataByCompanyIdAndCityId(db, fxtCompanyId, cityId);
            db.Close();
            return Utility.GetJson(1, "", obj);
        }
        /// <summary>
        /// 获得所有银行信息
        /// </summary>
        /// <returns></returns>
        public string GetPriviCompanyAllBank()
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL();
            string sql = string.Format("{0} FK_CompanyTypeCode in (2001013,2001014)",
                Utility.GetMSSQL_SQL(typeof(PriviCompany), Utility.PriviCompany));
            List<PriviCompany> list = mssqlado.GetList<PriviCompany>(sql);
            if (list.Count > 0)
                return Utility.GetJson(1, "Success", list);
            return Utility.GetJson(0, "");
        }
        #endregion

        #region(内部方法)
        /// <summary>
        /// 验证创建网络名
        /// </summary>
        /// <param name="_mssql"></param>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool CheckSYSProjectMatch(MSSQLDBDAL _mssql, SYSProjectMatch obj, out string message)
        {
            message = "";
            if (string.IsNullOrEmpty(obj.NetName))
            {
                message = "填写的网络名称";
                return false;
            }
            if (obj.ProjectNameId == null || obj.ProjectNameId.Value < 1)
            {
                message = "填写楼盘ID";
                return false;
            }
            if (obj.CityId == null || obj.CityId.Value < 1)
            {
                message = "选择正确的城市ID";
                return false;
            }
            SYSCity city = UtilityDALHelper.GetCityById(_mssql, obj.CityId.Value);
            if (city == null)
            {
                message = "选择正确的城市";
                return false;
            }
            IList<SYSProjectMatch> list = UtilityDALHelper.GetListSYSProjectMatchByCityIdAndNetName_Fxt(_mssql, obj.CityId.Value, obj.NetName, 1, 1);
            if (list != null && list.Count > 1)
            {
                message = "此网络名关联已经与其他楼盘关联";
                return false;
            }
            return true;
        }

        bool CheckDATCase(MSSQLDBDAL db, DATCase caseObj, out string message)
        {
            message = "";
            #region (验证非空)
            if (caseObj == null)
            {
                message = "请填写案例信息";
                return false;
            }
            if (caseObj.CityID == null)
            {
                message = "请选择城市";
                return false;
            }
            if (caseObj.PurposeCode == null)
            {
                message = "请填写用途";
                return false;
            }
            if (caseObj.BuildingArea == null)
            {
                message = "请填写面积";
                return false;
            }
            if (caseObj.UnitPrice == null)
            {
                message = "请填写单价";
                return false;
            }
            if (caseObj.TotalPrice == null)
            {
                message = "请填写总价";
                return false;
            }
            if (caseObj.CaseTypeCode == null)
            {
                message = "请填写案例类型";
                return false;
            }
            if (caseObj.CaseTypeCode == null)
            {
                message = "请填写案例类型";
                return false;
            }
            #endregion

            SYSCityTable table = UtilityDALHelper.GetCityTable(db, Convert.ToInt32(caseObj.CityID));
            if (table == null)
            {
                message = "城市不存在";
                return false;
            }
            string projectTableName = table.ProjectTable;
            DATProject proj = UtilityDALHelper.GetProjectByTableNameAndProjectId(db, projectTableName, caseObj.ProjectId);
            if (proj == null)
            {
                message = "楼盘不存在";
                return false;
            }
            if (caseObj.AreaId != null)
            {
                SYSArea area = UtilityDALHelper.GetSYSAreaById(db, Convert.ToInt32(caseObj.AreaId));
                if (area == null)
                {
                    message = "行政区不存在";
                    return false;
                }
            }
            if (!CheckDATCaseCode(db, caseObj, out message))
            {
                return false;
            }
            return true;
        }

        bool CheckDATCaseCode(MSSQLDBDAL db, DATCase caseObj, out string message)
        {
            message = "";
            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("PurposeCode", "" + Utility.CodeID_2 + ",用途");
            list.Add("CaseTypeCode", "" + Utility.CodeID_3 + ",案例类型");
            list.Add("StructureCode", "" + Utility.CodeID_4 + ",结构");
            list.Add("BuildingTypeCode", "" + Utility.CodeID_5 + ",建筑类型");
            list.Add("HouseTypeCode", "" + Utility.CodeID_6 + ",户型");
            list.Add("FrontCode", "" + Utility.CodeID_7 + ",朝向");
            list.Add("MoneyUnitCode", "" + Utility.CodeID_8 + ",币种");
            IEnumerator<KeyValuePair<string, string>> e = list.GetEnumerator();
            SYSCode codeObj = null;
            while (e.MoveNext())
            {
                var propertyObj = caseObj.GetType().GetProperties()
                               .Where(pInfo => pInfo.Name.Equals(e.Current.Key)).FirstOrDefault();
                if (propertyObj == null) continue;
                int codeId = Convert.ToInt32(e.Current.Value.Split(',')[0]);
                string text = e.Current.Value.Split(',')[1];
                object val = propertyObj.GetValue(caseObj, null);
                if (val != null)
                {
                    int code = Convert.ToInt32(val);
                    codeObj = UtilityDALHelper.GetSYSCodeByCodeAndID(db, code, codeId);
                    if (codeObj == null)
                    {
                        message = text + "不存在";
                        return false;
                    }
                }

            }
            return true;
        }

        #endregion

        #region 公共入口

        public object Entrance(string date, string code, string type, string name, string value)
        {
            EntranceHelper eHelper = new EntranceHelper();
            if (!eHelper.GetCode(date).Equals(code))
            {
                return Utility.GetJson(0, "验证码错误!");
            }
            else
            {
                MatchClass mc = eHelper.GetMatchClass(type);
                if (mc == null)
                {
                    return Utility.GetJson(0, "未找到对象!");
                }
                object objClass = System.Reflection.Assembly.Load(mc.Library).CreateInstance(mc.ClassName);
                if (objClass == null)
                {
                    return Utility.GetJson(0, "未找到对象!");
                }
                MethodInfo method = objClass.GetType().GetMethod(name);
                if (method == null)
                {
                    return Utility.GetJson(0, "未找到对象!");
                }
                try
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    object[] objvalue = null;
                    JObject jobject = new JObject();
                    if (!Utils.IsNullOrEmpty(value))
                    {
                        jobject = JObject.Parse(value);
                    }
                    if (jobject.Count != parameterInfos.Length)
                    {
                        return Utility.GetJson(0, "未找到对象!");
                    }
                    objvalue = new object[parameterInfos.Length];
                    for (int i = 0; i < parameterInfos.Length; i++)
                    {
                        ParameterInfo parameterInfo = parameterInfos[i];
                        if (jobject[parameterInfo.Name] == null)
                        {
                            return Utility.GetJson(0, "未找到对象!");
                        }
                        if (jobject[parameterInfo.Name].Type == JTokenType.Null)
                        {
                            objvalue[i] = null;
                            continue;
                        }
                        objvalue[i] = Utility.valueType(parameterInfo.ParameterType, jobject.Value<JValue>(parameterInfo.Name).Value);
                    }
                    return method.Invoke(objClass, objvalue);
                }
                catch (Exception exe)
                {
                    log.Error(exe.Message, exe);
                    return Utility.GetJson(0, exe.Message);
                }
            }
        }

        #endregion

    }
    /// <summary>
    /// 返回结果统一格式(主要用于数据更新操作)
    /// </summary>
    public class FxtApi_Result
    {
        /// <summary>
        /// 结果,1:成功,0:失败
        /// </summary>
        public int Result
        {
            get;
            set;
        }
        /// <summary>
        /// 消息结果
        /// </summary>
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// 返回的其他内容
        /// </summary>
        public string Detail
        {
            get;
            set;
        }
        public FxtApi_Result()
        { }
        public FxtApi_Result(int result, string message, string detail)
        {
            this.Result = result;
            this.Message = message;
            this.Detail = detail;
        }
        public FxtApi_Result(int result, string message)
        {
            this.Result = result;
            this.Message = message;
        }

    }

    public class FxtApi_ResultPageList
    {
        public int Count
        {
            get;
            set;
        }
        public string ObjJson
        {
            get;
            set;
        }
        public FxtApi_ResultPageList(int count, object obj)
        {
            this.Count = count;
            this.ObjJson = JsonConvert.SerializeObject(obj);
        }
    }
}
