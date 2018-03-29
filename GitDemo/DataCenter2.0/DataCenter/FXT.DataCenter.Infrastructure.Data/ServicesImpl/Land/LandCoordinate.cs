using System;
using System.Data.SqlClient;
using System.Data;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class LandCoordinate : ILandCoordinate
    {
        /// <summary>
        /// 新增土地坐标
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public int AddLandCoordinate(DAT_Land_Coordinate modal)
        {
            string sql = @"insert into FxtLand.dbo.DAT_Land_Coordinate with(rowlock)(landid,fxtcompanyid,cityid,x,y,valid) values(@landid,@fxtcompanyid,@cityid,@x,@y,@valid)";

            try
            {
                SqlParameter[] parameters = {
                            new SqlParameter("@landid", SqlDbType.Int),
                            new SqlParameter("@fxtcompanyid", SqlDbType.Int),
                            new SqlParameter("@cityid", SqlDbType.Int),
                            new SqlParameter("@x", SqlDbType.Decimal),
                            new SqlParameter("@y", SqlDbType.Decimal),
                            new SqlParameter("@valid",SqlDbType.Int)
                                        };
                int i = 0;
                parameters[i++].Value = modal.landid;
                parameters[i++].Value = modal.fxtcompanyid;
                parameters[i++].Value = modal.cityid;
                if (modal.x == null)
                {
                    parameters[i++].Value = DBNull.Value;
                }
                else
                {
                    parameters[i++].Value = modal.x;
                }
                if (modal.y == null)
                {
                    parameters[i++].Value = DBNull.Value;
                }
                else
                {
                    parameters[i++].Value = modal.y;
                }

                parameters[i++].Value = modal.valid == null ? 1 : modal.valid;
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
                int result = DBHelperSql.ExecuteNonQuery(sql, parameters);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新坐标点
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public int UpdateLandCoordinate(DAT_Land_Coordinate modal)
        {
            string sql = @"UPDATE [FxtLand].[dbo].[DAT_Land_Coordinate] with(rowlock)
                       SET [FxtCompanyId] = @FxtCompanyId
                          ,[CityID] = @CityID
                          ,[X] = @X
                          ,[Y] = @Y
                          ,[Valid] =@Valid where LandId=@LandId";
            SqlParameter[] parameters = {
                            new SqlParameter("@FxtCompanyId", SqlDbType.Int),
                            new SqlParameter("@CityID", SqlDbType.Int),
                            new SqlParameter("@X", SqlDbType.Decimal),
                            new SqlParameter("@Y", SqlDbType.Decimal),
                            new SqlParameter("@Valid",SqlDbType.Int),
                            new SqlParameter("@LandId",SqlDbType.Int)
                                        };
            int i = 0;
            parameters[i++].Value = modal.fxtcompanyid;
            parameters[i++].Value = modal.cityid;
            if (modal.x == null)
            {
                parameters[i++].Value = DBNull.Value;
            }
            else
            {
                parameters[i++].Value = modal.x;
            }
            if (modal.y == null)
            {
                parameters[i++].Value = DBNull.Value;
            }
            else
            {
                parameters[i++].Value = modal.y;
            }
            if (modal.valid == null)
            {
                parameters[i++].Value = DBNull.Value;
            }
            else
            {
                parameters[i++].Value = modal.valid;
            }
            parameters[i++].Value = modal.landid;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            int result = DBHelperSql.ExecuteNonQuery(sql, parameters);
            return result;

        }
        /// <summary>
        /// 删除坐标点
        /// </summary>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="landId">土地ID</param>
        /// <returns></returns>
        public int DeleteLandCoordinate(int fxtcompanyid, int cityId, long landId)
        {
            string sql = @"delete from FxtLand.dbo.DAT_Land_Coordinate with(rowlock) where fxtcompanyid=@fxtcompanyid and cityid=@cityid and landid=@landid";

            SqlParameter[] parameters = {
                            new SqlParameter("@fxtcompanyid", SqlDbType.Int),
                            new SqlParameter("@cityid", SqlDbType.Int),
                            new SqlParameter("@landid", SqlDbType.BigInt)
                                        };
            int i = 0;
            parameters[i++].Value = fxtcompanyid;
            parameters[i++].Value = cityId;
            parameters[i++].Value = landId;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            int result = DBHelperSql.ExecuteNonQuery(sql, parameters);
            return result;

        }
    }
}
