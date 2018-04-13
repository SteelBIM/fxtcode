using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Framework.DAL
{
    public class StackRedisHelper
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object _locker = new object();

        /// <summary>
        /// StackExchange.Redis对象
        /// </summary>
        private static ConnectionMultiplexer instance;

        /// <summary>
        /// 得到StackExchange.Redis单例对象
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_locker)
                    {
                        if (instance != null)
                            return instance;

                        instance = GetManager();
                        return instance;
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// 构建链接,返回对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ConnectionMultiplexer GetManager()
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["RedisPath"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("请配置Redis连接串！");
            }
            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}
