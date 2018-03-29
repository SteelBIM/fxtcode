using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Redis;

namespace CAS.Common
{
    public class RedisHelper
    {
        #region 各配置变量
        /// <summary>
        /// 是否存在redis配置
        /// </summary>
        public static bool ExistsConfig
        {
            get
            {
                string configInfo = System.Configuration.ConfigurationManager.AppSettings["RedisIpInfo"];
                if (string.IsNullOrEmpty(configInfo))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(RedisIP))
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 其他项目的缓存前缀
        /// </summary>
        public static List<string> OtherPrefix
        {
            get
            {
                string otherPrefix = RedisConfig["otherPrefix"];
                if (!string.IsNullOrEmpty(otherPrefix))
                {
                    List<string> list = otherPrefix.Split('|').ToList();
                    return list.Where(_obj => _obj.ToLower() != KeyPrefix.ToLower() && !string.IsNullOrEmpty(_obj)).Distinct().ToList();
                }
                return new List<string>();
            }
        }
        /// <summary>
        /// 是否用的redis集群
        /// </summary>
        static bool IsCodis
        {
            get
            {
                if (ExistsConfig)
                {
                    if (RedisConfig["codis"] == "1")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 获取缓存配置
        /// </summary>
        static Dictionary<string, string> RedisConfig
        {
            get
            {
                Dictionary<string, string> redisConfig = new Dictionary<string, string>();
                redisConfig["ip"] = "";
                redisConfig["pwd"] = "";
                redisConfig["prefix"] = "";
                redisConfig["maxWritePoolSize"] = "";
                redisConfig["maxReadPoolSize"] = "";
                redisConfig["autoStart"] = "true";
                redisConfig["codis"] = "0";
                redisConfig["otherPrefix"] = "";
                string configInfo = System.Configuration.ConfigurationManager.AppSettings["RedisIpInfo"];
                if (!string.IsNullOrEmpty(configInfo))
                {
                    List<string> configInfos = configInfo.Split(',').ToList();
                    foreach (string str in configInfos)
                    {
                        string[] strings = str.Split('=');
                        redisConfig[strings[0]] = strings[1];
                    }
                }
                return redisConfig;
            }
        }
        /// <summary>
        /// 缓存服务器的IP
        /// </summary>
        static string RedisIP
        {
            get
            {

                string ipInfo = RedisConfig["ip"];                
                if (!string.IsNullOrEmpty(ipInfo))
                {
                    string ip = ipInfo.Split(':')[0];
                    return ip;
                }
                return "";
            }
        }
        /// <summary>
        /// 缓存服务器的端口
        /// </summary>
        static int RedisPort
        {
            get
            {
                string ipInfo = RedisConfig["ip"];
                if (!string.IsNullOrEmpty(ipInfo))
                {
                    int port = Convert.ToInt32(ipInfo.Split(':')[1]);
                    return port;
                }
                return 0;
            }
        }
        /// <summary>
        /// 缓存服务器的密码
        /// </summary>
        static string RedisPWD
        {
            get
            {
                return RedisConfig["pwd"];
            }
        }
        /// <summary>
        /// 为了区分各产品的key所定义的key的前缀
        /// </summary>
        public static string KeyPrefix
        {
            get
            {
                return RedisConfig["prefix"];
            }
        }


        /// <summary>
        /// “写”链接池链接数
        /// </summary>
        static int MaxWritePoolSize
        {
            get
            {
                string maxWritePoolSize = RedisConfig["maxWritePoolSize"];
                if (!string.IsNullOrEmpty(maxWritePoolSize))
                {
                    return Convert.ToInt32(maxWritePoolSize);
                }
                return 2000;
            }
        }
        /// <summary>
        /// “读”链接池链接数
        /// </summary>
        static int MaxReadPoolSize
        {
            get
            {
                string maxReadPoolSize = RedisConfig["maxReadPoolSize"];
                if (!string.IsNullOrEmpty(maxReadPoolSize))
                {
                    return Convert.ToInt32(maxReadPoolSize);
                }
                return 2000;
            }
        }
        /// <summary>
        /// 自动重启
        /// </summary>
        static bool AutoStart
        {
            get
            {
                string autoStart = RedisConfig["autoStart"];
                if (!string.IsNullOrEmpty(autoStart))
                {
                    return autoStart == "true" ? true : false;
                }
                return true;
            }
        }
        #endregion
        static PooledRedisClientManager prcm = null;
            
        static RedisHelper()
        {
            if (ExistsConfig)
            {
                prcm = CreateManager();
            }
        }
        /// <summary>
        /// 获取一个客户端链接
        /// </summary>
        /// <returns></returns>
        static IRedisClient RedisObj()
        {
            if (prcm == null)
            {
                prcm = CreateManager();
            }
            IRedisClient r = prcm.GetClient(); //!string.IsNullOrEmpty(RedisIP) && RedisPort > 0 ? new RedisClient(RedisIP, RedisPort) : null;
            if (r != null)
            {
                if (r != null && !string.IsNullOrEmpty(RedisPWD))
                {
                    r.Password = RedisPWD;
                }

            }
            return r;
        }
        /// <summary>
        /// 创建一个链接池
        /// </summary>
        /// <returns></returns>
        static PooledRedisClientManager CreateManager()
        {
            //支持读写分离，均衡负载
            return new PooledRedisClientManager(new string[] { RedisIP + ":" + RedisPort }, new string[] { RedisIP + ":" + RedisPort }, new RedisClientManagerConfig
            {
                MaxWritePoolSize = MaxWritePoolSize,//“写”链接池链接数
                MaxReadPoolSize = MaxReadPoolSize,//“读”链接池链接数
                AutoStart = AutoStart,
            });
        }
        /// <summary>
        /// 返回指定的缓存，如果不存在将返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        /// <returns></returns>
        public static T Get<T>(string key, string otherKeyPrefix)
        {
            T t;
            using (IRedisClient r = RedisObj())
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                t = r.Get<T>(keyPrefix + key);
                //r.Quit();
            }
            return t;
        }
        /// <summary>
        /// 添加缓存，默认相对时间一天
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Set<T>(string key, T value, string otherKeyPrefix)
        {
            //set<T>(key, value);
            Set<T>(key, value, CacheTime.Default, CacheExpiresType.Sliding, otherKeyPrefix);
        }
        /// <summary>
        /// 移除指定的缓存
        /// </summary>
        /// <param name="key">缓存的key</param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Remove(string key, string otherKeyPrefix)
        {
            try
            {
                using (IRedisClient r = RedisObj())
                {
                    string _kp = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                    removeKeyList(r, _kp + key, _kp);
                    //r.Quit();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, message: string.Format("redis异常，方法:{0},p:{1}", "Remove", key ?? ""));
            }
        }
        /// <summary>
        /// 清除系统中当前产品应用程序中的所有缓存
        /// </summary>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Clear(string otherKeyPrefix)
        {
            try
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                using (IRedisClient r = RedisObj())
                {
                    Func<string, bool> checkFunc = delegate(string str)
                    {
                        string[] strs = str.Split(new string[] { keyPrefix }, StringSplitOptions.None);
                        if (strs == null || strs.Length < 2)
                        {
                            return false;
                        }
                        if (string.IsNullOrEmpty(strs[0]))
                        {
                            return true;
                        }
                        return false;
                    };
                    List<string> keys = getKeyList(r, otherKeyPrefix);
                    keys = keys.Where(_str => checkFunc(_str) == true).ToList();
                    removeKeyList(r, keys, otherKeyPrefix);
                    //r.Quit();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, message: string.Format("redis异常，方法:{0}", "Clear"));
            }
        }

        /// <summary>
        /// 获取当前产品中的所有缓存信息(返回的key中去除了产品前缀)
        /// </summary>
        /// <returns></returns>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static Dictionary<string, object> GetAllCache(string otherKeyPrefix)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                IDictionary<string, object> idic = new Dictionary<string, object>();

                using (IRedisClient r = RedisObj())
                {
                    Func<string, bool> checkFunc = delegate(string str)
                    {
                        string[] strs = str.Split(new string[] { keyPrefix }, StringSplitOptions.None);
                        if (strs == null || strs.Length < 2)
                        {
                            return false;
                        }
                        if (string.IsNullOrEmpty(strs[0]))
                        {
                            return true;
                        }
                        return false;
                    };
                    List<string> keys = getKeyList(r, keyPrefix);
                    keys = keys.Where(_str => checkFunc(_str) == true).ToList();
                    idic = r.GetAll<object>(keys);
                    //r.Quit();
                }
                if (idic != null)
                {
                    foreach (var keyStr in idic)
                    {
                        if (keyStr.Value != null)
                        {
                            List<string> nKeys = keyStr.Key.Split(new string[] { keyPrefix }, StringSplitOptions.None).ToList();
                            nKeys.RemoveAt(0);
                            string nKey = string.Join("", nKeys);
                            dic[nKey] = keyStr.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, message: string.Format("redis异常，方法:{0}", "GetAllCache"));
            }
            return dic;
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">相对时间有效期多少久</param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Set<T>(string key, T value, CacheTime cacheTime, string otherKeyPrefix)
        {
            Set<T>(key, value, cacheTime, CacheExpiresType.Sliding, otherKeyPrefix);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">有效期类型</param>
        /// <param name="cacheExpiresType">是绝对时间还是相对时间</param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Set<T>(string key, T value, CacheTime cacheTime, CacheExpiresType cacheExpiresType, string otherKeyPrefix)
        {
            DateTime absoluteExpiration = GetAbsoluteExpiration(cacheTime, cacheExpiresType);
            TimeSpan slidingExpiration = GetSlidingExpiration(cacheTime, cacheExpiresType);
            if (cacheTime != CacheTime.NotRemovable)
            {
                if (cacheExpiresType == CacheExpiresType.Absolute)
                {
                    set<T>(key, value, absoluteExpiration, otherKeyPrefix);
                }
                else if (cacheExpiresType == CacheExpiresType.Sliding)
                {
                    set<T>(key, value, slidingExpiration, otherKeyPrefix);
                }
            }
            else
            {

                Set<T>(key, value, otherKeyPrefix);
            }
            
        }
        
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">有效期相对时间多久，单位：分钟</param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Set<T>(string key, T value, int cacheTime, string otherKeyPrefix)
        {
            Set<T>(key, value, cacheTime, CacheExpiresType.Sliding, otherKeyPrefix);

        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">时长，单位分钟</param>
        /// <param name="cacheExpiresType">相对时间还是绝对时间</param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        public static void Set<T>(string key, T value, int cacheTime, CacheExpiresType cacheExpiresType,string otherKeyPrefix)
        {
            if (cacheExpiresType == CacheExpiresType.Sliding)
            {
                set<T>(key, value, TimeSpan.FromMinutes(cacheTime), otherKeyPrefix);
            }
            else
            {
                set<T>(key, value, DateTime.Now.AddMinutes(cacheTime), otherKeyPrefix);
            }

        }

        /// <summary>
        /// 检查系统中是否存在指定的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        /// <returns></returns>
        public static bool Contains<T>(string key, string otherKeyPrefix)
        {
            return containsKey(key, otherKeyPrefix); ;
        }



        #region 已封装的私有方法


        /// <summary>
        /// (内部方法)检查系统中是否存在指定的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        /// <returns></returns>
        static bool containsKey(string key,string otherKeyPrefix)
        {
            bool result = false;
            using (IRedisClient r = RedisObj())
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                result = r.ContainsKey(keyPrefix + key);
                //r.Quit();
            }
            return result;
        }
        /// <summary>
        /// (内部方法)设置绝对时间缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        static void set<T>(string key, T value, DateTime cacheTime, string otherKeyPrefix)
        {
            using (IRedisClient r = RedisObj())
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                setKeyList(r, keyPrefix + key, keyPrefix);
                r.Set<T>(keyPrefix + key, value, cacheTime);
                //r.Quit();
            }
        }
        /// <summary>
        /// (内部方法)设置相对时间缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        static void set<T>(string key, T value, TimeSpan cacheTime, string otherKeyPrefix)
        {
            using (IRedisClient r = RedisObj())
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                setKeyList(r, keyPrefix + key, keyPrefix);
                r.Set<T>(keyPrefix + key, value, cacheTime);
                //r.Quit();
            }
        }
        /// <summary>
        /// (内部方法)设置永久缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        static void set<T>(string key, T value, string otherKeyPrefix)
        {
            using (IRedisClient r = RedisObj())
            {
                string keyPrefix = string.IsNullOrEmpty(otherKeyPrefix) ? KeyPrefix : otherKeyPrefix;
                setKeyList(r, keyPrefix + key, keyPrefix);
                r.Set<T>(keyPrefix + key, value);
                //r.Quit();
            }
        }
        static DateTime GetAbsoluteExpiration(CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (cacheTime == CacheTime.NotRemovable || cacheExpiresType == CacheExpiresType.Sliding)
                return System.Web.Caching.Cache.NoAbsoluteExpiration;

            switch (cacheTime)
            {
                case CacheTime.Short:
                    return DateTime.Now.AddMinutes(20);
                default:
                case CacheTime.Default:
                case CacheTime.Normal:
                    return DateTime.Now.AddMinutes(60 * 24);//一天
                case CacheTime.Long:
                    return DateTime.Now.AddMinutes(60 * 24 * 365);//一年
            }
        }

        static TimeSpan GetSlidingExpiration(CacheTime cacheTime, CacheExpiresType cacheExpiresType)
        {
            if (cacheTime == CacheTime.NotRemovable || cacheExpiresType == CacheExpiresType.Absolute)
                return System.Web.Caching.Cache.NoSlidingExpiration;

            switch (cacheTime)
            {
                case CacheTime.Short:
                    return TimeSpan.FromMinutes(20);
                default:
                case CacheTime.Default:
                case CacheTime.Normal:
                    return TimeSpan.FromMinutes(60 * 24);//一天
                case CacheTime.Long:
                    return TimeSpan.FromMinutes(60 * 24 * 365);//一年
            }
        }

        #region 如果redis是用的集群那就不支持获取所有key的方式，只能自己手动把key存得到缓存里

        /// <summary>
        /// 集群里缓存当前站点key列表的名字
        /// </summary>
        static string listName ="keyList";
        /// <summary>
        /// 集群里缓存当前站点key列表的key
        /// </summary>
        static string listKey = KeyPrefix + listName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="key"></param>
        static void setKeyList(IRedisClient r, string key, string otherKeyPrefix)
        {
            if (IsCodis)
            {
                string _listKey = string.IsNullOrEmpty(otherKeyPrefix) ? listKey : otherKeyPrefix + listName;
                List<string> keyList = r.Get<List<string>>(_listKey);
                if (keyList == null || keyList.Count < 1)
                {
                    keyList = new List<string>();
                }
                if (!keyList.Contains(key))
                {
                    keyList.Add(key);
                    r.Set<List<string>>(_listKey, keyList);
                }
            }
        }
        /// <summary>
        /// 获取所有key,根据是否是集群判断
        /// </summary>
        /// <param name="r"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        /// <returns></returns>
        static List<string> getKeyList(IRedisClient r, string otherKeyPrefix)
        {
            string _listKey = string.IsNullOrEmpty(otherKeyPrefix) ? listKey : otherKeyPrefix + listName;
            List<string> keyList = new List<string>();
            if (IsCodis)
            {
                keyList = r.Get<List<string>>(_listKey);
                if (keyList == null || keyList.Count < 1)
                {
                    keyList = new List<string>();
                }
            }
            else
            {
                keyList = r.GetAllKeys();
            }
            return keyList;
        }
        /// <summary>
        /// 移除key，根据是否是集群判断
        /// </summary>
        /// <param name="r"></param>
        /// <param name="key"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        static void removeKeyList(IRedisClient r, string key, string otherKeyPrefix)
        {
            List<string> keys = new List<string>();
            keys.Add(key);
            removeKeyList(r, keys, otherKeyPrefix);
        }
        /// <summary>
        /// 批量移除key，根据是否是集群判断
        /// </summary>
        /// <param name="r"></param>
        /// <param name="keys"></param>
        /// <param name="otherKeyPrefix">其他项目的前缀，null则获取当前项目的前缀</param>
        static void removeKeyList(IRedisClient r, List<string> keys, string otherKeyPrefix)
        {
            string _listKey = !string.IsNullOrEmpty(otherKeyPrefix) ? (otherKeyPrefix + listName) : listKey;
            if (keys != null & keys.Count > 0)
            {
                r.RemoveAll(keys);
                if (IsCodis)
                {
                    List<string> keyList = r.Get<List<string>>(_listKey);
                    foreach (string str in keys)
                    {
                        keyList.Remove(str);
                    }
                    r.Set<List<string>>(_listKey, keyList);
                }
            }
        }
        #endregion

        #endregion
    }
}
