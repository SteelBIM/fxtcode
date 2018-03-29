using FxtDataAcquisition.Application.Services;
using FxtDataAcquisition.Data;
using FxtDataAcquisition.DTODomain.NHibernate;
using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class SYSMenuManager
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(SYSMenuManager));
        /// <summary>
        /// 获取当前用户所拥有菜单类型的页面信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="_db"></param>
        /// <returns></returns>
        public static IList<SYSMenu> GetSYSMenuPageByUserNameAndCompanyIdAndCityId(string userName, int companyId, int cityId, DataBase _db = null)
        {
            DataBase db = new DataBase(_db);
            try
            {
                string sql = @"{0} TypeCode=:typdCode and Id in (
                                select MenuId from {1} with(nolock) where RoleId in (
                                    select RoleId from {2} with(nolock) where  FxtCompanyId=:companyId and CityId in (0,:cityId) and UserName=:userName
                                )
                            )";
                sql = string.Format(sql, NHibernateUtility.GetMSSQL_SQL_NOLOCK(NHibernateUtility.TableName_SYSMenu),
                    NHibernateUtility.TableName_SYSRoleMenu, NHibernateUtility.TableName_SYSRoleUser);
                List<NHParameter> parameters = new List<NHParameter>();
                parameters.Add(new NHParameter("typdCode", SYSCodeManager.MENUTYPECODE_1, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("cityId", cityId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("companyId", companyId, NHibernateUtil.Int32));
                parameters.Add(new NHParameter("userName", userName, NHibernateUtil.String));
                IList<SYSMenu> list = db.DB.GetCustomSQLQueryList<SYSMenu>(sql, parameters);
                db.Close();
                return list;
            }
            catch (Exception ex)
            {
                db.Close();
                throw ex;
            }

            //SYS_Role_Menu SYS_Role_User
        }
    }
}
