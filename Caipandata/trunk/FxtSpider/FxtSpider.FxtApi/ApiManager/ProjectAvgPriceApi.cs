using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.FxtApi.Fxt.Wcf;
using FxtSpider.FxtApi.Model;
using FxtSpider.Dll.Manager;
using FxtSpider.FxtApi.Common;
using FxtSpider.Common;
using FxtSpider.FxtApi.Fxt.Api;
using Newtonsoft.Json.Linq;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;

namespace FxtSpider.FxtApi.ApiManager
{
    /// <summary>
    /// 均价api
    /// </summary>
    public static class ProjectAvgPriceApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(ProjectAvgPriceApi));

        #region 更新
        public static FxtApi_DATProjectAvgPrice ResetCross(int projectId, int fxtCityId, int purposeTypeCode, int buildingTypeCode, int buildingAreaType, string date,out string message, FxtAPIClientExtend _fxtApi = null)
        {
            message = "";
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            FxtApi_DATProjectAvgPrice avgPrice = null;
            try
            {

                string name = "ResetCrossBy";
                var para = new { projectId = projectId, cityId = fxtCityId, purposeTypeCode = purposeTypeCode, buildingTypeCode = buildingTypeCode, buildingAreaType = buildingAreaType, date = date };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                if (!string.IsNullOrEmpty(jsonStr))
                {
                    avgPrice = FxtApi_DATProjectAvgPrice.ConvertToObj(jsonStr);
                }
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                message = "系统异常";
                log.Error("ResetCross(int projectId, int fxtCityId, int purposeTypeCode, int buildingTypeCode, int buildingAreaType, string date, FxtAPIClientExtend _fxtApi = null)", ex);
                return null;
            }
            fxtApi.Abort();
            return avgPrice;

        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取楼盘平均交叉值
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fxtCityId"></param>
        /// <param name="codeType">当前用途(1002001普通住宅 or 1002027别墅)</param>
        /// <param name="date"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_DATProjectAvgPrice> GetCross(int projectId, int fxtCityId, int codeType, string date,FxtAPIClientExtend _fxtApi=null)
        {
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "Cross";
            var para = new { projectId = projectId, cityId = fxtCityId, codeType = codeType,  date = date };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                return list;
            }
            list = FxtApi_DATProjectAvgPrice.ConvertToObjList(jsonStr);
            fxtApi.Abort();
            return list;
        }
        public static List<FxtApi_DATProjectAvgPrice> GetCrossByCompanyId(int projectId, int fxtCityId, int codeType, string date, int fxtCompanyId, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "CrossByFxtCompanyId";
            var para = new { projectId = projectId, cityId = fxtCityId, codeType = codeType, date = date, fxtCompanyId = fxtCompanyId };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                return list;
            }
            list = FxtApi_DATProjectAvgPrice.ConvertToObjList(jsonStr);
            fxtApi.Abort();
            return list;
        }
        public static List<FxtApi_DATProjectAvgPrice> GetCross(int projectId, int fxtCityId, int[] codeTypes, string[] dates, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            if (codeTypes == null || codeTypes.Length < 1)
            {
                codeTypes = new int[] { 1002001, 1002027 };
            }
            try
            {
                foreach (int code in codeTypes)
                {
                    foreach (string date in dates)
                    {
                        List<FxtApi_DATProjectAvgPrice> _list = GetCross(projectId, fxtCityId, code, date, _fxtApi: fxtApi);
                        if (_list != null && _list.Count > 0)
                        {
                            list.AddRange(_list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("GetCross(int projectId, int fxtCityId, int[] codeTypes, string[] dates, FxtAPIClientExtend _fxtApi = null)", ex);
                fxtApi.Abort();
                return list;
            }
            fxtApi.Abort();
            return list;
        }
        public static List<FxtApi_DATProjectAvgPrice> GetCross(int[] projectIds, int fxtCityId, int[] codeTypes, string[] dates, FxtAPIClientExtend _fxtApi = null)
        {
            int[] villaTypeCode= SysCodeApi.PurposeTypeCodeVillaType.ToArray();
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            if (codeTypes == null || codeTypes.Length < 1)
            {
                codeTypes = new int[] { SysCodeApi.Code1, SysCodeApi.Code2 };
            }
            List<string> stringList = new List<string>();
            foreach (string str in dates)
            {
                stringList.Add(str.Replace("-", ""));
            }
            list = GetProjectAvgPriceByProjectIdsAndCityIdAndDateRangeAndAvgPriceDates(projectIds, fxtCityId, 3, stringList.ToArray(), _fxtApi: fxtApi);
            try
            {
                foreach (int projectId in projectIds)
                {
                    foreach (int code in codeTypes)
                    {
                        foreach (string date in dates)
                        {
                            Func<FxtApi_DATProjectAvgPrice, bool> where = p => p.ProjectId == projectId && p.PurposeType == code && p.AvgPriceDate == date.Replace("-", "");
                            if (code == 1002027)//如果为别墅
                            {
                                where = p => p.ProjectId == projectId && villaTypeCode.Contains(Convert.ToInt32(p.PurposeType)) && p.AvgPriceDate == date.Replace("-", "");
                            }
                            List<FxtApi_DATProjectAvgPrice> _list = list.Where(where).ToList();
                            if (_list == null || _list.Count < 1)
                            {
                                _list = GetCross(projectId, fxtCityId, code, date, _fxtApi: fxtApi);
                                if (_list != null && _list.Count > 0)
                                {
                                    list.AddRange(_list);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("GetCross(int[] projectIds, int fxtCityId, int[] codeTypes, string[] dates, FxtAPIClientExtend _fxtApi = null)", ex);
                fxtApi.Abort();
                return list;
            }
            fxtApi.Abort();

            return list;

        }
        public static List<FxtApi_DATProjectAvgPrice> GetProjectAvgPriceByProjectIdsAndCityIdAndDateRangeAndAvgPriceDates(int[] projectIds, int fxtCityId, int dateRange, string[] avgPriceDates, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);

            string name = "GetProjectAvgPriceByProjectIdsAndCityIdAndDateRangeAndAvgPriceDates";
            var para = new { projectIds = projectIds.ConvertToString(), cityId = fxtCityId, dateRange = dateRange, avgPriceDates = avgPriceDates.ConvertToString() };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

            if (string.IsNullOrEmpty(jsonStr))
            {
                return list;
            }
            list = FxtApi_DATProjectAvgPrice.ConvertToObjList(jsonStr);
            fxtApi.Abort();
            return list;
        }
        /// <summary>
        /// 根据条件获取DATProjectAvgPrice
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="fxtCityId"></param>
        /// <param name="purposeCode"></param>
        /// <param name="buildingTypeCode"></param>
        /// <param name="buildingAreaCode"></param>
        /// <param name="date"></param>
        /// <param name="dateRange"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_DATProjectAvgPrice> GetProjectAvgPriceByProjectIdAndCityIdAndBy(int projectId, int fxtCityId, int purposeCode, int? buildingTypeCode, int? buildingAreaCode, string date, int dateRange, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProjectAvgPriceByProjectIdAndCityIdAndBy";
                var para = new { projectId = projectId, cityId = fxtCityId, purposeCode = purposeCode, buildingTypeCode = buildingTypeCode, buildingAreaCode = buildingAreaCode, date = date, dateRange = dateRange };

                string json = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
                FxtApi_PublicResult result = FxtApi_PublicResult.ConvertToObj(json);
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return list;
                }
                list = FxtApi_DATProjectAvgPrice.ConvertToObjList(Convert.ToString(result.data));
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                log.Error("GetProjectAvgPriceByProjectIdAndCityIdAndBy", ex);
                fxtApi.Abort();
            }
            return list;
        }

        public static FxtApi_DATProjectAvgPrice GetProjectAvgNowMonths(int projectId, int fxtCityId, int purposeCode, string date, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_DATProjectAvgPrice> list = new List<FxtApi_DATProjectAvgPrice>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                list = GetProjectAvgPriceByProjectIdAndCityIdAndBy(projectId, fxtCityId, purposeCode, null, null, date, 1, _fxtApi: fxtApi);                
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                log.Error("GetProjectAvgNowMoths", ex);
                fxtApi.Abort();
            }
            if (list == null || list.Count < 1)
            {
                return null;
            }
            return list[0];
        }

        public static int GetCrossProjectByCodeType(int projectId, int fxtCityId, int codeType, string date, FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);


            string name = "CrossProjectByCodeType";
            var para = new { projectId = projectId, cityId = fxtCityId, codeType = codeType, date = date };
            string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
            fxtApi.Abort();
            FxtApi_PublicResult result = FxtApi_PublicResult.ConvertToObj(jsonStr);
            if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
            {
                return 0;
            }
            return Convert.ToInt32(result.data);
        }

        #endregion
    }
}
