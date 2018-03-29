using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtLogDBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_Login")]
    public class SYSLogin : BaseTO
    {
        private int _id;
        /// <summary>
        /// 登录日记表
        /// </summary>
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userid;
        /// <summary>
        /// 用户
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _fxtcompanyid;
        /// <summary>
        /// 公司
        /// </summary>
        public int fxtcompanyid
        {
            get { return _fxtcompanyid; }
            set { _fxtcompanyid = value; }
        }
        private DateTime _logindate;
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime logindate
        {
            get { return _logindate; }
            set { _logindate = value; }
        }
        private DateTime? _logoutdate;
        /// <summary>
        /// 退出时间
        /// </summary>
        public DateTime? logoutdate
        {
            get { return _logoutdate; }
            set { _logoutdate = value; }
        }
        private string _ipaddress;
        /// <summary>
        /// IP地址
        /// </summary>
        public string ipaddress
        {
            get { return _ipaddress; }
            set { _ipaddress = value; }
        }
        private string _pascode;
        /// <summary>
        /// 唯一识别码
        /// </summary>
        public string pascode
        {
            get { return _pascode; }
            set { _pascode = value; }
        }
        private int _systypecode;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private string _browsertype;
        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string browsertype
        {
            get { return _browsertype; }
            set { _browsertype = value; }
        }
        private DateTime? _activetime;
        /// <summary>
        /// 最后在线时间
        /// </summary>
        public DateTime? activetime
        {
            get { return _activetime; }
            set { _activetime = value; }
        }
    }
}