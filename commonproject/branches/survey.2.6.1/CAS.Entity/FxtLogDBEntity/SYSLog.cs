using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtLogDBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_Log")]
    public class SYSLog : BaseTO
    {
        private long _logid;
        /// <summary>
        /// 系统日志
        /// </summary>
        [SQLField("logid", EnumDBFieldUsage.PrimaryKey, true)]
        public long logid
        {
            get { return _logid; }
            set { _logid = value; }
        }
        private int _systype;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int systype
        {
            get { return _systype; }
            set { _systype = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _userid;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _username;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private int _logtype;
        /// <summary>
        /// 日志类型
        /// </summary>
        public int logtype
        {
            get { return _logtype; }
            set { _logtype = value; }
        }
        private int _eventtype;
        /// <summary>
        /// 事件类型
        /// </summary>
        public int eventtype
        {
            get { return _eventtype; }
            set { _eventtype = value; }
        }
        private string _objectid;
        /// <summary>
        /// 操作对象ID
        /// </summary>
        public string objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private string _objectname;
        /// <summary>
        /// 操作对象名称
        /// </summary>
        public string objectname
        {
            get { return _objectname; }
            set { _objectname = value; }
        }
        private string _objectid1;
        /// <summary>
        /// 操作对象1
        /// </summary>
        public string objectid1
        {
            get { return _objectid1; }
            set { _objectid1 = value; }
        }
        private string _objectname1;
        /// <summary>
        /// 操作对象名称1
        /// </summary>
        public string objectname1
        {
            get { return _objectname1; }
            set { _objectname1 = value; }
        }
        private string _remarks;
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        private DateTime _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private string _webip;
        /// <summary>
        /// 客户访问的IP地址
        /// </summary>
        public string webip
        {
            get { return _webip; }
            set { _webip = value; }
        }
    }
}