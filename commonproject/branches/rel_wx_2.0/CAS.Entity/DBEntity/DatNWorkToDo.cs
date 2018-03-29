using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkToDo")]
	public class DatNWorkToDo : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int? _businessid;
		public int? businessid
		{
			get{ return _businessid;}
			set{ _businessid=value;}
		}
		private string _workname;
		/// <summary>
		/// 工作名称
		/// </summary>
		public string workname
		{
			get{ return _workname;}
			set{ _workname=value;}
		}
		private int? _formid;
		/// <summary>
		/// 所用表单
		/// </summary>
		public int? formid
		{
			get{ return _formid;}
			set{ _formid=value;}
		}
		private int? _workflowid;
		/// <summary>
		/// 所用工作流程
		/// </summary>
		public int? workflowid
		{
			get{ return _workflowid;}
			set{ _workflowid=value;}
		}
        private int _businesstype;
        /// <summary>
        /// 流程类型0：行政、1：询价、2：预评、3：报告
        /// </summary>
        public int businesstype
        {
            get { return _businesstype; }
            set { _businesstype = value; }
        }
		private string _username;
		/// <summary>
		/// 发起人
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private DateTime? _timestr;
		/// <summary>
		/// 发起时间
		/// </summary>
		public DateTime? timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
		private string _formcontent;
		/// <summary>
		/// 表单内容
		/// </summary>
		public string formcontent
		{
			get{ return _formcontent;}
			set{ _formcontent=value;}
		}
		private string _fujianlist;
		/// <summary>
		/// 附件文件
		/// </summary>
		public string fujianlist
		{
			get{ return _fujianlist;}
			set{ _fujianlist=value;}
		}
		private string _shenpiyijian;
		/// <summary>
		/// 签注审批
		/// </summary>
		public string shenpiyijian
		{
			get{ return _shenpiyijian;}
			set{ _shenpiyijian=value;}
		}
		private int? _jiedianid;
		/// <summary>
		/// 当前所在节点
		/// </summary>
		public int? jiedianid
		{
			get{ return _jiedianid;}
			set{ _jiedianid=value;}
		}
		private string _jiedianname;
		/// <summary>
		/// 当前节点名称
		/// </summary>
		public string jiedianname
		{
			get{ return _jiedianname;}
			set{ _jiedianname=value;}
		}
		private string _shenpiuserlist;
		/// <summary>
		/// 当前审批用户（可以多个人）
		/// </summary>
		public string shenpiuserlist
		{
			get{ return _shenpiuserlist;}
			set{ _shenpiuserlist=value;}
		}
        private string _shengyushenpitruenamelist;
        /// <summary>
        /// 剩余的待审批用户姓名
        /// </summary>
        public string shengyushenpitruenamelist
        {
            get { return _shengyushenpitruenamelist; }
            set { _shengyushenpitruenamelist = value; }
        }
        private string _consignerusers;
        /// <summary>
        /// 原始审批用户(未替换被委托的人之前的审批用户)
        /// </summary>
        public string consignerusers
        {
            get { return _consignerusers; }
            set { _consignerusers = value; }
        }

		private string _okuserlist;
		/// <summary>
		/// 当前已审批通过的用户（可以多个人）
		/// </summary>
		public string okuserlist
		{
			get{ return _okuserlist;}
			set{ _okuserlist=value;}
		}
		private string _statenow;
		/// <summary>
		/// 当前状态
		/// </summary>
		public string statenow
		{
			get{ return _statenow;}
			set{ _statenow=value;}
		}
		private int? _state;
		/// <summary>
		/// 状态： 1为已发送超时提醒
		/// </summary>
		public int? state
		{
			get{ return _state;}
			set{ _state=value;}
		}
		private DateTime? _latetime;
		/// <summary>
		/// 超时时间（何时超时）
		/// </summary>        
		public DateTime? latetime
		{
			get{ return _latetime;}
			set{ _latetime=value;}
		}
		private string _readers;
		/// <summary>
		/// 开封者,使用","逗号分隔
		/// </summary>
		public string readers
		{
			get{ return _readers;}
			set{ _readers=value;}
		}
        private DateTime _timestamp = DateTime.Now;
        /// <summary>
        /// 时间戳（在跳转到其他节点时发生）
        /// </summary>
        public DateTime timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private DateTime? _endon;
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? endon
        {
            get { return _endon; }
            set { _endon = value; }
        }

	}
}