using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract;
using CBSS.Framework.Contract.API;
using CBSS.Framework.Contract.Enums;
using CBSS.IBS.Contract;
using CBSS.IBS.Contract.IBSResource;
using CBSS.Tbx.Contract.DataModel;
using CBSS.Tbx.Contract.ViewModel;
using CBSS.Tbx.IBLL;

namespace CBSS.Tbx.BLL
{
    public partial class TbxService : ITbxService
    {
        /// <summary>
        /// 获取趣配音模块视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse GetDubbingByCataId(v_BookInfo input)
        {
            int cId = string.IsNullOrEmpty(input.SecondTitleID) ? input.FirstTitleID : Convert.ToInt32(input.SecondTitleID);
            EnglishResourceModel englishResourceModel = new EnglishResourceModel
            {
                bookId = input.BookID,
                catalogueId = cId,
                moduleID = input.FirstModularID,
                type = (int)MODSourceTypeEnum.IntestingDubbing
            };
            //查询书籍信息
            MarketBook marketBook = repository.GetByID<MarketBook>(input.BookID);
            if (marketBook == null)
            {
                return APIResponse.GetErrorResponse("找不到书籍信息！");
            }

            //查询书籍目录信息
            MarketBookCatalog marketBookCatalogs = repository
                .SelectSearch<MarketBookCatalog>(i =>
                    i.MarketBookID == input.BookID && i.MarketBookCatalogID == cId).FirstOrDefault();
            if (marketBookCatalogs == null)
            {
                return APIResponse.GetErrorResponse("找不到书籍目录信息！");
            }

            //查询一级目录信息
            MarketBookCatalog bookCatalogs = repository.GetByID<MarketBookCatalog>(marketBookCatalogs.ParentCatalogID);
            if (bookCatalogs == null)
            {
                return APIResponse.GetErrorResponse("找不到书籍目录信息！");
            }

            List<V_DubbingInfo> dubbingList = new List<V_DubbingInfo>();

            var repon = GetModResource(englishResourceModel);
            if (repon.Success)
            {
                var videoList = repon.Data as List<V_DubbingInfo>;
                if (videoList == null || videoList.Count == 0)
                {
                    return APIResponse.GetErrorResponse("MOD资源不存在！");
                }

                videoList.ForEach(a =>
                {
                    a.BookID = marketBook.MarketBookID;
                    a.BookName = marketBook.MarketBookName;
                    a.FirstTitleID = bookCatalogs.MarketBookCatalogID;
                    a.FirstTitle = bookCatalogs.MarketBookCatalogName;
                    a.SecondTitleID = marketBookCatalogs.MarketBookCatalogID;
                    a.SecondTitle = marketBookCatalogs.MarketBookCatalogName;
                });

            }
            return APIResponse.GetResponse(dubbingList.ToJson());
        }

        /// <summary>
        /// 获取未发布/发布视频信息列表
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="State"></param>
        /// <param name="userId"></param>
        /// <param name="isEnableOss"></param>
        /// <returns></returns>
        public APIResponse GetVideoList(string appId, int PageIndex, int State, int userId, int isEnableOss)
        {
            int versionId = ConvertAppVersion(appId);
            int pageIndex = Convert.ToInt32(PageIndex);
            int state = Convert.ToInt32(State);
            List<VideoInfo> list = new List<VideoInfo>();

            List<TB_UserVideoDetails> userVideoDetails = InterestDubbingRepository.SelectSearch<TB_UserVideoDetails>(
                i => i.UserID == userId
                     && i.State == state
                     && i.VersionID == versionId
                     && i.VideoType == "0"
                     && i.VideoNumber != 0
                     && i.BookID != 0).OrderByDescending(i => i.ID).ToList();

            foreach (var item in userVideoDetails)
            {
                string img = "";
                string fileId = "";
                if (isEnableOss == 0)
                {
                    img = item.VideoImageAddress;
                    fileId = item.VideoFileID;
                }
                else
                {
                    string times = Convert.ToDateTime(item.CreateTime).ToString("yyyy/MM/dd");
                    img = item.IsEnableOss != 0 ? _getOssFilesUrl + item.VideoImageAddress : _getFilesUrl + "?FileID=" + item.VideoImageAddress;
                    fileId = item.IsEnableOss != 0 ? _getOssFilesUrl + item.VideoFileID : _getVideoFiles + times + "/" + item.VideoFileID;
                }
                DateTime time = new DateTime();
                VideoInfo videoInfo = new VideoInfo();
                string[] timeArr = new string[3];
                videoInfo.ID = item.ID;
                videoInfo.VideoID = item.VideoID;
                videoInfo.VideoImageAddress = img;
                videoInfo.VideoReleaseAddress = fileId;
                videoInfo.TotalScore = Convert.ToInt32(item.TotalScore);
                time = Convert.ToDateTime(item.CreateTime);
                videoInfo.Month = time.Month.ToString();
                timeArr = time.ToString("yyyy-MM-dd").Split('-');
                videoInfo.Day = timeArr[2];
                videoInfo.State = State.ToString();
                videoInfo.VideoType = item.VideoType;
                //TODO:通过书籍ID和视频序号获取视频标题（未实现）
                //List<videoDetailsInfo> videoDetails = details.Where(i => i.BookID == item.BookID & i.VideoNumber == item.VideoNumber).ToList();
                //if (videoDetails.Count == 0)
                //{
                //    videoInfo.VideoTitle = "";
                //}
                //else
                //{
                //    videoInfo.VideoTitle = StringTOJson(videoDetails[0].VideoTitle);
                //}
                list.Add(videoInfo);

            }
            list = list.Skip(pageIndex).Take(10).ToList();
            return APIResponse.GetResponse(list.ToJson());
        }

        /// <summary>
        /// 修改已发布/未发布视频信息状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="idStr"></param>
        /// <returns></returns>
        public APIResponse UpdateVedioInfo(string state, string idStr)
        {

            int s = InterestDubbingRepository.BatchUpdate<TB_UserVideoDetails>(new { State = state ?? "0" },
                it => idStr.Contains(it.ID.ToString()));
            if (s > 0)
            {
                if (state == "0")
                {
                    return APIResponse.GetResponse("视频信息删除成功");
                }
                if (state == "1")
                {
                    return APIResponse.GetResponse("视频信息发布成功");
                }
                return APIResponse.GetResponse("视频信息未发布");
            }
            else
            {
                return APIResponse.GetErrorResponse("视频信息发布/删除失败！");
            }
        }

        /// <summary>
        /// 插入趣配音用户配音视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse InsertVideoInfo(UserVideoInfo input)
        {
            string schoolid = "";
            string classid = "";
            string userType = "";
            string classnum = "";
            string tname = "暂未填写";
            string img = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";
            var user = ibsService.GetUserInfoByUserId(Convert.ToInt32(input.UserId));
            if (user != null)
            {
                img = user.IsEnableOss != 0 ? _getOssFilesUrl + (string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage) : _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage);
                tname = string.IsNullOrEmpty(user.TrueName) ? "暂未填写" : user.TrueName;
                userType = ((int)user.UserType).ToString();
                if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                {
                    var classinfo = user.ClassSchList.FirstOrDefault();
                    if (classinfo != null)
                    {
                        var clas = ibsService.GetClassUserRelationByClassId(classinfo.ClassID);
                        if (clas != null)
                        {
                            schoolid = clas.SchID.ToString();
                            classid = clas.ClassID.ToString();
                            classnum = clas.ClassNum;
                        }
                    }
                }

                int userVideoId = AddVideoInfo(input);
                if (userVideoId > 0)
                {
                    string ModuleType = "1";

                    List<string> lst = new List<string>();
                    RedisVideoInfo rvi = new RedisVideoInfo
                    {
                        VideoID = userVideoId.ToString(),
                        BookId = input.BookId.ToString(),
                        VideoNumber = input.VideoNumber.ToString(),
                        SchoolID = schoolid,
                        ClassID = classid,
                        UserId = input.UserId.ToString(),
                        TotalScore = input.TotalScore.ToString(),
                        CreateTime = DateTime.Now.ToString(),
                        NumberOfOraise = lst,
                        UserType = userType,
                        TrueName = tname,
                        UserImage = img,
                        ModuleType = ModuleType
                    };
                    RedisVideoInfo videoinfo = new RedisVideoInfo
                    {
                        VideoID = userVideoId.ToString(),
                        BookId = input.BookId.ToString(),
                        VideoNumber = input.VideoNumber.ToString(),
                        SchoolID = schoolid,
                        ClassID = classnum,
                        UserId = input.UserId.ToString(),
                        TotalScore = input.TotalScore.ToString(),
                        CreateTime = DateTime.Now.ToString(),
                        NumberOfOraise = lst,
                        UserType = userType,
                        TrueName = tname,
                        UserImage = img,
                        ModuleType = ModuleType,
                        VideoImageAddress = input.VideoImageAddress,
                        FirstTitleID = input.FirstTitleID,
                        SecondTitleID = input.SecondTitleID,
                        FirstModularID = input.FirstModularID,
                        IsEnableOss = input.IsEnableOss,
                        VideoTitle = input.VideoTitle
                    };
                    //队列
                    redisList.LPush("RankQueue", rvi.ToJson());
                    redisList.LPush("LearningReportQueue", videoinfo.ToJson());

                    string sj = "{\"ID\":" + userVideoId + "}";
                    return APIResponse.GetResponse(sj.ToJson());
                }
                else
                {
                    return APIResponse.GetErrorResponse("视频信息插入失败");
                }

            }
            else
            {
                return APIResponse.GetErrorResponse("找不到该用户！");
            }
        }

        /// <summary>
        /// 插入用户配音视频信息(事物)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int AddVideoInfo(UserVideoInfo input)
        {
            using (var db = InterestDubbingRepository.GetInstance())
            {
                try
                {
                    db.Ado.BeginTran();

                    int versionId = ConvertAppVersion(input.AppID);
                    TB_UserVideoDetails userVideo = new TB_UserVideoDetails();
                    userVideo.VersionID = versionId;
                    userVideo.UserID = input.UserId;
                    userVideo.BookID = Convert.ToInt32(input.BookId);
                    userVideo.VideoNumber = input.VideoNumber;
                    userVideo.VideoFileID = input.VideoFileId;
                    userVideo.VideoReleaseAddress = input.VideoReleaseAddress;
                    userVideo.VideoImageAddress = input.VideoImageAddress;
                    userVideo.TotalScore = input.TotalScore;
                    userVideo.State = Convert.ToInt32(input.State);
                    userVideo.VersionType = Convert.ToInt32(input.VersionType);
                    userVideo.VideoType = input.VideoType;
                    userVideo.IsEnableOss = input.IsEnableOss;

                    object i = db.Insertable<TB_UserVideoDetails>(userVideo).ExecuteReturnIdentity();

                    bool result = true;
                    if (Convert.ToInt32(i) > 0)
                    {
                        if (input.children.Length > 0)
                        {
                            List<TB_UserVideoDialogue> list = new List<TB_UserVideoDialogue>();
                            foreach (Children t1 in input.children)
                            {
                                TB_UserVideoDialogue userVideoDialogue = new TB_UserVideoDialogue();
                                userVideoDialogue.UserID = input.UserId;
                                userVideoDialogue.UserVideoID = (int)i;
                                userVideoDialogue.DialogueNumber = t1.DialogueNumber;
                                userVideoDialogue.DialogueScore = float.Parse(t1.DialogueScore.ToString("f1"));
                                userVideoDialogue.VideoType = input.Type;
                                userVideoDialogue.VideoFileID = input.VideoFileId;
                                list.Add(userVideoDialogue);
                            }
                            object j = db.Insertable<TB_UserVideoDialogue>(list.ToArray()).ExecuteReturnIdentity();
                            if ((int)j <= 0)
                            {
                                result = false;
                            }
                        }
                    }
                    if (result)
                    {
                        db.Ado.CommitTran();
                        return Convert.ToInt32(i);
                    }
                    else
                    {
                        db.Ado.RollbackTran();
                    }
                }
                catch (Exception ex)
                {
                    Log4NetHelper.Error(LoggerType.ApiExceptionLog, "插入趣配音用户数据失败；info" + input.ToJson(), ex);
                    db.Ado.RollbackTran();

                }
            }
            return 0;
        }

        /// <summary>
        /// 校级榜
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse SchoolRankInfo(UserSchoolRankParaModel input)
        {
            UserSchoolRankReponse response = new UserSchoolRankReponse();
            List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + input.BookID, input.SchoolID + "_" + input.VideoNumber);
            if (rin != null && rin.Count > 0)
            {
                List<string> listKeys = new List<string>();
                foreach (var item in rin)
                {
                    listKeys.Add(item.VideoId);
                }
                List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetAll<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + input.BookID, listKeys);
                if (listVideoInfos != null && listVideoInfos.Count > 0)
                {
                    List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                    foreach (var item in listVideoInfos)
                    {
                        if (item != null)
                        {
                            var sortModel = new Redis_IntDubb_VideoInfoSort()
                            {
                                VideoId = item.VideoId,
                                UserId = item.UserId,
                                TrueName = item.TrueName,
                                TotalScore = Math.Floor(double.Parse(string.IsNullOrEmpty(item.TotalScore) ? "0" : item.TotalScore) * 10) / 10,
                                NumberOfOraise = item.NumberOfOraise.Count(),
                                UserImage = item.UserImage,
                                CreateTime = item.CreateTime,
                                Sort = 0
                            };
                            listVideoInfo.Add(sortModel);
                        }
                    }
                    if (listVideoInfo.Count > 0)
                    {
                        listVideoInfo = listVideoInfo.OrderByDescending(i => i.TotalScore)
                            .ThenByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                        for (int i = 0; i < listVideoInfo.Count; i++)
                        {
                            listVideoInfo[i].Sort = i + 1;
                            if (listVideoInfo[i].UserId == input.UserID)
                            {
                                #region 获取本地DB用户姓名和头像

                                string uName = "暂未填写";
                                string uImg = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";

                                var user = ibsService.GetUserInfoByUserId(Convert.ToInt32(input.UserID));
                                if (user != null)
                                {
                                    uImg = user.IsEnableOss != 0 ? _getOssFilesUrl + (string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage) : _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage);
                                    uName = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                                }

                                #endregion

                                listVideoInfo[i].TrueName = uName;
                                listVideoInfo[i].UserImage = uImg;
                                response.CurrentUserRank = listVideoInfo[i];
                            }
                        }

                        listVideoInfo = listVideoInfo.Skip((input.PageIndex - 1) * input.PageCount).Take(input.PageCount).ToList();
                        List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                        foreach (var item in listVideoInfo)
                        {
                            var temUser = ibsService.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
                            if (temUser != null)
                            {
                                listUser.Add(temUser);
                            }
                        }

                        #region 获取本地DB用户姓名和头像

                        List<Redis_IntDubb_VideoInfoSort> listVideoInfoResult = new List<Redis_IntDubb_VideoInfoSort>();
                        foreach (var item in listVideoInfo)
                        {
                            var us = listUser.FirstOrDefault(a => a.UserID == Convert.ToInt32(item.UserId));
                            if (us != null)
                            {
                                item.TrueName = string.IsNullOrEmpty(us.TrueName) ? "暂未填写" : us.TrueName;
                                item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl + (string.IsNullOrEmpty(us.UserImage) ? "00000000-0000-0000-0000-000000000000" : us.UserImage) : _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(us.UserImage) ? "00000000-0000-0000-0000-000000000000" : us.UserImage);
                            }
                            listVideoInfoResult.Add(item);
                        }
                        response.RankList = listVideoInfoResult;
                        return APIResponse.GetResponse(response.ToJson());

                        #endregion
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse("没有对应的配音数据！");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("没有对应的配音数据！");
                }
            }
            else
            {
                return APIResponse.GetErrorResponse("没有对应的排行榜数据！");
            }
        }

        /// <summary>
        /// 班级榜
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse ClassRankInfo(UserSchoolRankParaModel input)
        {
            string classId = "";
            if (input.ClassID.Length <= 8)
            {
                IBS_ClassUserRelation classInfo = ibsService.GetClassUserRelationByClassOtherId(input.ClassID, 1);
                if (classInfo != null)
                {
                    classId = classInfo.ClassID;
                }
            }
            else
            {
                classId = input.ClassID;
            }
            UserSchoolRankReponse response = new UserSchoolRankReponse();
            List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + input.BookID, classId.ToLower() + "_" + input.VideoNumber);
            if (rin != null && rin.Count > 0)
            {
                List<string> listKeys = new List<string>();
                foreach (var item in rin)
                {
                    listKeys.Add(item.VideoId);
                }
                List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetAll<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + input.BookID, listKeys);
                if (listVideoInfos != null && listVideoInfos.Count > 0)
                {
                    List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                    foreach (var item in listVideoInfos)
                    {
                        if (item != null)
                        {
                            var sortModel = new Redis_IntDubb_VideoInfoSort()
                            {
                                VideoId = item.VideoId,
                                UserId = item.UserId,
                                TrueName = item.TrueName,
                                TotalScore = Math.Floor(double.Parse(string.IsNullOrEmpty(item.TotalScore) ? "0" : item.TotalScore) * 10) / 10,
                                NumberOfOraise = item.NumberOfOraise.Count(),
                                UserImage = item.UserImage,
                                CreateTime = item.CreateTime,
                                Sort = 0
                            };
                            listVideoInfo.Add(sortModel);
                        }
                    }
                    if (listVideoInfo.Count > 0)
                    {
                        listVideoInfo = listVideoInfo.OrderByDescending(i => i.TotalScore).ThenByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                        for (int i = 0; i < listVideoInfo.Count; i++)
                        {
                            listVideoInfo[i].Sort = i + 1;
                            if (listVideoInfo[i].UserId == input.UserID)
                            {
                                #region 获取本地DB用户姓名和头像

                                string uName = "暂未填写";
                                string uImg = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";

                                var user = ibsService.GetUserInfoByUserId(Convert.ToInt32(input.UserID));
                                if (user != null)
                                {
                                    uImg = user.IsEnableOss != 0 ? _getOssFilesUrl + (string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage) : _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(user.UserImage) ? "00000000-0000-0000-0000-000000000000" : user.UserImage);
                                    uName = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                                }

                                #endregion

                                listVideoInfo[i].TrueName = uName;
                                listVideoInfo[i].UserImage = uImg;
                                response.CurrentUserRank = listVideoInfo[i];
                            }
                        }
                        listVideoInfo = listVideoInfo.Skip((input.PageIndex - 1) * input.PageCount).Take(input.PageCount).ToList();
                        List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                        foreach (var item in listVideoInfo)
                        {
                            var temUser = ibsService.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
                            if (temUser != null)
                            {
                                listUser.Add(temUser);
                            }
                        }

                        #region 获取本地DB用户姓名和头像

                        List<Redis_IntDubb_VideoInfoSort> listVideoInfoResult = new List<Redis_IntDubb_VideoInfoSort>();
                        foreach (var item in listVideoInfo)
                        {
                            var us = listUser.FirstOrDefault(a => a.UserID == Convert.ToInt32(item.UserId));
                            if (us != null)
                            {
                                item.TrueName = string.IsNullOrEmpty(us.TrueName) ? "暂未填写" : us.TrueName;
                                item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl + (string.IsNullOrEmpty(us.UserImage) ? "00000000-0000-0000-0000-000000000000" : us.UserImage) : _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(us.UserImage) ? "00000000-0000-0000-0000-000000000000" : us.UserImage);
                            }
                            listVideoInfoResult.Add(item);
                        }
                        response.RankList = listVideoInfoResult;
                        return APIResponse.GetResponse(response.ToJson());

                        #endregion
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse("没有对应的配音数据！");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("没有对应的配音数据！");
                }
            }
            else
            {
                return APIResponse.GetErrorResponse("没有对应的排行榜数据！");
            }
        }

        /// <summary>
        /// 最新榜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse NewRankInfo(UserSchoolRankParaModel input)
        {

            List<Redis_IntDubb_NewRank> rin = redis.Get<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + input.BookID, input.VideoNumber);
            if (rin != null && rin.Count > 0)
            {
                List<string> listKeys = new List<string>();
                foreach (var item in rin)
                {
                    listKeys.Add(item.VideoId);
                }
                List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetAll<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + input.BookID, listKeys);
                if (listVideoInfos != null && listVideoInfos.Count > 0)
                {
                    List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                    for (int i = 0; i < listVideoInfos.Count; i++)
                    {
                        if (listVideoInfos[i] != null)
                        {
                            var sortModel = new Redis_IntDubb_VideoInfoSort
                            {
                                VideoId = listVideoInfos[i].VideoId,
                                UserId = listVideoInfos[i].UserId,
                                TrueName = listVideoInfos[i].TrueName,
                                TotalScore = Math.Floor(double.Parse(string.IsNullOrEmpty(listVideoInfos[i].TotalScore) ? "0" : listVideoInfos[i].TotalScore) * 10) / 10,
                                NumberOfOraise = listVideoInfos[i].NumberOfOraise.Count(),
                                UserImage = listVideoInfos[i].UserImage,
                                CreateTime = listVideoInfos[i].CreateTime,
                                Sort = i + 1
                            };
                            listVideoInfo.Add(sortModel);
                        }
                    }
                    if (listVideoInfo.Count > 0)
                    {
                        listVideoInfo = listVideoInfo.OrderByDescending(i => DateTime.Parse(i.CreateTime)).Skip((input.PageIndex - 1) * input.PageCount).Take(input.PageCount).ToList();
                        List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                        foreach (var item in listVideoInfo)
                        {
                            var temUser = ibsService.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
                            if (temUser != null)
                            {
                                listUser.Add(temUser);
                            }
                        }
                        #region 获取本地DB用户姓名和头像
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfoResult = new List<Redis_IntDubb_VideoInfoSort>();
                        foreach (var item in listVideoInfo)
                        {
                            var us = listUser.FirstOrDefault(a => a.UserID == Convert.ToInt32(item.UserId));
                            if (us != null)
                            {
                                item.TrueName = string.IsNullOrEmpty(us.TrueName) ? "暂未填写" : us.TrueName;
                                item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl + (string.IsNullOrEmpty(us.UserImage) ? "00000000-0000-0000-0000-000000000000" : us.UserImage) : _getFilesUrl + "?FileID=" + (string.IsNullOrEmpty(us.UserImage) ? "00000000-0000-0000-0000-000000000000" : us.UserImage);
                            }
                            listVideoInfoResult.Add(item);
                        }
                        return APIResponse.GetResponse(listVideoInfoResult.ToJson());
                        #endregion
                    }
                    else
                    {
                        return APIResponse.GetErrorResponse("没有对应的配音数据！");
                    }
                }
                else
                {
                    return APIResponse.GetErrorResponse("没有对应的配音数据！");
                }
            }
            else
            {
                return APIResponse.GetErrorResponse("没有对应的排行榜数据！");
            }
        }

        /// <summary>
        /// 获取已发布视频排行信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse VideoRankingInfo(UserVideoDetails input)
        {
            string where = "";
            if (input.Type == 1)
            {
                where = "a.State = '" + input.State + "' and VideoID='" + input.VideoID + "'";
            }
            else
            {
                //TODO：APPID是GUID，需要换成int查询（未实现，需胡江成协助）
                where = "a.BookID='" + input.BookID + "' AND a.VideoNumber='" + input.VideoNumber + "' AND a.VersionID = '" + input.AppID + "' AND a.State = '" + input.State + "'";
            }

            SqlParameter[] ps =
            {
                new SqlParameter("@PageIndex", SqlDbType.Int),
                new SqlParameter("@PageCount", SqlDbType.Int),
                new SqlParameter("@Where", SqlDbType.VarChar)
            };
            ps[0].Value = input.PageIndex;
            ps[1].Value = input.PageCount;
            ps[2].Value = where;

            List<VideoRankingInfo> vri = InterestDubbingRepository.ExecuteProcedure("Get_VideoRankingInfo", ps);

            List<RankInfo> rankInfoList = new List<RankInfo>();
            RankInfo rankInfo = new RankInfo();
            int num = 0;
            if (vri != null)
            {
                foreach (var item in vri.OrderByDescending(i => i.CreateTime))
                {
                    string img = "";
                    string fileid = "";
                    if (input.IsEnableOss == 0)
                    {
                        img = item.UserImage;
                        fileid = item.VideoFileID;
                    }
                    else
                    {
                        string time = Convert.ToDateTime(item.CreateTime).ToString("yyyy/MM/dd");
                        img = item.UIsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                        fileid = item.IsEnableOss != 0 ? _getOssFilesUrl + item.VideoFileID : _getVideoFiles + time + "/" + item.VideoFileID;
                    }

                    string tName = "";
                    if (!string.IsNullOrEmpty(item.TrueName))
                    {
                        tName = item.TrueName;
                    }
                    else
                    {
                        tName = "暂未填写";
                    }
                    rankInfo.ID = item.ID;
                    rankInfo.UserID = item.ID;
                    rankInfo.CreateTime = item.CreateTime;
                    rankInfo.NumberOfOraise = item.NumberOfOraise;
                    rankInfo.TotalScore = item.TotalScore;
                    rankInfo.VideoTitle = item.VideoTitle;
                    rankInfo.UserName = tName;
                    rankInfo.UserImage = img;
                    rankInfo.NickName = tName;
                    rankInfo.VideoFileId = fileid;
                    rankInfoList.Add(rankInfo);
                }
                return APIResponse.GetResponse(rankInfoList.ToJson());
            }
            else
            {
                return APIResponse.GetErrorResponse("视频信息不存在！");
            }
        }

        /// <summary>
        ///  获取视频详细成绩相关信息 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public APIResponse VideoAchievementInfo(VideoAchievement input)
        {
            string Title = "";

            TB_UserVideoDetails uVideo = InterestDubbingRepository.GetByID<TB_UserVideoDetails>(input.ID);

            int bookid = 0;
            if (uVideo != null)
            {
                //TODO：通过书籍ID与视频序号查询视频标题（需新mod提供字段）
                //bookid = UVideo.BookID;
                //sql = string.Format(@"SELECT TOP 1 VideoTitle FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE BookID={0} AND VideoNumber={1} ", UVideo.BookID, UVideo.VideoNumber);

                //DataSet dsTitle = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                //if (dsTitle.Tables.Count > 0)
                //{
                //    if (dsTitle.Tables[0].Rows.Count > 0)
                //    {
                //        Title = dsTitle.Tables[0].Rows[0][0].ToString();
                //    }
                //}
            }

            List<TB_UserVideoDialogue> dsDialog = InterestDubbingRepository.SelectSearch<TB_UserVideoDialogue>(i => i.UserVideoID == input.ID);

            int isBool = 0;
            int numberOfOraise = 0;
            Redis_IntDubb_VideoInfo intVideoInfo = redis.Get<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bookid, input.ID.ToString());
            if (intVideoInfo != null)
            {
                if (intVideoInfo.NumberOfOraise.Contains(input.UserID))
                {
                    isBool = 1;
                }
                numberOfOraise = intVideoInfo.NumberOfOraise.Count;
            }
            else
            {
                isBool = 0;
            }

            if (uVideo != null)
            {
                IBS_UserInfo ibsUserinfo = ibsService.GetUserInfoByUserId(uVideo.UserID);

                string img = "";
                string fileid = "";
                string trueName = "";
                string userid = "";
                if (ibsUserinfo != null)
                {
                    trueName = ibsUserinfo.TrueName;
                    userid = ibsUserinfo.UserID.ToString();
                    img = ibsUserinfo.IsEnableOss.ToString() != "0" ? _getOssFilesUrl + ibsUserinfo.UserImage : _getFilesUrl + "?FileID=" + ibsUserinfo.UserImage;
                }

                if (input.IsEnableOss == 0)
                {
                    if (uVideo.IsEnableOss.ToString() == "1")
                    {
                        string[] str = uVideo.VideoFileID.Split('/');
                        fileid = str[str.Length - 1].Split('.')[0];
                    }
                    else
                    {
                        fileid = uVideo.VideoFileID;
                    }
                }
                else
                {
                    string time = Convert.ToDateTime(uVideo.CreateTime).ToString("yyyy/MM/dd");
                    fileid = uVideo.IsEnableOss.ToString() != "0" ? _getOssFilesUrl + uVideo.VideoFileID : _getVideoFiles + time + "/" + uVideo.VideoFileID + ".mp4";
                }

                UserVideoAchievement userVideo = new UserVideoAchievement();
                userVideo.ID = uVideo.ID;
                userVideo.UserID = userid;
                userVideo.CreateTime = uVideo.CreateTime;
                userVideo.NumberOfOraise = numberOfOraise;
                userVideo.TotalScore = uVideo.TotalScore;
                userVideo.UserName = trueName;
                userVideo.VideoTitle = Title;
                userVideo.VideoType = uVideo.VideoType;
                userVideo.UserImage = img;
                userVideo.NickName = trueName;
                userVideo.State = uVideo.State;
                userVideo.IsBool = isBool;
                userVideo.VideoFileId = fileid;

                if (dsDialog != null)
                {
                    List<UserVideoChildren> childrenList = new List<UserVideoChildren>();
                    foreach (var item in dsDialog)
                    {
                        if (uVideo.ID == item.UserVideoID)
                        {
                            UserVideoChildren children = new UserVideoChildren
                            {
                                DialogueNumber = item.DialogueNumber,
                                DialogueScore = item.DialogueScore
                            };
                            childrenList.Add(children);
                        }
                    }
                    userVideo.children = childrenList.ToArray();
                }
                return APIResponse.GetResponse(userVideo.ToJson());
            }
            return APIResponse.GetErrorResponse("没有对应的数据！");

        }

        /// <summary>
        /// 增加点赞数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public APIResponse AddNumberOfOraise(NumberOfOraiseState input)
        {
            TB_UserVideoDetails userVideoDetails = InterestDubbingRepository.GetByID<TB_UserVideoDetails>(input.ID);
            if (userVideoDetails != null)
            {
                Redis_IntDubb_VideoInfo intVideoInfo = redis.Get<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + userVideoDetails.BookID, input.ID.ToString());
                if (intVideoInfo != null)
                {
                    List<string> list = intVideoInfo.NumberOfOraise;
                    if (input.State == 1)
                    {
                        if (!intVideoInfo.NumberOfOraise.Contains(input.UserID.ToString()))
                        {
                            list.Add(input.UserID.ToString());
                        }
                    }
                    else if (input.State == 0)
                    {
                        if (!intVideoInfo.NumberOfOraise.Contains(input.UserID.ToString()))
                        {
                            list.Remove(input.UserID.ToString());
                        }
                    }

                    intVideoInfo.NumberOfOraise = list;

                    redis.Set<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + userVideoDetails.BookID, input.ID.ToString(), intVideoInfo);
                    return APIResponse.GetResponse("点赞成功");
                }
            }
            return APIResponse.GetErrorResponse("点赞失败");
        }

        /// <summary>
        /// 根据app(guid)获取appid（int）
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public int ConvertAppVersion(string appId)
        {
            int versionId = 0;
            switch (appId)
            {

                case "0a94ceaf-8747-4266-bc05-ed8ae2e7e410":
                    versionId = (int)AppVersionTypeEnum.BjVersionId;
                    break;
                case "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385":
                    versionId = (int)AppVersionTypeEnum.GzVersionId;
                    break;
                case "241ea176-fce7-4bd7-a65f-a7978aac1cd2":
                    versionId = (int)AppVersionTypeEnum.SzVersionId;
                    break;
                case "37ca795d-42a6-4117-84f3-f4f856e03c62":
                    versionId = (int)AppVersionTypeEnum.GdVersionId;
                    break;
                case "41efcd18-ad8c-4585-8b6c-e7b61f49914c":
                    versionId = (int)AppVersionTypeEnum.XkbVersionId;
                    break;
                case "43716a9b-7ade-4137-bdc4-6362c9e1c999":
                    versionId = (int)AppVersionTypeEnum.ShbdVersionId;
                    break;
                case "5373bbc9-49d4-47df-b5b5-ae196dc23d6d":
                    versionId = (int)AppVersionTypeEnum.RjpepVersionId;
                    break;
                case "64a8de22-cea0-4026-ab36-5a70f94dd6e4":
                    versionId = (int)AppVersionTypeEnum.RjxqdVersionId;
                    break;
                case "333d7cfc-cb4f-49d2-8ded-025e7d0fe766":
                    versionId = (int)AppVersionTypeEnum.JsylVersionId;
                    break;
                case "8170b2bf-82a8-4c2d-9458-ae9d43cac5e3":
                    versionId = (int)AppVersionTypeEnum.RjVersionId;
                    break;
                case "9426808e-da8e-488c-9827-b082c19b62a7":
                    versionId = (int)AppVersionTypeEnum.ShqgVersionId;
                    break;
                case "f0a9e1a7-b4cf-4a37-8fd1-932a66070afa":
                    versionId = (int)AppVersionTypeEnum.SdVersionId;
                    break;
                default:
                    versionId = -1;
                    break;
            }
            return versionId;
        }
    }
}
