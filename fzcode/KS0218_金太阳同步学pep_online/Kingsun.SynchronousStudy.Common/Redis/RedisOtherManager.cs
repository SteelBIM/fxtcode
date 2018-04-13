using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common 
{
    public  class RedisOtherManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisOtherConfigInfo redisConfigInfo = RedisOtherConfigInfo.GetConfig();

        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisOtherManager()
        {
            CreateManager(1);
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager(int wr)
        {
            string[] writeServerList = SplitString(redisConfigInfo.WriteServerList, ",");
            string[] readServerList = SplitString(redisConfigInfo.ReadServerList, ",");

            if (wr == 1)
            {
                readServerList = writeServerList;
            }
            else
            {
                writeServerList = readServerList;
            }

            prcm = new PooledRedisClientManager(readServerList, writeServerList,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                                 MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                                 AutoStart = redisConfigInfo.AutoStart,
                             });
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient(int wr)
        {
            if (prcm == null)

                CreateManager(wr);

            return prcm.GetClient();
        }

    }
}
