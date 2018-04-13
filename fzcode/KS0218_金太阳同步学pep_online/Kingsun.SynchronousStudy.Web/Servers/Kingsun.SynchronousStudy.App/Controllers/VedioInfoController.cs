using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.SynchronousStudy.BLL;
using System.Web.Script.Serialization;
using log4net;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class VedioInfoController : ApiController
    {
        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();
        VideoDetailsBLL videoDetailsBLLs = new VideoDetailsBLL();

        string FiedURL = WebConfigurationManager.AppSettings["FileServerUrl"];
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];
        private readonly string _getVideoFiles = WebConfigurationManager.AppSettings["getVideoFiles"];

        BaseManagement bm = new BaseManagement();

        private RedisListHelper redisList = new RedisListHelper();
        static RedisHashHelper redis = new RedisHashHelper();

        /// <summary>
        /// 通过 BookID、FirstTitleID、SecondTitleID、ModuleID 获取模块下视频信息列表
        /// 修改为get请求。测试通过（2016年8月12日15:02:57）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage GetVideoInforListTest()
        {
            string strJson =
                "{\"BookID\":\"168\",\"FirstTitleID\":\"276296\",\"SecondTitleID\":\"276303\",\"FirstModularID\":\"2\"}";
            string where = "";
            VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
            JavaScriptSerializer js = new JavaScriptSerializer();
            BookInfo submitData = JsonHelper.DecodeJson<BookInfo>(strJson);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModularID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            where += "1=1 and a.BookID = " + ParseInt(submitData.BookID);
            where += " and a.FirstTitleID = " + ParseInt(submitData.FirstTitleID);
            where += " and a.FirstModularID = " + ParseInt(submitData.FirstModularID);
            if (!string.IsNullOrEmpty(submitData.SecondTitleID))
            {
                where += " and a.SecondTitleID = " + ParseInt(submitData.SecondTitleID);
            }
            try
            {
                StringBuilder json = new StringBuilder();
                DataSet set = videoDetailsBLL.GetVideoInforList(where);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = set.Tables[0];
                    json.Append("[");

                    string ID = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            ID = dt.Rows[i]["ID"].ToString();
                            json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"BookID\":\"" + dt.Rows[i]["BookID"] + "\",\"BookName\":\"" + String2Json(dt.Rows[i]["BookName"].ToString())
               + "\",\"FirstTitleID\":\"" + dt.Rows[i]["FirstTitleID"] + "\",\"FirstTitle\":\"" + String2Json(dt.Rows[i]["FirstTitle"].ToString()) + "\",\"SecondTitleID\":\""
               + dt.Rows[i]["SecondTitleID"].ToString() + "\",\"SecondTitle\":\"" + StringTOJson(dt.Rows[i]["SecondTitle"].ToString()) + "\",\"FirstModularID\":\""
               + dt.Rows[i]["FirstModularID"].ToString() + "\",\"FirstModular\":\"" + String2Json(dt.Rows[i]["FirstModular"].ToString()) + "\",\"SecondModularID\":\""
               + dt.Rows[i]["SecondModularID"].ToString() + "\",\"SecondModular\":\"" + String2Json(dt.Rows[i]["SecondModular"].ToString()) + "\",\"VideoTitle\":\""
               + String2Json(StringTOJson(dt.Rows[i]["VideoTitle"].ToString())) + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\",\"MuteVideo\":\""
               + StringTOJson(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + StringTOJson(dt.Rows[i]["CompleteVideo"].ToString()) + "\",\"VideoTime\":\""
               + StringTOJson(dt.Rows[i]["VideoTime"].ToString()) + "\",\"BackgroundAudio\":\"" + StringTOJson(dt.Rows[i]["BackgroundAudio"].ToString())
               + "\",\"VideoCover\":\"" + StringTOJson(dt.Rows[i]["VideoCover"].ToString()) + "\",\"VideoDesc\":\"" + StringTOJson(dt.Rows[i]["VideoDesc"].ToString())
               + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() + "\",\"CreateTime\":\"" + dt.Rows[i]["CreateTime"].ToString() + "\",\"children\":[");
                            json.Append("{\"ID\":\"" + dt.Rows[i]["BID"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoID"] + "\",\"DialogueText\":\""
                                   + String2Json(dt.Rows[i]["DialogueText"].ToString()) + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                   + "\",\"StartTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["EndTime"].ToString()))
                                   + "\",\"CreateTime\":\"" + dt.Rows[i]["BCreateTime"] + "\"},");
                        }
                        else
                        {
                            if (ID == dt.Rows[i]["ID"].ToString())
                            {
                                json.Append("{\"ID\":\"" + dt.Rows[i]["BID"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoID"] + "\",\"DialogueText\":\""
                                    + String2Json(dt.Rows[i]["DialogueText"].ToString()) + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                    + "\",\"StartTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["EndTime"].ToString()))
                                    + "\",\"CreateTime\":\"" + dt.Rows[i]["BCreateTime"] + "\"},");
                            }
                            else
                            {
                                json.Remove(json.Length - 1, 1);
                                json.Append("]},");
                                json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"BookID\":\"" + dt.Rows[i]["BookID"] + "\",\"BookName\":\"" + String2Json(dt.Rows[i]["BookName"].ToString())
              + "\",\"FirstTitleID\":\"" + dt.Rows[i]["FirstTitleID"] + "\",\"FirstTitle\":\"" + String2Json(dt.Rows[i]["FirstTitle"].ToString()) + "\",\"SecondTitleID\":\""
              + dt.Rows[i]["SecondTitleID"].ToString() + "\",\"SecondTitle\":\"" + String2Json(dt.Rows[i]["SecondTitle"].ToString()) + "\",\"FirstModularID\":\""
              + dt.Rows[i]["FirstModularID"].ToString() + "\",\"FirstModular\":\"" + String2Json(dt.Rows[i]["FirstModular"].ToString()) + "\",\"SecondModularID\":\""
              + dt.Rows[i]["SecondModularID"].ToString() + "\",\"SecondModular\":\"" + String2Json(dt.Rows[i]["SecondModular"].ToString()) + "\",\"VideoTitle\":\""
              + String2Json(dt.Rows[i]["VideoTitle"].ToString()) + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\",\"MuteVideo\":\""
              + StringTOJson(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + StringTOJson(dt.Rows[i]["CompleteVideo"].ToString()) + "\",\"VideoTime\":\""
              + StringTOJson(dt.Rows[i]["VideoTime"].ToString()) + "\",\"BackgroundAudio\":\"" + StringTOJson(dt.Rows[i]["BackgroundAudio"].ToString())
              + "\",\"VideoCover\":\"" + StringTOJson(dt.Rows[i]["VideoCover"].ToString()) + "\",\"VideoDesc\":\"" + StringTOJson(dt.Rows[i]["VideoDesc"].ToString())
              + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() + "\",\"CreateTime\":\"" + dt.Rows[i]["CreateTime"].ToString() + "\",\"children\":[");
                                json.Append("{\"ID\":\"" + dt.Rows[i]["BID"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoID"] + "\",\"DialogueText\":\""
                                   + String2Json(dt.Rows[i]["DialogueText"].ToString()) + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                   + "\",\"StartTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["EndTime"].ToString()))
                                   + "\",\"CreateTime\":\"" + dt.Rows[i]["BCreateTime"] + "\"},");

                            }
                            ID = dt.Rows[i]["ID"].ToString();
                        }
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]}]");
                }
                return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("没有更多数据");
        }
        /// <summary>
        /// 通过 BookID、FirstTitleID、SecondTitleID、ModuleID 获取模块下视频信息列表
        /// 修改为get请求。测试通过（2016年8月12日15:02:57）△
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetVideoInforList([FromBody]KingRequest request)
        {
            string where = "";
            VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
            JavaScriptSerializer js = new JavaScriptSerializer();
            BookInfo submitData = JsonHelper.DecodeJson<BookInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModularID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            where += "1=1 and a.BookID = " + ParseInt(submitData.BookID);
            where += " and a.FirstTitleID = " + ParseInt(submitData.FirstTitleID);
            where += " and a.FirstModularID = " + ParseInt(submitData.FirstModularID);
            if (!string.IsNullOrEmpty(submitData.SecondTitleID))
            {
                where += " and a.SecondTitleID = " + ParseInt(submitData.SecondTitleID);
            }
            try
            {
                StringBuilder json = new StringBuilder();

                DataSet set = videoDetailsBLL.GetVideoInforList(where);
                if (set != null && set.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = set.Tables[0];
                    json.Append("[");

                    string ID = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            ID = dt.Rows[i]["ID"].ToString();
                            json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"BookID\":\"" + dt.Rows[i]["BookID"] + "\",\"BookName\":\"" + String2Json(dt.Rows[i]["BookName"].ToString())
               + "\",\"FirstTitleID\":\"" + dt.Rows[i]["FirstTitleID"] + "\",\"FirstTitle\":\"" + String2Json(dt.Rows[i]["FirstTitle"].ToString()) + "\",\"SecondTitleID\":\""
               + dt.Rows[i]["SecondTitleID"].ToString() + "\",\"SecondTitle\":\"" + StringTOJson(dt.Rows[i]["SecondTitle"].ToString()) + "\",\"FirstModularID\":\""
               + dt.Rows[i]["FirstModularID"].ToString() + "\",\"FirstModular\":\"" + String2Json(dt.Rows[i]["FirstModular"].ToString()) + "\",\"SecondModularID\":\""
               + dt.Rows[i]["SecondModularID"].ToString() + "\",\"SecondModular\":\"" + String2Json(dt.Rows[i]["SecondModular"].ToString()) + "\",\"VideoTitle\":\""
               + String2Json(StringTOJson(dt.Rows[i]["VideoTitle"].ToString())) + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\",\"MuteVideo\":\""
               + StringTOJson(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + StringTOJson(dt.Rows[i]["CompleteVideo"].ToString()) + "\",\"VideoTime\":\""
               + StringTOJson(dt.Rows[i]["VideoTime"].ToString()) + "\",\"BackgroundAudio\":\"" + StringTOJson(dt.Rows[i]["BackgroundAudio"].ToString())
               + "\",\"VideoCover\":\"" + StringTOJson(dt.Rows[i]["VideoCover"].ToString()) + "\",\"VideoDesc\":\"" + StringTOJson(dt.Rows[i]["VideoDesc"].ToString())
               + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() + "\",\"CreateTime\":\"" + dt.Rows[i]["CreateTime"].ToString() + "\",\"children\":[");
                            json.Append("{\"ID\":\"" + dt.Rows[i]["BID"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoID"] + "\",\"DialogueText\":\""
                                   + String2Json(dt.Rows[i]["DialogueText"].ToString()) + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                   + "\",\"StartTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["EndTime"].ToString()))
                                   + "\",\"CreateTime\":\"" + dt.Rows[i]["BCreateTime"] + "\"},");
                        }
                        else
                        {
                            if (ID == dt.Rows[i]["ID"].ToString())
                            {
                                json.Append("{\"ID\":\"" + dt.Rows[i]["BID"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoID"] + "\",\"DialogueText\":\""
                                    + String2Json(dt.Rows[i]["DialogueText"].ToString()) + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                    + "\",\"StartTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["EndTime"].ToString()))
                                    + "\",\"CreateTime\":\"" + dt.Rows[i]["BCreateTime"] + "\"},");
                            }
                            else
                            {
                                json.Remove(json.Length - 1, 1);
                                json.Append("]},");
                                json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"BookID\":\"" + dt.Rows[i]["BookID"] + "\",\"BookName\":\"" + String2Json(dt.Rows[i]["BookName"].ToString())
              + "\",\"FirstTitleID\":\"" + dt.Rows[i]["FirstTitleID"] + "\",\"FirstTitle\":\"" + String2Json(dt.Rows[i]["FirstTitle"].ToString()) + "\",\"SecondTitleID\":\""
              + dt.Rows[i]["SecondTitleID"].ToString() + "\",\"SecondTitle\":\"" + String2Json(dt.Rows[i]["SecondTitle"].ToString()) + "\",\"FirstModularID\":\""
              + dt.Rows[i]["FirstModularID"].ToString() + "\",\"FirstModular\":\"" + String2Json(dt.Rows[i]["FirstModular"].ToString()) + "\",\"SecondModularID\":\""
              + dt.Rows[i]["SecondModularID"].ToString() + "\",\"SecondModular\":\"" + String2Json(dt.Rows[i]["SecondModular"].ToString()) + "\",\"VideoTitle\":\""
              + String2Json(dt.Rows[i]["VideoTitle"].ToString()) + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\",\"MuteVideo\":\""
              + StringTOJson(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + StringTOJson(dt.Rows[i]["CompleteVideo"].ToString()) + "\",\"VideoTime\":\""
              + StringTOJson(dt.Rows[i]["VideoTime"].ToString()) + "\",\"BackgroundAudio\":\"" + StringTOJson(dt.Rows[i]["BackgroundAudio"].ToString())
              + "\",\"VideoCover\":\"" + StringTOJson(dt.Rows[i]["VideoCover"].ToString()) + "\",\"VideoDesc\":\"" + StringTOJson(dt.Rows[i]["VideoDesc"].ToString())
              + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() + "\",\"CreateTime\":\"" + dt.Rows[i]["CreateTime"].ToString() + "\",\"children\":[");
                                json.Append("{\"ID\":\"" + dt.Rows[i]["BID"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoID"] + "\",\"DialogueText\":\""
                                   + String2Json(dt.Rows[i]["DialogueText"].ToString()) + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                   + "\",\"StartTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + StringTOJson(GetDate(dt.Rows[i]["EndTime"].ToString()))
                                   + "\",\"CreateTime\":\"" + dt.Rows[i]["BCreateTime"] + "\"},");

                            }
                            ID = dt.Rows[i]["ID"].ToString();
                        }
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]}]");
                }
                return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));

            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("没有更多数据");
        }

        /// <summary>
        /// 通过 BookID、FirstTitleID、SecondTitleID、ModuleID 获取模块下视频信息列表(未改前)
        /// 修改为get请求。测试通过（2016年8月12日15:02:57）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetVideoInforListAgo([FromBody]KingRequest request)
        {
            string where = "";
            VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
            JavaScriptSerializer js = new JavaScriptSerializer();
            BookInfo submitData = JsonHelper.DecodeJson<BookInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }

            if (string.IsNullOrEmpty(submitData.BookID) || string.IsNullOrEmpty(submitData.FirstTitleID) || string.IsNullOrEmpty(submitData.FirstModularID))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            where += "1=1 and a.BookID = " + ParseInt(submitData.BookID);
            where += " and a.FirstTitleID = " + ParseInt(submitData.FirstTitleID);
            if (!string.IsNullOrEmpty(submitData.SecondTitleID))
            {
                where += " and a.SecondTitleID = " + ParseInt(submitData.SecondTitleID);
            }
            where += " and a.FirstModularID = " + ParseInt(submitData.FirstModularID);
            //IList<TB_VideoDetails> videolist = videoDetailsBLL.GetVideoList(where);

            string sql = string.Format(@"SELECT  a.[ID] ,
                                                        a.[BookID] ,
                                                        b.TeachingNaterialName  AS BookName ,
                                                        a.[FirstTitleID] ,
                                                        b.[FirstTitle] ,
                                                        a.[SecondTitleID] ,
                                                        b.[SecondTitle] ,
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
                                                        a.[CreateTime]
                                                FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails] a LEFT JOIN dbo.TB_ModuleConfiguration b ON b.BookID = a.BookID AND a.FirstTitleID=b.FirstTitileID AND  ((b.SecondTitleID IS NULL AND a.SecondTitleID IS NULL) OR  a.SecondTitleID = b.SecondTitleID) WHERE {0}", where);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                IList<TB_VideoDetails> videolist = DataSetToIList<TB_VideoDetails>(ds, 0);
                if (videolist != null && videolist.Count > 0)
                {
                    string queryStr = "";
                    StringBuilder json = new StringBuilder();
                    json.Append("[");
                    IList<TB_VideoDialogue> videoDialogueList = new List<TB_VideoDialogue>();
                    for (int i = 0; i < videolist.Count; i++)
                    {
                        queryStr = "1=1";
                        queryStr += " and BookID = " + videolist[i].BookID;
                        queryStr += " and VideoID = " + videolist[i].VideoNumber;

                        videoDialogueList = videoDetailsBLL.GetVideoDialogueList(queryStr);

                        json.Append("{\"ID\":\"" + videolist[i].ID + "\",\"BookID\":\"" + videolist[i].BookID + "\",\"BookName\":\"" + String2Json(videolist[i].BookName)
                            + "\",\"FirstTitleID\":\"" + videolist[i].FirstTitleID + "\",\"FirstTitle\":\"" + String2Json(videolist[i].FirstTitle) + "\",\"SecondTitleID\":\""
                            + videolist[i].SecondTitleID + "\",\"SecondTitle\":\"" + String2Json(StringTOJson(videolist[i].SecondTitle)) + "\",\"FirstModularID\":\""
                            + videolist[i].FirstModularID + "\",\"FirstModular\":\"" + String2Json(StringTOJson(videolist[i].FirstModular)) + "\",\"SecondModularID\":\""
                            + videolist[i].SecondModularID + "\",\"SecondModular\":\"" + String2Json(StringTOJson(videolist[i].SecondModular)) + "\",\"VideoTitle\":\""
                            + String2Json(StringTOJson(videolist[i].VideoTitle)) + "\",\"VideoNumber\":\"" + videolist[i].VideoNumber + "\",\"MuteVideo\":\""
                            + StringTOJson(videolist[i].MuteVideo) + "\",\"CompleteVideo\":\"" + StringTOJson(videolist[i].CompleteVideo) + "\",\"VideoTime\":\""
                            + StringTOJson(videolist[i].VideoTime) + "\",\"BackgroundAudio\":\"" + StringTOJson(videolist[i].BackgroundAudio)
                            + "\",\"VideoCover\":\"" + StringTOJson(videolist[i].VideoCover) + "\",\"VideoDesc\":\"" + StringTOJson(videolist[i].VideoDesc)
                            + "\",\"VideoDifficulty\":\"" + videolist[i].VideoDifficulty + "\",\"CreateTime\":\"" + videolist[i].CreateTime + "\",\"children\":[");
                        if (videoDialogueList != null && videoDialogueList.Count > 0)
                        {
                            for (int j = 0; j < videoDialogueList.Count; j++)
                            {
                                json.Append("{\"ID\":\"" + videoDialogueList[j].ID + "\",\"VideoID\":\"" + videoDialogueList[j].VideoID + "\",\"DialogueText\":\""
                                    + String2Json(StringTOJson(videoDialogueList[j].DialogueText)) + "\",\"DialogueNumber\":" + videoDialogueList[j].DialogueNumber
                                    + ",\"StartTime\":\"" + StringTOJson(GetDate(videoDialogueList[j].StartTime)) + "\",\"EndTime\":\"" + StringTOJson(GetDate(videoDialogueList[j].EndTime))
                                    + "\",\"CreateTime\":\"" + videoDialogueList[j].CreateTime + "\"},");
                            }
                            json.Remove(json.Length - 1, 1);
                            json.Append("]},");
                        }
                        else
                        {
                            json.Append("]},");
                        }

                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("没有更多数据");
        }

        //DateTime时间格式转换为Unix时间戳格式
        private int DateTimeToStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// 获取未发布/发布视频信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetVideoListTest()
        {
            string strJson = "{\"UserID\":\"527248175\",\"Type\":\"1\",\"VersionType\":\"2\",\"PageIndex\":\"0\",\"PageSize\":\"10\",\"AppID\":\"241ea176-fce7-4bd7-a65f-a7978aac1cd2\",\"State\":\"1\"}";
            VideoRequset submitData = JsonHelper.DecodeJson<VideoRequset>(strJson);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.AppID) || string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.State) || string.IsNullOrEmpty(submitData.PageIndex) || string.IsNullOrEmpty(submitData.PageSize))
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            string appId = submitData.AppID;
            int pageIndex = Convert.ToInt32(submitData.PageIndex);
            int state = Convert.ToInt32(submitData.State);
            TB_APPManagement appInfo = new TB_APPManagement();
            string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
            appInfo = getAPPManagement(appId, path);// aPPManagementBLL.GetAPPManagement(appID);
            if (appInfo != null)
            {
                string where = "";
                where += " and UserID = " + Convert.ToInt32(submitData.UserID);
                where += " and State = " + state;
                where += " and VersionID = " + Convert.ToInt32(appInfo.VersionID);
                where += " and VideoType='0' ";
                where += " ORDER BY ID DESC ";

                string sql = string.Format(@"SELECT  ID ,
                                                        BookID ,
                                                        VideoNumber ,
                                                        VideoID ,
                                                        VideoImageAddress ,
                                                        VideoFileID ,
                                                        TotalScore ,
                                                        CreateTime ,
                                                        State ,
                                                        VideoType ,
                                                        IsEnableOss ,
                                                        UserID ,
                                                        VersionID
                                                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                                                WHERE   BookID IS NOT NULL
                                                        AND VideoNumber <> 0
                                                        AND BookID <> 0 {0}", where);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                IList<TB_UserVideoDetails> tb = JsonHelper.DataSetToIList<TB_UserVideoDetails>(ds, 0); //bm.Search<TB_GetVideoList>(where);
                List<VideoInfo> list = new List<VideoInfo>();

                string strSql = string.Format(@"SELECT  BookID ,
                                                            VideoNumber ,
                                                            VideoTitle
                                                    FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails]
                                                    WHERE   BookID IS NOT NULL
                                                            AND VideoNumber <> 0
                                                            AND BookID <> 0");
                DataSet dsTable = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
                List<videoDetailsInfo> details = JsonHelper.DataSetToIList<videoDetailsInfo>(dsTable, 0);
                // List<videoDetailsInfo> videoDetails = new List<videoDetailsInfo>();

                if (tb != null && tb.Count > 0)
                {
                    foreach (var item in tb)
                    {
                        string img = "";
                        string fileId = "";
                        if (submitData.IsEnableOss == 0)
                        {
                            img = StringTOJson(item.VideoImageAddress);
                            fileId = StringTOJson(item.VideoFileID);
                        }
                        else
                        {
                            string times = Convert.ToDateTime(item.CreateTime).ToString("yyyy/MM/dd");
                            img = item.IsEnableOss != 0 ? _getOssFilesUrl + StringTOJson(item.VideoImageAddress) : _getFilesUrl + "?FileID=" + StringTOJson(item.VideoImageAddress);
                            fileId = item.IsEnableOss != 0 ? _getOssFilesUrl + StringTOJson(item.VideoFileID) : _getVideoFiles + times + "/" + StringTOJson(item.VideoFileID);
                        }
                        DateTime time = new DateTime();
                        VideoInfo videoInfo = new VideoInfo();
                        string[] timeArr = new string[3];
                        videoInfo.ID = item.ID;
                        videoInfo.VideoID = item.VideoID; ;
                        videoInfo.VideoImageAddress = img;
                        videoInfo.VideoReleaseAddress = fileId;
                        videoInfo.TotalScore = Convert.ToInt32(item.TotalScore);
                        time = Convert.ToDateTime(item.CreateTime);
                        videoInfo.Month = time.Month.ToString();
                        timeArr = time.ToString("yyyy-MM-dd").Split('-');
                        videoInfo.Day = timeArr[2];
                        videoInfo.State = submitData.State;
                        videoInfo.VideoType = item.VideoType;
                        List<videoDetailsInfo> videoDetails = details.Where(i => i.BookID == item.BookID & i.VideoNumber == item.VideoNumber).ToList();
                        if (videoDetails.Count == 0)
                        {
                            videoInfo.VideoTitle = "";
                        }
                        else
                        {
                            videoInfo.VideoTitle = StringTOJson(videoDetails[0].VideoTitle);
                        }
                        list.Add(videoInfo);

                    }
                    list = list.Skip(pageIndex).Take(10).ToList();
                    return ObjectToJson.GetResult(list);
                }
                else
                {
                    return ObjectToJson.GetResult("", "没有更多数据");
                }
            }
            else
            {
                return ObjectToJson.GetResult("", "没有更多数据");
            }

        }


        /// <summary>
        /// 获取未发布/发布视频信息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetVideoListAgo([FromBody]KingRequest request)
        {
            VideoRequset submitData = JsonHelper.DecodeJson<VideoRequset>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.AppID) || string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.State) || string.IsNullOrEmpty(submitData.PageIndex) || string.IsNullOrEmpty(submitData.PageSize))
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            try
            {
                string s = "1";
                string sql = @"SELECT [State] FROM [TB_VersionChange] WHERE BooKID IS NULL ";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["State"].ToString() == "False")
                    {
                        s = "0";
                    }
                }

                string appID = submitData.AppID;
                int pageIndex = Convert.ToInt32(submitData.PageIndex);
                int pageSize = Convert.ToInt32(submitData.PageSize);
                int state = Convert.ToInt32(submitData.State);
                TB_APPManagement appInfo = new TB_APPManagement();
                APPManagementBLL aPPManagementBLL = new APPManagementBLL();
                VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
                appInfo = aPPManagementBLL.GetAPPManagement(appID);
                if (appInfo != null)
                {
                    string where = "1=1";
                    int versionID = Convert.ToInt32(appInfo.VersionID);
                    where += " and UserID = " + Convert.ToInt32(submitData.UserID);
                    where += " and State = " + state;
                    if (s == "0")
                    {
                        where += " and VersionID = " + versionID;
                        where += " and VideoType='0' ";
                    }
                    where += " ORDER BY ID DESC ";
                    IList<TB_UserVideoDetails> userVideoList = new List<TB_UserVideoDetails>();
                    userVideoList = videoDetailsBLL.GetUserVideoList(where);
                    if (userVideoList != null && userVideoList.Count > 0)
                    {
                        IList<TB_UserVideoDetails> removelist = new List<TB_UserVideoDetails>();
                        for (int i = 0; i < userVideoList.Count; i++)
                        {
                            if (i < pageIndex || i >= (pageIndex + pageSize))
                            {
                                removelist.Add(userVideoList[i]);
                            }
                        }
                        if (removelist != null && removelist.Count > 0)
                        {
                            for (int i = 0; i < removelist.Count; i++)
                            {
                                userVideoList.Remove(removelist[i]);
                            }
                        }
                        IList<VideoInfo> videoInfoList = new List<VideoInfo>();
                        for (int i = 0; i < userVideoList.Count; i++)
                        {
                            DateTime time = new DateTime();
                            VideoInfo videoInfo = new VideoInfo();
                            string[] timeArr = new string[3];
                            TB_VideoDetails videoDetails = new TB_VideoDetails();
                            videoInfo.ID = userVideoList[i].ID;
                            videoInfo.VideoID = userVideoList[i].VideoID;
                            //videoInfo.VideoTitle= userVideoList[i].
                            videoInfo.VideoImageAddress = StringTOJson(userVideoList[i].VideoImageAddress);
                            videoInfo.VideoReleaseAddress = StringTOJson(userVideoList[i].VideoFileID);
                            videoInfo.TotalScore = Convert.ToInt32(userVideoList[i].TotalScore);
                            time = Convert.ToDateTime(userVideoList[i].CreateTime);
                            videoInfo.Month = time.Month.ToString();
                            timeArr = time.ToString("yyyy-MM-dd").Split('-');
                            videoInfo.Day = timeArr[2];
                            videoInfo.State = submitData.State;
                            videoInfo.VideoType = userVideoList[i].VideoType;
                            videoDetails = videoDetailsBLL.GetVideoInfoByID(userVideoList[i].BookID, userVideoList[i].VideoNumber);
                            if (videoDetails != null)
                            {
                                videoInfo.VideoTitle = StringTOJson(videoDetails.VideoTitle);
                            }
                            else
                            {
                                videoInfo.VideoTitle = "";
                            }
                            videoInfoList.Add(videoInfo);
                        }
                        return ObjectToJson.GetResult(videoInfoList);
                    }
                }
                else
                {
                    return ObjectToJson.GetResult("", "没有更多数据");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有更多数据");

        }

        /// <summary>
        /// 获取未发布/发布视频信息列表 △
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetVideoList([FromBody]KingRequest request)
        {
            VideoRequset submitData = JsonHelper.DecodeJson<VideoRequset>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.AppID) || string.IsNullOrEmpty(submitData.UserID) || string.IsNullOrEmpty(submitData.State) || string.IsNullOrEmpty(submitData.PageIndex) || string.IsNullOrEmpty(submitData.PageSize))
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            //try
            //{
            string appId = submitData.AppID;
            int pageIndex = Convert.ToInt32(submitData.PageIndex);
            int state = Convert.ToInt32(submitData.State);
            TB_APPManagement appInfo = new TB_APPManagement();
            string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
            appInfo = getAPPManagement(appId, path);// aPPManagementBLL.GetAPPManagement(appID);
            if (appInfo != null)
            {
                string where = "";
                where += " and UserID = " + Convert.ToInt32(submitData.UserID);
                where += " and State = " + state;
                where += " and VersionID = " + Convert.ToInt32(appInfo.VersionID);
                where += " and VideoType='0' ";
                where += " ORDER BY ID DESC ";

                string sql;
                sql = string.Format(@"SELECT  ID ,
                                                        BookID ,
                                                        VideoNumber ,
                                                        VideoID ,
                                                        VideoImageAddress ,
                                                        VideoFileID ,
                                                        TotalScore ,
                                                        CreateTime ,
                                                        State ,
                                                        VideoType ,
                                                        IsEnableOss ,
                                                        UserID ,
                                                        VersionID
                                                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                                                WHERE   BookID IS NOT NULL
                                                        AND VideoNumber <> 0
                                                        AND BookID <> 0 {0}", where);


                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                IList<TB_UserVideoDetails> tb = JsonHelper.DataSetToIList<TB_UserVideoDetails>(ds, 0); //bm.Search<TB_GetVideoList>(where);
                List<VideoInfo> list = new List<VideoInfo>();

                string strSql;
                strSql = string.Format(@"SELECT  BookID ,
                                                            VideoNumber ,
                                                            VideoTitle
                                                    FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails]
                                                    WHERE   BookID IS NOT NULL
                                                            AND VideoNumber <> 0
                                                            AND BookID <> 0");

                DataSet dsTable = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
                List<videoDetailsInfo> details = JsonHelper.DataSetToIList<videoDetailsInfo>(dsTable, 0);
                // List<videoDetailsInfo> videoDetails = new List<videoDetailsInfo>();

                if (tb != null && tb.Count > 0)
                {
                    foreach (var item in tb)
                    {
                        string img = "";
                        string fileId = "";
                        if (submitData.IsEnableOss == 0)
                        {
                            img = StringTOJson(item.VideoImageAddress);
                            fileId = StringTOJson(item.VideoFileID);
                        }
                        else
                        {
                            string times = Convert.ToDateTime(item.CreateTime).ToString("yyyy/MM/dd");
                            img = item.IsEnableOss != 0 ? _getOssFilesUrl + StringTOJson(item.VideoImageAddress) : _getFilesUrl + "?FileID=" + StringTOJson(item.VideoImageAddress);
                            fileId = item.IsEnableOss != 0 ? _getOssFilesUrl + StringTOJson(item.VideoFileID) : _getVideoFiles + times + "/" + StringTOJson(item.VideoFileID);
                        }
                        DateTime time = new DateTime();
                        VideoInfo videoInfo = new VideoInfo();
                        string[] timeArr = new string[3];
                        videoInfo.ID = item.ID;
                        videoInfo.VideoID = item.VideoID; ;
                        videoInfo.VideoImageAddress = img;
                        videoInfo.VideoReleaseAddress = fileId;
                        videoInfo.TotalScore = Convert.ToInt32(item.TotalScore);
                        time = Convert.ToDateTime(item.CreateTime);
                        videoInfo.Month = time.Month.ToString();
                        timeArr = time.ToString("yyyy-MM-dd").Split('-');
                        videoInfo.Day = timeArr[2];
                        videoInfo.State = submitData.State;
                        videoInfo.VideoType = item.VideoType;
                        List<videoDetailsInfo> videoDetails = details.Where(i => i.BookID == item.BookID & i.VideoNumber == item.VideoNumber).ToList();
                        if (videoDetails.Count == 0)
                        {
                            videoInfo.VideoTitle = "";
                        }
                        else
                        {
                            videoInfo.VideoTitle = StringTOJson(videoDetails[0].VideoTitle);
                        }
                        list.Add(videoInfo);

                    }
                    list = list.Skip(pageIndex).Take(10).ToList();
                    return ObjectToJson.GetResult(list);
                }
                else
                {
                    return ObjectToJson.GetResult("", "没有更多数据");
                }
            }
            else
            {
                return ObjectToJson.GetResult("", "没有更多数据");
            }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("error", ex);
            //}
            //return ObjectToJson.GetResult("", "没有更多数据");
        }

        /// <summary>
        /// 修改已发布/未发布视频信息状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateVedioInfo([FromBody]KingRequest request)
        {
            DeleteVedioInfo submitData = JsonHelper.DecodeJson<DeleteVedioInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.IDStr) || string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.State))
            {
                return ObjectToJson.GetErrorResult("状态不能为空！");
            }
            try
            {
                string state = submitData.State ?? "0";
                string idStr = submitData.IDStr;
                string userID = submitData.UserID;
                string[] idArr = idStr.Split(',');
                IList<string> idList = new List<string>();
                for (int i = 0; i < idArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idArr[i]))
                    {
                        idList.Add(idArr[i]);
                    }
                }
                VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
                bool consequence = true;
                for (int i = 0; i < idList.Count; i++)
                {
                    bool result = false;
                    TB_UserVideoDetails videoDetails = new TB_UserVideoDetails();
                    IList<TB_UserVideoDetails> userVideoList = new List<TB_UserVideoDetails>();
                    videoDetails = videoDetailsBLL.GetUserVideoInfoByID(idList[i]);
                    if (videoDetails != null)
                    {

                        videoDetails.State = ParseInt(state);
                        result = videoDetailsBLL.UpdateUserVideoInfo(videoDetails);
                        if (!result)
                        {
                            consequence = false;
                        }
                    }
                    else
                    {
                        consequence = false;
                    }
                }
                if (consequence)
                {
                    if (submitData.State == "0")
                    {
                        return ObjectToJson.GetResult("", "视频信息删除成功");
                    }
                    if (submitData.State == "1")
                    {
                        return ObjectToJson.GetResult("", "视频信息发布成功");
                    }
                    return ObjectToJson.GetResult("", "视频信息未发布");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("视频信息删除失败");
        }

        /// <summary>
        /// 修改已发布/未发布视频信息状态(未修改)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateVedioInfoTest()
        {
            string json = "{\"IDStr\":\"113\",\"UserID\":\"78532691\",\"State\":\"1\"}";
            DeleteVedioInfo submitData = JsonHelper.DecodeJson<DeleteVedioInfo>(json);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.IDStr) || string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.State))
            {
                return ObjectToJson.GetErrorResult("状态不能为空！");
            }
            try
            {
                string state = submitData.State ?? "0";
                string idStr = submitData.IDStr;
                string userID = submitData.UserID;
                string[] idArr = idStr.Split(',');
                VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
                bool consequence = true;
                for (int i = 0; i < idArr.Length; i++)
                {
                    bool result = false;
                    TB_UserVideoDetails videoDetails = new TB_UserVideoDetails();
                    IList<TB_UserVideoDetails> userVideoList = new List<TB_UserVideoDetails>();
                    videoDetails = videoDetailsBLL.GetUserVideoInfoByID(idArr[i]);
                    //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, string.Format("select*from TB_UserVideoDetails where ID={0}", idArr[i])).Tables[0];
                    if (videoDetails != null)
                    {

                        videoDetails.State = ParseInt(state);
                        result = videoDetailsBLL.UpdateUserVideoInfo(videoDetails);
                        if (!result)
                        {
                            consequence = false;
                        }
                    }
                    else
                    {
                        consequence = false;
                    }
                }
                if (consequence)
                {
                    if (submitData.State == "0")
                    {
                        return ObjectToJson.GetResult("", "视频信息删除成功");
                    }
                    if (submitData.State == "1")
                    {
                        return ObjectToJson.GetResult("", "视频信息发布成功");
                    }
                    return ObjectToJson.GetResult("", "视频信息未发布");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("视频信息删除失败");
        }

        VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
        /// <summary>
        /// 插入用户配音视频信息(未改前)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertVideoInfoAgo([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            UserVideoInfo submitData = js.Deserialize<UserVideoInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("用户ID为空");
            }
            if (submitData.VideoType != "1")
            {
                if (string.IsNullOrEmpty(submitData.BookId))
                {
                    return ObjectToJson.GetErrorResult("BookID为空！");
                }
            }

            if (string.IsNullOrEmpty(submitData.State))
            {
                submitData.State = "2";
            }
            if (string.IsNullOrEmpty(submitData.VersionType))
            {
                submitData.VersionType = "1";
            }
            if (string.IsNullOrEmpty(submitData.Type))
            {
                submitData.Type = "0";
            }
            if (string.IsNullOrEmpty(submitData.VideoType))
            {
                submitData.VideoType = "0";
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber) && !string.IsNullOrEmpty(submitData.VideoID))
            {
                string strSql = @"SELECT BookID,VideoNumber FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE id='" + submitData.VideoID + "'";
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    submitData.BookId = ds.Tables[0].Rows[0]["BookID"].ToString();
                    submitData.VideoNumber = ds.Tables[0].Rows[0]["VideoNumber"].ToString();
                }
            }

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("UserID");
                // dt.Columns.Add("VideoID");
                dt.Columns.Add("UserVideoID");
                dt.Columns.Add("DialogueNumber");
                dt.Columns.Add("DialogueScore");
                dt.Columns.Add("CreateTime");
                dt.Columns.Add("VideoFileID");
                dt.Columns.Add("VideoType");
                string uvId = "";
                string sql = string.Empty;

                sql = @"SELECT [VersionID]  FROM [TB_APPManagement] WHERE ID='" + submitData.AppID + "'";
                int versionId = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));


                sql = string.Format(@"INSERT INTO [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
                   ([VersionID],[UserID],[BookID],[VideoNumber],[VideoFileID],[VideoReleaseAddress],[VideoImageAddress],[TotalScore],[State],[VersionType],[VideoType],[IsEnableOss])
                    VALUES
                   ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');SELECT @@IDENTITY AS ID;", versionId,
                    submitData.UserId, submitData.BookId, submitData.VideoNumber, submitData.VideoFileId,
                    submitData.VideoReleaseAddress, submitData.VideoImageAddress, submitData.TotalScore,
                    submitData.State, submitData.VersionType, submitData.VideoType, submitData.IsEnableOss);

                try
                {
                    uvId = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql).ToString();
                }
                catch (Exception ex)
                {
                    log.Error("error", ex);
                    return ObjectToJson.GetErrorResult("视频信息插入异常：" + ex.Message);
                }


                //将图片保存为永久文件
                if (!string.IsNullOrEmpty(submitData.VideoImageAddress))
                {
                    string field = FiedURL + "ConfirmHandler.ashx";
                    //get请求
                    string dd = JsonHelper.EncodeJson(submitData.VideoImageAddress);
                    string values = "t=[" + dd + "]";
                    HttpGet(field, values);
                }

                //将视频文件保存为永久文件
                if (!string.IsNullOrEmpty(submitData.VideoFileId))
                {
                    string field = FiedURL + "ConfirmHandler.ashx";
                    //get请求
                    string dd = JsonHelper.EncodeJson(submitData.VideoFileId);
                    string values = "t=[" + dd + "]";
                    HttpGet(field, values);
                }

                if (ParseInt(uvId) > 0)
                {
                    if (submitData.children.Length > 0)
                    {
                        foreach (Children t1 in submitData.children)
                        {
                            DataRow dr = dt.NewRow();
                            dr["UserID"] = submitData.UserId;
                            dr["UserVideoID"] = ParseInt(uvId);
                            dr["DialogueNumber"] = ParseInt(t1.DialogueNumber);
                            dr["DialogueScore"] = t1.DialogueScore;
                            dr["VideoType"] = submitData.Type;
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("视频信息插入失败");
                }

                SqlBulkCopy sbc = new SqlBulkCopy(SqlHelper.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction)
                {
                    BulkCopyTimeout = 5000,
                    NotifyAfter = dt.Rows.Count
                };

                string strSql = "DELETE FROM [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue] WHERE [UserVideoID]=" + ParseInt(uvId);
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql);

                try
                {
                    sbc.DestinationTableName = "TB_UserVideoDialogue";
                    sbc.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    log.Error("error", ex);
                    return ObjectToJson.GetErrorResult("参数传递错误:" + ex.Message);
                }
                string sj = "{\"ID\":" + uvId + "}";
                return sbc.NotifyAfter > 0 ? ObjectToJson.GetResult(js.DeserializeObject(sj), "视频信息发布成功") : ObjectToJson.GetErrorResult("视频对白信息发布失败");
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("错误");
        }

        /// <summary>
        /// 插入用户配音视频信息 △
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertVideoInfo([FromBody]KingRequest request)
        {
            //lock (locker)
            //{
            JavaScriptSerializer js = new JavaScriptSerializer();
            UserVideoInfo submitData = js.Deserialize<UserVideoInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("用户ID为空");
            }
            if (submitData.VideoType != "1")
            {
                if (string.IsNullOrEmpty(submitData.BookId))
                {
                    return ObjectToJson.GetErrorResult("BookID为空！");
                }
            }

            if (string.IsNullOrEmpty(submitData.State))
            {
                submitData.State = "2";
            }
            if (string.IsNullOrEmpty(submitData.VersionType))
            {
                submitData.VersionType = "1";
            }
            if (string.IsNullOrEmpty(submitData.Type))
            {
                submitData.Type = "0";
            }
            if (string.IsNullOrEmpty(submitData.VideoType))
            {
                submitData.VideoType = "0";
            }
            try
            {
                string schoolid = "";
                string classid = "";
                string userType = "";
                string classnum = "";
                string tname = "暂未填写";
                string img = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserId));
                if (user != null)
                {
                    img = user.IsEnableOss != 0 ? _getOssFilesUrl +
                              (string.IsNullOrEmpty(user.UserImage)
                                  ? "00000000-0000-0000-0000-000000000000"
                                  : user.UserImage) :
                                    _getFilesUrl + "?FileID=" +
                              (string.IsNullOrEmpty(user.UserImage)
                                  ? "00000000-0000-0000-0000-000000000000"
                                  : user.UserImage);
                    tname = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                    userType = ((int)user.UserType).ToString();
                    if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                    {
                        var classinfo = user.ClassSchList.FirstOrDefault();
                        if (classinfo != null)
                        {
                            var clas = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                            if (clas != null)
                            {
                                schoolid = clas.SchID.ToString();
                                classid = clas.ClassID.ToString();
                                classnum = clas.ClassNum;
                            }

                        }

                    }
                    int versionId = 0;
                    // = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
                    TB_APPManagement appInfo = getAPPManagement(submitData.AppID, path);
                    if (appInfo != null)
                    {
                        versionId = Convert.ToInt32(appInfo.VersionID);
                    }
                    if (versionId > 0)
                    {
                        int UserVideoID = addVideoInfo(submitData, versionId);
                        if (UserVideoID > 0)
                        {
                            string FirstTitleID = "";
                            string FirstTitle = "";
                            string SecondTitleID = "";
                            string SecondTitle = "";
                            string FirstModularID = "";
                            string FirstModular = "";
                            string VideoTitle = "";
                            string ModuleType = "";
                            string sql;
                            ModuleType = "1";
                            sql = string.Format(@"SELECT  [FirstTitleID] ,
                                                                [FirstTitle] ,
                                                                [SecondTitleID] ,
                                                                [SecondTitle] ,
                                                                [FirstModularID] ,
                                                                [FirstModular],
                                                                VideoTitle
                                                        FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails]
                                                        WHERE   BookID = '{0}'
                                                            AND VideoNumber = '{1}'", submitData.BookId,
                                submitData.VideoNumber);
                            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text,
                                sql);
                            List<VideoDetail> vi = JsonHelper.DataSetToIList<VideoDetail>(ds, 0);
                            if (vi.Count > 0)
                            {
                                VideoDetail vinfo = vi.FirstOrDefault();
                                FirstTitleID = vinfo.FirstTitleID.ToString();
                                FirstTitle = vinfo.FirstTitle;
                                SecondTitleID = vinfo.SecondTitleID.ToString();
                                SecondTitle = vinfo.SecondTitle;
                                FirstModularID = vinfo.FirstModularID.ToString();
                                FirstModular = vinfo.FirstModular;
                                VideoTitle = vinfo.VideoTitle;
                            }

                            List<string> lst = new List<string>();
                            RedisVideoInfo rvi = new RedisVideoInfo
                            {
                                VideoID = UserVideoID.ToString(),
                                BookId = submitData.BookId,
                                VideoNumber = submitData.VideoNumber,
                                SchoolID = schoolid,
                                ClassID = classid,
                                UserId = submitData.UserId,
                                TotalScore = submitData.TotalScore,
                                CreateTime = DateTime.Now.ToString(),
                                NumberOfOraise = lst,
                                UserType = userType,
                                TrueName = tname,
                                UserImage = img,
                                ModuleType = ModuleType
                            };
                            RedisVideoInfo videoinfo = new RedisVideoInfo
                            {
                                VideoID = UserVideoID.ToString(),
                                BookId = submitData.BookId,
                                VideoNumber = submitData.VideoNumber,
                                SchoolID = schoolid,
                                ClassID = classnum,
                                UserId = submitData.UserId,
                                TotalScore = submitData.TotalScore,
                                CreateTime = DateTime.Now.ToString(),
                                NumberOfOraise = lst,
                                UserType = userType,
                                TrueName = tname,
                                UserImage = img,
                                ModuleType = ModuleType,
                                VideoImageAddress = submitData.VideoImageAddress,
                                FirstTitleID = FirstTitleID,
                                FirstTitle = FirstTitle,
                                SecondTitleID = SecondTitleID,
                                SecondTitle = SecondTitle,
                                FirstModularID = FirstModularID,
                                FirstModular = FirstModular,
                                IsEnableOss = submitData.IsEnableOss,
                                VideoTitle = VideoTitle
                            };
                            //队列
                            redisList.LPush("RankQueue", JsonHelper.DeepEncodeJson(rvi));
                            redisList.LPush("LearningReportQueue", JsonHelper.DeepEncodeJson(videoinfo));

                            string sj = "{\"ID\":" + UserVideoID + "}";
                            return ObjectToJson.GetResult(js.DeserializeObject(sj), "视频信息发布成功");
                        }
                        else
                        {
                            return ObjectToJson.GetErrorResult("视频信息插入失败");
                        }

                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("该版本不存在");
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("找不到该用户！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InsertVideoInfo：" + ex.Message + "|UserID:" + submitData.UserId);
            }
            return ObjectToJson.GetErrorResult("错误");
            //}
        }


        /// <summary>
        /// 插入用户配音视频信息(事物)
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public int addYXVideoInfo(UserVideoInfo submitData, int versionId)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails_YX]
           ([VersionID],[UserID],[BookID],[VideoNumber],[VideoFileID],[VideoReleaseAddress],[VideoImageAddress],[TotalScore],[State],[VersionType],[VideoType],[IsEnableOss])
            VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');SELECT @@IDENTITY AS ID;", versionId,
                     submitData.UserId, submitData.BookId, submitData.VideoNumber, submitData.VideoFileId,
                     submitData.VideoReleaseAddress, submitData.VideoImageAddress, submitData.TotalScore,
                     submitData.State, submitData.VersionType, submitData.VideoType, submitData.IsEnableOss);
                string UserVideoID = ""; bool result = true;
                using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction ot = conn.BeginTransaction())
                    {
                        using (SqlCommand comm = new SqlCommand(sql, conn, ot))
                        {
                            object obj = comm.ExecuteScalar();
                            UserVideoID = obj == null ? "" : obj.ToString();
                            if (ParseInt(UserVideoID) > 0)
                            {
                                if (submitData.children.Length > 0)
                                {
                                    foreach (Children t1 in submitData.children)
                                    {
                                        sql = string.Format(@"insert into [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue_YX](UserID,UserVideoID,DialogueNumber,DialogueScore,VideoType,VideoFileID)
                                        values('{0}','{1}','{2}','{3}','{4}','{5}')", submitData.UserId, UserVideoID, t1.DialogueNumber, float.Parse(t1.DialogueScore.ToString("f1")), submitData.Type, submitData.VideoFileId);
                                        using (SqlCommand cmd = new SqlCommand(sql, conn, ot))
                                        {
                                            if (cmd.ExecuteNonQuery() <= 0)
                                            {
                                                result = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ot.Rollback();
                                return 0;
                            }
                        }
                        if (result)
                        {
                            ot.Commit();
                            return Convert.ToInt32(UserVideoID);
                        }
                        else
                        {
                            ot.Rollback();
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return 0;
        }

        /// <summary>
        /// 插入用户配音视频信息(事物)
        /// </summary>
        /// <param name="submitData"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public int addVideoInfo(UserVideoInfo submitData, int versionId)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails]
           ([VersionID],[UserID],[BookID],[VideoNumber],[VideoFileID],[VideoReleaseAddress],[VideoImageAddress],[TotalScore],[State],[VersionType],[VideoType],[IsEnableOss])
            VALUES
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');SELECT @@IDENTITY AS ID;", versionId,
                     submitData.UserId, submitData.BookId, submitData.VideoNumber, submitData.VideoFileId,
                     submitData.VideoReleaseAddress, submitData.VideoImageAddress, submitData.TotalScore,
                     submitData.State, submitData.VersionType, submitData.VideoType, submitData.IsEnableOss);
                string UserVideoID = ""; bool result = true;
                using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction ot = conn.BeginTransaction())
                    {
                        using (SqlCommand comm = new SqlCommand(sql, conn, ot))
                        {
                            object obj = comm.ExecuteScalar();
                            UserVideoID = obj == null ? "" : obj.ToString();
                            if (ParseInt(UserVideoID) > 0)
                            {
                                if (submitData.children.Length > 0)
                                {
                                    foreach (Children t1 in submitData.children)
                                    {
                                        sql = string.Format(@"insert into [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue](UserID,UserVideoID,DialogueNumber,DialogueScore,VideoType,VideoFileID)
                                        values('{0}','{1}','{2}','{3}','{4}','{5}')", submitData.UserId, UserVideoID, t1.DialogueNumber, float.Parse(t1.DialogueScore.ToString("f1")), submitData.Type, submitData.VideoFileId);
                                        using (SqlCommand cmd = new SqlCommand(sql, conn, ot))
                                        {
                                            if (cmd.ExecuteNonQuery() <= 0)
                                            {
                                                result = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ot.Rollback();
                                return 0;
                            }
                        }
                        if (result)
                        {
                            ot.Commit();
                            return Convert.ToInt32(UserVideoID);
                        }
                        else
                        {
                            ot.Rollback();
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return 0;
        }
        /// <summary>
        /// 插入用户配音视频信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage InsertVideoInfoTest()
        {
            string strJson = "{\"AppID\":\"241ea176-fce7-4bd7-a65f-a7978aac1cd2\",\"BookID\":\"168\",\"DialogueScore\":0.0,\"IsBool\":0,\"IsEnableOss\":1,\"NumberOfOraise\":0,\"State\":1,\"TotalScore\":17.584665,\"UserId\":\"1229552454\",\"VersionType\":2,\"VideoFileId\":\"2017/12/19/500f5501-5427-434d-a0e2-69c2b0e3f558.mp4\",\"VideoID\":\"3909\",\"VideoImageAddress\":\"2017/12/19/fe3aa174-1cfe-4436-a51a-0d6e7e252ac7.jpg\",\"VideoNumber\":\"3\",\"VideoType\":0,\"children\":[{\"DialogueNumber\":1,\"DialogueScore\":0.0},{\"DialogueNumber\":2,\"DialogueScore\":34.056},{\"DialogueNumber\":3,\"DialogueScore\":18.698}],\"isChoose\":false,\"isVisable\":false,\"IsYX\":2}";
            JavaScriptSerializer js = new JavaScriptSerializer();
            Log4Net.LogHelper.Info("InsertVideoInfo接口开始");
            UserVideoInfo submitData = js.Deserialize<UserVideoInfo>(strJson);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("用户ID为空");
            }
            if (submitData.VideoType != "1")
            {
                if (string.IsNullOrEmpty(submitData.BookId))
                {
                    return ObjectToJson.GetErrorResult("BookID为空！");
                }
            }

            if (string.IsNullOrEmpty(submitData.State))
            {
                submitData.State = "2";
            }
            if (string.IsNullOrEmpty(submitData.VersionType))
            {
                submitData.VersionType = "1";
            }
            if (string.IsNullOrEmpty(submitData.Type))
            {
                submitData.Type = "0";
            }
            if (string.IsNullOrEmpty(submitData.VideoType))
            {
                submitData.VideoType = "0";
            }
            try
            {
                string schoolid = "";
                string classid = "";
                string userType = "";
                string classnum = "";
                string tname = "暂未填写";
                string img = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserId));
                if (user != null)
                {
                    img = user.IsEnableOss != 0 ? _getOssFilesUrl +
                              (string.IsNullOrEmpty(user.UserImage)
                                  ? "00000000-0000-0000-0000-000000000000"
                                  : user.UserImage) :
                                    _getFilesUrl + "?FileID=" +
                              (string.IsNullOrEmpty(user.UserImage)
                                  ? "00000000-0000-0000-0000-000000000000"
                                  : user.UserImage);
                    tname = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                    userType = ((int)user.UserType).ToString();
                    if (user.ClassSchList != null && user.ClassSchList.Count > 0)
                    {
                        var classinfo = user.ClassSchList.FirstOrDefault();
                        if (classinfo != null)
                        {
                            var clas = classBLL.GetClassUserRelationByClassId(classinfo.ClassID);
                            if (clas != null)
                            {
                                schoolid = clas.SchID.ToString();
                                classid = clas.ClassID.ToString();
                                classnum = clas.ClassNum;
                            }

                        }

                    }
                    int versionId = 0;
                    // = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
                    TB_APPManagement appInfo = getAPPManagement(submitData.AppID, path);
                    if (appInfo != null)
                    {
                        versionId = Convert.ToInt32(appInfo.VersionID);
                    }
                    if (versionId > 0)
                    {
                        int UserVideoID = addVideoInfo(submitData, versionId);
                        if (UserVideoID > 0)
                        {
                            string FirstTitleID = "";
                            string FirstTitle = "";
                            string SecondTitleID = "";
                            string SecondTitle = "";
                            string FirstModularID = "";
                            string FirstModular = "";
                            string VideoTitle = "";
                            string ModuleType = "";
                            string sql;
                            if (submitData.IsYX == null || submitData.IsYX == 0)
                            {
                                ModuleType = "1";
                                sql = string.Format(@"SELECT  [FirstTitleID] ,
                                                                [FirstTitle] ,
                                                                [SecondTitleID] ,
                                                                [SecondTitle] ,
                                                                [FirstModularID] ,
                                                                [FirstModular],
                                                                VideoTitle
                                                        FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails]
                                                        WHERE   BookID = '{0}'
                                                            AND VideoNumber = '{1}'", submitData.BookId, submitData.VideoNumber);
                            }
                            else
                            {
                                ModuleType = "4";
                                sql = string.Format(@"SELECT  [FirstTitleID] ,
                                                                [FirstTitle] ,
                                                                [SecondTitleID] ,
                                                                [SecondTitle] ,
                                                                [FirstModularID] ,
                                                                [FirstModular],
                                                                VideoTitle
                                                        FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails_YX]
                                                        WHERE   BookID = '{0}'
                                                            AND VideoNumber = '{1}'", submitData.BookId, submitData.VideoNumber);
                            }

                            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                            List<VideoDetail> vi = JsonHelper.DataSetToIList<VideoDetail>(ds, 0);
                            if (vi.Count > 0)
                            {
                                VideoDetail vinfo = vi.FirstOrDefault();
                                FirstTitleID = vinfo.FirstTitleID.ToString();
                                FirstTitle = vinfo.FirstTitle;
                                SecondTitleID = vinfo.SecondTitleID.ToString();
                                SecondTitle = vinfo.SecondTitle;
                                FirstModularID = vinfo.FirstModularID.ToString();
                                FirstModular = vinfo.FirstModular;
                                VideoTitle = vinfo.VideoTitle;
                            }

                            List<string> lst = new List<string>();
                            RedisVideoInfo rvi = new RedisVideoInfo
                            {
                                VideoID = UserVideoID.ToString(),
                                BookId = submitData.BookId,
                                VideoNumber = submitData.VideoNumber,
                                SchoolID = schoolid,
                                ClassID = classid,
                                UserId = submitData.UserId,
                                TotalScore = submitData.TotalScore,
                                CreateTime = DateTime.Now.ToString(),
                                NumberOfOraise = lst,
                                UserType = userType,
                                TrueName = tname,
                                UserImage = img,
                                ModuleType = ModuleType
                            };
                            RedisVideoInfo videoinfo = new RedisVideoInfo
                            {
                                VideoID = UserVideoID.ToString(),
                                BookId = submitData.BookId,
                                VideoNumber = submitData.VideoNumber,
                                SchoolID = schoolid,
                                ClassID = classnum,
                                UserId = submitData.UserId,
                                TotalScore = submitData.TotalScore,
                                CreateTime = DateTime.Now.ToString(),
                                NumberOfOraise = lst,
                                UserType = userType,
                                TrueName = tname,
                                UserImage = img,
                                ModuleType = ModuleType,
                                VideoImageAddress = submitData.VideoImageAddress,
                                FirstTitleID = FirstTitleID,
                                FirstTitle = FirstTitle,
                                SecondTitleID = SecondTitleID,
                                SecondTitle = SecondTitle,
                                FirstModularID = FirstModularID,
                                FirstModular = FirstModular,
                                IsEnableOss = submitData.IsEnableOss,
                                VideoTitle = VideoTitle
                            };
                            //队列
                            redisList.LPush("RankQueue", JsonHelper.DeepEncodeJson(rvi));
                            redisList.LPush("LearningReportQueue", JsonHelper.DeepEncodeJson(videoinfo));

                            string sj = "{\"ID\":" + UserVideoID + "}";
                            return ObjectToJson.GetResult(js.DeserializeObject(sj), "视频信息发布成功");
                        }
                        else
                        {
                            return ObjectToJson.GetErrorResult("视频信息插入失败");
                        }
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("该版本不存在");
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("找不到该用户！");
                }

            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "InsertVideoInfo：" + ex.Message + "|UserID:" + submitData.UserId);
            }
            return ObjectToJson.GetErrorResult("错误");
        }

        /// <summary>
        /// get 请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        /// <summary>
        /// 发布配音视频
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PublishVedioInfor([FromBody]KingRequest request)
        {
            TB_UserVideoDetails submitData = JsonHelper.DecodeJson<TB_UserVideoDetails>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("参数不能为空");
            }
            try
            {
                string sql = @"UPDATE [FZ_InterestDubbing].dbo.TB_UserVideoDetails SET State=1 WHERE ID=" + submitData.ID;
                if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
                {
                    string s = "{\"state\":\"0\"}";
                    return ObjectToJson.GetResult(js.DeserializeObject(s), "发布成功");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("发布失败");
        }

        /// <summary>
        /// 校级榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SchoolRankInfo([FromBody]KingRequest request)
        {
            UserSchoolRankParaModel submitData = JsonHelper.DecodeJson<UserSchoolRankParaModel>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.SchoolID))
            {
                return ObjectToJson.GetErrorResult("学校ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber))
            {
                return ObjectToJson.GetErrorResult("视频序号不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            try
            {
                UserSchoolRankReponse response = new UserSchoolRankReponse();
                List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + submitData.BookID, submitData.SchoolID + "_" + submitData.VideoNumber);
                if (rin != null && rin.Count > 0)
                {
                    List<string> listKeys = new List<string>();
                    foreach (var item in rin)
                    {
                        listKeys.Add(item.VideoId);
                    }
                    List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetValues<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + submitData.BookID, listKeys);
                    if (listVideoInfos != null && listVideoInfos.Count > 0)
                    {
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                        Redis_IntDubb_VideoInfoSort sortModel = new Redis_IntDubb_VideoInfoSort();
                        foreach (var item in listVideoInfos)
                        {
                            if (item != null)
                            {
                                sortModel = new Redis_IntDubb_VideoInfoSort()
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
                        if (listVideoInfo != null && listVideoInfo.Count > 0)
                        {
                            listVideoInfo = listVideoInfo.OrderByDescending(i => i.TotalScore).ThenByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                            for (int i = 0; i < listVideoInfo.Count; i++)
                            {
                                listVideoInfo[i].Sort = i + 1;
                                if (listVideoInfo[i].UserId == submitData.UserID)
                                {
                                    #region 获取本地DB用户姓名和头像
                                    string UName = "暂未填写";
                                    string UImg = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";

                                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
                                    if (user != null)
                                    {
                                        UImg = user.IsEnableOss != 0 ? _getOssFilesUrl +
                                               (string.IsNullOrEmpty(user.UserImage)
                                                   ? "00000000-0000-0000-0000-000000000000"
                                                   : user.UserImage) :
                                                    _getFilesUrl + "?FileID=" +
                                                   (string.IsNullOrEmpty(user.UserImage)
                                                               ? "00000000-0000-0000-0000-000000000000"
                                                        : user.UserImage);
                                        UName = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                                    }
                                    #endregion
                                    listVideoInfo[i].TrueName = UName;
                                    listVideoInfo[i].UserImage = UImg;
                                    response.CurrentUserRank = listVideoInfo[i];
                                }
                            }

                            listVideoInfo = listVideoInfo.Skip((submitData.PageIndex - 1) * submitData.PageCount).Take(submitData.PageCount).ToList();
                            List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                            foreach (var item in listVideoInfo)
                            {
                                var temUser = userBLL.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
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
                                    item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl +
                                                (string.IsNullOrEmpty(us.UserImage)
                                                    ? "00000000-0000-0000-0000-000000000000"
                                                    : us.UserImage) :
                                                     _getFilesUrl + "?FileID=" +
                                                    (string.IsNullOrEmpty(us.UserImage)
                                                                ? "00000000-0000-0000-0000-000000000000"
                                                         : us.UserImage);
                                }
                                listVideoInfoResult.Add(item);
                            }
                            response.RankList = listVideoInfoResult;
                            return ObjectToJson.GetResult(response);

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有对应的数据！");
        }


        /// <summary>
        /// 校级榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SchoolRankInfoTest()
        {
            UserSchoolRankParaModel submitData = new UserSchoolRankParaModel();//JsonHelper.DecodeJson<UserSchoolRankParaModel>(request.Data);
            submitData.BookID = "168";
            submitData.VideoNumber = "3";
            submitData.PageCount = 20;
            submitData.PageIndex = 1;
            //JavaScriptSerializer js = new JavaScriptSerializer(submitData);
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.SchoolID))
            {
                return ObjectToJson.GetErrorResult("学校ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber))
            {
                return ObjectToJson.GetErrorResult("视频序号不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            try
            {
                UserSchoolRankReponse response = new UserSchoolRankReponse();
                List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_SchoolRank_" + submitData.BookID, submitData.SchoolID + "_" + submitData.VideoNumber);
                if (rin != null && rin.Count > 0)
                {
                    List<string> listKeys = new List<string>();
                    foreach (var item in rin)
                    {
                        listKeys.Add(item.VideoId);
                    }
                    List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetValues<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + submitData.BookID, listKeys);
                    if (listVideoInfos != null && listVideoInfos.Count > 0)
                    {
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                        Redis_IntDubb_VideoInfoSort sortModel = new Redis_IntDubb_VideoInfoSort();
                        foreach (var item in listVideoInfos)
                        {
                            if (item != null)
                            {
                                sortModel = new Redis_IntDubb_VideoInfoSort()
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
                        if (listVideoInfo != null && listVideoInfo.Count > 0)
                        {
                            listVideoInfo = listVideoInfo.OrderByDescending(i => i.TotalScore).ThenByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                            for (int i = 0; i < listVideoInfo.Count; i++)
                            {
                                listVideoInfo[i].Sort = i + 1;
                                if (listVideoInfo[i].UserId == submitData.UserID)
                                {
                                    #region 获取本地DB用户姓名和头像
                                    string UName = "暂未填写";
                                    string UImg = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";

                                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
                                    if (user != null)
                                    {
                                        UImg = user.IsEnableOss != 0 ? _getOssFilesUrl +
                                               (string.IsNullOrEmpty(user.UserImage)
                                                   ? "00000000-0000-0000-0000-000000000000"
                                                   : user.UserImage) :
                                                    _getFilesUrl + "?FileID=" +
                                                   (string.IsNullOrEmpty(user.UserImage)
                                                               ? "00000000-0000-0000-0000-000000000000"
                                                        : user.UserImage);
                                        UName = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                                    }
                                    #endregion
                                    listVideoInfo[i].TrueName = UName;
                                    listVideoInfo[i].UserImage = UImg;
                                    response.CurrentUserRank = listVideoInfo[i];
                                }
                            }

                            listVideoInfo = listVideoInfo.Skip((submitData.PageIndex - 1) * submitData.PageCount).Take(submitData.PageCount).ToList();
                            List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                            foreach (var item in listVideoInfo)
                            {
                                var temUser = userBLL.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
                                if (temUser != null)
                                {
                                    listUser.Add(temUser);
                                }
                            }
                            #region 获取本地DB用户姓名和头像

                            List<Redis_IntDubb_VideoInfoSort> listVideoInfoResult = new List<Redis_IntDubb_VideoInfoSort>();
                            foreach (var item in listVideoInfo)
                            {
                                var us = listUser.Where(a => a.UserID == Convert.ToInt32(item.UserId)).FirstOrDefault();
                                if (us != null)
                                {
                                    item.TrueName = string.IsNullOrEmpty(us.TrueName) ? "暂未填写" : us.TrueName;
                                    item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl +
                                                (string.IsNullOrEmpty(us.UserImage)
                                                    ? "00000000-0000-0000-0000-000000000000"
                                                    : us.UserImage) :
                                                     _getFilesUrl + "?FileID=" +
                                                    (string.IsNullOrEmpty(us.UserImage)
                                                                ? "00000000-0000-0000-0000-000000000000"
                                                         : us.UserImage);
                                }
                                listVideoInfoResult.Add(item);
                            }
                            response.RankList = listVideoInfoResult;
                            return ObjectToJson.GetResult(response);

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有对应的数据！");
        }

        /// <summary>
        /// 班级榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ClassRankInfo([FromBody]KingRequest request)
        {
            UserSchoolRankParaModel submitData = JsonHelper.DecodeJson<UserSchoolRankParaModel>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.ClassID))
            {
                return ObjectToJson.GetErrorResult("班级ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber))
            {
                return ObjectToJson.GetErrorResult("视频序号不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            try
            {
                string ClassID = "";
                if (submitData.ClassID.Length <= 8)
                {
                    IBS_ClassUserRelation classInfo = classBLL.GetClassUserRelationByClassOtherId(submitData.ClassID, 1);
                    if (classInfo != null)
                    {
                        ClassID = classInfo.ClassID;
                    }
                }
                else
                {
                    ClassID = submitData.ClassID;
                }
                UserSchoolRankReponse response = new UserSchoolRankReponse();
                List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + submitData.BookID, ClassID.ToLower() + "_" + submitData.VideoNumber);
                if (rin != null && rin.Count > 0)
                {
                    List<string> listKeys = new List<string>();
                    foreach (var item in rin)
                    {
                        listKeys.Add(item.VideoId);
                    }
                    List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetValues<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + submitData.BookID, listKeys);
                    if (listVideoInfos != null && listVideoInfos.Count > 0)
                    {
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                        Redis_IntDubb_VideoInfoSort sortModel = new Redis_IntDubb_VideoInfoSort();
                        foreach (var item in listVideoInfos)
                        {
                            if (item != null)
                            {
                                sortModel = new Redis_IntDubb_VideoInfoSort()
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
                        if (listVideoInfo != null && listVideoInfo.Count > 0)
                        {
                            listVideoInfo = listVideoInfo.OrderByDescending(i => i.TotalScore).ThenByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                            for (int i = 0; i < listVideoInfo.Count; i++)
                            {
                                listVideoInfo[i].Sort = i + 1;
                                if (listVideoInfo[i].UserId == submitData.UserID)
                                {
                                    #region 获取本地DB用户姓名和头像
                                    string UName = "暂未填写";
                                    string UImg = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";

                                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
                                    if (user != null)
                                    {
                                        UImg = user.IsEnableOss != 0 ? _getOssFilesUrl +
                                               (string.IsNullOrEmpty(user.UserImage)
                                                   ? "00000000-0000-0000-0000-000000000000"
                                                   : user.UserImage) :
                                                    _getFilesUrl + "?FileID=" +
                                                   (string.IsNullOrEmpty(user.UserImage)
                                                               ? "00000000-0000-0000-0000-000000000000"
                                                        : user.UserImage);
                                        UName = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                                    }
                                    #endregion
                                    listVideoInfo[i].TrueName = UName;
                                    listVideoInfo[i].UserImage = UImg;
                                    response.CurrentUserRank = listVideoInfo[i];
                                }
                            }
                            listVideoInfo = listVideoInfo.Skip((submitData.PageIndex - 1) * submitData.PageCount).Take(submitData.PageCount).ToList();
                            List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                            foreach (var item in listVideoInfo)
                            {
                                var temUser = userBLL.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
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
                                    item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl +
                                                (string.IsNullOrEmpty(us.UserImage)
                                                    ? "00000000-0000-0000-0000-000000000000"
                                                    : us.UserImage) :
                                                     _getFilesUrl + "?FileID=" +
                                                    (string.IsNullOrEmpty(us.UserImage)
                                                                ? "00000000-0000-0000-0000-000000000000"
                                                         : us.UserImage);
                                }
                                listVideoInfoResult.Add(item);
                            }
                            response.RankList = listVideoInfoResult;
                            return ObjectToJson.GetResult(response);
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有对应的数据！");
        }

        /// <summary>
        /// 班级榜
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ClassRankInfoTest(string ClassId, string BookId, string VideoNumber, string UserID, int PageCount, int PageIndex)
        {
            UserSchoolRankParaModel submitData = new UserSchoolRankParaModel();//JsonHelper.DecodeJson<UserSchoolRankParaModel>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            //{"ClassId":"16B5345C-F67C-4303-9A4C-A72EDC972C02","BookId":"168","VideoNumber":"3","UserID":"802681767","PageCount":20,"PageIndex":1}
            submitData.ClassID = ClassId;
            submitData.BookID = BookId;
            submitData.VideoNumber = VideoNumber;
            submitData.UserID = UserID;
            submitData.PageCount = PageCount;
            submitData.PageIndex = PageIndex;
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.ClassID))
            {
                return ObjectToJson.GetErrorResult("班级ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber))
            {
                return ObjectToJson.GetErrorResult("视频序号不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            try
            {
                string ClassID = "";
                if (submitData.ClassID.Length <= 8)
                {
                    IBS_ClassUserRelation classInfo = classBLL.GetClassUserRelationByClassOtherId(submitData.ClassID, 1);
                    if (classInfo != null)
                    {
                        ClassID = classInfo.ClassID;
                    }
                }
                else
                {
                    ClassID = submitData.ClassID;
                }
                UserSchoolRankReponse response = new UserSchoolRankReponse();
                List<Redis_IntDubb_Rank> rin = redis.Get<List<Redis_IntDubb_Rank>>("Redis_IntDubb_ClassRank_" + submitData.BookID, ClassID.ToLower() + "_" + submitData.VideoNumber);
                if (rin != null && rin.Count > 0)
                {
                    List<string> listKeys = new List<string>();
                    foreach (var item in rin)
                    {
                        listKeys.Add(item.VideoId);
                    }
                    List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetValues<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + submitData.BookID, listKeys);
                    if (listVideoInfos != null && listVideoInfos.Count > 0)
                    {
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                        Redis_IntDubb_VideoInfoSort sortModel = new Redis_IntDubb_VideoInfoSort();
                        foreach (var item in listVideoInfos)
                        {
                            if (item != null)
                            {
                                sortModel = new Redis_IntDubb_VideoInfoSort()
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
                        if (listVideoInfo != null && listVideoInfo.Count > 0)
                        {
                            listVideoInfo = listVideoInfo.OrderByDescending(i => i.TotalScore).ThenByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                            for (int i = 0; i < listVideoInfo.Count; i++)
                            {
                                listVideoInfo[i].Sort = i + 1;
                                if (listVideoInfo[i].UserId == submitData.UserID)
                                {
                                    #region 获取本地DB用户姓名和头像
                                    string UName = "暂未填写";
                                    string UImg = _getOssFilesUrl + "00000000-0000-0000-0000-000000000000";

                                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
                                    if (user != null)
                                    {
                                        UImg = user.IsEnableOss != 0 ? _getOssFilesUrl +
                                               (string.IsNullOrEmpty(user.UserImage)
                                                   ? "00000000-0000-0000-0000-000000000000"
                                                   : user.UserImage) :
                                                    _getFilesUrl + "?FileID=" +
                                                   (string.IsNullOrEmpty(user.UserImage)
                                                               ? "00000000-0000-0000-0000-000000000000"
                                                        : user.UserImage);
                                        UName = user.TrueName.IsNullOrEmpty() ? "暂未填写" : user.TrueName;
                                    }
                                    #endregion
                                    listVideoInfo[i].TrueName = UName;
                                    listVideoInfo[i].UserImage = UImg;
                                    response.CurrentUserRank = listVideoInfo[i];
                                }
                            }
                            listVideoInfo = listVideoInfo.Skip((submitData.PageIndex - 1) * submitData.PageCount).Take(submitData.PageCount).ToList();
                            List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                            foreach (var item in listVideoInfo)
                            {
                                var temUser = userBLL.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
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
                                    item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl +
                                                (string.IsNullOrEmpty(us.UserImage)
                                                    ? "00000000-0000-0000-0000-000000000000"
                                                    : us.UserImage) :
                                                     _getFilesUrl + "?FileID=" +
                                                    (string.IsNullOrEmpty(us.UserImage)
                                                                ? "00000000-0000-0000-0000-000000000000"
                                                         : us.UserImage);
                                }
                                listVideoInfoResult.Add(item);
                            }
                            response.RankList = listVideoInfoResult;
                            return ObjectToJson.GetResult(response);
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "没有对应的数据！");
        }

        /// <summary>
        /// 最新榜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage NewRankInfo([FromBody]KingRequest request)
        {
            UserSchoolRankParaModel submitData = JsonHelper.DecodeJson<UserSchoolRankParaModel>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber))
            {
                return ObjectToJson.GetErrorResult("视频序号不能为空！");
            }
            try
            {
                List<Redis_IntDubb_NewRank> rin = redis.Get<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + submitData.BookID, submitData.VideoNumber);
                if (rin != null && rin.Count > 0)
                {
                    List<string> listKeys = new List<string>();
                    foreach (var item in rin)
                    {
                        listKeys.Add(item.VideoId);
                    }
                    List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetValues<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + submitData.BookID, listKeys);
                    if (listVideoInfos != null && listVideoInfos.Count > 0)
                    {
                        //listVideoInfos = listVideoInfos.OrderByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                        Redis_IntDubb_VideoInfoSort sortModel = new Redis_IntDubb_VideoInfoSort();
                        for (int i = 0; i < listVideoInfos.Count; i++)
                        {
                            if (listVideoInfos[i] != null)
                            {
                                sortModel = new Redis_IntDubb_VideoInfoSort()
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
                        if (listVideoInfo != null && listVideoInfo.Count > 0)
                        {
                            listVideoInfo = listVideoInfo.OrderByDescending(i => DateTime.Parse(i.CreateTime)).Skip((submitData.PageIndex - 1) * submitData.PageCount).Take(submitData.PageCount).ToList();
                            List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                            foreach (var item in listVideoInfo)
                            {
                                var temUser = userBLL.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
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
                                    item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl +
                                                (string.IsNullOrEmpty(us.UserImage)
                                                    ? "00000000-0000-0000-0000-000000000000"
                                                    : us.UserImage) :
                                                     _getFilesUrl + "?FileID=" +
                                                    (string.IsNullOrEmpty(us.UserImage)
                                                                ? "00000000-0000-0000-0000-000000000000"
                                                         : us.UserImage);
                                }
                                listVideoInfoResult.Add(item);
                            }
                            return ObjectToJson.GetResult(listVideoInfoResult);

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("[]", "没有对应的数据！");
        }


        /// <summary>
        /// 最新榜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage NewRankInfoTest()
        {
            UserSchoolRankParaModel submitData = new UserSchoolRankParaModel(); //JsonHelper.DecodeJson<UserSchoolRankParaModel>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            submitData.BookID = "168";
            submitData.VideoNumber = "3";
            submitData.PageCount = 20;
            submitData.PageIndex = 1;
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.BookID))
            {
                return ObjectToJson.GetErrorResult("书籍ID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.VideoNumber))
            {
                return ObjectToJson.GetErrorResult("视频序号不能为空！");
            }
            try
            {
                List<Redis_IntDubb_NewRank> rin = redis.Get<List<Redis_IntDubb_NewRank>>("Redis_IntDubb_NewRank_" + submitData.BookID, submitData.VideoNumber);
                if (rin != null && rin.Count > 0)
                {
                    List<string> listKeys = new List<string>();
                    foreach (var item in rin)
                    {
                        listKeys.Add(item.VideoId);
                    }
                    List<Redis_IntDubb_VideoInfo> listVideoInfos = redis.GetValues<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + submitData.BookID, listKeys);
                    if (listVideoInfos != null && listVideoInfos.Count > 0)
                    {
                        //listVideoInfos = listVideoInfos.OrderByDescending(i => Convert.ToDateTime(i.CreateTime)).ToList();
                        List<Redis_IntDubb_VideoInfoSort> listVideoInfo = new List<Redis_IntDubb_VideoInfoSort>();
                        Redis_IntDubb_VideoInfoSort sortModel = new Redis_IntDubb_VideoInfoSort();
                        for (int i = 0; i < listVideoInfos.Count; i++)
                        {
                            if (listVideoInfos[i] != null)
                            {
                                sortModel = new Redis_IntDubb_VideoInfoSort()
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
                        if (listVideoInfo != null && listVideoInfo.Count > 0)
                        {
                            listVideoInfo = listVideoInfo.Skip((submitData.PageIndex - 1) * submitData.PageCount).Take(submitData.PageCount).ToList();
                            List<IBS_UserInfo> listUser = new List<IBS_UserInfo>();
                            foreach (var item in listVideoInfo)
                            {
                                var temUser = userBLL.GetUserInfoByUserId(Convert.ToInt32(item.UserId));
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
                                    item.UserImage = us.IsEnableOss != 0 ? _getOssFilesUrl +
                                                (string.IsNullOrEmpty(us.UserImage)
                                                    ? "00000000-0000-0000-0000-000000000000"
                                                    : us.UserImage) :
                                                     _getFilesUrl + "?FileID=" +
                                                    (string.IsNullOrEmpty(us.UserImage)
                                                                ? "00000000-0000-0000-0000-000000000000"
                                                         : us.UserImage);
                                }
                                listVideoInfoResult.Add(item);
                            }
                            return ObjectToJson.GetResult(listVideoInfoResult.OrderByDescending(i => i.VideoId ?? "").ToList());

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("[]", "没有对应的数据！");
        }

        /// <summary>
        /// 发布配音视频
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage PublishVedioInforTest()
        {
            TB_UserVideoDetails submitData = new TB_UserVideoDetails();//JsonHelper.DecodeJson<TB_UserVideoDetails>(request.data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            submitData.ID = 16;
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("参数不能为空");
            }
            string sql = @"UPDATE [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] SET State=1 WHERE ID=" + submitData.ID;
            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                string s = "{\"state\":\"0\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(s), "发布成功");
            }
            else
            {
                return ObjectToJson.GetErrorResult("发布失败");
            }
        }

        /// <summary>
        /// 获取已发布视频排行信息(未改前)
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage VideoRankingInfoAgo([FromBody]KingRequest request)
        {
            UserVideoDetails submitData = JsonHelper.DecodeJson<UserVideoDetails>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.State))
            {
                return ObjectToJson.GetErrorResult("状态不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.Type.ToString()))
            {
                submitData.Type = 0;
            }

            string where = "";
            try
            {
                if (submitData.Type == 1)
                {
                    where = "a.State = '" + submitData.State + "' and VideoID='" + submitData.VideoID + "'";
                }
                else
                {
                    if (string.IsNullOrEmpty(submitData.AppID))
                    {
                        return ObjectToJson.GetErrorResult("版本ID不能为空！");
                    }
                    if (string.IsNullOrEmpty(submitData.BookID))
                    {
                        return ObjectToJson.GetErrorResult("书籍ID不能为空！");
                    }
                    if (string.IsNullOrEmpty(submitData.VideoNumber))
                    {
                        return ObjectToJson.GetErrorResult("视频序号不能为空！");
                    }

                    string sql = string.Empty;
                    sql = @"SELECT [VersionID]  FROM [TB_APPManagement] WHERE ID='" + submitData.AppID + "'";
                    int versionId = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                    //if (!string.IsNullOrEmpty(submitData.VersionType) || submitData.VersionType == "2")
                    //{
                    where = "a.BookID='" + submitData.BookID + "' AND a.VideoNumber='" + submitData.VideoNumber + "' AND a.VersionID = '" + versionId + "' AND a.State = '" + submitData.State + "'";
                    //}
                    //else
                    //{
                    //
                    //  where = "a.BookID='" + submitData.BookID + "' AND a.VideoNumber='" + submitData.VideoNumber + "' AND a.VersionID = '" + versionId + "' AND a.State = '" + submitData.State + "' AND a.column1 in ('1','2')";
                    //}
                }

                SqlParameter[] ps =
                {
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageCount", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar)
                };
                ps[0].Value = submitData.PageIndex;
                ps[1].Value = submitData.PageCount;
                ps[2].Value = where;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_VideoRankingInfo", ps);

                StringBuilder json = new StringBuilder();
                int num = 0;
                json.Append("[");
                if (ds.Tables[0].Rows.Count > 0 && null != ds.Tables[0])
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string img = "";
                        string fileid = "";
                        if (submitData.IsEnableOss == 0)
                        {
                            img = ds.Tables[0].Rows[i]["UserImage"].ToString();
                            fileid = ds.Tables[0].Rows[i]["VideoFileId"].ToString();
                        }
                        else
                        {
                            string time = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreateTime"]).ToString("yyyy/MM/dd");
                            img = ds.Tables[0].Rows[i]["UIsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["UserImage"].ToString() : _getFilesUrl + "?FileID=" + ds.Tables[0].Rows[i]["UserImage"].ToString();
                            fileid = ds.Tables[0].Rows[i]["IsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["VideoFileId"].ToString() : _getVideoFiles + time + "/" + ds.Tables[0].Rows[i]["VideoFileId"].ToString();
                        }

                        json.Append("{\"ID\": \"" + ds.Tables[0].Rows[i]["ID"] + "\",\"UserID\": \"" + ds.Tables[0].Rows[i]["UserID"] + "\",\"CreateTime\": \"" + ds.Tables[0].Rows[i]["CreateTime"] + "\",\"NumberOfOraise\": \"" + ds.Tables[0].Rows[i]["NumberOfOraise"] + "\",");
                        json.Append("\"TotalScore\": \"" + ds.Tables[0].Rows[i]["TotalScore"] + "\",\"VideoTitle\": \"" + String2Json(ds.Tables[0].Rows[i]["VideoTitle"].ToString()) + "\",\"UserName\": \"" + ds.Tables[0].Rows[i]["UserName"] + "\",");
                        json.Append("\"UserImage\": \"" + img + "\",\"NickName\": \"" + ds.Tables[0].Rows[i]["NickName"] + "\",\"VideoFileId\": \"" + fileid + "\"");
                        json.Append("},");

                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            string ss = "[]";
            return ObjectToJson.GetResult(js.DeserializeObject(ss), "没有对应的数据！");
        }
        /// <summary>
        /// 获取已发布视频排行信息
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage VideoRankingInfo([FromBody]KingRequest request)
        {
            UserVideoDetails submitData = JsonHelper.DecodeJson<UserVideoDetails>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.PageIndex < 0)
            {
                return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            }
            if (submitData.PageCount < 0)
            {
                return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.State))
            {
                return ObjectToJson.GetErrorResult("状态不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.Type.ToString()))
            {
                submitData.Type = 0;
            }

            string where = "";

            try
            {
                if (submitData.Type == 1)
                {
                    where = "a.State = '" + submitData.State + "' and VideoID='" + submitData.VideoID + "'";
                }
                else
                {
                    if (string.IsNullOrEmpty(submitData.AppID))
                    {
                        return ObjectToJson.GetErrorResult("版本ID不能为空！");
                    }
                    if (string.IsNullOrEmpty(submitData.BookID))
                    {
                        return ObjectToJson.GetErrorResult("书籍ID不能为空！");
                    }
                    if (string.IsNullOrEmpty(submitData.VideoNumber))
                    {
                        return ObjectToJson.GetErrorResult("视频序号不能为空！");
                    }
                    int versionId = 0;
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
                    TB_APPManagement appInfo = getAPPManagement(submitData.AppID, path);
                    if (appInfo != null)
                    {
                        versionId = Convert.ToInt32(appInfo.VersionID);
                    }
                    where = "a.BookID='" + submitData.BookID + "' AND a.VideoNumber='" + submitData.VideoNumber + "' AND a.VersionID = '" + versionId + "' AND a.State = '" + submitData.State + "'";

                }

                SqlParameter[] ps =
                {
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageCount", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar)
                };
                ps[0].Value = submitData.PageIndex;
                ps[1].Value = submitData.PageCount;
                ps[2].Value = where;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_VideoRankingInfo", ps);
                List<VideoRankingInfo> vri = JsonHelper.DataSetToIList<VideoRankingInfo>(ds, 0);
                StringBuilder json = new StringBuilder();
                int num = 0;
                json.Append("[");
                if (vri != null)
                {
                    foreach (var item in vri.OrderByDescending(i => i.CreateTime))
                    {
                        string img = "";
                        string fileid = "";
                        if (submitData.IsEnableOss == 0)
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

                        json.Append("{\"ID\": \"" + item.ID + "\",\"UserID\": \"" + item.UserID + "\",\"CreateTime\": \"" + item.CreateTime + "\",\"NumberOfOraise\": \"" + item.NumberOfOraise + "\",");
                        json.Append("\"TotalScore\": \"" + item.TotalScore + "\",\"VideoTitle\": \"" + String2Json(item.VideoTitle) + "\",\"UserName\": \"" + tName + "\",");
                        json.Append("\"UserImage\": \"" + img + "\",\"NickName\": \"" + tName + "\",\"VideoFileId\": \"" + fileid + "\"");
                        json.Append("},");

                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            string ss = "[]";
            return ObjectToJson.GetResult(js.DeserializeObject(ss), "没有对应的数据！");
        }

        /// 获取已发布视频排行信息
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage VideoRankingInfoTest()
        {
            UserVideoDetails submitData = new UserVideoDetails();//JsonHelper.DecodeJson<UserVideoDetails>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            //            {"BookID":"168","VideoNumber":"4","VideoID":"21300","PageCount":"10","IsEnableOss":"1","State":"1","VersionType":"1","PageIndex":"1","AppID":"241ea176-fce7-4bd7-a65f-a7978aac1cd2"}

            submitData.BookID = "168";
            submitData.VideoNumber = "3";
            submitData.PageCount = 50;
            submitData.IsEnableOss = 1;
            submitData.State = "1";
            submitData.PageIndex = 1;
            submitData.AppID = "241ea176-fce7-4bd7-a65f-a7978aac1cd2";
            //if (submitData.PageIndex < 0)
            //{
            //    return ObjectToJson.GetErrorResult("当前页码不能小于0！");
            //}
            //if (submitData.PageCount < 0)
            //{
            //    return ObjectToJson.GetErrorResult("每页显示数量不能为空！");
            //}
            //if (string.IsNullOrEmpty(submitData.State))
            //{
            //    return ObjectToJson.GetErrorResult("状态不能为空！");
            //}
            if (string.IsNullOrEmpty(submitData.Type.ToString()))
            {
                submitData.Type = 0;
            }

            string where = "";
            try
            {
                if (submitData.Type == 1)
                {
                    where = "a.State = '" + submitData.State + "' and VideoID='" + submitData.VideoID + "'";
                }
                else
                {
                    if (string.IsNullOrEmpty(submitData.AppID))
                    {
                        return ObjectToJson.GetErrorResult("版本ID不能为空！");
                    }
                    if (string.IsNullOrEmpty(submitData.BookID))
                    {
                        return ObjectToJson.GetErrorResult("书籍ID不能为空！");
                    }
                    if (string.IsNullOrEmpty(submitData.VideoNumber))
                    {
                        return ObjectToJson.GetErrorResult("视频序号不能为空！");
                    }
                    int versionId = 0;
                    string path = System.Web.Hosting.HostingEnvironment.MapPath("/XmlFiles/APPManagement.xml");
                    TB_APPManagement appInfo = getAPPManagement(submitData.AppID, path);
                    if (appInfo != null)
                    {
                        versionId = Convert.ToInt32(appInfo.VersionID);
                    }
                    where = "a.BookID='" + submitData.BookID + "' AND a.VideoNumber='" + submitData.VideoNumber + "' AND a.VersionID = '" + versionId + "' AND a.State = '" + submitData.State + "'";

                }

                SqlParameter[] ps =
                {
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageCount", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar)
                };
                ps[0].Value = submitData.PageIndex;
                ps[1].Value = submitData.PageCount;
                ps[2].Value = where;

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.StoredProcedure, "Get_VideoRankingInfo", ps);
                List<VideoRankingInfo> vri = JsonHelper.DataSetToIList<VideoRankingInfo>(ds, 0);
                StringBuilder json = new StringBuilder();
                int num = 0;
                json.Append("[");
                if (vri != null)
                {
                    foreach (var item in vri.OrderByDescending(i => i.CreateTime))
                    {
                        string img = "";
                        string fileid = "";
                        if (submitData.IsEnableOss == 0)
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

                        json.Append("{\"ID\": \"" + item.ID + "\",\"UserID\": \"" + item.UserID + "\",\"CreateTime\": \"" + item.CreateTime + "\",\"NumberOfOraise\": \"" + item.NumberOfOraise + "\",");
                        json.Append("\"TotalScore\": \"" + item.TotalScore + "\",\"VideoTitle\": \"" + String2Json(item.VideoTitle) + "\",\"UserName\": \"" + tName + "\",");
                        json.Append("\"UserImage\": \"" + img + "\",\"NickName\": \"" + tName + "\",\"VideoFileId\": \"" + fileid + "\"");
                        json.Append("},");

                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            string ss = "[]";
            return ObjectToJson.GetResult(js.DeserializeObject(ss), "没有对应的数据！");
        }

        /// <summary>
        ///  获取视频详细成绩相关信息 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage VideoAchievementInfoAgo([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            VideoAchievement submitData = JsonHelper.DecodeJson<VideoAchievement>(request.Data);
            if (string.IsNullOrEmpty(submitData.ID))
            {
                return ObjectToJson.GetErrorResult("ID不能为0！");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }

            string sql = string.Empty;
            try
            {
                sql = string.Format(@"SELECT  a.ID ,
                                        a.NumberOfOraise ,
                                        a.TotalScore ,
                                        a.VideoFileID ,
                                        a.State ,
                                        (SELECT COUNT(UserID) FROM dbo.TB_PraiseStatistics WHERE UserVideoID='{0}' ) as NumberOfOraise ,
                                        b.UserName ,
                                        b.NickName ,
                                        b.UserImage ,
                                        b.UserID ,
                                        a.IsEnableOss,
                                        b.IsEnableOss as UIsEnableOss,
                                        c.VideoTitle,
                                        a.CreateTime,
                                        a.VideoType
                                FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                        LEFT JOIN ITSV_Base.[FZ_SynchronousStudy].dbo.Tb_UserInfo b ON b.UserID = a.UserID
                                        LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDetails] c ON c.BookID = a.BookID AND c.VideoNumber = a.VideoNumber OR c.ID=a.VideoID
                                    WHERE     a.ID='{0}'  AND b.IsUser=1", submitData.ID);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                sql = string.Format(@"SELECT    c.UserVideoID,  
                                            c.DialogueNumber ,
                                            c.DialogueScore
                                    FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                            INNER JOIN [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue] c ON a.ID = c.UserVideoID
                                    WHERE   a.ID='{0}'", submitData.ID);

                DataSet dsDialog = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                sql = string.Format(@"SELECT COUNT(1) FROM TB_PraiseStatistics WHERE  UserVideoID='{0}' AND UserID='{1}'", submitData.ID, submitData.UserID);
                int IsBool = 0;
                if (Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql)) <= 0)
                {
                    IsBool = 0;
                }
                else
                {
                    IsBool = 1;
                }

                StringBuilder json = new StringBuilder();
                //string strSql = string.Format(@"  UPDATE dbo.TB_UserVideoDetails SET PlayTimes+=1 WHERE id='{0}'", submitData.ID);
                //if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql) <= 0)
                //{
                //    return ObjectToJson.GetErrorResult("添加播放次数失败！");
                //}
                int num = 0;
                json.Append("[");
                if (ds.Tables[0].Rows.Count > 0 && null != ds.Tables[0])
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string img = "";
                        string fileid = "";
                        if (submitData.IsEnableOss == 0)
                        {

                            img = ds.Tables[0].Rows[i]["UserImage"].ToString();

                            if (ds.Tables[0].Rows[i]["IsEnableOss"].ToString() == "1")
                            {
                                string[] str = ds.Tables[0].Rows[i]["VideoFileId"].ToString().Split('/');
                                fileid = str[str.Length - 1].Split('.')[0];
                            }
                            else
                            {
                                fileid = ds.Tables[0].Rows[i]["VideoFileId"].ToString();
                            }
                        }
                        else
                        {
                            string time = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreateTime"]).ToString("yyyy/MM/dd");
                            img = ds.Tables[0].Rows[i]["UIsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["UserImage"].ToString() : _getFilesUrl + "?FileID=" + ds.Tables[0].Rows[i]["UserImage"].ToString();
                            fileid = ds.Tables[0].Rows[i]["IsEnableOss"].ToString() != "0" ? _getOssFilesUrl + ds.Tables[0].Rows[i]["VideoFileId"].ToString() : _getVideoFiles + time + "/" + ds.Tables[0].Rows[i]["VideoFileId"].ToString() + ".mp4";
                        }

                        json.Append("{\"ID\": \"" + ds.Tables[0].Rows[i]["ID"] + "\",\"UserID\": \"" + ds.Tables[0].Rows[i]["UserID"] + "\",\"CreateTime\": \"" + ds.Tables[0].Rows[i]["CreateTime"] + "\",\"NumberOfOraise\": \"" + ds.Tables[0].Rows[i]["NumberOfOraise"] + "\",");
                        json.Append("\"TotalScore\": \"" + ds.Tables[0].Rows[i]["TotalScore"] + "\",\"NumberOfOraise\": \"" + ds.Tables[0].Rows[i]["NumberOfOraise"] + "\",\"UserName\": \"" + ds.Tables[0].Rows[i]["UserName"] + "\",\"VideoTitle\": \"" + ds.Tables[0].Rows[i]["VideoTitle"] + "\",");
                        json.Append("\"VideoType\": \"" + ds.Tables[0].Rows[i]["VideoType"] + "\",\"UserImage\": \"" + img + "\",\"NickName\": \"" + ds.Tables[0].Rows[i]["NickName"] + "\",\"State\": \"" + ds.Tables[0].Rows[i]["State"] + "\",\"IsBool\": \"" + IsBool + "\",\"VideoFileId\": \"" + fileid + "\",");
                        json.Append("\"children\": [");

                        for (int j = 0; j < dsDialog.Tables[0].Rows.Count; j++)
                        {
                            if (ds.Tables[0].Rows[i]["ID"].ToString() == dsDialog.Tables[0].Rows[j]["UserVideoID"].ToString())
                            {
                                json.Append("{\"DialogueNumber\":\"" + dsDialog.Tables[0].Rows[j]["DialogueNumber"] + "\",\"DialogueScore\":\"" + dsDialog.Tables[0].Rows[j]["DialogueScore"] + "\"},");
                                num++;
                            }
                        };
                        if (num == 0)
                        {
                            json.Append("[");
                        }
                        else
                        {
                            num = 0;
                        }
                        json.Remove(json.Length - 1, 1);
                        json.Append("]},");

                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("没有对应的数据！");

        }
        /// <summary>
        ///  获取视频详细成绩相关信息 △
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage VideoAchievementInfo([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            VideoAchievement submitData = JsonHelper.DecodeJson<VideoAchievement>(request.Data);
            if (string.IsNullOrEmpty(submitData.ID))
            {
                return ObjectToJson.GetErrorResult("ID不能为0！");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            string Title = "";
            string sql = string.Empty;
            sql = string.Format(@"SELECT  ID ,
                                            TotalScore ,
                                            VideoFileID ,
                                            State ,
                                            UserID ,
                                            IsEnableOss,
                                            CreateTime,
                                            VideoType,
                                            BookID,
                                            VideoNumber
                                    FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] 
                                    WHERE     ID={0} AND State = 1
                                                AND BookID IS NOT NULL
                                                AND BookID <> 0 ", submitData.ID);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            IUserVideoDetails UVideo = JsonHelper.DataSetToIList<IUserVideoDetails>(ds, 0).FirstOrDefault();

            int bookid = 0;
            if (UVideo != null)
            {
                bookid = UVideo.BookID;
                sql = string.Format(@"SELECT TOP 1 VideoTitle FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE BookID={0} AND VideoNumber={1} ", UVideo.BookID, UVideo.VideoNumber);

                DataSet dsTitle = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (dsTitle.Tables.Count > 0)
                {
                    if (dsTitle.Tables[0].Rows.Count > 0)
                    {
                        Title = dsTitle.Tables[0].Rows[0][0].ToString();
                    }
                }
            }
            sql = string.Format(@"SELECT    UserVideoID,  
                                                DialogueNumber ,
                                                DialogueScore
                                        FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue] 
                                    WHERE   UserVideoID='{0}'", submitData.ID);



            DataSet dsDialog = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            int IsBool = 0;
            int NumberOfOraise = 0;
            //? 是否有问题，冲突
            Redis_IntDubb_VideoInfo intVideoInfo = redis.Get<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bookid, submitData.ID);
            if (intVideoInfo != null)
            {
                if (intVideoInfo.NumberOfOraise.Contains(submitData.UserID))
                {
                    IsBool = 1;
                }
                NumberOfOraise = intVideoInfo.NumberOfOraise.Count;
            }
            else
            {
                IsBool = 0;
            }

            StringBuilder json = new StringBuilder();

            int num = 0;
            json.Append("[");
            if (UVideo != null)
            {
                IBS_UserInfo ibsUserinfo = userBLL.GetUserInfoByUserId(UVideo.UserID);

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

                if (submitData.IsEnableOss == 0)
                {
                    if (UVideo.IsEnableOss.ToString() == "1")
                    {
                        string[] str = UVideo.VideoFileID.Split('/');
                        fileid = str[str.Length - 1].Split('.')[0];
                    }
                    else
                    {
                        fileid = UVideo.VideoFileID;
                    }
                }
                else
                {
                    string time = Convert.ToDateTime(UVideo.CreateTime).ToString("yyyy/MM/dd");
                    fileid = UVideo.IsEnableOss.ToString() != "0" ? _getOssFilesUrl + UVideo.VideoFileID : _getVideoFiles + time + "/" + UVideo.VideoFileID + ".mp4";
                }

                json.Append("{\"ID\": \"" + UVideo.ID + "\",\"UserID\": \"" + userid + "\",\"CreateTime\": \"" + UVideo.CreateTime + "\",\"NumberOfOraise\": \"" + NumberOfOraise + "\",");
                json.Append("\"TotalScore\": \"" + UVideo.TotalScore + "\",\"UserName\": \"" + trueName + "\",\"VideoTitle\": \"" + Title + "\",");
                json.Append("\"VideoType\": \"" + UVideo.VideoType + "\",\"UserImage\": \"" + img + "\",\"NickName\": \"" + trueName + "\",\"State\": \"" + UVideo.State + "\",\"IsBool\": \"" + IsBool + "\",\"VideoFileId\": \"" + fileid + "\",");
                json.Append("\"children\": [");

                for (int j = 0; j < dsDialog.Tables[0].Rows.Count; j++)
                {
                    if (UVideo.ID.ToString() == dsDialog.Tables[0].Rows[j]["UserVideoID"].ToString())
                    {
                        json.Append("{\"DialogueNumber\":\"" + dsDialog.Tables[0].Rows[j]["DialogueNumber"] + "\",\"DialogueScore\":\"" + Convert.ToDouble(dsDialog.Tables[0].Rows[j]["DialogueScore"]).ToString("0.0") + "\"},");
                        num++;
                    }
                };
                if (num == 0)
                {
                    json.Append("[");
                }
                else
                {
                    num = 0;
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]}]");

                return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
            }
            return ObjectToJson.GetErrorResult("没有对应的数据！");
            //}
            //catch (Exception ex)
            //{
            //    log.Error("接口：VideoAchievementInfo", ex);
            //}
        }
        /// <summary>
        ///  获取视频详细成绩相关信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage VideoAchievementInfoTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            VideoAchievement submitData = new VideoAchievement();//JsonHelper.DecodeJson<VideoAchievement>(request.Data);
            //if (string.IsNullOrEmpty(submitData.ID))
            //{
            //    return ObjectToJson.GetErrorResult("ID不能为0！");
            //}
            //if (string.IsNullOrEmpty(submitData.UserID))
            //{6
            //    return ObjectToJson.GetErrorResult("用户ID不能为0！");
            //}
            submitData.UserID = "591084426";
            submitData.ID = "9743";
            string Title = "";
            string sql = string.Empty;
            //try
            //{
            sql = string.Format(@"SELECT  ID ,
                                            TotalScore ,
                                            VideoFileID ,
                                            State ,
                                            UserID ,
                                            IsEnableOss,
                                            CreateTime,
                                            VideoType,
                                            BookID,
                                            VideoNumber
                                    FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] 
                                    WHERE     ID={0} AND State = 1
                                                AND BookID IS NOT NULL
                                                AND BookID <> 0 ", submitData.ID);

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            IUserVideoDetails UVideo = JsonHelper.DataSetToIList<IUserVideoDetails>(ds, 0).FirstOrDefault();

            int bookid = 0;
            if (UVideo != null)
            {
                bookid = UVideo.BookID;
                sql = string.Format(@"SELECT TOP 1 VideoTitle FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE BookID={0} AND VideoNumber={1} ", UVideo.BookID, UVideo.VideoNumber);
                DataSet dsTitle = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (dsTitle.Tables.Count > 0)
                {
                    if (dsTitle.Tables[0].Rows.Count > 0)
                    {
                        Title = dsTitle.Tables[0].Rows[0][0].ToString();
                    }
                }
            }
            sql = string.Format(@"SELECT    UserVideoID,  
                                                DialogueNumber ,
                                                DialogueScore
                                        FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue] 
                                    WHERE   UserVideoID='{0}'", submitData.ID);

            DataSet dsDialog = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            int IsBool = 0;
            int NumberOfOraise = 0;
            Redis_IntDubb_VideoInfo intVideoInfo = redis.Get<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bookid, submitData.ID);
            if (intVideoInfo != null)
            {
                if (intVideoInfo.NumberOfOraise.Contains(submitData.UserID))
                {
                    IsBool = 1;
                }
                NumberOfOraise = intVideoInfo.NumberOfOraise.Count;
            }
            else
            {
                IsBool = 0;
            }

            StringBuilder json = new StringBuilder();

            int num = 0;
            json.Append("[");
            if (UVideo != null)
            {
                IBS_UserInfo ibsUserinfo = userBLL.GetUserInfoByUserId(UVideo.UserID);

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

                if (submitData.IsEnableOss == 0)
                {
                    if (UVideo.IsEnableOss.ToString() == "1")
                    {
                        string[] str = UVideo.VideoFileID.Split('/');
                        fileid = str[str.Length - 1].Split('.')[0];
                    }
                    else
                    {
                        fileid = UVideo.VideoFileID.ToString();
                    }
                }
                else
                {
                    string time = Convert.ToDateTime(UVideo.CreateTime).ToString("yyyy/MM/dd");
                    fileid = UVideo.IsEnableOss.ToString() != "0" ? _getOssFilesUrl + UVideo.VideoFileID : _getVideoFiles + time + "/" + UVideo.VideoFileID + ".mp4";
                }

                json.Append("{\"ID\": \"" + UVideo.ID + "\",\"UserID\": \"" + userid + "\",\"CreateTime\": \"" + UVideo.CreateTime + "\",\"NumberOfOraise\": \"" + NumberOfOraise + "\",");
                json.Append("\"TotalScore\": \"" + UVideo.TotalScore + "\",\"UserName\": \"" + trueName + "\",\"VideoTitle\": \"" + Title + "\",");
                json.Append("\"VideoType\": \"" + UVideo.VideoType + "\",\"UserImage\": \"" + img + "\",\"NickName\": \"" + trueName + "\",\"State\": \"" + UVideo.State + "\",\"IsBool\": \"" + IsBool + "\",\"VideoFileId\": \"" + fileid + "\",");
                json.Append("\"children\": [");

                for (int j = 0; j < dsDialog.Tables[0].Rows.Count; j++)
                {
                    if (UVideo.ID.ToString() == dsDialog.Tables[0].Rows[j]["UserVideoID"].ToString())
                    {
                        json.Append("{\"DialogueNumber\":\"" + dsDialog.Tables[0].Rows[j]["DialogueNumber"] + "\",\"DialogueScore\":\"" + Convert.ToDouble(dsDialog.Tables[0].Rows[j]["DialogueScore"]).ToString("0.0") + "\"},");
                        num++;
                    }
                };
                if (num == 0)
                {
                    json.Append("[");
                }
                else
                {
                    num = 0;
                }
                json.Remove(json.Length - 1, 1);
                json.Append("]}]");

                return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
            }
            return ObjectToJson.GetErrorResult("没有对应的数据！");
        }

        /// <summary>
        /// 增加点赞数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddNumberOfOraise([FromBody]KingRequest request)
        {
            TB_UserVideoDetails submitData = JsonHelper.DecodeJson<TB_UserVideoDetails>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("ID不能为空！");
            }
            if (submitData.UserID <= 0)
            {
                return ObjectToJson.GetErrorResult("用户ID不能为空！");
            }
            try
            {
                string sql = string.Format(@"SELECT BookID FROM FZ_InterestDubbing.dbo.TB_UserVideoDetails WHERE id={0}", submitData.ID);
                string bid = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql).ToString();
                if (!string.IsNullOrEmpty(bid))
                {
                    Redis_IntDubb_VideoInfo intVideoInfo = redis.Get<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bid, submitData.ID.ToString());
                    if (intVideoInfo != null)
                    {
                        List<string> list = intVideoInfo.NumberOfOraise;
                        if (!intVideoInfo.NumberOfOraise.Contains(submitData.UserID.ToString()))
                        {
                            list.Add(submitData.UserID.ToString());
                        }

                        intVideoInfo.NumberOfOraise = list;

                        redis.Set<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bid, submitData.ID.ToString(), intVideoInfo);
                        string ss = "{\"state\":\"0\"}";
                        return ObjectToJson.GetResult(js.DeserializeObject(ss), "点赞+1");
                    }
                }


                //string sql = string.Format(@"INSERT INTO [TB_PraiseStatistics] ([UserVideoID],[UserID])VALUES({0},{1})", submitData.ID, submitData.UserID);
                //int flag = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                //if (flag > 0)
                //{
                //    string ss = "{\"state\":\"0\"}";
                //    return ObjectToJson.GetResult(js.DeserializeObject(ss), "点赞+1");
                //}
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("点赞失败");
        }

        /// <summary>
        /// 删除点赞数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteNumberOfOraise([FromBody]KingRequest request)
        {
            TB_UserVideoDetails submitData = JsonHelper.DecodeJson<TB_UserVideoDetails>(request.Data);
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("参数错误！");
            }
            if (submitData.UserID <= 0)
            {
                return ObjectToJson.GetErrorResult("用户ID不能为空！");
            }

            string sql = string.Format(@"SELECT BookID FROM FZ_InterestDubbing.dbo.TB_UserVideoDetails WHERE id={0}", submitData.ID);
            string bid = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql).ToString();
            if (!string.IsNullOrEmpty(bid))
            {
                Redis_IntDubb_VideoInfo intVideoInfo = redis.Get<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bid, submitData.ID.ToString());
                if (intVideoInfo != null)
                {
                    List<string> list = intVideoInfo.NumberOfOraise;
                    if (intVideoInfo.NumberOfOraise.Contains(submitData.UserID.ToString()))
                    {
                        list.Remove(submitData.UserID.ToString());
                    }

                    intVideoInfo.NumberOfOraise = list;

                    redis.Set<Redis_IntDubb_VideoInfo>("Redis_IntDubb_VideoInfo_" + bid, submitData.ID.ToString(), intVideoInfo);
                    string ss = "{\"state\":\"0\"}";
                    return ObjectToJson.GetResult("点赞-1");

                    //return ObjectToJson.GetResult(js.DeserializeObject(ss), "点赞+1");
                }
            }

            return ObjectToJson.GetErrorResult("取消点赞失败");
            //try
            //{
            //string sql = string.Format(@"DELETE FROM TB_PraiseStatistics WHERE UserVideoID={0} AND UserID={1}", submitData.ID, submitData.UserID);
            //int flag = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            //if (flag > 0)
            //{
            //}
            //else
            //{
            //    return ObjectToJson.GetErrorResult("取消点赞失败");
            //}
            //}
            //catch (Exception ex)
            //{
            //    log.Error("error", ex);
            //}
        }

        /// <summary>
        /// 删除点赞数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteNumberOfOraiseTest()
        {
            TB_UserVideoDetails submitData = new TB_UserVideoDetails();//JsonHelper.DecodeJson<TB_UserVideoDetails>(request.data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            submitData.ID = 2;
            submitData.UserID = 123;
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("参数错误！");
            }
            if (submitData.UserID <= 0)
            {
                return ObjectToJson.GetErrorResult("用户ID不能为空！");
            }
            //string strSql = @"UPDATE dbo.TB_UserVideoDetails SET NumberOfOraise=NumberOfOraise-1 WHERE ID=" + submitData.ID;
            //string sql = string.Format(@"DELETE FROM TB_PraiseStatistics WHERE UserVideoID='{0}' AND UserID='{1}'", submitData.ID, submitData.UserID);

            //if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0 && Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, strSql)) > 0)
            //{
            //    string aSql = @"SELECT NumberOfOraise FROM TB_UserVideoDetails WHERE ID=" + submitData.ID;
            //    int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));

            //    string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
            //    return ObjectToJson.GetResult(js.DeserializeObject(ss), "点赞-1");
            //}
            //else
            //{
            //    return ObjectToJson.GetErrorResult("取消点赞失败");
            //} 
            try
            {
                //string strSql = @"UPDATE dbo.TB_UserVideoDetails SET NumberOfOraise=NumberOfOraise-1 WHERE ID=" + submitData.ID;
                string sql = string.Format(@"DELETE FROM TB_PraiseStatistics WHERE UserVideoID='{0}' AND UserID='{1}'", submitData.ID, submitData.UserID);
                int flag = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                if (flag > 0)
                {
                    //string aSql = "select COUNT(1) from TB_PraiseStatistics where UserVideoID=" + submitData.ID;
                    //int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                    string ss = "{\"state\":\"0\"}";
                    return ObjectToJson.GetResult(js.DeserializeObject(ss), "点赞-1");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            #region 采用存储过程
            //try
            //{
            //    SqlParameter[] para = new SqlParameter[] { 
            //    new SqlParameter("@OperateType",DbType.Int32),
            //    new SqlParameter("@ID",submitData.ID),
            //    new SqlParameter("@UserID",submitData.UserID),
            //    new SqlParameter("@NumberOfOraise",DbType.Int32)
            //     };
            //    para[0].Value = 0;//0:删除；1:添加
            //    para[3].Direction = ParameterDirection.Output;
            //    int count = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, "proc_AddDelNumberOfOraise", para); 
            //    if (count > 1)
            //    {
            //        int num = Convert.ToInt32(para[3].Value);
            //        string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
            //        return ObjectToJson.GetResult(js.DeserializeObject(ss), "点赞-1");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("error", ex);
            //} 
            #endregion
            return ObjectToJson.GetErrorResult("取消点赞失败");
        }

        /// <summary>
        /// 增加播放次数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PlayTimes([FromBody]KingRequest request)
        {
            TB_UserVideoDetails submitData = JsonHelper.DecodeJson<TB_UserVideoDetails>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("参数错误！");
            }
            try
            {
                string sql = @"UPDATE dbo.[FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] SET PlayTimes=PlayTimes+1 WHERE ID=" + submitData.ID;
                if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
                {
                    string ss = "{\"state\":\"0\"}";
                    return ObjectToJson.GetResult(js.DeserializeObject(ss), "播放次数+1");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("添加失败");
        }

        /// <summary>
        /// 增加播放次数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage PlayTimesTest()
        {
            TB_UserVideoDetails submitData = new TB_UserVideoDetails();//JsonHelper.DecodeJson<TB_UserVideoDetails>(request.data);
            submitData.ID = 17;
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("参数错误！");
            }
            string sql = @"UPDATE [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] SET PlayTimes=PlayTimes+1 WHERE ID=" + submitData.ID;
            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                return ObjectToJson.GetResult("", "播放次数+1");
            }
            else
            {
                return ObjectToJson.GetErrorResult("添加失败");
            }
        }

        /// <summary>
        /// 根据UserVideoID获取用户点赞数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPraiseTest()
        {

            //TB_PraiseStatistics submitData = JsonHelper.DecodeJson<TB_PraiseStatistics>(request.Data);
            TB_PraiseStatistics submitData = new TB_PraiseStatistics();
            JavaScriptSerializer js = new JavaScriptSerializer();
            submitData.UserVideoID = 2;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("参数不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserVideoID.ToString()))
            {
                return ObjectToJson.GetErrorResult("配音视频ID不能为空！");
            }
            try
            {
                string sql = string.Format(@"SELECT COUNT(1) FROM TB_PraiseStatistics WHERE  UserVideoID='{0}'", submitData.UserVideoID);
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "获取用户点赞数成功");
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("获取用户点赞数失败");
        }
        /// <summary>
        /// 根据UserVideoID获取用户点赞数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetPraise([FromBody] KingRequest request)
        {

            TB_PraiseStatistics submitData = JsonHelper.DecodeJson<TB_PraiseStatistics>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("参数不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserVideoID.ToString()))
            {
                return ObjectToJson.GetErrorResult("配音视频ID不能为空！");
            }
            try
            {
                string sql = string.Format(@"SELECT COUNT(1) FROM TB_PraiseStatistics WHERE  UserVideoID='{0}'", submitData.UserVideoID);
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "获取用户点赞数成功");
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("获取用户点赞数失败");
        }
        /// <summary>
        /// 查询用户点赞记录(是否点赞)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectPraise([FromBody] KingRequest request)
        {
            TB_PraiseStatistics submitData = JsonHelper.DecodeJson<TB_PraiseStatistics>(request.Data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            string ss = string.Empty;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("参数不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.UserVideoID.ToString()))
            {
                return ObjectToJson.GetErrorResult("配音视频ID不能为空！");
            }
            if (submitData.UserID <= 0)
            {
                return ObjectToJson.GetErrorResult("用户ID不能为空！");
            }
            try
            {
                string sql = string.Format(@"SELECT COUNT(1) FROM TB_PraiseStatistics WHERE  UserVideoID='{0}' AND UserID='{1}'", submitData.UserVideoID, submitData.UserID);
                if (Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql)) <= 0)
                {
                    ss = "{\"state\":\"0\"}";
                    return ObjectToJson.GetResult(js.DeserializeObject(ss), "当前视频用户没点赞");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            ss = "{\"state\":\"1\"}";
            return ObjectToJson.GetResult(js.DeserializeObject(ss), "当前视频用户已点赞");
        }

        /// <summary>
        /// 获取用户APP使用时长
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserUseTime([FromBody]KingRequest request)
        {
            return ObjectToJson.GetResult("", "信息录入成功");
            string where = "";
            UserStatistics submitData = JsonHelper.DecodeJson<UserStatistics>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.UserId <= 0)
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            if (string.IsNullOrEmpty(submitData.AppId))
            {
                return ObjectToJson.GetErrorResult("AppID不能为空！");
            }
            if (string.IsNullOrEmpty(submitData.State))
            {
                return ObjectToJson.GetErrorResult("状态不能为空！");
            }
            int num = 0;
            //string sqls = string.Format(@"SELECT [CreateDate]  FROM [TB_UserClassRelation] WHERE UserID='{0}'", submitData.UserId);
            //DataSet dsC = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sqls);
            try
            {
                IBS_ClassUserRelation classlist = new IBS_ClassUserRelation();
                var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserId));
                if (user != null)
                {
                    user.ClassSchList.ForEach(a =>
                    {
                        var classinfo = classBLL.GetClassUserRelationByClassId(a.ClassID);
                        if (classinfo != null)
                        {
                            classlist.CreateDate = classinfo.CreateDate;
                        }
                    });
                }
                /* string sSql = string.Format(@"SELECT UserID,CreateDate FROM  TB_UserClassRelation WHERE UserID={0}", submitData.UserId);
                 DataSet dsClass = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sSql);*/

                string strSql = string.Format(@"SELECT [Number],[LoginTime],[CreateDate]  FROM [TB_UserStatistics] WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
                if (submitData.State == "1")
                {
                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                        string sql = string.Format(@"INSERT INTO TB_UserStatistics ([UserID],[AppID],[Number],[LoginTime],[UseTime]) VALUES  ('{0}','{1}','0',getdate(),0)", submitData.UserId, submitData.AppId);
                        if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
                        {
                            return ObjectToJson.GetResult("", "信息录入成功！");
                        }
                        else
                        {
                            return ObjectToJson.GetErrorResult("信息录入失败");
                        }
                    }
                    else
                    {
                        if (classlist != null)
                        {
                            if (DateTime.Now.Day - Convert.ToDateTime(classlist.CreateDate).Day <= 7)
                            {
                                if (Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]).Day < DateTime.Now.Day)
                                {
                                    //string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                                    string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET Number=Number+1,LoginTime=getdate() WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                                    num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                                }
                                else
                                {
                                    //string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                                    string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET LoginTime=getdate() WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                                    num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                                }
                            }
                            else
                            {
                                //string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                                string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET LoginTime=getdate() WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                                num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                            }
                        }
                        else
                        {
                            //string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                            string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET LoginTime=getdate() WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                            num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                        }
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                        string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET UseTime=UseTime+{2} WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId, time);
                        num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("信息录入失败");
                    }
                }
                if (num <= 0)
                {
                    return ObjectToJson.GetErrorResult("信息录入失败");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetResult("", "信息录入成功");

        }

        [HttpGet]
        public HttpResponseMessage GetUserUseTimeTest()
        {
            string where = "";
            UserStatistics submitData = new UserStatistics();//JsonHelper.DecodeJson<UserStatistics>(request.data);
            submitData.UserId = 5;
            submitData.AppId = "5";
            submitData.State = "1";
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.UserId <= 0)
            {
                return ObjectToJson.GetErrorResult("用户ID不能为0！");
            }
            if (string.IsNullOrEmpty(submitData.AppId))
            {
                return ObjectToJson.GetErrorResult("AppID不能为空！");
            }
            //if (string.IsNullOrEmpty(submitData.State))
            //{
            //    return ObjectToJson.GetErrorResult("状态不能为空！");
            //}
            int num = 0;
            string strSql = string.Format(@"SELECT [Number],[LoginTime],[CreateDate]  FROM [TB_UserStatistics] WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);
            if (submitData.State == "1")
            {
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    string sql = string.Format(@"INSERT INTO TB_UserStatistics ([UserID],[AppID],[Number],[LoginTime],[UseTime]) VALUES  ('{0}','{1}','1',getdate(),0)", submitData.UserId, submitData.AppId);
                    if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
                    {
                        return ObjectToJson.GetResult("", "信息录入成功！");
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("信息录入失败");
                    }
                }
                else
                {
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]).Day <= DateTime.Now.Day && (DateTime.Now.Day - Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]).Day) <= 7)
                    {
                        if ((Convert.ToDateTime(ds.Tables[0].Rows[0]["CreateDate"]).Day + Convert.ToInt32(ds.Tables[0].Rows[0]["Number"])) < DateTime.Now.Day)
                        {
                            //string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                            string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET Number=Number+1,LoginTime=getdate() WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                            num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                        }
                        else
                        {
                            //string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                            string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET LoginTime=getdate() WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId);
                            num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
                        }

                    }
                }
            }
            else
            {
                string time = DateDiff(DateTime.Now, Convert.ToDateTime(ds.Tables[0].Rows[0]["LoginTime"]));
                string sql = string.Format(@"UPDATE dbo.TB_UserStatistics SET UseTime=UseTime+{2} WHERE UserID='{0}' AND AppID='{1}'", submitData.UserId, submitData.AppId, time);
                num = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql);
            }
            if (num <= 0)
            {
                return ObjectToJson.GetErrorResult("信息录入失败");
            }
            else
            {
                return ObjectToJson.GetResult("", "信息录入成功");
            }
        }

        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts = DateTime1.Subtract(DateTime2).Duration();
            //dateDiff =  ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;
            return ts.TotalSeconds.ToString();
        }

        /// <summary>
        /// 添加视频分享次数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddShareVideoNum([FromBody] KingRequest request)
        {
            TB_UserVideoDetails submitData = JsonHelper.DecodeJson<TB_UserVideoDetails>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空！");
            }
            if (submitData.ID <= 0)
            {
                return ObjectToJson.GetErrorResult("ID不能为空！");
            }

            string sql = string.Format(@"SELECT BookID,VideoNumber,VideoID,VideoType FROM  dbo.TB_UserVideoDetails WHERE id='{0}'", submitData.ID);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            string str = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["VideoType"].ToString() == "0")
                {
                    str = string.Format(@"  UPDATE [FZ_InterestDubbing].[dbo].[TB_VideoDetails] SET ShareVideoNum=ShareVideoNum+1 WHERE BookID='{0}' AND VideoNumber='{1}'", ds.Tables[0].Rows[0]["BookID"], ds.Tables[0].Rows[0]["VideoNumber"]);
                }
                else if (ds.Tables[0].Rows[0]["VideoType"].ToString() == "1")
                {
                    str = string.Format(@"  UPDATE [FZ_InterestDubbing].[dbo].[TB_VideoDetails] SET ShareVideoNum=ShareVideoNum+1 WHERE id='{0}'", ds.Tables[0].Rows[0]["VideoID"]);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, str) > 0)
                    {
                        return ObjectToJson.GetResult("分享次数+1！");
                    }
                    return ObjectToJson.GetErrorResult("分享次数添加失败！");
                }
                return ObjectToJson.GetErrorResult("SQL语句为空！");
            }
            return ObjectToJson.GetErrorResult("用户配音不存在！");
        }

        /// <summary>
        /// 获取已发布视频信息列表  state 状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetPublishedList([FromBody]KingRequest request)
        {
            string where = "";
            UserVideo submitData = JsonHelper.DecodeJson<UserVideo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.UserID == null)
            {
                return ObjectToJson.GetErrorResult("当前用户信息不存在");
            }

            //判断版本信息是否存在 如果不传值 则默认是返回所有的
            if (submitData.VersionID != null)
            {
                where = " VersionID=" + submitData.VersionID;
            }
            if (submitData.State != null)
            {
                where += " and State=" + submitData.State;
            }
            where += "and UserID='" + submitData.UserID + "'";
            try
            {
                object obj = videoDetailsBLLs.GetVideoDetails(where);
                if (obj != null)
                {
                    return ObjectToJson.GetResult(obj);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("数据库信息为空");

        }

        /// <summary>
        /// 获取已发布视频信息列表  state 状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetPublishedList()
        {
            string json = "{\"VersionID\":\"24\",\"UserID\":\"372998811\",\"State\":\"1\"}";
            string where = "";
            UserVideo submitData = JsonHelper.DecodeJson<UserVideo>(json);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (submitData.UserID == null)
            {
                return ObjectToJson.GetErrorResult("当前用户信息不存在");
            }

            //判断版本信息是否存在 如果不传值 则默认是返回所有的
            if (submitData.VersionID != null)
            {
                where = " VersionID=" + submitData.VersionID;
            }
            if (submitData.State != null)
            {
                where += " and State=" + submitData.State;
            }
            where += "and UserID='" + submitData.UserID + "'";
            try
            {
                object obj = videoDetailsBLLs.GetVideoDetails(where);
                if (obj != null)
                {
                    return ObjectToJson.GetResult(obj);
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("数据库信息为空");

        }

        [HttpPost]
        public HttpResponseMessage InsertUserStatistics([FromBody]KingRequest request)
        {
            TB_UserStatistics submitData = JsonHelper.DecodeJson<TB_UserStatistics>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }

            return ObjectToJson.GetResult("");
        }

        //判断字符串是否为空
        private string StringTOJson(string str)
        {
            string result = "";
            if (string.IsNullOrEmpty(str))
            {
                return result;
            }
            return str;
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '+':
                        sb.Append("\\n"); break;
                    //case '+':
                    //    sb.Append("\\n"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换int型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ParseInt(object obj)
        {
            int reInt = -1;
            if (obj != null)
                int.TryParse(obj.ToString(), out reInt);
            return reInt;
        }

        /// <summary>
        /// 把字符串转化为秒
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetDate(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";
            }
            else
            {
                DateTime date = DateTime.Now;
                string dateStr = date.ToString("yyyy-MM-dd");
                DateTime nowdatetime = DateTime.Parse(dateStr);
                DateTime enddatetime = Convert.ToDateTime(s);
                TimeSpan nowtimespan = new TimeSpan(nowdatetime.Ticks);
                TimeSpan endtimespan = new TimeSpan(enddatetime.Ticks);
                TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                // DateTime dt = Convert.ToDateTime(dataRow[column.ColumnName].ToString());
                //  int time = (Convert.ToInt32(dt.Hour) * 3600) + (Convert.ToInt32(dt.Minute) * 60) + Convert.ToInt32(dt.Second);
                //  TimeSpan endtimespan = new TimeSpan(dt.Ticks);
                return timespan.TotalSeconds.ToString();
            }
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary>
        /// 根据ID和xml路径得到TB_APPManagement
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public TB_APPManagement getAPPManagement(string ID, string path)
        {
            TB_APPManagement model = null;
            try
            {
                bool flag = false;
                //读取
                XmlHelper xmlHelper = new XmlHelper(path);
                DataTable dt_xml = xmlHelper.GetDataSetByXml(path) == null ? null : xmlHelper.GetDataSetByXml(path).Tables[0];
                if (dt_xml != null && dt_xml.Rows.Count > 0)
                {
                    foreach (DataRow row in dt_xml.Rows)
                    {
                        if (row["ID"].Equals(ID))
                        {
                            model = new TB_APPManagement()
                            {
                                ID = row["ID"].ToString(),
                                VersionName = row["VersionName"].ToString(),
                                VersionID = Convert.ToInt32(string.IsNullOrEmpty(row["VersionID"].ToString()) ? 0 : row["VersionID"]),
                                CreatePerson = row["CreatePerson"].ToString(),
                                CreateDate = DateTime.Parse(row["CreateDate"].ToString())
                            };
                            flag = true;
                            break;
                        }
                    }
                }

                if (flag == false)
                {
                    DataTable dt = null;
                    dt = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, @"select * from TB_APPManagement").Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["ID"].Equals(ID))
                            {
                                model = new TB_APPManagement()
                                {
                                    ID = row["ID"].ToString(),
                                    VersionName = row["VersionName"].ToString(),
                                    VersionID = Convert.ToInt32(string.IsNullOrEmpty(row["VersionID"].ToString()) ? 0 : row["VersionID"]),
                                    CreatePerson = row["CreatePerson"].ToString(),
                                    CreateDate = DateTime.Parse(row["CreateDate"].ToString())
                                };
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag == true)
                    {
                        bool flag_xml = XmlHelper.WriteToXml(dt, path, "APPManagements", "APPManagement");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return model;
        }
    }

    public class UserVideoInfo
    {
        public string VideoID { get; set; }
        public string Type { get; set; }
        public string VideoType { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string VideoFileId { get; set; }
        public string UserImage { get; set; }
        public string BookId { get; set; }
        public string VideoNumber { get; set; }
        public string AppID { get; set; }
        public string VideoReleaseAddress { get; set; }
        public string VideoImageAddress { get; set; }
        public string TotalScore { get; set; }
        public string State { get; set; }
        public string UserVideoId { get; set; }
        public string NumberOfOraise { get; set; }
        public string DialogueNumber { get; set; }
        public string DialogueScore { get; set; }
        public string CreateTime { get; set; }
        public string VersionType { get; set; }
        public int IsEnableOss { get; set; }
        public Children[] children { get; set; }

        public int? IsYX { get; set; }
    }

    public class Children
    {
        public int ID { get; set; }
        public int DialogueNumber { get; set; }
        public float DialogueScore { get; set; }
    }

    public class VideoAchievement
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string VideoFileID { get; set; }
        public string TotalScore { get; set; }
        public string State { get; set; }
        public string DialogueNumber { get; set; }
        public string DialogueScore { get; set; }
        public int IsEnableOss { get; set; }

        public int? IsYX { get; set; }

    }

    public class VideoInfo
    {
        public int ID { get; set; }
        public int VideoID { get; set; }
        public string VideoTitle { get; set; }
        public string VideoImageAddress { get; set; }
        public string VideoReleaseAddress { get; set; }
        public double TotalScore { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string State { get; set; }
        public string VideoType { get; set; }
    }

    public class BookInfo
    {
        public string BookID { get; set; }
        public string FirstTitleID { get; set; }
        public string SecondTitleID { get; set; }
        public string FirstModularID { get; set; }

        public int? IsYX { get; set; }
    }

    public class UserVideo
    {
        public string UserID { get; set; }
        public string VersionID { get; set; }
        public string State { get; set; }
    }

    public class VideoRequset
    {
        public string Type { get; set; }
        public string AppID { get; set; }
        public string UserID { get; set; }
        public string State { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public int IsEnableOss { get; set; }

        public int? IsYX { get; set; }
    }

    public class DeleteVedioInfo
    {
        public string IDStr { get; set; }
        public string UserID { get; set; }
        public string State { get; set; }
    }

    public class UserVideoDetails
    {
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public string BookID { get; set; }
        public string VideoNumber { get; set; }
        public string AppID { get; set; }
        public string VideoID { get; set; }
        public string State { get; set; }
        public string VersionType { get; set; }
        public int Type { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class UserStatistics
    {
        public int UserId { get; set; }
        public string AppId { get; set; }
        public string State { get; set; }
    }

    public class videoDetailsInfo
    {
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public string VideoTitle { get; set; }
    }

    public class VideoAchievementList
    {
        public int ID { get; set; }
        public float TotalScore { get; set; }
        public string VideoFileID { get; set; }
        public int State { get; set; }
        public int NumberOfOraise { get; set; }
        public int UserID { get; set; }
        public int IsEnableOss { get; set; }
        public DateTime CreateTime { get; set; }
        public string VideoType { get; set; }
        public int IsBool { get; set; }
    }

    public class VideoAchievementUserList
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string TrueName { get; set; }
        public string UserImage { get; set; }
        public int UserID { get; set; }
        public int IsEnableOss { get; set; }
    }

    public class TBUserVideoDetails
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
    }

    public class VideoRankingInfo
    {
        public int ID { get; set; }
        public int NumberOfOraise { get; set; }
        public double TotalScore { get; set; }
        public string VideoFileID { get; set; }
        public int State { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string NickName { get; set; }
        public int UserID { get; set; }
        public string VideoTitle { get; set; }
        public int BookID { get; set; }
        public int VideoNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsEnableOss { get; set; }
        public string TrueName { get; set; }
        public int UIsEnableOss { get; set; }
    }
    public class UserSchoolRankParaModel
    {
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public string SchoolID { get; set; }
        public string ClassID { get; set; }
        public string BookID { get; set; }
        public string VideoNumber { get; set; }
        public string UserID { get; set; }
    }

    public class UserSchoolRankReponse
    {
        /// <summary>
        /// 当前用户排名
        /// </summary>
        public Redis_IntDubb_VideoInfoSort CurrentUserRank { get; set; }
        /// <summary>
        /// 用户排名列表
        /// </summary>
        public List<Redis_IntDubb_VideoInfoSort> RankList { get; set; }
    }
}