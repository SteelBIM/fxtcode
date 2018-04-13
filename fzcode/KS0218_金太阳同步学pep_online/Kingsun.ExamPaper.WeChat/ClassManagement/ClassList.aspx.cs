using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.ExamPaper.Common;
using Kingsun.ExamPaper.Model;
using Kingsun.ExamPaper.Wechat.api;
using Kingsun.ExamPaper.Wechat.Models;
using Kingsun.ExamPaper.WeChat.RelationService;

namespace Kingsun.ExamPaper.WeChat.ClassManagement
{
    public partial class ClassList : System.Web.UI.Page
    {
        public string Pageurl = "";
        public string WXappid = "";
        public string Timestamp = "";
        public string NonceStr = "";
        public string Signature = "";
        public string TrueName = "";
        public string Desc = "";
        public string Link = "";
        public string ImgUrl = "";
        public string Url = "";
        public string token = "";
        public string ticket = "";
        readonly string _url = System.Configuration.ConfigurationManager.AppSettings["WeChatAddress"];

        readonly BasicApi bapi = new BasicApi();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string userid = Request.QueryString["UserID"];
          
                TrueName = "加入班级";
                Desc = SetClassTitle(userid);
                Link = "http://" + _url + "/ExWechat/ClassManagement/joinClass.aspx?UserID=" + userid;
                RegisterWeiXinShareScript();
            }
        }

        private void RegisterWeiXinShareScript()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Pageurl = Request.Url.AbsoluteUri;
            WeChatApiClass wc = js.Deserialize<WeChatApiClass>(bapi.GetWxJsApi(Pageurl));

            if (wc != null)
            {
                WXappid = wc.appId;
                Timestamp = wc.timestamp;
                NonceStr = wc.nonceStr;
                Signature = wc.signature;
                //l1.Text = "【URL:" + wc.pageurl + "】【Token:" + wc.token + "】【Ticket:" + wc.ticket + "】";
            }
        }

        /// <summary>
        /// 分享提示语
        /// </summary>
        /// <param name="name"></param>
        /// <param name="versionName"></param>
        /// <returns></returns>
        public string SetClassTitle(string userID)
        {
            RelationService.RelationService relationService = new RelationService.RelationService();
            ReturnInfo result = relationService.GetUserInfoByUserID(userID);
            TB_UUMSUser userInfo = new TB_UUMSUser();
            string VName = "";
            if (result.Success)
            {
                userInfo = JsonHelper.DecodeJson<TB_UUMSUser>(result.Data.ToString());
            }

            return "我是英语老师" + userInfo.TrueName + "，请在金太阳教育软件中加入我们的班级，一起学习吧！";
        }
    }
}