using CourseActivate.Core.Utility;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Framework.DAL
{
    public class RedisHelper
    {
        string _Connect = "";
        int DefaultDb = 1;
        public RedisHelper(string connect)
        {
            _Connect = connect;
        }

        public List<string> GetList(string key)
        {
            using (var Core = RedisHelperRepository.GetClient())
            {
                Core.Db = DefaultDb;
                return Core.GetAllItemsFromList(key);
            }
        }

        /// <summary>
        /// 获取key中下标为star到end的值集合
        /// </summary>  
        public List<string> Get(string key, int star, int end)
        {
            using (var Core = RedisHelperRepository.GetClient())
            {
                Core.Db = DefaultDb;
                return Core.GetRangeFromList(key, star, end);
            }
        }

        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        public void LPush(string key, string value)
        {
            using (var Core = RedisHelperRepository.GetClient())
            {
                Core.Db = DefaultDb;
                Core.PushItemToList(key, value);
            }
        }



    }


    public class RedisHelperRepository
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfiguration RedisConfig = RedisConfiguration.GetConfig("SyncRedisConfig");

        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisHelperRepository()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] WriteServerConStr = SplitString(RedisConfig.WriteServerConStr, ",");
            string[] ReadServerConStr = SplitString(RedisConfig.ReadServerConStr, ",");
            prcm = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = RedisConfig.MaxWritePoolSize,
                                 MaxReadPoolSize = RedisConfig.MaxReadPoolSize,
                                 AutoStart = RedisConfig.AutoStart,
                                 DefaultDb=1,
                             });
            
            //prcm.ConnectTimeout = 500;
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }


    }
}
