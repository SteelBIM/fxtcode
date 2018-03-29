using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Case_Land")]
    public class DatCaseLand : BaseTO
    {
        private long _caseid;
        [SQLField("caseid", EnumDBFieldUsage.PrimaryKey, true)]
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
        /// 用途 1002
        /// </summary>
        public string purpose
        {
            get { return _purpose; }
            set { _purpose = value; }
        }
        private string _fieldno;
        /// <summary>
        /// 宗地号
        /// </summary>
        public string fieldno
        {
            get { return _fieldno; }
            set { _fieldno = value; }
        }
        private string _fieldname;
        /// <summary>
        /// 宗地名称
        /// </summary>
        public string fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }
        private string _buildingarea;
        /// <summary>
        /// 面积
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
        /// 案例类型 code 3001
        /// </summary>
        public int casetype
        {
            get { return _casetype; }
            set { _casetype = value; }
        }
        private int _dealtype;
        /// <summary>
        /// 土地出让形式 code 3004
        /// </summary>
        public int dealtype
        {
            get { return _dealtype; }
            set { _dealtype = value; }
        }
        private int _dealstatus;
        /// <summary>
        /// 成交状态 code 3003
        /// </summary>
        public int dealstatus
        {
            get { return _dealstatus; }
            set { _dealstatus = value; }
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
        private bool _cbautocalculate;
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
        [SQLReadOnly]
        public string cityname { get; set; }
        [SQLReadOnly]
        public string areaname { get; set; }
        [SQLReadOnly]
        public string subareaname { get; set; }
        [SQLReadOnly]
        public int objecttypecode { get; set; }
        [SQLReadOnly]
        public string dealtypename { get; set; }
        [SQLReadOnly]
        public string dealstatusname { get; set; }
        [SQLReadOnly]
        public string casetypename { get; set; }
        [SQLReadOnly]
        public string createusername { get; set; }
    }
}