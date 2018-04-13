using Kingsun.InterestDubbingGame.BLL;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kingsun.SynchronousStudy.Web.InterestDubbingGame
{
    public partial class PushList : System.Web.UI.Page
    {
        TB_InterestDubbingGame_PushMsgBLL bll = new TB_InterestDubbingGame_PushMsgBLL();
        string Action = "";
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActionInit();
            }
        }
        private void ActionInit()
        {
            try
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
                    case "query":
                        query();
                        break;
                    case "updatestate":
                        UpdateState();
                        break;
                    case "delstate":
                        DelState();
                        break;
                    case "del":
                        Del();
                        break;
                    case "add":
                        Add();
                        break;
                    case "getmodel":
                        GetModel();
                        break;
                    case "update":
                        Update();
                        break;
                    case "gettb_appmanagement":
                        GetTB_APPManagement();
                        break;
                    case "gettb_applicationversionbyversionid":
                        GetTB_ApplicationVersionByVersionID();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            Response.End();
        }
        /// <summary>
        /// 获取TB_ApplicationVersion
        /// </summary>
        public void GetTB_ApplicationVersionByVersionID()
        {
            try
            {
                string VersionID = Request["VersionID"];
                DataSet set = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, "select *from dbo.TB_ApplicationVersion where VersionID="+VersionID+" and State=1");
                IList<TB_ApplicationVersion> list = JsonHelper.DataSetToIList<TB_ApplicationVersion>(set, 0);
                string json = JsonHelper.EncodeJson(list);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("");
            }
        }
        /// <summary>
        /// 获取TB_APPManagement
        /// </summary>
        public void GetTB_APPManagement()
        {
            try
            {
                DataSet set = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString, CommandType.Text, " select *from dbo.TB_APPManagement");
                IList<TB_APPManagement> list = JsonHelper.DataSetToIList<TB_APPManagement>(set, 0);
                string json = JsonHelper.EncodeJson(list);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("");
            }
        }
        /// <summary>
        /// 物理删除
        /// </summary>
        public void DelState()
        {
            try
            {
                int ID = Convert.ToInt32(Request["ID"] == null ? "0" : Request["ID"].ToString());
                int count = SqlHelper.ExecuteNonQuery(SqlHelper.InterestDubbingGameConnectionStr, CommandType.Text, "update TB_InterestDubbingGame_PushMsg set DelState=0 where ID='" + ID + "'");
                if (count > 0)
                {
                    Response.Write("{'state':'1','msg':'删除成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'删除失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'删除失败'}");
            }
        }
        public void Update()
        {
            try
            {
                int ID = Convert.ToInt32(Request["ID"] == null ? "0" : Request["ID"].ToString());
                int VersionID = Convert.ToInt32(Request["VersionID"] == null ? "0" : Request["VersionID"].ToString());
                string VersionName = Request["VersionName"];
                string IdentityType = Request["IdentityType"];
                string VersionNumber = Request["VersionNumber"];
                DateTime PushTime = Convert.ToDateTime(Request["PushTime"] == null ? DateTime.Now : DateTime.Parse(Request["PushTime"].ToString()));
                string Jump = Request["Jump"];
                string Content = Request["Content"];
                TB_InterestDubbingGame_PushMsg model = new TB_InterestDubbingGame_PushMsg()
                {
                    ID = ID,
                    VersionID = VersionID,
                    VersionNumber = VersionNumber,
                    VersionName = VersionName,
                    IdentityType = IdentityType,
                    Jump = Jump,
                    Content = Content,
                    State = 1,
                    DelState = 1,
                    PushTime = PushTime,
                    CreateTime = DateTime.Now//.ToString("yyyy-MM-dd HH:mm:ss")
                };
                bool flag = bll.Update(model);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'修改成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'修改失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'修改失败'}");
            }
        }
        public void GetModel()
        {
            try
            {
                string ID = Request["ID"];
                IList<TB_InterestDubbingGame_PushMsg> courseList = bll.GetList("ID=" + ID);
                string json = JsonHelper.EncodeJson(courseList);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'获取数据失败'}");
            }
        }
        public void Add()
        {
            try
            {
                int VersionID = Convert.ToInt32(Request["VersionID"] == null ? "0" : Request["VersionID"].ToString());
                string VersionName = Request["VersionName"];
                string IdentityType = Request["IdentityType"];
                string VersionNumber = Request["VersionNumber"];
                DateTime PushTime = Convert.ToDateTime(Request["PushTime"] == null ? DateTime.Now : DateTime.Parse(Request["PushTime"].ToString()));
                string Jump = Request["Jump"];
                string Content = Request["Content"];
                TB_InterestDubbingGame_PushMsg model = new TB_InterestDubbingGame_PushMsg()
                {
                    VersionID = VersionID,
                    VersionNumber = VersionNumber,
                    VersionName = VersionName,
                    IdentityType = IdentityType,
                    Jump = Jump,
                    Content = Content,
                    State = 0,
                    DelState = 1,
                    PushTime = PushTime
                };
                bool flag = bll.Add(model);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'新增成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'新增失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'新增失败'}");
            }
        }
        /// <summary>
        /// 根据ID 删除
        /// </summary>
        public void Del()
        {
            try
            {
                string ID = Request["ID"];
                bool flag = bll.Del(ID);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'删除成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'删除失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'删除失败'}");
            }
        }
        /// <summary>
        /// 根据ID修改状态
        /// </summary>
        public void UpdateState()
        {
            try
            {
                string ID = Request["ID"];
                string State = Request["State"];
                bool flag = bll.UpdateState(ID, State);
                if (flag)
                {
                    Response.Write("{'state':'1','msg':'修改成功'}");
                }
                else
                {
                    Response.Write("{'state':'0','msg':'修改失败'}");
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("{'state':'0','msg':'修改失败'}");
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                IList<TB_InterestDubbingGame_PushMsg> courseList = new List<TB_InterestDubbingGame_PushMsg>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = courseList, total = totalcount };
                    Response.Write(JsonHelper.EncodeJson(obj1));
                }
                int pageindex = int.Parse(Request.Form["page"].ToString());
                int pagesize = int.Parse(Request.Form["rows"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["queryStr"]))
                {
                    strWhere = Request.QueryString["queryStr"];
                }
                else
                {
                    strWhere = "1=1 and DelState=1";
                }

                courseList = bll.GetList(strWhere);
                if (courseList == null)
                {
                    courseList = new List<TB_InterestDubbingGame_PushMsg>();
                }
                else
                {
                    totalcount = courseList.Count;
                    IList<TB_InterestDubbingGame_PushMsg> removelist = new List<TB_InterestDubbingGame_PushMsg>();
                    for (int i = 0; i < courseList.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(courseList[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            courseList.Remove(removelist[i]);
                        }
                    }
                }
                var obj = new { rows = courseList, total = totalcount };
                Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
    }
}