using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_Email")]
    public class SYSEmail : BaseTO
    {
        private long _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _title = "";
        /// <summary>
        /// 标题
        /// </summary>
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _sender = "";
        /// <summary>
        /// 发件人
        /// </summary>
        public string sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        private string _cc;
        /// <summary>
        /// 抄送
        /// </summary>
        public string cc
        {
            get { return _cc; }
            set { _cc = value; }
        }
        private string _recipients = "";
        /// <summary>
        /// 收件人
        /// </summary>
        public string recipients
        {
            get { return _recipients; }
            set { _recipients = value; }
        }
        private bool _issendalong = false;
        /// <summary>
        /// 是否单独发送
        /// </summary>
        public bool issendalong
        {
            get { return _issendalong; }
            set { _issendalong = value; }
        }
        private bool _issend = false;
        /// <summary>
        /// 是否已发送
        /// </summary>
        public bool issend
        {
            get { return _issend; }
            set { _issend = value; }
        }
        private DateTime? _sendtime;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? sendtime
        {
            get { return _sendtime; }
            set { _sendtime = value; }
        }
        private string _content;
        /// <summary>
        /// 邮件正文
        /// </summary>
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        private DateTime? _lasteditdate;
        /// <summary>
        /// 最后编辑
        /// </summary>
        public DateTime? lasteditdate
        {
            get { return _lasteditdate; }
            set { _lasteditdate = value; }
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
        private int? _createuserid;
        /// <summary>
        /// 创建人
        /// </summary>
        public int? createuserid
        {
            get { return _createuserid; }
            set { _createuserid = value; }
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
    }
}