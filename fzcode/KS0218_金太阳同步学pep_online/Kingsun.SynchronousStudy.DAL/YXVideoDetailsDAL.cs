using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class YXVideoDetailsDAL : BaseManagement
    {
        BaseManagement manage = new BaseManagement();
        /// <summary>
        ///  通过 BookID、FirstTitleID、SecondTitleID、ModuleID 获取模块下视频信息列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet GetVideoInforList(string where)
        {
            string sql = string.Format(@"SELECT DISTINCT a.[ID] ,
                                                    a.[BookID] ,
                                                    a.BookName ,
                                                    a.[FirstTitleID] ,
                                                    a.[FirstTitle] ,
                                                    a.[SecondTitleID] ,
                                                    a.[SecondTitle] ,
                                                    a.[FirstModularID] ,
                                                    a.[FirstModular] ,
                                                    a.[SecondModularID] ,
                                                    a.[SecondModular] ,
                                                    a.[VideoTitle] ,
                                                    a.[VideoNumber] ,
                                                    a.[MuteVideo] ,
                                                    a.[CompleteVideo] ,
                                                    a.[VideoTime] ,
                                                    a.[BackgroundAudio] ,
                                                    a.[VideoCover] ,
                                                    a.[VideoDesc] ,
                                                    a.[VideoDifficulty] ,
                                                    a.[CreateTime] ,
                                                    b.ID 'BID' ,
                                                    b.VideoID ,
                                                    b.DialogueText ,
                                                    b.DialogueNumber ,
                                                    b.StartTime ,
                                                    b.EndTime ,
                                                    b.CreateTime 'BCreateTime'
                                            FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails_YX] a
                                                    LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDialogue_YX] b ON a.BookID = b.BookID
                                                                                                          AND a.VideoNumber = b.VideoID
                                            WHERE {0}", where);
            return ExecuteSql(sql);
        }
        /// <summary>
        /// 查询所有TB_VideoDetails
        /// </summary>
        /// <returns></returns>
        public IList<TB_VideoDetails_YX> GetVideoList()
        {
            IList<TB_VideoDetails_YX> list = SelectAll<TB_VideoDetails_YX>();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VideoDetails_YX> GetVideoList(string where)
        {
            IList<TB_VideoDetails_YX> list = Search<TB_VideoDetails_YX>(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VideoDialogue_YX> GetVideoDialogueList(string where)
        {
            IList<TB_VideoDialogue_YX> list = Search<TB_VideoDialogue_YX>(where);
            return list;
        }

        /// <summary>
        /// 通过视频编号获取视频信息
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public TB_VideoDetails_YX GetVideoInfoByID(int bookId, int videoNumber)
        {
            return SelectByCondition<TB_VideoDetails_YX>("BookID=" + bookId + " AND VideoNumber=" + videoNumber);
        }

        /// <summary>
        /// 
        /// 通过视频编号获取视频信息
        /// </summary>
        /// <param name="videoid"></param>
        /// <returns></returns>
        public TB_UserVideoDetails_YX GetUserVideoInfoByID(string videoid)
        {
            return SelectByCondition<TB_UserVideoDetails_YX>("ID=" + videoid);
        }

        public TB_UserActiveVideo GetUserActiveVideoByID(string uservideoid)
        {
            return SelectByCondition<TB_UserActiveVideo>("UserVideoID=" + "'" + uservideoid + "'");
        }

        public TB_UserVideoDetails_YX GetVideoInfoByVideoFileID(string videoid)
        {
            return SelectByCondition<TB_UserVideoDetails_YX>("VideoFileID=" + "'" + videoid + "'");
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool UpdateVideoInfo(TB_VideoDetails_YX videoInfo)
        {
            return Update<TB_VideoDetails_YX>(videoInfo);
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool UpdateUserVideoInfo(TB_UserVideoDetails_YX videoInfo)
        {
            return Update<TB_UserVideoDetails_YX>(videoInfo);
        }

        /// <summary>
        /// 更新视频对话信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool UpdateVideoDialogue(TB_VideoDialogue_YX videoDialogueInfo)
        {
            return Update<TB_VideoDialogue_YX>(videoDialogueInfo);
        }

        /// <summary>
        /// 添加视频对话信息
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public bool InsertUserVideoInfo(TB_UserVideoDetails_YX videoInfo)
        {
            return manage.Insert<TB_UserVideoDetails_YX>(videoInfo);
        }


        public bool InsertActiveVideo(TB_UserActiveVideo videoInfo)
        {
            return manage.Insert<TB_UserActiveVideo>(videoInfo);
        }

        /// <summary>
        /// 添加视频对话信息
        /// </summary>
        /// <param name="videoDialogueInfo"></param>
        /// <returns></returns>
        public bool InsertVideoDialogueInfo(TB_UserVideoDialogue_YX videoDialogueInfo)
        {
            return manage.Insert<TB_UserVideoDialogue_YX>(videoDialogueInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetVideoDetails(string where)
        {
            var ds = ExecuteSql("select * from [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails_YX]  where " + where);
            if (ds != null && ds.Tables.Count > 0)
            {
                var dt = ds.Tables[0];


                object obj = new
                {
                    rows = FillData<TB_UserVideoDetails_YX>(dt)
                };

                return obj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserVideoDetails_YX> GetUserVideoList(string where)
        {
            IList<TB_UserVideoDetails_YX> list = Search<TB_UserVideoDetails_YX>(where);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<UserVideoDetails> GetUserVideoDetailsByWhere(string where)
        {
            DataSet ds = ExecuteSql(where);
            if (ds != null)
            {
                // DataTable dt = ds.Tables[0];
                List<UserVideoDetails> listUserVideoDetails = DataSetToIList<UserVideoDetails>(ds, 0);
                return listUserVideoDetails;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 通过条件获取投票记录
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_VoteRecord> GetVoteRecord(string where)
        {
            IList<TB_VoteRecord> list = Search<TB_VoteRecord>(where);
            return list;
        }


        /// <summary>
        /// 添加新的投票记录
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        public bool InsertVoteRecord(TB_VoteRecord recordInfo)
        {
            return manage.Insert<TB_VoteRecord>(recordInfo);
        }

    }
}
