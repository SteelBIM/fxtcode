using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_User_City_Product
    {
        private int _ucpid;
        /// <summary>
        /// 配置开通用户产品是否开通
        /// </summary>
        //[SQLField("ucpid", EnumDBFieldUsage.PrimaryKey, true)]
        public int ucpid
        {
            get { return _ucpid; }
            set { _ucpid = value; }
        }
        private string _userid;
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _cityid;
        /// <summary>
        /// 城市
        /// </summary>
        public int cityid
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        private int _systypecode;
        /// <summary>
        /// 产品代码
        /// </summary>
        public int systypecode
        {
            get { return _systypecode; }
            set { _systypecode = value; }
        }
        private DateTime? _overdate;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? overdate
        {
            get { return _overdate; }
            set { _overdate = value; }
        }
        private byte? _valid;
        /// <summary>
        /// 有效
        /// </summary>
        public byte? valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
        private byte? _online;
        /// <summary>
        /// 在线状态
        /// </summary>
        public byte? online
        {
            get { return _online; }
            set { _online = value; }
        }
        private DateTime? _lastonlinedate;
        public DateTime? lastonlinedate
        {
            get { return _lastonlinedate; }
            set { _lastonlinedate = value; }
        }

    }
}
