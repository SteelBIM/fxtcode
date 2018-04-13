using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Newtonsoft.Json;

namespace Kingsun.SynchronousStudy.Web.GeneralTools
{
    public partial class UpdateVideoInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["action"]))
            {
                InitAction(Request.QueryString["action"].ToLower());
            }
        }
        private void InitAction(string action)
        {
            VideoDetailsBLL videoDetailsBLL = new VideoDetailsBLL();
            switch (action)
            {
                case "getvideoinfo":
                    string where = "";
                    string videoID = Request.Form["VideoID"];
                    where += "1=1 and ID = " + "'" + videoID + "'";
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
                                                    FROM    [FZ_InterestDubbing].[dbo].[TB_VideoDetails] a LEFT JOIN dbo.TB_ModuleConfiguration b ON b.BookID = a.BookID  AND a.FirstTitleID=b.FirstTitileID AND  ((b.SecondTitleID IS NULL AND a.SecondTitleID IS NULL) OR  a.SecondTitleID = b.SecondTitleID) WHERE {0}", where);

                    DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, sql);
                    IList<TB_VideoDetails> videolist = DataSetToIList<TB_VideoDetails>(ds, 0);

                    //IList<TB_VideoDetails> videolist = videoDetailsBLL.GetVideoList(where);
                    TB_VideoDetails videoInfo = new TB_VideoDetails();
                    IList<TB_VideoDialogue> videoDialogueList = new List<TB_VideoDialogue>();
                    if (videolist != null && videolist.Count > 0)
                    {
                        string queryStr = "";
                        videoInfo = videolist[0];
                        queryStr += "BookID = " + videoInfo.BookID;
                        queryStr += " and VideoID = " + videoInfo.VideoNumber;
                        videoDialogueList = videoDetailsBLL.GetVideoDialogueList(queryStr);
                    }
                    var obj = new { obj1 = videoInfo, obj2 = videoDialogueList };
                    Response.Write(JsonHelper.EncodeJson(obj));
                    Response.End();
                    break;
                case "updatevideoinfo":
                    if (string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        where = "1=1";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                    }
                    videoInfo = JsonConvert.DeserializeObject<TB_VideoDetails>(where);
                    bool result = videoDetailsBLL.UpdateVideoInfo(videoInfo);
                    Response.Write(JsonHelper.EncodeJson(result));
                    Response.End();
                    break;
                case "updatevideodialogue":
                    if (string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                    {
                        where = "1=1";
                    }
                    else
                    {
                        where = Request.QueryString["queryStr"].ToString();
                    }
                    videoDialogueList = JsonConvert.DeserializeObject<IList<TB_VideoDialogue>>(where);
                    result = true;
                    if (videoDialogueList != null && videoDialogueList.Count > 0)
                    {
                        bool consequence = true;
                        for (int i = 0; i < videoDialogueList.Count; i++)
                        {
                            consequence = videoDetailsBLL.UpdateVideoDialogue(videoDialogueList[i]);
                            if (!consequence)
                            {
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                    Response.Write(JsonHelper.EncodeJson(result));
                    Response.End();
                    break;
                default:
                    break;
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
    }
}