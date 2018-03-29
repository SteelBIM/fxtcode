using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_QueryReport
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fk_qid;
        /// <summary>
        /// 询价ID
        /// </summary>
        public int fk_qid
        {
            get { return _fk_qid; }
            set { _fk_qid = value; }
        }
        private string _reportno;
        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportno
        {
            get { return _reportno; }
            set { _reportno = value; }
        }
        private int _reporttypecode;
        /// <summary>
        /// 报告类型
        /// </summary>
        public int reporttypecode
        {
            get { return _reporttypecode; }
            set { _reporttypecode = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 评估机构ID
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private string _saleman;
        /// <summary>
        /// 业务员
        /// </summary>
        public string saleman
        {
            get { return _saleman; }
            set { _saleman = value; }
        }
        private string _writer;
        /// <summary>
        /// 报告撰写人
        /// </summary>
        public string writer
        {
            get { return _writer; }
            set { _writer = value; }
        }
        private DateTime? _writedate;
        /// <summary>
        /// 报告撰写时间
        /// </summary>
        public DateTime? writedate
        {
            get { return _writedate; }
            set { _writedate = value; }
        }
        private string _surveyuser;
        /// <summary>
        /// 查勘员
        /// </summary>
        public string surveyuser
        {
            get { return _surveyuser; }
            set { _surveyuser = value; }
        }
        private DateTime? _surveydate;
        /// <summary>
        /// 查勘时间
        /// </summary>
        public DateTime? surveydate
        {
            get { return _surveydate; }
            set { _surveydate = value; }
        }
        private string _surveyremark;
        /// <summary>
        /// 查勘内容
        /// </summary>
        public string surveyremark
        {
            get { return _surveyremark; }
            set { _surveyremark = value; }
        }
        private string _checkuser1;
        /// <summary>
        /// 第一审核人
        /// </summary>
        public string checkuser1
        {
            get { return _checkuser1; }
            set { _checkuser1 = value; }
        }
        private string _checkremark1;
        /// <summary>
        /// 第一审核备注
        /// </summary>
        public string checkremark1
        {
            get { return _checkremark1; }
            set { _checkremark1 = value; }
        }
        private DateTime? _checkdate1;
        /// <summary>
        /// 第一审核时间
        /// </summary>
        public DateTime? checkdate1
        {
            get { return _checkdate1; }
            set { _checkdate1 = value; }
        }
        private int? _checkcode1;
        /// <summary>
        /// 第一审核结果ID
        /// </summary>
        public int? checkcode1
        {
            get { return _checkcode1; }
            set { _checkcode1 = value; }
        }
        private string _checkuser2;
        /// <summary>
        /// 第二审核人
        /// </summary>
        public string checkuser2
        {
            get { return _checkuser2; }
            set { _checkuser2 = value; }
        }
        private string _checkremark2;
        /// <summary>
        /// 第二审核备注
        /// </summary>
        public string checkremark2
        {
            get { return _checkremark2; }
            set { _checkremark2 = value; }
        }
        private DateTime? _checkdate2;
        /// <summary>
        /// 第二审核时间
        /// </summary>
        public DateTime? checkdate2
        {
            get { return _checkdate2; }
            set { _checkdate2 = value; }
        }
        private int? _checkcode2;
        /// <summary>
        /// 第二审核结果ID
        /// </summary>
        public int? checkcode2
        {
            get { return _checkcode2; }
            set { _checkcode2 = value; }
        }
        private string _checkuser3;
        /// <summary>
        /// 第三审核人
        /// </summary>
        public string checkuser3
        {
            get { return _checkuser3; }
            set { _checkuser3 = value; }
        }
        private string _checkremark3;
        /// <summary>
        /// 第三审核备注
        /// </summary>
        public string checkremark3
        {
            get { return _checkremark3; }
            set { _checkremark3 = value; }
        }
        private DateTime? _checkdate3;
        /// <summary>
        /// 第三审核时间
        /// </summary>
        public DateTime? checkdate3
        {
            get { return _checkdate3; }
            set { _checkdate3 = value; }
        }
        private int? _checkcode3;
        /// <summary>
        /// 第三审核结果ID
        /// </summary>
        public int? checkcode3
        {
            get { return _checkcode3; }
            set { _checkcode3 = value; }
        }
        private int? _reportstatecode;
        /// <summary>
        /// 报告状态
        /// </summary>
        public int? reportstatecode
        {
            get { return _reportstatecode; }
            set { _reportstatecode = value; }
        }
        private byte? _reportnumber;
        /// <summary>
        /// 报告份数
        /// </summary>
        public byte? reportnumber
        {
            get { return _reportnumber; }
            set { _reportnumber = value; }
        }
        private int? _chargetypecode;
        /// <summary>
        /// 收款情况ID
        /// </summary>
        public int? chargetypecode
        {
            get { return _chargetypecode; }
            set { _chargetypecode = value; }
        }
        private decimal? _pricetitular;
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal? pricetitular
        {
            get { return _pricetitular; }
            set { _pricetitular = value; }
        }
        private decimal? _pricereally;
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal? pricereally
        {
            get { return _pricereally; }
            set { _pricereally = value; }
        }
        private DateTime? _validdate;
        /// <summary>
        /// 报告有效期
        /// </summary>
        public DateTime? validdate
        {
            get { return _validdate; }
            set { _validdate = value; }
        }
        private string _reportfile;
        /// <summary>
        /// 报告附件
        /// </summary>
        public string reportfile
        {
            get { return _reportfile; }
            set { _reportfile = value; }
        }
        private byte _isreturn = ((0));
        /// <summary>
        /// 是否退件
        /// </summary>
        public byte isreturn
        {
            get { return _isreturn; }
            set { _isreturn = value; }
        }
        private string _reportremark;
        /// <summary>
        /// 报告备注
        /// </summary>
        public string reportremark
        {
            get { return _reportremark; }
            set { _reportremark = value; }
        }
        private DateTime _createdate = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private DateTime? _updatetime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? updatetime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }
        private string _updateuser;
        /// <summary>
        /// 修改用户
        /// </summary>
        public string updateuser
        {
            get { return _updateuser; }
            set { _updateuser = value; }
        }
        private byte _valid = ((1));
        /// <summary>
        /// 是否有效
        /// </summary>
        public byte valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

    }
}
