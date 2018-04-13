
//using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class RedisHashOtherHelper
    {
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        public bool Exist<T>(string hashId, string key)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    return Redis.HashContainsEntry(hashId, key);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|Key为：" + key);
                    return false;
                }
            }
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool Set<T>(string hashId, string key, T t)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    var value = JsonSerializer.SerializeToString<T>(t);//该方法字段值为null时会丢失这个字段
                    return Redis.SetEntryInHash(hashId, key, value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|Key为：" + key + "|Value为:" + JsonSerializer.SerializeToString<T>(t));
                    return false;
                }
            }
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public bool Set(string hashId, string key, string value)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    return Redis.SetEntryInHash(hashId, key, value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|Key为：" + key + "|Value为:" + value);
                    return false;
                }
            }
        }
        /// <summary>
        /// 批量存储数据到hash表
        /// </summary>
        public void Set(string hashId, List<KeyValuePair<string, string>> pairs)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    Redis.SetRangeInHash(hashId, pairs);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|pairs为：" + pairs);
                }
            }
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        public bool Remove(string hashId, string key)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    return Redis.RemoveEntryFromHash(hashId, key);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|Key为：" + key);
                    return false;
                }
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        public bool Remove(string key)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    return Redis.Remove(key);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：Key为：" + key);
                    return false;
                }
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        public T Get<T>(string hashId, string key)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    string value = Redis.GetValueFromHash(hashId, key);
                    if (string.IsNullOrEmpty(value))
                    {
                        return default(T);
                    }
                    return JsonSerializer.DeserializeFromString<T>(value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|Key为：" + key);
                    return default(T);
                }
            }
        }

        /// <summary>
        /// 获取hash表的数据数量
        /// </summary>  
        public long Count(string hashId)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    return Redis.GetHashCount(hashId);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 从hash表获取值
        /// </summary>
        public string Get(string hashId, string key)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    return Redis.GetValueFromHash(hashId, key);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|Key为：" + key);
                    return null;
                }
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        public List<T> GetAll<T>(string hashId)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
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
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId);
                    return new List<T>();
                }
            }
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        public void SetExpire(string hashId, DateTime datetime)
        {
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
                {
                    Redis.ExpireEntryAt(hashId, datetime);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|datetime:" + datetime);
                }
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
            try
            {
                List<string> hashKeys = new List<string>();
                using (var Redis = RedisOtherManager.GetClient(0))
                {
                    hashKeys = Redis.GetHashKeys(hashId);
                }
                if (hashKeys != null && hashKeys.Any() && !string.IsNullOrWhiteSpace(keywords))
                {
                    hashKeys = hashKeys.Where(o => o.StartsWith(keywords)).ToList();
                }
                return hashKeys;
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|keywords:" + keywords);
                return new List<string>();
            }
        }

        /// <summary>
        /// 指定哈希表搜索含keywords的key
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public List<string> GetContainsKeys(string hashId, string keywords)
        {
            try
            {
                List<string> hashKeys = new List<string>();
                using (var Redis = RedisOtherManager.GetClient(0))
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
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|keywords:" + keywords);
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
            using (var Redis = RedisOtherManager.GetClient(0))
            {
                try
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
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：HashID为：" + hashId + "|keys:" + keys);
                    return new List<T>();
                }
            }
        }
    }
}