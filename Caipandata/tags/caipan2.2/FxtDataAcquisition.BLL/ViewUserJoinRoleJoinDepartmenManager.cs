using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class ViewUserJoinRoleJoinDepartmenManager
    {
        const string ViewSql = "select ru.UserName,ru.CityID,ru.FxtCompanyID,ru.RoleID,r.RoleName,r.Valid as RValid,du.DepartmentID,d.DepartmentName,d.DValid from SYS_Role_User as ru with(nolock) left join SYS_Role as r with(nolock) on ru.RoleID=r.ID left join Privi_Department_User as du with(nolock) on  ru.FxtCompanyID=du.FxtCompanyID and ru.CityID=du.CityID and du.UserName=ru.UserName and  exists (select * from Privi_Department as _d where _d.DepartmentID=du.DepartmentID and Dvalid=1) left join Privi_Department as d with(nolock) on du.DepartmentID=d.DepartmentID";
        ///// <summary>
        ///// 将连表查询出来的数据分装到实体
        ///// </summary>
        ///// <param name="ilist"></param>
        ///// <returns></returns>
        //static List<View_UserJoinRoleJoinDepartmen> ConvertToList(IList ilist)
        //{
        //    List<View_UserJoinRoleJoinDepartmen> list = new List<View_UserJoinRoleJoinDepartmen>();
        //    for (int i = 0; i < ilist.Count; i++)
        //    {
        //        View_UserJoinRoleJoinDepartmen obj = new View_UserJoinRoleJoinDepartmen();
        //        object[] objs = ilist[i] as object[];
        //        obj.UserName = Convert.ToString(objs[0]);
        //        obj.CityID = Convert.ToInt32(objs[1]);
        //        obj.FxtCompanyID = Convert.ToInt32(objs[2]);
        //        obj.RoleID = Convert.ToInt32(objs[3]);
        //        obj.RoleName = Convert.ToString(objs[4]);
        //        obj.RValid = Convert.ToInt32(objs[5]);
        //        obj.DepartmentID = null;
        //        if (objs[6] != null)
        //        {
        //            obj.DepartmentID = Convert.ToInt32(objs[6]);
        //        }
        //        obj.DepartmentName = Convert.ToString(objs[7]);
        //        obj.DValid = Convert.ToInt32(objs[8]);
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
        static List<View_UserJoinRoleJoinDepartmen> GetList(string whereSql, List<NHParameter> parameters, UtilityPager utilityPager = null, string tblName = "tbl", DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select *  from (").Append(ViewSql).Append(") as ").Append(tblName).Append(" ");
                if (!string.IsNullOrEmpty(whereSql))
                {
                    sb.Append(" where ").Append(whereSql);
                }
                IList<View_UserJoinRoleJoinDepartmen> list = new List<View_UserJoinRoleJoinDepartmen>();
                //IList ilist = null;
                if (utilityPager != null)
                {
                    list = db.DB.PagerList<View_UserJoinRoleJoinDepartmen>(utilityPager, sb.ToString(), parameters, isDTO: true).ToList();
                    //ilist = db.DB.GetCustomSQLQueryObjectList(utilityPager, sb.ToString(), parameters);
                }
                else
                {
                    list = db.DB.GetCustomSQLQueryList<View_UserJoinRoleJoinDepartmen>(sb.ToString(), parameters, isDTO: true).ToList();
                    //ilist = db.DB.GetCustomSQLQueryObjectList(sb.ToString(), parameters);
                }
                db.Close();
                //List<View_UserJoinRoleJoinDepartmen> list = ConvertToList(ilist);
                return list as List<View_UserJoinRoleJoinDepartmen>;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 根据角色和用户名查询用户信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isGetCount"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static List<View_UserJoinRoleJoinDepartmen> GetUserJoinRoleJoinDepartmenByRoleIdAndUserName(int companyId, int cityId, string userName, int roleId, int pageIndex, int pageSize, out int count, bool isGetCount = true, DataBase _db = null)
        {
            count = 0;
            DataBase db = new DataBase(_db);
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("RValid=1  and (DValid=1 or DValid is null)  and RoleID=").Append(roleId);
                sbSql.Append(" and FxtCompanyID=").Append(companyId);
                sbSql.Append(" and CityID=").Append(cityId);
                sbSql.Append(" and (DValid=1 or DValid is null)");
                if (!string.IsNullOrEmpty(userName))
                {
                    sbSql.Append(" and UserName like '%").Append(userName).Append("%'");
                }
                UtilityPager pageInfo = new UtilityPager(pageSize: pageSize, pageIndex: pageIndex, isGetCount: isGetCount);
                List<View_UserJoinRoleJoinDepartmen> list = GetList(sbSql.ToString(), null, pageInfo, _db: db);
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
        public static List<View_UserJoinRoleJoinDepartmen> GetUserJoinRoleJoinDepartmenByUserNames(int companyId, int cityId, string[] userNames, DataBase _db = null)
        {
            if (userNames == null || userNames.Length < 1)
            {
                return new List<View_UserJoinRoleJoinDepartmen>();
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
                sbSql.Append(" RValid=1 and (DValid=1 or DValid is null)  and FxtCompanyID in (").Append("0," + companyId + ")");
                sbSql.Append(" and CityID in (").Append("0," + cityId + ")");
                sbSql.Append(" and UserName in (").Append(userNameSb.ToString().TrimEnd(',')).Append(")");
                List<View_UserJoinRoleJoinDepartmen> list = GetList(sbSql.ToString(), null, _db: db);
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
