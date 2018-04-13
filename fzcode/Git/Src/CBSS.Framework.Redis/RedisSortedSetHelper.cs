using CBSS.Framework.Redis;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Redis
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper”的 XML 注释
    public class RedisSortedSetHelper
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper”的 XML 注释
    {
        private string _managerName { get; set; }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper.RedisSortedSetHelper(string)”的 XML 注释
        public RedisSortedSetHelper(string managerName)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper.RedisSortedSetHelper(string)”的 XML 注释
        {
            _managerName = managerName;
        }


        /// <summary>
        /// 向有序集合中添加元素
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public bool AddItemToSortedSet(string setId, string value, double score)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.AddItemToSortedSet(setId, value, score);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper.AddRangeToSortedSet(string, List<string>, double)”的 XML 注释
        public bool AddRangeToSortedSet(string setId, List<string> values, double score)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper.AddRangeToSortedSet(string, List<string>, double)”的 XML 注释
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.AddRangeToSortedSet(setId, values, score);
            }
        }

#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetItemIndexInSortedSetDesc(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的降序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetItemIndexInSortedSetDesc(string setId, string value)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetItemIndexInSortedSetDesc(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetItemIndexInSortedSetDesc(setId, value);
            }
        }
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetItemIndexInSortedSet(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的升序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetItemIndexInSortedSet(string setId, string value)
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetItemIndexInSortedSet(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetItemIndexInSortedSet(setId, value);
            }
        }
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetItemScoreInSortedSet(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 获得有序集合中某个值得分数
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double GetItemScoreInSortedSet(string setId, string value)
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetItemScoreInSortedSet(string, string)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetItemScoreInSortedSet(setId, value);
            }
        }
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRankRangeFromSortedSet(string, int, int)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public List<string> GetRankRangeFromSortedSet(string setId, int beginRank, int endRank)
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRankRangeFromSortedSet(string, int, int)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetRangeFromSortedSet(setId, beginRank, endRank); ;
            }
        }


#pragma warning disable CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper.RemoveSotedSet(string, int, int)”的 XML 注释
        public void RemoveSotedSet(string setId, int minRank, int maxRank)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“RedisSortedSetHelper.RemoveSotedSet(string, int, int)”的 XML 注释
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                Redis.RemoveRangeFromSortedSet(setId, minRank, maxRank);
            }
        }

#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRankRangeFromSortedSetDesc(string, int, int)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public List<string> GetRankRangeFromSortedSetDesc(string setId, int beginRank, int endRank)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRankRangeFromSortedSetDesc(string, int, int)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetRangeFromSortedSetDesc(setId, beginRank, endRank);
            }
        }
#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRangeFromSortedSet(string, double, double)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，升序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSet(string setId, double beginScore, double endScore)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRangeFromSortedSet(string, double, double)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetRangeFromSortedSetByHighestScore(setId, beginScore, endScore);
            }
        }
#pragma warning disable CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRangeFromSortedSetDesc(string, double, double)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
#pragma warning disable CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，降序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSetDesc(string setId, double beginScore, double endScore)
#pragma warning restore CS1572 // XML 注释中有“set”的 param 标记，但是没有该名称的参数
#pragma warning restore CS1573 // 参数“setId”在“RedisSortedSetHelper.GetRangeFromSortedSetDesc(string, double, double)”的 XML 注释中没有匹配的 param 标记(但其他参数有)
        {
            using (var Redis = RedisManager.GetClient(0, _managerName))
            {
                return Redis.GetRangeFromSortedSetByLowestScore(setId, beginScore, endScore);
            }
        }

    }
}