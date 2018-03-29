using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Entity.DBEntity;
using CAS.Entity.FxtProject;
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace FxtCenterService.DataAccess
{
    public class DatProjectCaseDA : Base
    {
        /// <summary>
        /// 插入案例数据
        /// </summary>
        /// <returns></returns>
        public static string Add(int cityid, string values)
        {
            try
            {
                DataSet ds = new DataSet();
                CityTable city = CityTableDA.GetCityTable(cityid);

                if (city.casetable == "dbo.DAT_Case")
                {
                    ds = GetProjectCaseDataTable(values);//DAT_Case表结构
                }
                else
                {
                    ds = GetProjectCaseDataTable2(values);//其它表结构
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(SqlServerSet.GetConnectionString(SqlServerSet.ConnectionName), SqlBulkCopyOptions.UseInternalTransaction))
                    {
                        sqlbulkcopy.DestinationTableName = city.casetable;//数据库中的表名
                        sqlbulkcopy.BulkCopyTimeout = 1200000;
                        sqlbulkcopy.WriteToServer(ds.Tables[0]);
                    }
                }
                return "成功";
            }
            catch (Exception ex)
            {
                LogHelper.Error("案例上传Add报错，ex=" + ex.ToString());
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量写入案例数据
        /// </summary>
        /// <param name="houseHisFinallist"></param>
        /// <returns></returns>
        private static DataSet GetProjectCaseDataTable(string values)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("CaseID", typeof(Int64)));
                dt.Columns.Add(new DataColumn("ProjectId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("BuildingId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("HouseId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CompanyId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CaseDate", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("PurposeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("FloorNumber", typeof(Int64)));
                dt.Columns.Add(new DataColumn("BuildingName", typeof(string)));
                dt.Columns.Add(new DataColumn("HouseNo", typeof(string)));
                dt.Columns.Add(new DataColumn("BuildingArea", typeof(string)));
                dt.Columns.Add(new DataColumn("UsableArea", typeof(string)));
                dt.Columns.Add(new DataColumn("FrontCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
                dt.Columns.Add(new DataColumn("MoneyUnitCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("SightCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CaseTypeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("StructureCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("BuildingTypeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("HouseTypeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("Creator", typeof(string)));
                dt.Columns.Add(new DataColumn("Remark", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
                dt.Columns.Add(new DataColumn("OldID", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CityID", typeof(Int64)));
                dt.Columns.Add(new DataColumn("Valid", typeof(Int64)));
                dt.Columns.Add(new DataColumn("FXTCompanyId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("TotalFloor", typeof(Int64)));
                dt.Columns.Add(new DataColumn("RemainYear", typeof(Int64)));
                dt.Columns.Add(new DataColumn("Depreciation", typeof(string)));
                dt.Columns.Add(new DataColumn("FitmentCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("SurveyId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("SaveDateTime", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("SaveUser", typeof(string)));
                dt.Columns.Add(new DataColumn("SourceName", typeof(string)));
                dt.Columns.Add(new DataColumn("SourceLink", typeof(string)));
                dt.Columns.Add(new DataColumn("SourcePhone", typeof(string)));
                dt.Columns.Add(new DataColumn("AreaId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("AreaName", typeof(string)));
                dt.Columns.Add(new DataColumn("BuildingDate", typeof(string)));
                dt.Columns.Add(new DataColumn("ZhuangXiu", typeof(string)));
                dt.Columns.Add(new DataColumn("SubHouse", typeof(string)));
                dt.Columns.Add(new DataColumn("PeiTao", typeof(string)));

                string[] caselist = Regex.Split(values, "②", RegexOptions.IgnoreCase);
                foreach (string value in caselist)
                {
                    string[] casedetail = Regex.Split(value, "①", RegexOptions.IgnoreCase);
                    int detalnum=0;
                    DataRow dr = dt.NewRow();
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CaseID"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CaseID"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["ProjectId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["ProjectId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["HouseId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["HouseId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CompanyId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CompanyId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CaseDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CaseDate"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["PurposeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["PurposeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FloorNumber"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FloorNumber"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingName"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingName"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["HouseNo"] = DBNull.Value;
                    }
                    else
                    {
                        dr["HouseNo"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingArea"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingArea"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["UsableArea"] = DBNull.Value;
                    }
                    else
                    {
                        dr["UsableArea"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FrontCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FrontCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["UnitPrice"] = DBNull.Value;
                    }
                    else
                    {
                        dr["UnitPrice"] = casedetail[detalnum];
                    }
                    dr["UnitPrice"] = casedetail[detalnum];
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["MoneyUnitCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["MoneyUnitCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SightCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SightCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CaseTypeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CaseTypeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["StructureCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["StructureCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingTypeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingTypeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["HouseTypeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["HouseTypeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CreateDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CreateDate"] = casedetail[detalnum];
                    }
                    detalnum++; 
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Creator"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Creator"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Remark"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Remark"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["TotalPrice"] = DBNull.Value;
                    }
                    else
                    {
                        dr["TotalPrice"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["OldID"] = DBNull.Value;
                    }
                    else
                    {
                        dr["OldID"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CityID"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CityID"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Valid"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Valid"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FXTCompanyId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FXTCompanyId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["TotalFloor"] = DBNull.Value;
                    }
                    else
                    {
                        dr["TotalFloor"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["RemainYear"] = DBNull.Value;
                    }
                    else
                    {
                        dr["RemainYear"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Depreciation"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Depreciation"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FitmentCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FitmentCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SurveyId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SurveyId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SaveDateTime"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SaveDateTime"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SaveUser"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SaveUser"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SourceName"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SourceName"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SourceLink"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SourceLink"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SourcePhone"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SourcePhone"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["AreaId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["AreaId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["AreaName"] = DBNull.Value;
                    }
                    else
                    {
                        dr["AreaName"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingDate"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["ZhuangXiu"] = DBNull.Value;
                    }
                    else
                    {
                        dr["ZhuangXiu"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SubHouse"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SubHouse"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["PeiTao"] = DBNull.Value;
                    }
                    else
                    {
                        dr["PeiTao"] = casedetail[detalnum];
                    }
                    detalnum++;
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetProjectCaseDataTable出错,ex=" + ex.ToString());
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量写入案例2数据
        /// </summary>
        /// <param name="houseHisFinallist"></param>
        /// <returns></returns>
        private static DataSet GetProjectCaseDataTable2(string values)
        {
            try
            {
                #region 虚拟表
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();                
                dt.Columns.Add(new DataColumn("ProjectId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("BuildingId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("HouseId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CompanyId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CaseDate", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("PurposeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("FloorNumber", typeof(Int64)));
                dt.Columns.Add(new DataColumn("HouseNo", typeof(string)));
                dt.Columns.Add(new DataColumn("BuildingArea", typeof(string)));
                dt.Columns.Add(new DataColumn("UsableArea", typeof(string)));
                dt.Columns.Add(new DataColumn("FrontCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
                dt.Columns.Add(new DataColumn("MoneyUnitCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("SightCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CaseTypeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("StructureCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("BuildingTypeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("HouseTypeCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CreateDate", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("Creator", typeof(string)));
                dt.Columns.Add(new DataColumn("Remark", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
                dt.Columns.Add(new DataColumn("OldID", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CityID", typeof(Int64)));
                dt.Columns.Add(new DataColumn("Valid", typeof(Int64)));
                dt.Columns.Add(new DataColumn("CaseID", typeof(Int64)));
                dt.Columns.Add(new DataColumn("FXTCompanyId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("BuildingName", typeof(string)));
                dt.Columns.Add(new DataColumn("TotalFloor", typeof(Int64)));
                dt.Columns.Add(new DataColumn("RemainYear", typeof(Int64)));
                dt.Columns.Add(new DataColumn("Depreciation", typeof(string)));
                dt.Columns.Add(new DataColumn("FitmentCode", typeof(Int64)));
                dt.Columns.Add(new DataColumn("SurveyId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("SaveDateTime", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("SaveUser", typeof(string)));
                dt.Columns.Add(new DataColumn("SourceName", typeof(string)));
                dt.Columns.Add(new DataColumn("SourceLink", typeof(string)));
                dt.Columns.Add(new DataColumn("SourcePhone", typeof(string)));
                dt.Columns.Add(new DataColumn("AreaId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("AreaName", typeof(string)));
                dt.Columns.Add(new DataColumn("BuildingDate", typeof(string)));
                dt.Columns.Add(new DataColumn("ZhuangXiu", typeof(string)));
                dt.Columns.Add(new DataColumn("SubHouse", typeof(string)));
                dt.Columns.Add(new DataColumn("PeiTao", typeof(string)));
                #endregion

                #region 虚拟表赋值
                string[] caselist = Regex.Split(values, "②", RegexOptions.IgnoreCase);
                foreach (string value in caselist)
                {
                    string[] casedetail = Regex.Split(value, "①", RegexOptions.IgnoreCase);
                    int detalnum = 0;
                    DataRow dr = dt.NewRow();
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CaseID"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CaseID"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["ProjectId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["ProjectId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["HouseId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["HouseId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CompanyId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CompanyId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CaseDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CaseDate"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["PurposeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["PurposeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FloorNumber"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FloorNumber"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingName"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingName"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["HouseNo"] = DBNull.Value;
                    }
                    else
                    {
                        dr["HouseNo"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingArea"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingArea"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["UsableArea"] = DBNull.Value;
                    }
                    else
                    {
                        dr["UsableArea"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FrontCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FrontCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["UnitPrice"] = DBNull.Value;
                    }
                    else
                    {
                        dr["UnitPrice"] = casedetail[detalnum];
                    }
                    dr["UnitPrice"] = casedetail[detalnum];
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["MoneyUnitCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["MoneyUnitCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SightCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SightCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CaseTypeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CaseTypeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["StructureCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["StructureCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingTypeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingTypeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["HouseTypeCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["HouseTypeCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CreateDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CreateDate"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Creator"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Creator"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Remark"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Remark"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["TotalPrice"] = DBNull.Value;
                    }
                    else
                    {
                        dr["TotalPrice"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["OldID"] = DBNull.Value;
                    }
                    else
                    {
                        dr["OldID"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["CityID"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CityID"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Valid"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Valid"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FXTCompanyId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FXTCompanyId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["TotalFloor"] = DBNull.Value;
                    }
                    else
                    {
                        dr["TotalFloor"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["RemainYear"] = DBNull.Value;
                    }
                    else
                    {
                        dr["RemainYear"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["Depreciation"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Depreciation"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["FitmentCode"] = DBNull.Value;
                    }
                    else
                    {
                        dr["FitmentCode"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SurveyId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SurveyId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SaveDateTime"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SaveDateTime"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SaveUser"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SaveUser"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SourceName"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SourceName"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SourceLink"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SourceLink"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SourcePhone"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SourcePhone"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["AreaId"] = DBNull.Value;
                    }
                    else
                    {
                        dr["AreaId"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["AreaName"] = DBNull.Value;
                    }
                    else
                    {
                        dr["AreaName"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["BuildingDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["BuildingDate"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["ZhuangXiu"] = DBNull.Value;
                    }
                    else
                    {
                        dr["ZhuangXiu"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["SubHouse"] = DBNull.Value;
                    }
                    else
                    {
                        dr["SubHouse"] = casedetail[detalnum];
                    }
                    detalnum++;
                    if (casedetail[detalnum] == "null")
                    {
                        dr["PeiTao"] = DBNull.Value;
                    }
                    else
                    {
                        dr["PeiTao"] = casedetail[detalnum];
                    }
                    detalnum++;
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
                return ds;
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetProjectCaseDataTable2出错,ex=" + ex.ToString());
                throw new Exception(ex.Message);
            }
        }
    }
}
