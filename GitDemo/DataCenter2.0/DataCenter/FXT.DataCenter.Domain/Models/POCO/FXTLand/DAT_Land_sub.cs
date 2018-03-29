using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_Land_sub
    {
        private long _landid;
        //[SQLField("landid", EnumDBFieldUsage.PrimaryKey)]
        public long landid
        {
            get { return _landid; }
            set { _landid = value; }
        }
        private int _fxtcompanyid;
        //[SQLField("fxtcompanyid", EnumDBFieldUsage.PrimaryKey)]
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _cityid;
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
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
        private int? _subareaid;
        public int? subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private string _landno;
        public string landno
        {
            get { return _landno; }
            set { _landno = value; }
        }
        private string _fieldno = "";
        public string fieldno
        {
            get { return _fieldno; }
            set { _fieldno = value; }
        }
        private string _mapno;
        public string mapno
        {
            get { return _mapno; }
            set { _mapno = value; }
        }
        private string _landname;
        public string landname
        {
            get { return _landname; }
            set { _landname = value; }
        }
        private string _address;
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private int? _landtypecode;
        public int? landtypecode
        {
            get { return _landtypecode; }
            set { _landtypecode = value; }
        }
        private int? _usetypecode;
        public int? usetypecode
        {
            get { return _usetypecode; }
            set { _usetypecode = value; }
        }
        private DateTime? _startdate;
        public DateTime? startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }
        private DateTime? _enddate;
        public DateTime? enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }
        private int? _useyear;
        public int? useyear
        {
            get { return _useyear; }
            set { _useyear = value; }
        }
        private string _planpurpose;
        public string planpurpose
        {
            get { return _planpurpose; }
            set { _planpurpose = value; }
        }
        private string _factpurpose;
        public string factpurpose
        {
            get { return _factpurpose; }
            set { _factpurpose = value; }
        }
        private decimal? _landarea;
        public decimal? landarea
        {
            get { return _landarea; }
            set { _landarea = value; }
        }
        private decimal? _buildingarea;
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal? _cubagerate;
        public decimal? cubagerate
        {
            get { return _cubagerate; }
            set { _cubagerate = value; }
        }
        private decimal? _maxcubagerate;
        public decimal? maxcubagerate
        {
            get { return _maxcubagerate; }
            set { _maxcubagerate = value; }
        }
        private decimal? _mincubagerate;
        public decimal? mincubagerate
        {
            get { return _mincubagerate; }
            set { _mincubagerate = value; }
        }
        private decimal? _coverage;
        public decimal? coverage
        {
            get { return _coverage; }
            set { _coverage = value; }
        }
        private decimal? _maxcoverage;
        public decimal? maxcoverage
        {
            get { return _maxcoverage; }
            set { _maxcoverage = value; }
        }
        private decimal? _greenrage;
        public decimal? greenrage
        {
            get { return _greenrage; }
            set { _greenrage = value; }
        }
        private decimal? _mingreenrage;
        public decimal? mingreenrage
        {
            get { return _mingreenrage; }
            set { _mingreenrage = value; }
        }
        private int? _landshapecode;
        public int? landshapecode
        {
            get { return _landshapecode; }
            set { _landshapecode = value; }
        }
        private int? _developmentcode;
        public int? developmentcode
        {
            get { return _developmentcode; }
            set { _developmentcode = value; }
        }
        private string _landusestatus;
        public string landusestatus
        {
            get { return _landusestatus; }
            set { _landusestatus = value; }
        }
        private int? _landclass;
        public int? landclass
        {
            get { return _landclass; }
            set { _landclass = value; }
        }
        private int? _heightlimited;
        public int? heightlimited
        {
            get { return _heightlimited; }
            set { _heightlimited = value; }
        }
        private string _planlimited;
        public string planlimited
        {
            get { return _planlimited; }
            set { _planlimited = value; }
        }
        private string _east;
        public string east
        {
            get { return _east; }
            set { _east = value; }
        }
        private string _west;
        public string west
        {
            get { return _west; }
            set { _west = value; }
        }
        private string _south;
        public string south
        {
            get { return _south; }
            set { _south = value; }
        }
        private string _north;
        public string north
        {
            get { return _north; }
            set { _north = value; }
        }
        private int? _landownerid;
        public int? landownerid
        {
            get { return _landownerid; }
            set { _landownerid = value; }
        }
        private int? _landuseid;
        public int? landuseid
        {
            get { return _landuseid; }
            set { _landuseid = value; }
        }
        private decimal? _businesscenterdistance;
        public decimal? businesscenterdistance
        {
            get { return _businesscenterdistance; }
            set { _businesscenterdistance = value; }
        }
        private string _traffic;
        public string traffic
        {
            get { return _traffic; }
            set { _traffic = value; }
        }
        private string _infrastructure;
        public string infrastructure
        {
            get { return _infrastructure; }
            set { _infrastructure = value; }
        }
        private string _publicservice;
        public string publicservice
        {
            get { return _publicservice; }
            set { _publicservice = value; }
        }
        private int? _environmentcode;
        public int? environmentcode
        {
            get { return _environmentcode; }
            set { _environmentcode = value; }
        }
        private DateTime? _licencedate;
        public DateTime? licencedate
        {
            get { return _licencedate; }
            set { _licencedate = value; }
        }
        private string _landdetail;
        public string landdetail
        {
            get { return _landdetail; }
            set { _landdetail = value; }
        }
        private decimal _weight = 1M;
        public decimal weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        private decimal? _coefficient;
        public decimal? coefficient
        {
            get { return _coefficient; }
            set { _coefficient = value; }
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
        public int? xyscale
        {
            get { return _xyscale; }
            set { _xyscale = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _creator;
        public string creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private DateTime? _savedate;
        public DateTime? savedate
        {
            get { return _savedate; }
            set { _savedate = value; }
        }
        private string _saveuser;
        public string saveuser
        {
            get { return _saveuser; }
            set { _saveuser = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }
}
