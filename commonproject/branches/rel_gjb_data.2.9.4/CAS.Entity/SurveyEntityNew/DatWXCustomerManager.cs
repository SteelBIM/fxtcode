using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.SurveyEntityNew
{
    [Serializable]
    [TableAttribute("dbo.Dat_WXCustomerManager")]
    public class DatWXCustomerManager : BaseTO
    {
        private int _customerid;
        /// <summary>
        /// 客户id
        /// </summary>
        [SQLField("customerid", EnumDBFieldUsage.PrimaryKey, true)]
        public int customerid
        {
            get { return _customerid; }
            set { _customerid = value; }
        }
        private string _customername;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string customername
        {
            get { return _customername; }
            set { _customername = value; }
        }
        private string _telphone;
        /// <summary>
        /// 客户电话
        /// </summary>
        public string telphone
        {
            get { return _telphone; }
            set { _telphone = value; }
        }
        private string _wxopenid;
        /// <summary>
        /// 微信iD
        /// </summary>
        public string wxopenid
        {
            get { return _wxopenid; }
            set { _wxopenid = value; }
        }
        private int? _valid;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
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
        private string _companywxopenid;
        /// <summary>
        /// 公司微信ID
        /// </summary>
        public string companywxopenid
        {
            get { return _companywxopenid; }
            set { _companywxopenid = value; }
        }
        private string _companyname;
        public string companyname
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
    }

}
