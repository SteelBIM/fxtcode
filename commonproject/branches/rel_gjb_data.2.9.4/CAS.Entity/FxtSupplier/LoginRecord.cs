using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.LoginRecord")]
    public class LoginRecord : BaseTO
    {
        private int _id;
        /// <summary>
        /// id
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _username;
        /// <summary>
        /// 账号
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private DateTime? _logindate;
        /// <summary>
        /// 登录/退出时间
        /// </summary>
        public DateTime? logindate
        {
            get { return _logindate; }
            set { _logindate = value; }
        }
        private int? _loginstate;
        /// <summary>
        /// 登录/退出(1/0)
        /// </summary>
        public int? loginstate
        {
            get { return _loginstate; }
            set { _loginstate = value; }
        }
        private string _source;
        /// <summary>
        /// 来源
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _webip;
        /// <summary>
        /// 访问IP
        /// </summary>
        public string webip
        {
            get { return _webip; }
            set { _webip = value; }
        }
    }

}
