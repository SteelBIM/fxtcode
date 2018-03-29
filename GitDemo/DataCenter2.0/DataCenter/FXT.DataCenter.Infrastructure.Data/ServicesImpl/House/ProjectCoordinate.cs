using System;
using System.Linq;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data;
using Dapper;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class ProjectCoordinate
    {
        /// <summary>
        /// 新增楼盘坐标
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        public int AddProjectCoordinate(DAT_Project_Coordinate modal)
        {
            string sql = @"insert into dbo.DAT_Project_Coordinate with(rowlock) (projectid,fxtcompanyid,cityid,x,y,valid) 
                                            values(@projectid,@fxtcompanyid,@cityid,@x,@y,@valid)";

            try
            {
                SqlParameter[] parameters = {
                            new SqlParameter("@projectid", SqlDbType.Int),
                            new SqlParameter("@fxtcompanyid", SqlDbType.Int),
                            new SqlParameter("@cityid", SqlDbType.Int),
                            new SqlParameter("@x", SqlDbType.Decimal),
                            new SqlParameter("@y", SqlDbType.Decimal),
                            new SqlParameter("@valid",SqlDbType.Int)
                                        };
                int i = 0;
                parameters[i++].Value = modal.ProjectId;
                parameters[i++].Value = modal.FxtCompanyId;
                parameters[i++].Value = modal.CityID;
                if (Convert.IsDBNull(modal.X))
                {
                    parameters[i++].Value = DBNull.Value;
                }
                else
                {
                    parameters[i++].Value = modal.X;
                }
                if (Convert.IsDBNull(modal.Y))
                {
                    parameters[i++].Value = DBNull.Value;
                }
                else
                {
                    parameters[i++].Value = modal.Y;
                }
                if (Convert.IsDBNull(modal.Valid))
                {
                    parameters[i++].Value = DBNull.Value;
                }
                else
                {
                    parameters[i++].Value = modal.Valid;
                }
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
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
        public int UpdateProjectCoordinate(DAT_Project_Coordinate modal)
        {
            string sql = @"UPDATE dbo.DAT_Project_Coordinate with(rowlock)
                       SET [FxtCompanyId] = @FxtCompanyId
                          ,[CityID] = @CityID
                          ,[X] = @X
                          ,[Y] = @Y
                          ,[Valid] =@Valid where ProjectId=@ProjectId";
            SqlParameter[] parameters = {
                            new SqlParameter("@FxtCompanyId", SqlDbType.Int),
                            new SqlParameter("@CityID", SqlDbType.Int),
                            new SqlParameter("@X", SqlDbType.Decimal),
                            new SqlParameter("@Y", SqlDbType.Decimal),
                            new SqlParameter("@Valid",SqlDbType.Int),
                            new SqlParameter("@ProjectId",SqlDbType.Int)
                                        };
            int i = 0;
            parameters[i++].Value = modal.FxtCompanyId;
            parameters[i++].Value = modal.CityID;
            if (modal.X == null)
            {
                parameters[i++].Value = DBNull.Value;
            }
            else
            {
                parameters[i++].Value = modal.X;
            }
            if (modal.Y == null)
            {
                parameters[i++].Value = DBNull.Value;
            }
            else
            {
                parameters[i++].Value = modal.Y;
            }
            if (modal.Valid == null)
            {
                parameters[i++].Value = DBNull.Value;
            }
            else
            {
                parameters[i++].Value = modal.Valid;
            }
            parameters[i++].Value = modal.ProjectId;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
            int result = DBHelperSql.ExecuteNonQuery(sql, parameters);
            return result;

        }
        /// <summary>
        /// 删除坐标点
        /// </summary>
        /// <param name="fxtcompanyid">评估机构ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="ProjectId">楼盘ID</param>
        /// <returns></returns>
        public int DeleteProjectCoordinate(int fxtcompanyid, int cityId, long ProjectId)
        {
            string sql = @"delete from dbo.DAT_Project_Coordinate with(rowlock) where fxtcompanyid=@fxtcompanyid and cityid=@cityid and ProjectId=@ProjectId";

            SqlParameter[] parameters = {
                            new SqlParameter("@fxtcompanyid", SqlDbType.Int),
                            new SqlParameter("@cityid", SqlDbType.Int),
                            new SqlParameter("@ProjectId", SqlDbType.Int)
                                        };
            int i = 0;
            parameters[i++].Value = fxtcompanyid;
            parameters[i++].Value = cityId;
            parameters[i++].Value = ProjectId;
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtProject;
            int result = DBHelperSql.ExecuteNonQuery(sql, parameters);
            return result;

        }

        /// <summary>
        /// 获取坐标集合
        /// </summary>
        /// <param name="projectId">楼盘ID</param>
        /// <returns></returns>
        public IQueryable<DAT_Project_Coordinate> GetProCoordinateListByPid(int projectId)
        {
            string sql = "Select Id, ProjectId, FxtCompanyId, CityID, X, Y, Valid from dbo.DAT_Project_Coordinate where Valid=1 and ProjectId=@ProjectId";
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtProject))
            {
                return con.Query<DAT_Project_Coordinate>(sql, new
                {
                    ProjectId = projectId
                }).AsQueryable();
            }
          
        }
    }
}
