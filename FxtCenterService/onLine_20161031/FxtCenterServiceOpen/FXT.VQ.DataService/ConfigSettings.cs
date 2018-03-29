using System;

namespace FXT.VQ.DataService
{
    internal class ConfigSettings
    {
        internal readonly static string mDataCenterServiceURL = System.Configuration.ConfigurationManager.AppSettings["DataCenterServiceURL"].ToString();
        internal readonly static string mDataCenterCompanyid = System.Configuration.ConfigurationManager.AppSettings["DataCenterCompanyid"].ToString();
        internal readonly static string mDataCenterSignname = System.Configuration.ConfigurationManager.AppSettings["DataCenterSignname"].ToString();
        internal readonly static string mDataCenterAppid = System.Configuration.ConfigurationManager.AppSettings["DataCenterAppid"].ToString();
        internal readonly static string mDataCenterAppkey = System.Configuration.ConfigurationManager.AppSettings["DataCenterAppkey"].ToString();
        internal readonly static string mDataCenterApppwd = System.Configuration.ConfigurationManager.AppSettings["DataCenterApppwd"].ToString();
        internal readonly static string mDataCenterSystypecode = System.Configuration.ConfigurationManager.AppSettings["DataCenterSystypecode"].ToString();
    }
}
