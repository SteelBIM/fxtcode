using CBSS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CBSS.Framework.Redis
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisConfigInfo”的 XML 注释
    public class RedisConfigInfo : ConfigurationSection
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisConfigInfo”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisConfigInfo.GetConfig()”的 XML 注释
        public static RedisConfigInfo GetConfig()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisConfigInfo.GetConfig()”的 XML 注释
        {
            return XMLHelper.Read<RedisConfigInfo>("Config/RedisConfig.xml", "RedisConfig")["Tbx"];
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisConfigInfo.GetConfig(string)”的 XML 注释
        public static RedisConfigInfo GetConfig(string managerName)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisConfigInfo.GetConfig(string)”的 XML 注释
        {
            return XMLHelper.Read<RedisConfigInfo>("Config/RedisConfig.xml", "RedisConfig")[managerName];
        }

        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        public string WriteServerList { get; set; }


        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        public string ReadServerList { get; set; }

        /// <summary>
        /// 最大写链接数
        /// </summary>
        public string MaxWritePoolSize { get; set; } //int


        /// <summary>
        /// 最大读链接数
        /// </summary>
        public string MaxReadPoolSize { get; set; } //int

        /// <summary>
        /// 自动重启
        /// </summary>
        public string AutoStart { get; set; }

        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        public string LocalCacheTime { get; set; }//int

        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        public string RecordeLog { get; set; }
    }
}
