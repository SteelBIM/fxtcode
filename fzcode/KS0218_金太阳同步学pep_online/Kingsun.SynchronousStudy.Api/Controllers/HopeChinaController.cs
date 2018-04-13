using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.Api.Filter;

namespace Kingsun.SynchronousStudy.Api.Controllers
{
    public class HopeChinaController : ApiController
    {
        private KingRequest req = new KingRequest();
        KingResponse res = null;
        object obj = null;

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUrl([FromBody] KingRequest request)
        {
            return KingResponse.GetErrorResponseMessage("未完成");
        }

        /// <summary>
        /// 获取参赛组别内的题目列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetArticleListByUserId([FromBody]KingRequest request)
        {

            TempClass parameter = JsonHelper.DecodeJson<TempClass>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            if (parameter.Userid == null)
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            //查询用户信息
            UserinfoImplement userimplement = new UserinfoImplement();
            obj = new { Userid = parameter.Userid };
            req.Data = JsonHelper.EncodeJson(obj);
            req.Function = "QueryUserinfoById";
            res = userimplement.ProcessRequest(req);
            if (res.Success)
            {
                //根据用户报名信息查询文章列表
                TbUserInfo userinfo = (TbUserInfo)res.Data;
                ArticleImplement articleimplement = new ArticleImplement();
                obj = new { AContent = "APeriod='" + userinfo.Period.Trim() + "'" };
                req.Data = JsonHelper.EncodeJson(obj);
                req.Function = "QueryArticleByWhere";
                res = articleimplement.ProcessRequest(req);
                if (res.Success)
                {
                    if (res.Data != null)
                    {
                        var list = (IList<TbArticle>)res.Data;
                        if (list.Count > 0)
                        {
                            var resultlist = new List<TbArticle>();
                            foreach (var item in list)
                            {
                                var tbarticle = new TbArticle();
                                tbarticle = item;
                                tbarticle.APeriod = SetPeriod(item.APeriod);
                                resultlist.Add(tbarticle);
                            }

                            return KingResponse.GetResponseMessage(null, resultlist);
                        }
                        return KingResponse.GetResponseMessage(null, res.Data);
                    }
                    else
                    {
                        return KingResponse.GetErrorResponseMessage("参数错误");
                    }
                }
            }

            return KingResponse.GetErrorResponseMessage("获取信息错误");


        }

        /// <summary>
        /// 获取参赛者的题目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetArticleById([FromBody] KingRequest request)
        {
            TempClass parameter = JsonHelper.DecodeJson<TempClass>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            if (string.IsNullOrEmpty(parameter.Articleid))
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            if (string.IsNullOrEmpty(parameter.Userid))
            {
                return KingResponse.GetErrorResponseMessage("用户信息错误");
            }
            //查询用户是否能继续做题 如果超过三次 则返回空
            ScoreImplement scoreimplement = new ScoreImplement();
            int completeTimes = 0;
            if (string.IsNullOrEmpty(parameter.Id))
            {
                obj = new { Userid = parameter.Userid };
                req.Data = JsonHelper.EncodeJson(obj);
                req.Function = "QueryScoreByUserId";
                res = scoreimplement.ProcessRequest(req);

                if (res.Success)
                {
                    List<TbScore> list = (List<TbScore>)res.Data;
                    completeTimes = list.Count;
                }
            }
            //
            ArticleImplement articleimplement = new ArticleImplement();
            int iarticleid = 0;
            int.TryParse(parameter.Articleid, out iarticleid);
            obj = new { ID = iarticleid };
            req.Data = JsonHelper.EncodeJson(obj);
            req.Function = "QueryArticleById";
            res = articleimplement.ProcessRequest(req);
            var TaskTime = AppSetting.GetValue("TaskTime");
            if (res.Success)
            {
                if (res.Data != null)
                {
                    TbArticle article = new TbArticle();
                    article = (TbArticle)res.Data;
                    var obj1 = new { ID = article.ID, AContent = article.AContent, ATitle = article.ATitle, ARemark = article.ARemark, CompleteTimes = completeTimes, TaskTime = TaskTime };
                    return KingResponse.GetResponseMessage(null, obj1);
                }
                else
                {
                    return KingResponse.GetErrorResponseMessage("参数错误");
                }
            }

            return KingResponse.GetErrorResponseMessage("获取信息错误");
        }

        /// <summary>
        /// 上传每次的比赛成绩，开始做题上传一次后返回此次的id，提交成绩上传一次返回提交结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetScoreByUserId([FromBody] KingRequest request)
        {
            TempClass parameter = JsonHelper.DecodeJson<TempClass>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            if (string.IsNullOrEmpty(parameter.Userid))
            {
                return KingResponse.GetErrorResponseMessage("参数错误,无用户信息");
            }
            if (string.IsNullOrEmpty(parameter.Articleid))
            {
                return KingResponse.GetErrorResponseMessage("参数错误,无题目信息");
            }
            ScoreImplement scoreimplement = new ScoreImplement();
            if (string.IsNullOrEmpty(parameter.Id))
            {
                int iarticleid = 0;
                int.TryParse(parameter.Articleid, out iarticleid);
                obj = new { Articleid = iarticleid, Score = 0, Userid = parameter.Userid, Filepath = string.Empty };
                req.Data = JsonHelper.EncodeJson(obj);
                req.Function = "AddScore";
                res = scoreimplement.ProcessRequest(req);
                if (res.Success)
                {
                    TbScore score = new TbScore();
                    score = (TbScore)res.Data;
                    if (score != null)
                    {
                        return KingResponse.GetResponseMessage(null,
                            new { ID = score.ID, TaskTime = score.TaskTime });
                    }
                }
                return KingResponse.GetErrorResponseMessage("获取信息错误");
            }
            else
            {
                if (string.IsNullOrEmpty(parameter.Score))
                {
                    return KingResponse.GetErrorResponseMessage("参数错误,分数为0");
                }
                decimal iscore = 0m;
                decimal.TryParse(parameter.Score, out iscore);
                obj = new { ID = parameter.Id, Articleid = parameter.Articleid, Score = iscore, Userid = parameter.Userid, Filepath = parameter.Filepath };
                req.Data = JsonHelper.EncodeJson(obj);
                req.Function = "UpdScore";
                res = scoreimplement.ProcessRequest(req);
                if (res.Success)
                {
                    TbScore score = new TbScore();
                    score = (TbScore)res.Data;
                    if (score != null)
                    {
                        return KingResponse.GetResponseMessage(null, score.ID);
                    }
                }
                return KingResponse.GetErrorResponseMessage("获取信息错误");
            }
        }

        /// <summary>
        /// 提交试卷
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ShowApi]
        [HttpPost]
        public HttpResponseMessage SetFinishByUserId([FromBody] KingRequest request)
        {
            TempClass parameter = JsonHelper.DecodeJson<TempClass>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            if (string.IsNullOrEmpty(parameter.Userid))
            {
                return KingResponse.GetErrorResponseMessage("参数错误,无用户信息");
            }

            UserinfoImplement userimplement = new UserinfoImplement();
            obj = new { Userid = parameter.Userid };
            req.Data = JsonHelper.EncodeJson(obj);
            req.Function = "UpdUserFinish";
            res = userimplement.ProcessRequest(req);
            if (res.Success)
            {
                return KingResponse.GetResponseMessage(null, res.Data);
            }
            return KingResponse.GetErrorResponseMessage("参数错误,无用户信息");

        }

        /// <summary>
        /// 获取最后的比赛成绩
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetScoreUserId([FromBody] KingRequest request)
        {
            TempClass parameter = JsonHelper.DecodeJson<TempClass>(request.Data);
            if (parameter == null)
            {
                return KingResponse.GetErrorResponseMessage("参数错误");
            }
            if (string.IsNullOrEmpty(parameter.Userid))
            {
                return KingResponse.GetErrorResponseMessage("参数错误,无用户信息");
            }
            ScoreImplement scoreimplement = new ScoreImplement();
            if (string.IsNullOrEmpty(parameter.Id))
            {
                obj = new { Userid = parameter.Userid };
                req.Data = JsonHelper.EncodeJson(obj);
                req.Function = "QueryScoreByUserId";
                res = scoreimplement.ProcessRequest(req);
                if (res.Success)
                {
                    return KingResponse.GetResponseMessage(null, res.Data);
                }
                else
                {
                    var matchEndTime = AppSetting.GetValue("MatchEndTime").ToIntOrZero();
                    var dateNow = DateTime.Now.ToString("yyyyMMdd").ToIntOrZero();
                    if (dateNow > matchEndTime)
                    {

                        return KingResponse.GetResponseMessage(null, null);

                    }
                }
            }
            return KingResponse.GetErrorResponseMessage("获取信息错误");
        }

        private string SetPeriod(string period)
        {
            string text = string.Empty;
            switch (period.Trim())
            {
                case "F1":
                    text = "小学1~2年级(F)";
                    break;
                case "F2":
                    text = "小学3~4年级(F)";
                    break;
                case "F3":
                    text = "小学5~6年级(F)";
                    break;
                case "F4":
                    text = "初中(F)";
                    break;
                case "F5":
                    text = "高中(F)";
                    break;
                case "F6":
                    text = "大学(F)";
                    break;
                default:
                    text = "错误";
                    break;
            }
            return text;
        }


        /// <summary>
        /// 用于参数传递
        /// </summary>
        public class TempClass
        {
            public string Userid { get; set; }
            public string Articleid { get; set; }
            public string Score { get; set; }
            public string Filepath { get; set; }
            public int Tasktime { get; set; }
            public string Id { get; set; }
        }
    }
}
