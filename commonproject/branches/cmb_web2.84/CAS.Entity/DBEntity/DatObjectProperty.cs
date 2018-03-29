using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ObjectProperty")]
    public class DatObjectProperty : BaseTO
    {
        private long _objectid;
        [SQLField("objectid", EnumDBFieldUsage.PrimaryKey)]
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private long _entrustid = 0;
        /// <summary>
        /// 业务编号
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private string _projectname="";
        /// <summary>
        /// 物业名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _address;
        /// <summary>
        /// 物业(土地)位置
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _righttype;
        /// <summary>
        /// 产权性质((2007001))
        /// </summary>
        public string righttype
        {
            get { return _righttype; }
            set { _righttype = value; }
        }
        private string _owner;
        /// <summary>
        /// 权利人
        /// </summary>
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private string _ownerpercentage;
        /// <summary>
        /// 权利人份额
        /// </summary>
        public string ownerpercentage
        {
            get { return _ownerpercentage; }
            set { _ownerpercentage = value; }
        }
        private string _landno;
        /// <summary>
        /// 宗地号
        /// </summary>
        public string landno
        {
            get { return _landno; }
            set { _landno = value; }
        }
        private decimal? _landarea;
        /// <summary>
        /// 宗地面积
        /// </summary>
        public decimal? landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private string _landpurpose;
        /// <summary>
        /// 宗地用途
        /// </summary>
        public string landpurpose
        {
            get { return _landpurpose; }
            set { _landpurpose = value; }
        }
        private string _housepurpose;
        /// <summary>
        /// 房屋用途
        /// </summary>
        public string housepurpose
        {
            get { return _housepurpose; }
            set { _housepurpose = value; }
        }
        private decimal? _innerbuildingarea;
        /// <summary>
        /// 套内面积
        /// </summary>
        public decimal? innerbuildingarea
        {
            get { return _innerbuildingarea; }
            set { _innerbuildingarea = value; }
        }
        private decimal? _landpublicarea;
        /// <summary>
        /// 分摊土地面积
        /// </summary>
        public decimal? landpublicarea
        {
            get { return _landpublicarea; }
            set { _landpublicarea = value; }
        }
        private string _subhousetype = "";
        /// <summary>
        /// 附属房屋类型((1015001))
        /// </summary>
        public string subhousetype
        {
            get { return _subhousetype; }
            set { _subhousetype = value; }
        }
        private decimal? _subhousearea;
        /// <summary>
        /// 附属房屋面积
        /// </summary>
        public decimal? subhousearea
        {
            get { return _subhousearea; }
            set { _subhousearea = value; }
        }
        private DateTime _buildingenddate;
        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime buildingenddate
        {
            get { return _buildingenddate; }
            set { _buildingenddate = value; }
        }
        private int? _landusableyear;
        /// <summary>
        /// 土地使用期限
        /// </summary>
        public int? landusableyear
        {
            get { return _landusableyear; }
            set { _landusableyear = value; }
        }
        private DateTime? _landusadlestart;
        /// <summary>
        /// 土地起始时间
        /// </summary>
        public DateTime? landusadlestart
        {
            get { return _landusadlestart; }
            set { _landusadlestart = value; }
        }
        private DateTime? _landusadleend;
        /// <summary>
        /// 土地结束时间
        /// </summary>
        public DateTime? landusadleend
        {
            get { return _landusadleend; }
            set { _landusadleend = value; }
        }
        private decimal? _oldprice;
        /// <summary>
        /// 登记价
        /// </summary>
        public decimal? oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private string _others;
        /// <summary>
        /// 他项权利摘要
        /// </summary>
        public string others
        {
            get { return _others; }
            set { _others = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creater;
        public string creater
        {
            get { return _creater; }
            set { _creater = value; }
        }
        private DateTime? _savedate;
        public DateTime? savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuserid;
        public string saveuserid
        {
            get { return _saveuserid; }
            set { _saveuserid = value; }
        }
        private decimal? _landunitprice;
        /// <summary>
        /// 土地单价
        /// </summary>
        public decimal? landunitprice
        {
            get { return _landunitprice; }
            set { _landunitprice = value; }
        }
        private decimal? _landtotalprice;
        /// <summary>
        /// 土地总价
        /// </summary>
        public decimal? landtotalprice
        {
            get { return _landtotalprice; }
            set { _landtotalprice = value; }
        }
    }
}