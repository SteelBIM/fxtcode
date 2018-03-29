using System;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 楼层图片
    /// 此操作类尚未用到
    /// </summary>
    public class BusinessFloorPhoto : IBusinessFloorPhoto
    {

        /// <summary>
        /// 房讯通
        /// </summary>
        private int FxtComId = Convert.ToInt32(ConfigurationHelper.FxtCompanyId);//房讯通

        /// <summary>
        /// 获取Table
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">公司Id</param>
        /// <returns></returns>
        private IQueryable<SYS_City_Table> GetCityTable(int CityId, int FxtCompanyId = 0)
        {
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataCenter))
            {
                if (FxtCompanyId > 0)
                {
                    string strsql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],subhousepricetable,s.BizCompanyId as ShowCompanyId  FROM [FXTProject].[dbo].[SYS_City_Table] c with(nolock),[Privi_Company_ShowData] s with(nolock) where c.CityId=@CityId and c.CityId=s.CityId and FxtCompanyId=@FxtCompanyId and typecode= 1003002";
                    return (con.Query<SYS_City_Table>(strsql, new { CityId = CityId, FxtCompanyId = FxtCompanyId })).AsQueryable();
                }
                else
                {
                    string strsql = @"SELECT [ProjectTable],[BuildingTable],[HouseTable],[CaseTable],[QueryInfoTable],[ReportTable],[PrintTable],[HistoryTable],[QueryTaxTable],subhousepricetable FROM [FXTProject].[dbo].[SYS_City_Table] with(nolock) where CityId=@CityId";
                    return (con.Query<SYS_City_Table>(strsql, new { CityId = CityId })).AsQueryable();
                }
            }

        }

        /// <summary>
        /// 获取楼层图片列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<LNK_F_Photo> GetLNK_F_PhotoList(LNK_F_Photo model, bool self = true)
        {
            try
            {
                var city_table = GetCityTable(model.CityId, model.FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new LNK_B_Photo();
                //}
                string ComId = city_table.FirstOrDefault().ShowCompanyId;
                string attr = @"f.Id, f.FloorId, f.PhotoTypeCode, f.Path, f.PhotoDate, f.PhotoName, 
                            f.CityId, f.Valid, f.FxtCompanyId, f.SaveUser, f.SaveDate";
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");
                strSql.Append(attr);
                strSql.Append("  from LNK_F_Photo f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.Valid=1 and f.FloorId=@FloorId ");
                strSql.Append(" and not exists(select sub.Id from dbo.LNK_F_Photo_sub sub with(rowlock) where  f.FloorId=sub.FloorId ");
                strSql.Append(" and sub.FxtCompanyId=@FxtCompanyId and f.Id=sub.Id and f.CityId=sub.CityId) ");
                if (self)//查看别人
                {
                    strSql.Append(" and f.FxtCompanyId=@FxtCompanyId  ");
                }
                else
                {
                    strSql.Append(" and f.FxtCompanyId in (" + ComId + ") ");
                }
                strSql.Append(" union ");
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from LNK_F_Photo_sub f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId and f.Valid=1 and f.FloorId=@FloorId ");
                strSql.Append(" order by f.Id desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var LNK_f_Photolist = con.Query<LNK_F_Photo>(strSql.ToString(), new { CityId = model.CityId, FxtCompanyId = model.FxtCompanyId, FloorId = model.FloorId }).AsQueryable();
                    return LNK_f_Photolist;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取楼层图片信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <returns></returns>
        public LNK_F_Photo GetLNK_F_PhotoById(int id, int cityId, int fxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(cityId, fxtCompanyId);
                string ComId = city_table.FirstOrDefault().ShowCompanyId;
                if (string.IsNullOrEmpty(ComId)) ComId = fxtCompanyId.ToString();
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new LNK_B_Photo();
                //}
                string attr = @"f.Id, f.FloorId, f.PhotoTypeCode, f.Path, f.PhotoDate, f.PhotoName, 
                            f.CityId, f.Valid, f.FxtCompanyId, f.SaveUser, f.SaveDate";
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");
                strSql.Append(attr);
                strSql.Append("  from LNK_F_Photo f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.Valid=1 and f.Id=@Id ");
                strSql.Append(" and not exists(select sub.Id from dbo.LNK_F_Photo_sub sub with(rowlock) where f.Id=sub.Id ");
                strSql.Append(" and sub.FxtCompanyId=@FxtCompanyId and f.Id=sub.Id and f.CityId=sub.CityId) ");
                strSql.Append(" and (f.FxtCompanyId=@FxtCompanyId or f.FxtCompanyId in (" + ComId + ")) ");
                strSql.Append(" union ");
                strSql.Append("select ");
                strSql.Append(attr);
                strSql.Append("  from LNK_F_Photo_sub f with(nolock) ");
                strSql.Append(" where f.CityId=@CityId and f.FxtCompanyId=@FxtCompanyId and f.Valid=1 and f.Id=@Id ");
                strSql.Append(" order by f.Id desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var LNK_f_Photolist = con.Query<LNK_F_Photo>(strSql.ToString(), new { CityId = cityId, FxtCompanyId = fxtCompanyId, Id = id });
                    return LNK_f_Photolist.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 新增楼层图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddLNK_F_Photo(LNK_F_Photo model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [FxtData_Biz].[dbo].[LNK_F_Photo] with(rowlock) (");
                strSql.Append("[FloorId],[PhotoTypeCode],[Path],[PhotoDate],[PhotoName],[CityId],");
                strSql.Append("[Valid],[FxtCompanyId],[SaveUser],[SaveDate]");
                strSql.Append(") values (");
                strSql.Append("@FloorId,@PhotoTypeCode,@Path,@PhotoDate,@PhotoName,@CityId,");
                strSql.Append("@Valid,@FxtCompanyId,@SaveUser,@SaveDate");
                strSql.Append(") ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    int result = con.Execute(strSql.ToString(), model);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 修改楼层图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateLNK_F_Photo(LNK_F_Photo model, int currFxtCompanyId)
        {
            try
            {
                //var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return 0;
                //}
                StringBuilder strSql = new StringBuilder();
                string sql;
                strSql.Append(" SaveUser = @SaveUser , ");
                strSql.Append(" SaveDate = @SaveDate , ");
                strSql.Append(" PhotoTypeCode = @PhotoTypeCode , ");
                strSql.Append(" Path = @Path , ");
                strSql.Append(" PhotoName = @PhotoName ");
                strSql.Append(" where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    //房讯通
                    if (model.FxtCompanyId == FxtComId)
                    {
                        sql = " update FxtData_Biz.dbo.LNK_F_Photo with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    if (model.FxtCompanyId == currFxtCompanyId)
                    {
                        sql = " update FxtData_Biz.dbo.LNK_F_Photo with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    else
                    {
                        sql = "select Id from FxtData_Biz.dbo.LNK_F_Photo_sub with(nolock) where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        LNK_B_Photo buil_sub = con.Query<LNK_B_Photo>(sql, new { Id = model.Id, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                        if (buil_sub != null)//子表存在
                        {
                            sql = " update FxtData_Biz.dbo.LNK_F_Photo_sub with(rowlock) set ";
                            int reslut = con.Execute(sql + strSql.ToString(), model);
                            return reslut;
                        }
                        else
                        {
                            sql = "select Id from FxtData_Biz.dbo.LNK_F_Photo with(nolock) where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                            LNK_B_Photo buil = con.Query<LNK_B_Photo>(sql, new { Id = model.Id, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                            if (buil != null)//主表存在
                            {
                                sql = " update FxtData_Biz.dbo.LNK_F_Photo with(rowlock) set ";
                                int reslut = con.Execute(sql + strSql.ToString(), model);
                                return reslut;
                            }
                            else
                            {
                                //主表字表不在(在字表中插入一条记录来自主表)
                                strSql.Clear();
                                strSql.Append("insert into FxtData_Biz.dbo.LNK_F_Photo_sub (");
                                strSql.Append("Id,FxtCompanyId, SaveUser, SaveDate,");
                                strSql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid ");
                                strSql.Append(" )");
                                strSql.Append(" select ");
                                strSql.Append("Id,'" + currFxtCompanyId + "' as FxtCompanyId, '" + model.SaveUser + "' as SaveUser, GetDate() as SaveDate,");
                                strSql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid  ");
                                strSql.Append(" from FxtData_Biz.dbo.LNK_F_Photo with(nolock) ");
                                strSql.Append(" where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                                int reslut = con.Execute(strSql.ToString(), new { Id = model.Id, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId });
                                return reslut;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除楼层图片
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="userName"></param>
        /// <param name="productTypeCode"></param>
        /// <returns></returns>
        public bool DeleteLNK_F_Photo(int Id, int cityId, int fxtCompanyId, string userName, int productTypeCode, int currFxtCompanyId)
        {
            try
            {
                //var city_table = GetCityTable(CityId,FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return false;
                //}
                //////暂不判断是否IsDeleteTrue20161029
                //////CompanyProduct compro = null;
                //////using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtUserCenter))
                //////{
                //////    string sql = "SELECT CompanyId,IsDeleteTrue FROM CompanyProduct WITH(NOLOCK) WHERE CompanyId=@CompanyId and CityId=@CityId and ProductTypeCode=@ProductTypeCode";
                //////    compro = con.Query<CompanyProduct>(sql, new { CompanyId = fxtCompanyId, CityId = cityId, ProductTypeCode = productTypeCode }).FirstOrDefault();

                //////}
                //////if (compro != null)
                //////{
                //////    if (compro.IsDeleteTrue == 1)
                //////    {
                //////        return DeleteBuild(cityId, fxtCompanyId, Id);
                //////    }
                //////    else
                //////    {
                //////        return DeleteBuild(cityId, fxtCompanyId, Id, userName, productTypeCode,currFxtCompanyId);
                //////    }
                //////}
                //////else
                //////{
                //////    return false;
                //////}
                return DeleteBuild(cityId, fxtCompanyId, Id, userName, productTypeCode, currFxtCompanyId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private bool DeleteBuild(int cityId, int fxtCompanyId, int Id, string userName, int productTypeCode, int currFxtCompanyId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append(" set Valid=0,[SaveDate]=GetDate(),[SaveUser]='" + userName + "' ");
                    strsql.Append(" where [Id]=@Id and CityId=@CityId and FxtCompanyId=@FxtCompanyId");

                    if (fxtCompanyId == FxtComId)
                    {

                        string sql = "update FxtData_Biz.dbo.LNK_F_Photo with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        return result > 0;
                    }
                    if (fxtCompanyId == currFxtCompanyId)
                    {

                        string sql = "update FxtData_Biz.dbo.LNK_F_Photo with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                        return result > 0;
                    }
                    else
                    {
                        string sql_query = " select Id from FxtData_Biz.dbo.LNK_F_Photo_sub with(nolock) where [Id]=@Id and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        LNK_B_Photo sub = con.Query<LNK_B_Photo>(sql_query, new { Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId }).FirstOrDefault();
                        if (sub != null)
                        {
                            string sql = "update FxtData_Biz.dbo.LNK_F_Photo_sub with(rowlock) " + strsql.ToString();
                            int result = con.Execute(sql, new { BuildingId = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                            return result > 0;
                        }
                        else
                        {
                            strsql.Clear();
                            strsql.Append("insert into FxtData_Biz.dbo.LNK_F_Photo_sub with(rowlock) (");
                            strsql.Append("FxtCompanyId, SaveUser, SaveDate,");
                            strsql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid ");
                            strsql.Append(" )");
                            strsql.Append(" select ");
                            strsql.Append(" '" + currFxtCompanyId + "' as FxtCompanyId, '" + userName + "' as SaveUser, GetDate() as SaveDate,");
                            strsql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid  ");
                            strsql.Append(" from FxtData_Biz.dbo.LNK_F_Photo with(nolock) ");
                            strsql.Append(" where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                            int reslut = con.Execute(strsql.ToString(), new { Id = Id, CityId = cityId, FxtCompanyId = fxtCompanyId });
                            return reslut > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private bool DeleteBuild(int cityId, int fxtCompanyId, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
