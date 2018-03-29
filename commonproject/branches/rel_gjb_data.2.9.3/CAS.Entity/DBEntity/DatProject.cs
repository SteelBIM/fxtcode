using System;
using CAS.Entity.BaseDAModels;
using System.Collections.Generic;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.DAT_Project")]
    public class DATProject : BaseTO
    {
        private int _projectid;
        [SQLField("projectid", EnumDBFieldUsage.PrimaryKey, true)]
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private string _projectname;
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private int? _subareaid;
        public int? subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        [SQLReadOnly]
        public string subareaname { get; set; }
        private string _fieldno;
        public string fieldno
        {
            get { return _fieldno; }
            set { _fieldno = value; }
        }
        private int _purposecode = 1001001;
        /// <summary>
        /// 主用途
        /// </summary>
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        [SQLReadOnly]
        public string purposename { get; set; }
        [SQLReadOnly]
        public string projectpurposecodename { get; set; }
        private string _address;
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private decimal? _landarea;
        public decimal? landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private DateTime? _startdate;
        public DateTime? startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }
        private int? _usableyear;
        public int? usableyear
        {
            get { return _usableyear; }
            set { _usableyear = value; }
        }
        private decimal? _buildingarea;
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal? _salablearea;
        public decimal? salablearea
        {
            get { return _salablearea; }
            set { _salablearea = value; }
        }
        private decimal? _cubagerate;
        public decimal? cubagerate
        {
            get { return _cubagerate; }
            set { _cubagerate = value; }
        }
        private decimal? _greenrate;
        public decimal? greenrate
        {
            get { return _greenrate; }
            set { _greenrate = value; }
        }
        private DateTime? _buildingdate;
        public DateTime? buildingdate
        {
            get { return _buildingdate; }
            set { _buildingdate = value; }
        }
        private DateTime? _coverdate;
        public DateTime? coverdate
        {
            get { return _coverdate; }
            set { _coverdate = value; }
        }
        private DateTime? _saledate;
        public DateTime? saledate
        {
            get { return _saledate; }
            set { _saledate = value; }
        }
        private DateTime? _joindate;
        public DateTime? joindate
        {
            get { return _joindate; }
            set { _joindate = value; }
        }
        private DateTime? _enddate;
        public DateTime? enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }
        private DateTime? _innersaledate;
        public DateTime? innersaledate
        {
            get { return _innersaledate; }
            set { _innersaledate = value; }
        }
        private int? _rightcode;
        public int? rightcode
        {
            get { return _rightcode; }
            set { _rightcode = value; }
        }
        private int? _parkingnumber;
        public int? parkingnumber
        {
            get { return _parkingnumber; }
            set { _parkingnumber = value; }
        }
        private decimal? _averageprice;
        public decimal? averageprice
        {
            get { return _averageprice; }
            set { _averageprice = value; }
        }
        private string _managertel;
        public string managertel
        {
            get { return _managertel; }
            set { _managertel = value; }
        }
        private string _managerprice;
        public string managerprice
        {
            get { return _managerprice; }
            set { _managerprice = value; }
        }
        private int? _totalnum;
        public int? totalnum
        {
            get { return _totalnum; }
            set { _totalnum = value; }
        }
        private int? _buildingnum;
        public int? buildingnum
        {
            get { return _buildingnum; }
            set { _buildingnum = value; }
        }
        private string _detail;
        public string detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        private int? _buildingtypecode;
        /// <summary>
        /// 主建筑物类型
        /// </summary>
        public int? buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        [SQLReadOnly]
        public string buildingtypename { get; set; }
        private DateTime? _updatedatetime;
        public DateTime? updatedatetime
        {
            get { return _updatedatetime; }
            set { _updatedatetime = value; }
        }
        private decimal? _officearea;
        public decimal? officearea
        {
            get { return _officearea; }
            set { _officearea = value; }
        }
        private decimal? _otherarea;
        public decimal? otherarea
        {
            get { return _otherarea; }
            set { _otherarea = value; }
        }
        private string _planpurpose;
        public string planpurpose
        {
            get { return _planpurpose; }
            set { _planpurpose = value; }
        }
        private DateTime? _pricedate;
        public DateTime? pricedate
        {
            get { return _pricedate; }
            set { _pricedate = value; }
        }
        private int? _iscomplete = 0;
        /// <summary>
        /// 是否已完成
        /// </summary>
        public int? iscomplete
        {
            get { return _iscomplete; }
            set { _iscomplete = value; }
        }
        private string _othername;
        public string othername
        {
            get { return _othername; }
            set { _othername = value; }
        }
        private DateTime _savedatetime = DateTime.Now;
        public DateTime savedatetime
        {
            get { return _savedatetime; }
            set { _savedatetime = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private decimal _weight = 1.0M;
        /// <summary>
        /// RP修正系数
        /// </summary>
        public decimal weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private decimal? _businessarea;
        public decimal? businessarea
        {
            get { return _businessarea; }
            set { _businessarea = value; }
        }
        private decimal? _industryarea;
        public decimal? industryarea
        {
            get { return _industryarea; }
            set { _industryarea = value; }
        }
        private int? _isevalue = 0;
        public int? isevalue
        {
            get { return _isevalue; }
            set { _isevalue = value; }
        }
        private string _pinyin;
        public string pinyin
        {
            get { return _pinyin; }
            set { _pinyin = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _areaid;
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private string _oldid;
        public string oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        private DateTime _createtime = DateTime.Now;
        public DateTime createtime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        private int? _arealineid;
        public int? arealineid
        {
            get { return _arealineid; }
            set { _arealineid = value; }
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
        private string _pinyinall;
        /// <summary>
        /// 楼盘名称全拼
        /// </summary>
        public string pinyinall
        {
            get { return _pinyinall; }
            set { _pinyinall = value; }
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

        private int? _parkingstatus;
        /// <summary>
        /// 停车状况
        /// </summary>
        [SQLReadOnly]
        public int? parkingstatus
        {
            get { return _parkingstatus; }
            set { _parkingstatus = value; }
        }

        private string _east;
        /// <summary>
        /// 东
        /// </summary>
        public string east
        {
            get { return _east; }
            set { _east = value; }
        }

        private string _west;
        /// <summary>
        /// 南
        /// </summary>
        public string west
        {
            get { return _west; }
            set { _west = value; }
        }

        private string _south;
        /// <summary>
        /// 西
        /// </summary>
        public string south
        {
            get { return _south; }
            set { _south = value; }
        }

        private string _north;
        /// <summary>
        /// 北
        /// </summary>
        public string north
        {
            get { return _north; }
            set { _north = value; }
        }

        private int? _managerquality;
        /// <summary>
        /// 物业管理质量
        /// </summary>
        public int? managerquality
        {
            get { return _managerquality; }
            set { _managerquality = value; }
        }

        /// <summary>
        /// 开发商
        /// </summary>
        /// 
        [SQLReadOnly]
        public int developcompanyid { get; set; }
        [SQLReadOnly]
        public string developcompanyname { get; set; }
        [SQLReadOnly]
        public string devecompanyname { get; set; }

        /// <summary>
        /// 物业公司
        /// </summary>
        ///
        [SQLReadOnly]
        public int managercompanyid { get; set; }
        [SQLReadOnly]
        public string managercompanyname { get; set; }

        /// <summary>
        /// 案例条数
        /// </summary>
        [SQLReadOnly]
        public int casecnt { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [SQLReadOnly]
        public string areaname { get; set; }

        public string creator { get; set; }

        [SQLReadOnly]
        public string projectrightcodename { get; set; }
        [SQLReadOnly]
        public string projectmanagerqualityname { get; set; }
        
        [SQLReadOnly]
        public decimal? projectx { get; set; }
        [SQLReadOnly]
        public decimal? projecty { get; set; }
        [SQLReadOnly]
        public List<DATBuilding> buildinglist { get; set; }

    }
}