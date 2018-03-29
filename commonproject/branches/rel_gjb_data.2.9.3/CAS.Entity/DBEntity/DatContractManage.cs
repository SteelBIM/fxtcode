using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_ContractManage")]
    public class DatContractManage : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _subcompanyid;
        /// <summary>
        /// 分支机构
        /// </summary>
        public int subcompanyid
        {
            get { return _subcompanyid; }
            set { _subcompanyid = value; }
        }
        private string _contractnumber;
        /// <summary>
        /// 合同编号
        /// </summary>
        public string contractnumber
        {
            get { return _contractnumber; }
            set { _contractnumber = value; }
        }
        private DateTime _signeddate;
        /// <summary>
        /// 签订日期
        /// </summary>
        public DateTime signeddate
        {
            get { return _signeddate; }
            set { _signeddate = value; }
        }
        private int _biztype;
        /// <summary>
        /// 业务类型
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int? _reporttypecode;
        /// <summary>
        /// 报告类型
        /// </summary>
        public int? reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private string _reportsubtype;
        /// <summary>
        /// 报告子类型
        /// </summary>
        public string reportsubtype
        {
            get { return _reportsubtype; }
            set { _reportsubtype = value; }
        }
        private string _assesstype;
        /// <summary>
        /// 评估目的
        /// </summary>
        public string assesstype
        {
            get { return _assesstype; }
            set { _assesstype = value; }
        }
        private string _businesssource;
        /// <summary>
        /// 业务来源
        /// </summary>
        public string businesssource
        {
            get { return _businesssource; }
            set { _businesssource = value; }
        }
        private string _bankusername;
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string bankusername
        {
            get { return _bankusername; }
            set { _bankusername = value; }
        }
        private string _owner;
        /// <summary>
        /// 产权人
        /// </summary>
        public string owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private string _clientphone;
        /// <summary>
        /// 客户联系电话
        /// </summary>
        public string clientphone
        {
            get { return _clientphone; }
            set { _clientphone = value; }
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
        private decimal? _deposit;
        /// <summary>
        /// 押金
        /// </summary>
        public decimal? deposit
        {
            get { return _deposit; }
            set { _deposit = value; }
        }
        private DateTime? _receivedate;
        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? receivedate
        {
            get { return _receivedate; }
            set { _receivedate = value; }
        }
        private int? _receiveuserid;
        /// <summary>
        /// 领取人
        /// </summary>
        public int? receiveuserid
        {
            get { return _receiveuserid; }
            set { _receiveuserid = value; }
        }
        private int _contractprogress;
        /// <summary>
        /// 合同进度预评、报告、归档、欠款、完成
        /// </summary>
        public int contractprogress
        {
            get { return _contractprogress; }
            set { _contractprogress = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
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
        private DateTime _createdate;
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

        private int _valid;
        /// <summary>
        /// 是否删除
        /// </summary>
        public int valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }
}