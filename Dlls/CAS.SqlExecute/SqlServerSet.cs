using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CAS.SqlExecute
{
    /// <summary>
    /// 数据库配置参数。
    /// </summary>
    internal class SqlServerSet
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

        /// <summary>
        /// FXT数据库链接。
        /// </summary>
        public string FXTConnectionString
        {
            get
            {
                lock (this)
                {
                    return ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["FXTConnectionString"]].ConnectionString;
                }
            }
        }

        /// <summary>
        /// 测试数据库链接。
        /// </summary>
        public string TestConnectionString
        {
            get
            {
                lock (this)
                {
                    return ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["TestConnectionString"]].ConnectionString;
                }
            }
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
