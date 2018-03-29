using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_Report_Deliver")]
    public class DatReportDeliver : BaseTO
    {
        private long _deliverid;
        /// <summary>
        /// 主键ID
        /// </summary>
        [SQLField("deliverid", EnumDBFieldUsage.PrimaryKey, true)]
        public long deliverid
        {
            get { return _deliverid; }
            set { _deliverid = value; }
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
        private int _biztype;
        /// <summary>
        /// 业务类型 2018
        /// </summary>
        public int biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private int? _deliveruserid;
        /// <summary>
        /// 投送人、派送人
        /// </summary>
        public int? deliveruserid
        {
            get { return _deliveruserid; }
            set { _deliveruserid = value; }
        }
        private string _deliverusername;
        /// <summary>
        /// 投送人中文名（冗余字段）
        /// </summary>
        public string deliverusername
        {
            get { return _deliverusername; }
            set { _deliverusername = value; }
        }
        private int? _delivermode;
        /// <summary>
        /// 投送方式 1017
        /// </summary>
        public int? delivermode
        {
            get { return _delivermode; }
            set { _delivermode = value; }
        }
        private int? _delivernumber;
        /// <summary>
        /// 投递数量
        /// </summary>
        public int? delivernumber
        {
            get { return _delivernumber; }
            set { _delivernumber = value; }
        }
        private DateTime? _deliverdate;
        /// <summary>
        /// 派送时间、快递时间、自取时间
        /// </summary>
        public DateTime? deliverdate
        {
            get { return _deliverdate; }
            set { _deliverdate = value; }
        }
        private DateTime? _fristdeliverdate;
        /// <summary>
        /// 第一次派送时间、快递时间、自取时间
        /// </summary>
        public DateTime? fristdeliverdate
        {
            get { return _fristdeliverdate; }
            set { _fristdeliverdate = value; }
        }
        private string _expresscompany;
        /// <summary>
        /// 快递公司
        /// </summary>
        public string expresscompany
        {
            get { return _expresscompany; }
            set { _expresscompany = value; }
        }
        private string _expressorderid;
        /// <summary>
        /// 快递单号
        /// </summary>
        public string expressorderid
        {
            get { return _expressorderid; }
            set { _expressorderid = value; }
        }
        private string _takeuser;
        /// <summary>
        /// 自取人
        /// </summary>
        public string takeuser
        {
            get { return _takeuser; }
            set { _takeuser = value; }
        }
        private string _takeuserphone;
        /// <summary>
        /// 取报告人联系电话
        /// </summary>
        public string takeuserphone
        {
            get { return _takeuserphone; }
            set { _takeuserphone = value; }
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
        private int? _receivefileid;
        /// <summary>
        /// 附件ID，关联附件表
        /// </summary>
        public int? receivefileid
        {
            get { return _receivefileid; }
            set { _receivefileid = value; }
        }
        private string _receivefilename;
        /// <summary>
        /// 签收附件名称（冗余字段）
        /// </summary>
        public string receivefilename
        {
            get { return _receivefilename; }
            set { _receivefilename = value; }
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
        private DateTime? _fristreceivedate;
        /// <summary>
        /// 第一次签收时间
        /// </summary>
        public DateTime? fristreceivedate
        {
            get { return _fristreceivedate; }
            set { _fristreceivedate = value; }
        }
        private string _deliverremark;
        /// <summary>
        /// 投递备注
        /// </summary>
        public string deliverremark
        {
            get { return _deliverremark; }
            set { _deliverremark = value; }
        }
        private int? _returnnumber;
        /// <summary>
        /// 退还数量
        /// </summary>
        public int? returnnumber
        {
            get { return _returnnumber; }
            set { _returnnumber = value; }
        }
        private DateTime? _returndate;
        /// <summary>
        /// 退还时间
        /// </summary>
        public DateTime? returndate
        {
            get { return _returndate; }
            set { _returndate = value; }
        }
        private DateTime? _fristreturndate;
        /// <summary>
        /// 第一次退还时间
        /// </summary>
        public DateTime? fristreturndate
        {
            get { return _fristreturndate; }
            set { _fristreturndate = value; }
        }
        private int? _returnmode;
        /// <summary>
        /// 退还方式 1、自取 2、快递
        /// </summary>
        public int? returnmode
        {
            get { return _returnmode; }
            set { _returnmode = value; }
        }
        private string _returntakeuser;
        /// <summary>
        /// 退还取报告人
        /// </summary>
        public string returntakeuser
        {
            get { return _returntakeuser; }
            set { _returntakeuser = value; }
        }
        private string _returntakeuserphone;
        /// <summary>
        /// 退还取报告人联系电话
        /// </summary>
        public string returntakeuserphone
        {
            get { return _returntakeuserphone; }
            set { _returntakeuserphone = value; }
        }
        private string _returnexpresscompany;
        /// <summary>
        /// 退还快递公司
        /// </summary>
        public string returnexpresscompany
        {
            get { return _returnexpresscompany; }
            set { _returnexpresscompany = value; }
        }
        private string _returnexpressorderid;
        /// <summary>
        /// 退还快递单号
        /// </summary>
        public string returnexpressorderid
        {
            get { return _returnexpressorderid; }
            set { _returnexpressorderid = value; }
        }
        private bool _valid = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        private string _registrar;
        /// <summary>
        /// 登记人
        /// </summary>
        public string registrar
        {
            get { return _registrar; }
            set { _registrar = value; }
        }
        private DateTime? _deliverytime;
        /// <summary>
        /// 投递时间
        /// </summary>
        public DateTime? deliverytime
        {
            get { return _deliverytime; }
            set { _deliverytime = value; }
        }
    }
}