using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.Dat_PhoneMessage")]
    public class DatPhoneMessage : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _cityid = 0;
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _fxtcompanyid = 0;
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private int _companyid = 0;
        public int companyid
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        private string _userid;
        /// <summary>
        /// 收信人
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private string _phoneno;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phoneno
        {
            get { return _phoneno; }
            set { _phoneno = value; }
        }
        private string _text;
        /// <summary>
        /// 信息内容
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        private DateTime _sendtime = DateTime.Now;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime sendtime
        {
            get { return _sendtime; }
            set { _sendtime = value; }
        }
        private int _issucceed = 0;
        /// <summary>
        /// 是否发送成功
        /// </summary>
        public int issucceed
        {
            get { return _issucceed; }
            set { _issucceed = value; }
        }
        private int _objectid = 0;
        /// <summary>
        /// 业务数据ID
        /// </summary>
        public int objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        private int _messcode = 0;
        /// <summary>
        /// 信息类型
        /// </summary>
        public int messcode
        {
            get { return _messcode; }
            set { _messcode = value; }
        }
        private int _typecode = 0;
        /// <summary>
        /// 业务类型
        /// </summary>
        public int typecode
        {
            get { return _typecode; }
            set { _typecode = value; }
        }
        private string _pascode;
        public string pascode
        {
            get { return _pascode; }
            set { _pascode = value; }
        }
        private DateTime? _overdate;
        public DateTime? overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private bool _isused = false;
        /// <summary>
        /// 是否已经使用
        /// </summary>
        public bool isused
        {
            get { return _isused; }
            set { _isused = value; }
        }
        private int _surveyid = 0;
        public int surveyid
        {
            get { return _surveyid; }
            set { _surveyid = value; }
        }
        private string _extracode;
        public string extracode
        {
            get { return _extracode; }
            set { _extracode = value; }
        }
    }
}