using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkFlow")]
	public class DatNWorkFlow : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _workflowname;
		/// <summary>
		/// 流程名称
		/// </summary>
		public string workflowname
		{
			get{ return _workflowname;}
			set{ _workflowname=value;}
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
		private int _businesstype = 0;
		/// <summary>
		/// 0为自定义表单 1为询价 2为预评 3为报告
		/// </summary>
		public int businesstype
		{
			get{ return _businesstype;}
			set{ _businesstype=value;}
		}
		private string _userlistok;
		/// <summary>
		/// 允许使用人
		/// </summary>
		public string userlistok
		{
			get{ return _userlistok;}
			set{ _userlistok=value;}
		}
		private string _deplistok;
		/// <summary>
		/// 允许使用部门
		/// </summary>
		public string deplistok
		{
			get{ return _deplistok;}
			set{ _deplistok=value;}
		}
		private string _jiaoselistok;
		/// <summary>
		/// 允许使用角色
		/// </summary>
		public string jiaoselistok
		{
			get{ return _jiaoselistok;}
			set{ _jiaoselistok=value;}
		}
		private string _paixustr;
		/// <summary>
		/// 排序字符
		/// </summary>
		public string paixustr
		{
			get{ return _paixustr;}
			set{ _paixustr=value;}
		}
		private string _username;
		/// <summary>
		/// 录入人
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private DateTime? _timestr;
		/// <summary>
		/// 录入时间
		/// </summary>
		public DateTime? timestr
		{
			get{ return _timestr;}
			set{ _timestr=value;}
		}
		private string _backinfo;
		/// <summary>
		/// 简要说明
		/// </summary>
		public string backinfo
		{
			get{ return _backinfo;}
			set{ _backinfo=value;}
		}
		private string _ifok;
		/// <summary>
		/// 是否启用
		/// </summary>
		public string ifok
		{
			get{ return _ifok;}
			set{ _ifok=value;}
		}
		private int _version = 1;
		public int version
		{
			get{ return _version;}
			set{ _version=value;}
		}
		private int _valid = 0;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
        private int _workflowtype = 1;
        /// <summary>
        /// 流程类别：1、多节点流程 2、单节点流程
        /// </summary>
        public int workflowtype
        {
            get { return _workflowtype; }
            set { _workflowtype = value; }
        }

	}
}