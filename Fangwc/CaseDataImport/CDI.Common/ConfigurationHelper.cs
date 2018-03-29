using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDI.Common
{
    public static class ConfigurationHelper
    {


        //默认值
        /// <summary>
        /// 用户登录时长,默认30*24小时, 30天后需重新登录
        /// </summary>
        public static readonly int SessionTimeout = 720;

        static ConfigurationHelper()
        {
            int result;
            if (int.TryParse(ConfigurationManager.AppSettings["SessionTimeout"], out result))
            {
                SessionTimeout = result;
            }
            
        }
        /// <summary>
        /// 获取多个数据库服务连接配置，不包含本程序使用的FxtProduct连接配置。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ConnectionStringSettings> GetConnectionStrings()
        {
            Dictionary<string, ConnectionStringSettings> dbConns = null;
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                ConnectionStringSettings item = ConfigurationManager.ConnectionStrings[i];
                if (0 == string.Compare("FxtProduct", item.Name) ||
                    0 == string.Compare("LocalSqlServer", item.Name) ||
                    item.ConnectionString.Contains("|DataDirectory|"))
                {
                    continue;
                }
                if (dbConns == null)
                {
                    dbConns = new Dictionary<string, ConnectionStringSettings>();
                }
                dbConns.Add(item.Name, item);
            }

            return dbConns;
        }


    }
}
