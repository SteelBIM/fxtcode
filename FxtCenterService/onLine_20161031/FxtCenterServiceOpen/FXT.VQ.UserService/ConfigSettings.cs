using System;

/***********************************************************
 * 功能：用户中心配置获取
 *  
 * 创建：魏贝
 * 时间：2015/12
***********************************************************/

namespace FXT.VQ.UserService
{
    internal class ConfigSettings
    {
        internal readonly static string mUserCenterServiceURL = System.Configuration.ConfigurationManager.AppSettings["UserCenterServiceURL"].ToString();
        internal readonly static string mUserCenterCompanyid = System.Configuration.ConfigurationManager.AppSettings["UserCenterCompanyid"].ToString();
        internal readonly static string mUserCenterSignname = System.Configuration.ConfigurationManager.AppSettings["UserCenterSignname"].ToString();
        internal readonly static string mUserCenterAppid = System.Configuration.ConfigurationManager.AppSettings["UserCenterAppid"].ToString();
        internal readonly static string mUserCenterAppkey = System.Configuration.ConfigurationManager.AppSettings["UserCenterAppkey"].ToString();
        internal readonly static string mUserCenterApppwd = System.Configuration.ConfigurationManager.AppSettings["UserCenterApppwd"].ToString();
        internal readonly static string mUserCenterSystypecode = System.Configuration.ConfigurationManager.AppSettings["UserCenterSystypecode"].ToString();
    }
}
