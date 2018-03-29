using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseExport
{
    class ConfigHelper
    {
        //默认值
        //分页显示项目数量
        public static readonly int PageSize = 100;

        static ConfigHelper()
        {
            int result;
            if (int.TryParse(ConfigurationManager.AppSettings["PageSize"], out result))
            {
                PageSize = result;
            }


        }

        /// <summary>
        /// 获取多个数据库服务连接配置
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ConnectionStringSettings> GetConnectionStrings()
        {
            Dictionary<string, ConnectionStringSettings> dbConns = null;
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                ConnectionStringSettings item = ConfigurationManager.ConnectionStrings[i];
                if (0 == string.Compare("LocalSqlServer", item.Name) ||
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
