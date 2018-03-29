using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace FxtUserCenterService.DataAccess
{
    public class Common
    {
        /// <summary>
        /// 读取SQL语句，注意文件名和传入的参数大小写要匹配
        /// SQL文件必须修改为“嵌入的资源”
        /// 如果不用嵌入也可以使用文件方式读取，好处是应用程序不会重启，但明文存放可能引起容易被篡改等安全问题
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetSql(string sql)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = "FxtUserCenterService.DataAccess.SQL." + sql + ".sql";
            string result = "";
            try
            {
                Stream stream = _assembly.GetManifestResourceStream(resourceName);
                StreamReader myread = new StreamReader(stream);
                result = myread.ReadToEnd();
                myread.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //转为小写，避免sql与前台json大小写不一致
            return result.ToLower();
        }
    }
    /// <summary>
    /// sql类
    /// </summary>
    public class SQLName
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public class UserInfo 
        {
            /// <summary>
            /// 获得用户与App信息
            /// </summary>
            public static string GetUserAndAppInfo = Common.GetSql("UserInfo.GetUserAndAppInfo");

            /// <summary>
            /// 获得用户与App信息
            /// </summary>
            public static string GetApps = Common.GetSql("UserInfo.GetApps");

            /// <summary>
            /// 验证用户密码
            /// </summary>
            public static string CheckUserPwd = Common.GetSql("UserInfo.CheckUserPwd");            
        }
        /// <summary>
        /// 公司表
        /// </summary>
        public class Company
        {
            /// <summary>
            /// 获得公司信息通过signname
            /// </summary>
            public static string GetCompanyInfoBySignName = Common.GetSql("Company.GetCompanyInfoBySignName");
            
        }

        /// <summary>
        /// 公司表
        /// </summary>
        public class ProductApp
        {
            /// <summary>
            /// 获得Appkey
            /// </summary>
            public static string GetAppkey = Common.GetSql("ProductApp.GetAppkey");

        }
    }
}
