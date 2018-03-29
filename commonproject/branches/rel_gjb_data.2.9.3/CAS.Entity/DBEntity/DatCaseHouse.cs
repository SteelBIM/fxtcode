using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Case_House")]
    public class DatCaseHouse : BaseTO
    {
        private long _caseid;
        [SQLField("caseid", EnumDBFieldUsage.PrimaryKey,true)]
        public long caseid
        {
            get { return _caseid; }
            set { _caseid = value; }
        }
        private int _provinceid;
        /// <summary>
        /// 省份
        /// </summary>
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _areaid;
        /// <summary>
        /// 区域
        /// </summary>
        public int areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private int? _subareaid;
        /// <summary>
        /// 片区
        /// </summary>
        public int? subareaid
        {
            get { return _subareaid; }
            set { _subareaid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 公司ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分公司ID
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private string _projectname;
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _casename;
        /// <summary>
        /// 案例名称
        /// </summary>
        public string casename
        {
            get { return _casename; }
            set { _casename = value; }
        }
        private string _purpose;
        /// <summary>
        /// 用途 100135
        /// </summary>
        public string purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private string _buildingarea;
        /// <summary>
        /// 建筑面积
        /// </summary>
        public string buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal? _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
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
        private string _front;
        /// <summary>
        /// 朝向 code 100109
        /// </summary>
        public string front
        {
            get { return _front; }
            set { _front = value; }
        }
        private string _structure;
        /// <summary>
        /// 结构 code 100101
        /// </summary>
        public string structure
        {
            get { return _structure; }
            set { _structure = value; }
        }
        private string _fitment;
        /// <summary>
        /// 装修档次 code 100112
        /// </summary>
        public string fitment
        {
            get { return _fitment; }
            set { _fitment = value; }
        }
        private int _casetype;
        /// <summary>
        /// 案例类型 code 3001
        /// </summary>
        public int casetype
        {
            get { return _casetype; }
            set { _casetype = value; }
        }
        private DateTime _casedate;
        /// <summary>
        /// 案例时间
        /// </summary>
        public DateTime casedate
        {
            get { return _casedate; }
            set { _casedate = value; }
        }
        private DateTime _createdate;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private string _biztype;
        /// <summary>
        /// 业务类型 询价、预评、报告  新增的属于自建
        /// </summary>
        public string biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private long _objectid;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private bool _cbautocalculate=true;
        /// <summary>
        /// 是否联动计算
        /// </summary>
        public bool cbautocalculate
        {
            get { return _cbautocalculate; }
            set { _cbautocalculate = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private string _buildingname;
        /// <summary>
        /// 楼栋名称
        /// </summary>
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private string _floornumber;
        /// <summary>
        /// 楼层
        /// </summary>
        public string floornumber
        {
            get { return _floornumber; }
            set { _floornumber = value; }
        }
        private string _housename;
        /// <summary>
        /// 房号
        /// </summary>
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private string _totalfloor;
        /// <summary>
        /// 总楼层
        /// </summary>
        public string totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private string _buildingdate;
        /// <summary>
        /// 建筑年代
        /// </summary>
        public string buildingdate
        {
            get { return _buildingdate; }
            set { _buildingdate = value; }
        }
        private string _sight;
        /// <summary>
        /// 景观
        /// </summary>
        public string sight
        {
            get { return _sight; }
            set { _sight = value; }
        }
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// x坐标
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// y坐标
        /// </summary>
        public decimal? y { get; set; }
        /// <summary>
        /// 是否带电梯
        /// </summary>
        public int? iselevator { get; set; }
        private string _priceremark;
        /// <summary>
        /// 价格说明
        /// </summary>
        public string priceremark
        {
            get { return _priceremark; }
            set { _priceremark = value; }
        }
        [SQLReadOnly]
        public string cityname { get; set; }
        [SQLReadOnly]
        public string areaname { get; set; }
        [SQLReadOnly]
        public string subareaname { get; set; }
        [SQLReadOnly]
        public int objecttypecode { get; set; }
        [SQLReadOnly]
        public string casetypename { get; set; }
        [SQLReadOnly]
        public string createusername { get; set; }
    }
}