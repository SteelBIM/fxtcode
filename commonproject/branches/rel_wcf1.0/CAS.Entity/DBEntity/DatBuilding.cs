using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.DAT_Building")]
    public class DATBuilding : BaseTO
    {
        private int _buildingid;
        [SQLField("buildingid", EnumDBFieldUsage.PrimaryKey, true)]
        public int buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private string _buildingname;
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _purposecode;
        public int? purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private int? _structurecode;
        public int? structurecode
        {
            get { return _structurecode; }
            set { _structurecode = value; }
        }
        private int? _buildingtypecode;
        public int? buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        private int? _totalfloor;
        public int? totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private decimal? _floorhigh;
        public decimal? floorhigh
        {
            get { return _floorhigh; }
            set { _floorhigh = value; }
        }
        private string _salelicence;
        public string salelicence
        {
            get { return _salelicence; }
            set { _salelicence = value; }
        }
        private string _elevatorrate;
        public string elevatorrate
        {
            get { return _elevatorrate; }
            set { _elevatorrate = value; }
        }
        private int? _unitsnumber;
        public int? unitsnumber
        {
            get { return _unitsnumber; }
            set { _unitsnumber = value; }
        }
        private int? _totalnumber;
        public int? totalnumber
        {
            get { return _totalnumber; }
            set { _totalnumber = value; }
        }
        private decimal? _totalbuildarea;
        public decimal? totalbuildarea
        {
            get { return _totalbuildarea; }
            set { _totalbuildarea = value; }
        }
        private DateTime? _builddate;
        public DateTime? builddate
        {
            get { return _builddate; }
            set { _builddate = value; }
        }
        private DateTime? _saledate;
        public DateTime? saledate
        {
            get { return _saledate; }
            set { _saledate = value; }
        }
        private decimal? _averageprice;
        public decimal? averageprice
        {
            get { return _averageprice; }
            set { _averageprice = value; }
        }
        private int? _averagefloor;
        public int? averagefloor
        {
            get { return _averagefloor; }
            set { _averagefloor = value; }
        }
        private DateTime? _joindate;
        public DateTime? joindate
        {
            get { return _joindate; }
            set { _joindate = value; }
        }
        private DateTime? _licencedate;
        public DateTime? licencedate
        {
            get { return _licencedate; }
            set { _licencedate = value; }
        }
        private string _othername;
        public string othername
        {
            get { return _othername; }
            set { _othername = value; }
        }
        private decimal _weight = 1M;
        public decimal weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private int _isevalue = 0;
        public int isevalue
        {
            get { return _isevalue; }
            set { _isevalue = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private string _oldid;
        public string oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private decimal? _saleprice;
        public decimal? saleprice
        {
            get { return _saleprice; }
            set { _saleprice = value; }
        }
        private DateTime _savedatetime = DateTime.Now;
        public DateTime savedatetime
        {
            get { return _savedatetime; }
            set { _savedatetime = value; }
        }
        private string _saveuser;
        /// <summary>
        /// save user
        /// </summary>
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private int _locationcode = 2011001;
        /// <summary>
        /// 楼栋位置
        /// </summary>
        public int locationcode
        {
            get { return _locationcode; }
            set { _locationcode = value; }
        }
        private int? _sightcode;
        /// <summary>
        /// 楼栋景观
        /// </summary>
        public int? sightcode
        {
            get { return _sightcode; }
            set { _sightcode = value; }
        }
        private int? _frontcode;
        /// <summary>
        /// 楼栋朝向
        /// </summary>
        public int? frontcode
        {
            get { return _frontcode; }
            set { _frontcode = value; }
        }
        private decimal _structureweight = 0M;
        /// <summary>
        /// 楼栋结构修正价格
        /// </summary>
        public decimal structureweight
        {
            get { return _structureweight; }
            set { _structureweight = value; }
        }
        private decimal _buildingtypeweight = 0M;
        /// <summary>
        /// 建筑类型修正价格
        /// </summary>
        public decimal buildingtypeweight
        {
            get { return _buildingtypeweight; }
            set { _buildingtypeweight = value; }
        }
        private decimal _yearweight = 0M;
        /// <summary>
        /// 年期修正价格
        /// </summary>
        public decimal yearweight
        {
            get { return _yearweight; }
            set { _yearweight = value; }
        }
        private decimal _purposeweight = 0M;
        /// <summary>
        /// 用途修正价格
        /// </summary>
        public decimal purposeweight
        {
            get { return _purposeweight; }
            set { _purposeweight = value; }
        }
        private decimal _locationweight = 0M;
        /// <summary>
        /// 楼栋位置修正价格
        /// </summary>
        public decimal locationweight
        {
            get { return _locationweight; }
            set { _locationweight = value; }
        }
        private decimal _sightweight = 0M;
        /// <summary>
        /// 景观修正价格
        /// </summary>
        public decimal sightweight
        {
            get { return _sightweight; }
            set { _sightweight = value; }
        }
        private decimal _frontweight = 0M;
        /// <summary>
        /// 朝向修正价格
        /// </summary>
        public decimal frontweight
        {
            get { return _frontweight; }
            set { _frontweight = value; }
        }
        private int _fxtcompanyid = 25;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _fxt_companyid = 25;
        [SQLField("fxt_companyid", EnumDBFieldUsage.PrimaryKey, false)]
        public int fxt_companyid
        {
            get { return _fxt_companyid; }
            set { _fxt_companyid = value; }
        }
        private decimal? _x;
        public decimal? x
        {
            get { return _x; }
            set { _x = value; }
        }
        private decimal? _y;
        public decimal? y
        {
            get { return _y; }
            set { _y = value; }
        }
        private int? _xyscale;
        /// <summary>
        /// 比例尺
        /// </summary>
        public int? xyscale
        {
            get { return _xyscale; }
            set { _xyscale = value; }
        }
        private int? _wall;
        /// <summary>
        /// 外墙
        /// </summary>
        public int? wall
        {
            get { return _wall; }
            set { _wall = value; }
        }
        private int? _iselevator;
        /// <summary>
        /// 是否带电梯
        /// </summary>
        public int? iselevator
        {
            get { return _iselevator; }
            set { _iselevator = value; }
        }
        private decimal? _subaverageprice;
        /// <summary>
        /// 附属房屋均价
        /// </summary>
        public decimal? subaverageprice
        {
            get { return _subaverageprice; }
            set { _subaverageprice = value; }
        }
        [SQLReadOnly]
        public decimal projecaverageprice { get; set; }
    }
}