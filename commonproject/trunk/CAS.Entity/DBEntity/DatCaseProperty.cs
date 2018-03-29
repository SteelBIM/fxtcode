using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Case_Property")]
    public class DatCaseProperty : BaseTO
    {
        private long _caseid;
        /// <summary>
        /// 案例ID
        /// </summary>
        [SQLField("caseid", EnumDBFieldUsage.PrimaryKey, true)]
        public long caseid
        {
            get { return _caseid; }
            set { _caseid = value; }
        }
        private int _provinceid;
        /// <summary>
        /// 省份ID
        /// </summary>
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _areaid;
        /// <summary>
        /// 区域ID
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
        /// 公司
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分公司
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
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
        private int _casetype;
        /// <summary>
        /// 案例类型 3001
        /// </summary>
        public int casetype
        {
            get { return _casetype; }
            set { _casetype = value; }
        }
        private DateTime _casedate;
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
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _priceremark;
        /// <summary>
        /// 价格说明
        /// </summary>
        public string priceremark
        {
            get { return _priceremark; }
            set { _priceremark = value; }
        }
        /// <summary>
        /// x坐标
        /// </summary>
        public decimal? x { get; set; }
        /// <summary>
        /// y坐标
        /// </summary>
        public decimal? y { get; set; }
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
        /// <summary>
        /// 1:估价宝案例，2：数据中心案例
        /// </summary>
        [SQLReadOnly]
        public int casesource { get; set; }
    }
}