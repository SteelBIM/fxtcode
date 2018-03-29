using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_FxtCustomer_Contact")]
    public class DatFxtCustomerContact : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private long _custid = 0;
        /// <summary>
        /// 联系人公司
        /// </summary>
        public long custid
        {
            get { return _custid; }
            set { _custid = value; }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _position;
        /// <summary>
        /// 职务
        /// </summary>
        public string position
        {
            get { return _position; }
            set { _position = value; }
        }
        private string _phone1;
        public string phone1
        {
            get { return _phone1; }
            set { _phone1 = value; }
        }
        private string _phone2;
        public string phone2
        {
            get { return _phone2; }
            set { _phone2 = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _qq;
        public string qq
        {
            get { return _qq; }
            set { _qq = value; }
        }
        private string _fax;
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        private bool _issysadmin = false;
        /// <summary>
        /// 是否系统对接人
        /// </summary>
        public bool issysadmin
        {
            get { return _issysadmin; }
            set { _issysadmin = value; }
        }
        private bool _isdataadmin = false;
        /// <summary>
        /// 是否数据对接人
        /// </summary>
        public bool isdataadmin
        {
            get { return _isdataadmin; }
            set { _isdataadmin = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private bool _valid = true;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }
}