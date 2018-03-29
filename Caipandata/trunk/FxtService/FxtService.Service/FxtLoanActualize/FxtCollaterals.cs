using CAS.Common.MVC4;
using CAS.DataAccess.BaseDAModels;
using FxtCommonLibrary.LibraryUtils;
using FxtNHibernate.DATProjectDomain.Entities;
using FxtNHibernate.DTODomain.FxtLoanDTO;
using FxtNHibernate.FxtLoanDomain.Entities;
using FxtNHibernater.Data;
using FxtService.Common;
using FxtService.Contract.FxtLoanInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text;


/**
 * 作者:李晓东
 * 摘要:2014.01.22 新建
 *      2014.03.10 修改人:李晓东
 *                 新增:GetAllDataCollateralByFileId 某个押品文件归属押品信息
 *      2014.03.19 修改人:李晓东
 *                 新增:GetCollateralCountByPCA,GetCollateralClassification
 *      2014.03.20 修改人:李晓东
 *                 修改:GetCollateralCountByPCA
 *      2014.03.24 修改人:李晓东
 *                 修改:GetDetials 中的条件
 *      2014.03.25 修改人:李晓东
 *                 新增:GetExportByPCA 
 *      2014.03.26 修改人:贺黎亮
 *                 修改:GetCollateralCountByPCA 
 *                  1.type参数类型为string
 *                  2.添加 押品面积,原估价值,现估价值,担保金额,原抵押率,现抵押率返回字段
 *      2014.03.31 修改人:贺黎亮
 *                 1.GetCollateralClassification添加押品面积,原估价值,现估价值,担保金额返回字段
 *      2014.04.02 修改人:李晓东
 *                 修改:DATAHouseAdd 对单元(室号)的处理
 *      2014.05.26 修改人:李晓东
 *                 修改:押品复估中的所有执行语句且加上with(nolock)提升效率
 *      2014.05.27 修改人:李晓东
 *                 修改:压力测试、押品检测中的所有执行语句且加上with(nolock)提升效率
  *      2014.06.11 修改人:贺黎亮
 *                  添加文件
 *                  (),AddEditProjects()方法
 *       2014.06.12 修改人:贺黎亮
 *                  添加文件GetUploads(),GetFiles()方法
 *                 
 * **/
namespace FxtService.Service.FxtLoanActualize
{
    public class FxtCollaterals : IFxtCollaterals
    {

        #region DATAProject DATABuilding DATAHouse操作
        /// <summary>
        /// 贷后 临时楼盘新增
        /// </summary>
        /// <param name="dataProject">楼盘JSON对象</param>
        /// <returns></returns>
        public string DATAProjectAdd(string dataProject)
        {
            DataProject project = Utils.Deserialize<DataProject>(dataProject);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataProject>(project);

            if (objId > 0)
            {
                project.ProjectId = objId;
                return Utility.GetJson(1, "", project);
            }
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 贷后 临时楼栋新增
        /// </summary>
        /// <param name="dataBuilding">楼栋JSON对象</param>
        /// <returns></returns>
        public string DATABuildingAdd(string dataBuilding)
        {
            DataBuilding building = Utils.Deserialize<DataBuilding>(dataBuilding);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataBuilding>(building);
            if (objId > 0)
            {
                building.BuildingId = objId;
                return Utility.GetJson(1, "", building);
            }
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 贷后 临时房号新增
        /// </summary>
        /// <param name="dataHouse">房号JSON对象</param>
        /// <returns></returns>
        public string DATAHouseAdd(string dataHouse)
        {
            DataHouse house = Utils.Deserialize<DataHouse>(dataHouse);

            int floorLen = house.FloorNo.ToString().Length;
            house.UnitNo = System.Text.RegularExpressions.Regex.Replace(house.HouseName.Remove(0, floorLen), "[\u4e00-\u9fa5]", "");

            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataHouse>(house);
            if (objId > 0)
            {
                house.HouseId = objId;
                return Utility.GetJson(1, "", house);
            }
            return Utility.GetJson(0, "");
        }
        #endregion

        #region DataCollateral 押品标准化 操作

        /// <summary>
        /// 押品拆分保存
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="uploadFileId">文件ID</param>
        /// <returns></returns>
        public string DataCollateralAdd(string data)
        {
            DataCollateral collateral = Utils.Deserialize<DataCollateral>(data);
            if (collateral.MatchStatus != null)
                collateral.Status = 1;
            collateral.CreateDate = DateTime.Now;

            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataCollateral>(collateral);
            if (objId > 0)
            {
                //保存文件与押品的关系
                //RelationFileCollateral fileCollateral = new RelationFileCollateral()
                //{
                //    CollateralId = objId,
                //    UploadFileId = uploadFileId
                //};
                //CAS.DataAccess.DA.BaseDA.InsertFromEntity<RelationFileCollateral>(fileCollateral);
                //mssqldb.Create(fileCollateral);
                return Utility.GetJson(1, "", collateral);
            }
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 押品拆分修改
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <returns></returns>
        public string DataCollateralUpdate(string dataCollateral)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            DataCollateral collateral = Utils.Deserialize<DataCollateral>(dataCollateral);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(string.Format("update {0} set ", Utility.loan_Data_Collateral));
            Type _type = null; int i = 0;
            System.Reflection.PropertyInfo[] peroInfo = collateral.GetType().GetProperties();
            while (i < peroInfo.Length)
            {
                var proper = peroInfo[i];
                if (Array.IndexOf(Utility.filter, proper.Name) < 0 && !proper.Name.ToLower().Equals("id"))
                {
                    object obj = collateral.GetPropertyValue(proper.Name);
                    if (obj != null
                        && !obj.ToString().Equals("")
                        && !obj.ToString().Equals("0")
                        && !obj.ToString().Equals("0.00")
                        && !obj.ToString().Equals("0.0"))
                    {
                        if (!proper.PropertyType.IsGenericType)
                        {
                            _type = proper.PropertyType;
                        }
                        else
                        {
                            //泛型Nullable<>
                            Type genericTypeDefinition = proper.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                _type = genericTypeDefinition;
                            }
                        }
                        if (_type == typeof(int) || _type == typeof(Int32) || _type == typeof(decimal))
                        {
                            sbSql.AppendFormat("{0}={1},", proper.Name, obj);
                        }
                        else
                        {
                            sbSql.AppendFormat("{0}='{1}',", proper.Name, obj);
                        }
                    }
                }
                i++;
            }
            sbSql = new StringBuilder(sbSql.ToString().TrimEnd(','));
            if (collateral.MatchStatus != null)
            {
                sbSql.AppendFormat(",Status=1", collateral.MatchStatus);
            }
            sbSql.AppendFormat(" where Id={0}", collateral.Id);
            int objId = _mssql.CUD(sbSql.ToString());

            if (objId > 0)
            {
                return Utility.GetJson(1, "获取成功", collateral);
            }
            return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 获得已标准化列表信息
        /// </summary>
        /// <param name="pageSize">一页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="orderProperty">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="orderType">文件Id</param>
        /// <returns></returns>
        public string GetDataCollateral(int pageSize, int pageIndex, string orderProperty, string orderType, string cityarrid,
            string itemarrid, int uploadfileid, int customerid, int customertype)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid) && uploadfileid.Equals(0))
            { return Utility.GetJson(0, ""); }
            MSSQLADODAL mssqladodb = new MSSQLADODAL(Utility.DBFxtLoan);
            UtilityPager page = new UtilityPager();
            page.PageSize = pageSize;
            page.PageIndex = pageIndex;
            StringBuilder sbsqlWhere = new StringBuilder();
            if (uploadfileid.Equals(0))
            {
                sbsqlWhere.Append(" [status]=1 ");
                if (!Utils.IsNullOrEmpty(cityarrid))
                {
                    sbsqlWhere.Append(string.Format("  and CityId in ({0}) ", cityarrid));
                }
                if (!itemarrid.ToLower().Equals("null") && !itemarrid.Equals(""))
                {
                    sbsqlWhere.Append(string.Format("  and BankProjectId in ({0}) ", itemarrid));
                }
            }
            if (uploadfileid > 0)//未完成,且指定文件所属押品
            {
                sbsqlWhere.Append(string.Format(" Status=0 and UploadFileId ={0} ", uploadfileid));
            }

            string sql = string.Format("{0} {1}", Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral)
                , sbsqlWhere.ToString());

            if (Utils.IsNullOrEmpty(orderType))
                orderType = "Asc";
            else
                orderType = "Desc";
            if (!Utils.IsNullOrEmpty(orderProperty))
            {
                if (orderProperty.Equals("ProvinceId"))
                    sql = string.Format("{0} order by ProvinceId {1}", sql, orderType);
                else if (orderProperty.Equals("CityId"))
                    sql = string.Format("{0} order by CityId {1}", sql, orderType);
                else if (orderProperty.Equals("AreaId"))
                    sql = string.Format("{0} order by AreaId {1}", sql, orderType);
                else if (orderProperty.Equals("ProjectName"))
                    sql = string.Format("{0} order by ProjectName {1}", sql, orderType);
                else if (orderProperty.Equals("BuildingNumber"))
                    sql = string.Format("{0} order by BuildingNumber {1}", sql, orderType);
            }
            else
                sql = string.Format("{0} order by ProvinceId {1}", sql, orderType);

            IList<DataCollateral> list = mssqladodb.GetList<DataCollateral>(sql,
                page, string.Format("{0} where {1}", Utility.loan_Data_Collateral, sbsqlWhere.ToString()));
            List<DataCollaterals> listcmd = new List<DataCollaterals>();
            mssqladodb = new MSSQLADODAL();
            int i = 0;
            while (i < list.Count)
            {
                var item = list[i];
                DataCollaterals cmd = new DataCollaterals(item);
                //省份
                var province = UtilityDALHelper.GetADOProvinceById(mssqladodb, item.ProvinceId);
                cmd.ProvinceName = province != null ? province.ProvinceName : "";
                //城市
                var city = UtilityDALHelper.GetADOCityById(mssqladodb, item.CityId);
                cmd.CityName = city != null ? city.CityName : "";
                //行政区
                var area = UtilityDALHelper.GetADOSYSAreaById(mssqladodb, item.AreaId);
                cmd.AreaName = area != null ? area.AreaName : "";

                listcmd.Add(cmd);
                i++;
            }
            if (listcmd != null)
            {
                return Utility.GetJson(1, "成功", listcmd, page.Count);
            }

            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 根据条件,获得已有押品信息
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <returns></returns>
        public string GetDataCollateralByMoreWhere(string dataCollateral)
        {
            DataCollateral collateral = Utils.Deserialize<DataCollateral>(dataCollateral);
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);

            string sql =
                string.Format("{0} Number='{1}' and Branch='{2}' and PurposeCode={3} and Name='{4}' and BuildingArea={5} and Address='{6}'",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                collateral.Number, collateral.Branch, collateral.PurposeCode, collateral.Name,
                collateral.BuildingArea, collateral.Address);

            DataCollateral getmodel = mssqlado.GetModel<DataCollateral>(sql);
            if (getmodel != null)
                return Utility.GetJson(1, "", getmodel);
            return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 获得已有押品,根据押品编号,且状态为2(未匹配)的
        /// </summary>
        /// <param name="collNumber">押品编号</param>
        /// <returns></returns>
        public string GetDataCollateralByNumber(string collNumber)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);

            string sql =
                string.Format("{0} Number='{1}' and [Status]=2",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                collNumber);
            DataCollateral getmodel = mssqlado.GetModel<DataCollateral>(sql);
            if (getmodel != null)
                return Utility.GetJson(1, "", getmodel);
            return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 获得已有押品全部信息
        /// </summary>
        /// <returns></returns>
        public string GetAllDataCollateral(string cityarrid, string itemarrid)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid))
            {
                return Utility.GetJson(0, "获取失败");
            }
            StringBuilder sbsqlWhere = new StringBuilder();
            sbsqlWhere.Append(" [status]=1 ");

            if (!Utils.IsNullOrEmpty(cityarrid))
            {
                sbsqlWhere.Append(string.Format("  and CityId in ({0}) ", cityarrid));
            }
            if (!itemarrid.ToLower().Equals("null") && !itemarrid.Equals(""))
            {
                sbsqlWhere.Append(string.Format("  and BankProjectId in ({0}) ", itemarrid));
            }

            string sql = string.Format("{0} {1}", Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral)
                , sbsqlWhere.ToString());

            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);

            List<DataCollateral> list = mssqlado.GetList<DataCollateral>(sql); ;
            List<CollateralMonitorDetails> listCMD = new List<CollateralMonitorDetails>();
            mssqlado = new MSSQLADODAL();
            int i = 0;
            while (i < list.Count)
            {
                CollateralMonitorDetails cmd = new CollateralMonitorDetails(list[i]);
                SysBankProject sysBankProject = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysBankProject>(cmd.BankProjectId);
                cmd.BankProjectName = sysBankProject != null ? sysBankProject.ProjectName : "";
                var Province = UtilityDALHelper.GetADOProvinceById(mssqlado, cmd.ProvinceId);
                cmd.ProvinceName = Province != null ? Province.ProvinceName : "";
                var City = UtilityDALHelper.GetADOCityById(mssqlado, cmd.CityId);
                cmd.CityName = City != null ? City.CityName : "";
                var Area = UtilityDALHelper.GetADOSYSAreaById(mssqlado, cmd.AreaId);
                cmd.AreaName = Area != null ? Area.AreaName : "";
                i++;
                listCMD.Add(cmd);
            }
            if (list != null)
                return Utility.GetJson(1, "", listCMD);
            return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 获得已有押品全部信息根据某个文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="pageSize">一页记录</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public string GetAllDataCollateralByFileId(int fileId, int pageSize, int pageIndex)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            IList<RelationFileCollateral> listRFC = null;
            string sql = string.Format("{0} UploadFileId={1}",
                Utility.GetMSSQL_SQL(typeof(RelationFileCollateral), Utility.loand_Relation_File_Collateral), fileId),
                where = string.Format("{0} where UploadFileId={1}",
                Utility.loand_Relation_File_Collateral, fileId);

            if (!pageSize.Equals(0) && !pageIndex.Equals(0))
            {
                UtilityPager page = new UtilityPager();
                page.PageSize = pageSize;
                page.PageIndex = pageIndex;
                listRFC = mssqlado.GetList<RelationFileCollateral>(sql, page, where);
            }
            else
            {
                listRFC = mssqlado.GetList<RelationFileCollateral>(sql);
            }
            var item = listRFC.Select(x => x.CollateralId).ToArray();
            int i = 1;
            StringBuilder sb = new StringBuilder();
            while (i <= item.Length)
            {
                if (i == item.Length)
                    sb.AppendFormat("{0}", item[i]);
                else
                    sb.AppendFormat("{0},", item[i]);
                i++;
            }
            sql = string.Format("{0} Id in ({1})",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                sb.ToString());
            IList<DataCollateral> list = mssqlado.GetList<DataCollateral>(sql);

            if (list != null)
                return Utility.GetJson(1, "", list);
            return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 获得属于某个文件的所有押品总量
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <returns></returns>
        public string GetCountCollateralByFileId(int fileId)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            StringBuilder sbsql = new StringBuilder();
            sbsql.AppendFormat("select count(*) as Id from {0} with(nolock) where Status=1 and  UploadFileId={1} ", Utility.loan_Data_Collateral, fileId);
            List<DataCollateral> datalist = mssqlado.GetList<DataCollateral>(sbsql.ToString());
            long cntcount = 0;
            if (datalist != null && datalist.Count > 0)
            {
                cntcount = datalist.FirstOrDefault().Id;
            }
            return Utility.GetJson(1, "", cntcount);
        }

        /// <summary>
        /// 获得指定列值
        /// </summary>
        /// <param name="cId">城市ID</param>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="columnName">指定列</param>
        /// <returns></returns>
        public string GetCustomColumnsValue(int cId, int projectId, string columnName)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
            string flag = Utility.GetJson(0, "");
            if (cityTable != null)
            {
                string sql = string.Format("{0} ProjectId={1}",
                    Utility.GetMSSQL_SQL(typeof(DATProject), cityTable.ProjectTable), projectId);

                DATProject datProject = _mssql.GetModel<DATProject>(sql);
                if (datProject == null)
                { //尝试临时库
                    MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
                    sql = string.Format("{0} ProjectId={1}",
                    Utility.GetMSSQL_SQL(typeof(DataProject), Utility.loan_DataProject), projectId);
                    var dataProject = mssqlado.GetModel<DataProject>(sql);
                    if (dataProject != null)
                        flag = Utility.GetJson(1, "", dataProject.GetType().GetProperty(columnName).GetValue(dataProject, null));
                }
                else
                {
                    if (datProject != null)
                        flag = Utility.GetJson(1, "", datProject.GetType().GetProperty(columnName).GetValue(datProject, null));
                }
            }
            return flag;
        }

        /// <summary>
        /// 修改指定列信息
        /// </summary>
        /// <param name="data">对象</param>
        /// <param name="cid">城市</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public string UpdateCustomColumnsValue(string data, int cid, int type)
        {
            JArray _array = JArray.Parse(data);
            StringBuilder sbSql = new StringBuilder();
            int i = 0; bool flag = false;
            string strVal = string.Empty,
                   strColumn = string.Empty,
                   strProjectId = string.Empty;
            while (i < _array.Count)
            {
                JObject _jobj = JObject.Parse(_array[i].ToString());

                strColumn = _jobj["column"].ToString();
                if (!strColumn.ToLower().Equals("projectid"))
                {
                    strVal = string.Format("'{0}'", _jobj["value"].ToString());

                    if (_jobj["type"].ToString() == "number")
                    {
                        strVal = string.Format("{0}", _jobj["value"].ToString());
                    }

                    if (i == _array.Count - 1)
                    {
                        sbSql.AppendFormat(" {0}={1}", strColumn, strVal);
                    }
                    else
                    {
                        sbSql.AppendFormat(" {0}={1},", strColumn, strVal);
                    }
                }
                else
                {
                    strProjectId = _jobj["value"].ToString();
                }
                i++;
            }
            sbSql = new StringBuilder(sbSql.ToString().TrimEnd(','));
            //正式库
            if (type.Equals(0))
            {
                MSSQLADODAL mssql = new MSSQLADODAL();
                SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(mssql, cid);
                if (!Utils.IsNullOrEmpty(sbSql.ToString()))
                    flag = mssql.CUD(string.Format("update {0} set {1} where ProjectId={2}", cityTable.ProjectTable, sbSql.ToString(), strProjectId)) > 0;

                return Utility.GetJson(flag ? 1 : -1, "");
            }//临时库            
            else if (type.Equals(1))
            {
                MSSQLADODAL mssql = new MSSQLADODAL(Utility.DBFxtLoan);
                if (!Utils.IsNullOrEmpty(sbSql.ToString()))
                    flag = mssql.CUD(string.Format("update {0} set {1} where ProjectId={2}", Utility.loan_DataProject, sbSql.ToString(), strProjectId)) > 0;

                return Utility.GetJson(flag ? 1 : -1, "");
            }
            return Utility.GetJson(0, "");
        }
        #endregion

        #region 押品检测

        /// <summary>
        /// 根据省份或者城市或者行政区得到楼盘总量信息(地图统计)
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="type">统计类型</param>
        /// <param name="cityarrid">选择城市</param>
        /// <param name="itemarrid">选择城市</param>
        /// <returns></returns>
        public string GetCollateralCountByPCA(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age, string type, string itemarrid)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            List<JObject> listObject = new List<JObject>();
            //IList<DATProject> listProject = null;
            IList<DataCollateral> listColl = null;
            IList<SYSArea> listArea = UtilityDALHelper.GetADOSYSArea(_mssql, cId);
            StringBuilder sbLoanSql = new StringBuilder();
            StringBuilder sbOneSql = new StringBuilder();
            StringBuilder sbTwoSql = new StringBuilder();
            StringBuilder sbThreeSql = new StringBuilder();
            string strTempSql = string.Empty;

            #region 条件
            //物业类型
            if (!Utils.IsNullOrEmpty(houseType))
            {
                sbLoanSql.AppendFormat("PurposeCode in ({0})", houseType);
            }
            //贷款额度
            if (!Utils.IsNullOrEmpty(loanAmount))
            {
                string[] loanAmountArray = loanAmount.Split(',');
                strTempSql = Utility.GetArrayWhere(loanAmountArray, "LoanAmount");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //押品面积
            if (!Utils.IsNullOrEmpty(buildingArea))
            {
                string[] buildingAreaArray = buildingArea.Split(',');
                strTempSql = Utility.GetArrayWhere(buildingAreaArray, "BuildingArea");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //年龄
            if (!Utils.IsNullOrEmpty(age))
            {
                string[] ageArray = age.Split(',');
                strTempSql = Utility.GetArrayWhere(ageArray, "LoanAge");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            if (!Utils.IsNullOrEmpty(buildingType) || !Utils.IsNullOrEmpty(buildingDate))
            {
                //BuildingDate
                StringBuilder sbOneChSql = new StringBuilder();
                //BuildDate
                StringBuilder sbTwoChSql = new StringBuilder();
                SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
                string strOneTempSql = string.Empty;
                //建筑类型
                if (!Utils.IsNullOrEmpty(buildingType))
                {
                    sbOneChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                    sbTwoChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                }
                //年代
                if (!Utils.IsNullOrEmpty(buildingDate))
                {
                    string[] buildingDateArray = buildingDate.Split(',');
                    strTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildingDate,120))");
                    strOneTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildDate,120))");
                    if (!Utils.IsNullOrEmpty(sbOneChSql.ToString()))
                    {
                        sbOneChSql.AppendFormat(" and ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" and ({0})", strOneTempSql);
                    }
                    else
                    {
                        sbOneChSql.AppendFormat(" ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" ({0})", strOneTempSql);
                    }
                }
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    sbOneSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ", cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.CaseTable, sbOneChSql.ToString());
                }
                else
                {
                    sbOneSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ", cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.CaseTable, sbOneChSql.ToString());
                }
            }
            #endregion

            MSSQLADODAL _mssqladoLoan = new MSSQLADODAL(Utility.DBFxtLoan);

            #region 执行结果
            foreach (var item in listArea)
            {
                string strSql = string.Empty, strSqlExcute = string.Empty;
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    if (!Utils.IsNullOrEmpty(itemarrid))
                    {
                        strSql = string.Format("{0} {1} and Status=1 and AreaId={2} and BankProjectId in ({3})",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                        sbLoanSql.ToString(), item.AreaId, itemarrid);
                    }
                    else
                    {
                        strSql = string.Format("{0} {1} and Status=1 and AreaId={2}",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                        sbLoanSql.ToString(), item.AreaId);
                    }
                }
                else
                {
                    if (!Utils.IsNullOrEmpty(itemarrid))
                    {
                        strSql = string.Format("{0} Status=1 and AreaId={1} and BankProjectId in ({2})",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                        item.AreaId, itemarrid);
                    }
                    else
                    {
                        strSql = string.Format("{0} Status=1 and AreaId={1} ",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                        item.AreaId);
                    }
                }
                if (!Utils.IsNullOrEmpty(sbOneSql.ToString()) &&
                    !Utils.IsNullOrEmpty(sbTwoSql.ToString()) &&
                    !Utils.IsNullOrEmpty(sbThreeSql.ToString()))
                {
                    //默认楼栋
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1}", strSql, sbOneSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1}", strSql, sbOneSql.ToString());
                    }
                    listColl = _mssqladoLoan.GetList<DataCollateral>(strSqlExcute);

                    //如果楼栋是没有找到,就用楼盘
                    if (listColl.Count() == 0)
                    {
                        if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                        {
                            strSqlExcute = string.Format(" {0} {1}", strSql, sbTwoSql.ToString());
                        }
                        else
                        {
                            strSqlExcute = string.Format(" {0} and {1}", strSql, sbTwoSql.ToString());
                        }
                        listColl = _mssqladoLoan.GetList<DataCollateral>(strSqlExcute);
                    }
                    //如果楼盘是没有找到,就用案例
                    if (listColl.Count() == 0)
                    {
                        if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                        {
                            strSqlExcute = string.Format(" {0} {1}", strSql, sbThreeSql.ToString());
                        }
                        else
                        {
                            strSqlExcute = string.Format(" {0} and {1}", strSql, sbThreeSql.ToString());
                        }
                        listColl = _mssqladoLoan.GetList<DataCollateral>(strSqlExcute);
                    }
                }
                else
                {
                    listColl = _mssqladoLoan.GetList<DataCollateral>(strSql);
                }
                JObject _object = new JObject();
                _object.Add("Area", Utils.Serialize(item));
                int _ccount = listColl.Count();
                //押品总数
                _object.Add("CollNumberCount", _ccount);

                //贷款总额
                decimal loan = listColl.Sum(sitem => sitem.LoanAmount != null ? sitem.LoanAmount.Value : 0);
                _object.Add("CollTotal", decimal.Round(loan / 10000, 2));
                //贷款余额
                decimal loanBalance = listColl.Sum(sitem => sitem.LoanBalance != null ? sitem.LoanBalance.Value : 0);
                _object.Add("CollOver", decimal.Round(loanBalance / 10000, 2));

                //押品面积
                decimal CollateralArea = listColl.Sum(sitem => sitem.BuildingArea);
                _object.Add("CollateralArea", CollateralArea);
                //原估价值
                decimal OriginalValue = listColl.Sum(sitem => sitem.OldRate);
                _object.Add("OriginalValue", decimal.Round(OriginalValue / 10000, 2));

                //现估价值
                decimal AssessedValue = GetReassessmentValue(listColl, "sum(Price)");
                _object.Add("AssessedValue", decimal.Round(AssessedValue / 10000, 2));
                //担保金额
                decimal AmountValue = listColl.Sum(sitem => sitem.GuaranteePrice);
                _object.Add("AmountValue", AmountValue);
                //原抵押率
                decimal OriginalRate = !_ccount.Equals(0) ? listColl.Sum(sitem => sitem.OldMortgageRates) / _ccount : 0;
                _object.Add("OriginalRate", OriginalRate);
                //现抵押率
                decimal MortgageRate = !_ccount.Equals(0) ? GetReassessmentValue(listColl, "sum(ArrivedLoanRates)") / _ccount : 0;
                _object.Add("MortgageRate", MortgageRate);
                int[] typeArr = type.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                if (typeArr.Where(o => o.Equals(0)).Any())//押品总数
                {
                    _object.Add("Count", listColl.Count());
                }
                else if (typeArr.Where(o => o.Equals(1)).Any())//贷款总额
                {
                    _object.Add("Count", loan);
                }
                else if (typeArr.Where(o => o.Equals(2)).Any())//贷款余额
                {
                    _object.Add("Count", loanBalance);
                }
                else if (typeArr.Where(o => o.Equals(3)).Any())//押品面积
                {
                    _object.Add("Count", CollateralArea);
                }
                else if (typeArr.Where(o => o.Equals(4)).Any())//原估价值
                {
                    _object.Add("Count", OriginalValue);
                }
                else if (typeArr.Where(o => o.Equals(5)).Any())//现估价值
                {
                    _object.Add("Count", AssessedValue);
                }
                else if (typeArr.Where(o => o.Equals(6)).Any())//担保金额
                {
                    _object.Add("Count", AmountValue);
                }
                else if (typeArr.Where(o => o.Equals(7)).Any())//原抵押率
                {
                    _object.Add("Count", OriginalRate);
                }
                else if (typeArr.Where(o => o.Equals(8)).Any())//现抵押率
                {
                    _object.Add("Count", MortgageRate);
                }
                listObject.Add(_object);
            }
            #endregion

            if (listObject.Count > 0)
                return Utility.GetJson(1, "获取成功", listObject);
            else
                return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 获取复估表中指定的值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private decimal GetReassessmentValue(IList<DataCollateral> list, string where)
        {
            if (list.Count > 0)
            {
                string _sql = string.Format("SELECT {0} as Value FROM {1} where CollateralId in ({2}) and Months='{3}'",
                        where, Utility.loan_Data_Data_Reassessment,
                        Utility.GetArrayString(list.Select(sitem => sitem.Id).ToArray()),
                        Utils.GetDateTime("yyyyMM")), unResult = string.Empty;
                MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
                unResult = mssqlado.GetUniqueResult(_sql).ToString();
                unResult = Utils.IsNullOrEmpty(unResult) ? "0" : unResult;
                return !list.Count.Equals(0) ? decimal.Parse(unResult) : 0;
            }
            else
                return 0;
        }

        /// <summary>
        /// 押品监测地图导出
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <returns></returns>
        public string GetExportByPCA(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            IList<DataCollateral> listColl = null;
            List<JObject> listObject = new List<JObject>();
            //城市
            if (!pId.Equals(0) && !cId.Equals(0) && aId.Equals(0))
            {
                IList<SYSArea> listArea = UtilityDALHelper.GetADOSYSArea(_mssql, cId);
                StringBuilder sbLoanSql = new StringBuilder();
                StringBuilder sbOneSql = new StringBuilder();
                StringBuilder sbTwoSql = new StringBuilder();
                StringBuilder sbThreeSql = new StringBuilder();
                string strTempSql = string.Empty;
                #region 条件
                //物业类型
                if (!Utils.IsNullOrEmpty(houseType))
                {
                    sbLoanSql.AppendFormat("PurposeCode in ({0})", houseType);
                }
                //贷款额度
                if (!Utils.IsNullOrEmpty(loanAmount))
                {
                    string[] loanAmountArray = loanAmount.Split(',');
                    strTempSql = Utility.GetArrayWhere(loanAmountArray, "LoanAmount");
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                        sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                    else
                        sbLoanSql.AppendFormat(" ({0})", strTempSql);
                }
                //押品面积
                if (!Utils.IsNullOrEmpty(buildingArea))
                {
                    string[] buildingAreaArray = buildingArea.Split(',');
                    strTempSql = Utility.GetArrayWhere(buildingAreaArray, "BuildingArea");
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                        sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                    else
                        sbLoanSql.AppendFormat(" ({0})", strTempSql);
                }
                //年龄
                if (!Utils.IsNullOrEmpty(age))
                {
                    string[] ageArray = age.Split(',');
                    strTempSql = Utility.GetArrayWhere(ageArray, "LoanAge");
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                        sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                    else
                        sbLoanSql.AppendFormat(" ({0})", strTempSql);
                }
                if (!Utils.IsNullOrEmpty(buildingType) || !Utils.IsNullOrEmpty(buildingDate))
                {
                    //BuildingDate
                    StringBuilder sbOneChSql = new StringBuilder();
                    //BuildDate
                    StringBuilder sbTwoChSql = new StringBuilder();
                    SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
                    string strOneTempSql = string.Empty;
                    //建筑类型
                    if (!Utils.IsNullOrEmpty(buildingType))
                    {
                        sbOneChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                        sbTwoChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                    }
                    //年代
                    if (!Utils.IsNullOrEmpty(buildingDate))
                    {
                        string[] buildingDateArray = buildingDate.Split(',');
                        strTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildingDate,120))");
                        strOneTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildDate,120))");
                        if (!Utils.IsNullOrEmpty(sbOneChSql.ToString()))
                        {
                            sbOneChSql.AppendFormat(" and ({0})", strTempSql);
                            sbTwoChSql.AppendFormat(" and ({0})", strOneTempSql);
                        }
                        else
                        {
                            sbOneChSql.AppendFormat(" ({0})", strTempSql);
                            sbTwoChSql.AppendFormat(" ({0})", strOneTempSql);
                        }
                    }
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        sbOneSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.BuildingTable, sbTwoChSql.ToString());
                        sbTwoSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.ProjectTable, sbOneChSql.ToString());
                        sbThreeSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.CaseTable, sbOneChSql.ToString());
                    }
                    else
                    {
                        sbOneSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.BuildingTable, sbTwoChSql.ToString());
                        sbTwoSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ", cityTable.ProjectTable, sbOneChSql.ToString());
                        sbThreeSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", cityTable.CaseTable, sbOneChSql.ToString());
                    }
                }
                #endregion
                MSSQLADODAL _mssqlLoan = new MSSQLADODAL(Utility.DBFxtLoan);
                foreach (var item in listArea)
                {
                    string strSql = string.Empty, strSqlExcute = string.Empty;
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSql = string.Format("{0} {1} and Status=1 and AreaId={2}",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                        sbLoanSql.ToString(),
                        item.AreaId);
                    }
                    else
                    {
                        strSql = string.Format("{0} Status=1 and AreaId={1}",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                        item.AreaId);
                    }
                    if (!Utils.IsNullOrEmpty(sbOneSql.ToString()) &&
                        !Utils.IsNullOrEmpty(sbTwoSql.ToString()) &&
                        !Utils.IsNullOrEmpty(sbThreeSql.ToString()))
                    {
                        //默认楼栋
                        if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                        {
                            strSqlExcute = string.Format(" {0} {1}", strSql, sbOneSql.ToString());
                        }
                        else
                        {
                            strSqlExcute = string.Format(" {0} and {1}", strSql, sbOneSql.ToString());
                        }
                        listColl = _mssqlLoan.GetList<DataCollateral>(strSqlExcute);

                        //如果楼栋是没有找到,就用楼盘
                        if (listColl.Count() == 0)
                        {
                            if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                            {
                                strSqlExcute = string.Format(" {0} {1}", strSql, sbTwoSql.ToString());
                            }
                            else
                            {
                                strSqlExcute = string.Format(" {0} and {1}", strSql, sbTwoSql.ToString());
                            }
                            listColl = _mssqlLoan.GetList<DataCollateral>(strSqlExcute);
                        }
                        //如果楼盘是没有找到,就用案例
                        if (listColl.Count() == 0)
                        {
                            if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                            {
                                strSqlExcute = string.Format(" {0} {1}", strSql, sbThreeSql.ToString());
                            }
                            else
                            {
                                strSqlExcute = string.Format(" {0} and {1}", strSql, sbThreeSql.ToString());
                            }
                            listColl = _mssqlLoan.GetList<DataCollateral>(strSqlExcute);
                        }
                    }
                    else
                    {
                        listColl = _mssqlLoan.GetList<DataCollateral>(strSql);
                    }
                    JObject _object = new JObject();
                    _object.Add("Area", Utils.Serialize(item));
                    //押品总数
                    _object.Add("Count", listColl.Count);
                    //押品面积
                    _object.Add("BuildingArea", listColl.Sum(sitem => sitem.BuildingArea));
                    //原估值求和
                    _object.Add("OldRatePrice", listColl.Sum(sitem => sitem.BuildingArea));
                    //最近一次复估值求和
                    _object.Add("RecentRatePrice", listColl.Sum(sitem => sitem.BuildingArea));
                    //担保金额求和
                    _object.Add("GuaranteePrice", listColl.Sum(sitem => sitem.BuildingArea));
                    //原抵押率
                    _object.Add("OldMortgageRates", listColl.Sum(sitem => sitem.BuildingArea));
                    //现抵押率
                    _object.Add("MortgageRates", listColl.Sum(sitem => sitem.BuildingArea));
                    listObject.Add(_object);
                }
            }
            if (listObject.Count > 0)
                return Utility.GetJson(1, "获取成功", listObject);
            else
                return Utility.GetJson(0, "获取失败");
        }
        /// <summary>
        /// 押品分类统计
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="requirement">条件</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="type">返回类型(0:押品监测,1:押品资产价值动态监测)</param>
        /// <returns></returns>
        public string GetCollateralClassification(int pId, int cId, int aId,
            string requirement, string start, string end, int type, string cityarrid, string itemarrid)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid))
            {
                return Utility.GetJson(0, "获取失败");
            }

            MSSQLADODAL _mssql = new MSSQLADODAL();
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbLoanSql = new StringBuilder();

            IList<DataCollateral> listColl = null;
            List<JObject> list = new List<JObject>();
            string strTempSql = string.Empty,
                   strSql = string.Empty,
                   rateWhere = string.Empty,
                   strWhere = string.Empty,
                   sRate = @"select (select top 1 price from FxtLoan.dbo.Data_Reassessment with(nolock) where CollateralID=c.Id order by Months desc) from FxtLoan.dbo.Data_Collateral as c  where ";//现复估查询
            var arrCity = new string[] { };
            #region 条件

            if (!Utils.IsNullOrEmpty(requirement))
            {
                string[] array = requirement.Split(new string[] { "&&" }, StringSplitOptions.None);

                strSql = string.Format("{0} Status=1 ",
                                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral));
                rateWhere = " Status=1 ";
                if (!Utils.IsNullOrEmpty(start) && !Utils.IsNullOrEmpty(end))
                {
                    strSql = string.Format("{0} Status=1  and LoanDate between {1} and {2}",
                                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral), start, end);
                    rateWhere = string.Format(" Status=1 and LoanDate between {0} and {1}",
                         start, end);
                }
                if (!Utils.IsNullOrEmpty(cityarrid.ToString()))
                {
                    arrCity = cityarrid.Split(',');
                    strSql += string.Format(" and CityId in ({0}) ", cityarrid);
                    rateWhere += string.Format(" and CityId in ({0}) ", cityarrid);
                }
                if (!Utils.IsNullOrEmpty(itemarrid.ToString()))
                {
                    strSql += string.Format(" and BankProjectId in ({0}) ", itemarrid);
                    rateWhere += string.Format(" and BankProjectId in ({0}) ", itemarrid);
                }


                MSSQLADODAL adomssql = new MSSQLADODAL(Utility.DBFxtLoan);
                //物业类型
                if (array[0].Equals("0"))
                {
                    var wuye = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));
                    foreach (var item in wuye)
                    {
                        strTempSql = string.Format("{0} and PurposeCode={1}", strSql, item.Code);

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);

                        strTempSql = string.Format("{0} {1} and PurposeCode={2}", sRate, rateWhere, item.Code);

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                    }
                }
                //建筑类型
                else if (array[0].Equals("1"))
                {
                    var jzClass = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));

                    foreach (var item in jzClass)
                    {
                        StringBuilder whereTemp = new StringBuilder();
                        int aci = 0;
                        while (aci < arrCity.Length)
                        {
                            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, Convert.ToInt32(arrCity[aci]));
                            strWhere = GetProjectIdWhere(cityTable.BuildingTable, string.Format(" BuildingTypeCode={0} ", item.Code));
                            if (aci != arrCity.Length - 1)
                            {
                                whereTemp.AppendFormat("{0} and ", strWhere);
                            }
                            else
                            {
                                whereTemp.AppendFormat(" {0} ", strWhere);
                            }
                            aci++;
                        }
                        strTempSql = string.Format("{0} and {1}", strSql, whereTemp.ToString());

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);

                        strTempSql = string.Format("{0} {1} and {2}", sRate, rateWhere, whereTemp.ToString());

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                    }
                }
                //建筑年代
                else if (array[0].Equals("2"))
                {
                    var jzYear = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));
                    SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
                    int i = 0, count = jzYear.Count;
                    foreach (var item in jzYear)
                    {
                        strWhere = GetJZYearWhere(_mssql, arrCity,
                            Utility.GetArrayWhere(GetDBConvert("BuildDate"), item.CodeName, i, count), 0);

                        strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);
                        if (listColl.Count() == 0)
                        {
                            strWhere = GetJZYearWhere(_mssql, arrCity,
                            Utility.GetArrayWhere(GetDBConvert("BuildingDate"), item.CodeName, i, count), 1);

                            strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                            listColl = adomssql.GetList<DataCollateral>(strTempSql);
                        }
                        if (listColl.Count() == 0)
                        {
                            strWhere = GetJZYearWhere(_mssql, arrCity,
                            Utility.GetArrayWhere(GetDBConvert("BuildingDate"), item.CodeName, i, count), 2);

                            strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                            listColl = adomssql.GetList<DataCollateral>(strTempSql);
                        }

                        strTempSql = string.Format("{0} {1} and {2}", sRate, rateWhere, strWhere);

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                        i++;
                    }
                }
                //贷款额度
                else if (array[0].Equals("3"))
                {
                    var loanAmount = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));
                    int i = 0, count = loanAmount.Count;
                    foreach (var item in loanAmount)
                    {
                        strWhere = Utility.GetArrayWhere("LoanAmount", item.CodeName, i, count);

                        strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);

                        strTempSql = string.Format("{0} {1} and {2}", sRate, rateWhere, strWhere);

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                        i++;
                    }
                }
                //押品面积
                else if (array[0].Equals("4"))
                {
                    var loanAmount = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));
                    int i = 0, count = loanAmount.Count;
                    foreach (var item in loanAmount)
                    {
                        strWhere = Utility.GetArrayWhere("BuildingArea", item.CodeName, i, count);

                        strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);

                        strTempSql = string.Format("{0} {1} and {2}", sRate, rateWhere, strWhere);

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                        i++;
                    }
                }
                //年龄
                else if (array[0].Equals("5"))
                {
                    var loanAmount = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));
                    int i = 0, count = loanAmount.Count;
                    foreach (var item in loanAmount)
                    {
                        strWhere = Utility.GetArrayWhere("LoanAge", item.CodeName, i, count);

                        strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);

                        strTempSql = string.Format("{0} {1} and {2}", sRate, rateWhere, strWhere);

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                        i++;
                    }
                }
                //放贷日期
                else if (array[0].Equals("6"))
                {
                    var loanAmount = UtilityDALHelper.GetADOListSYSCODE(_mssql, Convert.ToInt32(array[1]));
                    int i = 0, count = loanAmount.Count;
                    foreach (var item in loanAmount)
                    {
                        strWhere = Utility.GetArrayWhere("LoanDate", item.CodeName, i, count);

                        strTempSql = string.Format("{0} and {1}", strSql, strWhere);

                        listColl = adomssql.GetList<DataCollateral>(strTempSql);

                        strTempSql = string.Format("{0} {1} and {2}", sRate, rateWhere, strWhere);

                        list.Add(GetJobject(item.CodeName, listColl, type, adomssql, strTempSql));
                        i++;
                    }
                }
            }
            #endregion

            if (list.Count > 0)
                return Utility.GetJson(1, "获取成功", list);
            else
                return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 押品明细查询
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="projectid">楼盘</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="cityarrid">选择城市</param>
        /// <param name="itemarrid">选择项目</param>
        /// <returns></returns>
        public string GetDetials(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age,
            int projectid, int companyid, string start, string end,
            int pageIndex, int pageSize, string cityarrid, string itemarrid)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid))
            {
                return Utility.GetJson(0, "获取失败");
            }
            MSSQLADODAL _mssql = new MSSQLADODAL();
            StringBuilder sbSql = new StringBuilder();
            UtilityPager pager = new UtilityPager(pageSize, pageIndex);

            IList<DataCollateral> listColl = null;
            StringBuilder sbLoanSql = new StringBuilder();
            StringBuilder sbOneSql = new StringBuilder();
            StringBuilder sbTwoSql = new StringBuilder();
            StringBuilder sbThreeSql = new StringBuilder();

            string strTempSql = string.Empty;
            #region 条件
            //物业类型
            if (!Utils.IsNullOrEmpty(houseType))
            {
                sbLoanSql.AppendFormat("PurposeCode in ({0})", houseType);
            }
            //贷款时间
            if (!Utils.IsNullOrEmpty(start) && !Utils.IsNullOrEmpty(end))
            {
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and (LoanDate between {0} and {1}) ", start, end);
                else
                    sbLoanSql.AppendFormat(" (LoanDate between {0} and {1}) ", start, end);
            }
            //楼盘
            if (!projectid.Equals(0))
            {
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ProjectId={0} ", projectid);
                else
                    sbLoanSql.AppendFormat(" ProjectId={0} ", projectid);
            }
            //开发商
            if (!companyid.Equals(0))
            {
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    sbLoanSql.Append(" and ProjectId in (select distinct ProjectId from [FXTProject].[dbo].LNK_P_Company where ");
                    sbLoanSql.AppendFormat("  CompanyType=2001001 and cityid={0} and CompanyId={1}) ", cId, companyid);
                }
                else
                {
                    sbLoanSql.Append(" ProjectId in (select distinct ProjectId from [FXTProject].[dbo].LNK_P_Company where ");
                    sbLoanSql.AppendFormat("  CompanyType=2001001 and cityid={0} and CompanyId={0}) ", cId, companyid);
                }
            }
            //贷款额度
            if (!Utils.IsNullOrEmpty(loanAmount))
            {
                string[] loanAmountArray = loanAmount.Split(',');
                strTempSql = Utility.GetArrayWhere(loanAmountArray, "LoanAmount");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //押品面积
            if (!Utils.IsNullOrEmpty(buildingArea))
            {
                string[] buildingAreaArray = buildingArea.Split(',');
                strTempSql = Utility.GetArrayWhere(buildingAreaArray, "BuildingArea");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //年龄
            if (!Utils.IsNullOrEmpty(age))
            {
                string[] ageArray = age.Split(',');
                strTempSql = Utility.GetArrayWhere(ageArray, "LoanAge");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            if (!Utils.IsNullOrEmpty(buildingType) || !Utils.IsNullOrEmpty(buildingDate))
            {
                //BuildingDate
                StringBuilder sbOneChSql = new StringBuilder();
                //BuildDate
                StringBuilder sbTwoChSql = new StringBuilder();
                SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
                string strOneTempSql = string.Empty;
                //建筑类型
                if (!Utils.IsNullOrEmpty(buildingType))
                {
                    sbOneChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                    sbTwoChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                }
                //年代
                if (!Utils.IsNullOrEmpty(buildingDate))
                {
                    string[] buildingDateArray = buildingDate.Split(',');
                    strTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildingDate,120))");
                    strOneTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildDate,120))");
                    if (!Utils.IsNullOrEmpty(sbOneChSql.ToString()))
                    {
                        sbOneChSql.AppendFormat(" and ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" and ({0})", strOneTempSql);
                    }
                    else
                    {
                        sbOneChSql.AppendFormat(" ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" ({0})", strOneTempSql);
                    }
                }
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    sbOneSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ",

cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.CaseTable, sbOneChSql.ToString());
                }
                else
                {
                    sbOneSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ",

cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.CaseTable, sbOneChSql.ToString());
                }
            }
            #endregion

            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);

            string strSql = Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                strSqlExcute = string.Empty,
                strWhere = string.Empty,
                strSqlExcuteWhere = string.Empty;
            if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
            {
                strSql = string.Format("{0} {1} and Status=1 ",
                    strSql, sbLoanSql.ToString());
                strWhere = string.Format(" {0} and Status=1 ",
                    sbLoanSql.ToString());
            }
            else
            {
                strSql = string.Format("{0} Status=1 ", strSql);
                strWhere = " Status=1 ";
            }
            if (!Utils.IsNullOrEmpty(cityarrid.ToString()))
            {
                strSql = string.Format(" {0} and CityId in ({1}) ", strSql, cityarrid);
                strWhere = string.Format(" {0} and CityId in ({1}) ", strWhere, cityarrid);
            }
            if (!Utils.IsNullOrEmpty(itemarrid.ToString()))
            {
                strSql = string.Format("{0} and BankProjectId in ({1}) ", strSql, itemarrid);
                strWhere = string.Format("{0} and BankProjectId in ({!}) ", strWhere, itemarrid);
            }
            strWhere = string.Format("{0} where {1} ", Utility.loan_Data_Collateral, strWhere);

            if (!Utils.IsNullOrEmpty(sbOneSql.ToString()) &&
                !Utils.IsNullOrEmpty(sbTwoSql.ToString()) &&
                !Utils.IsNullOrEmpty(sbThreeSql.ToString()))
            {
                //默认楼栋
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    strSqlExcute = string.Format(" {0} {1}", strSql, sbOneSql.ToString());
                    strSqlExcuteWhere = string.Format("{0} {1}", strWhere, sbOneSql.ToString());
                }
                else
                {
                    strSqlExcute = string.Format(" {0} and {1}", strSql, sbOneSql.ToString());
                    strSqlExcuteWhere = string.Format("{0} and {1}", strWhere, sbOneSql.ToString());
                }
                if (pager.PageSize.Equals(0))
                    listColl = _mssqlado.GetList<DataCollateral>(strSqlExcute);
                else
                    listColl = _mssqlado.GetList<DataCollateral>(strSqlExcute, pager, strSqlExcuteWhere);

                //如果楼栋是没有找到,就用楼盘
                if (listColl.Count() == 0)
                {
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1}", strSql, sbTwoSql.ToString());
                        strSqlExcuteWhere = string.Format("{0} {1}", strWhere, sbTwoSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1}", strSql, sbTwoSql.ToString());
                        strSqlExcuteWhere = string.Format("{0} {1}", strWhere, sbTwoSql.ToString());
                    }
                    if (pager.PageSize.Equals(0))
                        listColl = _mssqlado.GetList<DataCollateral>(strSqlExcute);
                    else
                        listColl = _mssqlado.GetList<DataCollateral>(strSqlExcute, pager, strSqlExcuteWhere);
                }
                //如果楼盘是没有找到,就用案例
                if (listColl.Count() == 0)
                {
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1}", strSql, sbThreeSql.ToString());
                        strSqlExcuteWhere = string.Format("{0} {1}", strWhere, sbThreeSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1}", strSql, sbThreeSql.ToString());
                        strSqlExcuteWhere = string.Format("{0} {1}", strWhere, sbThreeSql.ToString());
                    }
                    if (pager.PageSize.Equals(0))
                        listColl = _mssqlado.GetList<DataCollateral>(strSqlExcute);
                    else
                        listColl = _mssqlado.GetList<DataCollateral>(strSqlExcute, pager, strSqlExcuteWhere);
                }
            }
            else
            {
                if (pager.PageSize.Equals(0))
                    listColl = _mssqlado.GetList<DataCollateral>(strSql);
                else
                    listColl = _mssqlado.GetList<DataCollateral>(strSql, pager, strWhere);
            }
            List<CollateralMonitorDetails> list = new List<CollateralMonitorDetails>();
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            foreach (var item in listColl)
            {
                CollateralMonitorDetails cmd = new CollateralMonitorDetails(item);

                var code = UtilityDALHelper.GetADOSYSCodeByCode(_mssql, item.PurposeCode);
                cmd.PurposeCodeName = code != null ? code.CodeName : "";
                var city = UtilityDALHelper.GetADOCityById(_mssql, item.CityId);
                cmd.CityName = city != null ? city.CityName : "";
                var area = UtilityDALHelper.GetADOSYSAreaById(_mssql, item.AreaId);
                cmd.AreaName = area != null ? area.AreaName : "";
                SysBankProject sysBankProject = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysBankProject>(cmd.BankProjectId);
                cmd.BankProjectName = sysBankProject != null ? sysBankProject.ProjectName : "";

                list.Add(cmd);
            }

            if (list.Count > 0)
                return Utility.GetJson(1, "获取成功", list, pager.Count);
            else
                return Utility.GetJson(0, "获取失败");
        }
        /// <summary>
        /// 模糊搜索根据押品中已匹配的楼盘信息
        /// </summary>
        /// <param name="cId">城市</param>
        /// <param name="name">模糊信息</param>
        /// <returns></returns>
        public string GetProjectByDataCollateral(int cId, string name)
        {
            StringBuilder sql = new StringBuilder();
            MSSQLADODAL _mssql = new MSSQLADODAL();

            SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
            sql.Append(Utility.GetMSSQL_SQL(typeof(DATProject), cityTable.ProjectTable));
            sql.Append(" ProjectId in (select distinct ProjectId from FxtLoan.dbo.Data_Collateral ");
            sql.AppendFormat(" where status=1 and cityid={0}) and ProjectName like '%{1}%'", cId, name);

            IList<DATProject> list = _mssql.GetList<DATProject>(sql.ToString());

            if (list.Count > 0)
                return Utility.GetJson(1, "获取成功", list);
            else
                return Utility.GetJson(0, "获取失败");
        }

        /// <summary>
        /// 模糊获得开发商
        /// </summary>
        /// <param name="cId">城市</param>
        /// <param name="name">模糊信息</param>
        /// <returns></returns>
        public string GetCompanyByDataCollateral(int cId, string name)
        {
            StringBuilder sql = new StringBuilder();
            MSSQLADODAL _mssql = new MSSQLADODAL();
            int code = 2001001;
            sql.Append(Utility.GetMSSQL_SQL(typeof(DATCompany), "[FXTProject].[dbo].DAT_Company"));
            sql.AppendFormat(" CompanyId in (select distinct CompanyId from [FXTProject].[dbo].LNK_P_Company where CompanyType={0} and cityid={1} and", code, cId);
            sql.AppendFormat(" ProjectId in (select distinct ProjectId from FxtLoan.dbo.Data_Collateral where status=1 and cityid={0})) ", cId);
            sql.AppendFormat(" and CompanyTypeCode={0}  and cityid={1} and ChineseName like '%{2}%'", code, cId, name);

            IList<DATCompany> list = _mssql.GetList<DATCompany>(sql.ToString());

            if (list.Count > 0)
                return Utility.GetJson(1, "获取成功", list);
            else
                return Utility.GetJson(0, "获取失败");
        }

        private string GetProjectIdWhere(string table, string where)
        {
            return string.Format(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ", table, where);
        }

        private string GetDBConvert(string column)
        {
            return string.Format("convert(int,convert(nvarchar(4),{0},120))", column);
        }
        /// <summary>
        /// 建筑年代条件获取
        /// </summary>
        /// <param name="mssql"></param>
        /// <param name="carray"></param>
        /// <param name="where"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetJZYearWhere(MSSQLADODAL mssql, string[] carray, string where, int type)
        {
            StringBuilder whereTemp = new StringBuilder();
            int aci = 0;
            string strWhere = string.Empty;
            while (aci < carray.Length)
            {
                SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(mssql, Convert.ToInt32(carray[aci]));
                if (type.Equals(0))
                    strWhere = GetProjectIdWhere(cityTable.BuildingTable, where);
                else if (type.Equals(1))
                    strWhere = GetProjectIdWhere(cityTable.ProjectTable, where);
                else if (type.Equals(2))
                    strWhere = GetProjectIdWhere(cityTable.CaseTable, where);
                if (aci != carray.Length - 1)
                {
                    whereTemp.AppendFormat("{0} and ", strWhere);
                }
                else
                {
                    whereTemp.AppendFormat(" {0} ", strWhere);
                }
                aci++;
            }
            return whereTemp.ToString();
        }

        private JObject GetJobject(string name, IList<DataCollateral> list, int type, MSSQLADODAL _mssql, string rate)
        {
            JObject _jobject = new JObject();
            _jobject.Add("name", name);
            _jobject.Add("count", list.Count());
            _jobject.Add("buildingarea", list.Sum(o => o.BuildingArea));
            _jobject.Add("loanamount", list.Sum(o => o.LoanAmount));
            _jobject.Add("oldrate", list.Sum(o => o.OldRate));
            _jobject.Add("guaranteeprice", list.Sum(o => o.GuaranteePrice));
            decimal oldRea = list.Sum(o => o.OldRate);
            if (!oldRea.Equals(0))
                oldRea = list.Sum(o => o.LoanAmount).Value / oldRea;
            _jobject.Add("oldavergerates", decimal.Round(oldRea, 2));
            if (type.Equals(1))
            {
                IList rateList = _mssql.GetListObject(rate);
                decimal rateCount = 0;
                for (int i = 0; i < rateList.Count; i++)
                {
                    rateCount += !Utils.IsNullOrEmpty(rateList[i].ToString()) ? decimal.Parse(rateList[i].ToString()) : 0;
                }
                //现估价值
                _jobject.Add("rate", rateCount);
                if (!rateCount.Equals(0))
                    rateCount = list.Sum(o => o.LoanAmount).Value / rateCount;
                //现平均抵押率
                _jobject.Add("avergerates", decimal.Round(rateCount, 2));
            }
            return _jobject;
        }
        #endregion

        #region 押品复估

        /// <summary>
        /// 复估管理
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">县区</param>
        /// <param name="aId">一页记录数</param>
        /// <param name="aId">当前页</param>
        /// <returns></returns>
        public string GetCollateralsByReassessment(int pId, int cId, int aId, int pageSize, int pageIndex, string cityarrid, string itemarrid)
        {
            List<JObject> list = new List<JObject>();
            UtilityPager page = new UtilityPager();
            page.PageSize = pageSize;
            page.PageIndex = pageIndex;
            string flag = Utility.GetJson(0, ""), where = string.Empty;
            //省份
            if (!pId.Equals(0) && cId.Equals(0) && aId.Equals(0))
            {

            }//省份、城市
            else if (!pId.Equals(0) && !cId.Equals(0) && aId.Equals(0))
            {
                where = string.Format(" Status=1 and ProvinceId={0} and CityId={1} ", pId, cId);
                MSSQLADODAL adodb = new MSSQLADODAL();
                string strSql = string.Format("{0} {1}",
                        Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral), where);
                List<DataCollateral> listCollateral = adodb.GetList<DataCollateral>(strSql, page,
                    string.Format("{0} where {1}", Utility.loan_Data_Collateral, where));
                flag = Utility.GetJson(1, "获取成功", listCollateral, page.Count);

            }//省份、城市、县区
            else if (!pId.Equals(0) && !cId.Equals(0) && !aId.Equals(0))
            {
            }
            return flag;
        }

        /// <summary>
        /// 得到指定押品的估价值
        /// </summary>
        /// <param name="id">押品ID</param>
        /// <returns></returns>
        public string ReassessmentCalculation(int id)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string sqlado = string.Format("{0} [Status]=1 and Id={1}",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral), id),
                message = string.Empty;
            DataCollateral dataCollateral = mssqlado.GetModel<DataCollateral>(sqlado);
            int flag = 0;
            MSSQLADODAL _mssqlado = new MSSQLADODAL();
            var cityTable = UtilityDALHelper.GetCityADOTable(_mssqlado, dataCollateral.CityId);
            if (dataCollateral != null && cityTable != null)
            {
                try
                {
                    sqlado = string.Format("{0} CityId={1}",
                            Utility.GetMSSQL_SQL(typeof(SysCityFxtCompany), Utility.loand_SysCityFxtCompany), dataCollateral.CityId);
                    //获得城市对应公司,以备指定数据复估值
                    SysCityFxtCompany sysCityFxtCompany = mssqlado.GetModel<SysCityFxtCompany>(sqlado);
                    int companyId = sysCityFxtCompany != null ? sysCityFxtCompany.CompanyId : 25;

                    sqlado = string.Format("{0} IsFirst=1 and CollateralId={1}",
                        Utility.GetMSSQL_SQL(typeof(DataReassessment), Utility.loan_Data_Data_Reassessment),
                        dataCollateral.Id);
                    DataReassessment dataReassessment = mssqlado.GetModel<DataReassessment>(sqlado);//检测是否有首次复估值

                    int proId = dataCollateral.ProjectId,
                        buildId = dataCollateral.BuildingId != null ? dataCollateral.BuildingId.Value : 0,
                        houseId = dataCollateral.RoomId != null ? dataCollateral.RoomId.Value : 0;

                    _mssqlado = new MSSQLADODAL();
                    //cityTable.HistoryTable
                    string sql = Utility.GetMSSQL_SQL(typeof(DATQueryHistory), cityTable.HistoryTable),
                           excute = string.Empty,
                        //计算方式
                           calculationMode = string.Empty;

                    decimal? count = 0;
                    int isFirst = 0;

                    DateTime currentDT = DateTime.Now;

                    //得到楼盘的所有维度价格
                    FxtService.Contract.APIInterface.IFxtAPI fxtAPI = new FxtService.Service.APIActualize.FxtAPI();
                    List<DATProjectAvgPrice> listDatProAvgPrice = null;

                    if (dataReassessment == null)//是否首次复估
                    {
                        isFirst = 1;
                        if (!proId.Equals(0) && !buildId.Equals(0) && !houseId.Equals(0))//关联到房号
                        {
                            #region 关联到房号
                            excute = string.Format("{0} ProjectId={1} and BuildingId={2} and HouseId={3} and casestartdate>='{4}' and caseenddate<='{5}'",
                                     sql, proId, buildId, houseId,
                                     currentDT.AddMonths(-3).ToString("yyyy-MM-dd"),
                                     currentDT.ToString("yyyy-MM-dd"));

                            DATQueryHistory datQuery = _mssqlado.GetModel<DATQueryHistory>(excute);//自动估价信息
                            if (datQuery != null)//能否得到自动估价信息
                            {
                                count = datQuery.HEPrice;
                                calculationMode = "初次房号,得到自动估价";
                            }
                            else
                            {
                                //楼栋信息
                                DATBuilding datBuilding = _mssqlado
                                    .GetModel<DATBuilding>(string.Format("{0} BuildingId={1} and ProjectId={2} and FxtCompanyId={3}",
                                Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable), buildId, proId, companyId));
                                //得到房号信息
                                DATHouse datHouse = _mssqlado
                                    .GetModel<DATHouse>(string.Format("{0} BuildingId={1} and HouseId={2} and FxtCompanyId={3}",
                                Utility.GetMSSQL_SQL(typeof(DATHouse), cityTable.HouseTable), buildId, houseId, companyId));
                                if (datHouse != null)
                                {
                                    //房号的维度
                                    listDatProAvgPrice = Utils.Deserialize<List<DATProjectAvgPrice>>(fxtAPI.Cross(proId,
                                        dataCollateral.CityId, datHouse.PurposeCode.Value, currentDT.ToString("yyyy-MM-dd")));
                                    //维度价格是否为空
                                    if (listDatProAvgPrice != null && !listDatProAvgPrice.Count.Equals(0))
                                    {
                                        sqlado = string.Format("{0} CityId=0 and 总楼层开始={1} and 所在楼层={2}",
                                    Utility.GetMSSQL_SQL(typeof(SysFloorPrice), Utility.SysFloorPrice),
                                    datBuilding.TotalFloor, datHouse.FloorNo);//楼层差

                                        SysFloorPrice sysFP = _mssqlado.GetModel<SysFloorPrice>(sqlado);

                                        sysFP.楼层差 = 1 + (sysFP.楼层差 / 100);
                                        //朝向系数,景观系数
                                        decimal? front = GetSysModulusPrice(_mssqlado, 1033001, datHouse.FrontCode.Value),
                                                 sight = GetSysModulusPrice(_mssqlado, 1033002, datHouse.SightCode.Value);

                                        //面积段&建筑类型
                                        count = ReassessmentAreaAndBuildTypeCode(listDatProAvgPrice, datBuilding, dataCollateral,
                                            sysFP.楼层差, front, sight, datHouse);
                                        calculationMode = "初次房号,面积段&建筑类型";
                                        if (count.Equals(0M)) //“建筑类型”的细分均价平均值
                                        {
                                            count = ReassessmentBuildTypeCode(_mssqlado, listDatProAvgPrice, datBuilding, dataCollateral,
                                            sysFP.楼层差, front, sight, datHouse);
                                            calculationMode = "初次房号,“建筑类型”的细分均价平均值";
                                        }
                                        if (count.Equals(0M)) //“面积段”的细分均价平均值
                                        {
                                            count = ReassessmentArea(listDatProAvgPrice, datBuilding, dataCollateral,
                                                sysFP.楼层差, front, sight, datHouse);
                                            calculationMode = "初次房号,“面积段”的细分均价平均值";
                                        }
                                        if (count.Equals(0M)) //调用楼盘均价平均值
                                        {
                                            count = ReassessmentProject(dataCollateral, datHouse.PurposeCode.Value
                                                , currentDT, sysFP.楼层差, front, sight, 1);
                                            calculationMode = "初次房号,楼盘均价平均值";
                                        }
                                    }
                                }
                                else
                                {
                                    message = string.Format("编号为 {0} 押品,因初次复估,无法获取房号信息,无法复估",
                                        dataCollateral.Number);
                                }
                            }
                            #endregion
                        }
                        else if (!proId.Equals(0) && !buildId.Equals(0) && houseId.Equals(0))//关联到楼栋
                        {
                            #region 关联到楼栋

                            //楼栋信息
                            DATBuilding datBuilding = _mssqlado
                                .GetModel<DATBuilding>(string.Format("{0} BuildingId={1} and ProjectId={2} and FxtCompanyId={3}",
                                Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable), buildId, proId, companyId));

                            if (datBuilding != null)
                            {
                                listDatProAvgPrice = Utils.Deserialize<List<DATProjectAvgPrice>>(fxtAPI.Cross(proId,
                                    dataCollateral.CityId, datBuilding.PurposeCode.Value, currentDT.ToString("yyyy-MM-dd")));

                                if (System.Text.RegularExpressions.Regex.IsMatch(dataCollateral.FloorNumber, "^[0-9]+$") &&
                                    listDatProAvgPrice != null &&
                                    listDatProAvgPrice != null &&
                                    !listDatProAvgPrice.Count.Equals(0))
                                {
                                    sqlado = string.Format("{0} CityId=0 and 总楼层开始={1} and 所在楼层={2}",
                                    Utility.GetMSSQL_SQL(typeof(SysFloorPrice), Utility.SysFloorPrice),
                                    datBuilding.TotalFloor, dataCollateral.FloorNumber);//楼层差

                                    SysFloorPrice sysFP = _mssqlado.GetModel<SysFloorPrice>(sqlado);

                                    sysFP.楼层差 = 1 + (sysFP.楼层差 / 100);

                                    //面积段&建筑类型
                                    count = ReassessmentAreaAndBuildTypeCode(listDatProAvgPrice, datBuilding, dataCollateral,
                                        sysFP.楼层差, 0, 0);
                                    calculationMode = "初次楼栋,面积段&建筑类型";
                                    if (count.Equals(0M))//建筑类型维度楼盘均价
                                    {
                                        count = ReassessmentBuildTypeCode(_mssqlado, listDatProAvgPrice, datBuilding, dataCollateral,
                                            sysFP.楼层差, 0, 0);
                                        calculationMode = "初次楼栋,建筑类型维度楼盘均价";
                                    }
                                    if (count.Equals(0M))//面积段维度楼盘均价
                                    {
                                        count = ReassessmentArea(listDatProAvgPrice, datBuilding, dataCollateral,
                                            sysFP.楼层差, 0, 0);
                                        calculationMode = "初次楼栋,面积段维度楼盘均价";
                                    }
                                    if (count.Equals(0M))//楼盘均价
                                    {
                                        count = ReassessmentProject(dataCollateral, datBuilding.PurposeCode.Value
                                                , currentDT, sysFP.楼层差, 0, 0, 2);
                                        calculationMode = "初次楼栋,楼盘均价";
                                    }
                                }
                            }
                            else
                            {
                                message = string.Format("编号为 {0} 押品,因初次复估,无法获取楼栋信息,无法复估",
                                        dataCollateral.Number);
                            }
                            #endregion
                        }
                        else if (!proId.Equals(0) && buildId.Equals(0) && houseId.Equals(0))//关联到楼盘
                        {
                            #region 关联到楼盘
                            //得到正式库楼盘信息,不对匹配临时库进行复估
                            DATProject datProject = _mssqlado
                                .GetModel<DATProject>(string.Format("{0} ProjectId={1} and  and FxtCompanyId={2}",
                                Utility.GetMSSQL_SQL(typeof(DATProject), cityTable.ProjectTable), proId, companyId));

                            listDatProAvgPrice = Utils.Deserialize<List<DATProjectAvgPrice>>(fxtAPI.Cross(proId,
                                dataCollateral.CityId, datProject.PurposeCode, currentDT.ToString("yyyy-MM-dd")));
                            if (listDatProAvgPrice != null && !listDatProAvgPrice.Count.Equals(0))
                            {
                                //面积或大于18层
                                count = ReassessmentProjectArea(listDatProAvgPrice, dataCollateral, datProject.PurposeCode, currentDT);
                                calculationMode = "初次楼盘,面积或大于18层";
                                if (count.Equals(0M))//楼盘均价
                                {
                                    count = ReassessmentProject(dataCollateral, datProject.PurposeCode, currentDT, 0, 0, 0, 0);
                                    calculationMode = "初次楼盘,楼盘均价";
                                }
                            }
                            #endregion
                        }
                    }
                    else//之后每月各次复估单价
                    {
                        isFirst = 0;
                        if (!proId.Equals(0) && !buildId.Equals(0) && !houseId.Equals(0))//关联到房号
                        {
                            #region 关联到房号

                            excute = string.Format("{0} ProjectId={1} and BuildingId={2} and HouseId={3} and casestartdate>='{4}' and caseenddate<='{5}'",
                                     sql, proId, buildId, houseId,
                                     currentDT.AddMonths(-3).ToString("yyyy-MM-dd"),
                                     currentDT.ToString("yyyy-MM-dd"));

                            DATQueryHistory datQuery = _mssqlado.GetModel<DATQueryHistory>(excute);//自动估价信息

                            if (datQuery != null) //是否有自动估价结果
                            {
                                count = datQuery.HEPrice;
                                calculationMode = "之后每月各次复估房号,自动估价";
                            }
                            else //无自动估价结果,通过该房号,通过"面积段&建筑类型"
                            {
                                //楼栋信息
                                DATBuilding datBuilding = _mssqlado
                                    .GetModel<DATBuilding>(string.Format("{0} BuildingId={1} and ProjectId={2} and FxtCompanyId={3}",
                                Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable), buildId, proId, companyId));
                                //得到房号信息
                                DATHouse datHouse = _mssqlado
                                    .GetModel<DATHouse>(string.Format("{0} BuildingId={1} and HouseId={2} and FxtCompanyId={3}",
                                Utility.GetMSSQL_SQL(typeof(DATHouse), cityTable.HouseTable), buildId, houseId, companyId));
                                if (datHouse != null)
                                {
                                    //房号的维度
                                    listDatProAvgPrice = Utils.Deserialize<List<DATProjectAvgPrice>>(fxtAPI.Cross(proId,
                                        dataCollateral.CityId, datHouse.PurposeCode.Value, currentDT.ToString("yyyy-MM-dd")));
                                    //维度价格是否为空
                                    if (listDatProAvgPrice != null && !listDatProAvgPrice.Count.Equals(0))
                                    {
                                        sqlado = string.Format("{0} CityId=0 and 总楼层开始={1} and 所在楼层={2}",
                                   Utility.GetMSSQL_SQL(typeof(SysFloorPrice), Utility.SysFloorPrice),
                                   datBuilding.TotalFloor, datHouse.FloorNo);//楼层差

                                        SysFloorPrice sysFP = _mssqlado.GetModel<SysFloorPrice>(sqlado);


                                        sysFP.楼层差 = 1 + (sysFP.楼层差 / 100);
                                        //朝向系数,景观系数
                                        decimal? front = GetSysModulusPrice(_mssqlado, 1033001, datHouse.FrontCode.Value),
                                                 sight = GetSysModulusPrice(_mssqlado, 1033002, datHouse.SightCode.Value);

                                        //面积段&建筑类型
                                        count = ReassessmentAreaAndBuildTypeCode(listDatProAvgPrice, datBuilding, dataCollateral,
                                            sysFP.楼层差, front, sight, datHouse);
                                        calculationMode = "之后每月各次复估房号,面积段&建筑类型";
                                        if (count.Equals(0M)) //“建筑类型”的细分均价平均值
                                        {
                                            count = ReassessmentBuildTypeCode(_mssqlado, listDatProAvgPrice, datBuilding, dataCollateral,
                                             sysFP.楼层差, front, sight, datHouse);
                                            calculationMode = "之后每月各次复估房号,建筑类型细分均价平均值";
                                        }
                                        if (count.Equals(0M)) //“面积段”的细分均价平均值
                                        {
                                            count = ReassessmentArea(listDatProAvgPrice, datBuilding, dataCollateral,
                                             sysFP.楼层差, front, sight, datHouse);
                                            calculationMode = "之后每月各次复估房号,面积段细分均价平均值";
                                        }
                                        if (count.Equals(0M)) //调用楼盘均价平均值
                                        {
                                            count = ReassessmentProject(dataCollateral, datHouse.PurposeCode.Value
                                                , currentDT, sysFP.楼层差, front, sight, 1);
                                            calculationMode = "之后每月各次复估房号,楼盘均价平均值";
                                        }
                                        if (count.Equals(0M)) //无相应楼盘均价
                                        {
                                            count = ReassessmentNotProject(_mssqlado, dataCollateral, currentDT);
                                            calculationMode = "之后每月各次复估房号,无相应楼盘均价";
                                        }
                                    }
                                }
                                else
                                {
                                    message = string.Format("编号为 {0} 押品,因之后每月各次复估,无法获取房号信息,无法复估",
                                        dataCollateral.Number);
                                }
                            }
                            #endregion
                        }
                        else if (!proId.Equals(0) && !buildId.Equals(0) && houseId.Equals(0))//关联到楼栋
                        {
                            #region 关联到楼栋

                            //楼栋信息
                            DATBuilding datBuilding = _mssqlado
                                .GetModel<DATBuilding>(string.Format("{0} BuildingId={1} and ProjectId={2} and FxtCompanyId={3}",
                                Utility.GetMSSQL_SQL(typeof(DATBuilding), cityTable.BuildingTable), buildId, proId, companyId));

                            if (datBuilding != null)
                            {
                                listDatProAvgPrice = Utils.Deserialize<List<DATProjectAvgPrice>>(fxtAPI.Cross(proId,
                                    dataCollateral.CityId, datBuilding.PurposeCode.Value, currentDT.ToString("yyyy-MM-dd")));

                                if (System.Text.RegularExpressions.Regex.IsMatch(dataCollateral.FloorNumber, "^[0-9]+$") &&
                                    listDatProAvgPrice != null &&
                                    !listDatProAvgPrice.Count.Equals(0))
                                {
                                    sqlado = string.Format("{0} CityId=0 and 总楼层开始={1} and 所在楼层={2}",
                                   Utility.GetMSSQL_SQL(typeof(SysFloorPrice), Utility.SysFloorPrice),
                                   datBuilding.TotalFloor, dataCollateral.FloorNumber);//楼层差

                                    SysFloorPrice sysFP = _mssqlado.GetModel<SysFloorPrice>(sqlado);

                                    sysFP.楼层差 = 1 + (sysFP.楼层差 / 100);

                                    //面积段&建筑类型
                                    count = ReassessmentAreaAndBuildTypeCode(listDatProAvgPrice, datBuilding, dataCollateral,
                                        sysFP.楼层差, 0, 0);
                                    calculationMode = "之后每月各次复估楼栋,面积段&建筑类型";
                                    if (count.Equals(0M))//建筑类型维度楼盘均价
                                    {
                                        count = ReassessmentBuildTypeCode(_mssqlado, listDatProAvgPrice, datBuilding, dataCollateral,
                                            sysFP.楼层差, 0, 0);
                                        calculationMode = "之后每月各次复估楼栋,建筑类型维度楼盘均价";
                                    }
                                    if (count.Equals(0M))//面积段维度楼盘均价
                                    {
                                        count = ReassessmentArea(listDatProAvgPrice, datBuilding, dataCollateral,
                                             sysFP.楼层差, 0, 0);
                                        calculationMode = "之后每月各次复估楼栋,面积段维度楼盘均价";
                                    }
                                    if (count.Equals(0M))//楼盘均价
                                    {
                                        count = ReassessmentProject(dataCollateral, datBuilding.PurposeCode.Value
                                                , currentDT, sysFP.楼层差, 0, 0, 2);
                                        calculationMode = "之后每月各次复估楼栋,楼盘均价";
                                    }
                                    if (count.Equals(0M)) //无相应楼盘均价
                                    {
                                        count = ReassessmentNotProject(_mssqlado, dataCollateral, currentDT);
                                        calculationMode = "之后每月各次复估楼栋,无相应楼盘均价";
                                    }
                                }
                            }
                            else
                            {
                                message = string.Format("编号为 {0} 押品,因之后每月各次复估,无法获取楼栋信息,无法复估",
                                       dataCollateral.Number);
                            }
                            #endregion
                        }
                        else if (!proId.Equals(0) && buildId.Equals(0) && houseId.Equals(0))//关联到楼盘
                        {
                            #region 关联到楼盘
                            //得到正式库楼盘信息,不对匹配临时库进行复估
                            DATProject datProject = _mssqlado.GetModel<DATProject>(string.Format("{0} ProjectId={1} and FxtCompanyId={2}",
                                Utility.GetMSSQL_SQL(typeof(DATProject), cityTable.ProjectTable), proId, companyId));

                            listDatProAvgPrice = Utils.Deserialize<List<DATProjectAvgPrice>>(fxtAPI.Cross(proId,
                                dataCollateral.CityId, datProject.PurposeCode, currentDT.ToString("yyyy-MM-dd")));
                            if (listDatProAvgPrice != null && !listDatProAvgPrice.Count.Equals(0))
                            {
                                //面积或大于18层
                                count = ReassessmentProjectArea(listDatProAvgPrice, dataCollateral, datProject.PurposeCode, currentDT);
                                calculationMode = "之后每月各次复估楼盘,面积或大于18层";
                                if (count.Equals(0M))//楼盘均价
                                {
                                    count = ReassessmentProject(dataCollateral, datProject.PurposeCode, currentDT, 0, 0, 0, 0);
                                    calculationMode = "之后每月各次复估楼盘,楼盘均价";
                                }
                                if (count.Equals(0M)) //无相应楼盘均价
                                {
                                    count = ReassessmentNotProject(_mssqlado, dataCollateral, currentDT);
                                    calculationMode = "之后每月各次复估楼盘,无相应楼盘均价";
                                }
                            }
                            #endregion
                        }
                    }
                    //是否有错误信息
                    if (Utils.IsNullOrEmpty(message))
                    {
                        //存储计算后的复估结果
                        bool _isCount = count != null && !count.Equals(0M);
                        decimal _price = _isCount ? count.Value : 0,
                                _tprice = _isCount ? _price * (decimal)0.9 : 0,
                                _twprice = _isCount ? _price * (decimal)0.8 : 0;
                        SqlParameter[] param = new SqlParameter[]{
                        new SqlParameter("@CollateralId",dataCollateral.Id),
                        new SqlParameter("@Months",currentDT.ToString("yyyyMM")),
                        new SqlParameter("@CreateDate",DateTime.Now),
                        new SqlParameter("@CalculationMode",calculationMode),
                        new SqlParameter("@IsFirst",isFirst),
                        new SqlParameter("@Price",_price),
                        new SqlParameter("@TenPrice",_tprice),
                        new SqlParameter("@TwentyPrice",_twprice),
                        new SqlParameter("@ArrivedLoanRates",_isCount?dataCollateral.LoanBalance / _price:0),
                        new SqlParameter("@TenArrivedLoanRates",_isCount?dataCollateral.LoanBalance/_tprice:0),
                        new SqlParameter("@TwentyArrivedLoanRates",_isCount?dataCollateral.LoanBalance/_twprice:0)
                    };
                        _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);

                        sqlado = string.Format(@"Insert Into {0} with(rowlock) (CollateralId,Months,CreateDate,CalculationMode,IsFirst,Price,TenPrice,TwentyPrice,ArrivedLoanRates,TenArrivedLoanRates,TwentyArrivedLoanRates) 
values(@CollateralId,@Months,@CreateDate,@CalculationMode,@IsFirst,@Price,@TenPrice,@TwentyPrice,@ArrivedLoanRates,@TenArrivedLoanRates,@TwentyArrivedLoanRates)",
                            Utility.loan_Data_Data_Reassessment);

                        //保存复估值
                        flag = _mssqlado.CUD(sqlado, param);
                    }
                }
                catch (Exception exe)
                {
                    message = string.Format("编号为 {0} 押品,因{1},无法复估", dataCollateral.Number, exe.Message);
                }
            }
            else
            {
                if (cityTable == null)
                {
                    message = string.Format("编号为 {0} 押品因城市无法匹配,无法复估", dataCollateral.Number);
                }
            }
            return Utility.GetJson(flag > 0 ? 1 : 0, flag > 0 ? "成功" : message);
        }
        #region 复估计算

        /// <summary>
        /// 面积段&建筑类型 复估值
        /// </summary>
        /// <param name="mssql">数据库</param>
        /// <param name="listDatProAvgPrice">价格维度集合</param>
        /// <param name="datBuilding">楼栋对象</param>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="cityTable">区域城市对象</param>
        /// <param name="lcc">楼层差</param>
        /// <param name="front">朝向系数</param>
        /// <param name="sight">景观系数</param>
        /// <param name="house">房号对象</param>
        /// <returns></returns>
        decimal? ReassessmentAreaAndBuildTypeCode(List<DATProjectAvgPrice> listDatProAvgPrice,
            DATBuilding datBuilding, DataCollateral dataCollateral,
            decimal? lcc, decimal? front, decimal? sight, DATHouse house = null)
        {
            //得到楼盘均价,条件面积+建筑类型
            DATProjectAvgPrice datProAvgPrice = listDatProAvgPrice.Where(avgitem =>
                                    (avgitem.BuildingTypeCode != null ?
                                    avgitem.BuildingTypeCode == datBuilding.BuildingTypeCode : false) &&
                                    avgitem.BuildingTypeCode == datBuilding.BuildingTypeCode &&
                                    avgitem.PurposeType == (house != null ? house.PurposeCode : datBuilding.PurposeCode) &&
                                    avgitem.ProjectId == dataCollateral.ProjectId &&
                                    avgitem.CityId == dataCollateral.CityId).FirstOrDefault();

            decimal? count = 0M;
            //“面积段&建筑类型”维度价格
            if (datProAvgPrice != null)
            {
                //楼层差首先计算
                count = datProAvgPrice.AvgPrice * lcc;
                if (house != null)//只有关联到房号的才进行朝向、景观修正
                {
                    //如果有,朝向系数计算
                    if (!front.Equals(0M))
                    {
                        count = count * front;
                    }
                    //如果有,景观系数计算
                    if (!sight.Equals(0M))
                    {
                        count = count * sight;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 建筑类型 复估值
        /// </summary>
        /// <param name="mssql">数据库</param>
        /// <param name="listDatProAvgPrice">价格维度集合</param>
        /// <param name="datBuilding">楼层对象</param>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="cityTable">区域城市对象</param>
        /// <param name="lcc">楼层差系数</param>
        /// <param name="front">朝向系数</param>
        /// <param name="sight">景观系数</param>
        /// <returns></returns>
        decimal? ReassessmentBuildTypeCode(MSSQLADODAL mssql, List<DATProjectAvgPrice> listDatProAvgPrice,
            DATBuilding datBuilding, DataCollateral dataCollateral, decimal? lcc, decimal? front,
            decimal? sight, DATHouse house = null)
        {
            //得到楼盘均价,条件建筑类型
            IEnumerable<DATProjectAvgPrice> listBuildingType = listDatProAvgPrice.Where(item =>
                                    item.BuildingTypeCode == datBuilding.BuildingTypeCode &&
                                     item.PurposeType == (house != null ? house.PurposeCode : datBuilding.PurposeCode) &&
                                     item.ProjectId == dataCollateral.ProjectId &&
                                     item.CityId == dataCollateral.CityId &&
                                     item.FxtCompanyId == 25);

            //面积系数
            decimal? area = GetSysModulusPrice(mssql, 1033005, GetBuildAreaCode(dataCollateral.BuildingArea)),
                     count = 0M;
            if (!listBuildingType.Count().Equals(0))//结果不是零
            {
                //计算该建筑类型中各面积均价
                int bCodeSum = 0;
                foreach (var item in listBuildingType)
                {
                    bCodeSum += (int)(item.AvgPrice / GetSysModulusPrice(mssql, 1033005, item.BuildingAreaType.Value));
                }
                //楼层差、面积系数 修正
                count = bCodeSum * area * lcc;

                if (house != null)//如果关联到房号则进行朝向、景观修正
                {
                    //如果有,朝向系数计算
                    if (!front.Equals(0M))
                    {
                        count = count * front;
                    }
                    //如果有,景观系数计算
                    if (!sight.Equals(0M))
                    {
                        count = count * sight;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 面积 复估值
        /// </summary>
        /// <param name="mssql">数据库</param>
        /// <param name="listDatProAvgPrice">价格维度集合</param>
        /// <param name="datBuilding">楼层对象</param>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="cityTable">区域城市对象</param>
        /// <param name="lcc">楼层差系数</param>
        /// <param name="front">朝向系数</param>
        /// <param name="sight">景观系数</param>
        /// <returns></returns>
        decimal? ReassessmentArea(List<DATProjectAvgPrice> listDatProAvgPrice,
            DATBuilding datBuilding, DataCollateral dataCollateral, decimal? lcc,
            decimal? front, decimal? sight, DATHouse house = null)
        {
            DATProjectAvgPrice buildingArea = listDatProAvgPrice.Where(item =>
                                     item.BuildingAreaType == GetBuildAreaCode(dataCollateral.BuildingArea) &&
                                     item.BuildingTypeCode == 0 &&
                                     item.PurposeType == (house != null ? house.PurposeCode : datBuilding.PurposeCode) &&
                                     item.ProjectId == dataCollateral.ProjectId &&
                                     item.CityId == dataCollateral.CityId &&
                                     item.FxtCompanyId == 25).FirstOrDefault();//得到楼盘均价,条件面积段
            decimal? count = 0M;

            if (buildingArea != null)//结果不是空
            {
                //楼层差
                count = buildingArea.AvgPrice * lcc;
                if (house != null)
                {
                    //如果有,朝向系数计算
                    if (!front.Equals(0M))
                    {
                        count = count * front;
                    }
                    //如果有,景观系数计算
                    if (!sight.Equals(0M))
                    {
                        count = count * sight;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 楼盘 复估值
        /// </summary>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="codeType">用途类型</param>
        /// <param name="date">日期时间</param>
        /// <param name="lcc">楼层差</param>
        /// <param name="front">朝向系数</param>
        /// <param name="sight">景观系数</param>
        /// <returns></returns>
        decimal? ReassessmentProject(DataCollateral dataCollateral, int codeType,
            DateTime date, decimal? lcc, decimal? front, decimal? sight, int isHose)
        {
            //得到楼盘的所有维度价格
            FxtService.Contract.APIInterface.IFxtAPI fxtAPI = new FxtService.Service.APIActualize.FxtAPI();
            JObject data = JObject.Parse(fxtAPI.CrossProjectByCodeType(dataCollateral.ProjectId,
                dataCollateral.CityId, codeType, date.ToString("yyyy-MM-dd")));

            decimal? count = 0M;
            if (data["data"] != null)//结果存在
            {
                //楼层差
                if (isHose.Equals(1) && isHose.Equals(2))//如果是房号和楼栋就进行楼层差计算,否则直接给均价
                    count = int.Parse(data["data"].ToString()) * lcc;
                else
                    count = int.Parse(data["data"].ToString());

                if (isHose.Equals(1))//只有房号
                {
                    //如果有,朝向系数计算
                    if (!front.Equals(0))
                    {
                        count = count * front;
                    }
                    //如果有,景观系数计算
                    if (!sight.Equals(0))
                    {
                        count = count * sight;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 关联到楼盘的 面积或者大于18楼的均价
        /// </summary>
        /// <param name="listDatProAvgPrice">维度集合</param>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="codeType">用途</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        decimal? ReassessmentProjectArea(List<DATProjectAvgPrice> listDatProAvgPrice,
            DataCollateral dataCollateral, int codeType, DateTime date)
        {
            decimal? count = 0M;
            if (System.Text.RegularExpressions.Regex.IsMatch(dataCollateral.FloorNumber, "^[0-9]+$"))
            {
                if (Convert.ToInt32(dataCollateral.FloorNumber) <= 18)
                {
                    //得到楼盘的所有维度价格
                    FxtService.Contract.APIInterface.IFxtAPI fxtAPI = new FxtService.Service.APIActualize.FxtAPI();
                    JObject data = JObject.Parse(fxtAPI.CrossProjectByCodeType(dataCollateral.ProjectId,
                        dataCollateral.CityId, codeType, date.ToString("yyyy-MM-dd")));
                    if (data["data"] != null)//结果不是空
                    {
                        count = decimal.Parse(data["data"].ToString());
                    }
                }
                else
                {
                    //面积段、高层
                    DATProjectAvgPrice buildingAreaAndTypeCode = listDatProAvgPrice.Where(edapitem =>
                                             edapitem.BuildingAreaType == GetBuildAreaCode(dataCollateral.BuildingArea) &&
                                             edapitem.BuildingTypeCode == 2003004 &&
                                             edapitem.ProjectId == dataCollateral.ProjectId &&
                                             edapitem.CityId == dataCollateral.CityId).FirstOrDefault();
                    count = buildingAreaAndTypeCode.AvgPrice;
                }
            }
            return count;
        }

        /// <summary>
        /// 无相应楼盘均价 根据 上个月复估价格*（当月行政区楼盘均价/上月行政区楼盘均价） 得到该月复估单价
        /// </summary>
        /// <param name="mssql">数据库</param>
        /// <param name="dataCollateral">押品对象</param>
        /// <param name="dt">当前时间</param>
        /// <returns></returns>
        decimal? ReassessmentNotProject(MSSQLADODAL mssql, DataCollateral dataCollateral, DateTime dt)
        {
            //当月行政区单价

            string sql = string.Format("{0} ProjectId=0 and CityId={1} and AreaId={2} and SubAreaId=0 and AvgPriceDate={3}",
                       Utility.GetMSSQL_SQL(typeof(DATAvgPriceMonth), Utility.DATAvgPriceMonth),
                       dataCollateral.CityId, dataCollateral.AreaId, dt.ToString("yyyy-MM-dd"));

            DATAvgPriceMonth datCurrentAvgPriceMonth = mssql.GetModel<DATAvgPriceMonth>(sql);

            //上月行政区单价
            sql = string.Format("{0} ProjectId=0 and CityId={1} and AreaId={2} and SubAreaId=0 and AvgPriceDate={3}",
                       Utility.GetMSSQL_SQL(typeof(DATAvgPriceMonth), Utility.DATAvgPriceMonth),
                       dataCollateral.CityId, dataCollateral.AreaId, dt.AddMonths(-1).ToString("yyyy-MM-dd"));

            DATAvgPriceMonth datProAvgPriceMonth = mssql.GetModel<DATAvgPriceMonth>(sql);

            mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            //上月复估价格
            sql = string.Format("{0} CollateralId={1} and Months='{2}'",
                       Utility.GetMSSQL_SQL(typeof(DataReassessment), Utility.loan_Data_Data_Reassessment),
                       dataCollateral.Id, dt.AddMonths(-1).ToString("yyyyMM"));

            DataReassessment dataProReassessment = mssql.GetModel<DataReassessment>(sql);

            if (dataProReassessment != null && datProAvgPriceMonth != null && !datProAvgPriceMonth.AvgPrice.Equals(0))
            {
                return (datCurrentAvgPriceMonth.AvgPrice / datProAvgPriceMonth.AvgPrice) * dataProReassessment.Price;
            }
            else
                return 0;
        }

        /// <summary>   
        /// 根据相应的面积得到相关Code
        /// </summary>
        /// <param name="buildArea">面积</param>
        /// <returns></returns>
        int GetBuildAreaCode(decimal buildArea)
        {
            if (buildArea < 30)
            {//小于
                return 8006001;
            }
            else if (buildArea >= 30 && buildArea < 60)//大于等于30且小于60
            {
                return 8006002;
            }
            else if (buildArea >= 60 && buildArea < 90)//大于等于60且小于90
            {
                return 8006003;
            }
            else if (buildArea >= 90 && buildArea <= 120)//大于等于90且小于等于120
            {
                return 8006004;
            }
            else
            {//大于120
                return 8006005;
            }
        }

        /// <summary>
        /// 根据系数类型和系数细分类型得到 系数
        /// </summary>
        /// <param name="mssql"></param>
        /// <param name="ModulusTypeCode">系数类型</param>
        /// <param name="ModulusCode">系数细分类型</param>
        /// <returns></returns>
        decimal? GetSysModulusPrice(MSSQLADODAL mssql, int ModulusTypeCode, int ModulusCode)
        {
            string sql = string.Format("{0} CityId=0 and ModulusTypeCode={1} and ModulusCode={2}",
                        Utility.GetMSSQL_SQL(typeof(SysModulusPrice), Utility.SysModulusPrice),
                        ModulusTypeCode, ModulusCode);

            SysModulusPrice sysMP = mssql.GetModel<SysModulusPrice>(sql);
            return sysMP != null ? (sysMP.Percentage == 1 ? (1 + (sysMP.Modulus / 100)) : sysMP.Modulus) : 0;
        }
        #endregion

        /// <summary>
        /// 获取某个押品的复估列表
        /// </summary>
        /// <param name="id">押品ID</param>
        /// <param name="nMonths">月数</param>
        /// <returns></returns>
        public string GetReassessment(int id, int nMonths)
        {
            MSSQLADODAL adodb = new MSSQLADODAL(Utility.DBFxtLoan);

            //IList<DataReassessment> listReassessment = _mssql.GetListCustom<DataReassessment>(
            //    (Expression<Func<DataReassessment, bool>>)(item =>
            //    item.CollateralId == id &&
            //    item.CreateDate <= DateTime.Now &&
            //    item.CreateDate >= DateTime.Now.AddMonths(-nMonths)))
            //    .OrderBy(item => item.CreateDate).ToList();

            string strSql = string.Format("{0} CollateralId={1} and CreateDate between '{2}' and '{3}'",
                         Utility.GetMSSQL_SQL(typeof(DataReassessment), Utility.loan_Data_Data_Reassessment),
                         id, DateTime.Now.AddMonths(-nMonths).ToString("yyyy-MM-dd"),
                         DateTime.Now.ToString("yyyy-MM-dd"));

            List<DataReassessment> listReassessment = adodb.GetList<DataReassessment>(strSql);

            //_mssql.Close();
            return Utility.GetJson(listReassessment.Count > 0 ? 1 : 0,
                listReassessment.Count > 0 ? "成功" : "",
                listReassessment);
        }

        /// <summary>
        /// 人工复估
        /// </summary>
        /// <param name="id">复估ID</param>
        /// <param name="price">价格</param>
        /// <param name="oper">操作人</param>
        /// <returns></returns>
        public string UpdateReassessment(int id, int price, int oper)
        {
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            //获取已有复估值
            DataReassessment dataReassessment =
                CAS.DataAccess.DA.BaseDA.ExecuteToEntityByPrimaryKey<DataReassessment>(id);

            //储存历史复估值
            DataReassessmentHistory dataReaHistroy = new DataReassessmentHistory()
            {
                ReassessmentID = dataReassessment.ID,
                Price = dataReassessment.Price,
                ArrivedLoanRates = dataReassessment.ArrivedLoanRates,
                IsFirst = dataReassessment.IsFirst,
                CalculationMode = dataReassessment.CalculationMode,
                CreateDate = DateTime.Now,
                Operator = oper
            };
            //CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataReassessmentHistory>(dataReaHistroy);
            bool flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<DataReassessmentHistory>(dataReaHistroy) > 0;
            if (flag)
            {
                dataReassessment.Price = price;
                dataReassessment.IsFirst = 0;
                dataReassessment.CreateDate = DateTime.Now;
                dataReassessment.CalculationMode = "人工填写";
                //人工信息存储
                flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<DataReassessment>(dataReassessment) > 0;
            }

            return Utility.GetJson(flag ? 1 : 0, flag ? "成功" : "");
        }

        /// <summary>
        /// 复估风险分析
        /// </summary>
        /// <returns></returns>
        public string GetRiskAnalysis(int pId, int cId, int aId, string cityarrid, string itemarrid)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid))
            {
                return Utility.GetJson(0, "获取失败");
            }

            System.Text.RegularExpressions.Regex regxs = new System.Text.RegularExpressions.Regex("[\u4e00-\u9fa5]");
            string[,] array = { {"危险", "(1.2,+∞)", "(1.0,1.2]" }, 
                              { "风险","(0.9,10]", "(0.8,0.9]" },
                              { "正常","(0.7,0.8]", "(0.6,0.7]" },
                              { "安全","(0.5,0.6]", "(0,0.5]" } };
            int i = 1, j = 1;
            List<JObject> list = new List<JObject>();
            JObject jobject = null;
            string sql = @"select {0} from dbo.Data_Reassessment as a,Data_Collateral as b with(nolock) where a.CollateralId=b.Id",
                   excuteSql = string.Empty,
                   sWhere = string.Empty;
            decimal _twoCount = 0, _fourCount = 0, _sixCount = 0, _eightCount = 0, _tenCount = 0,
                _twoTempCount = 0, _fourTempCount = 0, _sixTempCount = 0, _eightTempCount = 0,
                _tenTempCount = 0;
            object excuteResult = null;
            StringBuilder cpWhere = new StringBuilder();
            if (!Utils.IsNullOrEmpty(cityarrid))
            {
                cpWhere.AppendFormat(" cityId in ({0}) ", cityarrid);
            }
            if (!itemarrid.ToLower().Equals("null") && !itemarrid.Equals(""))
            {
                cpWhere.AppendFormat(" BankProjectId in ({0}) ", itemarrid);
            }

            MSSQLADODAL _mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            foreach (var item in array)
            {
                if (i <= 3)
                {
                    if (!regxs.IsMatch(item))
                    {
                        string[] split = item.TrimStart('(').TrimEnd(']').TrimEnd(')').Split(',');
                        if (split[0].Equals("1.2") && split[1].Equals("+∞"))
                        {
                            sWhere = string.Format("ArrivedLoanRates>{0} and {1}", split[0], cpWhere.ToString());
                        }
                        else if (split[0].Equals("0") && split[1].Equals("0.5"))
                        {
                            sWhere = string.Format("ArrivedLoanRates<={0} and {1}", split[1], cpWhere.ToString());
                        }
                        else
                        {
                            sWhere = string.Format("ArrivedLoanRates>{0} and ArrivedLoanRates<={1} and {2}", split[0], split[1], cpWhere.ToString());
                        }
                        //业务量
                        excuteSql = string.Format("{0} and {1}", string.Format(sql, "count(a.ID)"), sWhere);
                        excuteResult = _mssql.GetUniqueResult(excuteSql);
                        decimal two = !Utils.IsNullOrEmpty(excuteResult.ToString()) ? decimal.Parse(excuteResult.ToString()) : 0;
                        _twoCount += two;
                        _twoTempCount += two;

                        //原贷款额度(贷款额)
                        excuteSql = string.Format("{0} and {1}", string.Format(sql, "avg(b.LoanAmount)"), sWhere);
                        excuteResult = _mssql.GetUniqueResult(excuteSql);
                        decimal four = !Utils.IsNullOrEmpty(excuteResult.ToString()) ? decimal.Parse(excuteResult.ToString()) : 0;
                        _fourCount += four;
                        _fourTempCount += four;

                        //贷款余额
                        excuteSql = string.Format("{0} and {1}", string.Format(sql, "avg(b.LoanBalance)"), sWhere);
                        excuteResult = _mssql.GetUniqueResult(excuteSql);
                        decimal six = !Utils.IsNullOrEmpty(excuteResult.ToString()) ? decimal.Parse(excuteResult.ToString()) : 0;
                        _sixCount += six;
                        _sixTempCount += six;

                        //原估价值
                        excuteSql = string.Format("{0} and {1}", string.Format(sql, "avg(b.OldRate)"), sWhere);
                        excuteResult = _mssql.GetUniqueResult(excuteSql);
                        decimal eight = !Utils.IsNullOrEmpty(excuteResult.ToString()) ? decimal.Parse(excuteResult.ToString()) : 0;
                        _eightCount += eight;
                        _eightTempCount += eight;

                        //现估价值
                        excuteSql = string.Format("{0} and {1}", string.Format(sql, "avg(a.Price)"), sWhere);
                        excuteResult = _mssql.GetUniqueResult(excuteSql);
                        decimal ten = !Utils.IsNullOrEmpty(excuteResult.ToString()) ? decimal.Parse(excuteResult.ToString()) : 0;
                        _tenCount += ten;
                        _tenTempCount += ten;

                        var json = new
                        {
                            one = item,
                            two = two,
                            three = 0,
                            four = four,
                            five = 0,
                            six = six,
                            seven = 0,
                            eight = eight,
                            nine = 0,
                            ten = ten,
                            eleven = 0
                        };
                        jobject.Add(string.Format("v{0}", i - 1), JsonConvert.SerializeObject(json));
                        if (i == 3)
                        {
                            json = new
                            {
                                one = "小计",
                                two = _twoCount,
                                three = 0,
                                four = _fourCount,
                                five = 0,
                                six = _sixCount,
                                seven = 0,
                                eight = _eightCount,
                                nine = 0,
                                ten = _tenCount,
                                eleven = 0
                            };
                            jobject.Add(string.Format("v{0}", i), JsonConvert.SerializeObject(json));
                            if (j == array.Length)
                                list.Add(jobject);
                            //重置
                            _twoTempCount = 0; _fourTempCount = 0; _sixTempCount = 0;
                            _eightTempCount = 0; _tenTempCount = 0; i = 0;
                        }
                    }
                    else
                    {
                        if (jobject != null)
                            list.Add(jobject);
                        jobject = new JObject();
                        jobject.Add("title", item);
                    }
                }
                i++; j++;
            }

            foreach (var item in list)
            {
                JObject j1 = JObject.Parse(item["v1"].ToString());
                JObject j2 = JObject.Parse(item["v3"].ToString());
                Operational(j1, _twoCount, _fourCount, _sixCount, _eightCount, _tenCount, j2);
                item["v1"] = j1;
                j1 = JObject.Parse(item["v2"].ToString());
                Operational(j1, _twoCount, _fourCount, _sixCount, _eightCount, _tenCount, j2);
                item["v2"] = j1;
                item["v3"] = j2;
            }

            if (list.Count > 0)
                return Utility.GetJson(1, "成功", list);
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 计算复估风险分析中各个维度占比值
        /// </summary>
        void Operational(JObject jobject, decimal v1, decimal v2, decimal v3, decimal v4, decimal v5, JObject jobject2)
        {
            foreach (var i1 in jobject)
            {
                if (i1.Key.Equals("two") && !v1.Equals(0M))
                {
                    jobject["three"] = decimal.Round(decimal.Parse(i1.Value.ToString()) / v1, 4) * 100;
                    jobject2["three"] = decimal.Parse(jobject2["three"].ToString()) +
                        decimal.Parse(jobject["three"].ToString());
                }
                else if (i1.Key.Equals("four") && !v2.Equals(0M))
                {
                    jobject["five"] = decimal.Round(decimal.Parse(i1.Value.ToString()) / v2, 4) * 100;
                    jobject2["five"] = decimal.Parse(jobject2["five"].ToString()) +
                        decimal.Parse(jobject["five"].ToString());
                }
                else if (i1.Key.Equals("six") && !v3.Equals(0M))
                {
                    jobject["seven"] = decimal.Round(decimal.Parse(i1.Value.ToString()) / v3, 4) * 100;
                    jobject2["seven"] = decimal.Parse(jobject2["seven"].ToString()) +
                        decimal.Parse(jobject["seven"].ToString());
                }
                else if (i1.Key.Equals("eight") && !v4.Equals(0M))
                {
                    jobject["nine"] = decimal.Round(decimal.Parse(i1.Value.ToString()) / v4, 4) * 100;
                    jobject2["nine"] = decimal.Parse(jobject2["nine"].ToString()) +
                        decimal.Parse(jobject["nine"].ToString());
                }
                else if (i1.Key.Equals("ten") && !v5.Equals(0M))
                {
                    jobject["eleven"] = decimal.Round(decimal.Parse(i1.Value.ToString()) / v5, 4) * 100;
                    jobject2["eleven"] = decimal.Parse(jobject2["eleven"].ToString()) +
                        decimal.Parse(jobject["eleven"].ToString());
                }
            }
        }

        /// <summary>
        /// 押品复估明细查询
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="projectid">楼盘</param>
        /// <param name="companyid">公司</param>
        /// <returns></returns>
        public string ReassessmentDetails(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age,
            int projectid, int companyid, int pageSize, int pageIndex, string cityarrid, string itemarrid)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid))
            {
                return Utility.GetJson(0, "获取失败");
            }
            MSSQLADODAL _mssql = new MSSQLADODAL();
            StringBuilder sbSql = new StringBuilder();

            UtilityPager pager = new UtilityPager();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;

            List<ReassessmentCollateral> list = null;
            StringBuilder sbLoanSql = new StringBuilder();
            StringBuilder sbOneSql = new StringBuilder();
            StringBuilder sbTwoSql = new StringBuilder();
            StringBuilder sbThreeSql = new StringBuilder();

            string strTempSql = string.Empty;
            #region 条件
            //物业类型
            if (!Utils.IsNullOrEmpty(houseType))
            {
                sbLoanSql.AppendFormat("PurposeCode in ({0})", houseType);
            }
            //楼盘
            if (!projectid.Equals(0))
            {
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ProjectId={0} ", projectid);
                else
                    sbLoanSql.AppendFormat(" ProjectId={0} ", projectid);
            }
            //开发商
            if (!companyid.Equals(0))
            {
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    sbLoanSql.Append(" and ProjectId in (select distinct ProjectId from [FXTProject].[dbo].LNK_P_Company where ");
                    sbLoanSql.AppendFormat("  CompanyType=2001001 and cityid={0} and CompanyId={1}) ", cId, companyid);
                }
                else
                {
                    sbLoanSql.Append(" ProjectId in (select distinct ProjectId from [FXTProject].[dbo].LNK_P_Company where ");
                    sbLoanSql.AppendFormat("  CompanyType=2001001 and cityid={0} and CompanyId={0}) ", cId, companyid);
                }
            }
            //贷款额度
            if (!Utils.IsNullOrEmpty(loanAmount))
            {
                string[] loanAmountArray = loanAmount.Split(',');
                strTempSql = Utility.GetArrayWhere(loanAmountArray, "LoanAmount");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //押品面积
            if (!Utils.IsNullOrEmpty(buildingArea))
            {
                string[] buildingAreaArray = buildingArea.Split(',');
                strTempSql = Utility.GetArrayWhere(buildingAreaArray, "BuildingArea");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //年龄
            if (!Utils.IsNullOrEmpty(age))
            {
                string[] ageArray = age.Split(',');
                strTempSql = Utility.GetArrayWhere(ageArray, "LoanAge");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            if (!Utils.IsNullOrEmpty(buildingType) || !Utils.IsNullOrEmpty(buildingDate))
            {
                //BuildingDate
                StringBuilder sbOneChSql = new StringBuilder();
                //BuildDate
                StringBuilder sbTwoChSql = new StringBuilder();
                SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
                string strOneTempSql = string.Empty;
                //建筑类型
                if (!Utils.IsNullOrEmpty(buildingType))
                {
                    sbOneChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                    sbTwoChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                }
                //年代
                if (!Utils.IsNullOrEmpty(buildingDate))
                {
                    string[] buildingDateArray = buildingDate.Split(',');
                    strTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildingDate,120))");
                    strOneTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildDate,120))");
                    if (!Utils.IsNullOrEmpty(sbOneChSql.ToString()))
                    {
                        sbOneChSql.AppendFormat(" and ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" and ({0})", strOneTempSql);
                    }
                    else
                    {
                        sbOneChSql.AppendFormat(" ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" ({0})", strOneTempSql);
                    }
                }
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    sbOneSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ",

cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.CaseTable, sbOneChSql.ToString());
                }
                else
                {
                    sbOneSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ",

cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",

cityTable.CaseTable, sbOneChSql.ToString());
                }
            }
            #endregion
            MSSQLADODAL _mssqlLoan = new MSSQLADODAL(Utility.DBFxtLoan);

            string strSql = string.Empty, strSqlExcute = string.Empty
                , sqlColumn = @"select c.Id,c.Branch,c.PurposeName,c.PurposeCode,c.Name,c.BuildingArea,c.LoanAmount,c.LoanBalance,c.OldMortgageRates,c.OldRate,
 max(r.Months) as Months,r.Price,r.ArrivedLoanRates,r.CalculationMode,c.LoanDate
from {0} as c with(nolock) inner join {1} as r with(nolock)  on c.Id=r.CollateralId where ",
                   sqlGroupBy = @"group by c.Id,c.Branch,c.PurposeName,c.PurposeCode,c.Name,c.BuildingArea,c.LoanAmount,c.LoanBalance,c.OldMortgageRates,c.OldRate,
 Months,r.Price,r.ArrivedLoanRates,r.CalculationMode,c.LoanDate",
                   sqlWhere = string.Empty,
                   strSqlExcuteWhere = string.Empty;
            #region 另外条件
            StringBuilder sbCityOrProject = new StringBuilder();
            if (!Utils.IsNullOrEmpty(cityarrid.ToString()))
            {
                sbCityOrProject.AppendFormat(" and c.CityId in ({0}) ", cityarrid);
            }
            if (!Utils.IsNullOrEmpty(itemarrid.ToString()))
            {
                sbCityOrProject.AppendFormat(" and c.BankProjectId in ({0}) ", itemarrid);
            }
            if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))//是否有条件
            {
                strSql = string.Format("{0} {1} and Status=1 {2} ",
                    string.Format(sqlColumn, Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment),
                    sbLoanSql.ToString(), sbCityOrProject.ToString());
                sqlWhere = string.Format("{0} as c where {1} and Status=1 {2}",
                    Utility.loan_Data_Collateral, sbLoanSql.ToString(),
                    sbCityOrProject.ToString());
            }
            else
            {
                strSql = string.Format("{0} Status=1 {1} ",
                    string.Format(sqlColumn, Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment),
                    sbCityOrProject.ToString());

                sqlWhere = string.Format("{0} as c where Status=1 {1}", Utility.loan_Data_Collateral,
                    sbCityOrProject.ToString());

            }

            if (!Utils.IsNullOrEmpty(sbOneSql.ToString()) &&
                !Utils.IsNullOrEmpty(sbTwoSql.ToString()) &&
                !Utils.IsNullOrEmpty(sbThreeSql.ToString()))
            {
                //默认楼栋
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    strSqlExcute = string.Format(" {0} {1} {2}", strSql, sbOneSql.ToString(), sqlGroupBy);
                    strSqlExcuteWhere = string.Format("{0} {1}", sqlWhere, sbOneSql.ToString());
                }
                else
                {
                    strSqlExcute = string.Format(" {0} and {1} {2}", strSql, sbOneSql.ToString(), sqlGroupBy);
                    strSqlExcuteWhere = string.Format("{0} and {1}", sqlWhere, sbOneSql.ToString());
                }
                if (pager.PageSize.Equals(0))
                    list = _mssqlLoan.GetList<ReassessmentCollateral>(strSqlExcute);
                else
                    list = _mssqlLoan.GetList<ReassessmentCollateral>(strSqlExcute, pager, strSqlExcuteWhere);

                //如果楼栋是没有找到,就用楼盘
                if (list.Count == 0)
                {
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1} {2}", strSql, sbTwoSql.ToString(), sqlGroupBy);
                        strSqlExcuteWhere = string.Format("{0} {1}", sqlWhere, sbTwoSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1} {2}", strSql, sbTwoSql.ToString(), sqlGroupBy);
                        strSqlExcuteWhere = string.Format("{0} and {1}", sqlWhere, sbTwoSql.ToString());
                    }
                    if (pager.PageSize.Equals(0))
                        list = _mssqlLoan.GetList<ReassessmentCollateral>(strSqlExcute);
                    else
                        list = _mssqlLoan.GetList<ReassessmentCollateral>(strSqlExcute, pager, strSqlExcuteWhere);
                }
                //如果楼盘是没有找到,就用案例
                if (list.Count == 0)
                {
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1} {2}", strSql, sbThreeSql.ToString(), sqlGroupBy);
                        strSqlExcuteWhere = string.Format("{0} {1}", sqlWhere, sbThreeSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1} {2}", strSql, sbThreeSql.ToString(), sqlGroupBy);
                        strSqlExcuteWhere = string.Format("{0} and {1}", sqlWhere, sbThreeSql.ToString());
                    }
                    if (pager.PageSize.Equals(0))
                        list = _mssqlLoan.GetList<ReassessmentCollateral>(strSqlExcute);
                    else
                        list = _mssqlLoan.GetList<ReassessmentCollateral>(strSqlExcute, pager, strSqlExcuteWhere);
                }
            }
            else
            {
                strSql = string.Format("{0} {1}", strSql, sqlGroupBy);
                if (pager.PageSize.Equals(0))
                    list = _mssqlLoan.GetList<ReassessmentCollateral>(strSql);
                else
                {
                    list = _mssqlLoan.GetList<ReassessmentCollateral>(strSql, pager, sqlWhere);
                }
            }
            #endregion
            _mssql = new MSSQLADODAL();
            for (int i = 0; i < list.Count; i++)
            {
                var code = UtilityDALHelper.GetADOSYSCodeByCode(_mssql, list[i].PurposeCode);
                list[i].PurposeCodeName = code != null ? code.CodeName : "";
                var city = UtilityDALHelper.GetADOCityById(_mssql, list[i].CityId);
                list[i].CityName = city != null ? city.CityName : "";
                var area = UtilityDALHelper.GetADOSYSAreaById(_mssql, list[i].AreaId);
                list[i].AreaName = area != null ? area.AreaName : "";
            }
            if (list.Count > 0)
                return Utility.GetJson(1, "获取成功", list, pager.Count);
            else
                return Utility.GetJson(0, "获取失败");
        }
        /// <summary>
        /// 替换复估详细查询中的相关参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string GetReplaceReassessmentDetailsWhere(string str)
        {
            return str
                .Replace("~", "-")
                .Replace("&&<", "以下")
                .Replace("&&=", "")
                .Replace("&&==", "")
                .Replace("&&>", "以上");
        }
        #endregion

        #region 压力测试

        /// <summary>
        /// 押品价格走势分析
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="type">走势分析类型</param>
        /// <param name="ptwhere">走势分析类型条件</param>
        /// <param name="cityarrid">城市Id</param>
        /// <param name="itemarrid">项目Id</param>
        /// <returns></returns>
        public string CollateralPriceTrend(int pId, int cId, int aId, int type, string ptwhere, string itemarrid)
        {
            string sql = string.Empty, sqlYesterday = string.Empty,
                sqlPriceMonth = Utility.GetMSSQL_SQL(typeof(DATAvgPriceMonth), Utility.DATAvgPriceMonth),
                excutePriceMonth = string.Empty;
            DateTime currentDT;
            List<JObject> listObject = new List<JObject>();
            StringBuilder sbProject = new StringBuilder();
            if (!Utils.IsNullOrEmpty(itemarrid.ToString()))
            {
                sbProject.AppendFormat(" and BankProjectId in ({0}) ", itemarrid);
            }
            int i = 11, month = 0;
            while (i >= 0)
            {
                JObject obj = new JObject();
                month = -i;//-(12 - i);
                currentDT = DateTime.Now.AddMonths(month);
                //标题
                obj.Add("title", currentDT);
                //总量价格分析
                if (type.Equals(0))
                {
                    sql = string.Format(@"select (select top 1 price from {1} with(nolock) where CollateralID=c.Id and Months='{3}'  order by CreateDate desc) as Price
from {0} as c with(nolock) where  status=1 and cityid={2} {4}", Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment,
                                              cId, currentDT.ToString("yyyyMM"), sbProject.ToString());

                    sqlYesterday = string.Format(@"select (select top 1 price from {1} with(nolock) where CollateralID=c.Id and Months='{3}'  order by CreateDate desc) as Price
from {0} as c with(nolock) where  status=1 and cityid={2} {4}", Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment,
                                              cId, currentDT.AddMonths(-1).ToString("yyyyMM"), sbProject.ToString());

                    //本月市场均价
                    MSSQLADODAL _mssqlado = new MSSQLADODAL();
                    excutePriceMonth = string.Format("{0} cityid={1} and areaid={2} and projectid={3} and AvgPriceDate='{4}'",
                        sqlPriceMonth, cId, 0, 0, Convert.ToDateTime(currentDT.ToString("yyyy-MM-dd")));
                    DATAvgPriceMonth dpmToday = _mssqlado.GetModel<DATAvgPriceMonth>(excutePriceMonth);
                    decimal mToday = dpmToday != null ? dpmToday.AvgPrice : 0;
                    obj.Add("marketavg", decimal.Round(mToday, 2));

                    //上月市场均价                    
                    excutePriceMonth = string.Format("{0} cityid={1} and areaid={2} and projectid={3} and AvgPriceDate='{4}'",
                        sqlPriceMonth, cId, 0, 0, Convert.ToDateTime(currentDT.AddMonths(-1).ToString("yyyy-MM-dd")));
                    DATAvgPriceMonth dpmYesterday = _mssqlado.GetModel<DATAvgPriceMonth>(excutePriceMonth);
                    decimal mYesterday = dpmYesterday != null ? dpmYesterday.AvgPrice : 0;
                    //市场均价涨跌幅
                    mYesterday = decimal.Round(!mToday.Equals(0) ? mYesterday / mToday : 0, 2);
                    obj.Add("marketpricechange", mYesterday);

                }
                else if (type.Equals(1))//行政区价格分析
                {
                    sql = string.Format(@"select (select  top 1 price from {1} with(nolock) where CollateralID=c.Id and Months='{3}'  order by CreateDate desc) as Price
from {0} as c with(nolock) where  status=1 and areaid={2} ", Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment,
                                              aId, currentDT.ToString("yyyyMM"));

                    sqlYesterday = string.Format(@"select (select  top 1 price from {1} with(nolock) where CollateralID=c.Id and Months='{3}'  order by CreateDate desc) as Price
from {0} as c with(nolock) where  status=1 and areaid={2} ", Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment,
                                              aId, currentDT.AddMonths(-1).ToString("yyyyMM"));
                    //本月市场均价
                    MSSQLADODAL _mssqlado = new MSSQLADODAL();
                    //DATAvgPriceMonth dpmToday = _mssql.GetCustom<DATAvgPriceMonth>(
                    //    (Expression<Func<DATAvgPriceMonth, bool>>)
                    //    (item => item.CityId == 0 && item.AreaId == aId && item.ProjectId == 0 &&
                    //    item.AvgPriceDate == Convert.ToDateTime(currentDT.ToString("yyyy-MM-dd"))));

                    excutePriceMonth = string.Format("{0} cityid={1} and areaid={2} and projectid={3} and AvgPriceDate='{4}'",
                        sqlPriceMonth, 0, aId, 0, Convert.ToDateTime(currentDT.ToString("yyyy-MM-dd")));
                    DATAvgPriceMonth dpmToday = _mssqlado.GetModel<DATAvgPriceMonth>(excutePriceMonth);
                    decimal mToday = dpmToday != null ? dpmToday.AvgPrice : 0;
                    obj.Add("marketavg", decimal.Round(mToday, 2));
                    //上月市场均价
                    //DATAvgPriceMonth dpmYesterday = _mssql.GetCustom<DATAvgPriceMonth>(
                    //    (Expression<Func<DATAvgPriceMonth, bool>>)
                    //    (item => item.CityId == 0 && item.AreaId == aId && item.ProjectId == 0 &&
                    //    item.AvgPriceDate == Convert.ToDateTime(currentDT.AddMonths(-1).ToString("yyyy-MM-dd"))));

                    excutePriceMonth = string.Format("{0} cityid={1} and areaid={2} and projectid={3} and AvgPriceDate='{4}'",
                        sqlPriceMonth, 0, aId, 0, Convert.ToDateTime(currentDT.AddMonths(-1).ToString("yyyy-MM-dd")));
                    DATAvgPriceMonth dpmYesterday = _mssqlado.GetModel<DATAvgPriceMonth>(excutePriceMonth);
                    decimal mYesterday = dpmYesterday != null ? dpmYesterday.AvgPrice : 0;
                    //市场均价涨跌幅
                    mYesterday = decimal.Round(!mToday.Equals(0) ? mYesterday / mToday : 0, 2);
                    obj.Add("marketpricechange", mYesterday);

                }
                else if (type.Equals(2))//面积段价格分析  暂时省略
                {
                    sql = string.Format(@"select (select price from {1} with(nolock) where CollateralID=c.Id and Months='{3}') as Price
from {0} as c with(nolock) where  status=1 and buildingareacode={2} ", Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment,
                                              cId, ptwhere);

                    sqlYesterday = string.Format(@"select (select price from {1} with(nolock) where CollateralID=c.Id and Months='{3}') as Price
from {0} as c with(nolock) where  status=1 and buildingareacode={2} ", Utility.loan_Data_Collateral, Utility.loan_Data_Data_Reassessment,
                                              cId, ptwhere);
                    //本月市场均价
                    MSSQLADODAL _mssqlado = new MSSQLADODAL();
                    //DATAvgPriceMonth dpmToday = _mssql.GetCustom<DATAvgPriceMonth>(
                    //    (Expression<Func<DATAvgPriceMonth, bool>>)
                    //    (item => item.CityId == 0 && item.AreaId == aId && item.ProjectId == 0 &&
                    //    item.AvgPriceDate == Convert.ToDateTime(currentDT.ToString("yyyy-MM-dd"))));

                    excutePriceMonth = string.Format("{0} cityid={1} and areaid={2} and projectid={3} and AvgPriceDate='{4}'",
                        sqlPriceMonth, 0, aId, 0, Convert.ToDateTime(currentDT.ToString("yyyy-MM-dd")));
                    DATAvgPriceMonth dpmToday = _mssqlado.GetModel<DATAvgPriceMonth>(excutePriceMonth);
                    decimal mToday = dpmToday != null ? dpmToday.AvgPrice : 0;
                    obj.Add("marketavg", decimal.Round(mToday, 2));
                    //上月市场均价
                    //DATAvgPriceMonth dpmYesterday = _mssql.GetCustom<DATAvgPriceMonth>(
                    //    (Expression<Func<DATAvgPriceMonth, bool>>)
                    //    (item => item.CityId == 0 && item.AreaId == aId && item.ProjectId == 0 &&
                    //    item.AvgPriceDate == Convert.ToDateTime(currentDT.AddMonths(-1).ToString("yyyy-MM-dd"))));

                    excutePriceMonth = string.Format("{0} cityid={1} and areaid={2} and projectid={3} and AvgPriceDate='{4}'",
                        sqlPriceMonth, 0, aId, 0, Convert.ToDateTime(currentDT.AddMonths(-1).ToString("yyyy-MM-dd")));
                    DATAvgPriceMonth dpmYesterday = _mssqlado.GetModel<DATAvgPriceMonth>(excutePriceMonth);
                    decimal mYesterday = dpmYesterday != null ? dpmYesterday.AvgPrice : 0;
                    //市场均价涨跌幅
                    mYesterday = decimal.Round(!mToday.Equals(0) ? mYesterday / mToday : 0, 2);
                    obj.Add("marketpricechange", mYesterday);

                }
                GetPriceTrendJobject(sql, sqlYesterday, obj);
                i--;
                listObject.Add(obj);
            }
            if (listObject.Count > 0)
            {
                return Utility.GetJson(1, "成功", listObject);
            }
            return Utility.GetJson(0, "");
        }
        /// <summary>
        /// 押品均价
        /// </summary>
        /// <param name="sql0">本月sql</param>
        /// <param name="sql1">上月sql</param>
        /// <param name="obj"></param>
        void GetPriceTrendJobject(string sql0, string sql1, JObject obj)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            IList listToday = _mssql.GetListObject(sql0);
            decimal countToday = 0, TodayDivide = 0, countYesterday = 0, YeserdayDivide = 0;

            //本月押品均价
            for (int j = 0; j < listToday.Count; j++)
                countToday += !Utils.IsNullOrEmpty(listToday[j].ToString()) ? decimal.Parse(listToday[j].ToString()) : 0;
            TodayDivide = !listToday.Count.Equals(0) ? countToday / listToday.Count : 0;
            obj.Add("collavg", decimal.Round(TodayDivide, 2));
            IList listYesterday = _mssql.GetListObject(sql1);

            //上月押品均价
            for (int j = 0; j < listYesterday.Count; j++)
                countYesterday += !Utils.IsNullOrEmpty(listYesterday[j].ToString()) ? decimal.Parse(listYesterday[j].ToString()) : 0;
            YeserdayDivide = !listYesterday.Count.Equals(0) ? countYesterday / listYesterday.Count : 0;

            //押品均价涨跌幅
            YeserdayDivide = decimal.Round(!TodayDivide.Equals(0) ? YeserdayDivide / TodayDivide : 0, 2);
            obj.Add("collpricechange", YeserdayDivide);
        }

        /// <summary>
        /// 压力测试
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="houseType">物业类型(押品)</param>
        /// <param name="buildingType">建筑类型(Fxt库)</param>
        /// <param name="buildingDate">建筑年代(Fxt库)</param>
        /// <param name="loanAmount">贷款额度(押品)</param>
        /// <param name="buildingArea">押品面积(押品)</param>
        /// <param name="age">年龄(押品)</param>
        /// <param name="twhere">测试条件</param>
        /// <returns></returns>
        public string StressTest(int pId, int cId, int aId,
            string houseType, string buildingType, string buildingDate,
            string loanAmount, string buildingArea, string age,
            string start, string end, string twhere, string cityarrid, string itemarrid)
        {
            if (CityOrPeojectIsNull(cityarrid, itemarrid))
            {
                return Utility.GetJson(0, "获取失败");
            }

            MSSQLADODAL _mssql = new MSSQLADODAL();

            IList<DataCollateral> listColl = null;
            StringBuilder sbLoanSql = new StringBuilder();
            StringBuilder sbOneSql = new StringBuilder();
            StringBuilder sbTwoSql = new StringBuilder();
            StringBuilder sbThreeSql = new StringBuilder();

            string strTempSql = string.Empty;

            #region 条件
            //物业类型
            if (!Utils.IsNullOrEmpty(houseType))
            {
                sbLoanSql.AppendFormat("PurposeCode in ({0})", houseType);
            }
            //贷款时间
            if (!Utils.IsNullOrEmpty(start) && !Utils.IsNullOrEmpty(end))
            {
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and (LoanDate between {0} and {1}) ", start, end);
                else
                    sbLoanSql.AppendFormat(" (LoanDate between {0} and {1}) ", start, end);
            }
            //贷款额度
            if (!Utils.IsNullOrEmpty(loanAmount))
            {
                string[] loanAmountArray = loanAmount.Split(',');
                strTempSql = Utility.GetArrayWhere(loanAmountArray, "LoanAmount");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //押品面积
            if (!Utils.IsNullOrEmpty(buildingArea))
            {
                string[] buildingAreaArray = buildingArea.Split(',');
                strTempSql = Utility.GetArrayWhere(buildingAreaArray, "BuildingArea");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            //年龄
            if (!Utils.IsNullOrEmpty(age))
            {
                string[] ageArray = age.Split(',');
                strTempSql = Utility.GetArrayWhere(ageArray, "LoanAge");
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    sbLoanSql.AppendFormat(" and ({0})", strTempSql);
                else
                    sbLoanSql.AppendFormat(" ({0})", strTempSql);
            }
            if (!Utils.IsNullOrEmpty(buildingType) || !Utils.IsNullOrEmpty(buildingDate))
            {
                //BuildingDate
                StringBuilder sbOneChSql = new StringBuilder();
                //BuildDate
                StringBuilder sbTwoChSql = new StringBuilder();
                SYSCityTable cityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);
                string strOneTempSql = string.Empty;
                //建筑类型
                if (!Utils.IsNullOrEmpty(buildingType))
                {
                    sbOneChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                    sbTwoChSql.AppendFormat(" BuildingTypeCode in ({0})", buildingType);
                }
                //年代
                if (!Utils.IsNullOrEmpty(buildingDate))
                {
                    string[] buildingDateArray = buildingDate.Split(',');
                    strTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildingDate,120))");
                    strOneTempSql = Utility.GetArrayWhere(buildingDateArray, "convert(int,convert(nvarchar(4),BuildDate,120))");
                    if (!Utils.IsNullOrEmpty(sbOneChSql.ToString()))
                    {
                        sbOneChSql.AppendFormat(" and ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" and ({0})", strOneTempSql);
                    }
                    else
                    {
                        sbOneChSql.AppendFormat(" ({0})", strTempSql);
                        sbTwoChSql.AppendFormat(" ({0})", strOneTempSql);
                    }
                }
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    sbOneSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",
cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ",
cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" and ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",
cityTable.CaseTable, sbOneChSql.ToString());
                }
                else
                {
                    sbOneSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",
cityTable.BuildingTable, sbTwoChSql.ToString());
                    sbTwoSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1})  ",
cityTable.ProjectTable, sbOneChSql.ToString());
                    sbThreeSql.AppendFormat(" ProjectId in (select distinct ProjectId from FXTProject.{0} where {1}) ",
cityTable.CaseTable, sbOneChSql.ToString());
                }
            }
            #endregion

            MSSQLADODAL _mssqlLoan = new MSSQLADODAL(Utility.DBFxtLoan);

            string strSql = string.Empty, strSqlExcute = string.Empty;

            StringBuilder sbCityOrProject = new StringBuilder();
            if (!Utils.IsNullOrEmpty(cityarrid.ToString()))
            {
                sbCityOrProject.AppendFormat(" and CityId in ({0}) ", cityarrid);
            }
            if (!Utils.IsNullOrEmpty(itemarrid.ToString()))
            {
                sbCityOrProject.AppendFormat(" and BankProjectId in ({0}) ", itemarrid);
            }

            if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
            {
                strSql = string.Format("{0} {1} and Status=1 {2}",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                sbLoanSql.ToString(), sbCityOrProject.ToString());
            }
            else
            {
                strSql = string.Format("{0} Status=1 {1}",
                Utility.GetMSSQL_SQL(typeof(DataCollateral), Utility.loan_Data_Collateral),
                sbCityOrProject.ToString());
            }
            if (!Utils.IsNullOrEmpty(sbOneSql.ToString()) &&
                !Utils.IsNullOrEmpty(sbTwoSql.ToString()) &&
                !Utils.IsNullOrEmpty(sbThreeSql.ToString()))
            {
                //默认楼栋
                if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                {
                    strSqlExcute = string.Format(" {0} {1}", strSql, sbOneSql.ToString());
                }
                else
                {
                    strSqlExcute = string.Format(" {0} and {1}", strSql, sbOneSql.ToString());
                }
                listColl = _mssqlLoan.GetList<DataCollateral>(strSqlExcute);

                //如果楼栋是没有找到,就用楼盘
                if (listColl.Count() == 0)
                {
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1}", strSql, sbTwoSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1}", strSql, sbTwoSql.ToString());
                    }
                    listColl = _mssqlLoan.GetList<DataCollateral>(strSqlExcute);
                }
                //如果楼盘是没有找到,就用案例
                if (listColl.Count() == 0)
                {
                    if (!Utils.IsNullOrEmpty(sbLoanSql.ToString()))
                    {
                        strSqlExcute = string.Format(" {0} {1}", strSql, sbThreeSql.ToString());
                    }
                    else
                    {
                        strSqlExcute = string.Format(" {0} and {1}", strSql, sbThreeSql.ToString());
                    }
                    listColl = _mssqlLoan.GetList<DataCollateral>(strSqlExcute);
                }
            }
            else
            {
                listColl = _mssqlLoan.GetList<DataCollateral>(strSql);
            }
            string[] tarray = twhere.Split(',');
            StringBuilder sbCId = new StringBuilder();
            foreach (var item in listColl.Select(item => item.Id).ToArray())
            {
                sbCId.AppendFormat("{0},", item);
            }
            string strDataReassSql = string.Format("{0} CollateralId in({1}) and Months='{2}'",
                        Utility.GetMSSQL_SQL(typeof(DataReassessment), Utility.loan_Data_Data_Reassessment),
                        sbCId.ToString().TrimEnd(','), DateTime.Now.ToString("yyyyMM"));

            IList<DataReassessment> listReass = _mssqlLoan.GetList<DataReassessment>(strDataReassSql);
            List<JObject> list = new List<JObject>();
            JObject obj1 = new JObject();
            //风险
            var vv1 = listReass.Where(item => item.ArrivedLoanRates <= 1 && item.ArrivedLoanRates >= (decimal)0.8);
            //危险
            var vv2 = listReass.Where(item => item.ArrivedLoanRates > 1);
            obj1.Add("title", "正常");
            var _loanAmount = listColl.Where(item => vv1.Where(vitem => item.Id.Equals(item.Id)).Any());
            var valueRisk = new
            {
                title = "风险",
                count = vv1.Count(),
                LoanAmount = !vv1.Count().Equals(0) ? _loanAmount.Sum(item => item.LoanAmount) / vv1.Count() : 0,
                Price = !vv1.Count().Equals(0) ? vv1.Sum(item => item.Price) / vv1.Count() : 0,
                ArrivedLoanRates = !vv1.Count().Equals(0) ? vv1.Sum(item => item.ArrivedLoanRates) / vv1.Count() : 0
            };
            obj1.Add("v1", JObject.Parse(Utils.Serialize(valueRisk)));
            _loanAmount = listColl.Where(item => vv2.Where(vitem => item.Id.Equals(item.Id)).Any());
            valueRisk = new
            {
                title = "危险",
                count = vv2.Count(),
                LoanAmount = !vv2.Count().Equals(0) ? _loanAmount.Sum(item => item.LoanAmount) / vv2.Count() : 0,
                Price = !vv2.Count().Equals(0) ? vv2.Sum(item => item.Price) / vv2.Count() : 0,
                ArrivedLoanRates = !vv2.Count().Equals(0) ? vv2.Sum(item => item.ArrivedLoanRates) / vv2.Count() : 0
            };
            obj1.Add("v2", JObject.Parse(Utils.Serialize(valueRisk)));
            list.Add(obj1);
            foreach (var titem in tarray)
            {
                if (Utils.IsNullOrEmpty(titem)) break;
                obj1 = new JObject();
                if (titem.Equals("10"))
                {
                    //风险
                    vv1 = listReass.Where(item => item.TenArrivedLoanRates <= 1 && item.TenArrivedLoanRates >= (decimal)0.8);
                    //危险
                    vv2 = listReass.Where(item => item.TenArrivedLoanRates > 1);
                    obj1.Add("title", "下跌10%");
                    _loanAmount = listColl.Where(item => vv1.Where(vitem => item.Id.Equals(item.Id)).Any());
                    valueRisk = new
                    {
                        title = "风险",
                        count = vv1.Count(),
                        LoanAmount = !vv1.Count().Equals(0) ? _loanAmount.Sum(item => item.LoanAmount) / vv1.Count() : 0,
                        Price = !vv1.Count().Equals(0) ? vv1.Sum(item => item.TenPrice) / vv1.Count() : 0,
                        ArrivedLoanRates = !vv1.Count().Equals(0) ? vv1.Sum(item => item.TenArrivedLoanRates) / vv1.Count() : 0
                    };
                    obj1.Add("v1", JObject.Parse(Utils.Serialize(valueRisk)));
                    _loanAmount = listColl.Where(item => vv2.Where(vitem => item.Id.Equals(item.Id)).Any());
                    valueRisk = new
                    {
                        title = "危险",
                        count = vv2.Count(),
                        LoanAmount = !vv2.Count().Equals(0) ? _loanAmount.Sum(item => item.LoanAmount) / vv2.Count() : 0,
                        Price = !vv2.Count().Equals(0) ? vv2.Sum(item => item.TenPrice) / vv2.Count() : 0,
                        ArrivedLoanRates = !vv2.Count().Equals(0) ? vv2.Sum(item => item.TenArrivedLoanRates) / vv2.Count() : 0
                    };
                    obj1.Add("v2", JObject.Parse(Utils.Serialize(valueRisk)));
                }
                else if (titem.Equals("20"))
                {
                    //风险
                    vv1 = listReass.Where(item => item.TwentyArrivedLoanRates <= 1 && item.TwentyArrivedLoanRates >= (decimal)0.8);
                    //危险
                    vv2 = listReass.Where(item => item.TwentyArrivedLoanRates > 1);
                    obj1.Add("title", "下跌20%");
                    _loanAmount = listColl.Where(item => vv1.Where(vitem => item.Id.Equals(item.Id)).Any());
                    valueRisk = new
                    {
                        title = "风险",
                        count = vv1.Count(),
                        LoanAmount = !vv1.Count().Equals(0) ? _loanAmount.Sum(item => item.LoanAmount) / vv1.Count() : 0,
                        Price = !vv1.Count().Equals(0) ? vv1.Sum(item => item.TwentyPrice) / vv1.Count() : 0,
                        ArrivedLoanRates = !vv1.Count().Equals(0) ? vv1.Sum(item => item.TwentyArrivedLoanRates) / vv1.Count() : 0
                    };
                    obj1.Add("v1", JObject.Parse(Utils.Serialize(valueRisk)));
                    _loanAmount = listColl.Where(item => vv2.Where(vitem => item.Id.Equals(item.Id)).Any());
                    valueRisk = new
                    {
                        title = "危险",
                        count = vv2.Count(),
                        LoanAmount = !vv2.Count().Equals(0) ? _loanAmount.Sum(item => item.LoanAmount) / vv2.Count() : 0,
                        Price = !vv2.Count().Equals(0) ? vv2.Sum(item => item.TwentyPrice) / vv2.Count() : 0,
                        ArrivedLoanRates = !vv2.Count().Equals(0) ? vv2.Sum(item => item.TwentyArrivedLoanRates) / vv2.Count() : 0
                    };
                    obj1.Add("v2", JObject.Parse(Utils.Serialize(valueRisk)));
                }
                list.Add(obj1);
            }
            if (list.Count() > 0)
                return Utility.GetJson(1, "成功", list);
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 风险预警
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="type">统计类型</param>
        /// <returns></returns>
        public string RiskWarning(int pId, int cId, int aId, int type, string itemarrid)
        {
            MSSQLADODAL _mssql = new MSSQLADODAL();
            SYSCityTable sysCityTable = UtilityDALHelper.GetCityADOTable(_mssql, cId);

            string sql = @"select max(Months) as mMonths,r.Collateralid,r.{1},
c.LoanAmount,c.LoanBalance,c.BuildingArea,c.LoanInterestRate,
(select X from FXTProject.{2} with(nolock) where ProjectId=c.ProjectId) as X,
(select Y from FXTProject.{2} with(nolock) where ProjectId=c.ProjectId) as Y
 from Data_Reassessment as r with(nolock) inner join Data_Collateral as c with(nolock) on r.CollateralId=c.Id
 where {3}{0} group by r.Collateralid,r.{1},c.LoanAmount,
 c.LoanBalance,c.BuildingArea,c.LoanInterestRate,c.ProjectId",
executeSqlR = string.Empty, executeSqlE = string.Empty, excuteBPWhere = string.Empty;
            if (!Utils.IsNullOrEmpty(itemarrid) && !itemarrid.ToLower().Equals("null"))
            {
                excuteBPWhere = string.Format("BankProjectid in ({0}) and ", itemarrid);
            }
            MSSQLADODAL adomssql = new MSSQLADODAL(Utility.DBFxtLoan);
            if (type.Equals(0))//当前风险
            {
                //风险
                executeSqlR = string.Format(sql, string.Format("ArrivedLoanRates>=0.8 and  ArrivedLoanRates<1 "),
                    "ArrivedLoanRates", sysCityTable.ProjectTable, excuteBPWhere);
                //危险
                executeSqlE = string.Format(sql, string.Format("ArrivedLoanRates>1 "), "ArrivedLoanRates"
                    , sysCityTable.ProjectTable, excuteBPWhere);
            }
            else if (type.Equals(1))//下跌10%风险
            {
                //风险
                executeSqlR = string.Format(sql, string.Format("TenArrivedLoanRates>=0.8 and  TenArrivedLoanRates<1 "),
                    "TenArrivedLoanRates", sysCityTable.ProjectTable, excuteBPWhere);
                //危险
                executeSqlE = string.Format(sql, string.Format("TenArrivedLoanRates>1 "),
                    "TenArrivedLoanRates", sysCityTable.ProjectTable, excuteBPWhere);
            }
            else if (type.Equals(2))//下跌20%风险
            {
                //风险
                executeSqlR = string.Format(sql, string.Format("TwentyArrivedLoanRates>=0.8 and  TwentyArrivedLoanRates<1 "),
                    "TwentyArrivedLoanRates", sysCityTable.ProjectTable, excuteBPWhere);
                //危险
                executeSqlE = string.Format(sql, string.Format("TwentyArrivedLoanRates>1 "),
                    "TwentyArrivedLoanRates", sysCityTable.ProjectTable, excuteBPWhere);
            }
            IList list = adomssql.GetListObject(executeSqlR);
            var json = new
            {
                mMonths = "",
                Collateralid = "",
                ArrivedRates = "",
                LoanAmount = "",
                LoanBalance = "",
                BuildingArea = "",
                LoanInterestRate = "",
                X = "",
                Y = ""
            };
            JObject robject = new JObject();
            List<JObject> listJobject = new List<JObject>();
            foreach (var item in list)
            {
                //JObject obj = new JObject();
                object[] objs = item as object[];
                json = new
                {
                    mMonths = objs[0] != null ? objs[0].ToString() : "",
                    Collateralid = objs[1] != null ? objs[1].ToString() : "",
                    ArrivedRates = objs[2] != null ? objs[2].ToString() : "",
                    LoanAmount = objs[3] != null ? objs[3].ToString() : "",
                    LoanBalance = objs[4] != null ? objs[4].ToString() : "",
                    BuildingArea = objs[5] != null ? objs[5].ToString() : "",
                    LoanInterestRate = objs[6] != null ? objs[6].ToString() : "",
                    X = objs[7] != null ? objs[7].ToString() : "",
                    Y = objs[8] != null ? objs[8].ToString() : ""
                };
                //obj.Add(json);
                listJobject.Add(JObject.Parse(Utils.Serialize(json)));
            }
            robject.Add("v1", JArray.Parse(Utils.Serialize(listJobject)));
            list = adomssql.GetListObject(executeSqlE);
            listJobject = new List<JObject>();
            foreach (var item in list)
            {
                //JObject obj = new JObject();
                object[] objs = item as object[];
                json = new
                {
                    mMonths = objs[0] != null ? objs[0].ToString() : "",
                    Collateralid = objs[1] != null ? objs[1].ToString() : "",
                    ArrivedRates = objs[2] != null ? objs[2].ToString() : "",
                    LoanAmount = objs[3] != null ? objs[3].ToString() : "",
                    LoanBalance = objs[4] != null ? objs[4].ToString() : "",
                    BuildingArea = objs[5] != null ? objs[5].ToString() : "",
                    LoanInterestRate = objs[6] != null ? objs[6].ToString() : "",
                    X = objs[7] != null ? objs[7].ToString() : "",
                    Y = objs[8] != null ? objs[8].ToString() : ""
                };
                listJobject.Add(JObject.Parse(Utils.Serialize(json)));
            }
            robject.Add("v2", JArray.Parse(Utils.Serialize(listJobject)));
            if (robject.Count > 0)
                return Utility.GetJson(1, "成功", robject);
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 风险预警 获取危险值列表
        /// </summary>
        /// <param name="pId">省份</param>
        /// <param name="cId">城市</param>
        /// <param name="aId">行政区</param>
        /// <param name="itemarrid">项目列表ID</param>
        /// <returns></returns>
        public string RikWarningToDanger(int pId, int cId, int aId, int type, string itemarrid)
        {
            string sql = string.Empty, sqlProjectWhere = string.Empty;
            if (!itemarrid.Equals("") && !itemarrid.Equals("null"))
            {
                sqlProjectWhere = string.Format(" and BankProjectId in ({0})", itemarrid);
            }
            if (type.Equals(0))//正常
            {
                sql = string.Format(@"select distinct r.Collateralid,r.ArrivedLoanRates,c.LoanAmount,c.LoanBalance,c.OldRate,
 (select top 1 Price from Data_Reassessment with(nolock) where collateralid=c.Id order by CreateDate desc) as Rate
 from Data_Reassessment as r with(nolock) inner join Data_Collateral as c with(nolock) on r.CollateralId=c.Id
 where r.ArrivedLoanRates>1 and c.cityid={0} {1}", cId, sqlProjectWhere);
            }
            else if (type.Equals(1))//下跌10%
            {
                sql = string.Format(@"select distinct r.Collateralid,r.TenArrivedLoanRates,c.LoanAmount,c.LoanBalance,c.OldRate,
 (select top 1 Price from Data_Reassessment with(nolock) where collateralid=c.Id order by CreateDate desc) as Rate
 from Data_Reassessment as r with(nolock) inner join Data_Collateral as c with(nolock) on r.CollateralId=c.Id
 where r.TenArrivedLoanRates>1 and c.cityid={0} {1}", cId, sqlProjectWhere);
            }
            else if (type.Equals(2))//下跌20%
            {
                sql = string.Format(@"select distinct r.Collateralid,r.TwentyArrivedLoanRates,c.LoanAmount,c.LoanBalance,c.OldRate,
 (select top 1 Price from Data_Reassessment with(nolock) where collateralid=c.Id order by CreateDate desc) as Rate
 from Data_Reassessment as r with(nolock) inner join Data_Collateral as c with(nolock) on r.CollateralId=c.Id
 where r.TwentyArrivedLoanRates>1 and c.cityid={0} {1}", cId, sqlProjectWhere);
            }
            MSSQLADODAL mssql = new MSSQLADODAL(Utility.DBFxtLoan);
            List<object> list = mssql.GetListObject(sql);

            string[] array = new string[] { "(1.0,1.2]", "(1.2,+∞)" };
            List<JObject> listJobject = new List<JObject>();
            JObject pjobject = new JObject(); JObject jobject = null;
            int count = 0;
            decimal LoanAmount = 0, LoanBalance = 0, OldRate = 0, Rate = 0, OldAverageRates = 0, AverageRates = 0;
            pjobject.Add("title", "危险");
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                    list = list.Where(item => Convert.ToDecimal((item as object[])[1]) > 1.0M
                    && Convert.ToDecimal((item as object[])[1]) <= 1.2M).ToList();
                else
                    list = list.Where(item => Convert.ToDecimal((item as object[])[1]) > 1.2M).ToList();

                jobject = new JObject();
                jobject.Add("Title", array[i]);
                jobject.Add("Count", list.Count); count += list.Count;
                decimal _LoanAmount = list.Sum(item => Convert.ToDecimal(GetObjectArrayValue(item, 2)));
                jobject.Add("LoanAmount", _LoanAmount); LoanAmount += _LoanAmount;
                decimal _LoanBalance = list.Sum(item => Convert.ToDecimal(GetObjectArrayValue(item, 3)));
                jobject.Add("LoanBalance", _LoanBalance); LoanBalance += _LoanBalance;
                decimal _OldRate = list.Sum(item => Convert.ToDecimal(GetObjectArrayValue(item, 4)));
                jobject.Add("OldRate", _OldRate); OldRate += _OldRate;
                decimal _Rate = list.Sum(item => Convert.ToDecimal(GetObjectArrayValue(item, 5)));
                jobject.Add("Rate", _Rate); Rate += _Rate;
                decimal _OldAverageRates = !_OldRate.Equals(0M) ? _LoanAmount / _OldRate : 0;
                OldAverageRates += _OldAverageRates;
                jobject.Add("OldAverageRates", _OldAverageRates);
                decimal _AverageRates = !_Rate.Equals(0M) ? _LoanBalance / _Rate : 0;
                AverageRates += _AverageRates;
                jobject.Add("AverageRates", _AverageRates);

                pjobject.Add(string.Format("v{0}", i + 1), jobject);
            }
            jobject = new JObject();
            jobject.Add("Title", "小计");
            jobject.Add("Count", count);
            jobject.Add("LoanAmount", LoanAmount);
            jobject.Add("LoanBalance", LoanBalance);
            jobject.Add("OldRate", OldRate);
            jobject.Add("Rate", Rate);
            jobject.Add("OldAverageRates", OldAverageRates);
            jobject.Add("AverageRates", AverageRates);
            pjobject.Add(string.Format("v{0}", array.Length + 1), jobject);
            listJobject.Add(pjobject);
            if (listJobject.Count > 0)
                return Utility.GetJson(1, "Success", listJobject);
            return Utility.GetJson(0, "");
        }
        private string GetObjectArrayValue(object obj, int index)
        {
            object[] objarray = obj as object[];
            return Convert.IsDBNull(objarray[index]) ? "0" : objarray[index].ToString();
        }
        #endregion

        #region 任务

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="task">任务模型</param>
        /// <returns></returns>
        public string AddTask(string task)
        {
            SysTask sysTask = Utils.Deserialize<SysTask>(task);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int objId = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysTask>(sysTask);
            if (objId > 0)
            {
                return Utility.GetJson(1, "成功");
            }
            return Utility.GetJson(0, "");
        }

        #endregion

        #region 文件项目

        /// <summary>
        /// 新增修改 文件项目
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns></returns>
        public string AddEditProjects(string data)
        {
            SysBankProject sysBP = Utils.Deserialize<SysBankProject>(data);
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = 0;
            if (sysBP.Id <= 0)
            {
                flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysBankProject>(sysBP);
            }
            else if (sysBP.Valid == 1)
            {
                flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysBankProject>(sysBP);
            }
            else if (sysBP.Valid == 0)
            {
                sysBP.SetAvailableFields(new string[] { "valid" });
                flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysBankProject>(sysBP);
            }
            if (flag > 0)
                return Utility.GetJson(1, "Success");
            return Utility.GetJson(0, "");
        }



        /// <summary>
        /// 获取文件项目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="id">主键Id</param>
        /// <param name="orderProperty">排序</param>
        /// <returns></returns>
        public string GetSysBankProjectList(int pageIndex, int pageSize, int id
            , string orderProperty, string key, int bankid, int customerid, int customertype)
        {
            UtilityPager pager = null;
            if (id <= 0 && pageSize > 0)
            {
                pager = new UtilityPager();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
            }
            List<BankProject> list = null;
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, searchwhere = string.Empty;
            string strSql = string.Format("{0} 1=1 ",
                Utility.GetMSSQL_SQL(typeof(SysBankProject), Utility.loand_Sys_BankProject));
            string where = "";
            SqlParameter[] parameters = null;
            where += " and valid=1 ";
            if (bankid > 0)
            {
                where += " and bankid=" + bankid;
            }
            if (id > 0)
            {
                where += "  and id=" + id;
            }
            if (customertype != (int)EnumCustomerType.Company_Bank && customerid > 0)
            {
                where += "  and customerid=" + customerid;
            }
            if (!Utils.IsNullOrEmpty(key))
            {
                where += " and (projectname like @KEY escape '$')  ";
                parameters = new SqlParameter[] { SqlHelper.GetSqlParameter("@KEY", "%" + key + "%", SqlDbType.NVarChar, 100) };
            }
            strSql = strSql + where;
            if (!Utils.IsNullOrEmpty(orderProperty))
            {
                strSql += " order by " + orderProperty;
            }
            list = _mssqlado.GetList<BankProject>(strSql, pager, Utility.loand_Sys_BankProject + " where 1=1 " + where, parameters);
            if (list.Count > 0)
            {
                string sql = string.Format("{0} CustomerId in (" + string.Join(",", list.Select(o => o.BankId).ToArray().Distinct()) + ") ",
                    Utility.GetMSSQL_SQL(typeof(SysCustomer), Utility.loan_Sys_Customer));
                List<SysCustomer> privlist = _mssqlado.GetList<SysCustomer>(sql);
                if (privlist == null || privlist.Count <= 0)
                {
                    return Utility.GetJson(0, "获取失败,数据有误");
                }
                list.ForEach(o =>
                {
                    o.BankName = privlist.Where(p => p.CustomerId == o.BankId).FirstOrDefault().CustomerName;
                });
                if (id > 0)
                {
                    return Utility.GetJson(1, "获取成功", list.FirstOrDefault(), (pager == null) ? list.Count : pager.Count);
                }
                else
                {
                    return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
                }
            }
            else
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
        }


        /// <summary>
        /// 获取指定城市列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string GetAppointCity(int customerId)
        {
            string banksql = string.Empty,
                sql = string.Format("select distinct CityId from {0} with(nolock) where CityId>0 and  Status=1  ",
                Utility.loan_Data_Collateral);

            if (customerId.Equals(1))
            {
                banksql = string.Format("select Id from {0} ", Utility.loand_Sys_BankProject);
                sql = string.Format("{0} and BankProjectId in ({1})", sql, banksql);
            }
            else
            {
                banksql = string.Format("select Id from {0} where CustomerId={1}", Utility.loand_Sys_BankProject, customerId);
                sql = string.Format("{0} and BankProjectId in ({1})", sql, banksql);
            }

            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            IList<SYSCity> list = _mssqlado.GetList<SYSCity>(sql);
            MSSQLADODAL _mssql = new MSSQLADODAL();
            if (list.Count > 0)
            {
                list = UtilityDALHelper.GetADOCity(_mssql, string.Join(",", list.Select(o => o.CityId).ToArray().Distinct()));
            }
            if (list.Count > 0)
                return Utility.GetJson(1, "", list);
            return Utility.GetJson(0, "");
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="id">主键Id</param>
        /// <param name="orderProperty">排序</param>
        /// <returns></returns>
        public string GetTaskList(int pageIndex, int pageSize, int id
            , string orderProperty, string key, int bankid, int status, int customerid, int customertype)
        {
            UtilityPager pager = null;
            if (id <= 0 && pageSize > 0)
            {
                pager = new UtilityPager();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
            }
            List<SysTask> list = null;
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, searchwhere = string.Empty;
            string strSql = string.Format("{0} 1=1 ",
                Utility.GetMSSQL_SQL(typeof(SysTask), Utility.loand_Sys_Task));
            string where = "";
            if (customertype != (int)EnumCustomerType.Company_Fxt)
            {
                where += string.Format(" and BankProjectId in (select Id from {0} where CustomerId={1}) ", Utility.loand_Sys_BankProject, customerid);
            }
            SqlParameter[] parameters = null;
            if (status != -1)
            {
                where += " and status=" + status;
            }
            if (!Utils.IsNullOrEmpty(key))
            {
                where += " and (title like @KEY escape '$')  ";
                parameters = new SqlParameter[] { SqlHelper.GetSqlParameter("@KEY", "%" + key + "%", SqlDbType.NVarChar, 100) };
            }
            strSql += where;
            if (!Utils.IsNullOrEmpty(orderProperty))
            {
                strSql += " order by " + orderProperty;
            }
            list = _mssqlado.GetList<SysTask>(strSql, pager, Utility.loand_Sys_Task + " where 1=1 " + where
              , parameters);
            if (list.Count > 0)
            {
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
            }
            else
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
        }


        /// <summary>
        /// 获取指定任务日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="taskid">taskid</param>
        /// <param name="orderProperty">排序</param>
        /// <returns></returns>
        public string GetTaskLogList(int pageIndex, int pageSize, int taskid
            , string orderProperty, string key)
        {
            UtilityPager pager = null;
            if (pageSize > 0)
            {
                pager = new UtilityPager();
                pager.PageIndex = pageIndex;
                pager.PageSize = pageSize;
            }
            List<SysTaskLog> list = null;
            MSSQLADODAL _mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            string strWhere = string.Empty, searchwhere = string.Empty;
            string strSql = string.Format("{0} 1=1 ",
                Utility.GetMSSQL_SQL(typeof(SysTaskLog), Utility.loand_Sys_TaskLog));
            string where = "";
            SqlParameter[] parameters = null;
            if (taskid > 0)
            {
                where += " and taskid=" + taskid;
            }
            strSql += where;
            if (!Utils.IsNullOrEmpty(orderProperty))
            {
                strSql += " order by " + orderProperty;
            }
            list = _mssqlado.GetList<SysTaskLog>(strSql, pager, Utility.loand_Sys_TaskLog + " where 1=1 " + where
              , parameters);
            if (list.Count > 0)
            {
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
            }
            else
                return Utility.GetJson(1, "获取成功", list, (pager == null) ? list.Count : pager.Count);
        }


        /// <summary>
        /// 修改任务执行状态
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns></returns>
        public string EditTaskStatus(int status, int id)
        {
            SysTask sysBP = new SysTask();
            sysBP.Id = id;
            sysBP.Status = status;
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            int flag = 0;
            sysBP.SetAvailableFields(new string[] { "status" });
            flag = CAS.DataAccess.DA.BaseDA.UpdateFromEntity<SysTask>(sysBP);
            if (flag > 0)
                return Utility.GetJson(1, "Success");
            return Utility.GetJson(0, "");
        }
        #endregion

        #region 文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="otype"></param>
        /// <returns></returns>
        public bool Uploads(string model, string otype)
        {
            SysUploadFile sufModel = JsonConvert.DeserializeObject<SysUploadFile>(model);
            bool flag = false;
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            if (otype.ToUpper().Equals("C"))
            {
                sufModel.CreateDate = DateTime.Now;
                flag = CAS.DataAccess.DA.BaseDA.InsertFromEntity<SysUploadFile>(sufModel) > 0;
            }
            else if (otype.ToUpper().Equals("U"))
            {
                SysUploadFile sufUpdate = CAS.DataAccess.DA.BaseDA
                    .ExecuteToEntityByPrimaryKey<SysUploadFile>(sufModel.Id);

                if (sufModel.PageIndex > 0)
                    sufUpdate.PageIndex = sufModel.PageIndex;
                else
                    sufUpdate.PageIndex = 0;

                flag = CAS.DataAccess.DA.BaseDA
                    .UpdateFromEntity<SysUploadFile>(sufUpdate) > 0;
            }
            else if (otype.ToUpper().Equals("D"))
            {
                flag = CAS.DataAccess.DA.BaseDA
                    .DeleteByPrimaryKey<SysUploadFile>(sufModel) > 0;
            }
            return flag;
        }
        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <param name="uploadfile"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex"></param>
        /// <param name="bankid"></param>
        /// <param name="proid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetUploads(string uploadfile, int pageSize, int pageIndex
           , int bankid, int proid, string key, int customerid, int customertype)
        {
            MSSQLADODAL mssqlado = new MSSQLADODAL(Utility.DBFxtLoan);
            SysUploadFile sysUploadFile = JsonConvert.DeserializeObject<SysUploadFile>(uploadfile);
            UtilityPager pager = new UtilityPager(pageSize, pageIndex);
            SqlParameter[] parameters = null;
            string sql = string.Format("{0} {1}",
                Utility.GetMSSQL_SQL(typeof(SysUploadFile), Utility.SysUploadFile),
                Utility.GetModelFieldKeyValue(sysUploadFile));
            if (customertype != (int)EnumCustomerType.Company_Fxt)
            {
                sql += string.Format(" and BankProId in (select Id from {0} where CustomerId={1}) ", Utility.loand_Sys_BankProject, customerid);
            }
            if (bankid > 0)
            {
                sql += " and BankId=" + bankid;
            }
            if (proid > 0)
            {
                sql += " and BankProId=" + proid;
            }
            if (!Utils.IsNullOrEmpty(key))
            {
                sql += " and ([Name] like @KEY escape '$')  ";
                parameters = new SqlParameter[] {                     SqlHelper.GetSqlParameter("@KEY", "%" + key + "%", SqlDbType.NVarChar, 100)
                };
            }
            List<SysUploadFile> list = mssqlado.GetList<SysUploadFile>(sql, pager,
                Utility.SysUploadFile, parameters);
            List<SysUploadFiles> lists = new List<SysUploadFiles>();

            list.ForEach(item =>
            {
                SysUploadFiles sups = new SysUploadFiles(item);
                sql = string.Format("select count(*) as Id from {0} with(nolock) where Status=1 and  UploadFileId={1} ",
                    Utility.loan_Data_Collateral, item.Id);
                List<DataCollateral> datalist = mssqlado.GetList<DataCollateral>(sql);
                if (datalist != null && datalist.Count > 0)
                {
                    sups.FileSuccessCount = datalist.FirstOrDefault().Id;
                }
                lists.Add(sups);
            });
            return Utility.GetJson(1, null, lists, pager.Count);
        }

        /// <summary>
        /// 导出复估押品
        /// </summary>
        /// <returns></returns>
        public string TaskExport(int uploadfileid, int customerid)
        {
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "[dbo].[CollateralAssessment]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@uploadfileid", uploadfileid, SqlDbType.Int));
            DataSet dset = CAS.DataAccess.DA.BaseDA.ExecuteDataSet(cmd);
            return Utility.GetJson(1, null, dset.Tables[0], 1);
        }

        /// <summary>
        /// 导入复估押品
        /// </summary>
        /// <returns></returns>
        public string TaskExcelUp(string objResolve, int uploadfileid, int rows, int cols)
        {
            object[] sysBP = Utils.Deserialize<object[]>(objResolve);
            StringBuilder sqlbuild = new StringBuilder();
            string sql = " update " + Utility.loan_Data_Data_Reassessment + " set [Price]={0}, CalculationMode='{2}' where Id={1} ";
            if (sysBP != null)
            {
                string id = "", price = "", xfgval = "", autofgval = "", CalculationMode = "";
                for (int i = 0; i < rows; i++)
                {
                    id = sysBP[(i * 2)].ToString();
                    price = sysBP[(i * 2) + 1].ToString();
                    xfgval = sysBP[(i * 2) + 2].ToString();
                    autofgval = sysBP[(i * 2) + 3].ToString();
                    if (price == autofgval)
                    {
                        CalculationMode = "自动估价";
                    }
                    else if (price == xfgval)
                    {
                        CalculationMode = "案例估价";
                    }
                    else
                    {
                        CalculationMode = "人工复估";
                    }
                    if (id != "" && Int32.Parse(id) > 0)
                    {
                        sqlbuild.Append(string.Format(sql, (price == "") ? "0" : price, id, CalculationMode));
                    }
                }
            }
            MSSQLADODAL.SetConnection(Utility.DBFxtLoan);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlbuild.ToString();
            cmd.CommandType = CommandType.Text;
            int icount = CAS.DataAccess.DA.BaseDA.ExecuteNonQuery(cmd);
            return Utility.GetJson((icount > 0) ? 1 : 0, (icount > 0) ? "导入成功" : "导入失败", "", icount);
        }

        #endregion

        /// <summary>
        /// 获取的城市和项目是否都为空
        /// </summary>
        /// <param name="city"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        private bool CityOrPeojectIsNull(string city, string project)
        {
            return (city.ToLower().Equals("null") || city.Equals("")) &&
                (project.ToLower().Equals("null") || project.Equals(""));
        }
    }
}
