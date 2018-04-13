using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Kingsun.SynchronousStudy.App.Common;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using System.Data;
using System.Reflection;
using System.Web.Script.Serialization;
using log4net;
using System.Web.Configuration;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.App.Controllers
{
    public class MessagePushController : ApiController
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        private static IIBSData_ClassUserRelationBLL classBLL = new IBSData_ClassUserRelationBLL();

        /// <summary>
        /// 统计打开推送消息的次数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddOpenMessageNumber([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            TB_MessagePush submitData = JsonHelper.DecodeJson<TB_MessagePush>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            try
            {
                string sql = string.Format(@"  UPDATE dbo.TB_MessagePush SET OpenNumber=OpenNumber+1 WHERE id={0}", submitData.ID);
                if (SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql) > 0)
                {
                    string aSql = @"SELECT OpenNumber FROM TB_MessagePush WHERE ID=" + submitData.ID;
                    int num = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, aSql));
                    string ss = "{\"state\":\"1\"}";
                    return ObjectToJson.GetResult(js.DeserializeObject(ss), "更新成功");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("更新失败");
        }

        /// <summary>
        /// 获取推送信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage MessageList([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string sql = string.Format(@"SELECT TOP 1  [ID]
                      ,[MessageTitle]
                      ,[TitleState]
                      ,[Image]
                      ,[ButtonImage]
                      ,[PushEdition]
                      ,[JumpLink]
                      ,[UseTime]
                      ,[Number]
                      ,[ClassID]
                      ,[TestDsc]
                      ,[StartTime]
                      ,[EndTime]
                      ,[CreateDate]
                      ,[OpenNumber]
                      ,[State]
                  FROM [TB_MessagePush] WHERE State=1 ORDER BY CreateDate DESC");
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                IList<TB_MessagePush> tm = DataSetToIList<TB_MessagePush>(ds, 0);
                if (tm != null && tm.Count > 0)
                {
                    string queryStr = "";
                    StringBuilder json = new StringBuilder();
                    json.Append("[");
                    for (int i = 0; i < tm.Count; i++)
                    {
                        json.Append("{\"ID\":\"" + tm[i].ID + "\",\"MessageTitle\":\"" + tm[i].MessageTitle + "\",\"TitleState\":\"" + tm[i].TitleState + "\",\"Image\":\"" + Image(tm[i].Image) + "\",\"ButtonImage\":\"" +
                                      Image(tm[i].ButtonImage) + "\",\"PushEdition\":\"" + tm[i].PushEdition + "\",\"JumpLink\":\"" + tm[i].JumpLink + "\",\"UseTime\":\""
                                     + tm[i].UseTime + "\",\"Number\":\"" + tm[i].Number + "\",\"ClassID\":\"" + tm[i].ClassID + "\",\"TestDsc\":\"" +
                                     String2Json(StringTOJson(tm[i].TestDsc)) + "\",\"StartTime\":\"" + tm[i].StartTime + "\",\"EndTime\":\"" + tm[i].EndTime + "\",\"CreateDate\":\""
                                     + tm[i].CreateDate + "\",\"OpenNumber\":\"" + tm[i].OpenNumber + "\"");
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
            return ObjectToJson.GetErrorResult("获取失败");
        }

        [HttpGet]
        public HttpResponseMessage MessageListTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            string sql = string.Format(@"SELECT  [ID]
                      ,[MessageTitle]
                      ,[TitleState]
                      ,[Image]
                      ,[ButtonImage]
                      ,[PushEdition]
                      ,[JumpLink]
                      ,[UseTime]
                      ,[Number]
                      ,[ClassID]
                      ,[TestDsc]
                      ,[StartTime]
                      ,[EndTime]
                      ,[CreateDate]
                      ,[OpenNumber]
                      ,[State]
                  FROM [TB_MessagePush] WHERE State=1 ORDER BY CreateDate DESC");
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                IList<TB_MessagePush> tm = DataSetToIList<TB_MessagePush>(ds, 0);
                if (tm != null && tm.Count > 0)
                {
                    string queryStr = "";
                    StringBuilder json = new StringBuilder();
                    json.Append("[");
                    for (int i = 0; i < tm.Count; i++)
                    {
                        json.Append("{\"ID\":\"" + tm[i].ID + "\",\"MessageTitle\":\"" + tm[i].MessageTitle + "\",\"TitleState\":\"" + tm[i].TitleState + "\",\"Image\":\"" + Image(tm[i].Image) + "\",\"ButtonImage\":\"" +
                                     Image(tm[i].ButtonImage) + "\",\"PushEdition\":\"" + tm[i].PushEdition + "\",\"JumpLink\":\"" + tm[i].JumpLink + "\",\"UseTime\":\""
                                    + tm[i].UseTime + "\",\"Number\":\"" + tm[i].Number + "\",\"ClassID\":\"" + tm[i].ClassID + "\",\"TestDsc\":\"" +
                                    String2Json(StringTOJson(tm[i].TestDsc)) + "\",\"StartTime\":\"" + tm[i].StartTime + "\",\"EndTime\":\"" + tm[i].EndTime + "\",\"CreateDate\":\""
                                    + tm[i].CreateDate + "\",\"OpenNumber\":\"" + tm[i].OpenNumber + "\"");
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
            return ObjectToJson.GetErrorResult("获取失败");
        }

        /// <summary>
        /// 获取最新推送消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetNewMessage([FromBody]KingRequest request)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            MessageInfo submitData = JsonHelper.DecodeJson<MessageInfo>(request.Data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            if (string.IsNullOrEmpty(submitData.ID))
            {
                return ObjectToJson.GetErrorResult("AppID不能为空");
            }
            string classId = "0";
            string usetime = "0";
            string number = "0";
            DataSet ds = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(submitData.UserID))
                {
                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
                    if (user != null) 
                    {
                        if (user.ClassSchList.Count > 0) 
                        {
                            classId = user.ClassSchList[0].ClassID;
                        }
                    }
                }

                string strSql = string.Format(@"SELECT AppID,UseTime,Number FROM TB_UserStatistics WHERE UserID='{0}' AND AppID='{1}'", submitData.UserID, submitData.ID);
                DataSet dsUser = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);

                string where = " 1=1 AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                if (!string.IsNullOrEmpty(submitData.ID))
                {
                    where += " AND b.ID='" + submitData.ID + "'";
                }
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    if (dsUser.Tables[0].Rows[0]["UseTime"].ToString().Trim() != "")
                    {
                        usetime = dsUser.Tables[0].Rows[0]["UseTime"].ToString().Trim();
                    }
                    if (dsUser.Tables[0].Rows[0]["Number"].ToString().Trim() != "")
                    {
                        number = dsUser.Tables[0].Rows[0]["Number"].ToString().Trim();
                    }
                }
                string sql = "";
                int count = 0;
                //if (string.IsNullOrEmpty(submitData.UserID))
                //{
                //    where = "1=1 And  a.ClassID='0' And a.UseTime='0' And a.Number='0' AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                //    if (!string.IsNullOrEmpty(submitData.ID))
                //    {
                //        where += " AND b.ID='" + submitData.ID + "'";
                //    }
                //    sql = stringSql(where, sql);
                //    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                //    count = ds.Tables[0].Rows.Count;
                //}
                if (classId != "0" && ParseInt(usetime) >= 0 && ParseInt(number) >= 0)
                {
                    where += " And  a.ClassID='" + classId + "' And a.UseTime<='" + usetime + "' And a.Number<='" + number + "'";
                    sql = stringSql(where, sql);
                    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    count = ds.Tables[0].Rows.Count;
                }
                if (count <= 0)
                {
                    if (ParseInt(usetime) >= 0 && ParseInt(number) >= 0)
                    {
                        where += "  And a.UseTime<='" + usetime + "' And a.Number<='" + number + "' AND a.ClassID<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (classId != "0" && ParseInt(usetime) >= 0)
                    {
                        where += " And  a.ClassID='" + classId + "' And a.UseTime<='" + usetime + "' AND a.Number<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (classId != "0" && ParseInt(number) >= 0)
                    {
                        where += " And  a.ClassID='" + classId + "' And a.Number<='" + number + "' AND a.UseTime<=0";
                        sql = stringSql(where, sql);//Sql语句
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (classId != "0")
                    {
                        where += " And  a.ClassID='" + classId + "' AND a.Number<=0 AND a.UseTime<=0 ";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (ParseInt(usetime) >= 0)
                    {
                        where += "  And a.UseTime<='" + usetime + "' AND a.ClassID<=0 AND a.Number<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (ParseInt(number) >= 0)
                    {
                        where += "  And a.Number<='" + number + "' AND a.ClassID<=0 AND a.Number<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    //where = " 1=1 AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                    //if (!string.IsNullOrEmpty(submitData.ID))
                    //{
                    //    where += " AND b.ID='" + submitData.ID + "'";
                    //}
                    //if (!string.IsNullOrEmpty(submitData.UserID))
                    //{
                    //    sql = stringSql(where, sql);
                    //    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    //    count = ds.Tables[0].Rows.Count;
                    //}
                    where = "1=1 And  a.ClassID='0' And a.UseTime='0' And a.Number='0' AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                    if (!string.IsNullOrEmpty(submitData.ID))
                    {
                        where += " AND b.ID='" + submitData.ID + "'";
                    }
                    sql = stringSql(where, sql);
                    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    count = ds.Tables[0].Rows.Count;
                }

                IList<TB_MessagePush> tm = DataSetToIList<TB_MessagePush>(ds, 0);
                if (tm != null && tm.Count > 0)
                {
                    string queryStr = "";
                    StringBuilder json = new StringBuilder();
                    json.Append("[");
                    for (int i = 0; i < tm.Count; i++)
                    {
                        json.Append("{\"ID\":\"" + tm[i].ID + "\",\"MessageTitle\":\"" + tm[i].MessageTitle + "\",\"TitleState\":\"" + tm[i].TitleState + "\",\"Image\":\"" + Image(tm[i].Image) + "\",\"ButtonImage\":\"" +
                                      Image(tm[i].ButtonImage) + "\",\"PushEdition\":\"" + tm[i].PushEdition + "\",\"JumpLink\":\"" + tm[i].JumpLink + "\",\"UseTime\":\""
                                     + tm[i].UseTime + "\",\"Number\":\"" + tm[i].Number + "\",\"ClassID\":\"" + tm[i].ClassID + "\",\"TestDsc\":\"" +
                                     String2Json(StringTOJson(tm[i].TestDsc)) + "\",\"StartTime\":\"" + tm[i].StartTime + "\",\"EndTime\":\"" + tm[i].EndTime + "\",\"CreateDate\":\""
                                     + tm[i].CreateDate + "\",\"OpenNumber\":\"" + tm[i].OpenNumber + "\"");
                        json.Append("},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
                return ObjectToJson.GetResult("", "数据不存在！");
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("获取失败");
        }

        [HttpGet]
        public HttpResponseMessage GetNewMessageTest()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            MessageInfo submitData = new MessageInfo();//JsonHelper.DecodeJson<MessageInfo>(request.data);
            if (submitData == null)
            {
                return ObjectToJson.GetErrorResult("当前信息为空");
            }
            //if (string.IsNullOrEmpty(submitData.ID))
            //{
            //    return ObjectToJson.GetErrorResult("AppID不能为空");
            //}
            submitData.UserID = "1543932852";
            submitData.ID = "1548d0a3-ca8e-4702-9c2c-f0ba0cacd385";
            string classId = "0";
            string usetime = "0";
            string number = "0";
            DataSet ds = new DataSet();
            try
            {
                if (!string.IsNullOrEmpty(submitData.UserID))
                {
                    var user = userBLL.GetUserInfoByUserId(Convert.ToInt32(submitData.UserID));
                    if (user != null)
                    {
                        if (user.ClassSchList.Count > 0)
                        {
                            classId = user.ClassSchList[0].ClassID;
                        }
                    }
                }

                string strSql = string.Format(@"SELECT AppID,UseTime,Number FROM TB_UserStatistics WHERE UserID='{0}' AND AppID='{1}'", submitData.UserID, submitData.ID);
                DataSet dsUser = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, strSql);

                string where = " 1=1 AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                if (!string.IsNullOrEmpty(submitData.ID))
                {
                    where += " AND b.ID='" + submitData.ID + "'";
                }
                if (dsUser.Tables[0].Rows.Count > 0)

                {
                    if (dsUser.Tables[0].Rows[0]["UseTime"].ToString().Trim() != "")
                    {
                        usetime = dsUser.Tables[0].Rows[0]["UseTime"].ToString().Trim();
                    }
                    if (dsUser.Tables[0].Rows[0]["Number"].ToString().Trim() != "")
                    {
                        number = dsUser.Tables[0].Rows[0]["Number"].ToString().Trim();
                    }
                }
                string sql = "";
                int count = 0;
                //if (string.IsNullOrEmpty(submitData.UserID))
                //{
                //    where = "1=1 And  a.ClassID='0' And a.UseTime='0' And a.Number='0' AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                //    if (!string.IsNullOrEmpty(submitData.ID))
                //    {
                //        where += " AND b.ID='" + submitData.ID + "'";
                //    }
                //    sql = stringSql(where, sql);
                //    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                //    count = ds.Tables[0].Rows.Count;
                //}
                if (classId != "0" && ParseInt(usetime) >= 0 && ParseInt(number) >= 0)
                {
                    where += " And  a.ClassID='" + classId + "' And a.UseTime<='" + usetime + "' And a.Number<='" + number + "'";
                    sql = stringSql(where, sql);
                    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    count = ds.Tables[0].Rows.Count;
                }
                if (count <= 0)
                {
                    if (ParseInt(usetime) >= 0 && ParseInt(number) >= 0)
                    {
                        where += "  And a.UseTime<='" + usetime + "' And a.Number<='" + number + "' AND a.ClassID<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (classId != "0" && ParseInt(usetime) >= 0)
                    {
                        where += " And  a.ClassID='" + classId + "' And a.UseTime<='" + usetime + "' AND a.Number<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (classId != "0" && ParseInt(number) >= 0)
                    {
                        where += " And  a.ClassID='" + classId + "' And a.Number<='" + number + "' AND a.UseTime<=0";
                        sql = stringSql(where, sql);//Sql语句
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (classId != "0")
                    {
                        where += " And  a.ClassID='" + classId + "' AND a.Number<=0 AND a.UseTime<=0 ";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (ParseInt(usetime) >= 0)
                    {
                        where += "  And a.UseTime<='" + usetime + "' AND a.ClassID<=0 AND a.Number<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    if (ParseInt(number) >= 0)
                    {
                        where += "  And a.Number<='" + number + "' AND a.ClassID<=0 AND a.Number<=0";
                        sql = stringSql(where, sql);
                        ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                        count = ds.Tables[0].Rows.Count;
                    }
                }
                if (count <= 0)
                {
                    //where = " 1=1 AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                    //if (!string.IsNullOrEmpty(submitData.ID))
                    //{
                    //    where += " AND b.ID='" + submitData.ID + "'";
                    //}
                    //if (!string.IsNullOrEmpty(submitData.UserID))
                    //{
                    //    sql = stringSql(where, sql);
                    //    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    //    count = ds.Tables[0].Rows.Count;
                    //}
                    where = "1=1 And  a.ClassID='0' And a.UseTime='0' And a.Number='0' AND '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' BETWEEN a.StartTime AND a.EndTime";
                    if (!string.IsNullOrEmpty(submitData.ID))
                    {
                        where += " AND b.ID='" + submitData.ID + "'";
                    }
                    sql = stringSql(where, sql);
                    ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    count = ds.Tables[0].Rows.Count;
                }

                IList<TB_MessagePush> tm = DataSetToIList<TB_MessagePush>(ds, 0);
                if (tm != null && tm.Count > 0)
                {
                    string queryStr = "";
                    StringBuilder json = new StringBuilder();
                    json.Append("[");
                    for (int i = 0; i < tm.Count; i++)
                    {
                        json.Append("{\"ID\":\"" + tm[i].ID + "\",\"MessageTitle\":\"" + tm[i].MessageTitle + "\",\"TitleState\":\"" + tm[i].TitleState + "\",\"Image\":\"" + Image(tm[i].Image) + "\",\"ButtonImage\":\"" +
                                      Image(tm[i].ButtonImage) + "\",\"PushEdition\":\"" + tm[i].PushEdition + "\",\"JumpLink\":\"" + tm[i].JumpLink + "\",\"UseTime\":\""
                                     + tm[i].UseTime + "\",\"Number\":\"" + tm[i].Number + "\",\"ClassID\":\"" + tm[i].ClassID + "\",\"TestDsc\":\"" +
                                     String2Json(StringTOJson(tm[i].TestDsc)) + "\",\"StartTime\":\"" + tm[i].StartTime + "\",\"EndTime\":\"" + tm[i].EndTime + "\",\"CreateDate\":\""
                                     + tm[i].CreateDate + "\",\"OpenNumber\":\"" + tm[i].OpenNumber + "\"");
                        json.Append("},");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("]");
                    return ObjectToJson.GetResult(js.DeserializeObject(json.ToString()));
                }
                return ObjectToJson.GetResult("", "数据不存在！");
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return ObjectToJson.GetErrorResult("获取失败");
        }

        private static string stringSql(string where, string sql)
        {
            sql = string.Format(@"SELECT TOP 1  a.[ID] ,
                                                a.[MessageTitle] ,
                                                a.[TitleState] ,
                                                a.[Image] ,
                                                a.[ButtonImage] ,
                                                a.[PushEdition] ,
                                                a.[JumpLink] ,
                                                a.[UseTime] ,
                                                a.[Number] ,
                                                a.[ClassID] ,
                                                a.[TestDsc] ,
                                                a.[StartTime] ,
                                                a.[EndTime] ,
                                                a.[CreateDate] ,
                                                a.[OpenNumber] ,
                                                a.[State]
                                        FROM    [TB_MessagePush] a
                                                LEFT JOIN dbo.TB_APPManagement b ON a.PushEdition = b.VersionID
                                        WHERE   {0}  AND a.State=1
                                        ORDER BY a.CreateDate DESC ;", where);
            ///删除了TB_UserClassRelation表关联 需测试是否报错
            return sql;
        }


        private static string Image(string s)
        {
            string files = WebConfigurationManager.AppSettings["getFiles"];
            string image = "";
            if (!string.IsNullOrEmpty(s))
            {
                image = files + "?FileID=" + s;
            }
            return image;
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

        public class MessageInfo
        {
            public string ID { get; set; }
            public string UserID { get; set; }
        }
    }

}
