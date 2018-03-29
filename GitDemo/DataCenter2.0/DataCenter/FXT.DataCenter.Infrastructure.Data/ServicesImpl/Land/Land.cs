using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class DAT_LandDAL : IDAT_Land
    {
        #region 添加
        /// <summary>
        /// 新增土地信息
        /// </summary>
        /// <param name="modal">土地模型</param>
        /// <returns></returns>
        public int AddDAT_Land(DAT_Land modal)
        {

            if (string.IsNullOrEmpty(modal.fieldno)) modal.fieldno = "";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [FxtLand].[dbo].[DAT_Land] with(rowlock)(");
            strSql.Append("[FxtCompanyId],[CityID],[AreaId],[SubAreaId],[LandNo],[FieldNo],[MapNo],[LandName],[Address],[LandTypeCode]");
            strSql.Append(",[UseTypeCode],[StartDate],[EndDate],[UseYear],[PlanPurpose],[FactPurpose],[LandArea],[BuildingArea]");
            strSql.Append(",[CubageRate],[MaxCubageRate],[MinCubageRate],[CoveRage],[MaxCoveRage],[GreenRage],[MinGreenRage]");
            strSql.Append(",[LandShapeCode],[DevelopmentCode],[LandUseStatus],[LandClass] ,[HeightLimited],[PlanLimited],[East]");
            strSql.Append(",[West],[South],[North],[LandOwnerId],[LandUseId],[BusinessCenterDistance],[Traffic],[Infrastructure]");
            strSql.Append(",[PublicService],[EnvironmentCode],[LicenceDate],[LandDetail],[Weight],[Coefficient],[X],[Y],[XYScale]");
            strSql.Append(",[CreateDate],[Creator],[SaveDate],[SaveUser],[Remark],[Valid]");
            strSql.Append(" )VALUES(");
            strSql.Append("@FxtCompanyId,@CityID,@AreaId,@SubAreaId,@LandNo,@FieldNo,@MapNo,@LandName,@Address,@LandTypeCode");
            strSql.Append(",@UseTypeCode,@StartDate,@EndDate,@UseYear,@PlanPurpose,@FactPurpose,@LandArea,@BuildingArea");
            strSql.Append(",@CubageRate,@MaxCubageRate,@MinCubageRate,@CoveRage,@MaxCoveRage,@GreenRage,@MinGreenRage");
            strSql.Append(",@LandShapeCode,@DevelopmentCode,@LandUseStatus,@LandClass,@HeightLimited,@PlanLimited,@East");
            strSql.Append(",@West,@South,@North,@LandOwnerId,@LandUseId,@BusinessCenterDistance,@Traffic,@Infrastructure");
            strSql.Append(",@PublicService,@EnvironmentCode,@LicenceDate,@LandDetail,@Weight,@Coefficient,@X,@Y,@XYScale");
            strSql.Append(",@CreateDate,@Creator,@SaveDate,@SaveUser,@Remark,@Valid");
            strSql.Append(");Select SCOPE_IDENTITY() as Id ");
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                ////IDbTransaction transaction = con.BeginTransaction();
                //int reslut = con.Execute(strSql.ToString(), modal);
                //if (reslut > 0)
                //{
                //    dynamic identity = con.Query("SELECT max(LandId) as Id from [FxtLand].[dbo].[DAT_Land] with(nolock)", null).Single();
                //    int newId = Convert.ToInt32(identity.Id);
                //    if (newId < 1)
                //    {
                //        //transaction.Rollback();
                //        return 0;
                //    }
                //    else
                //    {

                //        return AddLandXY(modal, newId);
                //    }
                //}
                //return 0;
                dynamic identity = con.Query(strSql.ToString(), modal).Single();
                int newId = Convert.ToInt32(identity.Id);
                if (newId > 0)
                {
                    return AddLandXY(modal, newId);
                }
                return newId;
            }
        }
        /// <summary>
        /// 添加坐标
        /// </summary>
        /// <param name="modal"></param>
        /// <param name="transaction"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        private static int AddLandXY(DAT_Land modal, int newId)
        {
            try
            {
                var a = new LandCoordinate();
                if (modal.LngOrLat != null && modal.LngOrLat.Contains("|"))
                {
                    string[] lngOrlat = modal.LngOrLat.Split('|');
                    for (int i = 0; i < lngOrlat.Length; i++)
                    {
                        DAT_Land_Coordinate mo = new DAT_Land_Coordinate();
                        mo.landid = Convert.ToInt64(newId);
                        mo.cityid = modal.cityid;
                        mo.fxtcompanyid = modal.fxtcompanyid;
                        mo.x = Convert.ToDecimal(lngOrlat[i].Split(',')[0]);
                        mo.y = Convert.ToDecimal(lngOrlat[i].Split(',')[1]);
                        mo.valid = modal.valid;
                        a.AddLandCoordinate(mo);
                    }
                }
                else
                {
                    DAT_Land_Coordinate mo = new DAT_Land_Coordinate();
                    mo.landid = Convert.ToInt64(newId);
                    mo.cityid = modal.cityid;
                    mo.fxtcompanyid = modal.fxtcompanyid;
                    if (modal.LngOrLat != null && modal.LngOrLat.Contains(","))
                    {
                        if (Convert.ToInt32(Convert.ToDecimal(modal.LngOrLat.Split(',')[0])) > 0 || Convert.ToInt32(Convert.ToDecimal(modal.LngOrLat.Split(',')[1])) > 0)
                        {
                            mo.valid = 1;
                        }
                        else
                        {
                            mo.valid = 0;
                        }
                        mo.x = Convert.ToDecimal(modal.LngOrLat.Split(',')[0]);
                        mo.y = Convert.ToDecimal(modal.LngOrLat.Split(',')[1]);
                    }
                    else
                    {
                        mo.valid = 0;
                        mo.x = Convert.ToDecimal(modal.LngOrLat);
                        mo.y = Convert.ToDecimal(modal.LngOrLat);
                    }
                    a.AddLandCoordinate(mo);
                }
                //transaction.Commit();
            }
            catch
            {

                //transaction.Rollback();
                return 0;
            }
            return newId;
        }
        /// <summary>
        /// excel导入
        /// </summary>
        /// <param name="la"></param>
        /// <returns></returns>
        public int AddExcelImport(DAT_Land la)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO [FxtLand].[dbo].[DAT_Land] with(rowlock)(");
            strSql.Append("[FxtCompanyId],[CityID],[AreaId],[SubAreaId],[LandNo],[FieldNo],[MapNo],[LandName],[Address],[LandTypeCode]");
            strSql.Append(",[UseTypeCode],[StartDate],[EndDate],[UseYear],[PlanPurpose],[FactPurpose],[LandArea],[BuildingArea]");
            strSql.Append(",[CubageRate],[MaxCubageRate],[MinCubageRate],[CoveRage],[MaxCoveRage],[GreenRage],[MinGreenRage]");
            strSql.Append(",[LandShapeCode],[DevelopmentCode],[LandUseStatus],[LandClass] ,[HeightLimited],[PlanLimited],[East]");
            strSql.Append(",[West],[South],[North],[LandOwnerId],[LandUseId],[BusinessCenterDistance],[Traffic],[Infrastructure]");
            strSql.Append(",[PublicService],[EnvironmentCode],[LicenceDate],[LandDetail],[Weight],[Coefficient],[X],[Y],[XYScale]");
            strSql.Append(",[CreateDate],[Creator],[SaveDate],[SaveUser],[Remark],[Valid]");
            strSql.Append(" )VALUES(");
            strSql.Append("@FxtCompanyId,@CityID,@AreaId,@SubAreaId,@LandNo,@FieldNo,@MapNo,@LandName,@Address,@LandTypeCode");
            strSql.Append(",@UseTypeCode,@StartDate,@EndDate,@UseYear,@PlanPurpose,@FactPurpose,@LandArea,@BuildingArea");
            strSql.Append(",@CubageRate,@MaxCubageRate,@MinCubageRate,@CoveRage,@MaxCoveRage,@GreenRage,@MinGreenRage");
            strSql.Append(",@LandShapeCode,@DevelopmentCode,@LandUseStatus,@LandClass,@HeightLimited,@PlanLimited,@East");
            strSql.Append(",@West,@South,@North,@LandOwnerId,@LandUseId,@BusinessCenterDistance,@Traffic,@Infrastructure");
            strSql.Append(",@PublicService,@EnvironmentCode,@LicenceDate,@LandDetail,@Weight,@Coefficient,@X,@Y,@XYScale");
            strSql.Append(",@CreateDate,@Creator,@SaveDate,@SaveUser,@Remark,@Valid");
            strSql.Append(")");
            try
            {

                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    int reslut = con.Execute(strSql.ToString(), la);
                    return reslut;
                }
            }
            catch
            {

                return 0;
            }
        }


        #endregion
        #region 更新
        /// <summary>
        /// 根据土地信息Id更新土地信息
        /// </summary>
        /// <param name="modal">土地模型</param>
        /// <param name="landId">土地Id</param>
        /// <param name="fxtcompanyId">fxtcompanyId</param>
        /// <returns></returns>
        public int UpdateDAT_Land(DAT_Land modal, int currFxtcompanyId)
        {
            modal.planpurpose = modal.opValue;
            if (string.IsNullOrEmpty(modal.fieldno)) modal.fieldno = "";
            string sql = "";
            //if (!modal.command)//修改主表
            //{
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                //IDbTransaction transaction = con.BeginTransaction();
                #region --------------------------------Sql-------------------------------
                string table = " update FxtLand.dbo.DAT_Land with(rowlock) ";
                string attr = @"  set AreaId=@AreaId,SubAreaId=@SubAreaId,landno = @landno,fieldno = @fieldno,mapno = @mapno,landname = @landname,address = @address,
                            landtypecode = @landtypecode,usetypecode = @usetypecode,startdate = @startdate,enddate = @enddate,
                            useyear = @useyear,planpurpose = @planpurpose,factpurpose = @factpurpose,landarea = @landarea,
                            buildingarea = @buildingarea,cubagerate = @cubagerate,maxcubagerate = @maxcubagerate,mincubagerate = @mincubagerate,
                            coverage = @coverage,maxcoverage = @maxcoverage,greenrage = @greenrage,mingreenrage = @mingreenrage,
                            landshapecode = @landshapecode,developmentcode = @developmentcode,landusestatus = @landusestatus,landclass = @landclass,
                            heightlimited = @heightlimited,planlimited = @planlimited,east = @east,west = @west,south = @south,
                            north = @north,landownerid = @landownerid,landuseid = @landuseid,businesscenterdistance = @businesscenterdistance,
                            traffic = @traffic,infrastructure = @infrastructure,publicservice = @publicservice,environmentcode = @environmentcode,
                            licencedate = @licencedate,landdetail = @landdetail,weight = @weight,coefficient = @coefficient,x = @x,
                            y = @y,xyscale = @xyscale,createdate = @createdate,creator = @creator,savedate = @savedate,saveuser = @saveuser,
                            remark = @remark,valid = @valid ";
                string table_sub = " update FxtLand.dbo.DAT_Land_sub with(rowlock) ";
                string where = "  where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                #endregion
                if (modal.fxtcompanyid.ToString() == ConfigurationHelper.FxtCompanyId)
                {

                    sql = table + attr + where;
                    int result = con.Execute(sql, modal);
                    if (result > 0)
                    {
                        int num = UpdataLandXY(modal);
                        //if (num > 0)
                        //    transaction.Commit();
                        //else
                        //    transaction.Rollback();
                        return num;
                    }
                    else
                    {
                        //transaction.Rollback();
                        return 0;
                    }

                }
                if (modal.fxtcompanyid == currFxtcompanyId)
                {
                    sql = table + attr + where;
                    int result = con.Execute(sql, modal);
                    if (result > 0)
                    {
                        int num = UpdataLandXY(modal);
                        //if (num > 0)
                        //    transaction.Commit();
                        //else
                        //    transaction.Rollback();
                        return num;
                    }
                    else
                    {
                        //transaction.Rollback();
                        return 0;
                    }
                }
                else
                {
                    sql = "select landid from FxtLand.dbo.DAT_Land_sub with(rowlock) where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                    var list_sub = con.Query<DAT_Land>(sql, new { LandId = modal.landid, CityID = modal.cityid, FxtCompanyId = modal.fxtcompanyid });
                    if (list_sub != null && list_sub.Count() > 0)
                    {
                        sql = table_sub + attr + where;
                        int result = con.Execute(sql, modal);
                        if (result > 0)
                        {
                            int num = UpdataLandXY(modal);
                            //if (num > 0)
                            //    transaction.Commit();
                            //else
                            //    transaction.Rollback();
                            return num;
                        }
                        else
                        {
                            //transaction.Rollback();
                            return 0;
                        }
                    }
                    else
                    {
                        sql = "select landid from FxtLand.dbo.DAT_Land with(rowlock) where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                        var list = con.Query<DAT_Land>(sql, new { LandId = modal.landid, CityID = modal.cityid, FxtCompanyId = modal.fxtcompanyid });
                        if (list != null && list.Count() > 0)
                        {
                            sql = table + attr + where;
                            int result = con.Execute(sql, modal);
                            if (result > 0)
                            {
                                int num = UpdataLandXY(modal);
                                //if (num > 0)
                                //    transaction.Commit();
                                //else
                                //    transaction.Rollback();
                                return num;
                            }
                            else
                            {
                                //transaction.Rollback();
                                return 0;
                            }
                        }
                        else
                        {
                            sql = @"INSERT INTO [FxtLand].[dbo].[DAT_Land]_sub with(rowlock)
                                   (landId,fxtcompanyid,cityid,areaid,subareaid,landno,fieldno,mapno,landname,address,
                                    landtypecode,usetypecode,startdate,enddate,useyear,planpurpose,factpurpose,landarea,
                                    buildingarea,cubagerate,maxcubagerate,mincubagerate,coverage,maxcoverage,greenrage,
                                    mingreenrage,landshapecode,developmentcode,landusestatus,landclass,heightlimited,
                                    planlimited,east,west,south,north,landownerid,landuseid,businesscenterdistance,
                                    traffic,infrastructure,publicservice,environmentcode,licencedate,landdetail,weight,
                                    coefficient,x,y,xyscale,createdate,creator,savedate,saveuser,remark,valid) 
                             select
                                    landId,'" + currFxtcompanyId + @"' as fxtcompanyid,cityid,areaid,subareaid,landno,fieldno,mapno,landname,address,
                                    landtypecode,usetypecode,startdate,enddate,useyear,planpurpose,factpurpose,landarea,
                                    buildingarea,cubagerate,maxcubagerate,mincubagerate,coverage,maxcoverage,greenrage,
                                    mingreenrage,landshapecode,developmentcode,landusestatus,landclass,heightlimited,
                                    planlimited,east,west,south,north,landownerid,landuseid,businesscenterdistance,
                                    traffic,infrastructure,publicservice,environmentcode,licencedate,landdetail,weight,
                                    coefficient,x,y,xyscale,createdate,creator,savedate,saveuser,remark,valid 
                            from [FxtLand].[dbo].[DAT_Land] 
                            where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                            int result = con.Execute(sql, new { LandId = modal.landid, CityID = modal.cityid, FxtCompanyId = modal.fxtcompanyid });
                            //if (result > 0)
                            //    transaction.Commit();
                            //else
                            //    transaction.Rollback();
                            return result;
                        }
                    }
                }

            }

        }

        public int UpdateLandInfo4Excel(DAT_Land modal, int currFxtcompanyId, List<string> modifiedProperty)
        {
            modal.planpurpose = modal.opValue;
            if (string.IsNullOrEmpty(modal.fieldno)) modal.fieldno = "";

            using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                if (!modifiedProperty.Any()) return 0;

                var field = modifiedProperty.Aggregate(" set ", (current, item) => current + item);

                #region 注释
                //                var attr = @"  set AreaId=@AreaId,SubAreaId=@SubAreaId,landno = @landno,fieldno = @fieldno,mapno = @mapno,landname = @landname,address = @address,
                //                            landtypecode = @landtypecode,usetypecode = @usetypecode,startdate = @startdate,enddate = @enddate,
                //                            useyear = @useyear,planpurpose = @planpurpose,factpurpose = @factpurpose,landarea = @landarea,
                //                            buildingarea = @buildingarea,cubagerate = @cubagerate,maxcubagerate = @maxcubagerate,mincubagerate = @mincubagerate,
                //                            coverage = @coverage,maxcoverage = @maxcoverage,greenrage = @greenrage,mingreenrage = @mingreenrage,
                //                            landshapecode = @landshapecode,developmentcode = @developmentcode,landusestatus = @landusestatus,landclass = @landclass,
                //                            heightlimited = @heightlimited,planlimited = @planlimited,east = @east,west = @west,south = @south,
                //                            north = @north,landownerid = @landownerid,landuseid = @landuseid,businesscenterdistance = @businesscenterdistance,
                //                            traffic = @traffic,infrastructure = @infrastructure,publicservice = @publicservice,environmentcode = @environmentcode,
                //                            licencedate = @licencedate,landdetail = @landdetail,weight = @weight,coefficient = @coefficient,x = @x,
                //                            y = @y,xyscale = @xyscale,createdate = @createdate,creator = @creator,savedate = @savedate,saveuser = @saveuser,
                //                            remark = @remark,valid = @valid ";
                #endregion

                var table = " update FxtLand.dbo.DAT_Land with(rowlock) ";
                var tableSub = " update FxtLand.dbo.DAT_Land_sub with(rowlock) ";
                var where = "  where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";

                string sql;
                if (modal.fxtcompanyid.ToString() == ConfigurationHelper.FxtCompanyId)
                {

                    sql = table + field + where;
                    var result = con.Execute(sql, modal);
                    if (result <= 0) return 0;
                    var num = UpdataLandXY(modal);

                    return num;
                }
                if (modal.fxtcompanyid == currFxtcompanyId)
                {
                    sql = table + field + where;
                    var result = con.Execute(sql, modal);
                    if (result <= 0) return 0;
                    var num = UpdataLandXY(modal);

                    return num;
                }
                sql = "select landid from FxtLand.dbo.DAT_Land_sub with(rowlock) where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                var listSub = con.Query<DAT_Land>(sql, new { LandId = modal.landid, CityID = modal.cityid, FxtCompanyId = modal.fxtcompanyid });
                if (listSub != null && listSub.Any())
                {
                    sql = tableSub + field + where;
                    var result = con.Execute(sql, modal);
                    if (result <= 0) return 0;
                    var num = UpdataLandXY(modal);

                    return num;
                }
                sql = "select landid from FxtLand.dbo.DAT_Land with(rowlock) where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                var list = con.Query<DAT_Land>(sql, new { LandId = modal.landid, CityID = modal.cityid, FxtCompanyId = modal.fxtcompanyid });
                if (list != null && list.Any())
                {
                    sql = table + field + where;
                    var result = con.Execute(sql, modal);
                    if (result <= 0) return 0;
                    var num = UpdataLandXY(modal);
                    return num;
                }
                else
                {
                    sql = @"INSERT INTO [FxtLand].[dbo].[DAT_Land]_sub with(rowlock)
                                   (landId,fxtcompanyid,cityid,areaid,subareaid,landno,fieldno,mapno,landname,address,
                                    landtypecode,usetypecode,startdate,enddate,useyear,planpurpose,factpurpose,landarea,
                                    buildingarea,cubagerate,maxcubagerate,mincubagerate,coverage,maxcoverage,greenrage,
                                    mingreenrage,landshapecode,developmentcode,landusestatus,landclass,heightlimited,
                                    planlimited,east,west,south,north,landownerid,landuseid,businesscenterdistance,
                                    traffic,infrastructure,publicservice,environmentcode,licencedate,landdetail,weight,
                                    coefficient,x,y,xyscale,createdate,creator,savedate,saveuser,remark,valid) 
                             select
                                    landId,'" + currFxtcompanyId + @"' as fxtcompanyid,cityid,areaid,subareaid,landno,fieldno,mapno,landname,address,
                                    landtypecode,usetypecode,startdate,enddate,useyear,planpurpose,factpurpose,landarea,
                                    buildingarea,cubagerate,maxcubagerate,mincubagerate,coverage,maxcoverage,greenrage,
                                    mingreenrage,landshapecode,developmentcode,landusestatus,landclass,heightlimited,
                                    planlimited,east,west,south,north,landownerid,landuseid,businesscenterdistance,
                                    traffic,infrastructure,publicservice,environmentcode,licencedate,landdetail,weight,
                                    coefficient,x,y,xyscale,createdate,creator,savedate,saveuser,remark,valid 
                            from [FxtLand].[dbo].[DAT_Land] 
                            where landid = @LandId and CityID=@CityID and FxtCompanyId=@FxtCompanyId";
                    var result = con.Execute(sql, new { LandId = modal.landid, CityID = modal.cityid, FxtCompanyId = modal.fxtcompanyid });

                    return result;
                }
            }

        }

        private static int UpdataLandXY(DAT_Land modal)
        {
            try
            {
                var a = new LandCoordinate();
                if (modal.LngOrLat != null && modal.LngOrLat.Contains("|"))
                {
                    a.DeleteLandCoordinate(modal.fxtcompanyid, modal.cityid, modal.landid);
                    string[] lngOrlat = modal.LngOrLat.Split('|');
                    for (int j = 0; j < lngOrlat.Length; j++)
                    {
                        DAT_Land_Coordinate mo = new DAT_Land_Coordinate();
                        mo.landid = modal.landid;
                        mo.cityid = modal.cityid;
                        mo.fxtcompanyid = modal.fxtcompanyid;
                        mo.x = Convert.ToDecimal(lngOrlat[j].Split(',')[0]);
                        mo.y = Convert.ToDecimal(lngOrlat[j].Split(',')[1]);
                        mo.valid = modal.valid;
                        a.AddLandCoordinate(mo);
                    }
                }
                else
                {
                    a.DeleteLandCoordinate(modal.fxtcompanyid, modal.cityid, modal.landid);
                    DAT_Land_Coordinate mo = new DAT_Land_Coordinate();
                    mo.landid = modal.landid;
                    mo.cityid = modal.cityid;
                    mo.fxtcompanyid = modal.fxtcompanyid;
                    if (modal.LngOrLat != null && modal.LngOrLat.Contains(","))
                    {
                        if (Convert.ToInt32(Convert.ToDecimal(modal.LngOrLat.Split(',')[0])) > 0 || Convert.ToInt32(Convert.ToDecimal(modal.LngOrLat.Split(',')[1])) > 0)
                        {
                            mo.valid = 1;
                        }
                        else
                        {
                            mo.valid = 0;
                        }
                        mo.x = Convert.ToDecimal(modal.LngOrLat.Split(',')[0]);
                        mo.y = Convert.ToDecimal(modal.LngOrLat.Split(',')[1]);
                    }
                    else
                    {
                        mo.valid = 0;
                        mo.x = Convert.ToDecimal(modal.LngOrLat);
                        mo.y = Convert.ToDecimal(modal.LngOrLat);
                    }
                    a.AddLandCoordinate(mo);
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        #endregion
        #region 删除
        /// <summary>
        /// 根据土地信息Id删除土地数据
        /// </summary>
        /// <param name="landId"></param>
        /// <returns></returns>
        public bool DeleteDAT_Land(int landId)
        {
            string sql = @"update FxtLand.dbo.DAT_Land with(rowlock)  set valid = 0 where landid = @landid";
            try
            {
                SqlParameter parameter = new SqlParameter("@landid", SqlDbType.Int);
                parameter.Value = landId;
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
                int reslut = DBHelperSql.ExecuteNonQuery(sql, parameter);
                if (reslut > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message); ;
            }

        }
        #endregion
        #region 查询
        /// <summary>
        /// 查询土地
        /// </summary>
        /// <param name="mode">土地条件</param>
        /// <returns></returns>
        public IQueryable<DAT_Land> GetAllLandInfo(DAT_Land mode, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {

            try
            {
                //查询字段
                string field = @" select land.LandId, land.FxtCompanyId, land.CityID as CityID, area.AreaId as AreaId, land.SubAreaId as SubAreaId, LandNo, FieldNo, MapNo, LandName, land.Address as [Address], 
                                     LandTypeCode, UseTypeCode, StartDate, EndDate, UseYear, planpurpose, factpurpose, LandArea, BuildingArea, 
                                     CubageRate, MaxCubageRate, MinCubageRate, CoveRage, MaxCoveRage, GreenRage, MinGreenRage, LandShapeCode, 
                                     DevelopmentCode, LandUseStatus, LandClass, HeightLimited, PlanLimited, East, West, South, North, LandOwnerId, 
                                     LandUseId, BusinessCenterDistance, Traffic, Infrastructure, PublicService, EnvironmentCode, LicenceDate,
                                     LandDetail, Weight, Coefficient, land.X as X, land.Y as Y, land.XYScale as XYScale, land.CreateDate as CreateDate, Creator, SaveDate, SaveUser, land.Remark, land.Valid 
                                    ,city.CityName as cityName,com.CompanyName as CompanyName,area.AreaName,SubAreaName,ltypename.codename as LandTypeName
                                    ,lxingzhi.codename as UserTypeName,lxingzhuang.codename as LandshapeName,lkaifa.codename as DevelopmentName,ldengji.codename as LandClassName
                                    ,lhuangj.codename as EnvironmentName ";
                string PTable = " from FxtLand.dbo.DAT_Land land with(nolock) ";
                string SubTable = " from FxtLand.dbo.DAT_Land_sub land with(nolock)  ";
                string LinkTable = @" left join FxtDataCenter.dbo.SYS_City city with(nolock) on land.CityID=city.CityId
                                  left join FxtDataCenter.dbo.SYS_Code ltypename with(nolock) on ltypename.code=land.LandTypeCode
                                  left join FxtDataCenter.dbo.SYS_Code lxingzhi with(nolock) on lxingzhi.code=land.UseTypeCode
                                  left join FxtDataCenter.dbo.SYS_Code lxingzhuang with(nolock) on lxingzhuang.code=land.LandShapeCode
                                  left join FxtDataCenter.dbo.SYS_Code lkaifa with(nolock) on lkaifa.code=land.DevelopmentCode
                                  left join FxtDataCenter.dbo.SYS_Code ldengji with(nolock) on ldengji.code=land.LandClass
                                  left join FxtDataCenter.dbo.SYS_Code lhuangj with(nolock) on lhuangj.code=land.EnvironmentCode
                                  left join FXTProject.dbo.Privi_Company com with(nolock) on land.FxtCompanyId=com.CompanyId
                                  left join FxtDataCenter.dbo.SYS_Area area with(nolock) on land.AreaId=area.AreaId and land.CityID=area.CityId
                                  left join FxtDataCenter.dbo.SYS_SubArea subarea with(nolock) on subarea.AreaId=land.AreaId and subarea.SubAreaId=land.SubAreaId
                                  left join FxtLand.dbo.DAT_Land_Coordinate xy with(nolock) on land.LandId=xy.Id";
                string Where = @" where not exists(select LandId from FxtLand.dbo.DAT_Land_sub landsub with(rowlock) where  
                                    landsub.FxtCompanyId=@FxtCompanyId and land.LandId=landsub.LandId and landsub.CityID=landsub.CityID) 
                                    and land.Valid=1 and land.CityID=@CityID ";
                string Where2 = @" where land.Valid=1 and land.FxtCompanyId=@FxtCompanyId and land.CityID=@CityID ";
                if (self)//查询自己
                {
                    Where += @" and land.FxtCompanyId=@FxtCompanyId ";
                }
                else//查询所有
                {
                    Where += @"  and (land.fxtCompanyid=@FxtCompanyId or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@CityID and fxtCompanyid=@FxtCompanyId and typecode= 1003002),','))) ";
                }


                if (mode.areaid > 0)
                {
                    Where += " and land.AreaId=@AreaId";
                    Where2 += " and land.AreaId=@AreaId";

                }
                if (mode.subareaid > 0)
                {
                    Where += " and land.SubAreaId=@SubAreaId";
                    Where2 += " and land.SubAreaId=@SubAreaId";

                }
                if (!string.IsNullOrEmpty(mode.landno))
                {
                    Where += " and land.LandNo like @LandNo";
                    Where2 += " and land.LandNo like @LandNo";
                }
                if (!string.IsNullOrEmpty(mode.fieldno))
                {
                    Where += " and land.FieldNo like @FieldNo";
                    Where2 += " and land.FieldNo like @FieldNo";

                }
                if (Convert.ToInt32(mode.landownerid) > 0)
                {
                    Where += " and land.LandOwnerId=@LandOwnerId";
                    Where2 += " and land.LandOwnerId=@LandOwnerId";

                }
                if (Convert.ToInt32(mode.landuseid) > 0)
                {
                    Where += " and land.LandUseId=@LandUseId";
                    Where2 += " and land.LandUseId=@LandUseId";

                }
                if (!string.IsNullOrWhiteSpace(mode.planpurpose))
                {
                    Where += " and land.PlanPurpose like @planpurpose";
                    Where2 += " and land.PlanPurpose like @planpurpose";

                }
                if (mode.startdate > DateTime.MinValue)
                {
                    Where += " and land.StartDate>=@StartDate";
                    Where2 += " and land.StartDate>=@StartDate";

                }
                if (mode.enddate > DateTime.MinValue)
                {
                    Where += " and land.StartDate<=@EndDate";
                    Where2 += " and land.StartDate<=@EndDate";

                }
                string sql = field + PTable + LinkTable + Where;
                sql += " union " + field + SubTable + LinkTable + Where2;
                string pageSql = "";
                //if (pageIndex == 1)
                //{
                //    //pageSql = "select top " + pageSize + "  * from (" + sql + ") as t1 order by t1.LandId desc";
                //}
                //else
                //{
                //    //pageSql = "select top " + pageSize + "  * from (" + sql + ") as t1 " +
                //    //" where (t1.LandId>(select max(t3.LandId)" +
                //    //" from (select top " + (pageIndex - 1) * pageSize + " t2.LandId from (" + sql + ") as t2 order by t2.LandId) as" +
                //    //" t3)) order by t1.LandId desc";
                //}
                pageSql = "select top " + pageSize + " tt.*";
                pageSql += " from (";
                pageSql += " select ROW_NUMBER() over(order by landid desc) rownumber,t.*";
                pageSql += " from (" + sql + ") t";
                pageSql += " ) tt";
                pageSql += " where tt.rownumber>" + (pageIndex - 1) * pageSize + "";
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    //总记录数
                    #region 总记录数
                    string countSql = "select count(1) from (" + sql + ") as t1";
                    totalCount = con.Query<int>(countSql, new
                    {
                        CityID = mode.cityid,
                        FxtCompanyId = mode.fxtcompanyid,
                        AreaId = mode.areaid,
                        SubAreaId = mode.subareaid,
                        LandNo = "%" + mode.landno + "%",
                        FieldNo = "%" + mode.fieldno + "%",
                        LandOwnerId = mode.landownerid,
                        LandUseId = mode.landuseid,
                        planpurpose = "%" + mode.planpurpose + "%",
                        StartDate = Convert.ToDateTime(mode.startdate).ToString("yyyy-MM-dd") + " 00:00:00",
                        EndDate = Convert.ToDateTime(mode.enddate).ToString("yyyy-MM-dd") + " 23:59:59"
                    }).FirstOrDefault();
                    #endregion
                    #region 土地列表
                    var landList = con.Query<DAT_Land>(pageSql, new
                    {
                        CityID = mode.cityid,
                        FxtCompanyId = mode.fxtcompanyid,
                        AreaId = mode.areaid,
                        SubAreaId = mode.subareaid,
                        LandNo = "%" + mode.landno + "%",
                        FieldNo = "%" + mode.fieldno + "%",
                        LandOwnerId = mode.landownerid,
                        LandUseId = mode.landuseid,
                        planpurpose = "%" + mode.planpurpose + "%",
                        StartDate = Convert.ToDateTime(mode.startdate).ToString("yyyy-MM-dd") + " 00:00:00",
                        EndDate = Convert.ToDateTime(mode.enddate).ToString("yyyy-MM-dd") + " 23:59:59"
                    }).AsQueryable();
                    #endregion
                    return landList;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public IQueryable<DAT_Land> GetAllLandInfoImport(DAT_Land mode, bool self = true)
        {

            try
            {
                //查询字段
                string field = @" select land.LandId, land.FxtCompanyId, land.CityID as CityID, area.AreaId as AreaId, land.SubAreaId as SubAreaId, LandNo, FieldNo, MapNo, LandName, land.Address as [Address], 
                                     LandTypeCode, UseTypeCode, StartDate, EndDate, UseYear, planpurpose, factpurpose, LandArea, BuildingArea, 
                                     CubageRate, MaxCubageRate, MinCubageRate, CoveRage, MaxCoveRage, GreenRage, MinGreenRage, LandShapeCode, 
                                     DevelopmentCode, LandUseStatus, LandClass, HeightLimited, PlanLimited, East, West, South, North, LandOwnerId, 
                                     LandUseId, BusinessCenterDistance, Traffic, Infrastructure, PublicService, EnvironmentCode, LicenceDate,
                                     LandDetail, Weight, Coefficient, land.X as X, land.Y as Y, land.XYScale as XYScale, land.CreateDate as CreateDate, Creator, SaveDate, SaveUser, land.Remark, land.Valid 
                                    ,city.CityName as cityName,com.CompanyName as CompanyName,area.AreaName,SubAreaName,xy.X,xy.Y,ltypename.codename as LandTypeName
                                    ,lxingzhi.codename as UserTypeName,lxingzhuang.codename as LandshapeName,lkaifa.codename as DevelopmentName,ldengji.codename as LandClassName
                                    ,lhuangj.codename as EnvironmentName ";
                string PTable = " from FxtLand.dbo.DAT_Land land with(nolock) ";
                string SubTable = " from FxtLand.dbo.DAT_Land_sub land with(nolock)  ";
                string LinkTable = @" left join FxtDataCenter.dbo.SYS_City city with(nolock) on land.CityID=city.CityId
                                  left join FxtDataCenter.dbo.SYS_Code ltypename with(nolock) on ltypename.code=land.LandTypeCode
                                  left join FxtDataCenter.dbo.SYS_Code lxingzhi with(nolock) on lxingzhi.code=land.UseTypeCode
                                  left join FxtDataCenter.dbo.SYS_Code lxingzhuang with(nolock) on lxingzhuang.code=land.LandShapeCode
                                  left join FxtDataCenter.dbo.SYS_Code lkaifa with(nolock) on lkaifa.code=land.DevelopmentCode
                                  left join FxtDataCenter.dbo.SYS_Code ldengji with(nolock) on ldengji.code=land.LandClass
                                  left join FxtDataCenter.dbo.SYS_Code lhuangj with(nolock) on lhuangj.code=land.EnvironmentCode
                                  left join FXTProject.dbo.Privi_Company com with(nolock) on land.FxtCompanyId=com.CompanyId
                                  left join FxtDataCenter.dbo.SYS_Area area with(nolock) on land.AreaId=area.AreaId and land.CityID=area.CityId
                                  left join FxtDataCenter.dbo.SYS_SubArea subarea with(nolock) on subarea.AreaId=land.AreaId and subarea.SubAreaId=land.SubAreaId
                                  left join FxtLand.dbo.DAT_Land_Coordinate xy with(nolock) on land.LandId=xy.Id";
                string Where = @" where not exists(select LandId from FxtLand.dbo.DAT_Land_sub landsub with(rowlock) where  
                                    landsub.FxtCompanyId=@FxtCompanyId and land.LandId=landsub.LandId and landsub.CityID=landsub.CityID) 
                                    and land.Valid=1 and land.CityID=@CityID ";
                string Where2 = @" where land.Valid=1 and land.FxtCompanyId=@FxtCompanyId and land.CityID=@CityID ";
                if (self)//查询自己
                {
                    Where += @" and land.FxtCompanyId=@FxtCompanyId ";
                }
                else//查询所有
                {
                    Where += @"  and (land.fxtCompanyid=@FxtCompanyId or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@CityID and fxtCompanyid=@FxtCompanyId and typecode= 1003002),','))) ";
                }


                if (mode.areaid > 0)
                {
                    Where += " and land.AreaId=@AreaId";
                    Where2 += " and land.AreaId=@AreaId";

                }
                if (mode.subareaid > 0)
                {
                    Where += " and land.SubAreaId=@SubAreaId";
                    Where2 += " and land.SubAreaId=@SubAreaId";

                }
                if (!string.IsNullOrEmpty(mode.landno))
                {
                    Where += " and land.LandNo like @LandNo";
                    Where2 += " and land.LandNo like @LandNo";
                }
                if (!string.IsNullOrEmpty(mode.fieldno))
                {
                    Where += " and land.FieldNo like @FieldNo";
                    Where2 += " and land.FieldNo like @FieldNo";

                }
                if (Convert.ToInt32(mode.landownerid) > 0)
                {
                    Where += " and land.LandOwnerId=@LandOwnerId";
                    Where2 += " and land.LandOwnerId=@LandOwnerId";

                }
                if (Convert.ToInt32(mode.landuseid) > 0)
                {
                    Where += " and land.LandUseId=@LandUseId";
                    Where2 += " and land.LandUseId=@LandUseId";

                }
                if (Convert.ToInt32(mode.planpurpose) > 0)
                {
                    Where += " and land.PlanPurpose like @planpurpose";
                    Where2 += " and land.PlanPurpose like @planpurpose";

                }
                if (mode.startdate > DateTime.MinValue)
                {
                    Where += " and land.StartDate>=@StartDate";
                    Where2 += " and land.StartDate>=@StartDate";

                }
                if (mode.enddate > DateTime.MinValue)
                {
                    Where += " and land.StartDate<=@EndDate";
                    Where2 += " and land.StartDate<=@EndDate";

                }
                string sql = field + PTable + LinkTable + Where;
                sql += " union " + field + SubTable + LinkTable + Where2;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    var landList = con.Query<DAT_Land>(sql, new
                    {
                        CityID = mode.cityid,
                        FxtCompanyId = mode.fxtcompanyid,
                        AreaId = mode.areaid,
                        SubAreaId = mode.subareaid,
                        LandNo = "%" + mode.landno + "%",
                        FieldNo = "%" + mode.fieldno + "%",
                        LandOwnerId = mode.landownerid,
                        LandUseId = mode.landuseid,
                        planpurpose = "%" + mode.planpurpose + "%",
                        StartDate = mode.startdate + " 00:00:00",
                        EndDate = mode.enddate + " 23:59:59"
                    }).AsQueryable();
                    return landList;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public DAT_Land GetAllLandInfo(int fxtcompanyId, int cityId, int areaId, string landNo)
        {
            try
            {
                //查询字段
                string field = @" select LandId, FxtCompanyId, CityID, AreaId, SubAreaId, LandNo, FieldNo, MapNo, 
                            LandName, Address, LandTypeCode, UseTypeCode, StartDate, EndDate, UseYear, PlanPurpose, 
                            FactPurpose,LandArea, BuildingArea, CubageRate, MaxCubageRate, MinCubageRate, CoveRage, 
                            MaxCoveRage,GreenRage, MinGreenRage, LandShapeCode, DevelopmentCode, LandUseStatus, LandClass, 
                            HeightLimited, PlanLimited, East, West, South, North, LandOwnerId, LandUseId, 
                            BusinessCenterDistance, Traffic, Infrastructure, PublicService, EnvironmentCode, 
                            LicenceDate, LandDetail, Weight, Coefficient, X, Y, XYScale, CreateDate, 
                            Creator, SaveDate, SaveUser, Remark, Valid ";
                string PTable = " from FxtLand.dbo.DAT_Land land with(nolock) ";
                string SubTable = " from FxtLand.dbo.DAT_Land_sub land with(nolock)  ";
                string Where = @" where not exists(select LandId from FxtLand.dbo.DAT_Land_sub landsub with(nolock) where  
                                    landsub.FxtCompanyId=@FxtCompanyId and land.LandId=landsub.LandId and landsub.CityID=landsub.CityID) 
                                    and land.Valid=1 and land.FxtCompanyId=@FxtCompanyId and land.CityID=@CityID and AreaId=@AreaId and LandNo = @LandNo  ";
                string Where2 = @" where land.Valid=1 and land.FxtCompanyId=@FxtCompanyId and land.CityID=@CityID and AreaId=@AreaId and LandNo = @LandNo  ";
                string sql = field + PTable + Where;
                sql += " union " + field + SubTable + Where2;
                using (IDbConnection con = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
                {
                    return con.Query<DAT_Land>(sql, new { FxtCompanyId = fxtcompanyId, CityID = cityId, AreaId = areaId, LandNo = landNo }).AsQueryable().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 查询土地
        /// </summary>
        /// <param name="landName">土地Id</param>
        /// <param name="cityId">城市Id</param>
        /// <param name="companyId">评估机构Id</param>
        /// <returns></returns>
        public DAT_Land GetAllLandByLandId(int landId, int cityId, int companyId)
        {
            DAT_Land land = new DAT_Land();

            try
            {
                string strSql = @"select land.LandId, land.FxtCompanyId, land.CityID as CityID, area.AreaId as AreaId, land.SubAreaId as SubAreaId, LandNo, FieldNo, MapNo, LandName, land.Address as [Address], 
                                     LandTypeCode, UseTypeCode, StartDate, EndDate, UseYear, PlanPurpose, FactPurpose, LandArea, BuildingArea, 
                                     CubageRate, MaxCubageRate, MinCubageRate, CoveRage, MaxCoveRage, GreenRage, MinGreenRage, LandShapeCode, 
                                     DevelopmentCode, LandUseStatus, LandClass, HeightLimited, PlanLimited, East, West, South, North, LandOwnerId, 
                                     LandUseId, BusinessCenterDistance, Traffic, Infrastructure, PublicService, EnvironmentCode, LicenceDate,
                                     LandDetail, Weight, Coefficient, land.X as X, land.Y as Y, land.XYScale as XYScale, land.CreateDate as CreateDate, Creator, SaveDate, SaveUser, land.Remark, land.Valid 
,city.CityName as cityName,com.CompanyName as CompanyName,area.AreaName,SubAreaName,FxtLand.dbo.Fun_GetLandXYList(landcoo.LandId,landcoo.FxtCompanyId,landcoo.CityID,'|')as LngOrLat,lo.ChineseName as LandOwnerName,lu.ChineseName as LandUseName
from FxtLand.dbo.DAT_Land land
left join FxtDataCenter.dbo.SYS_City city with(nolock) on land.CityID=city.CityId
left join FXTProject.dbo.Privi_Company com with(nolock) on land.FxtCompanyId=com.CompanyId
left join FxtDataCenter.dbo.SYS_Area area with(nolock) on land.AreaId=area.AreaId and land.CityID=area.CityId
left join FxtDataCenter.dbo.SYS_SubArea subarea with(nolock) on subarea.AreaId=land.AreaId and subarea.SubAreaId=land.SubAreaId
left join FxtLand.dbo.DAT_Land_Coordinate landcoo with(nolock) on land.LandId=landcoo.LandId
left join FxtDataCenter.dbo.DAT_Company lo with(nolock) on lo.CompanyId=land.LandOwnerId
left join FxtDataCenter.dbo.DAT_Company lu with(nolock) on lu.CompanyId=land.LandUseId
where land.Valid=1  and land.CityID=@CityID and land.LandId=@landid
and not exists(select LandId from FxtLand.dbo.DAT_Land_sub landsub with(rowlock) where landsub.FxtCompanyId=@FxtCompanyId and land.LandId=landsub.LandId and landsub.CityID=landsub.CityID) and (land.fxtCompanyid=@FxtCompanyId or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@CityID and fxtCompanyid=@FxtCompanyId and typecode= 1003002),','))) 
union 
select land.LandId, land.FxtCompanyId, land.CityID as CityID, area.AreaId as AreaId, land.SubAreaId as SubAreaId, LandNo, FieldNo, MapNo, LandName, land.Address as [Address], 
                                     LandTypeCode, UseTypeCode, StartDate, EndDate, UseYear, PlanPurpose, FactPurpose, LandArea, BuildingArea, 
                                     CubageRate, MaxCubageRate, MinCubageRate, CoveRage, MaxCoveRage, GreenRage, MinGreenRage, LandShapeCode, 
                                     DevelopmentCode, LandUseStatus, LandClass, HeightLimited, PlanLimited, East, West, South, North, LandOwnerId, 
                                     LandUseId, BusinessCenterDistance, Traffic, Infrastructure, PublicService, EnvironmentCode, LicenceDate,
                                     LandDetail, Weight, Coefficient, land.X as X, land.Y as Y, land.XYScale as XYScale, land.CreateDate as CreateDate, Creator, SaveDate, SaveUser, land.Remark, land.Valid 
,city.CityName as cityName,com.CompanyName as CompanyName,area.AreaName,SubAreaName,FxtLand.dbo.Fun_GetLandXYList(landcoo.LandId,landcoo.FxtCompanyId,landcoo.CityID,'|')as LngOrLat,lo.ChineseName as LandOwnerName,lu.ChineseName as LandUseName 
from FxtLand.dbo.DAT_Land_sub land
left join FxtDataCenter.dbo.SYS_City city with(nolock) on land.CityID=city.CityId
left join FXTProject.dbo.Privi_Company com with(nolock) on land.FxtCompanyId=com.CompanyId
left join FxtDataCenter.dbo.SYS_Area area with(nolock) on land.AreaId=area.AreaId and land.CityID=area.CityId
left join FxtDataCenter.dbo.SYS_SubArea subarea with(nolock) on subarea.AreaId=land.AreaId and subarea.SubAreaId=land.SubAreaId
left join FxtLand.dbo.DAT_Land_Coordinate landcoo with(nolock) on land.LandId=landcoo.LandId
left join FxtDataCenter.dbo.DAT_Company lo with(nolock) on lo.CompanyId=land.LandOwnerId
left join FxtDataCenter.dbo.DAT_Company lu with(nolock) on lu.CompanyId=land.LandUseId
where land.Valid=1 and land.FxtCompanyId=@FxtCompanyId and land.CityID=@CityID and land.LandId=@landid";
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
                SqlParameter[] parameter = {
                                           new SqlParameter("@FxtCompanyId", SqlDbType.Int),
                                           new SqlParameter("@CityID", SqlDbType.Int),
                                           new SqlParameter("@landid", SqlDbType.Int),
                                       };
                parameter[0].Value = companyId;
                parameter[1].Value = cityId;
                parameter[2].Value = landId;
                land = SqlModelHelper<DAT_Land>.GetSingleObjectBySql(strSql, parameter);
                return land;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// 验证宗地号是否唯一
        /// </summary>
        /// <param name="citiId">城市Id</param>
        /// <param name="fxtcomId">公司ID</param>
        /// <param name="landNo">宗地号</param>
        /// <returns></returns>
        public bool ValidLandNo(int citiId, int fxtcomId, string landNo)
        {
            string sql = @"select LandNo from FxtLand.dbo.DAT_Land land with(nolock)
                           where land.CityID=@CityID 
                                 and land.Valid=1 and land.CityID=@CityID
                                 and land.LandNo=@LandNo
                                 and (land.fxtCompanyid=@FxtCompanyId or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@CityID and fxtCompanyid=@FxtCompanyId and typecode= 1003002),','))) 
                                 and not exists(select LandId from FxtLand.dbo.DAT_Land_sub landsub with(rowlock) where landsub.FxtCompanyId=@FxtCompanyId and land.LandId=landsub.LandId and landsub.CityID=landsub.CityID)
                            union 
                            select LandNo from FxtLand.dbo.DAT_Land_sub land with(nolock)
                           where land.CityID=@CityID and land.FxtCompanyId=@FxtCompanyId and land.Valid=1 and land.LandNo=@LandNo";
            DBHelperSql.ConnectionString = ConfigurationHelper.FxtLand;
            SqlParameter[] parameter = {
                                           new SqlParameter("@CityID", citiId),
                                           new SqlParameter("@FxtCompanyId", fxtcomId),
                                           new SqlParameter("@LandNo", landNo),
                                       };
            var dt = DBHelperSql.ExecuteDataTable(sql, parameter);
            return dt != null && dt.Rows.Count > 0;
        }

        public IQueryable<string> GetLandNo(int cityId, int fxtCompanyId)
        {
            var strSql = @"select landNo from  FxtLand.dbo.DAT_Land land with(nolock)  
                           where land.CityID=@CityID 
                                 and land.Valid=1 and land.CityID=@CityID 
                                 and (land.fxtCompanyid=@FxtCompanyId or land.fxtCompanyid in(select [value] from fxtproject.dbo.[SplitToTable]((select LandCompanyId from FxtDataCenter.dbo.Privi_Company_ShowData with(nolock) where Cityid=@CityID and fxtCompanyid=@FxtCompanyId and typecode= 1003002),','))) 
                                 and not exists(select LandId from FxtLand.dbo.DAT_Land_sub landsub with(rowlock) where landsub.FxtCompanyId=@FxtCompanyId and land.LandId=landsub.LandId and landsub.CityID=landsub.CityID)  
                            union 
                            select LandNo from FxtLand.dbo.DAT_Land_sub land with(nolock) 
                           where land.CityID=@CityID and land.FxtCompanyId=@FxtCompanyId and land.Valid=1";

            using (IDbConnection conn = DapperAdapter.OpenConnection(ConfigurationHelper.FxtLand))
            {
                return conn.Query<string>(strSql, new { cityId, fxtCompanyId }).AsQueryable();
            }

        }
    }
}
