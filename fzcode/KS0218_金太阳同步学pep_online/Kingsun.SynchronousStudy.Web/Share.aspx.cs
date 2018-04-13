using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.BLL;

namespace Kingsun.SynchronousStudy.Web
{
    public partial class Share : System.Web.UI.Page
    {
        IIBSData_UserInfoBLL userBLL = new IBSData_UserInfoBLL();
        string Action = "";
        //  string videoIDes = System.Configuration.ConfigurationManager.AppSettings["FileServerUrl"];
        private readonly string _getOssFilesUrl = WebConfigurationManager.AppSettings["getOssFiles"];
        private readonly string _getVideoFiles = WebConfigurationManager.AppSettings["getVideoFiles"];
        private readonly string _getVideoFilesHttps = WebConfigurationManager.AppSettings["getVideoFilesHttps"];
        private readonly string _getOssFilesHttps = WebConfigurationManager.AppSettings["getOssFilesHttps"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        private void DataBind()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
            {
                Action = Request.QueryString["Action"].Trim().ToLower();//去掉空格并变小写 
            }
            else
            {
                return;
            }
            switch (Action)
            {
                case "getuser":
                    string userID = Request.Form["userID"];
                    var user = userBLL.Search(a => a.UserID == Convert.ToInt32(userID));
                    if (user != null)
                    {
                        Response.Write(JsonHelper.EncodeJson(user));
                        Response.End();
                    }
                    else
                    {
                        Response.Write(JsonHelper.EncodeJson(""));
                        Response.End();
                    }
                    break;
                case "getvideo":
                    string IsYX = "0";
                    string videoid = Request.Form["VideoFileID"];
                    string IsEnableOss = Request.Form["IsEnableOss"];
                    string UserID = Request.Form["UserID"];
                    IsYX = Request.Form["fromModule"].ToLower();//默认为0，0为同步学，YouXue为优学
                    string videoFileID = "";
                    if (IsEnableOss == "1")
                    {
                        if (videoid.Trim().Contains(_getOssFilesUrl))
                        {
                            videoFileID = videoid.Trim().Replace(_getOssFilesUrl, "");
                        }
                        else if (videoid.Trim().Contains(_getVideoFiles))
                        {
                            string[] vi = videoid.Trim().Replace(_getVideoFiles, "").Split('/');
                            videoFileID = vi[vi.Length - 1].Split('.')[0];
                        }
                        else if (videoid.Trim().Contains(_getVideoFilesHttps))
                        {
                            string[] vi = videoid.Trim().Replace(_getVideoFilesHttps, "").Split('/');
                            videoFileID = vi[vi.Length - 1].Split('.')[0];
                        }
                        else if (videoid.Trim().Contains(_getOssFilesHttps))
                        {
                            string[] vi = videoid.Trim().Replace(_getOssFilesHttps, "").Split('/');
                            videoFileID = vi[vi.Length - 1].Split('.')[0];
                        }
                        else
                        {
                            videoFileID = videoid;
                        }
                    }
                    else
                    {
                        videoFileID = videoid;
                    }

                    VideoDetailsBLL videobll = new VideoDetailsBLL();
                    string where = "";
                    if (IsYX == "youxue")
                    {
                        where = string.Format(@"SELECT  a.VideoFileID ,
                                                            NumberOfOraise ,
                                                            TotalScore ,
                                                            VideoReleaseAddress ,
                                                            DialogueNumber ,
                                                            DialogueScore ,
                                                            a.CreateTime,
                                                            a.IsEnableOss
                                                    FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails_YX] a
                                                            LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue_YX] b ON a.ID = b.UserVideoID where a.UserID='" + UserID + "' AND a.VideoFileID like '%" + videoFileID + "%'");
                    }
                    else
                    {
                        where = string.Format(@"SELECT  a.VideoFileID ,
                                                            NumberOfOraise ,
                                                            TotalScore ,
                                                            VideoReleaseAddress ,
                                                            DialogueNumber ,
                                                            DialogueScore ,
                                                            a.CreateTime,
                                                            a.IsEnableOss
                                                    FROM    [FZ_InterestDubbing].[dbo].[TB_UserVideoDetails] a
                                                            LEFT JOIN [FZ_InterestDubbing].[dbo].[TB_UserVideoDialogue] b ON a.ID = b.UserVideoID where a.UserID='" + UserID + "' AND a.VideoFileID like '%" + videoFileID + "%'");
                    }
                    List<UserVideoDetails> listVideo = videobll.GetUserVideoDetailsByWhere(where);
                    if (listVideo != null)
                    {
                        if (listVideo.Count > 0)
                        {

                            int perfectnum = 0;
                            int execlentnum = 0;
                            int greetnum = 0;
                            int goodnum = 0;
                            foreach (UserVideoDetails uservideo in listVideo)
                            {
                                if (uservideo.DialogueScore >= 90)
                                {
                                    perfectnum++;
                                }
                                else if (uservideo.DialogueScore >= 80)
                                {
                                    execlentnum++;
                                }
                                else if (uservideo.DialogueScore >= 60)
                                {
                                    greetnum++;
                                }
                                else
                                {
                                    goodnum++;
                                }
                            }

                            foreach (UserVideoDetails uservideos in listVideo)
                            {

                                string fiales =
                                    Convert.ToDateTime(uservideos.CreateTime).ToString("yyyy/MM/dd").Replace('-', '/');
                                //string url = "http://video.kingsun.cn/2016/08/31/91ca3717-cc48-4006-99de-143669e77a2b.mp4";
                                string url = "";
                                if (uservideos.IsEnableOss == 0)
                                {
                                    url = "http://video.kingsun.cn/" + fiales + "/" + uservideos.VideoFileID + ".mp4";
                                }
                                else
                                {
                                    url = _getOssFilesUrl + uservideos.VideoFileID;
                                }

                                object objs = new
                                {
                                    VideoFileID = uservideos.VideoFileID,
                                    TotalScore = uservideos.TotalScore,
                                    UserVideoID = url,
                                    NumberOfOraise = uservideos.NumberOfOraise,
                                    perfectnum = perfectnum,
                                    execlentnum = execlentnum,
                                    greetnum = greetnum,
                                    goodnum = goodnum,
                                    creatTime = uservideos.CreateTime
                                };

                                Response.Write(JsonHelper.EncodeJson(objs));
                                Response.End();
                            }
                        }
                        else
                        {
                            Response.Write(JsonHelper.EncodeJson("数据不存在"));
                            Response.End();
                        }
                    }
                    break;
                default:
                    Response.Write("");
                    Response.End();
                    break;
            }
        }
    }
}