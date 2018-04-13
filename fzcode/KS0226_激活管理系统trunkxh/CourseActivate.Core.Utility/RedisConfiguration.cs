using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Core.Utility
{
    public class RedisConfiguration : ConfigurationSection
    {

        public static RedisConfiguration GetConfig()
        {
            RedisConfiguration section = GetConfig("RedisConfig");
            return section;
        }

        public static RedisConfiguration GetConfig(string sectionName)
        {
            RedisConfiguration section = (RedisConfiguration)ConfigurationManager.GetSection(sectionName);
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }
        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        [ConfigurationProperty("WriteServerList", IsRequired = false)]
        public string WriteServerConStr
        {
            get
            {
                return (string)base["WriteServerList"];
            }
            set
            {
                base["WriteServerList"] = value;
            }
        }


        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        [ConfigurationProperty("ReadServerList", IsRequired = false)]
        public string ReadServerConStr
        {
            get
            {
                return (string)base["ReadServerList"];
            }
            set
            {
                base["ReadServerList"] = value;
            }
        }
        /// <summary>
        /// 最大写链接数
        /// </summary>
        [ConfigurationProperty("MaxWritePoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxWritePoolSize
        {
            get
            {
                int _maxWritePoolSize = (int)base["MaxWritePoolSize"];
                return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
            }
            set
            {
                base["MaxWritePoolSize"] = value;
            }
        }


        /// <summary>
        /// 最大读链接数
        /// </summary>
        [ConfigurationProperty("MaxReadPoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxReadPoolSize
        {
            get
            {
                int _maxReadPoolSize = (int)base["MaxReadPoolSize"];
                return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
            }
            set
            {
                base["MaxReadPoolSize"] = value;
            }
        }


        /// <summary>
        /// 自动重启
        /// </summary>
        [ConfigurationProperty("AutoStart", IsRequired = false, DefaultValue = true)]
        public bool AutoStart
        {
            get
            {
                return (bool)base["AutoStart"];
            }
            set
            {
                base["AutoStart"] = value;
            }
        }



        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        [ConfigurationProperty("LocalCacheTime", IsRequired = false, DefaultValue = 36000)]
        public int LocalCacheTime
        {
            get
            {
                return (int)base["LocalCacheTime"];
            }
            set
            {
                base["LocalCacheTime"] = value;
            }
        }


        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        [ConfigurationProperty("RecordeLog", IsRequired = false, DefaultValue = false)]
        public bool RecordeLog
        {
            get
            {
                return (bool)base["RecordeLog"];
            }
            set
            {
                base["RecordeLog"] = value;
            }
        }

        public static string BatchKey { get { return "Batch:"; } }

        public static string GetBatchKey(int batchid)
        {
            return BatchKey + batchid;
        }

        public static string ActivateKey { get { return "ActivateCode:"; } }

        public static string GetActivateKey(string activatecode)
        {
            return ActivateKey + activatecode;
        }

        public static string ActivateTypeKey { get { return "ActivateType:"; } }

        public static string GetActivateTypeKey(int typeid)
        {
            return ActivateTypeKey + typeid;
        }

        public static string ActivateUseKey { get { return "ActivateUse:"; } }

        public static string GetActivateUseKey(int activateid)
        {
            return ActivateUseKey + activateid;
        }

        public static string ActivateUseDeviceKey { get { return "ActivateUseDevice:"; } }

        public static string GetActivateUseDeviceKey(Guid activateuseid)
        {
            return ActivateUseDeviceKey + activateuseid;
        }

        public static string ResourceKey { get { return "Resource:"; } }

        public static string GetResourceKey(int CourseID, int modulerid)
        {
            return ResourceKey + CourseID + ":" + modulerid;
        }

        public static string BatchBookKey { get { return "BatchBook:"; } }

        public static string GetBatchBookKey(int batchid)
        {
            return BatchBookKey + batchid;
        }

        public static string AppVersionKey { get { return "AppVersion:"; } }

        public static string GetAppVersionKey(Guid appID)
        {
            return AppVersionKey + appID;
        }

        public static string ActivateStatiscKey { get { return "ActivateStatisc:"; } }

        public static string GetActivateStatiscKey(string k)
        {
            return ActivateStatiscKey + k;
        }
        public static string SyncDBKey { get { return "SyncDBKey:"; } }

        public static string GetSyncDBKey(string k)
        {
            return SyncDBKey + k;
        }
    }
}
