using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_FxtCustomer")]
    public class DatFxtCustomer : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _provinceid;
        public int provinceid
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }
        private int? _cityid;
        public int? cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _address;
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        private string _artificialperson;
        /// <summary>
        /// 法人
        /// </summary>
        public string artificialperson
        {
            get { return _artificialperson; }
            set { _artificialperson = value; }
        }
        private string _personincharge;
        /// <summary>
        /// 负责人
        /// </summary>
        public string personincharge
        {
            get { return _personincharge; }
            set { _personincharge = value; }
        }
        private int? _marketingtype;
        /// <summary>
        /// 客户营销状态，code:4101
        /// </summary>
        public int? marketingtype
        {
            get { return _marketingtype; }
            set { _marketingtype = value; }
        }
        private int? _marketinglevel;
        /// <summary>
        /// 客户分级
        /// </summary>
        public int? marketinglevel
        {
            get { return _marketinglevel; }
            set { _marketinglevel = value; }
        }
        private string _origincustomer;
        /// <summary>
        /// 起因客户，如果当前客户由其它客户介绍，介绍客户则为起因客户
        /// </summary>
        public string origincustomer
        {
            get { return _origincustomer; }
            set { _origincustomer = value; }
        }
        private string _description;
        /// <summary>
        /// 客户情况说明
        /// </summary>
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _website;
        public string website
        {
            get { return _website; }
            set { _website = value; }
        }
        private string _phone;
        public string phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        private string _qq;
        public string qq
        {
            get { return _qq; }
            set { _qq = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _createuserid = 0;
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private bool? _valid;
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private int _businesslevel = 0;
        /// <summary>
        /// 资质等级，code:4103
        /// </summary>
        public int businesslevel
        {
            get { return _businesslevel; }
            set { _businesslevel = value; }
        }

        private bool _issigning;
        /// <summary>
        /// 是否签约
        /// </summary>
        public bool issigning
        {
            get { return _issigning; }
            set { _issigning = value; }
        }
        private DateTime? _signingdate;
        /// <summary>
        /// 签约日期
        /// </summary>
        public DateTime? signingdate
        {
            get { return _signingdate; }
            set { _signingdate = value; }
        }
        private byte? _renewmonth;
        /// <summary>
        /// 续约月份
        /// </summary>
        public byte? renewmonth
        {
            get { return _renewmonth; }
            set { _renewmonth = value; }
        }
        private bool _istrained;
        /// <summary>
        /// 是否培训
        /// </summary>
        public bool istrained
        {
            get { return _istrained; }
            set { _istrained = value; }
        }
        private string _csuser;
        /// <summary>
        /// 客服userId
        /// </summary>
        public string csuser
        {
            get { return _csuser; }
            set { _csuser = value; }
        }
        public string businessmanager { get; set; }
        
        public string oldbusinessmanager { get; set; }

        public int signingmoney { get; set; }
    }
}