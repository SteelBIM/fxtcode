using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class ViewUserJoinDepartmentManager
    {
        /// <summary>
        /// 公共连表查询sql
        /// </summary>
        const string ViewSql = "select du.UserName,du.DepartmentID,du.CityID,du.FxtCompanyID,d.DepartmentName,d.DValid from Privi_Department_User as du with(nolock) left join Privi_Department as d with(nolock) on du.DepartmentID=d.DepartmentID";
        ///// <summary>
        ///// 将连表查询出来的数据分装到实体
        ///// </summary>
        ///// <param name="ilist"></param>
        ///// <returns></returns>
        //static List<View_UserJoinDepartment> ConvertToList(IList ilist)
        //{
        //    List<View_UserJoinDepartment> list = new List<View_UserJoinDepartment>();
        //    for (int i = 0; i < ilist.Count; i++)
        //    {
        //        View_UserJoinDepartment obj = new View_UserJoinDepartment();
        //        object[] objs = ilist[i] as object[];
        //        obj.UserName = Convert.ToString(objs[0]);
        //        obj.DepartmentID = Convert.ToInt32(objs[1]);
        //        obj.CityID = Convert.ToInt32(objs[2]);
        //        obj.FxtCompanyID = Convert.ToInt32(objs[3]);
        //        obj.DepartmentName = Convert.ToString(objs[4]);
        //        obj.DValid = Convert.ToInt32(objs[5]);
        //        list.Add(obj);
        //    }
        //    return list;
        //}
        /// <summary>
        /// 查询View_DepartmentJoinRole信息
        /// </summary>
        /// <param name="whereSql"></param>
        /// <param name="utilityPager"></param>
        /// <param name="tblName">放于表后的别名</param>
        /// <param name="_db"></param>
        /// <returns></returns>
        static List<View_UserJoinDepartment> GetList(string whereSql, List<NHParameter> parameters, UtilityPager utilityPager = null, string tblName = "tbl", DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from (").Append(ViewSql).Append(") as ").Append(tblName).Append(" ");
                if (!string.IsNullOrEmpty(whereSql))
                {
                    sb.Append(" where ").Append(whereSql);
                }
                //IList ilist = null;
                IList<View_UserJoinDepartment> list = new List<View_UserJoinDepartment>();

                if (utilityPager != null)
                {
                    list = db.DB.PagerList<View_UserJoinDepartment>(utilityPager, sb.ToString(), parameters, isDTO: true).ToList();
                    //ilist = db.DB.GetCustomSQLQueryObjectList(utilityPager, sb.ToString(), parameters);
                }
                else
                {
                    list = db.DB.GetCustomSQLQueryList<View_UserJoinDepartment>(sb.ToString(), parameters, isDTO: true).ToList();
                    //ilist = db.DB.GetCustomSQLQueryObjectList(sb.ToString(), parameters);
                }
                db.Close();
                //List<View_UserJoinDepartment> list = ConvertToList(ilist);
                return list as List<View_UserJoinDepartment>;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据小组和角色获取用户信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="userName"></param>
        /// <param name="departmentId"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isGetCount"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static List<View_UserJoinDepartment> GetUserJoinDepartmentByDepartmentIdAndRoleIdAndUserName(int companyId, int cityId, string userName, int departmentId, int? roleId, int pageIndex, int pageSize, out int count, bool isGetCount = true, DataBase _db = null)
        {
            count = 0;
            string tblName = "tbl";
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("DValid=1 and DepartmentId=").Append(departmentId);
                sbSql.Append(" and FxtCompanyID=").Append(companyId);
                sbSql.Append(" and CityID=").Append(cityId);
                if (roleId != null && Convert.ToInt32(roleId) > 0)
                {
                    sbSql.Append(string.Format(" and exists (select * from {0} as ru where ru.FxtCompanyID={1} and ru.CityID={2} and {3}.UserName=ru.UserName and RoleId={4})",
                        NHibernateUtility.TableName_SYSRoleUser, companyId, cityId, tblName, roleId));
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    sbSql.Append(" and UserName like '%").Append(userName).Append("%'");
                }
                UtilityPager pageInfo = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
                List<View_UserJoinDepartment> list = GetList(sbSql.ToString(), null, pageInfo, _db: db);
                db.Close();
                count = pageInfo.Count;
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据多个用户名获取其所在组
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="userNames"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static List<View_UserJoinDepartment> GetUserJoinDepartmentByUserNames(int companyId, int cityId, string[] userNames, DataBase _db = null)
        {
            if (userNames == null || userNames.Length < 1)
            {
                return new List<View_UserJoinDepartment>();
            }
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder userNameSb = new StringBuilder();
                foreach (string str in userNames)
                {
                    userNameSb.Append("'").Append(str).Append("',");
                }
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(" DValid=1 and  FxtCompanyID=").Append(companyId);
                sbSql.Append(" and CityID=").Append(cityId);
                sbSql.Append(" and UserName in (").Append(userNameSb.ToString().TrimEnd(',')).Append(")");
                List<View_UserJoinDepartment> list = GetList(sbSql.ToString(), null, _db: db);
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
    }
}
