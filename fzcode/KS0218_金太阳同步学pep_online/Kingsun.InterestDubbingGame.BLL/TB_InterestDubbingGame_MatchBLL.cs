using Kingsun.InterestDubbingGame.DAL;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
namespace Kingsun.InterestDubbingGame.BLL
{
    public class TB_InterestDubbingGame_MatchBLL
    {
        static RedisSortedSetHelper sortedSetRedis = new RedisSortedSetHelper();
        static RedisHashHelper hashRedis = new RedisHashHelper();
        static DAL.TB_InterestDubbingGame_MatchDAL dal = new TB_InterestDubbingGame_MatchDAL();
        static TB_InterestDubbingGame_UserInfoDAL userInfoDal = new TB_InterestDubbingGame_UserInfoDAL();
        /// <summary>
        /// 刷新学生所在学校排行榜redis数据
        /// </summary>
        /// <returns></returns>
        public string UpdateUserSchoolRankList()
        {
            try
            {
                var allUserScores = hashRedis.GetAll<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore");
                if (allUserScores == null)
                    return "redis中无学生成绩数据";
                else
                {
                    allUserScores = allUserScores.Where(o => o.BookPlayScore > 0 && o.StoryReadScore > 0).ToList();
                }
                List<Redis_InterestDubbingGame_UserTotalScore_Rank> userScoreRankList = new List<Redis_InterestDubbingGame_UserTotalScore_Rank>();//用户信息+分数，List

                foreach (var userScore in allUserScores)
                {
                    var user = hashRedis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", userScore.UserID.ToString());//用户信息

                    if (user != null)
                    {
                        if (!user.SchoolID.HasValue)
                        {
                            Log4Net.LogHelper.Info("用户:" + user.UserName + ",没有对应的学校，不参与排名,用户ID:" + user.UserID);
                            continue;
                        }

                        var rank = new Redis_InterestDubbingGame_UserTotalScore_Rank
                            {
                                SchoolID = user.SchoolID.HasValue ? user.SchoolID.Value : 0,
                                TotalScore = userScore.TotalScore,
                                UserID = userScore.UserID.ToString(),
                                UserName = user.UserName
                            };
                        if (!user.GradeID.HasValue || user.GradeID.Value <= 3)//没有年级，默认低年级段
                        {
                            rank.GradeRange = 1;//低
                        }
                        else if (user.GradeID.Value >= 4 && user.GradeID.Value <= 5)
                        {
                            rank.GradeRange = 2;//中
                        }
                        else
                        {
                            rank.GradeRange = 3;//高
                        }
                        userScoreRankList.Add(rank);
                    }
                }
                var groups = userScoreRankList.GroupBy(o => new { SchoolID = o.SchoolID, GradeRange = o.GradeRange }).ToList();//按学校和年级段分组

                foreach (var group in groups)//每学校和年级段确定一个组
                {
                    List<Redis_InterestDubbingGame_UserTotalScore_Rank> userScoreList = group.OrderByDescending(o => o.TotalScore).ToList();

                    userScoreList.ForEach(o =>
                    {
                        o.UserName = string.IsNullOrWhiteSpace(o.UserName) ? "暂未填写" : o.UserName;
                    });

                    //往Redis_InterestDubbingGame_UserNameSchoolRank表写入
                    var uGroup = userScoreList.GroupBy(o => o.UserName).ToList();
                    hashRedis.Remove("Redis_InterestDubbingGame_UserNameSchoolRank_" + group.Key.SchoolID + "_" + group.Key.GradeRange);//先移除
                    foreach (var u in uGroup)
                    {
                        hashRedis.Set("Redis_InterestDubbingGame_UserNameSchoolRank_" + group.Key.SchoolID + "_" + group.Key.GradeRange, u.Key, u.Select(o => o.UserID).ToList());
                    }

                    //Redis_InterestDubbingGame_UserSchoolRank,每条记录根据分数排行               
                    foreach (var u in userScoreList)
                    {
                        sortedSetRedis.AddItemToSortedSet("Redis_InterestDubbingGame_UserSchoolRank_" + group.Key.SchoolID + "_" + group.Key.GradeRange, u.UserID, u.TotalScore);
                    }
                }

                return "成功";
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "成绩同步到排行榜出错");
                return "成绩同步到排行榜出错";
            }
        }

        /// <summary>
        /// 获取整个排行榜（不搜索）
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="gradeRange"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public UserSchoolRankReponse GetAllUserSchoolRankList(string userId, string gradeRange, int pageIndex, int pageSize, string schoolId_mod = "")
        {
            UserSchoolRankReponse response = new UserSchoolRankReponse();
            //学生信息表
            string userInfoHashId = "TB_InterestDubbingGame_UserInfo";

            var currentUser = hashRedis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, userId);

            string schoolId = "";
            if (!string.IsNullOrWhiteSpace(schoolId_mod))
            {
                schoolId = schoolId_mod;
            }
            else
            {
                schoolId = currentUser.SchoolID.Value.ToString();//学校
            }


            //总成绩表
            string scoreHashId = "Redis_InterestDubbingGame_UserTotalScore";
            //学生姓名和ID哈希表
            string userNameSchoolRankHashId = "Redis_InterestDubbingGame_UserNameSchoolRank" + schoolId + "_" + gradeRange;
            //排行榜表
            string rankSetId = "Redis_InterestDubbingGame_UserSchoolRank_" + schoolId + "_" + gradeRange;

            #region 排行榜数据
            List<string> userIds = new List<string>();

            userIds = sortedSetRedis.GetRankRangeFromSortedSetDesc(rankSetId, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);//分页获取排行,

            List<Redis_InterestDubbingGame_UserTotalScore_Rank> userRanks = new List<Redis_InterestDubbingGame_UserTotalScore_Rank>();


            var votedList = hashRedis.Get<List<string>>("Redis_InterestDubbingGame_VoterRecord", userId);
            votedList = votedList ?? new List<string>();
            foreach (string uId in userIds)
            {
                var sort = sortedSetRedis.GetItemIndexInSortedSetDesc(rankSetId, uId) + 1;//获取名次
                var uScore = sortedSetRedis.GetItemScoreInSortedSet(rankSetId, uId);
                var score = hashRedis.Get<Redis_InterestDubbingGame_UserTotalScore>(scoreHashId, uId);//分数
                var user = hashRedis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, uId);
                if (score != null && user != null)
                {
                    Redis_InterestDubbingGame_UserTotalScore_Rank userRank = new Redis_InterestDubbingGame_UserTotalScore_Rank
                    {
                        Sort = sort,
                        UserImage = user.UserImage,
                        UserName = user.UserName,
                        UserID = user.UserID,
                        TotalScore = uScore,
                        ClassName = user.ClassName,
                        SchoolName = user.SchoolName,
                        //  VoteNum = score.VoteNum
                        Voted = votedList.Any(o => o == userId)
                    };
                    userRanks.Add(userRank);
                }
            }
            #endregion

            #region 当前用户(如果是老师或者未报名学生则为null)
            var cSort = sortedSetRedis.GetItemIndexInSortedSetDesc(rankSetId, userId) + 1;//获取名次
            var cUScore = sortedSetRedis.GetItemScoreInSortedSet(rankSetId, userId);//从排行表中取分数（没有则是NaN）
            var cScore = hashRedis.Get<Redis_InterestDubbingGame_UserTotalScore>(scoreHashId, userId);//分数
            var cUser = hashRedis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, userId);
            int? cGradeRange = -1;//默认没年级段
            if (cUser != null)//不为null肯定是已报名学生
            {
                cGradeRange = cUser.GradeID >= 4 && cUser.GradeID <= 5 ? 2 : (cUser.GradeID <= 3 ? 1 : 3);
            }

            Redis_InterestDubbingGame_UserTotalScore_Rank currentUserRank = null;
            if (cUScore > 0 && cScore != null && cUser != null && gradeRange == cGradeRange.ToString())
            {//当前用户必须在当前查询的学段下
                currentUserRank = new Redis_InterestDubbingGame_UserTotalScore_Rank
               {
                   Sort = cSort,
                   UserImage = cUser.UserImage,
                   UserName = cUser.UserName,
                   UserID = cUser.UserID,
                   TotalScore = cUScore > 0 ? cUScore : 0,
                   ClassName = cUser.ClassName,
                   SchoolName = cUser.SchoolName,
                   //  VoteNum = score.VoteNum
                   Voted = votedList.Any(o => o == userId)
               };
            }
            #endregion

            //当前学生
            response.currentUserRank = currentUserRank;
            //其他学生排行
            response.RankList = userRanks;

            #region 用户头像
            List<string> userIDs = new List<string>();
            if (currentUserRank != null)
            {
                userIDs.Add(currentUserRank.UserID);
            };
            if (userRanks != null && userRanks.Any())
            {
                userIds.AddRange(userRanks.Select(o => o.UserID).ToArray());
            }
            var userImages = userInfoDal.GetUserInfoModelByUserID(userIds);
            if (userImages != null && userImages.Any())
            {
                if (currentUserRank != null && userImages.Any(o => o.UserID.ToString() == currentUserRank.UserID))
                {
                    currentUserRank.UserImage = userImages.First(o => o.UserID.ToString() == currentUserRank.UserID).UserImage;
                }
                if (userRanks != null && userRanks.Any())
                {
                    foreach (var u in userRanks)
                    {
                        var image = userImages.FirstOrDefault(o => o.UserID.ToString() == u.UserID);
                        if (image != null) u.UserImage = image.UserImage;
                    }
                }
            }
            #endregion
            return response;
        }

        /// <summary>
        /// 根据学生姓名搜索成绩和排名
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="gradeRange"></param>
        /// <param name="username"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public UserSchoolRankReponse SearchUserSchoolRankList(string userId, string gradeRange, string username, int pageIndex, int pageSize, string schoolId_mod = "")
        {
            UserSchoolRankReponse response = new UserSchoolRankReponse();

            //学生信息表
            string userInfoHashId = "TB_InterestDubbingGame_UserInfo";

            var currentUser = hashRedis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, userId);

            string schoolId = "";
            if (!string.IsNullOrWhiteSpace(schoolId_mod))
            {
                schoolId = schoolId_mod;
            }
            else
            {
                schoolId = currentUser.SchoolID.Value.ToString();//学校
            }


            //总成绩表
            string scoreHashId = "Redis_InterestDubbingGame_UserTotalScore";
            //学生姓名和ID哈希表
            string userNameSchoolRankHashId = "Redis_InterestDubbingGame_UserNameSchoolRank_" + schoolId + "_" + gradeRange;
            //排行榜表
            string rankSetId = "Redis_InterestDubbingGame_UserSchoolRank_" + schoolId + "_" + gradeRange;

            #region 排行榜数据
            List<string> userIds = new List<string>();

            userIds = hashRedis.Get<List<string>>(userNameSchoolRankHashId, username.Trim());
            userIds = userIds ?? new List<string>();

            List<Redis_InterestDubbingGame_UserTotalScore_Rank> userRanks = new List<Redis_InterestDubbingGame_UserTotalScore_Rank>();

            var votedList = hashRedis.Get<List<string>>("Redis_InterestDubbingGame_VoterRecord", userId);
            votedList = votedList ?? new List<string>();
            foreach (string uId in userIds)
            {

                var sort = sortedSetRedis.GetItemIndexInSortedSetDesc(rankSetId, uId) + 1;//获取名次
                var score = hashRedis.Get<Redis_InterestDubbingGame_UserTotalScore>(scoreHashId, uId);//分数
                var user = hashRedis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, uId);
                if (score != null && user != null)
                {
                    Redis_InterestDubbingGame_UserTotalScore_Rank userRank = new Redis_InterestDubbingGame_UserTotalScore_Rank
                    {
                        Sort = sort,
                        UserImage = user.UserImage,
                        UserName = user.UserName,
                        UserID = user.UserID,
                        TotalScore = score.TotalScore,
                        ClassName = user.ClassName,
                        SchoolName = user.SchoolName,
                        //  VoteNum = score.VoteNum
                        Voted = votedList.Any(o => o == userId)
                    };
                    userRanks.Add(userRank);
                }
            }
            #endregion

            #region 当前用户排名(如果是老师或者未报名学生则为null)
            var cSort = sortedSetRedis.GetItemIndexInSortedSetDesc(rankSetId, userId) + 1;//获取名次
            var cUScore = sortedSetRedis.GetItemScoreInSortedSet(rankSetId, userId);
            var cScore = hashRedis.Get<Redis_InterestDubbingGame_UserTotalScore>(scoreHashId, userId);//分数
            var cUser = hashRedis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, userId);
            Redis_InterestDubbingGame_UserTotalScore_Rank currentUserRank = new Redis_InterestDubbingGame_UserTotalScore_Rank();
            if (cScore != null && cUser != null)
            {
                currentUserRank = new Redis_InterestDubbingGame_UserTotalScore_Rank
                {
                    Sort = cSort,
                    UserImage = cUser.UserImage,
                    UserName = cUser.UserName,
                    UserID = cUser.UserID,
                    TotalScore = cUScore > 0 ? cUScore : 0,
                    ClassName = cUser.ClassName,
                    SchoolName = cUser.SchoolName,
                    //  VoteNum = score.VoteNum
                    Voted = votedList.Any(o => o == userId)
                };
            }
            #endregion

            //   response.currentUserRank = currentUserRank;
            response.RankList = userRanks;

            #region 用户头像
            List<string> userIDs = new List<string>();
            if (currentUserRank != null)
            {
                userIDs.Add(currentUserRank.UserID);
            };
            if (userRanks != null && userRanks.Any())
            {
                userIds.AddRange(userRanks.Select(o => o.UserID).ToArray());
            }
            var userImages = userInfoDal.GetUserInfoModelByUserID(userIds);
            if (userImages != null && userImages.Any())
            {
                if (currentUserRank != null && userImages.Any(o => o.UserID.ToString() == currentUserRank.UserID))
                {
                    currentUserRank.UserImage = userImages.First(o => o.UserID.ToString() == currentUserRank.UserID).UserImage;
                }
                if (userRanks != null && userRanks.Any())
                {
                    foreach (var u in userRanks)
                    {
                        var image = userImages.FirstOrDefault(o => o.UserID.ToString() == u.UserID);
                        if (image != null) u.UserImage = image.UserImage;
                    }
                }
            }
            #endregion

            return response;
        }



        public KingResponse Vote(string voterId, string userId)
        {
            KingResponse response = new KingResponse();
            var userList = hashRedis.Get<List<string>>("Redis_InterestDubbingGame_VoterRecord", voterId);

            userList = userList ?? new List<string>();//为null则新建

            if (userList.Count < 3)
            {
                if (userList.Any(o => o == userId))
                {
                    return KingResponse.GetErrorResponse("不能重复投票！");
                }

                userList.Add(userId);
                hashRedis.Set("Redis_InterestDubbingGame_VoterRecord", voterId, userList);

                using (var redis = RedisManager.GetClient(0))
                {
                    using (redis.AcquireLock("Redis_InterestDubbingGame_UserTotalScore_" + userId))
                    {//锁
                        var score = hashRedis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", userId);
                        if (score.VoteNum < 200)
                        {
                            score.TotalScore += 0.1;
                        }
                        score.VoteNum += 1;
                        hashRedis.Set("Redis_InterestDubbingGame_UserTotalScore", userId, score);
                    }
                }

                response.Success = true;
                response.Data = "投票成功，您还剩下" + (3 - userList.Count) + "次投票机会";
            }
            else
            {
                response.ErrorMsg = "您的投票次数已用完！";
            }
            return response;
        }

        public KingResponse GetUserScore(string userId, string voterId)
        {
            var score = hashRedis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", userId);
            var user = hashRedis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", userId);
            string UserImage = "";
            var userImages = userInfoDal.GetUserInfoModelByUserID(new List<string>() { user.UserID });
            if (userImages != null && userImages.Count > 0)
            {
                UserImage = userImages[0].UserImage;
            }
            KingResponse response = new KingResponse();

            if (user != null && score != null)
            {
                var votedList = hashRedis.Get<List<string>>("Redis_InterestDubbingGame_VoterRecord", voterId);
                votedList = votedList ?? new List<string>();

                Redis_InterestDubbingGame_UserTotalScore_Rank userScore = new Redis_InterestDubbingGame_UserTotalScore_Rank()
            {
                UserID = user.UserID,
                UserImage = UserImage,
                UserName = user.UserName,
                TotalScore = score.TotalScore,
                VoteNum = score.VoteNum,
                BookPlayScore = score.BookPlayScore,
                StoryReadScore = score.StoryReadScore,
                Voted = votedList.Any(o => o == userId),
                RemainVote = 3 - votedList.Count()
            };
                response.Success = true;
                response.Data = userScore;
            }
            return response;
        }

        public KingResponse GetUserContentRecord(string userId)
        {
            KingResponse response = new KingResponse();
            var records = dal.GetUserContentRecord(userId);
            if (records.Any())
            {
                response.Success = true;
                response.Data = records;
            }
            return response;
        }
    }
}
