using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_TaxSet
    {
        private int _cityid;
        //[SQLField("cityid", EnumDBFieldUsage.PrimaryKey)]
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _companyid;
        /// <summary>
        /// 公司ID，0表示公共的税费设置
        /// </summary>
        //[SQLField("companyid", EnumDBFieldUsage.PrimaryKey)]
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private int _iscompany;
        /// <summary>
        /// 产权，0表示个人，1表示企业
        /// </summary>
        //[SQLField("iscompany", EnumDBFieldUsage.PrimaryKey)]
        public int iscompany
        {
            get { return _iscompany; }
            set { _iscompany = value; }
        }
        private decimal _deedtax;
        /// <summary>
        /// 契税，小于90平米的税率
        /// </summary>
        public decimal deedtax
        {
            get { return _deedtax; }
            set { _deedtax = value; }
        }
        private decimal _deedtax1;
        /// <summary>
        /// 契税，90至144的税率
        /// </summary>
        public decimal deedtax1
        {
            get { return _deedtax1; }
            set { _deedtax1 = value; }
        }
        private decimal _deedtax2;
        /// <summary>
        /// 契税，大于144的税率
        /// </summary>
        public decimal deedtax2
        {
            get { return _deedtax2; }
            set { _deedtax2 = value; }
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
        private decimal _citytax;
        /// <summary>
        /// 城市建设税
        /// </summary>
        public decimal citytax
        {
            get { return _citytax; }
            set { _citytax = value; }
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
        private decimal _stamptax;
        /// <summary>
        /// 印花税
        /// </summary>
        public decimal stamptax
        {
            get { return _stamptax; }
            set { _stamptax = value; }
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
        private decimal _landtax;
        /// <summary>
        /// 土地增值税
        /// </summary>
        public decimal landtax
        {
            get { return _landtax; }
            set { _landtax = value; }
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
        private decimal _landrate1;
        /// <summary>
        /// 深圳土地增值税计算参数1
        /// </summary>
        public decimal landrate1
        {
            get { return _landrate1; }
            set { _landrate1 = value; }
        }
        private decimal _landrate2;
        /// <summary>
        /// 深圳土地增值税计算参数2
        /// </summary>
        public decimal landrate2
        {
            get { return _landrate2; }
            set { _landrate2 = value; }
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

    }
}
