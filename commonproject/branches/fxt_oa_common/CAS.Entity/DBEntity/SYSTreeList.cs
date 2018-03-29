using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_TreeList")]
	public class SYSTreeList : BaseTO
	{
		private int _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public int id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private string _textstr;
		/// <summary>
		/// 显示文字
		/// </summary>
		public string textstr
		{
			get{ return _textstr;}
			set{ _textstr=value;}
		}
		private string _imageurlstr;
		/// <summary>
		/// 所用图片
		/// </summary>
		public string imageurlstr
		{
			get{ return _imageurlstr;}
			set{ _imageurlstr=value;}
		}
		private string _valuestr;
		/// <summary>
		/// 后台数值
		/// </summary>
		public string valuestr
		{
			get{ return _valuestr;}
			set{ _valuestr=value;}
		}
		private string _navigateurlstr;
		/// <summary>
		/// 链接地址
		/// </summary>
		public string navigateurlstr
		{
			get{ return _navigateurlstr;}
			set{ _navigateurlstr=value;}
		}
		private string _target;
		/// <summary>
		/// 目标框架
		/// </summary>
		public string target
		{
			get{ return _target;}
			set{ _target=value;}
		}
		private int? _parentid;
		/// <summary>
		/// 父节点
		/// </summary>
		public int? parentid
		{
			get{ return _parentid;}
			set{ _parentid=value;}
		}
		private string _quanxianlist;
		/// <summary>
		/// 权限
		/// </summary>
		public string quanxianlist
		{
			get{ return _quanxianlist;}
			set{ _quanxianlist=value;}
		}
		private int? _paixustr;
		/// <summary>
		/// 排序
		/// </summary>
		public int? paixustr
		{
			get{ return _paixustr;}
			set{ _paixustr=value;}
		}
		private string _parentclass;
		public string parentclass
		{
			get{ return _parentclass;}
			set{ _parentclass=value;}
		}
        private bool _show = true;
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }

	}
}