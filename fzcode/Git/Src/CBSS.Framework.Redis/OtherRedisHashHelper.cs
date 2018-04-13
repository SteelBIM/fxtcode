
//using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Redis
{
    public class OtherRedisHashHelper:RedisBase
    {
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        public bool Exist<T>(string hashId, string key)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.HashContainsEntry(hashId, key);
            }
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool Set<T>(string hashId, string key, T t)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
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
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.SetEntryInHash(hashId, key, value);
            }
        }
        /// <summary>
        /// 批量存储数据到hash表
        /// </summary>
        public void Set(string hashId, List<KeyValuePair<string, string>> pairs)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                Redis.SetRangeInHash(hashId, pairs);
            }
        }


        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        public bool Remove(string hashId, string key)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.RemoveEntryFromHash(hashId, key);
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        public bool Remove(string key)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.Remove(key);
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        public T Get<T>(string hashId, string key)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                string value = Redis.GetValueFromHash(hashId, key);
                if (string.IsNullOrEmpty(value))
                {
                    return default(T);
                }
                return JsonSerializer.DeserializeFromString<T>(value);
            }
        }

        /// <summary>
        /// 获取hash表的数据数量
        /// </summary>  
        public long Count(string hashId)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.GetHashCount(hashId);
            }
        }

        /// <summary>
        /// 从hash表获取值
        /// </summary>
        public string Get(string hashId, string key)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.GetValueFromHash(hashId, key);
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        public List<T> GetAll<T>(string hashId)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                var result = new List<T>();
                var list = Redis.GetHashValues(hashId);
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
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                Redis.ExpireEntryAt(hashId, datetime);
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
            using (var Redis = OtherRedisManager.GetClient(0))
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
            using (var Redis = OtherRedisManager.GetClient(0))
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

        /// <summary>
        /// 批量获取哈希表的数据
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<T> GetValues<T>(string hashId, List<string> keys)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
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

        public OtherRedisHashHelper(string name) : base(name)
        {
        }
    }
}