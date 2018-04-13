using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CBSS.Core.Log;
using CBSS.Core.Utility;
using CBSS.Framework.Contract.API;
using CBSS.Tbx.Contract.ViewModel;

namespace CBSS.Web.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserRecordController
    {
        /// <summary>
        ///  获取趣配音模块资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetVideoInforList_YX(string inputStr)
        {
            v_BookInfo input;
            var verifyResult = tbxService.VerifyParam<v_BookInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            try
            {
                return tbxService.GetDubbingByCataId(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "获取趣配音模块视频信息失败", ex);
                return APIResponse.GetErrorResponse("获取趣配音模块视频信息失败！");
            }
        }

        /// <summary>
        /// 获取未发布/发布视频信息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse GetVideoList_YX(string inputStr)
        {
            VideoRequset input;
            var verifyResult = tbxService.VerifyParam<VideoRequset>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            try
            {
                return tbxService.GetVideoList(input.AppID, Convert.ToInt32(input.PageIndex), Convert.ToInt32(input.State), Convert.ToInt32(input.UserID), input.IsEnableOss);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "获取趣配音视频信息列表失败", ex);
                return APIResponse.GetErrorResponse("获取趣配音视频信息列表失败！");
            }
        }

        /// <summary>
        /// 修改已发布/未发布视频信息状态
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse UpdateVedioInfo_YX(string inputStr)
        {
            DeleteVedioInfo input;
            var verifyResult = tbxService.VerifyParam<DeleteVedioInfo>(inputStr, out input);
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            try
            {
                return tbxService.UpdateVedioInfo(input.State, input.IDStr);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "视频信息发布/删除失败；id=" + input.IDStr, ex);
                return APIResponse.GetErrorResponse("视频信息发布/删除失败！");
            }
        }

        /// <summary>
        /// 插入用户配音视频信息 △
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public static APIResponse InsertVideoInfo_YX(string inputStr)
        {
            UserVideoInfo input;
            var verifyResult = tbxService.VerifyParam<UserVideoInfo>(inputStr, out input, new List<string> { "VideoType", "BookId", "State", "VersionType", "Type" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            if (input.VideoType != "1")
            {
                if (string.IsNullOrEmpty(input.BookId))
                {
                    return APIResponse.GetErrorResponse("BookID为空！");
                }
            }

            if (string.IsNullOrEmpty(input.State))
            {
                input.State = "2";
            }
            if (string.IsNullOrEmpty(input.VersionType))
            {
                input.VersionType = "1";
            }
            if (string.IsNullOrEmpty(input.Type))
            {
                input.Type = "0";
            }
            if (string.IsNullOrEmpty(input.VideoType))
            {
                input.VideoType = "0";
            }
            try
            {
                return tbxService.InsertVideoInfo(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "插入趣配音用户数据失败；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("插入趣配音用户数据失败！");
            }
        }

        /// <summary>
        /// 校级榜
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse SchoolRankInfo_YX(string inputStr)
        {
            UserSchoolRankParaModel input;
            var verifyResult = tbxService.VerifyParam<UserSchoolRankParaModel>(inputStr, out input, new List<string> { "ClassID" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            try
            {
                return tbxService.SchoolRankInfo(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "没有对应的数据；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("没有对应的校级排行榜数据！");
            }
        }

        /// <summary>
        /// 班级榜
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse ClassRankInfo_YX(string inputStr)
        {
            UserSchoolRankParaModel input;
            var verifyResult = tbxService.VerifyParam<UserSchoolRankParaModel>(inputStr, out input, new List<string> { "SchoolID" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            try
            {
                return tbxService.ClassRankInfo(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "没有对应的数据；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("没有对应的班级排行榜数据！");
            }
        }

        /// <summary>
        /// 最新榜单
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse NewRankInfo_YX(string inputStr)
        {
            UserSchoolRankParaModel input;
            var verifyResult = tbxService.VerifyParam<UserSchoolRankParaModel>(inputStr, out input, new List<string> { "SchoolID", "ClassID", "UserID" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            try
            {
                return tbxService.NewRankInfo(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "没有对应的数据；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("没有对应的班级排行榜数据！");
            }
        }

        /// <summary>
        /// 获取已发布视频排行信息
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse VideoRankingInfo_YX(string inputStr)
        {
            UserVideoDetails input;
            var verifyResult = tbxService.VerifyParam<UserVideoDetails>(inputStr, out input, new List<string> { "Type", "VideoID", "VersionType", "IsEnableOss" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            if (string.IsNullOrEmpty(input.Type.ToString()))
            {
                input.Type = 0;
            }

            try
            {
                return tbxService.VideoRankingInfo(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "没有对应的数据；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("没有对应的班级排行榜数据！");
            }
        }

        /// <summary>
        ///  获取视频详细成绩相关信息
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse VideoAchievementInfo_YX(string inputStr)
        {
            VideoAchievement input;
            var verifyResult = tbxService.VerifyParam<VideoAchievement>(inputStr, out input, new List<string> { "IsEnableOss" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }

            try
            {
                return tbxService.VideoAchievementInfo(input);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "没有对应的数据；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("没有对应的班级排行榜数据！");
            }
        }

        /// <summary>
        /// 增加点赞数
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResponse AddNumberOfOraise_YX(string inputStr)
        {
            NumberOfOraiseState input;
            var verifyResult = tbxService.VerifyParam<NumberOfOraiseState>(inputStr, out input, new List<string> { "IsEnableOss" });
            if (!verifyResult.Success)
            {
                return verifyResult;
            }
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(LoggerType.ApiExceptionLog, "没有对应的数据；info" + input.ToJson(), ex);
                return APIResponse.GetErrorResponse("修改状态失败！");

            }
        }
    }
}