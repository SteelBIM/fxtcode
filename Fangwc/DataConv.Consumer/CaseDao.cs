using DataConv.Library;
using DataConv.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace DataConv.Consumer
{
    public class CaseDao
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CaseDao));

        public string CaseUrl = Convert.ToString(ConfigurationManager.ConnectionStrings["CaseUrl"]);

        public int AddExceptionData(DataCase dc)
        {
            string insert_sql = @"
INSERT INTO [FxtData_Case].[dbo].[住宅案例_出售_异常案例]
(       [楼盘]             ,[行政区]             ,[楼栋]           ,[楼层]    ,[房号] ,[总楼层]      ,[案例时间]                                    ,[用途]              ,[面积]                   ,[单价]               ,[总价]              ,[案例类型]              ,[朝向]             ,[建筑类型]                    ,[户型]                  ,[户型结构]            ,[建筑年代]                                  ,[装修]              ,[使用面积],[剩余年限]       ,[成新率]             ,[币种]                    ,[附属房屋],[配套]          ,[来源]             ,[电话],               [备注]          ,[链接]             ,[标题] ,[地址],[城市]    ,[城市ID],[案例统计周])
values ('" + dc.ProjectName + @"','" + dc.AreaName + @"',NULL,'" + dc.FloorNumber + @"',NULL,'" + dc.TotalFloor + @"','" + dc.CaseDate.ToString("yyyy-MM-dd") + @"','" + dc.PurposeName + @"','" + dc.BuildingArea + @"','" + dc.UnitPrice + @"','" + dc.TotalPrice + @"','" + dc.CaseTypeName + @"','" + dc.FrontName + @"','" + dc.BuildingTypeName + @"','" + dc.HouseTypeName + @"','" + dc.StructureName + @"','" + dc.BuildingDate + @"','" + dc.ZhuangXiu + @"',NULL,'" + dc.RemainYear + @"','" + dc.Depreciation + @"','" + dc.MoneyUnitName + @"',NULL,'" + dc.PeiTao + @"','" + dc.SourceName + @"','" + dc.SourcePhone + @"','" + dc.Remark + @"','" + dc.SourceLink + @"',NULL,NULL,NULL,'" + dc.CityID + @"','" + dc.RecordWeek + @"')

";
            logger.DebugFormat("AddExceptionData SQL={0}", insert_sql);
            int rows = 0;
            try
            {
                rows = DBUtils.insertData(CaseUrl, insert_sql);
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.ErrorFormat("AddExceptionData SQL={0}", insert_sql);
            }
            return rows;
        }

        /// <summary>
        /// 分页查询案例数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="paramMaps"></param>
        /// <returns></returns>
        public List<Model.DataCase> PagingQueryDataCase(int pageIndex, int pageSize, Dictionary<string, string> paramMaps)
        {
            string sql = @"SELECT TOP " + pageSize + @" * FROM
(
SELECT [ProjectName]
      ,[行政区] as 'AreaName'
      ,case when 所在楼层_列表页 is not null and 所在楼层_列表页 <> '' then 所在楼层_列表页
		    when (所在楼层_列表页 is null or 所在楼层_列表页 = '') and (所在楼层_详情页 is not null and 所在楼层_详情页 <> '') then 所在楼层_详情页
		    else null end as 'FloorNumber'
	  ,case when 总楼层_列表页 is not null and 总楼层_列表页 <> '' then 总楼层_列表页
		    when (总楼层_列表页 is null or 总楼层_列表页 = '') and (总楼层_详情页 is not null and 总楼层_详情页 <> '') then 总楼层_详情页
		    else null end as 'TotalFloor'
      ,convert(nvarchar(10),[案例时间],21) as 'CaseDate'
      ,[用途描述] as 'PurposeName'
      ,[面积] as 'BuildingArea'
      ,[单价] as 'UnitPrice'
      ,[总价_万]*10000 as 'TotalPrice'
      ,[案例类型描述] as 'CaseTypeName'
      ,[朝向归类] as 'FrontName'
      ,[建筑类型描述] as 'BuildingTypeName'
      ,[户型归类] as 'HouseTypeName'
      ,[结构归类] as 'StructureName'
      ,[建筑年代] as 'BuildingDate'
      ,[装修归类] as 'ZhuangXiu'
      ,[币种描述] as 'MoneyUnitName'
      ,[配套设施] as 'PeiTao'
      ,[数据来源] as 'SourceName'    
      ,[来源电话] as 'SourcePhone'
      ,[数据链接] as 'SourceLink'         
      ,城市表.CityId as 'CityID'
      ,案例表.案例统计周 as 'RecordWeek'
      ,案例表.[房源编号] as 'HouseIndex'
      
  FROM [FxtData_Case].[dbo].[住宅案例_出售] as 案例表
  left join FxtDataCenter.dbo.SYS_City as 城市表
  on 案例表.CityId=城市表.CityId
  left join dbo.户型 as 户型表
  on 案例表.户型=户型表.code
  left join dbo.朝向 as 朝向表
  on 案例表.朝向=朝向表.code
  left join dbo.结构 as 结构表
  on 案例表.结构=结构表.code
  left join dbo.装修 as 装修表
  on 案例表.装修=装修表.code
  left join dbo.用途 as 用途表
  on 案例表.用途=用途表.code
  left join dbo.建筑类型 as 建筑类型表
  on 案例表.建筑类型=建筑类型表.code
  left join dbo.币种 as 币种表
  on 案例表.币种=币种表.code
  left join dbo.案例类型 as 案例类型表
  on 案例表.案例类型=案例类型表.code
  ) t1
  WHERE 1=1 AND t1.HouseIndex NOT IN
  (
  SELECT TOP " + (pageIndex - 1) * pageSize + @" t2.HouseIndex FROM
  (
  SELECT convert(nvarchar(10),[案例时间],21) as 'CaseDate'       
      ,城市表.CityId as 'CityID'
      ,案例表.[房源编号] as 'HouseIndex'
      
  FROM [FxtData_Case].[dbo].[住宅案例_出售] as 案例表
  left join FxtDataCenter.dbo.SYS_City as 城市表
  on 案例表.CityId=城市表.CityId
  left join dbo.户型 as 户型表
  on 案例表.户型=户型表.code
  left join dbo.朝向 as 朝向表
  on 案例表.朝向=朝向表.code
  left join dbo.结构 as 结构表
  on 案例表.结构=结构表.code
  left join dbo.装修 as 装修表
  on 案例表.装修=装修表.code
  left join dbo.用途 as 用途表
  on 案例表.用途=用途表.code
  left join dbo.建筑类型 as 建筑类型表
  on 案例表.建筑类型=建筑类型表.code
  left join dbo.币种 as 币种表
  on 案例表.币种=币种表.code
  left join dbo.案例类型 as 案例类型表
  on 案例表.案例类型=案例类型表.code
  )t2 
  WHERE 1=1 
  and t2.CityId =[#CityID#]
  and t2.CaseDate >= '[#StartTime#]'
  and t2.CaseDate < '[#EndTime#]'
  order by t2.CaseDate
  )
  and t1.CityId =[#CityID#]
  and t1.CaseDate >= '[#StartTime#]'
  and t1.CaseDate < '[#EndTime#]'
  order by t1.CaseDate
";
            foreach (KeyValuePair<string, string> item in paramMaps)
            {
                logger.DebugFormat("PagingQueryDataCase key={0}, Value = {1}", item.Key, item.Value);
                sql = sql.Replace(item.Key, item.Value);
            }
            logger.DebugFormat("PagingQueryDataCase SQL: {0}", sql);
            List<Model.DataCase> list = new List<Model.DataCase>();
            try
            {
                DataTable dt_housecase = DBUtils.getData(CaseUrl, sql);
                
                list = ConvUtils<Model.DataCase>.ConvertToList(dt_housecase);
                logger.DebugFormat("PagingQueryDataCase list Count={0}", list.Count);
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.ErrorFormat("PagingQueryDataCase SQL: {0}", sql);
            }
            return list;
        }

        public int RemoveDataCase(Dictionary<string, string> paramMaps)
        {
            string sql = @"DELETE FROM [FxtData_Case].[dbo].[住宅案例_出售] WITH (NOLOCK) WHERE CityId = [#CityID#] and [案例时间] >= '[#StartTime#]' and [案例时间] < '[#EndTime#]'";
            foreach (KeyValuePair<string, string> item in paramMaps)
            {
                logger.DebugFormat("RemoveDataCase key={0}, Value = {1}", item.Key, item.Value);
                sql = sql.Replace(item.Key, item.Value);
            }
            int cnt = 0;
            try
            {
                cnt = DBUtils.DeleteData(CaseUrl, sql);
                logger.DebugFormat("Delete DataCase Count={0}", cnt);
            }
            catch (Exception e)
            {
                logger.Error(e);
                logger.ErrorFormat("RemoveDataCase SQL: {0}", sql);
            }
            return cnt;
        }


    }
}
