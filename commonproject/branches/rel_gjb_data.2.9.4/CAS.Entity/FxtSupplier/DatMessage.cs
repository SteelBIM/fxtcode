using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.Dat_Message")]
    public class DatMessage : BaseTO
    {
        private int _messageid;
        /// <summary>
        /// 标示Id
        /// </summary>
        [SQLField("messageid", EnumDBFieldUsage.PrimaryKey, true)]
        public int messageid
        {
            get { return _messageid; }
            set { _messageid = value; }
        }
        private string _messagecontent;
        /// <summary>
        /// 消息内容
        /// </summary>
        public string messagecontent
        {
            get { return _messagecontent; }
            set { _messagecontent = value; }
        }
        private string _fromusername;
        /// <summary>
        /// 发送人账号
        /// </summary>
        public string fromusername
        {
            get { return _fromusername; }
            set { _fromusername = value; }
        }
        private string _tousername;
        /// <summary>
        /// 接收人账号
        /// </summary>
        public string tousername
        {
            get { return _tousername; }
            set { _tousername = value; }
        }
        private string _fromtruename;
        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string fromtruename
        {
            get { return _fromtruename; }
            set { _fromtruename = value; }
        }
        private string _source;
        /// <summary>
        /// 来消息来源
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private bool _isread = false;
        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool isread
        {
            get { return _isread; }
            set { _isread = value; }
        }
        private bool _valid =true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool valid
        {
            get { return _valid; }
            set { _valid = value; }
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
        private int _messagetype = 0;
        /// <summary>
        /// 0-文本消息；1-文件消息
        /// </summary>
        public int messagetype
        {
            get { return _messagetype; }
            set { _messagetype = value; }
        }
        private int _messagespecies = 0;
        /// <summary>
        /// 消息种类:0-业务消息
        /// </summary>
        public int messagespecies
        {
            get { return _messagespecies; }
            set { _messagespecies = value; }
        }
        private int? _objectid;
        /// <summary>
        /// 受理业务的ID:-1公共消息
        /// </summary>
        public int? objectid
        {
            get { return _objectid; }
            set { _objectid = value; }
        }

    }
}
