using System;
using System.Data;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    /// <summary>
    /// 楼栋图片
    /// </summary>
    public class BusinessBuildingPhoto : IBusinessBuildingPhoto
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
        /// 获取楼栋图片列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<LNK_B_Photo> GetLNK_B_PhotoList(LNK_B_Photo model, bool self = true)
        {
            try
            {
                var city_table = GetCityTable(Convert.ToInt32(model.CityId), model.FxtCompanyId);

                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new List<LNK_B_Photo>();
                //}
                string ComId = city_table.FirstOrDefault().ShowCompanyId;
                string attr = " p.Id, p.FxtCompanyId, p.SaveUser, p.SaveDate, p.BuildingId,p.PhotoTypeCode, p.Path, p.PhotoDate, p.PhotoName, p.CityId, p.Valid  ";
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");
                strSql.Append(attr);
                strSql.Append("  from [FxtData_Biz].[dbo].[LNK_B_Photo] p with(nolock) ");
                strSql.Append(" where p.CityId=@CityId and p.Valid=1 and p.BuildingId=@BuildingId ");
                strSql.Append(" and not exists(select sub.Id from [FxtData_Biz].[dbo].[LNK_B_Photo] sub with(rowlock) where  sub.BuildingId=@BuildingId ");
                strSql.Append(" and sub.FxtCompanyId=@FxtCompanyId and p.Id=sub.Id and p.CityId=sub.CityId) ");
                if (self)//查看自己
                {
                    strSql.Append(" and p.FxtCompanyId=@FxtCompanyId  ");
                }
                else {
                    strSql.Append(" and p.FxtCompanyId in (" + ComId + ") ");
                }
                strSql.Append(" union ");
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from [FxtData_Biz].[dbo].[LNK_B_Photo] p with(nolock) ");
                strSql.Append(" where p.CityId=@CityId and p.FxtCompanyId=@FxtCompanyId and p.Valid=1 and p.BuildingId=@BuildingId ");
                strSql.Append(" order by p.Id desc");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var LNK_B_Photolist = con.Query<LNK_B_Photo>(strSql.ToString(), new { CityId = model.CityId, FxtCompanyId = model.FxtCompanyId, BuildingId=model.BuildingId }).AsQueryable();
                    return LNK_B_Photolist;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取楼栋图片信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <returns></returns>
        public LNK_B_Photo GetLNK_B_PhotoById(int Id, int CityId, int FxtCompanyId)
        {
            try
            {
                var city_table = GetCityTable(CityId, FxtCompanyId);
                //if (city_table != null && city_table.Count() > 0)
                //{
                //    return new LNK_B_Photo();
                //}
                string ComId = city_table.FirstOrDefault().ShowCompanyId;
                StringBuilder strSql = new StringBuilder();
                string attr = "p.Id, p.FxtCompanyId, p.SaveUser, p.SaveDate, p.BuildingId,p.PhotoTypeCode, p.Path, p.PhotoDate, p.PhotoName, p.CityId, p.Valid  ";
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from [FxtData_Biz].[dbo].[LNK_B_Photo] p with(nolock) ");
                strSql.Append(" where p.CityId=@CityId and p.Valid=1 and p.Id=@Id ");
                strSql.Append(" and not exists(select sub.Id from [FxtData_Biz].[dbo].[LNK_B_Photo_sub] sub with(rowlock) where p.Id=sub.Id ");
                strSql.Append(" and sub.FxtCompanyId=@FxtCompanyId and p.CityId=sub.CityId) ");
                strSql.Append(" and (p.FxtCompanyId=@FxtCompanyId or p.FxtCompanyId in (" + ComId + ")) ");
                strSql.Append(" union ");
                strSql.Append(" select ");
                strSql.Append(attr);
                strSql.Append("  from [FxtData_Biz].[dbo].[LNK_B_Photo_sub] p with(nolock) ");
                strSql.Append(" where p.CityId=@CityId and p.FxtCompanyId=@FxtCompanyId and p.Valid=1 and p.Id=@Id ");
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    var LNK_B_Photo = con.Query<LNK_B_Photo>(strSql.ToString(), new { CityId = CityId, FxtCompanyId = FxtCompanyId, Id = Id }).FirstOrDefault();
                    return LNK_B_Photo;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 新增楼栋图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddLNK_B_Photo(LNK_B_Photo model)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [FxtData_Biz].[dbo].[LNK_B_Photo] with(rowlock) (");
                strSql.Append("FxtCompanyId,SaveUser,SaveDate,BuildingId,PhotoTypeCode,Path,PhotoDate,PhotoName,CityId,Valid");
                strSql.Append(") values (");
                strSql.Append("@FxtCompanyId,@SaveUser,@SaveDate,@BuildingId,@PhotoTypeCode,@Path,@PhotoDate,@PhotoName,@CityId,@Valid");
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
        /// 修改楼栋图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateLNK_B_Photo(LNK_B_Photo model, int currFxtCompanyId)
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
                        sql = " update FxtData_Biz.dbo.LNK_B_Photo with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    //房讯通
                    if (model.FxtCompanyId == currFxtCompanyId)
                    {
                        sql = " update FxtData_Biz.dbo.LNK_B_Photo with(rowlock) set ";
                        int reslut = con.Execute(sql + strSql.ToString(), model);
                        return reslut;
                    }
                    else
                    {
                        sql = "select Id from FxtData_Biz.dbo.LNK_B_Photo_sub with(nolock) where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        LNK_B_Photo buil_sub = con.Query<LNK_B_Photo>(sql, new { Id = model.Id, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                        if (buil_sub != null)//子表存在
                        {
                            sql = " update FxtData_Biz.dbo.LNK_B_Photo_sub with(rowlock) set ";
                            int reslut = con.Execute(sql + strSql.ToString(), model);
                            return reslut;
                        }
                        else
                        {
                            sql = "select Id from FxtData_Biz.dbo.LNK_B_Photo with(nolock) where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                            LNK_B_Photo buil = con.Query<LNK_B_Photo>(sql, new { Id = model.Id, CityId = model.CityId, FxtCompanyId = model.FxtCompanyId }).FirstOrDefault();
                            if (buil != null)//主表存在
                            {
                                sql = " update FxtData_Biz.dbo.LNK_B_Photo with(rowlock) set ";
                                int reslut = con.Execute(sql + strSql.ToString(), model);
                                return reslut;
                            }
                            else
                            {
                                //主表字表不在(在字表中插入一条记录来自主表)
                                strSql.Clear();
                                strSql.Append("insert into FxtData_Biz.dbo.LNK_B_Photo_sub (");
                                strSql.Append("Id,FxtCompanyId, SaveUser, SaveDate,");
                                strSql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid ");
                                strSql.Append(" )");
                                strSql.Append(" select ");
                                strSql.Append("Id,'"+currFxtCompanyId+"' as FxtCompanyId, '" + model.SaveUser + "' as SaveUser, GetDate() as SaveDate,");
                                strSql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid  ");
                                strSql.Append(" from FxtData_Biz.dbo.LNK_B_Photo with(nolock) ");
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
       /// 删除楼栋图片
       /// </summary>
       /// <param name="Id">图片ID</param>
       /// <param name="CityId">城市ID</param>
       /// <param name="FxtCompanyId">评估机构ID</param>
       /// <param name="userId">用户ID</param>
       /// <param name="ProductTypeCode">产品Code</param>
       /// <returns></returns>
        public bool DeleteLNK_B_Photo(int Id, int CityId, int FxtCompanyId, string userId, int ProductTypeCode, int currFxtCompanyId)
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
                //////    compro = con.Query<CompanyProduct>(sql, new { CompanyId = FxtCompanyId, CityId = CityId, ProductTypeCode = ProductTypeCode }).FirstOrDefault();

                //////}
                //////if (compro != null)
                //////{
                //////    if (compro.IsDeleteTrue == 1)
                //////    {
                //////        return DeleteBuild(CityId, FxtCompanyId, Id);
                //////    }
                //////    else
                //////    {
                //////        return DeleteBuild(CityId, FxtCompanyId, Id, userId, ProductTypeCode, currFxtCompanyId);
                //////    }
                //////}
                //////else
                //////{
                //////    return false;
                //////}
                return DeleteBuild(CityId, FxtCompanyId, Id, userId, ProductTypeCode, currFxtCompanyId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除楼栋(更新valid)
        /// </summary>
        /// <param name="CityId">城市ID</param>
        /// <param name="FxtCompanyId">评估ID</param>
        /// <param name="Id">图片ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="ProductTypeCode">产品Code</param>
        /// <returns></returns>
        private bool DeleteBuild(int CityId, int FxtCompanyId, int Id, string userId, int ProductTypeCode, int currFxtCompanyId)
        {
            try
            {
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append(" set Valid=0,[SaveDate]=GetDate(),[SaveUser]='" + userId + "' ");
                    strsql.Append(" where [Id]=@Id and CityId=@CityId and FxtCompanyId=@FxtCompanyId");

                    if (FxtCompanyId == FxtComId)
                    {

                        string sql = "update FxtData_Biz.dbo.LNK_B_Photo with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { Id = Id, CityId = CityId, FxtCompanyId = FxtCompanyId });
                        return result > 0;
                    }
                    if (FxtCompanyId == currFxtCompanyId)
                    {

                        string sql = "update FxtData_Biz.dbo.LNK_B_Photo with(rowlock) " + strsql.ToString();
                        int result = con.Execute(sql, new { Id = Id, CityId = CityId, FxtCompanyId = FxtCompanyId });
                        return result > 0;
                    }
                    else
                    {
                        string sql_query = " select Id from FxtData_Biz.dbo.LNK_B_Photo_sub with(nolock) where [Id]=@Id and CityId=@CityId and FxtCompanyId=@FxtCompanyId";
                        LNK_B_Photo sub = con.Query<LNK_B_Photo>(sql_query, new { Id = Id, CityId = CityId, FxtCompanyId = FxtCompanyId }).FirstOrDefault();
                        if (sub != null)
                        {
                            string sql = "update FxtData_Biz.dbo.LNK_B_Photo_sub with(rowlock) " + strsql.ToString();
                            int result = con.Execute(sql, new { BuildingId = Id, CityId = CityId, FxtCompanyId = FxtCompanyId });
                            return result > 0;
                        }
                        else
                        {
                            strsql.Clear();
                            strsql.Append("insert into FxtData_Biz.dbo.LNK_B_Photo_sub with(rowlock) (");
                            strsql.Append("FxtCompanyId, SaveUser, SaveDate,");
                            strsql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid ");
                            strsql.Append(" )");
                            strsql.Append(" select ");
                            strsql.Append(" '" + currFxtCompanyId + "' as FxtCompanyId, '" + userId + "' as SaveUser, GetDate() as SaveDate,");
                            strsql.Append("BuildingId,PhotoTypeCode, Path, PhotoDate, PhotoName, CityId, Valid  ");
                            strsql.Append(" from FxtData_Biz.dbo.LNK_B_Photo with(nolock) ");
                            strsql.Append(" where Id=@Id and  CityId=@CityId and FxtCompanyId=@FxtCompanyId ");
                            int reslut = con.Execute(strsql.ToString(), new { Id = Id, CityId = CityId, FxtCompanyId = FxtCompanyId });
                            return reslut>0;
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
        /// 直接删除楼栋图片(谨慎使用)
        /// </summary>
        /// <param name="CityId"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        private bool DeleteBuild(int CityId, int FxtCompanyId, int Id)
        {
            try
            {
                string sql = " delete FxtData_Biz.dbo.LNK_B_Photo with(rowlock) where FxtCompanyId in(25," + FxtCompanyId + ") and Id=" + Id + " and CityId=" + CityId + " "
                      + " delete FxtData_Biz.dbo.LNK_B_Photo_sub with(rowlock) where FxtCompanyId in(25," + FxtCompanyId + ") and Id=" + Id + " and CityId=" + CityId + " ";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtDataBiz))
                {
                    int result = con.Execute(sql);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
