using CBSS.Framework.Redis;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Redis
{
    public class OtherRedisSortedSetHelper
    {
        /// <summary>
        /// 向有序集合中添加元素
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public bool AddItemToSortedSet(string setId, string value, double score)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.AddItemToSortedSet(setId, value, score);
            }
        }

        public bool AddRangeToSortedSet(string setId, List<string> values, double score)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                return Redis.AddRangeToSortedSet(setId, values, score);
            }
        }

        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的降序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetItemIndexInSortedSetDesc(string setId, string value)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                int index = Redis.GetItemIndexInSortedSetDesc(setId, value);
                return index;
            }
        }
        /// <summary>
        /// 获得某个值在有序集合中的排名，按分数的升序排列
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetItemIndexInSortedSet(string setId, string value)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                int index = Redis.GetItemIndexInSortedSet(setId, value);
                return index;
            }
        }
        /// <summary>
        /// 获得有序集合中某个值得分数
        /// </summary>
        /// <param name="set"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double GetItemScoreInSortedSet(string setId, string value)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                double score = Redis.GetItemScoreInSortedSet(setId, value);
                return score;
            }
        }
        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public List<string> GetRankRangeFromSortedSet(string setId, int beginRank, int endRank)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                List<string> valueList = Redis.GetRangeFromSortedSet(setId, beginRank, endRank);
                return valueList;
            }
        }

        public void RemoveSotedSet(string setId, int minRank, int maxRank)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                Redis.RemoveRangeFromSortedSet(setId, minRank, maxRank);
            }
        }

        /// <summary>
        /// 获得有序集合中，某个排名范围的所有值
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginRank"></param>
        /// <param name="endRank"></param>
        /// <returns></returns>
        public List<string> GetRankRangeFromSortedSetDesc(string setId, int beginRank, int endRank)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                List<string> valueList = Redis.GetRangeFromSortedSetDesc(setId, beginRank, endRank);
                return valueList;
            }
        }
        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，升序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSet(string setId, double beginScore, double endScore)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                List<string> valueList = Redis.GetRangeFromSortedSetByHighestScore(setId, beginScore, endScore);
                return valueList;
            }
        }
        /// <summary>
        /// 获得有序集合中，某个分数范围内的所有值，降序
        /// </summary>
        /// <param name="set"></param>
        /// <param name="beginScore"></param>
        /// <param name="endScore"></param>
        /// <returns></returns>
        public List<string> GetRangeFromSortedSetDesc(string setId, double beginScore, double endScore)
        {
            using (var Redis = OtherRedisManager.GetClient(0))
            {
                List<string> vlaueList = Redis.GetRangeFromSortedSetByLowestScore(setId, beginScore, endScore);
                return vlaueList;
            }
        }

    }
}