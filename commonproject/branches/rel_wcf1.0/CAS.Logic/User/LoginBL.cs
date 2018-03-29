using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity;
using CAS.DataAccess;
using System.Data;
using CAS.DataAccess.DA.User;
using CAS.Entity.DBEntity;
using CAS.Common;
namespace CAS.Logic.User
{
    public class LoginBL
    {
        /// <summary>
        /// 云查勘手机登录
        /// </summary>
        /// <param name="search"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PriviUser LoginBySurveyMobile(SearchBase search, string password)
        {
            return LoginDA.LoginBySurveyMobile(search, password);
        }
    }
}
