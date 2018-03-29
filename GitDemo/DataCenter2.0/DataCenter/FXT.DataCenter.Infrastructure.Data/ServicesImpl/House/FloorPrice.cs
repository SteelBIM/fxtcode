using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class FloorPrice : IFloorPrice
    {
        public int AddFloorPrice(Sys_FloorPrice model)
        {
            string str_sql = @"
insert into FXTProject.dbo.Sys_FloorPrice(CityId,总楼层开始,总楼层结束,所在楼层,楼栋均价开始,楼栋均价结束,楼层差,是否带电梯,均价层,是否百分比,fxtcompanyid)
VALUES(@CityId,@StartTotalFloor,@EndTotalFloor,@CurrFloor,null,null,@FloorDifference,@IsLift,null,1,@fxtcompanyid)";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Execute(str_sql, new
                {
                    CityId = model.CityId,
                    StartTotalFloor = model.StartTotalFloor,
                    EndTotalFloor = model.EndTotalFloor,
                    CurrFloor = model.CurrFloor,
                    FloorDifference = model.FloorDifference,
                    IsLift = model.IsLift,
                    fxtcompanyid = model.fxtcompanyid
                });
            }
        }

        public IQueryable<Sys_FloorPrice> FindAll(int cityId, int fxtCompanyId)
        {
            var strSql = @"
select id,CityId,总楼层开始,总楼层结束,所在楼层,CONVERT(numeric(18,2),楼层差) as 楼层差,是否带电梯,fxtcompanyid
from FXTProject.dbo.Sys_FloorPrice
where 1 = 1
and CityId = @CityId
and fxtcompanyid = @fxtcompanyid
order BY id desc";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                var result = con.Query<Sys_FloorPrice>(strSql, new { cityId, fxtCompanyId }).AsQueryable();
                return result;
            }
        }

        public DataTable ExportFloorCode(int cityId, int fxtCompanyId)
        {
            List<SqlParameter> paramet = new List<SqlParameter>();
            var strSql = @"
SELECT 总楼层开始
	,总楼层结束
	,所在楼层
	,CONVERT(NUMERIC(18, 2), 楼层差) AS 楼层差_百分比
	,CASE 是否带电梯 WHEN 1 THEN '是' ELSE '否' END AS 是否带电梯
FROM FXTProject.dbo.Sys_FloorPrice
WHERE 1 = 1
	AND CityId = @CityId
	AND fxtcompanyid = @fxtcompanyid
ORDER BY id DESC";

            using (IDbConnection conn = DapperAdapter.OpenConnection())
            {
                paramet.Add(new SqlParameter("@CityID", cityId));
                paramet.Add(new SqlParameter("@FxtCompanyId", fxtCompanyId));

                SqlParameter[] param = paramet.ToArray();
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
                var result = DBHelperSql.ExecuteDataTable(strSql, param);
                return result;
            }
        }

        public int UpdateFloorPriceInImport(Sys_FloorPrice model)
        {
            string str_sql = @"
update FXTProject.dbo.Sys_FloorPrice SET 楼层差 = @FloorDifference
where 1 = 1
and CityId = @CityId 
and fxtcompanyid = @fxtcompanyid
and 总楼层开始 = @StartTotalFloor
and 总楼层结束 = @EndTotalFloor
and 所在楼层 = @CurrFloor
and 是否带电梯 = @IsLift";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Execute(str_sql, new
                {
                    CityId = model.CityId,
                    StartTotalFloor = model.StartTotalFloor,
                    EndTotalFloor = model.EndTotalFloor,
                    CurrFloor = model.CurrFloor,
                    IsLift = model.IsLift,
                    FloorDifference = model.FloorDifference,
                    fxtcompanyid = model.fxtcompanyid
                });
            }
        }

        public int DeleteFloorPrice(int cityId, int fxtcompanyid)
        {
            string strsql = @"
delete FXTProject.dbo.Sys_FloorPrice
where 1 = 1
and CityId = @CityId 
and fxtcompanyid = @fxtcompanyid";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Execute(strsql, new
                {
                    CityId = cityId,
                    fxtcompanyid = fxtcompanyid
                });
            }
        }
    }
}
