using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_NWorkFlowNode")]
	public class DatNWorkFlowNode : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int? _workflowid;
		/// <summary>
		/// 工作流程
		/// </summary>
		public int? workflowid
		{
			get{ return _workflowid;}
			set{ _workflowid=value;}
		}
		private string _nodeserils;
		/// <summary>
		/// 节点序号
		/// </summary>
		public string nodeserils
		{
			get{ return _nodeserils;}
			set{ _nodeserils=value;}
		}
		private string _nodename;
		/// <summary>
		/// 节点名称
		/// </summary>
		public string nodename
		{
			get{ return _nodename;}
			set{ _nodename=value;}
		}
		private string _nodeaddr;
		/// <summary>
		/// 节点位置
		/// </summary>
		public string nodeaddr
		{
			get{ return _nodeaddr;}
			set{ _nodeaddr=value;}
		}
		private string _nextnode;
		/// <summary>
		/// 下一结点
		/// </summary>
		public string nextnode
		{
			get{ return _nextnode;}
			set{ _nextnode=value;}
		}
		private string _ifcanjump;
		/// <summary>
		/// 是否允许强制跳转
		/// </summary>
		public string ifcanjump
		{
			get{ return _ifcanjump;}
			set{ _ifcanjump=value;}
		}
		private string _ifcanview;
		/// <summary>
		/// 是否允许查看附件
		/// </summary>
		public string ifcanview
		{
			get{ return _ifcanview;}
			set{ _ifcanview=value;}
		}
		private string _ifcanedit;
		/// <summary>
		/// 是否允许编辑附件
		/// </summary>
		public string ifcanedit
		{
			get{ return _ifcanedit;}
			set{ _ifcanedit=value;}
		}
		private string _ifcandel;
		/// <summary>
		/// 是否允许删除附件
		/// </summary>
		public string ifcandel
		{
			get{ return _ifcandel;}
			set{ _ifcandel=value;}
		}
		private int? _jieshuhours;
		/// <summary>
		/// 超时设置
		/// </summary>
		public int? jieshuhours
		{
			get{ return _jieshuhours;}
			set{ _jieshuhours=value;}
		}
		private string _pstype;
		/// <summary>
		/// 评审模式（一人通过可向下流转、全部通过可向下流转）)
		/// </summary>
		public string pstype
		{
			get{ return _pstype;}
			set{ _pstype=value;}
		}
		private string _sptype;
		/// <summary>
        /// 审批人选择模式(审批时自由指定、从默认审批人中选择、从默认审批部门中选择、从默认审批角色中选择、自动选择流程发起人、自动选择本部门主管、自动选择上一级部门主管、无需审批人)
		/// </summary>
		public string sptype
		{
			get{ return _sptype;}
			set{ _sptype=value;}
		}
		private string _spdefaultlist;
		/// <summary>
		/// 默认待选值（人、部门、角色）支持多个
		/// </summary>
		public string spdefaultlist
		{
			get{ return _spdefaultlist;}
			set{ _spdefaultlist=value;}
		}
		private string _canwriteset;
		/// <summary>
		/// 可写字段设置
		/// </summary>
		public string canwriteset
		{
			get{ return _canwriteset;}
			set{ _canwriteset=value;}
		}
		private string _secretset;
		/// <summary>
		/// 保密字段设置
		/// </summary>
		public string secretset
		{
			get{ return _secretset;}
			set{ _secretset=value;}
		}
		private string _conditionset;
		/// <summary>
		/// 条件字段设置
		/// </summary>
		public string conditionset
		{
			get{ return _conditionset;}
			set{ _conditionset=value;}
		}
		private string _nodeleft;
		/// <summary>
		/// 节点左坐标
		/// </summary>
		public string nodeleft
		{
			get{ return _nodeleft;}
			set{ _nodeleft=value;}
		}
		private string _nodetop;
		/// <summary>
		/// 节点上坐标
		/// </summary>
		public string nodetop
		{
			get{ return _nodetop;}
			set{ _nodetop=value;}
		}
		private bool _performancedependent = false;
		/// <summary>
		/// 指示该步骤是否参与业务统计 1为参与 0为不参与
		/// </summary>
		public bool performancedependent
		{
			get{ return _performancedependent;}
			set{ _performancedependent=value;}
		}
		private int? _nodetype;
		/// <summary>
        /// 节点类型：0互审、1初审、2二审、3三审、4终审、5盖章、6复印、7其他
		/// </summary>
		public int? nodetype
		{
			get{ return _nodetype;}
			set{ _nodetype=value;}
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
		private int _valid = 0;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}