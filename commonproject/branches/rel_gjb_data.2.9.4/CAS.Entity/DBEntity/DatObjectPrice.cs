using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Object_Price")]
    public class DatObjectPrice : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
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
        private int _objecttypecode;
        /// <summary>
        /// 委估对象类别,2018006 报告,2018005 预评
        /// </summary>
        public int objecttypecode
        {
            get { return _objecttypecode; }
            set { _objecttypecode = value; }
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
        private decimal? _tax;
        /// <summary>
        /// 税费
        /// </summary>
        public decimal? tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal? _netprice;
        /// <summary>
        /// 净值
        /// </summary>
        public decimal? netprice
        {
            get { return _netprice; }
            set { _netprice = value; }
        }
        private decimal? _legalpayment;
        /// <summary>
        /// 法定优先受偿款
        /// </summary>
        public decimal? legalpayment
        {
            get { return _legalpayment; }
            set { _legalpayment = value; }
        }
        private DateTime? _biddate;
        /// <summary>
        /// 回价时间
        /// </summary>
        public DateTime? biddate
        {
            get { return _biddate; }
            set { _biddate = value; }
        }
        private decimal? _shouldfilllandprice;
        /// <summary>
        /// 应补地价
        /// </summary>
        public decimal? shouldfilllandprice
        {
            get { return _shouldfilllandprice; }
            set { _shouldfilllandprice = value; }
        }
        private decimal? _liquidityvalue;
        /// <summary>
        /// 强制变现值
        /// </summary>
        public decimal? liquidityvalue
        {
            get { return _liquidityvalue; }
            set { _liquidityvalue = value; }
        }
        private decimal? _liquiditytaxvalue;
        /// <summary>
        /// 强制变现税费额
        /// </summary>
        public decimal? liquiditytaxvalue
        {
            get { return _liquiditytaxvalue; }
            set { _liquiditytaxvalue = value; }
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
        private decimal? _normalprice;
        /// <summary>
        /// 正常单价
        /// </summary>
        public decimal? normalprice
        {
            get { return _normalprice; }
            set { _normalprice = value; }
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
        private string _priceremark;
        /// <summary>
        /// 价格说明
        /// </summary>
        public string priceremark
        {
            get { return _priceremark; }
            set { _priceremark = value; }
        }
        private long? _entrustid;
        /// <summary>
        /// 业务编号
        /// </summary>
        public long? entrustid
        {
            get { return _entrustid; }
            set { _entrustid = value; }
        }

    }

}
