using System;
using CAS.Entity.BaseDAModels;
namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkStepLog")]
	public class DatNWorkStepLog : BaseTO
	{
		private int _steplogid;
		[SQLField("steplogid", EnumDBFieldUsage.PrimaryKey, true)]
		public int steplogid
		{
			get{ return _steplogid;}
			set{ _steplogid=value;}
		}
		private int _fk_flownodeid;
		public int fk_flownodeid
		{
			get{ return _fk_flownodeid;}
			set{ _fk_flownodeid=value;}
		}
		private int _fk_todoid;
		public int fk_todoid
		{
			get{ return _fk_todoid;}
			set{ _fk_todoid=value;}
		}
		private bool _performancedependent;
		/// <summary>
		/// 指示该步骤是否参与业务统计 1为参与 0为不参与
		/// </summary>
		public bool performancedependent
		{
			get{ return _performancedependent;}
			set{ _performancedependent=value;}
		}
		private bool _reportedresults = false;
		/// <summary>
		/// 指示该步骤是否参与业绩上报 1为参与 0为不参与
		/// </summary>
		public bool reportedresults
		{
			get{ return _reportedresults;}
			set{ _reportedresults=value;}
		}
		private string _flownodename;
		public string flownodename
		{
			get{ return _flownodename;}
			set{ _flownodename=value;}
		}
		private int _status;
		/// <summary>
		/// 0等待审核、1审核通过、2保存并驳回发文人(可再次提交)、3保存并驳回发文人(不允许再次提交)、4保存并驳回到其他节点、5登记错误不过流程
		/// </summary>
		public int status
		{
			get{ return _status;}
			set{ _status=value;}
		}
		private int? _nodetype;
		/// <summary>
		/// 节点类型：0互审、1初审、2二审、3三审、4终审、5盖章、6复印
		/// </summary>
		public int? nodetype
		{
			get{ return _nodetype;}
			set{ _nodetype=value;}
		}
		private int? _jumpstepid;
		/// <summary>
		/// 如果为驳回其他节点，则该节点存在值
		/// </summary>
		public int? jumpstepid
		{
			get{ return _jumpstepid;}
			set{ _jumpstepid=value;}
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
		private DateTime? _starton;
		/// <summary>
		/// 开始处理时间
		/// </summary>
		public DateTime? starton
		{
			get{ return _starton;}
			set{ _starton=value;}
		}
		private DateTime? _endon;
		/// <summary>
		/// 结束处理时间
		/// </summary>
		public DateTime? endon
		{
			get{ return _endon;}
			set{ _endon=value;}
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

        private string _consignerusers;
        /// <summary>
        /// 原始审批用户(未替换被委托的人之前的审批用户),跟随步骤变化而改变
        /// </summary>
        public string consignerusers
        {
            get { return _consignerusers; }
            set { _consignerusers = value; }
        }

	}
}