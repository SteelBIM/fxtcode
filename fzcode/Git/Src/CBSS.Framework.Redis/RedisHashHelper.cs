
//using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Redis
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisHashHelper”的 XML 注释
    public class RedisHashHelper
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisHashHelper”的 XML 注释
    {
        private string _managerName { get; set; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisHashHelper.RedisHashHelper(string)”的 XML 注释
        public RedisHashHelper(string managerName)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisHashHelper.RedisHashHelper(string)”的 XML 注释
        {
            _managerName = managerName;
        }

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        public bool Exist<T>(string hashId, string key)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.HashContainsEntry(hashId, key);
            }
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool Set<T>(string hashId, string key, T t)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                var value = JsonSerializer.SerializeToString<T>(t);//该方法字段值为null时会丢失这个字段
                return Redis.SetEntryInHash(hashId, key, value);
            }
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool Set(string hashId, string key, string value)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.SetEntryInHash(hashId, key, value);
            }
        }
        /// <summary>
        /// 批量存储数据到hash表
        /// </summary>
        public void Set(string hashId, List<KeyValuePair<string, string>> pairs)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                Redis.SetRangeInHash(hashId, pairs);
            }
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        public bool Remove(string hashId, string key)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.RemoveEntryFromHash(hashId, key);
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        public bool Remove(string key)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.Remove(key);
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        public T Get<T>(string hashId, string key)
        {
            string value;
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                value = Redis.GetValueFromHash(hashId, key);
            }
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return default(T);
                }
                return JsonSerializer.DeserializeFromString<T>(value);
            }
            catch (Exception ex)
            {
                throw new Exception("获取redis数据异常:hashId=" + hashId + ",key=" + key + ",value=" + value + ",异常:" + ex.Message + ex.StackTrace);
            }

        }

        /// <summary>
        /// 获取hash表的数据数量
        /// </summary>  
        public long Count(string hashId)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetHashCount(hashId);
            }
        }

        /// <summary>
        /// 从hash表获取值
        /// </summary>
        public string Get(string hashId, string key)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetValueFromHash(hashId, key);
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        public List<T> GetAll<T>(string hashId)
        {
            var result = new List<T>();
            List<String> list;
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                list = Redis.GetHashValues(hashId);
            }
            if (list != null && list.Count > 0)
            {
                list.ForEach(x =>
                {
                    var value = JsonSerializer.DeserializeFromString<T>(x);
                    result.Add(value);
                });
            }
            return result;
        }

        /// <summary>
        /// 批量获取哈希表的数据
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string hashId, List<string> keys)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                var result = new List<T>();
                var list = Redis.GetValuesFromHash(hashId, keys.ToArray());
                if (list != null && list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        var value = JsonSerializer.DeserializeFromString<T>(x);
                        result.Add(value);
                    });
                }
                return result;
            }
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        public void SetExpire(string hashId, DateTime datetime)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                Redis.ExpireEntryAt(hashId, datetime);
            }
        }

        /// <summary>
        /// 设置缓存过期(时间段)
        /// </summary>
        public void SetExpire(string hashId, TimeSpan timeSpan)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                Redis.ExpireEntryIn(hashId, timeSpan);
            }
        }

        /// <summary>
        /// 指定哈希表搜索以keywords开头的key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public List<string> GetStartWithKeys(string hashId, string keywords)
        {
            List<string> hashKeys = new List<string>();
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                hashKeys = Redis.GetHashKeys(hashId);
            }
            if (hashKeys != null && hashKeys.Any() && !string.IsNullOrWhiteSpace(keywords))
            {
                hashKeys = hashKeys.Where(o => o.StartsWith(keywords)).ToList();
            }
            return hashKeys;
        }

        /// <summary>
        /// 指定哈希表搜索含keywords的key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public List<string> GetContainsKeys(string hashId, string keywords)
        {
            List<string> hashKeys = new List<string>();
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                hashKeys = Redis.GetHashKeys(hashId);
            }
            if (hashKeys != null && hashKeys.Any())
            {
                return hashKeys.Where(o => o.Contains(keywords)).ToList();
            }
            else
            {
                return new List<string>();
            }
        }


    }
}