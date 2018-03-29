using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("dbo.SYS_RiZhi")]
    public class SYSRiZhi : BaseTO
    {
        private int _id;
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _username;
        /// <summary>
        /// 用户名
        /// </summary>
        public string username
        {
            get { return _username; }
            set { _username = value; }
        }
        private DateTime _timestr = DateTime.Now;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime timestr
        {
            get { return _timestr; }
            set { _timestr = value; }
        }
        private string _dosomething;
        /// <summary>
        /// 操作内容
        /// </summary>
        public string dosomething
        {
            get { return _dosomething; }
            set { _dosomething = value; }
        }
        private string _ipstr;
        /// <summary>
        /// IP地址
        /// </summary>
        public string ipstr
        {
            get { return _ipstr; }
            set { _ipstr = value; }
        }
        private string _truename;
        public string truename
        {
            get { return _truename; }
            set { _truename = value; }
        }

        public string os { get; set; }
        public string osbit { get; set; }
        public string browser { get; set; }
        public string browserversion { get; set; }
        public string useragent { get; set; }
    }
}