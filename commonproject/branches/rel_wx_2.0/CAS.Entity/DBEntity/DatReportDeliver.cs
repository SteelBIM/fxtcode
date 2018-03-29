using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Report_Deliver")]
    public class DatReportDeliver : BaseTO
    {
        private long _deliverid;
        /// <summary>
        /// 投递ID
        /// </summary>
        [SQLField("deliverid", EnumDBFieldUsage.PrimaryKey, true)]
        public long deliverid
        {
            get { return _deliverid; }
            set { _deliverid = value; }
        }
        private long _reportid;
        /// <summary>
        /// 报告编号,预评编号
        /// </summary>
        public long reportid
        {
            get { return _reportid; }
            set { _reportid = value; }
        }
        private int? _biztype;
        /// <summary>
        /// 业务类型 2018
        /// </summary>
        public int? biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private string _deliveruser;
        /// <summary>
        /// 投递人
        /// </summary>
        public string deliveruser
        {
            get { return _deliveruser; }
            set { _deliveruser = value; }
        }
        private int? _delivermode;
        /// <summary>
        /// 投递方式   对应code1017
        /// </summary>
        public int? delivermode
        {
            get { return _delivermode; }
            set { _delivermode = value; }
        }
        private DateTime? _deliverdate;
        /// <summary>
        /// 投递时间
        /// </summary>
        public DateTime? deliverdate
        {
            get { return _deliverdate; }
            set { _deliverdate = value; }
        }
        private int _isdeliver;
        /// <summary>
        /// 是否已投递  0否 1是
        /// </summary>
        public int isdeliver
        {
            get { return _isdeliver; }
            set { _isdeliver = value; }
        }
        private string _deliveraddress;
        /// <summary>
        /// 投递地址
        /// </summary>
        public string deliveraddress
        {
            get { return _deliveraddress; }
            set { _deliveraddress = value; }
        }
        private string _contactuser;
        /// <summary>
        /// 联系人
        /// </summary>
        public string contactuser
        {
            get { return _contactuser; }
            set { _contactuser = value; }
        }
        private string _contactphone;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string contactphone
        {
            get { return _contactphone; }
            set { _contactphone = value; }
        }
        private string _receiveuser;
        /// <summary>
        /// 签收人
        /// </summary>
        public string receiveuser
        {
            get { return _receiveuser; }
            set { _receiveuser = value; }
        }
        private string _receiverphone;
        /// <summary>
        /// 签收人电话
        /// </summary>
        public string receiverphone
        {
            get { return _receiverphone; }
            set { _receiverphone = value; }
        }
        private DateTime? _receivedate;
        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime? receivedate
        {
            get { return _receivedate; }
            set { _receivedate = value; }
        }
        private string _deliverremark;
        /// <summary>
        /// 投递描述
        /// </summary>
        public string deliverremark
        {
            get { return _deliverremark; }
            set { _deliverremark = value; }
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
    }
}