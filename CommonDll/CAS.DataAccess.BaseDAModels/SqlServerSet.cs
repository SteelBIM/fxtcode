using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CAS.DataAccess.BaseDAModels
{

    /// <summary>
    /// SqlServer数据库版本。
    /// </summary>
    public enum SqlServerVersion
    {
        /// <summary>
        /// SqlServer2000
        /// </summary>
        SqlServer2000,
        /// <summary>
        /// SqlServer2005
        /// </summary>
        SqlServer2005,
        SqlServer2008
    }

    /// <summary>
    /// 自定义获取连接字符串的方式，例如：从缓存或Session中取得。
    /// </summary>
    public delegate string DelegateCustomGetConnectionString(string connectionName);
    /// <summary>
    /// 数据库配置参数。
    /// </summary>
    public class SqlServerSet
    {
        /// <summary>
        /// 单件实例。
        /// </summary>
        private static readonly SqlServerSet _sqlServerSet = new SqlServerSet();

        /// <summary>
        /// 隐藏默认构造方法
        /// </summary>
        private SqlServerSet()
        {
        }

        /// <summary>
        /// FXT数据源访问实例。
        /// </summary>
        public static SqlServerSet Instance
        {
            get
            {
                lock (_sqlServerSet)
                {
                    return _sqlServerSet;
                }
            }
        }
        private static string connectionName;
        public static string ConnectionName
        {
            get
            {
                if (string.IsNullOrEmpty(connectionName))
                {
                    connectionName = "FXTConnectionString";
                }
                return connectionName;
            }
            set
            {
                connectionName = value;
            }
        }
        /// <summary>
        /// 自定义获取连接字符串的方式，例如：从缓存或Session中取得。
        /// </summary>
        public static DelegateCustomGetConnectionString CustomGetConnectionString;
        /// <summary>
        /// 默认从.config文件中获取连接字符串，如果要以其它方式获取连接字符串则需要实现委托DelegateCustomGetConnectionString
        /// </summary>
        public static string GetConnectionString(string connectionName)
        {
            string result = "";
            if (null == CustomGetConnectionString)
            {
                result = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }
            else
            {
                result = CustomGetConnectionString(connectionName);
            }
            return result;
        }
        public static string GetConnectionString()
        {
            return GetConnectionString(ConnectionName);
        }

       
        
        /// <summary>
        /// 数据库版本。
        /// </summary>
        public SqlServerVersion FXTDataBaseVersion
        {
            get
            {
                lock (this)
                {
                    SqlServerVersion sqlServerVersion = SqlServerVersion.SqlServer2005;
                    Enum.TryParse<SqlServerVersion>(ConfigurationManager.AppSettings["FXTDataBaseVersion"], out sqlServerVersion);
                    return sqlServerVersion;
                }
            }
        }

        /// <summary>
        /// 测试数据库版本。
        /// </summary>
        public SqlServerVersion TestDataBaseVersion
        {
            get
            {
                lock (this)
                {
                    SqlServerVersion sqlServerVersion = SqlServerVersion.SqlServer2005;
                    Enum.TryParse<SqlServerVersion>(ConfigurationManager.AppSettings["TestDataBaseVersion"], out sqlServerVersion);
                    return sqlServerVersion;
                }
            }
        }
    }
}
