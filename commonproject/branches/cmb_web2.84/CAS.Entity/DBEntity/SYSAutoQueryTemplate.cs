using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_AutoQueryTemplate")]
    public class SYSAutoQueryTemplate : BaseTO
    {
        private int _qtid;
        [SQLField("qtid", EnumDBFieldUsage.PrimaryKey, true)]
        public int qtid
        {
            get { return _qtid; }
            set { _qtid = value; }
        }
        private string _qtname;
        public string qtname
        {
            get { return _qtname; }
            set { _qtname = value; }
        }
        private string _biztype;
        /// <summary>
        /// 业务类型，个人、对公
        /// </summary>
        public string biztype
        {
            get { return _biztype; }
            set { _biztype = value; }
        }
        private string _biztypetext;
        /// <summary>
        /// 业务类型，个人、对公
        /// </summary>
        public string biztypetext
        {
            get { return _biztypetext; }
            set { _biztypetext = value; }
        }
        private string _bank;
        /// <summary>
        /// 适用银行
        /// </summary>
        public string bank
        {
            get { return _bank; }
            set { _bank = value; }
        }

        private string _banktext;
        /// <summary>
        /// 适用银行
        /// </summary>
        public string banktext
        {
            get { return _banktext; }
            set { _banktext = value; }
        }
        private string _remark;
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private int _wordfileid;
        public int wordfileid
        {
            get { return _wordfileid; }
            set { _wordfileid = value; }
        }
        private int _createuserid;
        public int createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        private DateTime _createdate;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        private bool _valid;
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private bool _issingle;
        /// <summary>
        /// 是否单套模板
        /// </summary>
        public bool issingle
        {
            get { return _issingle; }
            set { _issingle = value; }
        }
    }
}