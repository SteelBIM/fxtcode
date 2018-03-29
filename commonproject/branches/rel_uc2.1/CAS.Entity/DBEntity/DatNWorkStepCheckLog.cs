using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkStepCheckLog")]
	public class DatNWorkStepCheckLog : BaseTO
	{
		private int _stepchecklogid;
		[SQLField("stepchecklogid", EnumDBFieldUsage.PrimaryKey, true)]
		public int stepchecklogid
		{
			get{ return _stepchecklogid;}
			set{ _stepchecklogid=value;}
		}
		private int _fk_steplogid;
		public int fk_steplogid
		{
			get{ return _fk_steplogid;}
			set{ _fk_steplogid=value;}
		}
		private string _checkuserid;
		/// <summary>
		/// 操作审批的审核人
		/// </summary>
		public string checkuserid
		{
			get{ return _checkuserid;}
			set{ _checkuserid=value;}
		}
        private string _consigneruserid;
        /// <summary>
        /// 委托人用户
        /// </summary>
        public string consigneruserid
        {
            get { return _consigneruserid; }
            set { _consigneruserid = value; }
        }
		private int _checkstatus;
		/// <summary>
		/// 1审核通过、2保存并驳回发文人(可再次提交)、3保存并驳回发文人(不允许再次提交)、4保存并驳回到其他节点、5登记错误不过流程
		/// </summary>
		public int checkstatus
		{
			get{ return _checkstatus;}
			set{ _checkstatus=value;}
		}
		private DateTime _checkdate = DateTime.Now;
		public DateTime checkdate
		{
			get{ return _checkdate;}
			set{ _checkdate=value;}
		}
		private string _remark;
		public string remark
		{
			get{ return _remark;}
			set{ _remark=value;}
		}
		private string _errorsremark;
		public string errorsremark
		{
			get{ return _errorsremark;}
			set{ _errorsremark=value;}
		}
        private decimal? _totalprice;
        /// <summary>
        /// 审定评估价
        /// </summary>
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private decimal? _markettotalprice;
        /// <summary>
        /// 审定市场价
        /// </summary>
        public decimal? markettotalprice
        {
            get { return _markettotalprice; }
            set { _markettotalprice = value; }
        }
        private decimal? _marketpricedeviation;
        /// <summary>
        /// 审定偏差率
        /// </summary>
        public decimal? marketpricedeviation
        {
            get { return _marketpricedeviation; }
            set { _marketpricedeviation = value; }
        }
        private int? _error1;
        /// <summary>
        /// 原则性错误
        /// </summary>
        public int? error1
        {
            get { return _error1; }
            set { _error1 = value; }
        }
        private int? _error2;
        /// <summary>
        /// 非原则性错误
        /// </summary>
        public int? error2
        {
            get { return _error2; }
            set { _error2 = value; }
        }
	}
}