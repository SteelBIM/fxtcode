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
using Kingsun.SynchronousStudy.Common.Base;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class ActiveVideoInfoController : ApiController
    {

        string FiedURL = WebConfigurationManager.AppSettings["FileServerUrl"];
        BaseManagement bm = new BaseManagement();
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getFilesUrl = WebConfigurationManager.AppSettings["getFiles"];

        /// <summary>
        /// 获取万圣节活动视频信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetActiveVideoInforList([FromBody]KingRequest request)
        {
            TB_VideoDetails submitData = JsonHelper.DecodeJson<TB_VideoDetails>(request.Data);
            string where = "";
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = @"SELECT [ID]
                                  ,[BookID]
                                  ,[BookName]
                                  ,[FirstTitleID]
                                  ,[FirstTitle]
                                  ,[SecondTitleID]
                                  ,[SecondTitle]
                                  ,[FirstModularID]
                                  ,[FirstModular]
                                  ,[SecondModularID]
                                  ,[SecondModular]
                                  ,[VideoTitle]
                                  ,[VideoNumber]
                                  ,[MuteVideo]
                                  ,[CompleteVideo]
                                  ,[BackgroundAudio]
                                  ,[VideoCover]
                                  ,[VideoDesc]
                                  ,[VideoDifficulty]
                                  ,[State]
                                  ,[CreateTime]
                                  ,[VideoTime]
                                  ,[VideoType]
                              FROM [FZ_InterestDubbing].[dbo].[TB_VideoDetails] WHERE VideoType='1' ORDER BY VideoNumber ASC";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
            IList<TB_VideoDetails> videolist = DataSetToIList<TB_VideoDetails>(ds, 0);
            if (videolist != null && videolist.Count > 0)
            {
                StringBuilder json = new StringBuilder();
                json.Append("[");
                for (int i = 0; i < videolist.Count; i++)
                {
                    string strSql = string.Format(@"SELECT  [ID]
                                                          ,[VideoID]
                                                          ,[BookID]
                                                          ,[FirstTitleID]
                                                          ,[SecondTitleID]
                                                          ,[FirstModularID]
                                                          ,[SecondModularID]
                                                          ,[DialogueText]
                                                          ,[DialogueNumber]
                                                          ,[StartTime]
                                                          ,[EndTime]
                                                          ,[CreateTime]
                                                          ,[ActiveID]
                                                      FROM [FZ_InterestDubbing].[dbo].[TB_VideoDialogue] WHERE ActiveID='{0}'", videolist[i].ID);
                    IList<TB_VideoDialogue> videoDialogueList = DataSetToIList<TB_VideoDialogue>(SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql), 0);

                    json.Append("{\"ID\":\"" + videolist[i].ID + "\",\"VideoNumber\":\"" + videolist[i].VideoNumber + "\",\"VideoTitle\":\"" + String2Json(StringTOJson(videolist[i].VideoTitle)) + "\",\"MuteVideo\":\""
                        + StringTOJson(videolist[i].MuteVideo) + "\",\"CompleteVideo\":\"" + StringTOJson(videolist[i].CompleteVideo) + "\",\"BackgroundAudio\":\""
                        + StringTOJson(videolist[i].BackgroundAudio) + "\",\"VideoCover\":\"" + StringTOJson(videolist[i].VideoCover) + "\",\"VideoDesc\":\""
                        + StringTOJson(videolist[i].VideoDesc) + "\",\"VideoDifficulty\":\"" + videolist[i].VideoDifficulty + "\",\"CreateTime\":\"" + videolist[i].CreateTime + "\",\"children\":[");
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
            return ObjectToJson.GetErrorResult("没有更多数据");
        }

        /// <summary>
        /// 给奖项2投票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertPrizeTwo([FromBody]KingRequest request)
        {
            TB_VideoDetails submitData = JsonHelper.DecodeJson<TB_VideoDetails>(request.Data);
            string where = "";
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = string.Format(@"UPDATE dbo.TB_UserActiveVideo SET PrizeTwo=PrizeTwo+1 WHERE id={0}", submitData.ID);

            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                string aSql = @"SELECT PrizeTwo FROM TB_UserActiveVideo WHERE ID=" + submitData.ID;
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "投票成功");
            }
            return ObjectToJson.GetErrorResult("投票失败");
        }

        /// <summary>
        /// 给奖项3投票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertPrizeThree([FromBody]KingRequest request)
        {
            TB_VideoDetails submitData = JsonHelper.DecodeJson<TB_VideoDetails>(request.Data);
            string where = "";
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = string.Format(@"UPDATE dbo.TB_UserActiveVideo SET PrizeThree=PrizeThree+1 WHERE id={0}", submitData.ID);

            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                string aSql = @"SELECT PrizeThree FROM TB_UserActiveVideo WHERE ID=" + submitData.ID;
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "投票成功");
            }
            return ObjectToJson.GetErrorResult("投票失败");
        }

        /// <summary>
        /// 给奖项4投票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertPrizeFour([FromBody]KingRequest request)
        {
            TB_VideoDetails submitData = JsonHelper.DecodeJson<TB_VideoDetails>(request.Data);
            string where = "";
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = string.Format(@"UPDATE dbo.TB_UserActiveVideo SET PrizeFour=PrizeFour+1 WHERE id={0}", submitData.ID);

            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                string aSql = @"SELECT PrizeFour FROM TB_UserActiveVideo WHERE ID=" + submitData.ID;
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "投票成功");
            }
            return ObjectToJson.GetErrorResult("投票失败");
        }

        /// <summary>
        /// 给奖项5投票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertPrizeFive([FromBody]KingRequest request)
        {
            TB_VideoDetails submitData = JsonHelper.DecodeJson<TB_VideoDetails>(request.Data);
            string where = "";
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = string.Format(@"UPDATE dbo.TB_UserActiveVideo SET PrizeFive=PrizeFive+1 WHERE id={0}", submitData.ID);

            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                string aSql = @"SELECT PrizeFive FROM TB_UserActiveVideo WHERE ID=" + submitData.ID;
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "投票成功");
            }
            return ObjectToJson.GetErrorResult("投票失败");
        }

        /// <summary>
        /// 给奖项1投票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage InsertPrizeOne([FromBody]KingRequest request)
        {
            TB_VideoDetails submitData = JsonHelper.DecodeJson<TB_VideoDetails>(request.Data);
            string where = "";
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = string.Format(@"UPDATE dbo.TB_UserActiveVideo SET PrizeOne=PrizeOne+1 WHERE id={0}", submitData.ID);

            if (Convert.ToInt32(SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql)) > 0)
            {
                string aSql = @"SELECT PrizeOne FROM TB_UserActiveVideo WHERE ID=" + submitData.ID;
                int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                string ss = "{\"state\":\"0\",\"num\":\"" + num + "\"}";
                return ObjectToJson.GetResult(js.DeserializeObject(ss), "投票成功");
            }
            return ObjectToJson.GetErrorResult("投票失败");
        }

        /// <summary>
        /// 获取万圣节资源地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetModularLatestVersion([FromBody] KingRequest request)
        {
            VersionChangeBLL versionbll = new VersionChangeBLL();
            TB_VersionChange submitData = JsonHelper.DecodeJson<TB_VersionChange>(request.Data);

            string where = "BooKID IS NULL and State=1";
            where += " order by CreateDate DESC";
            TB_VersionChange newversion = versionbll.GetNewVersionChange(where);
            if (newversion != null)
            {
                if (string.IsNullOrEmpty(newversion.MD5))
                {
                    return ObjectToJson.GetErrorResult("不存在模块资源信息");
                }
                else
                {
                    return ObjectToJson.GetResult(newversion);
                }
            }
            else
            {
                return ObjectToJson.GetErrorResult("不存在模块资源信息");
            }
        }

        /// <summary>
        /// 活动记录列表
        /// 2016-11-10(万里)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetActiveRecord([FromBody] KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            APPManagementBLL appManagementBLL = new BLL.APPManagementBLL();
            ActiveRecordInfo submitData = js.Deserialize<ActiveRecordInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.AppID) || string.IsNullOrEmpty(submitData.PageIndex.ToString()) || string.IsNullOrEmpty(submitData.PageCount.ToString()))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                int? versionID = 0;
                TB_APPManagement appInfo = appManagementBLL.GetAPPManagement(submitData.AppID);
                if (appInfo != null)
                {
                    versionID = appInfo.VersionID;
                    string sql = string.Format(@"SELECT *  FROM [TB_MessagePush] Where PushEdition='{0}' and State=1  order by CreateDate desc", versionID);
                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    List<ActiveRecordInfo> list = new List<ActiveRecordInfo>();
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (submitData.PageIndex + submitData.PageCount - 1 <= ds.Tables[0].Rows.Count)
                        {
                            for (int i = (submitData.PageIndex - 1); i < (submitData.PageIndex + submitData.PageCount - 1); i++)
                            {
                                ActiveRecordInfo recordInfo = new ActiveRecordInfo();
                                recordInfo.Title = ds.Tables[0].Rows[i]["MessageTitle"].ToString();
                                recordInfo.Content = ds.Tables[0].Rows[i]["TestDsc"].ToString();
                                recordInfo.Image = ds.Tables[0].Rows[i]["Image"].ToString();
                                recordInfo.Link = ds.Tables[0].Rows[i]["JumpLink"].ToString();
                                if (string.IsNullOrEmpty(recordInfo.Link))
                                {
                                    recordInfo.State = false;
                                }
                                else
                                {
                                    recordInfo.State = true;
                                }
                                list.Add(recordInfo);
                            }
                        }
                        else
                        {
                            for (int i = submitData.PageIndex - 1; i < ds.Tables[0].Rows.Count; i++)
                            {
                                ActiveRecordInfo recordInfo = new ActiveRecordInfo();
                                recordInfo.Title = ds.Tables[0].Rows[i]["MessageTitle"].ToString();
                                recordInfo.Content = ds.Tables[0].Rows[i]["TestDsc"].ToString();
                                recordInfo.Image = ds.Tables[0].Rows[i]["Image"].ToString();
                                recordInfo.Link = ds.Tables[0].Rows[i]["JumpLink"].ToString();
                                if (string.IsNullOrEmpty(recordInfo.Link))
                                {
                                    recordInfo.State = false;
                                }
                                else
                                {
                                    recordInfo.State = true;
                                }
                                list.Add(recordInfo);
                            }
                        }
                        if (list.Count == 0)
                        {
                            return ObjectToJson.GetResult(list, "没有更多数据");
                        }
                        else
                        {
                            return ObjectToJson.GetResult(list);
                        }
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("没有更多数据");
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("暂无相关信息");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }
/*
        /// <summary>
        /// 获取广场数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetSquareRecord([FromBody] KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            APPManagementBLL appManagementBLL = new BLL.APPManagementBLL();
            SquareInfo submitData = js.Deserialize<SquareInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.AppID) || string.IsNullOrEmpty(submitData.PageIndex.ToString()) || string.IsNullOrEmpty(submitData.PageCount.ToString()))
            {
                return ObjectToJson.GetErrorResult("当前信息有误");
            }
            try
            {
                TB_APPManagement appInfo = appManagementBLL.GetAPPManagement(submitData.AppID);
                if (appInfo != null)
                {
                    string where = " AND a.VersionID= '" + appInfo.VersionID + "' ";

                    string sql = string.Format(@"   SELECT DISTINCT  a.ID ,
                                a.CreateTime ,
                                a.UserID ,
                                a.NumberOfOraise ,
                                a.PlayTimes ,
                                a.VideoFileID ,
                                a.VersionID,
                                b.VideoTitle ,
                                c.NickName ,
                                c.TrueName ,
                                c.UserName ,
                                c.UserImage ,
                                c.IsEnableOss
                        FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_VideoDetails] b ON b.BookID = a.BookID
                                                              AND b.VideoNumber = a.VideoNumber
                                LEFT JOIN dbo.Tb_UserInfo c ON c.UserID = a.UserID
                        WHERE    c.IsUser = 1  {0} --AND b.BookID IS NOT NULL
                        ORDER BY a.ID DESC", where);

                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);

                    IList<TB_GetSquareRecord> tb = DataSetToIList<TB_GetSquareRecord>(ds, 0); ;
                    List<SquareInfo> list = new List<SquareInfo>();
                    if (tb != null && tb.Count > 0)
                    {
                        foreach (var item in tb)
                        {
                            string img = "";
                            if (submitData.IsEnableOss == 0)
                            {
                                img = item.UserImage;
                            }
                            else
                            {
                                img = item.IsEnableOss != 0 ? _getOssFilesUrl + item.UserImage : _getFilesUrl + "?FileID=" + item.UserImage;
                            }
                            list.Add(new SquareInfo
                            {
                                VideoID = item.ID,
                                NickName = item.NickName,
                                UserImage = img,
                                VideoTitle = item.VideoTitle,
                                CreateTime = item.CreateTime.ToShortDateString(),
                                NumberOfOraise = item.NumberOfOraise,
                                PlayTimes = item.PlayTimes.ToString(),
                                UserID = item.UserID,

                            });
                        }
                        list = list.Skip((submitData.PageIndex - 1) < 0 ? 0 : submitData.PageIndex - 1).Take(submitData.PageCount).ToList();
                        return ObjectToJson.GetResult(list);
                    }
                    else
                    {
                        return ObjectToJson.GetErrorResult("暂无相关信息");
                    }
                }
                else
                {
                    return ObjectToJson.GetErrorResult("暂无相关信息");
                }
            }
            catch (Exception ex)
            {
                return ObjectToJson.GetErrorResult(ex.Message);
            }
        }*/

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

        public class UserVideoInfo
        {
            public string UserId { get; set; }
            public string VideoID { get; set; }
            public string UserName { get; set; }
            public string VideoFileId { get; set; }
            public string UserImage { get; set; }
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
            //public List<Children> children { get; set; }
            public Children[] children { get; set; }
            //public string children { get; set; }
        }

        public class Children
        {
            //public int VideoId { get; set; }
            //public int UserId { get; set; }
            //public int UserVideoId { get; set; }
            public int ID { get; set; }
            public int DialogueNumber { get; set; }
            public int DialogueScore { get; set; }
        }

        /// <summary>
        /// 活动记录信息
        /// </summary>
        public class ActiveRecordInfo
        {
            public string AppID { get; set; } //版本ID
            public int PageCount { get; set; }
            public int PageIndex { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Image { get; set; }
            public string Link { get; set; }
            public bool State { get; set; }
        }

        /// <summary>
        /// 活动记录信息
        /// </summary>
        public class SquareInfo
        {
            public string AppID { get; set; } //版本ID
            public int VideoID { get; set; } //视频ID         
            public int PageCount { get; set; }
            public int PageIndex { get; set; }
            public string NickName { get; set; }
            public string UserImage { get; set; }
            public string VideoTitle { get; set; }
            public string CreateTime { get; set; }
            public int NumberOfOraise { get; set; }
            public string PlayTimes { get; set; }
            public int IsEnableOss { get; set; }
            public int ID { get; set; }
            public int UserID { get; set; }
        }
    }
}