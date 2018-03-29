using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_FxtCustomer_Followup")]
    public class DatFxtCustomerFollowup : BaseTO
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
        /// 客户
        /// </summary>
        public long custid
        {
            get { return _custid; }
            set { _custid = value; }
        }
        private int _createuserid;
        /// <summary>
        /// 跟进人
        /// </summary>
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 跟进日期
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private int _followuptype;
        /// <summary>
        /// 跟进类型，code:4102
        /// </summary>
        public int followuptype
        {
            get { return _followuptype; }
            set { _followuptype = value; }
        }
        private string _remark;
        /// <summary>
        /// 跟进内容
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private bool? _valid;
        public bool? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
    }

}