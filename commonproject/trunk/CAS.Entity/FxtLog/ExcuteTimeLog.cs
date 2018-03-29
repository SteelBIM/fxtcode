using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("FxtLog.dbo.ExecuteTimeLog")]
    public class ExecuteTimeLog : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _functionname;
        /// <summary>
        /// 接口方法名称
        /// </summary>
        public string functionname
        {
            get { return _functionname; }
            set { _functionname = value; }
        }
        private int? _usercentertime;
        /// <summary>
        /// 用户中心验证使用时间（毫秒）
        /// </summary>
        public int? usercentertime
        {
            get { return _usercentertime; }
            set { _usercentertime = value; }
        }
        private int? _cityauthoritytime;
        /// <summary>
        /// 城市权限验证使用时间（毫秒）
        /// </summary>
        public int? cityauthoritytime
        {
            get { return _cityauthoritytime; }
            set { _cityauthoritytime = value; }
        }
        private int? _overflowtime;
        /// <summary>
        /// 流量控制使用时间（毫秒）
        /// </summary>
        public int? overflowtime
        {
            get { return _overflowtime; }
            set { _overflowtime = value; }
        }
        private int? _getdatatime;
        /// <summary>
        /// 查询数据使用时间（毫秒）
        /// </summary>
        public int? getdatatime
        {
            get { return _getdatatime; }
            set { _getdatatime = value; }
        }
        private int? _totaltime;
        /// <summary>
        /// 总执行时间（毫秒）
        /// </summary>
        public int? totaltime
        {
            get { return _totaltime; }
            set { _totaltime = value; }
        }
        private int? _sqltime;
        /// <summary>
        /// sql执行时间
        /// </summary>
        public int? sqltime
        {
            get { return _sqltime; }
            set { _sqltime = value; }
        }
        private Guid? _ident;
        /// <summary>
        /// 标示
        /// </summary>
        public Guid? ident
        {
            get { return _ident; }
            set { _ident = value; }
        }
        private string _time;
        /// <summary>
        /// 调用者传过来的时间
        /// </summary>
        public string time
        {
            get { return _time; }
            set { _time = value; }
        }
        private DateTime? _addtime;
        public DateTime? addtime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
        private string _requestparam;
        /// <summary>
        /// 请求参数
        /// </summary>
        public string requestparam
        {
            get { return _requestparam; }
            set { _requestparam = value; }
        }
        private string _serverid;
        public string serverid
        {
            get { return _serverid; }
            set { _serverid = value; }
        }
        private string _code;
        /// <summary>
        /// 业务编号
        /// </summary>
        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        private DateTime? _starttime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? starttime
        {
            get { return _starttime; }
            set { _starttime = value; }
        }
        private int? _fxtcompanyid;
        public int? fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int? _systypecode;
        public int? systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int? _sqlconnetiontime;
        /// <summary>
        /// 数据库连接时间
        /// </summary>
        public int? sqlconnetiontime
        {
            get { return _sqlconnetiontime; }
            set { _sqlconnetiontime = value; }
        }
        private int? _sqlexecutetime;
        /// <summary>
        /// 数据库执行时间
        /// </summary>
        public int? sqlexecutetime
        {
            get { return _sqlexecutetime; }
            set { _sqlexecutetime = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注说明
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}