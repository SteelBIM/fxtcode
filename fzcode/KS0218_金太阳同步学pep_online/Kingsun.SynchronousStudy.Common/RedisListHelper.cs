using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class RedisListHelper
    {
        #region 赋值
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        public void LPush(string listid, string value)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.PushItemToList(listid, value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listid为：" + listid + "|value:" + value);
                }
            }
        }
        /// <summary>
        /// 从左侧向list中添加值，并设置过期时间
        /// </summary>
        public void LPush(string listId, string value, DateTime dt)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.PushItemToList(listId, value);
                    Redis.ExpireEntryAt(listId, dt);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value + "|DateTime:" + dt);
                }
            }
        }
        /// <summary>
        /// 从左侧向list中添加值，设置过期时间
        /// </summary>
        public void LPush(string listId, string value, TimeSpan sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.PushItemToList(listId, value);
                    Redis.ExpireEntryIn(listId, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value + "|TimeSpan:" + sp);
                }
            }
        }
        /// <summary>
        /// 从左侧向list中添加值
        /// </summary>
        public void RPush(string listId, string value)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.PrependItemToList(listId, value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value);
                }
            }
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>    
        public void RPush(string listId, string value, DateTime dt)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.PrependItemToList(listId, value);
                    Redis.ExpireEntryAt(listId, dt);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value + "|DateTime:" + dt);
                }
            }
        }
        /// <summary>
        /// 从右侧向list中添加值，并设置过期时间
        /// </summary>        
        public void RPush(string listId, string value, TimeSpan sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.PrependItemToList(listId, value);
                    Redis.ExpireEntryIn(listId, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value + "|TimeSpan:" + sp);
                }
            }
        }
        /// <summary>
        /// 添加listId/value
        /// </summary>     
        public void Add(string listId, string value)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.AddItemToList(listId, value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value);
                }
            }
        }
        /// <summary>
        /// 添加listId/value ,并设置过期时间
        /// </summary>  
        public void Add(string listId, string value, DateTime dt)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.AddItemToList(listId, value);
                    Redis.ExpireEntryAt(listId, dt);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value + "|DateTime:" + dt);
                }
            }
        }
        /// <summary>
        /// 添加listId/value。并添加过期时间
        /// </summary>  
        public void Add(string listId, string value, TimeSpan sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.AddItemToList(listId, value);
                    Redis.ExpireEntryIn(listId, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value + "|TimeSpan:" + sp);
                }
            }
        }
        /// <summary>
        /// 为listId添加多个值
        /// </summary>  
        public void Add(string listId, List<string> values)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.AddRangeToList(listId, values);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + values);
                }
            }
        }
        /// <summary>
        /// 为listId添加多个值，并设置过期时间
        /// </summary>  
        public void Add(string listId, List<string> values, DateTime dt)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.AddRangeToList(listId, values);
                    Redis.ExpireEntryAt(listId, dt);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + values + "|DateTime:" + dt);
                }
            }
        }
        /// <summary>
        /// 为listId添加多个值，并设置过期时间
        /// </summary>  
        public void Add(string listId, List<string> values, TimeSpan sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    Redis.AddRangeToList(listId, values);
                    Redis.ExpireEntryIn(listId, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + values + "|TimeSpan:" + sp);
                }
            }
        }
        #endregion

        #region 获取值
        /// <summary>
        /// 获取list中listId包含的数据数量
        /// </summary>  
        public long Count(string listId)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.GetListCount(listId);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId);
                    return 0;
                }
            }
        }
        /// <summary>
        /// 获取listId包含的所有数据集合
        /// </summary>  
        public List<string> Get(string listId)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.GetAllItemsFromList(listId);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId);
                    return new List<string>();
                }
            }
        }
        /// <summary>
        /// 获取listId中下标为star到end的值集合
        /// </summary>  
        public List<string> Get(string listId, int star, int end)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.GetRangeFromList(listId, star, end);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|star:" + star + "|end:" + end);
                    return new List<string>();
                }
            }
        }

        public List<string> GetList(string listId, int start, int end)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    List<string> strlist = Redis.GetRangeFromList(listId, start, end);
                    return strlist;
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|start:" + start + "|end:" + end);
                    return new List<string>();
                }
            }
        }

        #endregion

        #region 阻塞命令
        /// <summary>
        ///  阻塞命令：从list中listIds的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingPopItemFromList(string listId, TimeSpan? sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.BlockingDequeueItemFromList(listId, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|TimeSpan:" + sp);
                    return null;
                }
            }
        }

        /// <summary>
        ///  阻塞命令：从list中listIds的尾部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingDequeueItemFromList(string listId, TimeSpan? sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.BlockingDequeueItemFromList(listId, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|TimeSpan:" + sp);
                    return null;
                }
            }
        }

        /// <summary>
        /// 阻塞命令：从list中listId的头部移除一个值，并返回移除的值，阻塞时间为sp
        /// </summary>  
        public string BlockingRemoveStartFromList(string listIds, TimeSpan? sp)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.BlockingRemoveStartFromList(listIds, sp);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listIds为：" + listIds + "|TimeSpan:" + sp);
                    return null;
                }
            }
        }

        #endregion

        #region 删除
        /// <summary>
        /// 从尾部移除数据，返回移除的数据
        /// </summary>  
        public string PopItemFromList(string listId)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.PopItemFromList(listId);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId);
                    return null;
                }
            }
        }
        /// <summary>
        /// 移除list中，listId/value,与参数相同的值，并返回移除的数量
        /// </summary>  
        public long RemoveItemFromList(string listId, string value)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.RemoveItemFromList(listId, value);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId + "|value:" + value);
                    return 0;
                }
            }
        }
        /// <summary>
        /// 从list的尾部移除一个数据，返回移除的数据
        /// </summary>  
        public string RemoveEndFromList(string listId)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.RemoveEndFromList(listId);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId);
                    return null;
                }
            }
        }
        /// <summary>
        /// 从list的头部移除一个数据，返回移除的值
        /// </summary>  
        public string RemoveStartFromList(string listId)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.RemoveStartFromList(listId);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：listId为：" + listId);
                    return null;
                }
            }
        }
        #endregion

        #region 其它
        /// <summary>
        /// 从一个list的尾部移除一个数据，添加到另外一个list的头部，并返回移动的值
        /// </summary>  
        public string PopAndPushItemBetweenLists(string fromKey, string toKey)
        {
            using (var Redis = RedisManager.GetClient(0))
            {
                try
                {
                    return Redis.PopAndPushItemBetweenLists(fromKey, toKey);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误：fromKey为：" + fromKey + "|toKey:" + toKey);
                    return null;
                }
            }
        }
        #endregion
    }
}