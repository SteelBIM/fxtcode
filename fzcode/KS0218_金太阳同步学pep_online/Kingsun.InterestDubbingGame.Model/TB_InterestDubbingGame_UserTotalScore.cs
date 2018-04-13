using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.InterestDubbingGame.Model
{
    public class Redis_InterestDubbingGame_UserTotalScore
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public double TotalScore { get; set; }
        /// <summary>
        /// 课本剧分数
        /// </summary>
        public double BookPlayScore { get; set; }
        /// <summary>
        /// 故事朗读分数
        /// </summary>
        public double StoryReadScore { get; set; }
        /// <summary>
        /// 获得投票数
        /// </summary>
        public double VoteNum { get; set; }
    }

    public class Redis_InterestDubbingGame_UserTotalScore_Rank
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 课本剧分数
        /// </summary>
        public double BookPlayScore { get; set; }
        /// <summary>
        /// 故事朗读分数
        /// </summary>
        public double StoryReadScore { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// 年级段（1-2低，3-4中，5-6高）
        /// </summary>
        public int GradeRange { get; set; }

        public int SchoolID { get; set; }

        public string SchoolName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        public int Sort { get; set; }

        public double VoteNum { get; set; }
        /// <summary>
        /// 当前用户是否已投票
        /// </summary>
        public bool Voted { get; set; }
        /// <summary>
        /// 剩余投票次数
        /// </summary>
        public int RemainVote { get; set; }

        public string ClassName { get; set; }
    }

    public class UserSchoolRankReponse
    {
        /// <summary>
        /// 当前用户排名
        /// </summary>
        public Redis_InterestDubbingGame_UserTotalScore_Rank currentUserRank { get; set; }
        /// <summary>
        /// 用户排名列表
        /// </summary>
        public List<Redis_InterestDubbingGame_UserTotalScore_Rank> RankList { get; set; }

        public string SchoolID { get; set; }

        public string SchoolName { get; set; }
    }
}
