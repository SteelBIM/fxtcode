using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter
{
    [Serializable]
    [TableAttribute("dbo.SYL_Dat_tzsyl")]
    public class SYLDat : BaseTO
    {
        /// <summary>
        /// 数据日期
        /// </summary>
        private DateTime _datadate;
        [SQLField("datadate", EnumDBFieldUsage.CombinPrimaryKey)]
        public DateTime datadate { get { return _datadate; } set { _datadate = value; } }
        /// <summary>
        /// 城市id
        /// </summary>
        private int _cityid;
        [SQLField("cityid", EnumDBFieldUsage.CombinPrimaryKey)]
        public int cityid { get { return _cityid; } set { _cityid = value; } }
        /// <summary>
        /// 城市id
        /// </summary>
        private string _cityname;
        public string cityname { get { return _cityname; } set { _cityname = value; } }
        /// <summary>
        /// 行政区
        /// </summary>
        private string _areaname;
        [SQLField("areaname", EnumDBFieldUsage.CombinPrimaryKey)]
        public string areaname { get { return _areaname; } set { _areaname = value; } }
        /// <summary>
        /// 市场区域
        /// </summary>
        private string _subareaname;
        [SQLField("subareaname", EnumDBFieldUsage.CombinPrimaryKey)]
        public string subareaname { get { return _subareaname; } set { _subareaname = value; } }
        /// <summary>
        /// 住宅类历史房地产投资收益率平均值%
        /// </summary>
        private string _househisavg;
        public string househisavg { get { return _househisavg; } set { _househisavg = value; } }
        /// <summary>
        /// 住宅类历史房地产投资收益率上限%
        /// </summary>
        private string _househismax;
        public string househismax { get { return _househismax; } set { _househismax = value; } }
        /// <summary>
        /// 住宅类历史房地产投资收益率下限%
        /// </summary>
        private string _househismin;
        public string househismin { get { return _househismin; } set { _househismin = value; } }
        /// <summary>
        /// 住宅类当前平均直接资本化率%
        /// </summary>
        private string _housecur;
        public string housecur { get { return _housecur; } set { _housecur = value; } }
        /// <summary>
        /// 住宅类预期房地产投资收益率平均值%
        /// </summary>
        private string _housefuravg;
        public string housefuravg { get { return _housefuravg; } set { _housefuravg = value; } }
        /// <summary>
        /// 住宅类预期房地产投资收益率上限%
        /// </summary>
        private string _housefurmax;
        public string housefurmax { get { return _housefurmax; } set { _housefurmax = value; } }
        /// <summary>
        /// 住宅类预期房地产投资收益率下限%
        /// </summary>
        private string _housefurmin;
        public string housefurmin { get { return _housefurmin; } set { _housefurmin = value; } }
        /// <summary>
        /// 商业类历史房地产投资收益率平均值%
        /// </summary>
        private string _bizhisavg;
        public string bizhisavg { get { return _bizhisavg; } set { _bizhisavg = value; } }
        /// <summary>
        /// 商业类历史房地产投资收益率上限%
        /// </summary>
        private string _bizhismax;
        public string bizhismax { get { return _bizhismax; } set { _bizhismax = value; } }
        /// <summary>
        /// 商业类历史房地产投资收益率下限%
        /// </summary>
        private string _bizhismin;
        public string bizhismin { get { return _bizhismin; } set { _bizhismin = value; } }
        /// <summary>
        /// 商业类当前平均直接资本化率%
        /// </summary>
        private string _bizcur;
        public string bizcur { get { return _bizcur; } set { _bizcur = value; } }
        /// <summary>
        /// 商业类预期房地产投资收益率平均值%
        /// </summary>
        private string _bizfuravg;
        public string bizfuravg { get { return _bizfuravg; } set { _bizfuravg = value; } }
        /// <summary>
        /// 商业类预期房地产投资收益率上限%
        /// </summary>
        private string _bizfurmax;
        public string bizfurmax { get { return _bizfurmax; } set { _bizfurmax = value; } }
        /// <summary>
        /// 商业类预期房地产投资收益率下限%
        /// </summary>
        private string _bizfurmin;
        public string bizfurmin { get { return _bizfurmin; } set { _bizfurmin = value; } }
        /// <summary>
        /// 办公类历史房地产投资收益率平均值%
        /// </summary>
        private string _officehisavg;
        public string officehisavg { get { return _officehisavg; } set { _officehisavg = value; } }
        /// <summary>
        /// 办公类历史房地产投资收益率上限%
        /// </summary>
        private string _officehismax;
        public string officehismax { get { return _officehismax; } set { _officehismax = value; } }
        /// <summary>
        /// 办公类历史房地产投资收益率下限%
        /// </summary>
        private string _officehismin;
        public string officehismin { get { return _officehismin; } set { _officehismin = value; } }
        /// <summary>
        /// 办公类当前平均直接资本化率%
        /// </summary>
        private string _officecur;
        public string officecur { get { return _officecur; } set { _officecur = value; } }
        /// <summary>
        /// 办公类预期房地产投资收益率平均值%
        /// </summary>
        private string _officefuravg;
        public string officefuravg { get { return _officefuravg; } set { _officefuravg = value; } }
        /// <summary>
        /// 办公类预期房地产投资收益率上限%
        /// </summary>
        private string _officefurmax;
        public string officefurmax { get { return _officefurmax; } set { _officefurmax = value; } }
        /// <summary>
        /// 办公类预期房地产投资收益率下限%
        /// </summary>
        private string _officefurmin;
        public string officefurmin { get { return _officefurmin; } set { _officefurmin = value; } }
    }
}
