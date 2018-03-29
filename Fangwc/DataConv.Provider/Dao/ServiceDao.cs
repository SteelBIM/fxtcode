using DataConv.Library;
using DataConv.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace DataConv.Provider.Dao
{
    public class ServiceDao
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ServiceDao));

        public string CenterUrl = Convert.ToString(ConfigurationManager.ConnectionStrings["CenterUrl"]);
        public string CaseUrl = Convert.ToString(ConfigurationManager.ConnectionStrings["ProjectUrl"]);


        /// <summary>
        ///  查询城市信息
        /// </summary>
        /// <returns></returns>
        public List<Model.City> QueryCityList()
        {
            List<City> list = new List<City>();
            string sql = @"SELECT 
	t1.CityId,t1.CityName,t1.ProvinceId,t2.ProjectTable,t2.CaseTable
FROM [FxtDataCenter].[dbo].[SYS_City] t1
inner join [FxtDataCenter].dbo.SYS_City_Table t2
on t1.CityId = t2.CityId
";
            DataTable dt_city = DBUtils.getData(CenterUrl, sql);

            list = ConvUtils<City>.ConvertToList(dt_city);

            return list;
        }

        /// <summary>
        /// 分页查询城市数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Model.City> PagingQueryCityList(int page)
        {
            int pageSize = 50;
            List<City> list = new List<City>();
            string sql = @"
SELECT top "+pageSize+@" t1.CityId,t1.CityName,t1.ProvinceId,t2.ProjectTable,t2.CaseTable
FROM [FxtDataCenter].[dbo].[SYS_City] t1 inner join [FxtDataCenter].dbo.SYS_City_Table t2 on t1.CityId = t2.CityId
WHERE t1.CityId NOT IN
(
 SELECT top "+(page - 1)*pageSize+@" t1.CityId FROM [FxtDataCenter].[dbo].[SYS_City] t1 inner join [FxtDataCenter].dbo.SYS_City_Table t2 on t1.CityId = t2.CityId
)
";
            DataTable dt_city = DBUtils.getData(CenterUrl, sql);

            list = ConvUtils<City>.ConvertToList(dt_city);
            return list;
        }

        /// <summary>
        /// 查询城市对应的行政区
        /// </summary>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public List<Model.Area> QueryAreaList(int cityID)
        {
            List<Area> list = new List<Area>();
            string sql = "SELECT [AreaId],[AreaName] FROM [FxtDataCenter].[dbo].[SYS_Area] where CityId = " + cityID;
            DataTable dt_area = DBUtils.getData(CenterUrl, sql);
            logger.Debug("QueryAreaList SQL:" + sql);
            list = ConvUtils<Area>.ConvertToList(dt_area);
            return list;
        }

        /// <summary>
        /// 查询城市所有楼盘名称信息
        /// </summary>
        /// <param name="cityID"></param>
        /// <returns></returns>
        public List<Model.DataProject> QueryDataProjectList(int cityID, int AreaID, 
            string tableName)
        {
            List<DataProject> list = new List<DataProject>();
            string sql = "SELECT [ProjectId],[ProjectName],[OtherName],[PinYin],[PinYinAll] FROM [FXTProject]." + tableName + @" where CityId = " + cityID;
            logger.DebugFormat("QueryDataProjectList CityID={0}, AreaID={1}, tname={2}, SQL={3}",
                cityID, AreaID, tableName, sql);
            DataTable data = DBUtils.getData(CaseUrl, sql);
            list = ConvUtils<DataProject>.ConvertToList(data);
            return list;
        }

        /// <summary>
        /// 分页查询城市所有楼盘名称信息
        /// </summary>
        /// <param name="CityID"></param>
        /// <param name="AreaID"></param>
        /// <param name="tName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Model.DataProject> PagingQueryProjectList(int CityID, int AreaID, 
            string tName, int page)
        {
            List<DataProject> list = new List<DataProject>();
            string sql = @"SELECT TOP "+100+@" 
[ProjectId],
[ProjectName],
[OtherName],
[PinYin],
[PinYinAll]
FROM [FXTProject].[#tName#] WHERE ProjectId NOT IN
(SELECT TOP "+(page - 1)*100+@" [ProjectId] FROM [FXTProject].[#tName#] WHERE CityID = [#CityID#])
AND CityID = [#CityID#]";

            sql = sql.Replace("[#tName#]", tName).Replace("[#CityID#]", Convert.ToString(CityID));
            DataTable data = DBUtils.getData(CaseUrl, sql);
            list = ConvUtils<DataProject>.ConvertToList(data);
            return list;
        }

        /// <summary>
        /// 查询字段信息映射
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<Model.SysCode> QuerySysCodeList(string strWhere)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            string sql = "SELECT [Code],[CodeName] FROM [FxtDataCenter].[dbo].[SYS_Code] where 1 = 1 ";

            if (!strWhere.Equals(""))
            {
                sql = sql + strWhere;
                logger.DebugFormat("QuerySysCodeList SQL: {0}", sql);
            }
            else
            {
                return null;
            }

            return ConvUtils<SysCode>.ConvertToList(DBUtils.getData(CenterUrl, sql));
        }

        /// <summary>
        /// 模拟批量插入数据
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public int BatchInsertDataCase(Model.DataCase[] dc, string tableName)
        {
            logger.InfoFormat("BatchInsertDataCase time={0}, tName={1}, insert Rows={2}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tableName, dc.Length);
            string Creator = "xq";
            string Remark = "析取";
            int Valid = 1;
            int FXTCompanyId = 25;

            if (dc.Length == 0)
            {
                return -1;
            }
            string select_sql = "";
            foreach (Model.DataCase dcitem in dc)
            {
                if (dcitem != null)
                {
                    if (!select_sql.Equals(""))
                    {
                        select_sql = select_sql + " union all ";
                    }
                    select_sql = select_sql + " select " + dcitem.ProjectId + ",'" + dcitem.CaseDate + "'," + dcitem.PurposeCode + ",[#FloorNumber#]," + dcitem.BuildingArea + "," + dcitem.FrontCode + "," + dcitem.UnitPrice + "," + dcitem.MoneyUnitCode + "," + dcitem.CaseTypeCode + "," + dcitem.StructureCode + "," + dcitem.BuildingTypeCode + "," + dcitem.HouseTypeCode + ",'" + Creator + "','" + Remark + "'," + dcitem.UnitPrice * dcitem.BuildingArea + "," + dcitem.CityID + "," + Valid + "," + FXTCompanyId + ",[#TotalFloor#],[#RemainYear#],[#Depreciation#],'" + dcitem.SourceName + "','" + dcitem.SourceLink + "','" + dcitem.SourcePhone + "'," + dcitem.AreaId + ",'" + dcitem.AreaName + "','" + dcitem.BuildingDate + "','" + dcitem.ZhuangXiu + "','" + dcitem.PeiTao + "'";

                    if (dcitem.FloorNumber == null)
                    {
                        select_sql = select_sql.Replace("[#FloorNumber#]", "null");
                    }
                    else
                    {
                        select_sql = select_sql.Replace("[#FloorNumber#]", Convert.ToString(dcitem.FloorNumber));
                    }
                    if (dcitem.TotalFloor == null)
                    {
                        select_sql = select_sql.Replace("[#TotalFloor#]", "null");
                    }
                    else
                    {
                        select_sql = select_sql.Replace("[#TotalFloor#]", Convert.ToString(dcitem.TotalFloor));
                    }
                    if (dcitem.RemainYear == null)
                    {
                        select_sql = select_sql.Replace("[#RemainYear#]", "null");
                    }
                    else
                    {
                        select_sql = select_sql.Replace("[#RemainYear#]", Convert.ToString(dcitem.RemainYear));
                    }
                    if (dcitem.Depreciation == null)
                    {
                        select_sql = select_sql.Replace("[#Depreciation#]", "null");
                    }
                    else
                    {
                        select_sql = select_sql.Replace("[#Depreciation#]", Convert.ToString(dcitem.Depreciation));
                    }
                }
                else
                {
                    logger.InfoFormat("===================== >> BatchInsertDataCase DataCase Is Null");
                }
            }
            string insert_sql = @"INSERT INTO [FXTProject]." + tableName + @"
            ([ProjectId],[CaseDate],[PurposeCode],[FloorNumber],[BuildingArea],[FrontCode],[UnitPrice],[MoneyUnitCode],[CaseTypeCode],[StructureCode],[BuildingTypeCode],[HouseTypeCode],[Creator],[Remark],[TotalPrice],[CityID],[Valid],[FXTCompanyId],[TotalFloor],[RemainYear],[Depreciation],[SourceName],[SourceLink],[SourcePhone],[AreaId],[AreaName],[BuildingDate],[ZhuangXiu],[PeiTao])" + select_sql;

            logger.DebugFormat("BatchInsertDataCase tname={0} SQL:{1}", tableName, insert_sql);

            int rows = 0;
            try
            {
                rows = DBUtils.insertData(CaseUrl, insert_sql);
            }
            catch (Exception e)
            {
                logger.ErrorFormat("BatchInsertDataCase tname={0} SQL:{1}", tableName, insert_sql);
                logger.Error(e);
            }
            return rows;
        }


    }
}
