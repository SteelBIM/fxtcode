using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_BONAFinancial")]
    public class DatObjectBONAFinancial : BaseTO
    {
        private long _objectid;
        /// <summary>
        /// 委估对象ID
        /// </summary>
        [SQLField("objectid", EnumDBFieldUsage.PrimaryKey,IsIdentify=false)]
        public long objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
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
        private long _entrustid;
        /// <summary>
        /// 业务编号
        /// </summary>
        public long entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }
        private string _entrusttypecode;
        /// <summary>
        /// 业务类型编码(M 代表按揭 H 代表抵业务类型编码 M 代表按揭 H 代表抵押
        /// </summary>
        public string entrusttypecode
        {
            get { return _entrusttypecode; }
            set { _entrusttypecode = value; }
        }
        private string _buyhouseowner;
        /// <summary>
        /// 买房人姓名
        /// </summary>
        public string buyhouseowner
        {
            get { return _buyhouseowner; }
            set { _buyhouseowner = value; }
        }
        private string _buyhouseownerphone;
        /// <summary>
        /// 买房人联系方式
        /// </summary>
        public string buyhouseownerphone
        {
            get { return _buyhouseownerphone; }
            set { _buyhouseownerphone = value; }
        }
        private string _buyhouseowneridcard;
        /// <summary>
        /// 买房人身份证号
        /// </summary>
        public string buyhouseowneridcard
        {
            get { return _buyhouseowneridcard; }
            set { _buyhouseowneridcard = value; }
        }
        private string _buyhouseregioer;
        /// <summary>
        /// 买房人户籍
        /// </summary>
        public string buyhouseregioer
        {
            get { return _buyhouseregioer; }
            set { _buyhouseregioer = value; }
        }
        private string _housecertno;
        /// <summary>
        /// 房产证号
        /// </summary>
        public string housecertno
        {
            get { return _housecertno; }
            set { _housecertno = value; }
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
        private DateTime? _housecertdate;
        /// <summary>
        /// 房产证登记日期
        /// </summary>
        public DateTime? housecertdate
        {
            get { return _housecertdate; }
            set { _housecertdate = value; }
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
        private decimal? _shoufuamount;
        /// <summary>
        /// 首付金额
        /// </summary>
        public decimal? shoufuamount
        {
            get { return _shoufuamount; }
            set { _shoufuamount = value; }
        }
        private decimal? _houseprepareloanamount;
        /// <summary>
        /// 拟贷金额
        /// </summary>
        public decimal? houseprepareloanamount
        {
            get { return _houseprepareloanamount; }
            set { _houseprepareloanamount = value; }
        }
        private string _housepurpose;
        /// <summary>
        /// 使用情况
        /// </summary>
        public string housepurpose
        {
            get { return _housepurpose; }
            set { _housepurpose = value; }
        }
        private int? _roomecount;
        /// <summary>
        /// 室
        /// </summary>
        public int? roomecount
        {
            get { return _roomecount; }
            set { _roomecount = value; }
        }
        private DateTime? _surveyfinishdate;
        /// <summary>
        /// 查勘完成时间
        /// </summary>
        public DateTime? surveyfinishdate
        {
            get { return _surveyfinishdate; }
            set { _surveyfinishdate = value; }
        }
        private string _surveystaff;
        /// <summary>
        /// 查勘员
        /// </summary>
        public string surveystaff
        {
            get { return _surveystaff; }
            set { _surveystaff = value; }
        }
        private string _address;
        /// <summary>
        /// 房产地址
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private int? _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int? _areaid;
        /// <summary>
        /// 行政区
        /// </summary>
        public int? areaid
        {
            get { return _areaid; }
            set { _areaid = value; }
        }
        private string _bankname;
        /// <summary>
        /// 贷款银行
        /// </summary>
        public string bankname
        {
            get { return _bankname; }
            set { _bankname = value; }
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
        private decimal? _housetotalprice;
        /// <summary>
        /// 评估总价
        /// </summary>
        public decimal? housetotalprice
        {
            get { return _housetotalprice; }
            set { _housetotalprice = value; }
        }
        private DateTime _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private string _fulladdress;
        /// <summary>
        /// 完整地址
        /// </summary>
        public string fulladdress
        {
            get { return _fulladdress; }
            set { _fulladdress = value; }
        }
        /// <summary>
        /// 装修价值
        /// </summary>
        private decimal? _decorationvalue;
        public decimal? decorationvalue
        {
            get { return _decorationvalue; }
            set { _decorationvalue = value; }
        }
        private string _developers;
        /// <summary>
        /// 开发商
        /// </summary>
        public string developers
        {
            get { return _developers; }
            set { _developers = value; }
        }
    }
}