using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_QueryTax
    {
        private int _queryid;
        //[SQLField("queryid", EnumDBFieldUsage.PrimaryKey)]
        public int queryid
        {
            get { return _queryid; }
            set { _queryid = value; }
        }
        private int _cityid;
       // [SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司ID，0表示公共的税费设置
        /// </summary>
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private decimal _buildingarea;
        public decimal buildingarea
        {
            get { return _buildingarea; }
            set { _buildingarea = value; }
        }
        private decimal _totalprice;
        public decimal totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private decimal _oldprice;
        public decimal oldprice
        {
            get { return _oldprice; }
            set { _oldprice = value; }
        }
        private decimal _netvalue;
        public decimal netvalue
        {
            get { return _netvalue; }
            set { _netvalue = value; }
        }
        private int _iscompany;
        /// <summary>
        /// 产权，0表示个人，1表示企业
        /// </summary>
        public int iscompany
        {
            get { return _iscompany; }
            set { _iscompany = value; }
        }
        private decimal _deedtax;
        /// <summary>
        /// 契税
        /// </summary>
        public decimal deedtax
        {
            get { return _deedtax; }
            set { _deedtax = value; }
        }
        private string _deeddetail;
        public string deeddetail
        {
            get { return _deeddetail; }
            set { _deeddetail = value; }
        }
        private decimal _businesstax;
        /// <summary>
        /// 营业税
        /// </summary>
        public decimal businesstax
        {
            get { return _businesstax; }
            set { _businesstax = value; }
        }
        private string _businessdetail;
        public string businessdetail
        {
            get { return _businessdetail; }
            set { _businessdetail = value; }
        }
        private decimal _citytax;
        /// <summary>
        /// 城市建设税
        /// </summary>
        public decimal citytax
        {
            get { return _citytax; }
            set { _citytax = value; }
        }
        private string _citytaxdetail;
        public string citytaxdetail
        {
            get { return _citytaxdetail; }
            set { _citytaxdetail = value; }
        }
        private decimal _educationtax;
        /// <summary>
        /// 教育附加税
        /// </summary>
        public decimal educationtax
        {
            get { return _educationtax; }
            set { _educationtax = value; }
        }
        private string _educationdetail;
        public string educationdetail
        {
            get { return _educationdetail; }
            set { _educationdetail = value; }
        }
        private decimal _stamptax;
        /// <summary>
        /// 印花税
        /// </summary>
        public decimal stamptax
        {
            get { return _stamptax; }
            set { _stamptax = value; }
        }
        private string _stampdetail;
        public string stampdetail
        {
            get { return _stampdetail; }
            set { _stampdetail = value; }
        }
        private decimal _incometax;
        /// <summary>
        /// 所得税
        /// </summary>
        public decimal incometax
        {
            get { return _incometax; }
            set { _incometax = value; }
        }
        private string _incomedetail;
        public string incomedetail
        {
            get { return _incomedetail; }
            set { _incomedetail = value; }
        }
        private decimal _landtax;
        /// <summary>
        /// 土地增值税
        /// </summary>
        public decimal landtax
        {
            get { return _landtax; }
            set { _landtax = value; }
        }
        private string _landdetail;
        public string landdetail
        {
            get { return _landdetail; }
            set { _landdetail = value; }
        }
        private decimal _disposaltax;
        /// <summary>
        /// 处置税
        /// </summary>
        public decimal disposaltax
        {
            get { return _disposaltax; }
            set { _disposaltax = value; }
        }
        private string _disposaldetail;
        public string disposaldetail
        {
            get { return _disposaldetail; }
            set { _disposaldetail = value; }
        }
        private decimal _islandrate;
        /// <summary>
        /// 土地增值税征收方式
        /// </summary>
        public decimal islandrate
        {
            get { return _islandrate; }
            set { _islandrate = value; }
        }
        private decimal _transactiontax;
        /// <summary>
        /// 房产交易税(元/平方米)
        /// </summary>
        public decimal transactiontax
        {
            get { return _transactiontax; }
            set { _transactiontax = value; }
        }
        private string _transactiondetail;
        public string transactiondetail
        {
            get { return _transactiondetail; }
            set { _transactiondetail = value; }
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
        private int _taxtype = 1;
        /// <summary>
        /// 税费类型:0深圳模式，1有登记价，2无登记价
        /// </summary>
        public int taxtype
        {
            get { return _taxtype; }
            set { _taxtype = value; }
        }
        private string _taxdetail;
        public string taxdetail
        {
            get { return _taxdetail; }
            set { _taxdetail = value; }
        }
        private string _taxselect = "0";
        /// <summary>
        /// 选择的税费条件:非普通住宅，企业，满5年，生活唯一用房，首次购房
        /// </summary>
        public string taxselect
        {
            get { return _taxselect; }
            set { _taxselect = value; }
        }
        private decimal _totaltax;
        /// <summary>
        /// 税费总额
        /// </summary>
        public decimal totaltax
        {
            get { return _totaltax; }
            set { _totaltax = value; }
        }

    }
}
