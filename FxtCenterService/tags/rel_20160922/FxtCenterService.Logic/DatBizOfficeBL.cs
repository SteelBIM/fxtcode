using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;
using CAS.Entity.FxtDataCenter;

namespace FxtCenterService.Logic
{
    public class DatBizOfficeBL
    {
        public static List<DatCaseBiz> GetCaseListBiz(SearchBase search, string projectname, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator,string structurecodename)
        {
            var result = DatBizOfficeDA.GetCaseListBiz(search, projectname, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);
            return result;
        }
        public static List<DatCaseBiz> GetCaseListBiz_MCAS(SearchBase search, int areaid, int subareaid, int casetype, DateTime casestatime, DateTime caseendtime, string projectname, decimal? buildstaarea, decimal? buildendarea, decimal? pricesta, decimal? priceend)
        {
            var result = DatBizOfficeDA.GetCaseListBiz_MCAS(search, areaid, subareaid, casetype, casestatime, caseendtime, projectname, buildstaarea, buildendarea, pricesta, priceend);
            return result;
        }
        public static DataTable GetCaseInfoBiz(SearchBase search, int[] ids)
        {
            var result = DatBizOfficeDA.GetCaseInfoBiz(search, ids);
            return result;
        }
        public static List<DatCaseBiz> GetCaseInfoBiz_MCAS(SearchBase search, int id)
        {
            var result = DatBizOfficeDA.GetCaseInfoBiz_MCAS(search, id);
            return result;
        }
        public static List<DatCaseOff> GetCaseListOffice(SearchBase search, string projectname, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, int officeType, int purposeCode, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator,string structurecodename)
        {
            var result = DatBizOfficeDA.GetCaseListOffice(search, projectname, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, officeType, purposeCode, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);
            return result;
        }
        public static List<DatCaseOff> GetCaseListOffice_MCAS(SearchBase search, int areaid, int subareaid, int casetype, DateTime casestatime, DateTime caseendtime, string projectname, decimal? buildstaarea, decimal? buildendarea, decimal? pricesta, decimal? priceend)
        {
            var result = DatBizOfficeDA.GetCaseListOffice_MCAS(search, areaid, subareaid, casetype, casestatime, caseendtime, projectname, buildstaarea, buildendarea, pricesta, priceend);
            return result;
        }
        public static DataTable GetCaseInfoOffice(SearchBase search, int[] ids)
        {
            var result = DatBizOfficeDA.GetCaseInfoOffice(search, ids);
            return result;
        }
        public static List<DatCaseLand> GetCaseListLand(SearchBase search, string landno, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, string landPurposeDesc, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate)
        {
            var result = DatBizOfficeDA.GetCaseListLand(search, landno, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, landPurposeDesc, areaid, subareaid, startCaseDate, endCaseDate);
            return result;
        }
        public static DataTable GetCaseInfoLand(SearchBase search, int[] ids)
        {
            var result = DatBizOfficeDA.GetCaseInfoLand(search, ids);
            return result;
        }
        public static string GetCaseInfo(SearchBase search, int[] ids)
        {
            var result = DatBizOfficeDA.GetCaseInfo(search, ids);
            return result.ToJson();
        }
        public static List<DatCaseIndustry> GetCaseListIndustry(SearchBase search, string projectName, decimal minBuildingArea, decimal maxBuildingArea, decimal minUnitPrice, decimal maxUnitPrice, int purposeCode, int caseTypeCode, int areaid, int subareaid, DateTime startCaseDate, DateTime endCaseDate, int structurecode, int iselevator, string structurecodename)
        {
            var result = DatBizOfficeDA.GetCaseListIndustry(search, projectName, minBuildingArea, maxBuildingArea, minUnitPrice, maxUnitPrice, purposeCode, caseTypeCode, areaid, subareaid, startCaseDate, endCaseDate, structurecode, iselevator, structurecodename);
            return result;
        }
        public static string GetCaseInfoIndustry(SearchBase search, int[] ids)
        {
            var result = DatBizOfficeDA.GetCaseInfoIndustry(search, ids);
            return result.ToJson();
        }
    }
}
