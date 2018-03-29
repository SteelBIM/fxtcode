using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyDBEntity
{
    /// <summary>
    /// 查勘库跟进表 caoq 2013-11-11
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_FollowUp")]
    public class DatFollowUp : BaseTO
    {
        private long _followupid;
        /// <summary>
        /// 自增ID
        /// </summary>
        [SQLField("followupid", EnumDBFieldUsage.PrimaryKey, true)]
        public long followupid
        {
            get { return _followupid; }
            set { _followupid = value; }
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
        private long _objectid;
        /// <summary>
        /// 业务对象ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _objecttypecode = 2018001;
        /// <summary>
        /// 业务类型：委托 委估对象 询价 查勘 预评 报告
        /// </summary>
        public int objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
        }
        private int? _businesstypecode;
        /// <summary>
        /// 业务环节流程CODE：5016020
        /// </summary>
        public int? businesstypecode
        {
            get { return _businesstypecode; }
            set { _businesstypecode = value; }
        }
        private long _entrustid = 0;
        /// <summary>
        /// 业务编号
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private string _content;
        /// <summary>
        /// 跟进内容
        /// </summary>
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private string _createuserid;
        /// <summary>
        /// 录入人
        /// </summary>
        public string createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        /// <summary>
        /// 录入人名称
        /// </summary>
        [SQLReadOnly]
        public string createtruename { get; set; }
         /// <summary>
        /// 录入人电话
        /// </summary>
        [SQLReadOnly]
        public string createmobile { get; set; }        

        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createusertype = 2;
        /// <summary>
        /// 录入类型：1为人工，2为系统
        /// </summary>
        public int createusertype
        {
            get { return _createusertype; }
            set { _createusertype = value; }
        }
        private bool _istixing = false;
        /// <summary>
        /// 是否已经提醒
        /// </summary>
        public bool istixing
        {
            get { return _istixing; }
            set { _istixing = value; }
        }
        private DateTime _tixingtime = DateTime.Now;
        /// <summary>
        /// 提醒时间
        /// </summary>
        public DateTime tixingtime
        {
            get { return _tixingtime; }
            set { _tixingtime = value; }
        }
        private string _tixinguserids;
        /// <summary>
        /// 提醒人
        /// </summary>
        public string tixinguserids
        {
            get { return _tixinguserids; }
            set { _tixinguserids = value; }
        }
        private bool _issendsmessage = false;
        /// <summary>
        /// 是否消息提醒
        /// </summary>
        public bool issendsmessage
        {
            get { return _issendsmessage; }
            set { _issendsmessage = value; }
        }

        private bool _iftixing = false;
        /// <summary>
        /// 是否提醒
        /// </summary>
        public bool iftixing
        {
            get { return _iftixing; }
            set { _iftixing = value; }
        }

        private string _newaddflag;
        /// <summary>
        /// 新增标识字段
        /// </summary>
        public string newaddflag
        {
            get { return _newaddflag; }
            set { _newaddflag = value; }
        }

        private int? _statecode;
        /// <summary>
        /// 外部statecode
        /// </summary>
        public int? statecode
        {
            get { return _statecode; }
            set { _statecode = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }
}