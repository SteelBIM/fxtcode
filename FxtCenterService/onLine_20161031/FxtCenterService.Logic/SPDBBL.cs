using CAS.Common;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtCenterService.Logic
{
    public class SPDBBL
    {
        /// <summary>
        /// 浦发楼盘获取方法
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cityzipcode">城市国标码</param>
        /// <param name="key">楼盘关键字</param>
        /// <returns></returns>
        public static List<object> GetProjectList(SearchBase search, string key)
        {
            List<object> listResult = new List<object>();
            DataTable dt = SPDBDA.GetProjectList(search, key);
            foreach (DataRow item in dt.Rows)
            {
                int areaID;
                int purposeCode;
                int buildingTypeCode;
                DateTime builddate;
                DateTime opendate;
                string builddateStr = string.Empty;
                string opendateStr = string.Empty;
                int.TryParse(item["AreaID"].ToString(), out areaID);
                int.TryParse(item["PurposeCode"].ToString(), out purposeCode);
                int.TryParse(item["BuildingTypeCode"].ToString(), out buildingTypeCode);
                if (DateTime.TryParse(item["EndDate"].ToString(), out builddate))
                {
                    builddateStr = builddate.ToString("yyyy年");
                }
                if (DateTime.TryParse(item["SaleDate"].ToString(), out opendate))
                {
                    opendateStr = opendate.ToString("yyyy-MM-dd");
                }
                listResult.Add(new
                {
                    ProjectId = int.Parse(item["ProjectId"].ToString()),
                    CityID = int.Parse(item["CityID"].ToString()),
                    CityZipcode = item["CityZipcode"].ToString(),
                    CityName = item["CityName"].ToString(),
                    AreaID = areaID,
                    AreaName = item["AreaName"].ToString(),
                    ProjectName = item["ProjectName"].ToString(),
                    category = "residence",
                    PurposeCode = purposeCode,
                    PurposeCodeName = item["PurposeCodeName"].ToString(),
                    BuildingTypeCode = buildingTypeCode,
                    BuildingTypeCodeName = item["BuildingTypeCodeName"].ToString(),
                    Address = item["Address"].ToString(),
                    block = item["SubAreaName"].ToString(),
                    builddate = builddateStr,
                    opendate = opendateStr
                });
            }
            return listResult;
        }

        /// <summary>
        /// 浦发楼盘案例获取方法
        /// </summary>
        /// <param name="search"></param>
        /// <param name="projectid">楼盘ID</param>
        /// <param name="buildingarea">房屋面积</param>
        /// <param name="purposecode">案例用途Code</param>
        /// <param name="casetypecode">案例类型Code</param>
        /// <param name="floorno">所在楼层</param>
        /// <param name="houseno">楼房号</param>
        /// <param name="buildingtypecode">建筑类型</param>
        /// <returns></returns>
        public static string GetProjectCaseList(
            SearchBase search,
            int projectid,
            decimal buildingarea,
            List<int> purposecodes,
            int casetypecode,
            int floorno,
            string houseno,
            int buildingtypecode,
            int housetypecode)
        {
            List<object> listResult = new List<object>();
            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DataTable dt = SPDBDA.GetProjectCaseList(search,
                projectid,
                buildingarea,
                purposecodes,
                casetypecode,
                floorno,
                houseno,
                buildingtypecode,
                housetypecode,
                time.AddMonths(-6),
                time
            );
            foreach (DataRow item in dt.Rows)
            {
                string caseDateStr = "";
                DateTime caseDate;
                string saleDateStr = "";
                DateTime saleDate;
                string endDateStr = "";
                DateTime endDate;
                if (item["EndDate"].ToString().Length < 8 && item["EndDate"].ToString().Length>=4)
                {
                    endDateStr = item["EndDate"].ToString().Substring(0, 4) + "年";
                }
                else if (DateTime.TryParse(item["EndDate"].ToString(),out endDate))
                {
                    endDateStr = endDate.ToString("yyyy年");
                }
                if (DateTime.TryParse(item["CaseDate"].ToString(),out caseDate))
                {
                    caseDateStr = caseDate.GetDateTimeFormats('s')[0];
                }
                if (DateTime.TryParse(item["SaleDate"].ToString(), out saleDate))
                {
                    saleDateStr = saleDate.GetDateTimeFormats('s')[0];
                }
                listResult.Add(new {
                    CityID = int.Parse(item["CityID"].ToString()),
                    CityZipcode = item["CityZipcode"].ToString(),
                    CityName = item["CityName"].ToString(),
                    AreaID = int.Parse(item["AreaID"].ToString()),
                    AreaName = item["AreaName"].ToString(),
                    SubAreaId = int.Parse(item["SubAreaId"].ToString()),
                    SubAreaName = item["SubAreaName"].ToString(),
                    ProjectId = int.Parse(item["ProjectId"].ToString()),
                    ProjectName = item["ProjectName"].ToString(),
                    BuildingArea = Math.Round(decimal.Parse(item["BuildingArea"].ToString()), 2),
                    BuildingTypeCode = int.Parse(item["BuildingTypeCode"].ToString()),
                    BuildingTypeCodeName = item["BuildingTypeCodeName"].ToString(),
                    FrontCode = int.Parse(item["FrontCode"].ToString()),
                    FrontCodeName = item["FrontCodeName"].ToString(),
                    ZhuangXiu = item["ZhuangXiu"].ToString(),
                    TotalFloor = int.Parse(item["TotalFloor"].ToString()),
                    HouseTypeCode = int.Parse(item["HouseTypeCode"].ToString()),
                    HouseTypeCodeName = item["HouseTypeCodeName"].ToString(),
                    CaseDate = caseDateStr,
                    TotalPrice = Math.Round(decimal.Parse(item["TotalPrice"].ToString()), 2),
                    UnitPrice = Math.Round(decimal.Parse(item["UnitPrice"].ToString()), 2),
                    SourceName = item["SourceName"].ToString(),
                    FloorNumber = int.Parse(item["FloorNumber"].ToString()),
                    RemainYear = int.Parse(item["RemainYear"].ToString()),
                    Address = item["Address"].ToString(),
                    PurposeCode = int.Parse(item["PurposeCode"].ToString()),
                    PurposeCodeName = item["PurposeCodeName"].ToString(),
                    SaleDate = saleDateStr,
                    AveragePrice = Math.Round(decimal.Parse(item["AveragePrice"].ToString()), 2),
                    EndDate = endDateStr,
                    ManagerPrice = item["ManagerPrice"].ToString(),
                    CubageRate = Math.Round(decimal.Parse(item["CubageRate"].ToString()), 2),
                    GreenRate = Math.Round(decimal.Parse(item["GreenRate"].ToString()), 2),
                    X = Math.Round(decimal.Parse(item["X"].ToString()), 2),
                    Y = Math.Round(decimal.Parse(item["Y"].ToString()), 2)
               });
            }
            return listResult.ToJson();
        }
    }
}
