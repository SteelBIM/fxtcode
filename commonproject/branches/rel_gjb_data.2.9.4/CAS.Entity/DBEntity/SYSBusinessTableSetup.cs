using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_BusinessTableSetup")]
	public class SYSBusinessTableSetup : BaseTO
	{
		private int _btsid;
		/// <summary>
		/// 自增ID
		/// </summary>
		[SQLField("btsid", EnumDBFieldUsage.PrimaryKey, true)]
		public int btsid
		{
			get{ return _btsid;}
			set{ _btsid=value;}
		}
		private int _fxtcompanyid;
		/// <summary>
		/// 机构ID
		/// </summary>
		public int fxtcompanyid
		{
			get{ return _fxtcompanyid;}
			set{ _fxtcompanyid=value;}
		}
		private int _subcompanyid = 0;
		/// <summary>
		/// 分支机构ID
		/// </summary>
		public int subcompanyid
		{
			get{ return _subcompanyid;}
			set{ _subcompanyid=value;}
		}
		private int _typecode = 2018004;
		/// <summary>
		/// 业务类型，询价、查勘、初评、报告\ 价格纠错\价格争议
		/// </summary>
		public int typecode
		{
			get{ return _typecode;}
			set{ _typecode=value;}
		}
		private int _subtypecode = 1031001;
		/// <summary>
		/// 业务子类型（委估对象类型、报告类型）
		/// </summary>
		public int subtypecode
		{
			get{ return _subtypecode;}
			set{ _subtypecode=value;}
		}
		private string _fieldscontent;
		/// <summary>
		/// 字段配置，网页HTML
		/// </summary>
		public string fieldscontent
		{
			get{ return _fieldscontent;}
			set{ _fieldscontent=value;}
		}
		private string _excelpath;
		/// <summary>
		/// 查勘EXCEL模板地址
		/// </summary>
		public string excelpath
		{
			get{ return _excelpath;}
			set{ _excelpath=value;}
		}
		private DateTime _createtime = DateTime.Now;
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
		private int _createuserid;
		public int createuserid
		{
			get{ return _createuserid;}
			set{ _createuserid=value;}
		}

        public int fieldgrouptype { get; set; }

        private string _wxfieldscontent;
        /// <summary>
        /// 字段配置，微信网页HTML
        /// </summary>
        public string wxfieldscontent
        {
            get { return _wxfieldscontent; }
            set { _wxfieldscontent = value; }
        }
        /// <summary>
        /// 是否存在新增字段
        /// 插入时值为true
        /// </summary>
        public bool isnewfield { get; set; }
	}
}