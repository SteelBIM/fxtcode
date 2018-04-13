using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Framework.DAL
{
    #region 旧版本引用
    /*
    public class DoRedisString : RedisBase
    {
        #region 赋值
        /// <summary>
        /// 设置key的value
        /// </summary>
        public bool Set(string key, string value)
        {
            return Core.Set<string>(key, value);
        }
        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public bool Set(string key, string value, DateTime dt)
        {
            return Core.Set<string>(key, value, dt);
        }
        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public bool Set(string key, string value, TimeSpan sp)
        {
            return Core.Set<string>(key, value, sp);
        }

        /// <summary>
        /// 设置key的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
           
            return Core.Set<T>(key, value);
        }

        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public bool Set<T>(string key, T value, TimeSpan sp)
        {
            return Core.Set<T>(key, value, sp);
        }

        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public void Set<T>(Dictionary<string, T> dic)
        {
            Core.SetAll<T>(dic);
        }


        /// <summary>
        /// 设置多个key/value
        /// </summary>
        public void Set(Dictionary<string, string> dic)
        {
            Core.SetAll(dic);
        }

        #endregion

        #region 获取值
        /// <summary>
        /// 获取key的value值
        /// </summary>
        public string Get(string key)
        {
            return Core.GetValue(key);
        }
        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        public List<string> Get(List<string> keys)
        {
            return Core.GetValues(keys);
        }
        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        public List<T> Get<T>(List<string> keys)
        {
            return Core.GetValues<T>(keys);
        }
        /// <summary>
        /// 获取key的value值
        /// </summary>
        public T Get<T>(string key)
        {
            
            return Core.Get<T>(key);
        }

        public List<T> GetAll<T>(string key)
        {
            List<string> lk = new List<string>();
            lk.Add(key);
            IDictionary<string, T> dic = Core.GetAll<T>(lk);
            if (dic == null)
                return new List<T>();
            return dic.Values.ToList<T>();
        }

        public List<T> GetAll<T>(List<string> key)
        {
            IDictionary<string, T> dic = Core.GetAll<T>(key);
            if (dic == null)
                return new List<T>();
            return dic.Values.ToList<T>();
        }

        #endregion

        #region 辅助方法
       
        /// <summary>
        /// 自增1，返回自增后的值
        /// </summary>
        public long Incr(string key)
        {
            return Core.IncrementValue(key);
        }
        /// <summary>
        /// 自增count，返回自增后的值
        /// </summary>
        public double IncrBy(string key, int count)
        {
            return Core.IncrementValueBy(key, count);
        }
        /// <summary>
        /// 自减1，返回自减后的值
        /// </summary>
        public long Decr(string key)
        {
            return Core.DecrementValue(key);
        }
        /// <summary>
        /// 自减count ，返回自减后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public long DecrBy(string key, int count)
        {
            return Core.DecrementValueBy(key, count);
        }
        #endregion

        #region 删除
        public bool Remove(string key)
        {
            return Core.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            List<string> list = Core.SearchKeys(pattern);
            if (list.Count > 0)
            {
                Remove(list);
            }
        }

        public void Remove(List<string> list)
        {
            Core.RemoveAll(list);
            //Core.DeleteByIds<string>(list);
        }

        #endregion

    }
     */
    #endregion
    public class DoRedisString
    {
        #region 赋值
        /// <summary>
        /// 设置key的value
        /// </summary>
        public bool Set(string key, string value)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Set<string>(key, value);
            }
        }
        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public bool Set(string key, string value, DateTime dt)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Set<string>(key, value, dt);
            }
        }
        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public bool Set(string key, string value, TimeSpan sp)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Set<string>(key, value, sp);
            }
        }

        /// <summary>
        /// 设置key的value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Set<T>(key, value);
            }
        }

        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public bool Set<T>(string key, T value, TimeSpan sp)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Set<T>(key, value, sp);
            }
        }

        /// <summary>
        /// 设置key的value并设置过期时间
        /// </summary>
        public void Set<T>(Dictionary<string, T> dic)
        {
            using (var Core = RedisRepository.GetClient())
            {
                Core.SetAll<T>(dic);
            }
        }


        /// <summary>
        /// 设置多个key/value
        /// </summary>
        public void Set(Dictionary<string, string> dic)
        {
            using (var Core = RedisRepository.GetClient())
            {
                Core.SetAll(dic);
            }
        }

        #endregion

        #region 获取值
        /// <summary>
        /// 获取key的value值
        /// </summary>
        public string Get(string key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.GetValue(key);
            }
        }
        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        public List<string> Get(List<string> keys)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.GetValues(keys);
            }
        }
        /// <summary>
        /// 获取多个key的value值
        /// </summary>
        public List<T> Get<T>(List<string> keys)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.GetValues<T>(keys);
            }
        }
        /// <summary>
        /// 获取key的value值
        /// </summary>
        public T Get<T>(string key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Get<T>(key);
            }
        }

        public List<T> GetAll<T>(string key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                List<string> lk = new List<string>();
                lk.Add(key);
                IDictionary<string, T> dic = Core.GetAll<T>(lk);
                if (dic == null)
                    return new List<T>();
                return dic.Values.ToList<T>();
            }
        }

        public List<T> GetAll<T>(List<string> key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                IDictionary<string, T> dic = Core.GetAll<T>(key);
                if (dic == null)
                    return new List<T>();
                return dic.Values.ToList<T>();
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 自增1，返回自增后的值
        /// </summary>
        public long Incr(string key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.IncrementValue(key);
            }
        }
        /// <summary>
        /// 自增count，返回自增后的值
        /// </summary>
        public double IncrBy(string key, int count)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.IncrementValueBy(key, count);
            }
        }
        /// <summary>
        /// 自减1，返回自减后的值
        /// </summary>
        public long Decr(string key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.DecrementValue(key);
            }
        }
        /// <summary>
        /// 自减count ，返回自减后的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public long DecrBy(string key, int count)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.DecrementValueBy(key, count);
            }
        }
        #endregion

        #region 删除
        public bool Remove(string key)
        {
            using (var Core = RedisRepository.GetClient())
            {
                return Core.Remove(key);
            }
        }

        public void RemoveByPattern(string pattern)
        {
            using (var Core = RedisRepository.GetClient())
            {
                List<string> list = Core.SearchKeys(pattern);
                if (list.Count > 0)
                {
                    Remove(list);
                }
            }
        }

        public void Remove(List<string> list)
        {
            using (var Core = RedisRepository.GetClient())
            {
                Core.RemoveAll(list);
                //Core.DeleteByIds<string>(list);
            }
        }
        public void FlushDB() {
            using (var Core = RedisRepository.GetClient())
            {
                Core.FlushDb();
            }
        }

        #endregion

    }
}
