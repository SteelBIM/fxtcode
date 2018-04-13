using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using Kingsun.InterestDubbingGame.BLL;
using Kingsun.InterestDubbingGame.Model;
using log4net;
using System.Data.SqlClient;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.InterestDubbingGame.Web.Controllers
{
    public class InterestDubbingGameMatchApiController : ApiController
    {
        private InterestDubbingGameMatchApiBLL idgbll = new InterestDubbingGameMatchApiBLL();
        private TB_InterestDubbingGame_MatchBLL matchbll = new TB_InterestDubbingGame_MatchBLL();
        JavaScriptSerializer js = new JavaScriptSerializer();
        static RedisHashHelper redis = new RedisHashHelper();
        public string Time = System.Configuration.ConfigurationManager.AppSettings["setActiveTime"];

        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        #region 补全redis和DB的用户信息
        /// <summary>
        /// 补全redis上用户信息，同事也补全DB的用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SyncUserInfoToRedisDB()
        {
            try
            {
                string connect_com = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;
                string kingsun_qpy = System.Configuration.ConfigurationManager.ConnectionStrings["InterestDubbingGameConnectionStr"].ConnectionString;
                List<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo> list = redis.GetAll<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo");
                foreach (var item in list)
                {
                    //读取本地用户表的头像信息
                    string sql_getUserImg = string.Format("select UserImage from ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_userinfo] where UserID='{0}'", item.UserID);
                    DataTable dt_getUserImg = SqlHelper.ExecuteDataset(connect_com, CommandType.Text, sql_getUserImg).Tables[0];
                    if (dt_getUserImg != null && dt_getUserImg.Rows.Count > 0)
                    {
                        item.UserImage = dt_getUserImg.Rows[0]["UserImage"].ToString();
                    }
                    else
                    {
                        item.UserImage = "";
                    }
                    if (item.State == 0)
                    {
                        #region 通过Umms获取用户信息
                        try
                        {
                            var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(item.UserID));
                            if (user != null)
                            {
                                if (string.IsNullOrEmpty(item.UserName))
                                {
                                    item.UserName = user.iBS_UserInfo.TrueName;
                                }
                                if (user.ClassSchDetailList.Count > 0)
                                {
                                    var classinfo = classBLL.GetClassUserRelationByClassId(user.ClassSchDetailList[0].ClassID);
                                    if (classinfo != null)
                                    {
                                        if (string.IsNullOrEmpty(item.ClassID))
                                        {
                                            item.ClassID = classinfo.ClassNum.ToString();
                                            item.ClassName = classinfo.ClassName;
                                        }
                                        if (string.IsNullOrEmpty(item.GradeID.ToString()) || item.GradeID == 0)
                                        {
                                            item.GradeID = classinfo.GradeID;
                                            item.GradeName = GetGradeName(classinfo.GradeID);
                                        }
                                        if (!string.IsNullOrEmpty(classinfo.SchID.ToString()))
                                        {
                                            item.SchoolID = classinfo.SchID;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(user.ClassSchDetailList[0].SchID.ToString()))
                                        {
                                            item.SchoolID = user.ClassSchDetailList[0].SchID;
                                        }
                                        if (!string.IsNullOrEmpty(user.ClassSchDetailList[0].SchName))
                                        {
                                            item.SchoolName = user.ClassSchDetailList[0].SchName;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(item.SchoolID.ToString()) && item.SchoolID != 0)
                                    {
                                        try
                                        {
                                            item.AreaName = user.ClassSchDetailList[0].AreaName;
                                            item.SchoolName = user.ClassSchDetailList[0].SchName;
                                        }
                                        catch (Exception ex)
                                        {
                                            Log4Net.LogHelper.Info(string.Format("SyncUserInfoRedisFromUMS中SchoolID={0},UserID={1},未找到区域ID和区域", item.SchoolID, item.UserID));
                                        }
                                    }
                                    else
                                    {
                                        Log4Net.LogHelper.Info(string.Format("SyncUserInfoRedisFromUMS中SchoolID={0},UserID={1},未找到区域ID和区域", item.SchoolID, item.UserID));
                                    }
                                }
                                item.State = 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4Net.LogHelper.Error(string.Format("SyncUserInfoRedisFromUMS中UserID={0},同步异常,{1}", item.UserID,ex.Message));
                        }
                        #endregion
                    }
                    //同步数据到Redis
                    redis.Set<Kingsun.InterestDubbingGame.Model.TB_InterestDubbingGame_UserInfo>("TB_InterestDubbingGame_UserInfo", item.UserID, item);
                    //同步数据到DB
                    string sqlUpdate = string.Format(@"update TB_InterestDubbingGame_UserInfo set SchoolID='{1}',SchoolName='{2}',GradeID='{3}',GradeName='{4}',ClassID='{5}',ClassName='{6}', AreaID='{7}',AreaName='{8}',UserName='{9}',State=1 where UserID='{0}'", 
                        item.UserID, item.SchoolID, item.SchoolName, item.GradeID, item.GradeName, item.ClassID, item.ClassName,item.AreaID, item.AreaName, item.UserName);
                    if (SqlHelper.ExecuteNonQueryTimeOut(kingsun_qpy, CommandType.Text, sqlUpdate) <= 0)
                    {
                        Log4Net.LogHelper.Info("SyncUserInfoToRedisDB趣配音手动同步数据到DB失败，UserID=" + item.UserID);
                    }
                }
                return ObjectToJson.GetResult(true, "同步成功");
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "SyncUserInfoRedisFromUMS");
                return ObjectToJson.GetErrorResult("同步数据失败，" + ex.Message);
            }
        }
        /// <summary>
        /// 根据GradeID得到年级名称
        /// </summary>
        /// <param name="GradeID"></param>
        /// <returns></returns>
        public static string GetGradeName(int? GradeID)
        {
            string GradeName = "";
            switch (GradeID)
            {
                case 1:
                    GradeName = "学前";
                    break;
                case 2:
                    GradeName = "一年级";
                    break;
                case 3:
                    GradeName = "二年级";
                    break;
                case 4:
                    GradeName = "三年级";
                    break;
                case 5:
                    GradeName = "四年级";
                    break;
                case 6:
                    GradeName = "五年级";
                    break;
                case 7:
                    GradeName = "六年级";
                    break;
                case 8:
                    GradeName = "七年级";
                    break;
                case 9:
                    GradeName = "八年级";
                    break;
                case 10:
                    GradeName = "九年级";
                    break;
                default:
                    GradeName = "其它";
                    break;
            }
            return GradeName;
        } 
        #endregion
        /// <summary>
        /// 测试接口
        /// 金石
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage jinshitest()
        {
            for (int i = 0; i < 10; i++)
            {
                string value = "adfadsf";
                bool issuccess = redis.Set("jinshiTest222", "132456", value);
            }
            return ObjectToJson.GetErrorResult("没有对应的数据！");
        }

        /// <summary>
        /// 获取课本剧资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetBookDramatList([FromBody] KingRequest request)
        {
            IDGBookDramat submitData = JsonHelper.DecodeJson<IDGBookDramat>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            Redis_InterestDubbingGame_UserInfo idgUserinfo = idgbll.GetUserInfo(submitData.UserId);
            if (idgUserinfo != null)
            {
                try
                {
                    StringBuilder json = new StringBuilder();
                    string groupName = "";
                    switch (idgUserinfo.GradeID.ToString())
                    {
                        case "2":
                            groupName = "F1";
                            break;
                        case "3":
                            groupName = "F1";
                            break;
                        case "4":
                            groupName = "F2";
                            break;
                        case "5":
                            groupName = "F2";
                            break;
                        case "6":
                            groupName = "F3";
                            break;
                        case "7":
                            groupName = "F3";
                            break;
                        default:
                            return ObjectToJson.GetErrorResult("该功能只支持小学阶段的用户！");
                            break;
                    }
                    DataSet ds = idgbll.GetBookDramatList(groupName);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        json.Append("[");

                        string ID = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                ID = dt.Rows[i]["ID"].ToString();
                                json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"VideoTitle\":\"" + dt.Rows[i]["VideoTitle"].ToString() + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\"," +
                                            "\"MuteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["CompleteVideo"].ToString()) +
                                            "\",\"BackgroundAudio\":\"" + JsonHelper.String2Json(dt.Rows[i]["BackgroundAudio"].ToString()) + "\",\"VideoCover\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoCover"].ToString()) +
                                            "\",\"VideoDesc\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoDesc"].ToString()) + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() +
                                            "\",\"CreateTime\":\"" + dt.Rows[i]["CreateDate"].ToString() + "\",\"children\":[{\"ID\":\"" + dt.Rows[i]["Did"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoNumber"] +
                                            "\",\"DialogueText\":\"" + dt.Rows[i]["DialogueText"].ToString() + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"] +
                                            "\",\"StartTime\":\"" + JsonHelper.String2Json(JsonHelper.GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + JsonHelper.GetDate(dt.Rows[i]["EndTime"].ToString()) + "\"},");
                            }
                            else
                            {
                                if (ID == dt.Rows[i]["ID"].ToString())
                                {
                                    json.Append("{\"ID\":\"" + dt.Rows[i]["Did"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoNumber"] + "\",\"DialogueText\":\""
                                        + dt.Rows[i]["DialogueText"].ToString() + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                        + "\",\"StartTime\":\"" + JsonHelper.GetDate(dt.Rows[i]["StartTime"].ToString()) + "\",\"EndTime\":\"" + JsonHelper.String2Json(JsonHelper.GetDate(dt.Rows[i]["EndTime"].ToString()))
                                        + "\"},");
                                }
                                else
                                {
                                    json.Remove(json.Length - 1, 1);
                                    json.Append("]},");
                                    json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"VideoTitle\":\"" + dt.Rows[i]["VideoTitle"].ToString() + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\"," +
                                          "\"MuteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["CompleteVideo"].ToString()) +
                                          "\",\"BackgroundAudio\":\"" + JsonHelper.String2Json(dt.Rows[i]["BackgroundAudio"].ToString()) + "\",\"VideoCover\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoCover"].ToString()) +
                                          "\",\"VideoDesc\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoDesc"].ToString()) + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() +
                                          "\",\"CreateTime\":\"" + dt.Rows[i]["CreateDate"].ToString() + "\",\"children\":[{\"ID\":\"" + dt.Rows[i]["Did"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoNumber"] +
                                          "\",\"DialogueText\":\"" + dt.Rows[i]["DialogueText"].ToString() + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"] +
                                          "\",\"StartTime\":\"" + JsonHelper.String2Json(JsonHelper.GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + JsonHelper.GetDate(dt.Rows[i]["EndTime"].ToString()) + "\"},");

                                }
                                ID = dt.Rows[i]["ID"].ToString();
                            }
                        }
                        json.Remove(json.Length - 1, 1);
                        json.Append("]}]");
                        return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("数据为空！");
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误");
                    return ObjectToJson.GetErrorResult("没有对应的数据！");
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("没有对应的数据！");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBookDramatListTest()
        {
            IDGBookDramat submitData = new IDGBookDramat();//JsonHelper.DecodeJson<IDGBookDramat>(request.Data);
            submitData.UserId = "1000059685";
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            Redis_InterestDubbingGame_UserInfo idgUserinfo = idgbll.GetUserInfo(submitData.UserId);
            if (idgUserinfo != null)
            {
                try
                {
                    StringBuilder json = new StringBuilder();
                    string groupName = "";
                    switch (idgUserinfo.GradeID.ToString())
                    {
                        case "2":
                            groupName = "F1";
                            break;
                        case "3":
                            groupName = "F1";
                            break;
                        case "4":
                            groupName = "F2";
                            break;
                        case "5":
                            groupName = "F2";
                            break;
                        case "6":
                            groupName = "F3";
                            break;
                        case "7":
                            groupName = "F3";
                            break;
                        default:
                            return ObjectToJson.GetErrorResult("该功能只支持小学阶段的用户！");
                            break;
                    }
                    DataSet ds = idgbll.GetBookDramatList(groupName);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        json.Append("[");

                        string ID = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                ID = dt.Rows[i]["ID"].ToString();
                                json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"VideoTitle\":\"" + dt.Rows[i]["VideoTitle"].ToString() + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\"," +
                                            "\"MuteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["CompleteVideo"].ToString()) +
                                            "\",\"BackgroundAudio\":\"" + JsonHelper.String2Json(dt.Rows[i]["BackgroundAudio"].ToString()) + "\",\"VideoCover\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoCover"].ToString()) +
                                            "\",\"VideoDesc\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoDesc"].ToString()) + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() +
                                            "\",\"CreateTime\":\"" + dt.Rows[i]["CreateDate"].ToString() + "\",\"children\":[{\"ID\":\"" + dt.Rows[i]["Did"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoNumber"] +
                                            "\",\"DialogueText\":\"" + dt.Rows[i]["DialogueText"].ToString() + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"] +
                                            "\",\"StartTime\":\"" + JsonHelper.String2Json(JsonHelper.GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + JsonHelper.GetDate(dt.Rows[i]["EndTime"].ToString()) + "\"},");
                            }
                            else
                            {
                                if (ID == dt.Rows[i]["ID"].ToString())
                                {
                                    json.Append("{\"ID\":\"" + dt.Rows[i]["Did"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoNumber"] + "\",\"DialogueText\":\""
                                        + dt.Rows[i]["DialogueText"].ToString() + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"]
                                        + "\",\"StartTime\":\"" + JsonHelper.GetDate(dt.Rows[i]["StartTime"].ToString()) + "\",\"EndTime\":\"" + JsonHelper.String2Json(JsonHelper.GetDate(dt.Rows[i]["EndTime"].ToString()))
                                        + "\"},");
                                }
                                else
                                {
                                    json.Remove(json.Length - 1, 1);
                                    json.Append("]},");
                                    json.Append("{\"ID\":\"" + dt.Rows[i]["ID"] + "\",\"VideoTitle\":\"" + dt.Rows[i]["VideoTitle"].ToString() + "\",\"VideoNumber\":\"" + dt.Rows[i]["VideoNumber"].ToString() + "\"," +
                                          "\"MuteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["MuteVideo"].ToString()) + "\",\"CompleteVideo\":\"" + JsonHelper.String2Json(dt.Rows[i]["CompleteVideo"].ToString()) +
                                          "\",\"BackgroundAudio\":\"" + JsonHelper.String2Json(dt.Rows[i]["BackgroundAudio"].ToString()) + "\",\"VideoCover\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoCover"].ToString()) +
                                          "\",\"VideoDesc\":\"" + JsonHelper.String2Json(dt.Rows[i]["VideoDesc"].ToString()) + "\",\"VideoDifficulty\":\"" + dt.Rows[i]["VideoDifficulty"].ToString() +
                                          "\",\"CreateTime\":\"" + dt.Rows[i]["CreateDate"].ToString() + "\",\"children\":[{\"ID\":\"" + dt.Rows[i]["Did"] + "\",\"VideoID\":\"" + dt.Rows[i]["VideoNumber"] +
                                          "\",\"DialogueText\":\"" + dt.Rows[i]["DialogueText"].ToString() + "\",\"DialogueNumber\":\"" + dt.Rows[i]["DialogueNumber"] +
                                          "\",\"StartTime\":\"" + JsonHelper.String2Json(JsonHelper.GetDate(dt.Rows[i]["StartTime"].ToString())) + "\",\"EndTime\":\"" + JsonHelper.GetDate(dt.Rows[i]["EndTime"].ToString()) + "\"},");

                                }
                                ID = dt.Rows[i]["ID"].ToString();
                            }
                        }
                        json.Remove(json.Length - 1, 1);
                        json.Append("]}]");
                        return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("数据为空！");
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误");
                    return ObjectToJson.GetErrorResult("没有对应的数据！");
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("没有对应的数据！");
            }
        }

        /// <summary>
        /// 获取故事资源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetStoryReadList([FromBody] KingRequest request)
        {
            IDGBookDramat submitData = JsonHelper.DecodeJson<IDGBookDramat>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            Redis_InterestDubbingGame_UserInfo idgUserinfo = idgbll.GetUserInfo(submitData.UserId);
            if (idgUserinfo != null)
            {
                try
                {
                    string groupName = "";
                    switch (idgUserinfo.GradeID.ToString())
                    {
                        case "2":
                            groupName = "F1";
                            break;
                        case "3":
                            groupName = "F1";
                            break;
                        case "4":
                            groupName = "F2";
                            break;
                        case "5":
                            groupName = "F2";
                            break;
                        case "6":
                            groupName = "F3";
                            break;
                        case "7":
                            groupName = "F3";
                            break;
                        default:
                            return ObjectToJson.GetErrorResult("该功能只支持小学阶段的用户！");
                            break;
                    }
                    DataSet ds = idgbll.GetStoryReadList(groupName);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectToJson.GetResult(JsonHelper.DataSetToIList<QTB_IDG_StoryRead>(ds, 0));
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("数据为空！");
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误");
                    return ObjectToJson.GetErrorResult("没有对应的数据！");
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("没有对应的数据！");
            }
        }

        /// <summary>
        /// 获取故事资源
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetStoryReadListTest()
        {
            IDGBookDramat submitData = new IDGBookDramat();//JsonHelper.DecodeJson<IDGBookDramat>(request.Data);
            submitData.UserId = "319596948";
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            Redis_InterestDubbingGame_UserInfo idgUserinfo = idgbll.GetUserInfo(submitData.UserId);
            if (idgUserinfo != null)
            {
                try
                {
                    string groupName = "";
                    switch (idgUserinfo.GradeID.ToString())
                    {
                        case "2":
                            groupName = "F1";
                            break;
                        case "3":
                            groupName = "F1";
                            break;
                        case "4":
                            groupName = "F2";
                            break;
                        case "5":
                            groupName = "F2";
                            break;
                        case "6":
                            groupName = "F3";
                            break;
                        case "7":
                            groupName = "F3";
                            break;
                        default:
                            return ObjectToJson.GetErrorResult("该功能只支持小学阶段的用户！");
                            break;
                    }
                    DataSet ds = idgbll.GetStoryReadList(groupName);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return ObjectToJson.GetResult(JsonHelper.DataSetToIList<QTB_IDG_StoryRead>(ds, 0));
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("数据为空！");
                    }
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "错误");
                    return ObjectToJson.GetErrorResult("没有对应的数据！");
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("没有对应的数据！");
            }
        }

        /// <summary>
        /// 插入用户成绩
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertUserContentsRecord([FromBody] KingRequest request)
        {
            IDGUserContentsRecord submitData = JsonHelper.DecodeJson<IDGUserContentsRecord>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId.ToString()))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }
            if (string.IsNullOrEmpty(submitData.Type.ToString()))
            {
                return ObjectToJson.GetErrorResult("类型不能为空");
            }
            if (string.IsNullOrEmpty(submitData.VideoID.ToString()))
            {
                return ObjectToJson.GetErrorResult("视频ID不能为空");
            }
            if (string.IsNullOrEmpty(submitData.DubbingFilePath))
            {
                return ObjectToJson.GetErrorResult("视频路径不能为空");
            }
            if (string.IsNullOrEmpty(submitData.DubbingScore.ToString()))
            {
                return ObjectToJson.GetErrorResult("分数不能为空");
            }
            else
            {
                submitData.DubbingScore = double.Parse(submitData.DubbingScore.ToString("0.0"));//保留一位小数
            }
            bool bl = false;

            submitData.DubbingScore = submitData.DubbingScore <= 0 ? 0.1 : submitData.DubbingScore;

            TB_InterestDubbingGame_UserContentsRecord idgUserContent = new TB_InterestDubbingGame_UserContentsRecord
            {
                UserID = submitData.UserId,
                DubbingTitle = JsonHelper.String2Json(submitData.DubbingTitle),
                DubbingFilePath = JsonHelper.String2Json(submitData.DubbingFilePath),
                DubbingScore = submitData.DubbingScore,
                Type = submitData.Type,
                VideoID = submitData.VideoID
            };
            Redis_InterestDubbingGame_UserTotalScore usertotal = new Redis_InterestDubbingGame_UserTotalScore();

            Redis_InterestDubbingGame_UserTotalScore ridgUserTotalScore = redis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", submitData.UserId.ToString());
            if (ridgUserTotalScore == null)
            {
                switch (submitData.Type)
                {
                    case 0://课本剧
                        usertotal.BookPlayScore = submitData.DubbingScore;
                        usertotal.StoryReadScore = 0;
                        break;
                    case 1://故事朗诵
                        usertotal.BookPlayScore = 0;
                        usertotal.StoryReadScore = submitData.DubbingScore;
                        break;
                    default:
                        break;
                }
                usertotal.UserID = submitData.UserId;
                usertotal.TotalScore = submitData.DubbingScore;
                usertotal.VoteNum = 0;
            }
            else
            {
                switch (submitData.Type)
                {
                    case 0://课本剧
                        if (ridgUserTotalScore.BookPlayScore > 0) return ObjectToJson.GetResult("新增成功！");
                        usertotal.BookPlayScore = submitData.DubbingScore;
                        usertotal.StoryReadScore = ridgUserTotalScore.StoryReadScore;
                        break;
                    case 1://故事朗诵
                        if (ridgUserTotalScore.StoryReadScore > 0) return ObjectToJson.GetResult("新增成功！");
                        usertotal.BookPlayScore = ridgUserTotalScore.BookPlayScore;
                        usertotal.StoryReadScore = submitData.DubbingScore;
                        break;
                    default:
                        break;
                }
                usertotal.UserID = ridgUserTotalScore.UserID;
                usertotal.VoteNum = ridgUserTotalScore.VoteNum;
                usertotal.TotalScore = usertotal.BookPlayScore + usertotal.StoryReadScore + usertotal.VoteNum * 0.1;
            }

            try
            {
                redis.Set<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", submitData.UserId.ToString(), usertotal);
                idgbll.InsertUserContentsRecord(idgUserContent);
                string sql = string.Format(@"SELECT COUNT(1) FROM TB_InterestDubbingGame_UserContentsRecord WHERE UserID={0}", submitData.UserId);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql));
                if (count > 1)
                {
                    bl = true;
                }
                return ObjectToJson.GetResult(bl, "新增成功！");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) //已插入数据库
                {
                    return ObjectToJson.GetResult(bl, "新增成功！");
                }
                else
                {
                    Log4Net.LogHelper.Error(ex, "插入数据库失败，submitData=" + request.Data);
                    return ObjectToJson.GetErrorResult("插入数据库失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "错误");
                return ObjectToJson.GetErrorResult("新增失败！");
            }
        }

        [HttpGet]
        public HttpResponseMessage InsertUserContentsRecordTest(int Userid, int type)
        {
            IDGUserContentsRecord submitData = new IDGUserContentsRecord();//JsonHelper.DecodeJson<IDGUserContentsRecord>(request.Data);
            submitData.UserId = Userid;
            submitData.DubbingTitle = "Let's Go to Disneyland";
            submitData.DubbingFilePath = "2017/10/12/8ee90e27-3b89-4f2a-b0e9-e784c30ae1d5.mp3";
            submitData.DubbingScore = 89.7000;
            submitData.Type = type;
            submitData.VideoID = 135;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserId.ToString()))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }
            if (string.IsNullOrEmpty(submitData.Type.ToString()))
            {
                return ObjectToJson.GetErrorResult("类型不能为空");
            }
            if (string.IsNullOrEmpty(submitData.VideoID.ToString()))
            {
                return ObjectToJson.GetErrorResult("视频ID不能为空");
            }
            if (string.IsNullOrEmpty(submitData.DubbingFilePath))
            {
                return ObjectToJson.GetErrorResult("视频路径不能为空");
            }
            if (string.IsNullOrEmpty(submitData.DubbingScore.ToString()))
            {
                return ObjectToJson.GetErrorResult("分数不能为空");
            }
            bool bl = false;
            double storyReadScore = 0.0;
            double bookPlayScore = 0.0;
            submitData.DubbingScore = submitData.DubbingScore <= 0 ? 0.1 : submitData.DubbingScore;

            TB_InterestDubbingGame_UserContentsRecord idgUserContent = new TB_InterestDubbingGame_UserContentsRecord
            {
                UserID = submitData.UserId,
                DubbingTitle = JsonHelper.String2Json(submitData.DubbingTitle),
                DubbingFilePath = JsonHelper.String2Json(submitData.DubbingFilePath),
                DubbingScore = submitData.DubbingScore,
                Type = submitData.Type,
                VideoID = submitData.VideoID
            };
            Redis_InterestDubbingGame_UserTotalScore usertotal = new Redis_InterestDubbingGame_UserTotalScore();

            Redis_InterestDubbingGame_UserTotalScore ridgUserTotalScore = redis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", submitData.UserId.ToString());
            if (ridgUserTotalScore == null)
            {
                switch (submitData.Type)
                {
                    case 0://课本剧
                        usertotal.BookPlayScore = submitData.DubbingScore;
                        usertotal.StoryReadScore = 0;
                        break;
                    case 1://故事朗诵
                        usertotal.BookPlayScore = 0;
                        usertotal.StoryReadScore = submitData.DubbingScore;
                        break;
                    default:
                        break;
                }
                usertotal.UserID = submitData.UserId;
                usertotal.TotalScore = submitData.DubbingScore;
                usertotal.VoteNum = 0;
            }
            else
            {
                switch (submitData.Type)
                {
                    case 0://课本剧
                        if (ridgUserTotalScore.BookPlayScore > 0) return ObjectToJson.GetResult("新增成功！");
                        usertotal.BookPlayScore = submitData.DubbingScore;
                        usertotal.StoryReadScore = ridgUserTotalScore.StoryReadScore;
                        break;
                    case 1://故事朗诵
                        if (ridgUserTotalScore.StoryReadScore > 0) return ObjectToJson.GetResult("新增成功！");
                        usertotal.BookPlayScore = ridgUserTotalScore.BookPlayScore;
                        usertotal.StoryReadScore = submitData.DubbingScore;
                        break;
                    default:
                        break;
                }
                usertotal.UserID = ridgUserTotalScore.UserID;
                usertotal.VoteNum = ridgUserTotalScore.VoteNum;
                usertotal.TotalScore = usertotal.BookPlayScore + usertotal.StoryReadScore + usertotal.VoteNum * 0.1;
            }

            try
            {
                redis.Set<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", submitData.UserId.ToString(), usertotal);
                if (idgbll.InsertUserContentsRecord(idgUserContent) > 0)
                {
                    string sql = string.Format(@"SELECT COUNT(1) FROM TB_InterestDubbingGame_UserContentsRecord WHERE UserID={0}", submitData.UserId);
                    int count = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, sql));
                    if (count > 1)
                    {
                        bl = true;
                    }
                    return ObjectToJson.GetResult(bl, "新增成功！");
                }
                else
                {
                    return ObjectToJson.GetResult("新增失败！");
                }

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) //已插入数据库
                {
                    return ObjectToJson.GetResult(bl, "新增成功！");
                }
                else
                {
                    Log4Net.LogHelper.Error(ex, "插入数据库失败，submitData=");
                    return ObjectToJson.GetErrorResult("插入数据库失败");
                }
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "错误");
                return ObjectToJson.GetErrorResult("新增失败！");
            }
        }

        /// <summary>
        /// 获取比赛报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetGameScore([FromBody] KingRequest request)
        {
            IDGGameScore submitData = JsonHelper.DecodeJson<IDGGameScore>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.ClassID))
            {
                return ObjectToJson.GetErrorResult("ClassID不能为空");
            }

            return idgbll.GetGameScore(submitData.ClassID, submitData.pageNumber); ;
        }


        /// <summary>
        /// 获取比赛报告
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetGameScore()
        {
            IDGGameScore submitData = new IDGGameScore();//JsonHelper.DecodeJson<IDGGameScore>(request.Data);
            submitData.ClassID = "74903562";
            submitData.pageNumber = 0;
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.ClassID))
            {
                return ObjectToJson.GetErrorResult("ClassID不能为空");
            }

            return idgbll.GetGameScore(submitData.ClassID, submitData.pageNumber);
        }

        /// <summary>
        /// 获取用户课本剧数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserBookDramaInfo([FromBody] KingRequest request)
        {
            IDGUserID submitData = JsonHelper.DecodeJson<IDGUserID>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            return idgbll.GetUserBookDramaInfo(Convert.ToInt32(submitData.UserID));
        }

        /// <summary>
        /// 获取用户课本剧数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserBookDramaInfoTest()
        {
            IDGUserID submitData = new IDGUserID();//JsonHelper.DecodeJson<IDGUserID>(request.Data);
            submitData.UserID = "1250625452";
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            return idgbll.GetUserBookDramaInfo(Convert.ToInt32(submitData.UserID));
        }

        /// <summary>
        /// 获取用户故事朗读数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetUserStoryReadInfo([FromBody] KingRequest request)
        {
            IDGUserID submitData = JsonHelper.DecodeJson<IDGUserID>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            return idgbll.GetUserStoryReadInfo(Convert.ToInt32(submitData.UserID));
        }

        /// <summary>
        /// 获取用户故事朗读数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetUserStoryReadInfoTest()
        {
            IDGUserID submitData = new IDGUserID();//JsonHelper.DecodeJson<IDGUserID>(request.Data);
            submitData.UserID = "420009767";
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.UserID))
            {
                return ObjectToJson.GetErrorResult("UserID不能为空");
            }

            return idgbll.GetUserStoryReadInfo(Convert.ToInt32(submitData.UserID));
        }

        /// <summary>
        /// 设置活动时间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetActiveTime([FromBody] KingRequest request)
        {
            return ObjectToJson.GetResult(Time);
        }

        [HttpPost]
        public HttpResponseMessage UserSchoolRankList([FromBody] KingsRequest request)
        {
            UserSchoolRankListSearch submitData = JsonHelper.DecodeJson<UserSchoolRankListSearch>(request.Data);


            string userInfoHashId = "TB_InterestDubbingGame_UserInfo";

            var currentUser = redis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, submitData.userId);
            if (currentUser == null)
            {
                currentUser = new TB_InterestDubbingGame_UserInfo();
                #region 从mod获取学校信息
                var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(submitData.userId));
                if (user != null) 
                {
                    if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                    {
                        currentUser.SchoolID = user.ClassSchDetailList[0].SchID;
                        currentUser.SchoolName = user.ClassSchDetailList[0].SchName;

                    }
                }
                #endregion
            }


            try
            {
                var result = new UserSchoolRankReponse();
                if (string.IsNullOrWhiteSpace(submitData.username))
                {
                    result = matchbll.GetAllUserSchoolRankList(submitData.userId, submitData.gradeRange, submitData.pageIndex, submitData.pageSize, currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "");
                }
                else
                {
                    result = matchbll.SearchUserSchoolRankList(submitData.userId, submitData.gradeRange, submitData.username, submitData.pageIndex, submitData.pageSize);
                }
                //response.Data = result;
                //response.Success = true;
                result.SchoolID = currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "0";
                result.SchoolName = currentUser.SchoolName;

                return ObjectToJson.GetResult(result);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "获取排行榜出错");

                return ObjectToJson.GetErrorResult("获取排行榜出错");
            }
        }

        [HttpPost]
        public HttpResponseMessage SearchUserSchoolRankList([FromBody] KingsRequest request)
        {

            //    KingResponse response = new KingResponse();


            try
            {
                UserSchoolRankListSearch submitData = JsonHelper.DecodeJson<UserSchoolRankListSearch>(request.Data);

                string userInfoHashId = "TB_InterestDubbingGame_UserInfo";

                var currentUser = redis.Get<TB_InterestDubbingGame_UserInfo>(userInfoHashId, submitData.userId);
                if (currentUser == null)
                {
                    currentUser = new TB_InterestDubbingGame_UserInfo();
                    #region 从mod获取学校信息
                    var user = userBLL.GetUserAllInfoByUserId(Convert.ToInt32(submitData.userId));
                    if (user != null)
                    {
                        if (user.ClassSchDetailList != null && user.ClassSchDetailList.Count > 0)
                        {
                            currentUser.SchoolID = user.ClassSchDetailList[0].SchID;
                            currentUser.SchoolName = user.ClassSchDetailList[0].SchName;

                        }
                    }
                    #endregion

                }



                var result = new UserSchoolRankReponse();
                if (string.IsNullOrWhiteSpace(submitData.username))
                {
                    result = matchbll.GetAllUserSchoolRankList(submitData.userId, submitData.gradeRange, submitData.pageIndex, submitData.pageSize, currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "");
                }
                else
                {
                    // result = matchbll.SearchUserSchoolRankList(submitData.userId, submitData.gradeRange, submitData.username, submitData.pageIndex, submitData.pageSize);
                    result = matchbll.SearchUserSchoolRankList(submitData.userId, "1", submitData.username, submitData.pageIndex, submitData.pageSize, currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "");

                    var result2 = matchbll.SearchUserSchoolRankList(submitData.userId, "2", submitData.username, submitData.pageIndex, submitData.pageSize, currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "");

                    if (result2 != null && result2.RankList != null && result2.RankList.Any())
                        result.RankList.AddRange(result2.RankList);

                    var result3 = matchbll.SearchUserSchoolRankList(submitData.userId, "3", submitData.username, submitData.pageIndex, submitData.pageSize, currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "");

                    if (result3 != null && result3.RankList != null && result3.RankList.Any())
                        result.RankList.AddRange(result3.RankList);
                }
                //response.Data = result;
                //response.Success = true;
                result.SchoolID = currentUser.SchoolID.HasValue ? currentUser.SchoolID.Value.ToString() : "0";
                result.SchoolName = currentUser.SchoolName;
                return ObjectToJson.GetResult(result);
            }
            catch (Exception ex)
            {
                Log4Net.LogHelper.Error(ex, "获取排行榜出错");

                return ObjectToJson.GetErrorResult("获取排行榜出错");
            }
        }

        private class UserSchoolRankListSearch
        {
            public string userId { get; set; }
            public string gradeRange { get; set; }
            public int pageIndex { get; set; }
            public int pageSize { get; set; }

            public string username { get; set; }
        }

        public class SchoolInfoModel
        {
            public string DistrictID { get; set; }
            public string Area { get; set; }
            public string SchoolName { get; set; }
        }
    }
}