using System;
using System.Configuration;

namespace FXT.DataCenter.Infrastructure.Common.DBHelper
{
    public class ConfigurationHelper
    {

        public static readonly string FxtCompanyId = ConfigurationManager.AppSettings["FxtCompanyId"];
        /// <summary>
        /// 住宅基准房价更新日期
        /// </summary>
        public static readonly string AvgPriceUpdatedDate = ConfigurationManager.AppSettings["AvgPriceUpdatedDate"];

        public static readonly bool IsOss = ConfigurationManager.AppSettings["IsOss"].ToUpper() == "TRUE" ? true : false;
        public static readonly string OssUri = ConfigurationManager.AppSettings["OssUri"];
        public static readonly string OssImgServer = ConfigurationManager.AppSettings["OssImgServer"];

        //用户中心信息
        public static readonly string fxtusercenterloginservice = ConfigurationManager.AppSettings["fxtusercenterloginservice"];
        public static readonly string fxtusercenterservice = ConfigurationManager.AppSettings["fxtusercenterservice"];
        public static readonly string usercenterserviceappid = ConfigurationManager.AppSettings["usercenterserviceappid"];
        public static readonly string usercenterserviceapppwd = ConfigurationManager.AppSettings["usercenterserviceapppwd"];
        public static readonly string usercenterserviceappkey = ConfigurationManager.AppSettings["usercenterserviceappkey"];
        public static readonly string signname = ConfigurationManager.AppSettings["signname"];

        #region 数据库连接字符串
        /// <summary>
        /// 数据中心连接字符串
        /// 用户角色
        /// </summary>
        public static readonly string FxtDataCenter = ConfigurationManager.ConnectionStrings["FxtDataCenter_Role"].ConnectionString;

        /// <summary>
        /// 数据中心连接字符串
        /// </summary>
        public static readonly string FxtProject = ConfigurationManager.ConnectionStrings["FXTProject"].ConnectionString;

        /// <summary>
        /// 土地连接字符串
        /// </summary>
        public static readonly string FxtLand = ConfigurationManager.ConnectionStrings["FxtLand"].ConnectionString;
        /// <summary>
        /// 数据中心连接字符串
        /// 用户
        /// </summary>
        public static readonly string FxtUserCenter = ConfigurationManager.ConnectionStrings["FxtUserCenter"].ConnectionString;
        
        /// <summary>
        /// 商业连接字符串
        /// </summary>
        public static readonly string FxtDataBiz = ConfigurationManager.ConnectionStrings["FxtData_Biz"].ConnectionString;

        /// <summary>
        /// 办公连接字符串
        /// </summary>
        public static readonly string FxtDataOffice = ConfigurationManager.ConnectionStrings["FxtDataOffice"].ConnectionString;

        /// <summary>
        /// 工业连接字符串
        /// </summary>
        public static readonly string FxtDataIndustry;

        /// <summary>
        /// 业主信息连接字符串
        /// </summary>
        public static readonly string FxtDataHuman;

        /// <summary>
        /// 系统日志连接字符串
        /// </summary>
        public static readonly string FXTLog;
        #endregion

        static ConfigurationHelper()
        {
            FxtDataIndustry = null;
            FxtDataHuman = null;
            FXTLog = null;
            ConnectionStringSettings cns = ConfigurationManager.ConnectionStrings["FxtDataIndustry"];
            if (cns != null)
            {
                FxtDataIndustry = cns.ConnectionString;
            }
            cns = ConfigurationManager.ConnectionStrings["FxtDataHuman"];
            if (cns != null)
            {
                FxtDataHuman = cns.ConnectionString;
            }
            cns = ConfigurationManager.ConnectionStrings["FXTLog"];
            if (cns != null)
            {
                FXTLog = cns.ConnectionString;
            }
        }

    }
}
