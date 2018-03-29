using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_BuMen")]
	public class SYSBuMen : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int? _danweiid;
		/// <summary>
		/// 公司ID(单位ID)
		/// </summary>
		public int? danweiid
		{
			get{ return _danweiid;}
			set{ _danweiid=value;}
		}
		private int? _bumentype;
		/// <summary>
		/// 1-分支机构，2-部门
		/// </summary>
		public int? bumentype
		{
			get{ return _bumentype;}
			set{ _bumentype=value;}
		}
		private string _bumenname;
		/// <summary>
		/// 部门名称
		/// </summary>
		public string bumenname
		{
			get{ return _bumenname;}
			set{ _bumenname=value;}
		}
		private string _chargeman;
		/// <summary>
		/// 负责人
		/// </summary>
		public string chargeman
		{
			get{ return _chargeman;}
			set{ _chargeman=value;}
		}
		private string _telstr;
		/// <summary>
		/// 电话
		/// </summary>
		public string telstr
		{
			get{ return _telstr;}
			set{ _telstr=value;}
		}
		private string _chuanzhen;
		/// <summary>
		/// 传真
		/// </summary>
		public string chuanzhen
		{
			get{ return _chuanzhen;}
			set{ _chuanzhen=value;}
		}
		private string _backinfo;
		/// <summary>
		/// 备注说明
		/// </summary>
		public string backinfo
		{
			get{ return _backinfo;}
			set{ _backinfo=value;}
		}
		private string _address;
		public string address
		{
			get{ return _address;}
			set{ _address=value;}
		}
		private string _email;
		public string email
		{
			get{ return _email;}
			set{ _email=value;}
		}
		private string _linkman;
		public string linkman
		{
			get{ return _linkman;}
			set{ _linkman=value;}
		}
        private bool _allowsview = false;
        /// <summary>
        /// 允许查看其他公司业务
        /// </summary>
        public bool allowsview
        {
            get { return _allowsview; }
            set { _allowsview = value; }
        }
        
		private int _dirid = 0;
		/// <summary>
		/// 上级部门ID
		/// </summary>
		public int dirid
		{
			get{ return _dirid;}
			set{ _dirid=value;}
		}
		private int? _cityid;
		/// <summary>
		/// 城市ID
		/// </summary>
		public int? cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private int _valid = 1;
		/// <summary>
		/// 是否有效
		/// </summary>
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}

        private string _businessscope;
		/// <summary>
		/// 业务范围
		/// </summary>
        public string businessscope
		{
            get { return _businessscope; }
            set { _businessscope = value; }
		}

        private string _classiccase;
        /// <summary>
        /// 业务范围
        /// </summary>
        public string classiccase
        {
            get { return _classiccase; }
            set { _classiccase = value; }
        }
        
	}
}