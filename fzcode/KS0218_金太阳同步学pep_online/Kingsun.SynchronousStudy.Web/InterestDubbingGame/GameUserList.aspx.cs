using Kingsun.SynchronousStudy.Common;
using Kingsun.InterestDubbingGame.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kingsun.SynchronousStudy.BLL;
using Kingsun.InterestDubbingGame.BLL;
using System.Data;
using System.IO;

namespace Kingsun.SynchronousStudy.Web.InterestDubbingGame
{
    public partial class GameUserList : System.Web.UI.Page
    {
        static RedisHashHelper redis = new RedisHashHelper();
        TB_InterestDubbingGame_UserInfoBLL bll = new TB_InterestDubbingGame_UserInfoBLL();
        TB_InterestDubbingGame_MatchTimeBLL m_bll = new TB_InterestDubbingGame_MatchTimeBLL();
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string Action = "";
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
                if (string.IsNullOrEmpty(Request.QueryString["Action"]))//获取form的Action中的参数 
                    return;
                Action = Request.QueryString["Action"];
                switch (Action)
                {
                    case "Query":
                        query();
                        break;
                    case "setting":
                        Set();
                        break;
                    case "operationData":
                        operationData();
                        break;
                    case "GetTB_APPManagement":
                        GetTB_APPManagement();
                        break;
                    case "excel":
                        excel();
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
        /// Excel导出
        /// </summary>
        public void excel()
        {
            try
            {
                DataTable dt = bll.GetSearchUserInfoTable(" 1=1");
                DataTable newDt = new DataTable("newDt");
                newDt.Columns.Add("UserID", typeof(string));
                newDt.Columns.Add("姓名", typeof(string));
                newDt.Columns.Add("手机号码", typeof(string));
                newDt.Columns.Add("版本", typeof(string));
                newDt.Columns.Add("SchoolID", typeof(string));
                newDt.Columns.Add("学校名称", typeof(string));
                newDt.Columns.Add("ClassID", typeof(string));
                newDt.Columns.Add("班级名称", typeof(string));
                newDt.Columns.Add("报名时间", typeof(string));
                newDt.Columns.Add("区域ID", typeof(string));
                newDt.Columns.Add("区域名称", typeof(string));
                newDt.Columns.Add("学段", typeof(string));
                newDt.Columns.Add("总分(配音+朗读+投票)", typeof(string));
                newDt.Columns.Add("配音作品地址", typeof(string));
                newDt.Columns.Add("朗读作品地址", typeof(string)); 
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = newDt.NewRow();
                        string UserID = dt.Rows[i]["UserID"].ToString();
                        dr["UserID"] = dt.Rows[i]["UserID"].ToString();
                        dr["姓名"] = dt.Rows[i]["UserName"].ToString();
                        dr["手机号码"] = dt.Rows[i]["ContactPhone"].ToString();
                        dr["版本"] = dt.Rows[i]["VersionName"].ToString();
                        dr["SchoolID"] = dt.Rows[i]["SchoolID"].ToString();
                        dr["学校名称"] = dt.Rows[i]["SchoolName"].ToString();
                        dr["ClassID"] = dt.Rows[i]["ClassID"].ToString();
                        dr["班级名称"] = dt.Rows[i]["ClassName"].ToString();
                        dr["报名时间"] = dt.Rows[i]["SignUpTime"].ToString();
                        dr["区域ID"] = dt.Rows[i]["AreaID"].ToString();
                        dr["区域名称"] = dt.Rows[i]["AreaName"].ToString();
                        dr["学段"] = dt.Rows[i]["Stage"].ToString();
                        //dr["总分(配音+朗读+投票)"] = dt.Rows[i]["TotalScore"].ToString();
                        dr["配音作品地址"] = dt.Rows[i]["DubbingAddress"].ToString();
                        dr["朗读作品地址"] = dt.Rows[i]["ReadAddress"].ToString();
                        Redis_InterestDubbingGame_UserTotalScore redisUserTotalScore = redis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", UserID);
                        if (redisUserTotalScore != null)
                        {
                            dr["总分(配音+朗读+投票)"] = redisUserTotalScore.TotalScore + "：" + redisUserTotalScore.BookPlayScore + "+" +
                            redisUserTotalScore.StoryReadScore + "+" + redisUserTotalScore.VoteNum.ToString();
                        }
                        else
                        {
                            dr["总分(配音+朗读+投票)"] = dt.Rows[i]["TotalScore"].ToString();
                        }
                        newDt.Rows.Add(dr);
                    }
                }
                MemoryStream s = newDt.ToExcel() as MemoryStream;
                if (s != null)
                {
                    byte[] excel = s.ToArray();
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=比赛用户列表.xlsx"));
                    Response.AddHeader("Content-Length", excel.Length.ToString());
                    Response.BinaryWrite(excel);
                    s.Close();
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
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
                IList<Kingsun.SynchronousStudy.Models.TB_APPManagement> list = JsonHelper.DataSetToIList<Kingsun.SynchronousStudy.Models.TB_APPManagement>(set, 0);
                string json = JsonHelper.EncodeJson(list);
                Response.Write(json);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write("");
            }
        }

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                string strWhere = "";
                int totalcount = 0;
                IList<SearchUserInfo> list = new List<SearchUserInfo>();
                IList<SearchUserInfo> gameList = new List<SearchUserInfo>();
                if (string.IsNullOrEmpty(Request.Form["page"]) || string.IsNullOrEmpty(Request.Form["rows"]))
                {
                    var obj1 = new { rows = gameList, total = totalcount };
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
                    strWhere = "1=1";
                }
                gameList = bll.GetSearchUserInfo(strWhere);
                if (gameList == null)
                {
                    gameList = new List<SearchUserInfo>();
                }
                else
                {
                    totalcount = gameList.Count;
                    IList<SearchUserInfo> removelist = new List<SearchUserInfo>();
                    for (int i = 0; i < gameList.Count; i++)
                    {
                        if (i < (pageindex - 1) * pagesize || i >= pageindex * pagesize)
                        {
                            removelist.Add(gameList[i]);
                        }
                    }
                    if (removelist != null && removelist.Count > 0)
                    {
                        for (int i = 0; i < removelist.Count; i++)
                        {
                            gameList.Remove(removelist[i]);
                        }
                    }
                }
                foreach (SearchUserInfo item in gameList)
                {
                    Redis_InterestDubbingGame_UserTotalScore redisUserTotalScore = redis.Get<Redis_InterestDubbingGame_UserTotalScore>("Redis_InterestDubbingGame_UserTotalScore", item.UserID);
                    if (redisUserTotalScore != null)
                    {
                        item.TotalScore = redisUserTotalScore.TotalScore + "：" + redisUserTotalScore.BookPlayScore + "+" +
                            redisUserTotalScore.StoryReadScore + "+" + redisUserTotalScore.VoteNum.ToString();
                        list.Add(item);
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
                var obj = new { rows = list, total = totalcount };
                Response.Write(JsonHelper.EncodeJson(obj));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        #endregion

        #region 回绑数据到页面上
        public void Set()
        {
            try
            {
                var _list = new TB_InterestDubbingGame_MatchTimeBLL().QueryList();
                if (_list == null || _list.Count == 0)
                {
                    Response.Write(JsonHelper.EncodeJson(new { result = false }));
                }
                else
                {
                    Response.Write(JsonHelper.EncodeJson(new { result = true, mod = _list.FirstOrDefault() }));
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        #endregion

        #region 操作数据
        /// <summary>
        /// 操作数据
        /// </summary>
        public void operationData()
        {
            try
            {
                bool result = true;

                string id = Request["id"];
                string SignUpStartTime = Request["SignUpStartTime"];
                string SignUpEndTime = Request["SignUpEndTime"];
                string FirstGameStartTime = Request["FirstGameStartTime"];
                string FirstGameEndTime = Request["FirstGameEndTime"];
                string SecondGameStartTime = Request["SecondGameStartTime"];
                string SecondGameEndTime = Request["SecondGameEndTime"];
                string FinalsStartTime = Request["FinalsStartTime"];
                string FinalsEndTime = Request["FinalsEndTime"];

                if (string.IsNullOrWhiteSpace(id))//新增
                {
                    result = new TB_InterestDubbingGame_MatchTimeBLL().InsertData(new TB_InterestDubbingGame_MatchTime
                    {
                        SignUpStartTime = Convert.ToDateTime(SignUpStartTime),
                        SignUpEndTime = Convert.ToDateTime(SignUpEndTime),
                        FirstGameStartTime = Convert.ToDateTime(FirstGameStartTime),
                        FirstGameEndTime = Convert.ToDateTime(FirstGameEndTime),
                        SecondGameStartTime = Convert.ToDateTime(SecondGameStartTime),
                        SecondGameEndTime = Convert.ToDateTime(SecondGameEndTime),
                        FinalsStartTime = Convert.ToDateTime(FinalsStartTime),
                        FinalsEndTime = Convert.ToDateTime(FinalsEndTime),
                        CreateTime = DateTime.Now
                    });
                }
                else//修改
                {
                    TB_InterestDubbingGame_MatchTime _m = new TB_InterestDubbingGame_MatchTimeBLL().GetModel(Convert.ToInt32(id));
                    if (_m == null)
                        Response.Write(JsonHelper.EncodeJson(new { state = 0, msg = "操作失败！" }));
                    _m.SignUpStartTime = Convert.ToDateTime(SignUpStartTime);
                    _m.SignUpEndTime = Convert.ToDateTime(SignUpEndTime);
                    _m.FirstGameStartTime = Convert.ToDateTime(FirstGameStartTime);
                    _m.FirstGameEndTime = Convert.ToDateTime(FirstGameEndTime);
                    _m.SecondGameStartTime = Convert.ToDateTime(SecondGameStartTime);
                    _m.SecondGameEndTime = Convert.ToDateTime(SecondGameEndTime);
                    _m.FinalsStartTime = Convert.ToDateTime(FinalsStartTime);
                    _m.FinalsEndTime = Convert.ToDateTime(FinalsEndTime);
                    result = new TB_InterestDubbingGame_MatchTimeBLL().UpdateData(_m);
                }
                if (result)
                    Response.Write(JsonHelper.EncodeJson(new { state = 1, msg = "操作成功！" }));
                else
                    Response.Write(JsonHelper.EncodeJson(new { state = 0, msg = "操作失败！" }));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                Response.Write(JsonHelper.EncodeJson(new { state = 0, msg = "操作失败！" }));
            }
        }
        #endregion

    }
}