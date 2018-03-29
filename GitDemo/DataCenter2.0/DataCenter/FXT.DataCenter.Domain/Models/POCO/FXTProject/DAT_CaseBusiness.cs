using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_CaseBusiness
    {
        private int _caseid;
        /// <summary>
        /// ID
        /// </summary>
        public int caseid
        {
            get { return _caseid; }
            set { _caseid = value; }
        }
        private int _projectid;
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _buildingid;
        /// <summary>
        /// 楼宇ID
        /// </summary>
        public int? buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private int? _houseid;
        /// <summary>
        /// 物业ID
        /// </summary>
        public int? houseid
        {
            get { return _houseid; }
            set { _houseid = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private DateTime _casedate = DateTime.Now;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime casedate
        {
            get { return _casedate; }
            set { _casedate = value; }
        }
        private int _purposecode;
        /// <summary>
        /// 用途Code
        /// </summary>
        public int purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private int? _floornumber;
        /// <summary>
        /// 楼层
        /// </summary>
        public int? floornumber
        {
            get { return _floornumber; }
            set { _floornumber = value; }
        }
        private string _housename;
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private decimal _buildingarea;
        /// <summary>
        /// 建筑面积
        /// </summary>
        public decimal buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private int? _frontcode;
        /// <summary>
        /// 朝向Code
        /// </summary>
        public int? frontcode
        {
            get { return _frontcode; }
            set { _frontcode = value; }
        }
        private decimal _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private int _moneyunitcode = 2002001;
        /// <summary>
        /// 货币单位Code
        /// </summary>
        public int moneyunitcode
        {
            get { return _moneyunitcode; }
            set { _moneyunitcode = value; }
        }
        private int? _sightcode;
        /// <summary>
        /// 景观Code
        /// </summary>
        public int? sightcode
        {
            get { return _sightcode; }
            set { _sightcode = value; }
        }
        private int _casetypecode = 3001001;
        /// <summary>
        /// 案例类型
        /// </summary>
        public int casetypecode
        {
            get { return _casetypecode; }
            set { _casetypecode = value; }
        }
        private string _location;
        /// <summary>
        /// 位置
        /// </summary>
        public string location
        {
            get { return _location; }
            set { _location = value; }
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
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private decimal? _totalprice;
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private int? _oldid;
        public int? oldid
        {
            get { return _oldid; }
            set { _oldid = value; }
        }
        private int _cityid = 6;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _updateuser;
        public string updateuser
        {
            get { return _updateuser; }
            set { _updateuser = value; }
        }
        private DateTime? _updatedate;
        public DateTime? updatedate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        private int _valid = 1;
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _fxtcompanyid = 25;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _buildingname;
        /// <summary>
        /// 总层数
        /// </summary>
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private int? _totalfloor;
        /// <summary>
        /// 总层数
        /// </summary>
        public int? totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private int? _remainyear;
        /// <summary>
        /// 剩余年限
        /// </summary>
        public int? remainyear
        {
            get { return _remainyear; }
            set { _remainyear = value; }
        }
        private decimal? _depreciation;
        /// <summary>
        /// 成新率
        /// </summary>
        public decimal? depreciation
        {
            get { return _depreciation; }
            set { _depreciation = value; }
        }
        private decimal? _floorheight;
        /// <summary>
        /// 层高
        /// </summary>
        public decimal? floorheight
        {
            get { return _floorheight; }
            set { _floorheight = value; }
        }
        private int? _businesstypecode;
        public int? businesstypecode
        {
            get { return _businesstypecode; }
            set { _businesstypecode = value; }
        }
        private DateTime? _savedatetime;
        public DateTime? savedatetime
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
        private string _sourcename;
        public string sourcename
        {
            get { return _sourcename; }
            set { _sourcename = value; }
        }
        private string _sourcelink;
        public string sourcelink
        {
            get { return _sourcelink; }
            set { _sourcelink = value; }
        }
        private string _sourcephone;
        public string sourcephone
        {
            get { return _sourcephone; }
            set { _sourcephone = value; }
        }

    }
}
