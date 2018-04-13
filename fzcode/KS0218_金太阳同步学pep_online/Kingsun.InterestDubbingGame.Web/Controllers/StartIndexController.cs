using Kingsun.InterestDubbingGame.BLL;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kingsun.InterestDubbingGame.Web.Controllers
{
    public class StartIndexController : Controller
    {
        static RedisHashHelper redis = new RedisHashHelper();
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //
        // GET: /StartIndex/
        public ActionResult Index()
        {
          
            string UserID = ""; string AppID = "";
            StartIndexStateModel model = null;
            if (Request["UserID"] == null || Request["UserID"] == "")
            {
                Response.Write("<script>alert('参数错误！UserID不能为空！')</script>");
                return View(model);
            }
            else if (Request["AppID"] == null || Request["AppID"] == "")
            {
                Response.Write("<script>alert('参数错误！AppID不能为空！')</script>");
                return View(model);
            }
            else
            {
                UserID = Request["UserID"];
                AppID = Request["AppID"];
                TB_InterestDubbingGame_UserInfo userInfo = redis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", UserID);
                model = GetStateInfo(UserID, AppID);
            }
            return View(model);
        }
        /// <summary>
        /// 获取按钮状态
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public StartIndexStateModel GetStateInfo(string UserID, string AppID)
        {
            try
            {
                StartIndexStateModel sim = new StartIndexStateModel();
                //判断用户是否报名 
                TB_InterestDubbingGame_UserInfo userInfo = redis.Get<TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", UserID);
                if (userInfo == null)//未报名
                {
                    sim.StateName = "开始比赛";
                    sim.StateID = -1;
                }
                else//已报名
                {
                    #region 已报名
                    //判断是否到比赛时间
                    DateTime? FirstGameStartTime;//初赛开始时间
                    DateTime? FirstGameEndTime;//初赛结束时间
                    List<TB_InterestDubbingGame_MatchTime> matchTime = new TB_InterestDubbingGame_MatchTimeBLL().GetList().Take(1).ToList();
                    if (matchTime != null && matchTime.Count > 0)
                    {
                        FirstGameStartTime = matchTime[0].FirstGameStartTime;
                        FirstGameEndTime = matchTime[0].FirstGameEndTime;
                        if (DateTime.Now >= FirstGameStartTime && DateTime.Now <= FirstGameEndTime)
                        {
                            //判断完成环节情况
                            Redis_InterestDubbingGame_UserTotalScore redisUserTotalScore = redis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", UserID);
                            if (redisUserTotalScore != null)
                            {
                                if (redisUserTotalScore.BookPlayScore > 0 && redisUserTotalScore.StoryReadScore > 0) //用户已完成两个环节
                                {
                                    sim.StateName = "查看成绩";
                                    sim.StateID = 3;//完成两个环节
                                }
                                else// if (redisUserTotalScore.BookPlayScore > 0 || redisUserTotalScore.StoryReadScore > 0)//用户已完成一个环节
                                {
                                    sim.StateName = "继续比赛";
                                    sim.StateID = 2;//完成一个环节
                                }
                            }
                            else //用户未完成任意环节
                            {
                                sim.StateName = "开始比赛";
                                sim.StateID = 1;//比赛开始
                            }
                        }
                        else
                        {
                            sim.StateName = "开始比赛";
                            sim.StateID = 0;//比赛未开始
                        }
                    }
                    #endregion
                }
                sim.UserID = UserID;
                sim.AppID = AppID;
                return sim;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                return null;
            }
        }
  
    }
    public class StartIndexStateModel
    {
        public string UserID { get; set; }
        public string AppID { get; set; }
        public string StateName { get; set; }
        /// <summary>
        /// -1:未报名,0:比赛未开始，1：比赛开始，2：完成一个环节，3：完成两个环节
        /// </summary>
        public int StateID { get; set; }
    }
}