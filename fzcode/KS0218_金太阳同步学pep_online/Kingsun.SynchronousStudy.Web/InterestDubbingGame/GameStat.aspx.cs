using Kingsun.InterestDubbingGame;
using Kingsun.InterestDubbingGame.DAL;
using Kingsun.InterestDubbingGame.Model;
using Kingsun.SynchronousStudy.Common;
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
    public partial class GameStat : System.Web.UI.Page
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["Action"]))
                    return;
                query();
                Response.End();
            }
        }
        static RedisHashHelper redis = new RedisHashHelper();
        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        public void query()
        {
            try
            {
                List<StatData> _list = new List<StatData>();
                StatData _model = new StatData();
                int _total = 0;
                string _where = string.Empty;
                string _star = Request.QueryString["star"];
                string _end = Request.QueryString["end"];
                if (!string.IsNullOrWhiteSpace(_star) && !string.IsNullOrWhiteSpace(_end))
                    _where = string.Format("SignUpTime BETWEEN '{0}' AND '{1}'", _star, _end);
                else
                    _where = " 1=1 ";
                DataSet _ds = new TB_InterestDubbingGame_UserInfoDAL().BackStatsData(_where);
                if (_ds.Tables[0].Rows.Count == 0)
                {
                    Response.Write(JsonHelper.EncodeJson(new { rows = _list, total = 0 }));
                    Response.End();
                }

                for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = _ds.Tables[0].Rows[i];
                    TB_InterestDubbingGame_UserInfo _m = new TB_InterestDubbingGame_UserInfoDAL().BackModel(string.Format("GradeID={0}", Convert.ToInt32(dr["GradeID"])));
                    switch (_m.GradeName)
                    {
                        case "一年级":
                            _model.One = Convert.ToInt32(dr["total"]);
                            _total += _model.One;
                            break;
                        case "二年级":
                            _model.Two = Convert.ToInt32(dr["total"]);
                            _total += _model.Two;
                            break;
                        case "三年级":
                            _model.Three = Convert.ToInt32(dr["total"]);
                            _total += _model.Three;
                            break;
                        case "四年级":
                            _model.Four = Convert.ToInt32(dr["total"]);
                            _total += _model.Four;
                            break;
                        case "五年级":
                            _model.Five = Convert.ToInt32(dr["total"]);
                            _total += _model.Five;
                            break;
                        case "六年级":
                            _model.Six = Convert.ToInt32(dr["total"]);
                            _total += _model.Six;
                            break;
                        default:
                            _model.Other = Convert.ToInt32(dr["total"]);
                            _total += _model.Other;
                            break;
                    }
                }

                _model.Total = _total;
                //获取投票总人数 
                long VoterRecordTotal = redis.Count("Redis_InterestDubbingGame_VoterRecord");
                _model.VoterRecordTotal = VoterRecordTotal;
                _list.Add(_model);
                Response.Write(JsonHelper.EncodeJson(new { rows = _list, total = 1 }));
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
        }
        #endregion
    }
    public class StatData
    { 
        public int Five { get; set; }
        public int Four { get; set; }
        public int One { get; set; }
        public int Other { get; set; }
        public int Six { get; set; }
        public int Three { get; set; }
        public int Total { get; set; }
        public int Two { get; set; }
        /// <summary>
        /// 投票总人数
        /// </summary>
        public long VoterRecordTotal { get; set; }
    }
}