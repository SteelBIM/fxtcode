using Kingsun.IBS.BLL;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.HallowmasPage
{
    public partial class HallowmasPage : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            switch (action)
            {
                case "preventrepeatvote":
                    PreventRepeatVote();
                    break;
                case "getuserinfo":
                    GetUserInfo();
                    break;
                case "getnumberofprize":
                    GetNumberOfPrize();
                    break;
                case "getranklist":
                    GetRankList();
                    break;
                case "insertprize":
                    InsertPrize();
                    break;
                default:
                    break;
            }
        }

        #region
        /// <summary>
        /// 通过 userid 获取用户信息
        /// </summary>
        private void GetUserInfo()
        {
            try
            {
                string userID = Request.Form["UserID"];
                string videoID = Request.Form["VideoID"];
                if (String.IsNullOrEmpty(userID) || String.IsNullOrEmpty(videoID))
                {
                    Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "参数异常" }));
                    Response.End();
                }
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(userID));
                Tb_UserInfo userInfo = null;
                if (user != null) 
                {
                    userInfo = new Tb_UserInfo();
                    userInfo.UserID = Convert.ToInt32(user.UserID);
                    userInfo.TrueName = user.TrueName;
                    userInfo.UserName = user.UserName;
                    userInfo.UserRoles = user.UserRoles;
                    userInfo.TelePhone = user.TelePhone;
                    userInfo.NickName = user.TrueName;
                    userInfo.IsUser = user.IsUser;
                    userInfo.isLogState = user.isLogState;
                    userInfo.IsEnableOss = user.IsEnableOss;
                    userInfo.CreateTime = user.Regdate;
                    userInfo.AppId = user.AppID;
                }
                TB_UserVideoDetails userVideoInfo = videoDetailsBLL.GetVideoInfoByVideoFileID(videoID);

                if (userInfo != null && userVideoInfo != null)
                {
                    TB_UserActiveVideo userActiveVideoInfo = videoDetailsBLL.GetUserActiveVideoByID(videoID);
                    if (userActiveVideoInfo == null)
                    {
                        TB_UserActiveVideo activeVideoInfo = new TB_UserActiveVideo();
                        activeVideoInfo.UserVideoID = videoID;
                        activeVideoInfo.PrizeOne = 0;
                        activeVideoInfo.PrizeTwo = 0;
                        activeVideoInfo.PrizeThree = 0;
                        activeVideoInfo.PrizeFour = 0;
                        activeVideoInfo.PrizeFive = 0;
                        activeVideoInfo.CreateDate = DateTime.Now;
                        bool result = videoDetailsBLL.InsertActiveVideo(activeVideoInfo);
                        userVideoInfo.CreateTime = DateTime.Now;
                    }
                    else
                    {
                        userVideoInfo.CreateTime = userActiveVideoInfo.CreateDate;
                    }
                    string fiales = Convert.ToDateTime(userVideoInfo.CreateTime).ToString("yyyy/MM/dd").Replace('-', '/');
                    string url = "http://video.kingsun.cn/" + fiales + "/" + userVideoInfo.VideoFileID + ".mp4";
                    userVideoInfo.VideoFileID = url;
                    Response.Write(JsonHelper.EncodeJson(new { UserInfo = userInfo, UserVideoInfo = userVideoInfo, Success = true }));
                }
                else
                {
                    Response.Write(JsonHelper.EncodeJson(""));
                }
            }
            catch (Exception)
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "" }));
            }
            finally
            {
                Response.End();
            }
        }

        /// <summary>
        /// 获取点赞数目
        /// </summary>
        private void GetNumberOfPrize()
        {
            string videoID = Request.Form["VideoID"];
            if (String.IsNullOrEmpty(videoID))
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "参数异常" }));
                Response.End();
            }
            try
            {
                TB_UserActiveVideo userActiveVideoInfo = videoDetailsBLL.GetUserActiveVideoByID(videoID);
                if (userActiveVideoInfo != null)
                {
                    Response.Write(JsonHelper.EncodeJson(new { UserActiveVideoInfo = userActiveVideoInfo, Success = true }));
                }
                else
                {
                    Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "视频信息不存在" }));
                }
            }
            catch (Exception)
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "" }));
            }
            finally
            {
                Response.End();
            }
        }

        /// <summary>
        /// 获取点赞排行榜
        /// </summary>
        private void GetRankList()
        {
            string prize = Request.Form["Prize"];
            if (string.IsNullOrEmpty(prize))
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "参数异常！" }));
                Response.End();
            }
            try
            {
                string sql = string.Format(@"  SELECT TOP 10 a.{0} ,
                                                    c.UserID ,
                                                    c.UserName ,
                                                    c.NickName ,
                                                    c.UserImage
                                          FROM      dbo.TB_UserActiveVideo a,
                                                    dbo.[FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] b,
                                                    ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo c
                                          WHERE     a.UserVideoID = b.VideoFileID
                                                    AND b.UserID = c.UserID  AND c.IsUser=1 order by a.{0} desc", prize);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                List<object> list = new List<object>();
                if (ds.Tables[0].Rows.Count > 0 && null != ds.Tables[0])
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        object objects = new
                        {
                            UserID = ds.Tables[0].Rows[i]["UserID"],
                            UserName = ds.Tables[0].Rows[i]["UserName"],
                            NickName = ds.Tables[0].Rows[i]["NickName"],
                            UserImage = ds.Tables[0].Rows[i]["UserImage"],
                            Count = ds.Tables[0].Rows[i]["" + prize + ""]
                        };
                        list.Add(objects);
                    }
                    Response.Write(JsonHelper.EncodeJson(new { List = list, Success = true }));
                }
                else
                {
                    Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "没有对应的数据！" }));
                }
            }
            catch (Exception)
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "" }));
            }
            finally
            {
                Response.End();
            }

        }

        /// <summary>
        /// 新增点赞数
        /// </summary>
        private void InsertPrize()
        {
            try
            {
                string prize = Request.Form["Prize"];
                string videoID = Request.Form["VideoID"];
                string sql = string.Format(@"UPDATE dbo.TB_UserActiveVideo SET {0}={0}+1 WHERE UserVideoID='{1}'", prize, videoID);
                if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
                {
                    string userIP = Request.UserHostAddress.ToString();
                    //userIP = Request.ServerVariables.Get("Remote_Addr").ToString();
                    TB_VoteRecord voteInfo = new TB_VoteRecord();
                    voteInfo.UserIP = userIP;
                    voteInfo.VideoID = videoID;
                    voteInfo.CreateDate = DateTime.Now;
                    bool result = videoDetailsBLL.InsertVoteRecord(voteInfo);
                    Response.Write(JsonHelper.EncodeJson(new { Success = true }));
                }
                else
                {
                    Response.Write(JsonHelper.EncodeJson(new { Success = false }));
                }
            }
            catch (Exception)
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false }));
            }
            finally
            {
                Response.End();
            }
        }

        /// <summary>
        /// 通过IP，防止重复投票
        /// </summary>
        private void PreventRepeatVote()
        {
            string videoID = Request.Form["VideoID"];
            if (string.IsNullOrEmpty(videoID))
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "参数异常" }));
                Response.End();
            }
            try
            {
                string userIP = Request.UserHostAddress.ToString();
                string where = "1=1";
                where += " and VideoID = " + "'" + videoID + "'";
                where += " and UserIP = " + "'" + userIP + "'";
                IList<TB_VoteRecord> voteRecordList = videoDetailsBLL.GetVoteRecord(where);
                if (voteRecordList != null && voteRecordList.Count > 0)
                {
                    Response.Write(JsonHelper.EncodeJson(new { Success = true }));
                }
                else
                {
                    Response.Write(JsonHelper.EncodeJson(new { Success = false }));
                }
            }
            catch (Exception)
            {
                Response.Write(JsonHelper.EncodeJson(new { Success = false, Msg = "服务器内部错误" }));
            }
            finally
            {
                Response.End();
            }


        }
        #endregion

    }
}