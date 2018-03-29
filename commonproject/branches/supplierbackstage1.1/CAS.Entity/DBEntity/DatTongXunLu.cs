using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.Dat_TongXunLu")]
	public class DatTongXunLu : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _username;
		/// <summary>
		/// 用户名
		/// </summary>
		public string username
		{
			get{ return _username;}
			set{ _username=value;}
		}
		private string _ifshare;
		/// <summary>
		/// 是否共享
		/// </summary>
		public string ifshare
		{
			get{ return _ifshare;}
			set{ _ifshare=value;}
		}
		private string _typestr;
		/// <summary>
		/// 分类
		/// </summary>
		public string typestr
		{
			get{ return _typestr;}
			set{ _typestr=value;}
		}
		private string _fenzu;
		/// <summary>
		/// 分组
		/// </summary>
		public string fenzu
		{
			get{ return _fenzu;}
			set{ _fenzu=value;}
		}
		private string _namestr;
		/// <summary>
		/// 姓名
		/// </summary>
		public string namestr
		{
			get{ return _namestr;}
			set{ _namestr=value;}
		}
		private string _sex;
		/// <summary>
		/// 性别
		/// </summary>
		public string sex
		{
			get{ return _sex;}
			set{ _sex=value;}
		}
		private string _birthday;
		/// <summary>
		/// 生日
		/// </summary>
		public string birthday
		{
			get{ return _birthday;}
			set{ _birthday=value;}
		}
		private string _nicheng;
		/// <summary>
		/// 昵称
		/// </summary>
		public string nicheng
		{
			get{ return _nicheng;}
			set{ _nicheng=value;}
		}
		private string _zhiwu;
		/// <summary>
		/// 职务
		/// </summary>
		public string zhiwu
		{
			get{ return _zhiwu;}
			set{ _zhiwu=value;}
		}
		private string _peiou;
		/// <summary>
		/// 配偶
		/// </summary>
		public string peiou
		{
			get{ return _peiou;}
			set{ _peiou=value;}
		}
		private string _zinv;
		/// <summary>
		/// 子女
		/// </summary>
		public string zinv
		{
			get{ return _zinv;}
			set{ _zinv=value;}
		}
		private string _danweimingcheng;
		/// <summary>
		/// 单位名称
		/// </summary>
		public string danweimingcheng
		{
			get{ return _danweimingcheng;}
			set{ _danweimingcheng=value;}
		}
		private string _danweidizhi;
		/// <summary>
		/// 单位地址
		/// </summary>
		public string danweidizhi
		{
			get{ return _danweidizhi;}
			set{ _danweidizhi=value;}
		}
		private string _danweiyoubian;
		/// <summary>
		/// 单位邮编
		/// </summary>
		public string danweiyoubian
		{
			get{ return _danweiyoubian;}
			set{ _danweiyoubian=value;}
		}
		private string _danwiedianhua;
		/// <summary>
		/// 单位电话
		/// </summary>
		public string danwiedianhua
		{
			get{ return _danwiedianhua;}
			set{ _danwiedianhua=value;}
		}
		private string _danweichuanzhen;
		/// <summary>
		/// 单位传真
		/// </summary>
		public string danweichuanzhen
		{
			get{ return _danweichuanzhen;}
			set{ _danweichuanzhen=value;}
		}
		private string _jiatingzhuzhi;
		/// <summary>
		/// 家庭住址
		/// </summary>
		public string jiatingzhuzhi
		{
			get{ return _jiatingzhuzhi;}
			set{ _jiatingzhuzhi=value;}
		}
		private string _jiatingyoubian;
		/// <summary>
		/// 家庭邮编
		/// </summary>
		public string jiatingyoubian
		{
			get{ return _jiatingyoubian;}
			set{ _jiatingyoubian=value;}
		}
		private string _jiatingdianhua;
		/// <summary>
		/// 家庭座机
		/// </summary>
		public string jiatingdianhua
		{
			get{ return _jiatingdianhua;}
			set{ _jiatingdianhua=value;}
		}
		private string _shouji;
		/// <summary>
		/// 手机
		/// </summary>
		public string shouji
		{
			get{ return _shouji;}
			set{ _shouji=value;}
		}
		private string _xiaolingtong;
		/// <summary>
		/// 小灵通
		/// </summary>
		public string xiaolingtong
		{
			get{ return _xiaolingtong;}
			set{ _xiaolingtong=value;}
		}
		private string _email;
		/// <summary>
		/// 邮箱
		/// </summary>
		public string email
		{
			get{ return _email;}
			set{ _email=value;}
		}
		private string _qq;
		/// <summary>
		/// QQ
		/// </summary>
		public string qq
		{
			get{ return _qq;}
			set{ _qq=value;}
		}
		private string _msn;
		/// <summary>
		/// MSN
		/// </summary>
		public string msn
		{
			get{ return _msn;}
			set{ _msn=value;}
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
	}
}