using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.DAT_QueryHistory")]
    public class DATCASQueryHistory : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _customerid;
        public int customerid
        {
            get { return _customerid; }
            set { _customerid = value; }
        }
        private int _projectid;
        public int projectid
        {
            get { return _projectid; }
            set { _projectid = value; }
        }
        private int? _buildingid;
        public int? buildingid
        {
            get { return _buildingid; }
            set { _buildingid = value; }
        }
        private int? _houseid;
        public int? houseid
        {
            get { return _houseid; }
            set { _houseid = value; }
        }
        private string _projectname;
        public string projectname
        {
            get { return _projectname; }
            set { _projectname = value; }
        }
        private string _buildingname;
        public string buildingname
        {
            get { return _buildingname; }
            set { _buildingname = value; }
        }
        private string _housename;
        public string housename
        {
            get { return _housename; }
            set { _housename = value; }
        }
        private DateTime _querydate = DateTime.Now;
        /// <summary>
        /// 询价时间
        /// </summary>
        public DateTime querydate
        {
            get { return _querydate; }
            set { _querydate = value; }
        }
        private decimal? _unitprice;
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private decimal? _totalprice;
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private decimal? _tax;
        /// <summary>
        /// 税费总额
        /// </summary>
        public decimal? tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal? _newprice;
        /// <summary>
        /// 净值
        /// </summary>
        public decimal? newprice
        {
            get { return _newprice; }
            set { _newprice = value; }
        }
        private decimal? _oldprice;
        /// <summary>
        /// 原购价
        /// </summary>
        public decimal? oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private decimal? _buildingarea;
        public decimal? buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private int _systypecode = 1003001;
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private DateTime? _casestartdate;
        /// <summary>
        /// 案例开始时间
        /// </summary>
        public DateTime? casestartdate
        {
            get { return _casestartdate; }
            set { _casestartdate = value; }
        }
        private DateTime? _caseenddate;
        /// <summary>
        /// 案例结束时间
        /// </summary>
        public DateTime? caseenddate
        {
            get { return _caseenddate; }
            set { _caseenddate = value; }
        }
        private int? _casenumber;
        /// <summary>
        /// 案例数
        /// </summary>
        public int? casenumber
        {
            get { return _casenumber; }
            set { _casenumber = value; }
        }
        private int? _casemaxprice;
        /// <summary>
        /// 案例最大值
        /// </summary>
        public int? casemaxprice
        {
            get { return _casemaxprice; }
            set { _casemaxprice = value; }
        }
        private int? _caseminprice;
        /// <summary>
        /// 案例最小值
        /// </summary>
        public int? caseminprice
        {
            get { return _caseminprice; }
            set { _caseminprice = value; }
        }
        private decimal? _eprice;
        /// <summary>
        /// 项目均价
        /// </summary>
        public decimal? eprice
        {
            get { return _eprice; }
            set { _eprice = value; }
        }
        private int _caseavgprice = 0;
        /// <summary>
        /// 案例均价
        /// </summary>
        public int caseavgprice
        {
            get { return _caseavgprice; }
            set { _caseavgprice = value; }
        }
        private decimal? _beprice;
        /// <summary>
        /// 楼栋均价
        /// </summary>
        public decimal? beprice
        {
            get { return _beprice; }
            set { _beprice = value; }
        }
        private decimal? _heprice;
        /// <summary>
        /// 房号估价
        /// </summary>
        public decimal? heprice
        {
            get { return _heprice; }
            set { _heprice = value; }
        }
        private string _ipaddress;
        public string ipaddress
        {
            get { return _ipaddress; }
            set { _ipaddress = value; }
        }
        private int? _companyid;
        /// <summary>
        /// 客户单位ID
        /// </summary>
        public int? companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _fileno;
        public string fileno
        {
            get { return _fileno; }
            set { _fileno = value; }
        }
        private decimal? _saleprice;
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal? saleprice
        {
            get { return _saleprice; }
            set { _saleprice = value; }
        }
        private string _taxselect;
        /// <summary>
        /// 税费条件选择:非普通住宅，企业，满5年，生活唯一用房，首次购房
        /// </summary>
        public string taxselect
        {
            get { return _taxselect; }
            set { _taxselect = value; }
        }
        private string _taxdetali;
        /// <summary>
        /// 税费明细
        /// </summary>
        public string taxdetali
        {
            get { return _taxdetali; }
            set { _taxdetali = value; }
        }
        private int _querypurposecode = 1004001;
        /// <summary>
        /// 询价目的
        /// </summary>
        public int querypurposecode
        {
            get { return _querypurposecode; }
            set { _querypurposecode = value; }
        }
        private long? _qid;
        public long? qid
        {
            get { return _qid; }
            set { _qid = value; }
        }
        private int? _subhousetype;
        /// <summary>
        /// 附属房屋类型
        /// </summary>
        public int? subhousetype
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
        private decimal? _subhouseunitprice;
        /// <summary>
        /// 附属房屋单价
        /// </summary>
        public decimal? subhouseunitprice
        {
            get { return _subhouseunitprice; }
            set { _subhouseunitprice = value; }
        }
        private decimal? _subhousetotalprice;
        /// <summary>
        /// 附属房屋总价
        /// </summary>
        public decimal? subhousetotalprice
        {
            get { return _subhousetotalprice; }
            set { _subhousetotalprice = value; }
        }
        private decimal? _discountuintprice;
        public decimal? discountuintprice
        {
            get { return _discountuintprice; }
            set { _discountuintprice = value; }
        }
        private decimal? _discounttotalprice;
        public decimal? discounttotalprice
        {
            get { return _discounttotalprice; }
            set { _discounttotalprice = value; }
        }
        private int? _housetypecode;
        /// <summary>
        /// 户型
        /// </summary>
        public int? housetypecode
        {
            get { return _housetypecode; }
            set { _housetypecode = value; }
        }
        private int? _purposecode;
        /// <summary>
        /// 房屋用途
        /// </summary>
        public int? purposecode
        {
            get { return _purposecode; }
            set { _purposecode = value; }
        }
        private string _nominalfloor;
        /// <summary>
        /// 楼层
        /// </summary>
        public string nominalfloor
        {
            get { return _nominalfloor; }
            set { _nominalfloor = value; }
        }
        private int? _buildingtypecode;
        /// <summary>
        /// 建筑类型
        /// </summary>
        public int? buildingtypecode
        {
            get { return _buildingtypecode; }
            set { _buildingtypecode = value; }
        }
        private int? _totalfloor;
        /// <summary>
        /// 总楼层
        /// </summary>
        public int? totalfloor
        {
            get { return _totalfloor; }
            set { _totalfloor = value; }
        }
        private DateTime? _builddate;
        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime? builddate
        {
            get { return _builddate; }
            set { _builddate = value; }
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
        private string _customercompanyfullname;
        /// <summary>
        /// 公司全称
        /// </summary>
        public string customercompanyfullname
        {
            get { return _customercompanyfullname; }
            set { _customercompanyfullname = value; }
        }
        private string _source;
        /// <summary>
        /// 来源
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _projectfullname;
        public string projectfullname
        {
            get { return _projectfullname; }
            set { _projectfullname = value; }
        }
        [SQLReadOnly]
        public string cityname
        {
            get;
            set;
        }
        [SQLReadOnly]
        public string truename
        {
            get;
            set;
        }
    }
}