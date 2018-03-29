using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
	[Serializable]
	[TableAttribute("dbo.DAT_House")]
	public class DATHouse : BaseTO
	{
		private int _houseid;
		/// <summary>
		/// id
		/// </summary>
		[SQLField("houseid", EnumDBFieldUsage.PrimaryKey, true)]
		public int houseid
		{
			get{ return _houseid;}
			set{ _houseid=value;}
		}
		private int _buildingid;
		/// <summary>
		/// 楼栋
		/// </summary>
		public int buildingid
		{
			get{ return _buildingid;}
			set{ _buildingid=value;}
		}
		private string _housename;
		/// <summary>
		/// 房号名称
		/// </summary>
		public string housename
		{
			get{ return _housename;}
			set{ _housename=value;}
		}
		private int? _housetypecode;
		/// <summary>
		/// 户型
		/// </summary>
		public int? housetypecode
		{
			get{ return _housetypecode;}
			set{ _housetypecode=value;}
		}
		private int _floorno;
		/// <summary>
		/// 楼层
		/// </summary>
		public int floorno
		{
			get{ return _floorno;}
			set{ _floorno=value;}
		}
		private string _unitno;
		/// <summary>
		/// 单元(室号)
		/// </summary>
		public string unitno
		{
			get{ return _unitno;}
			set{ _unitno=value;}
		}
		private decimal? _buildarea;
		/// <summary>
		/// 面积
		/// </summary>
		public decimal? buildarea
		{
			get{ return _buildarea;}
			set{ _buildarea=value;}
		}
		private int? _frontcode;
		/// <summary>
		/// 朝向
		/// </summary>
		public int? frontcode
		{
			get{ return _frontcode;}
			set{ _frontcode=value;}
		}
		private int? _sightcode;
		/// <summary>
		/// 景观
		/// </summary>
		public int? sightcode
		{
			get{ return _sightcode;}
			set{ _sightcode=value;}
		}
		private decimal? _unitprice;
		/// <summary>
		/// 单价
		/// </summary>
		public decimal? unitprice
		{
			get{ return _unitprice;}
			set{ _unitprice=value;}
		}
		private decimal? _saleprice;
		/// <summary>
		/// 售价
		/// </summary>
		public decimal? saleprice
		{
			get{ return _saleprice;}
			set{ _saleprice=value;}
		}
		private decimal? _weight;
		/// <summary>
		/// 权重值
		/// </summary>
		public decimal? weight
		{
			get{ return _weight;}
			set{ _weight=value;}
		}
		private string _photoname;
		public string photoname
		{
			get{ return _photoname;}
			set{ _photoname=value;}
		}
		private string _remark;
		public string remark
		{
			get{ return _remark;}
			set{ _remark=value;}
		}
		private int? _structurecode;
		/// <summary>
		/// 户型结构
		/// </summary>
		public int? structurecode
		{
			get{ return _structurecode;}
			set{ _structurecode=value;}
		}
		private decimal? _totalprice;
		/// <summary>
		/// 总价
		/// </summary>
		public decimal? totalprice
		{
			get{ return _totalprice;}
			set{ _totalprice=value;}
		}
		private int _purposecode = 1002001;
		/// <summary>
		/// 用途
		/// </summary>
		public int purposecode
		{
			get{ return _purposecode;}
			set{ _purposecode=value;}
		}
		private int _isevalue = 1;
		/// <summary>
		/// 是否可估
		/// </summary>
		public int isevalue
		{
			get{ return _isevalue;}
			set{ _isevalue=value;}
		}
		private int _cityid;
		/// <summary>
		/// 城市ID
		/// </summary>
		public int cityid
		{
			get{ return _cityid;}
			set{ _cityid=value;}
		}
		private string _oldid;
		public string oldid
		{
			get{ return _oldid;}
			set{ _oldid=value;}
		}
		private DateTime _createtime = DateTime.Now;
		public DateTime createtime
		{
			get{ return _createtime;}
			set{ _createtime=value;}
		}
		private int _valid = 1;
		public int valid
		{
			get{ return _valid;}
			set{ _valid=value;}
		}
		private DateTime _savedatetime = DateTime.Now;
		public DateTime savedatetime
		{
			get{ return _savedatetime;}
			set{ _savedatetime=value;}
		}
		private string _saveuser;
		public string saveuser
		{
			get{ return _saveuser;}
			set{ _saveuser=value;}
		}
		private int _fxtcompanyid = 25;
		public int fxtcompanyid
		{
			get{ return _fxtcompanyid;}
			set{ _fxtcompanyid=value;}
		}
		private int _isshowbuildingarea = 1;
		/// <summary>
		/// 面积确认
		/// </summary>
		public int isshowbuildingarea
		{
			get{ return _isshowbuildingarea;}
			set{ _isshowbuildingarea=value;}
		}
		private decimal? _innerbuildingarea;
		/// <summary>
		/// 套内面积
		/// </summary>
		public decimal? innerbuildingarea
		{
			get{ return _innerbuildingarea;}
			set{ _innerbuildingarea=value;}
		}
		private int? _subhousetype;
		/// <summary>
		/// 附属房屋类型
		/// </summary>
		public int? subhousetype
		{
			get{ return _subhousetype;}
			set{ _subhousetype=value;}
		}
		private decimal? _subhousearea;
		/// <summary>
		/// 附属房屋面积
		/// </summary>
		public decimal? subhousearea
		{
			get{ return _subhousearea;}
			set{ _subhousearea=value;}
		}
		private string _creator;
		public string creator
		{
			get{ return _creator;}
			set{ _creator=value;}
		}
		private string _nominalfloor;
		/// <summary>
		/// 名义层（实际层）
		/// </summary>
		public string nominalfloor
		{
			get{ return _nominalfloor;}
			set{ _nominalfloor=value;}
		}
		private int? _vdcode;
		/// <summary>
		/// 通风采光
		/// </summary>
		public int? vdcode
		{
			get{ return _vdcode;}
			set{ _vdcode=value;}
		}
		private int? _fitmentcode;
		/// <summary>
		/// 装修
		/// </summary>
		public int? fitmentcode
		{
			get{ return _fitmentcode;}
			set{ _fitmentcode=value;}
		}
		private int? _cookroom;
		/// <summary>
		/// 是否有厨房
		/// </summary>
		public int? cookroom
		{
			get{ return _cookroom;}
			set{ _cookroom=value;}
		}
		private int? _balcony;
		/// <summary>
		/// 阳台数
		/// </summary>
		public int? balcony
		{
			get{ return _balcony;}
			set{ _balcony=value;}
		}
		private int? _toilet;
		/// <summary>
		/// 洗手间数
		/// </summary>
		public int? toilet
		{
			get{ return _toilet;}
			set{ _toilet=value;}
		}
		private int? _noisecode;
		/// <summary>
		/// 噪音情况(2025)
		/// </summary>
		public int? noisecode
		{
			get{ return _noisecode;}
			set{ _noisecode=value;}
		}
	}
}