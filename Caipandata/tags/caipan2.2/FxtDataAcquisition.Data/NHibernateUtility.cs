using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Data
{
    public static class NHibernateUtility
    {


        /// <summary>
        /// 楼盘表名
        /// </summary>
        public const string TableName_DATProject = "DAT_Project";
        /// <summary>
        /// 照片表名
        /// </summary>
        public const string TableName_LNKPPhoto = "LNK_P_Photo";
        /// <summary>
        /// 房号表
        /// </summary>
        public const string TableName_DATHouse = "DAT_House";
        /// <summary>
        /// 楼栋表
        /// </summary>
        public const string TableName_DATBuilding = "DAT_Building";
        /// <summary>
        /// 角色表
        /// </summary>
        public const string TableName_SYSRole = "SYS_Role";
        /// <summary>
        /// 菜单表
        /// </summary>
        public const string TableName_SYSMenu = "SYS_Menu";
        /// <summary>
        /// 角色对应菜单表
        /// </summary>
        public const string TableName_SYSRoleMenu = "SYS_Role_Menu";
        /// <summary>
        /// 角色对应菜单对应操作功能表
        /// </summary>
        public const string TableName_SYSRoleMenuFunction = "SYS_Role_Menu_Function";
        /// <summary>
        /// 用户对应角色表
        /// </summary>
        public const string TableName_SYSRoleUser = "SYS_Role_User";
        /// <summary>
        /// 小组表
        /// </summary>
        public const string TableName_PriviDepartment = "Privi_Department";
        /// <summary>
        /// 小组用户表
        /// </summary>
        public const string TableName_PriviDepartmentUser = "Privi_Department_User";
        /// <summary>
        /// 楼盘企业表
        /// </summary>
        public const string TableName_LNKPCompany = "LNK_P_Company";
        /// <summary>
        /// 楼盘配套表
        /// </summary>
        public const string TableName_LNKPAppendage = "LNK_P_Appendage";
        /// <summary>
        /// 任务表
        /// </summary>
        public const string TableName_DatAllotSurvey = "Dat_AllotSurvey";
        /// <summary>
        /// 任务表
        /// </summary>
        public const string TableName_DatAllotFlow = "Dat_AllotFlow";
        /// <summary>
        /// 任务审核表
        /// </summary>
        public const string TableName_DatCheck = "Dat_Check";
        /// <summary>
        /// 视图-任务表连接楼盘表
        /// </summary>
        public const string ViewName_DatAllotFlowJoinProject = "select af.id,af.CityId as AllotCityId,af.FxtCompanyId as AllotFxtCompanyId,af.CreateTime as AllotCreateTime,af.StateDate,af.StateCode,af.DatType,af.UserName,af.UserTrueName,af.SurveyUserName,af.SurveyUserTrueName,"
                                                               + "pro.* from " + TableName_DatAllotFlow + " as af with(nolock)  left join " + TableName_DATProject + " as pro with(nolock)  on af.DatId=pro.ProjectId and af.CityId=pro.CityID and af.FxtCompanyId=pro.FxtCompanyId";

        /// <summary>
        /// 获得某个实体的所有属性
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static string GetModelFields(Type modelType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var propertys = modelType.GetProperties();
            int i = 0;
            do
            {
                stringBuilder.AppendFormat("{0},", propertys[i].Name);
                i++;
            } while (propertys.Count() > i);
            return stringBuilder.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 获得公共HSQL语句(HSQL)
        /// </summary>
        /// <param name="modelType">实体,TypeOf</param>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类)</param>
        /// <returns></returns>
        public static string GetMSSQL_HSQL(Type modelType, string tablename = null)//, string keyword = null
        {
            if (string.IsNullOrEmpty(tablename))
                tablename = modelType.Name;
            //return string.Format("select {0} {1} from {2} where ", keyword, GetModelFields(modelType), GetEntitesName(tablename));
            return string.Format("from {0} _tb where ", GetEntitesName(tablename));
        }

        /// <summary>
        /// HSQL
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string GetMSSQL_HSQL(string tablename)
        {
            tablename = tablename.Replace("_", "").Replace("dbo.", "");
            return string.Format("from {0} _tb where ", tablename);
        }
        /// <summary>
        /// 获得公共SQL语句(SQL)
        /// </summary>
        /// <param name="modelType">实体,TypeOf</param>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类)</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL(Type modelType, string tablename = null, string keyword = null)
        {
            if (string.IsNullOrEmpty(tablename))
                tablename = modelType.Name;
            return string.Format("select {0} {1} from {2} as _tb where ", keyword, GetModelFields(modelType), tablename);
        }

        /// <summary>
        /// 获得公共SQL语句(SQL)(用于不锁表)
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类,用于列名之前的关键字)</param>
        /// <param name="columns">指定查询列名</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL_NOLOCK(string tablename = null, string keyword = null,string columns=null)
        {
            string _columns = "*";
            if (!string.IsNullOrEmpty(columns))
            {
                _columns = columns;
            }
            return string.Format("select {0} {1} from {2} as _tb with(nolock) where ", keyword,_columns, tablename);
        }
        
        /// <summary>
        /// 获得公共SQL语句(SQL)(用于视图连接查询)
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="keyword">关键字,默认为空(top之类,用于列名之前的关键字)</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL_VIEW(string viewname = null, string keyword = null)
        {
            //return string.Format("select {0} id,AllotCityId,AllotFxtCompanyId,AllotCreateTime,StateDate,StateCode,DatType,UserName,SurveyUserName,ProjectId,ProjectName,SubAreaId,FieldNo,PurposeCode,Address,LandArea,StartDate,UsableYear,BuildingArea,SalableArea,CubageRate,GreenRate,BuildingDate,CoverDate,SaleDate,JoinDate,EndDate,InnerSaleDate,RightCode,ParkingNumber,AveragePrice,ManagerTel,ManagerPrice,TotalNum,BuildingNum,Detail,BuildingTypeCode UpdateDateTime,OfficeArea,OtherArea,PlanPurpose,PriceDate,IsComplete,OtherName,SaveDateTime,SaveUser,Weight,BusinessArea,IndustryArea,IsEValue,PinYin,CityID,AreaID,OldId,CreateTime,AreaLineId,Valid,SalePrice,FxtCompanyId,PinYinAll,X,Y,XYScale,Creator,IsEmpty,TotalId,East,West,South,North,FxtProjectId,Status from ({1}) as _tb  where ", keyword, viewname);
            return string.Format("select {0} * from ({1}) as _tb  where ", keyword, viewname);
        }
        public static string GetMSSQL_SQL_AVG(string column, string tablename)
        {
            return string.Format("select avg({0}) from {1} as _tb where ", column, tablename);
        }
        public static string GetMSSQL_SQL_COUNT(string tablename)
        {
            return string.Format("select count(*) from {0} as _tb with(nolock) where ", tablename);
        }
        /// <summary>
        /// SQL
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static string GetMSSQL_SQL(string tablename)
        {
            return string.Format("from {0} _tb where ", tablename);
        }
        /// <summary>
        /// 获取实体名(用于HSQL)
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static string GetEntitesName(string tableName)
        {
            return tableName.Replace("_", "").Replace("dbo.", "");
        }
    }
}
