using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.SYS_Preevaluate_Template")]
	public class SYSPreevaluateTemplate : BaseTO
	{
		private long _id;
		[SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
		public long id
		{
			get{ return _id;}
			set{ _id=value;}
		}
		private int _city;
		/// <summary>
		/// 城市ID
		/// </summary>
		public int city
		{
			get{ return _city;}
			set{ _city=value;}
		}
		private int _companyid;
		/// <summary>
		/// 公司Id
		/// </summary>
		public int companyid
		{
			get{ return _companyid;}
			set{ _companyid=value;}
		}
		private string _name;
		/// <summary>
		/// 文件名称
		/// </summary>
		public string name
		{
			get{ return _name;}
			set{ _name=value;}
		}
		private string _path;
		/// <summary>
		/// 文件路径
		/// </summary>
		public string path
		{
			get{ return _path;}
			set{ _path=value;}
		}
		private string _templatename;
		/// <summary>
		/// 模板名称
		/// </summary>
		public string templatename
		{
			get{ return _templatename;}
			set{ _templatename=value;}
		}
		private string _created;
		/// <summary>
		/// 上传人
		/// </summary>
		public string created
		{
			get{ return _created;}
			set{ _created=value;}
		}
		private DateTime _createddate = DateTime.Now;
		/// <summary>
		/// 上传时间
		/// </summary>
		public DateTime createddate
		{
			get{ return _createddate;}
			set{ _createddate=value;}
		}
		private string _saveuser;
		/// <summary>
		/// 修改人
		/// </summary>
		public string saveuser
		{
			get{ return _saveuser;}
			set{ _saveuser=value;}
		}
		private DateTime? _savedate;
		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? savedate
		{
			get{ return _savedate;}
			set{ _savedate=value;}
		}
		private int _valid = 1;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
	}
}