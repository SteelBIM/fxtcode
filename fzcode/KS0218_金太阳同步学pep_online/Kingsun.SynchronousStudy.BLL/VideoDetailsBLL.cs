using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class VideoDetailsBLL
    {
        VideoDetailsDAL videoDetailsDAL = new VideoDetailsDAL();

        /// <summary>
        ///  通过 BookID、FirstTitleID、SecondTitleID、ModuleID 获取模块下视频信息列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetVideoInforList(string where)
        { 
            return videoDetailsDAL.GetVideoInforList(where);
        }
        /// <summary>
        /// 查询所有TB_VideoDetails
        /// </summary>
        /// <returns></returns>
        public IList<TB_VideoDetails> GetVideoList()
        {
            IList<TB_VideoDetails> list = videoDetailsDAL.GetVideoList();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VideoDetails> GetVideoList(string where)
        {
            IList<TB_VideoDetails> list = videoDetailsDAL.GetVideoList(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VideoDialogue> GetVideoDialogueList(string where)
        {
            IList<TB_VideoDialogue> list = videoDetailsDAL.GetVideoDialogueList(where);
            return list;
        }

        /// <summary>
        /// 通过视频编号获取视频信息
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public TB_VideoDetails GetVideoInfoByID(int bookId, int videoNumber)
        {
            return videoDetailsDAL.GetVideoInfoByID(bookId, videoNumber);
        }

        /// <summary>
        /// 通过视频编号获取视频信息
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public TB_UserVideoDetails GetUserVideoInfoByID(string videoid)
        {
            return videoDetailsDAL.GetUserVideoInfoByID(videoid);
        }

        /// <summary>
        /// 通过视频编号获取视频信息
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public TB_UserActiveVideo GetUserActiveVideoByID(string uservideoid)
        {
            return videoDetailsDAL.GetUserActiveVideoByID(uservideoid);
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool UpdateVideoInfo(TB_VideoDetails videoInfo)
        {
            return videoDetailsDAL.UpdateVideoInfo(videoInfo);
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool UpdateUserVideoInfo(TB_UserVideoDetails videoInfo)
        {
            return videoDetailsDAL.UpdateUserVideoInfo(videoInfo);
        }

        /// <summary>
        /// 更新视频对话信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool UpdateVideoDialogue(TB_VideoDialogue videoDialogueInfo)
        {
            return videoDetailsDAL.UpdateVideoDialogue(videoDialogueInfo);
        }

        /// <summary>
        /// 添加视频对话信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool InsertUserVideoInfo(TB_UserVideoDetails videoInfo)
        {
            return videoDetailsDAL.InsertUserVideoInfo(videoInfo);
        }

        /// <summary>
        /// 添加视频对话信息
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public bool InsertVideoDialogueInfo(TB_UserVideoDialogue videoDialogueInfo)
        {
            return videoDetailsDAL.InsertVideoDialogueInfo(videoDialogueInfo);
        }

        public object GetVideoDetails(string where)
        {
            return videoDetailsDAL.GetVideoDetails(where);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserVideoDetails> GetUserVideoList(string where)
        {
            IList<TB_UserVideoDetails> list = videoDetailsDAL.GetUserVideoList(where);
            return list;
        }

        /// <summary>
        /// 通过条件获取信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<UserVideoDetails> GetUserVideoDetailsByWhere(string where)
        {
            return videoDetailsDAL.GetUserVideoDetailsByWhere(where);
        }

        public TB_UserVideoDetails GetVideoInfoByVideoFileID(string videoid)
        {
            return videoDetailsDAL.GetVideoInfoByVideoFileID(videoid);
        }

        /// <summary>
        /// 通过条件获取投票记录
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VoteRecord> GetVoteRecord(string where)
        {
            IList<TB_VoteRecord> list = videoDetailsDAL.GetVoteRecord(where);
            return list;
        }

        /// <summary>
        /// 添加新的投票记录
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        public bool InsertVoteRecord(TB_VoteRecord recordInfo)
        {
            return videoDetailsDAL.InsertVoteRecord(recordInfo);
        }


        public bool InsertActiveVideo(TB_UserActiveVideo videoInfo)
        {
            return videoDetailsDAL.InsertActiveVideo(videoInfo);
        }
    }
}
