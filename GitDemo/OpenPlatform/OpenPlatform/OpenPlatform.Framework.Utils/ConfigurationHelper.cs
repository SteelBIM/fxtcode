using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Framework.Utils
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// 数据库连接字符串(博纳)
        /// </summary>
        public static readonly string ConnString4Bona = Convert.ToString(ConfigurationManager.ConnectionStrings["OpenPlatform4bona"]);
        /// <summary>
        ///数据库连接字符串(流量控制)
        /// </summary>
        public static readonly string ConnString = Convert.ToString(ConfigurationManager.ConnectionStrings["OpenPlatform"]);
        #region 估价宝
        /// <summary>
        /// AppId
        /// </summary>
        public static readonly string GjbAppId = Convert.ToString(ConfigurationManager.AppSettings["gjbappid"]);

        /// <summary>
        /// AppWd
        /// </summary>
        public static readonly string GjbAppWd = Convert.ToString(ConfigurationManager.AppSettings["gjbappwd"]);

        /// <summary>
        /// systypecode
        /// </summary>
        public static readonly string GjbSysTypeCode = Convert.ToString(ConfigurationManager.AppSettings["gjbsystypecode"]);

        /// <summary>
        /// SignName
        /// </summary>
        public static readonly string GjbSignName = Convert.ToString(ConfigurationManager.AppSettings["gjbsignname"]);
        /// <summary>
        /// appkey
        /// </summary>
        public static readonly string GjbAppKey = Convert.ToString(ConfigurationManager.AppSettings["gjbappkey"]);
        /// <summary>
        /// AppUrl
        /// </summary>
        public static readonly string GjbAppUrl = Convert.ToString(ConfigurationManager.AppSettings["gjbappurl"]);
        /// <summary>
        /// funname
        /// </summary>
        public static readonly string GjbFunName = Convert.ToString(ConfigurationManager.AppSettings["gjbfunname"]);

        #endregion

        #region 云查勘
        /// <summary>
        /// AppId
        /// </summary>
        public static readonly string YckAppId = Convert.ToString(ConfigurationManager.AppSettings["yckappid"]);

        /// <summary>
        /// AppWd
        /// </summary>
        public static readonly string YckAppWd = Convert.ToString(ConfigurationManager.AppSettings["yckappwd"]);

        /// <summary>
        /// systypecode
        /// </summary>
        public static readonly string YckSysTypeCode = Convert.ToString(ConfigurationManager.AppSettings["ycksystypecode"]);

        /// <summary>
        /// SignName
        /// </summary>
        public static readonly string YckSignName = Convert.ToString(ConfigurationManager.AppSettings["ycksignname"]);
        /// <summary>
        /// appkey
        /// </summary>
        public static readonly string YckAppKey = Convert.ToString(ConfigurationManager.AppSettings["yckappkey"]);
        /// <summary>
        /// AppUrl
        /// </summary>
        public static readonly string YckAppUrl = Convert.ToString(ConfigurationManager.AppSettings["yckappurl"]);
        /// <summary>
        /// funname
        /// </summary>
        public static readonly string YckFunName1 = Convert.ToString(ConfigurationManager.AppSettings["yckfunname1"]);
        /// <summary>
        /// funname
        /// </summary>
        public static readonly string YckFunName2 = Convert.ToString(ConfigurationManager.AppSettings["yckfunname2"]);
        /// <summary>
        /// funname
        /// </summary>
        public static readonly string YckFunName3 = Convert.ToString(ConfigurationManager.AppSettings["yckfunname3"]);

        #endregion

        #region 数据中心自动估价
        /// <summary>
        /// AppId
        /// </summary>
        public static readonly string DcAppId = Convert.ToString(ConfigurationManager.AppSettings["dcappid"]);

        /// <summary>
        /// AppWd
        /// </summary>
        public static readonly string DcAppWd = Convert.ToString(ConfigurationManager.AppSettings["dcappwd"]);

        /// <summary>
        /// systypecode
        /// </summary>
        public static readonly string DcSysTypeCode = Convert.ToString(ConfigurationManager.AppSettings["ycksystypecode"]);

        /// <summary>
        /// SignName
        /// </summary>
        public static readonly string DcSignName = Convert.ToString(ConfigurationManager.AppSettings["dcsignname"]);
        /// <summary>
        /// appkey
        /// </summary>
        public static readonly string DcAppKey = Convert.ToString(ConfigurationManager.AppSettings["dcappkey"]);
        /// <summary>
        /// AppUrl
        /// </summary>
        public static readonly string DcAppUrl = Convert.ToString(ConfigurationManager.AppSettings["dcappurl"]);
        /// <summary>
        /// funname
        /// </summary>
        public static readonly string DcFunName = Convert.ToString(ConfigurationManager.AppSettings["dcfunname"]);

        #endregion
        /// <summary>
        /// 加密KEY
        /// </summary>
        public const string Encryptkey = "fxtop";

    }
}