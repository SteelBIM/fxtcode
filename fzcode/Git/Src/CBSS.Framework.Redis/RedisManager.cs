using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CBSS.Framework.Redis
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisManager”的 XML 注释
    public class RedisManager
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisManager”的 XML 注释
    {

        #region TbxManager
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig("Tbx");
        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
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
                                 MaxWritePoolSize = Convert.ToInt32(redisConfigInfo.MaxWritePoolSize),
                                 MaxReadPoolSize = Convert.ToInt32(redisConfigInfo.MaxReadPoolSize),
                                 AutoStart = redisConfigInfo.AutoStart == "false" ? false : true,
                             });
        }
        #endregion

        #region IBSManager
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfoIBS = RedisConfigInfo.GetConfig("IBS");
        private static PooledRedisClientManager prcmIBS;

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManagerIBS(int wr)
        {
            string[] writeServerList = SplitString(redisConfigInfoIBS.WriteServerList, ",");
            string[] readServerList = SplitString(redisConfigInfoIBS.ReadServerList, ",");

            if (wr == 1)
            {
                readServerList = writeServerList;
            }
            else
            {
                writeServerList = readServerList;
            }

            prcmIBS = new PooledRedisClientManager(readServerList, writeServerList,
                             new RedisClientManagerConfig
                             {
                                 MaxWritePoolSize = Convert.ToInt32(redisConfigInfoIBS.MaxWritePoolSize),
                                 MaxReadPoolSize = Convert.ToInt32(redisConfigInfoIBS.MaxReadPoolSize),
                                 AutoStart = redisConfigInfoIBS.AutoStart == "false" ? false : true,
                             });
        }
        #endregion

        //#region CBSSManager
        ///// <summary>
        ///// redis配置文件信息
        ///// </summary>
        //private static RedisConfigInfo redisConfigInfoCBSS = RedisConfigInfo.GetConfig("CBSS");
        //private static PooledRedisClientManager prcmCBSS;

        ///// <summary>
        ///// 创建链接池管理对象
        ///// </summary>
        //private static void CreateManagerCBSS(int wr)
        //{
        //    string[] writeServerList = SplitString(redisConfigInfoCBSS.WriteServerList, ",");
        //    string[] readServerList = SplitString(redisConfigInfoCBSS.ReadServerList, ",");

        //    if (wr == 1)
        //    {
        //        readServerList = writeServerList;
        //    }
        //    else
        //    {
        //        writeServerList = readServerList;
        //    }

        //    prcmCBSS = new PooledRedisClientManager(readServerList, writeServerList,
        //                     new RedisClientManagerConfig
        //                     {
        //                         MaxWritePoolSize = Convert.ToInt32(redisConfigInfoCBSS.MaxWritePoolSize),
        //                         MaxReadPoolSize = Convert.ToInt32(redisConfigInfoCBSS.MaxReadPoolSize),
        //                         AutoStart = redisConfigInfoCBSS.AutoStart == "false" ? false : true,
        //                     });
        //}
        //#endregion

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient(int wr,string managerName)
        { 
            IRedisClient result;
            switch (managerName)
            {
                case "Tbx":
                    if (prcm == null)
                        CreateManager(wr);
                    result = prcm.GetClient();
                    break;
                case "IBS":
                    if (prcmIBS == null)
                        CreateManagerIBS(wr);
                    result = prcmIBS.GetClient();
                    break;
                default:
                    if (prcm == null)
                        CreateManager(wr);
                    result = prcm.GetClient();
                    break;
            }
            return result;
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

    }
}
