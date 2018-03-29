using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_UserShowTip
    {
        private long _id;
        /// <summary>
        /// 用户是否已经查看了版本更新信息
        /// </summary>
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userid;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _systype = 1000002;
        /// <summary>
        /// 系统类型
        /// </summary>
        public int systype
        {
            get { return _systype; }
            set { _systype = value; }
        }
        private string _version;
        /// <summary>
        /// 版本号
        /// </summary>
        public string version
        {
            get { return _version; }
            set { _version = value; }
        }
        private bool _isshow = true;
        /// <summary>
        /// 是否查看
        /// </summary>
        public bool isshow
        {
            get { return _isshow; }
            set { _isshow = value; }
        }
        private DateTime _createdate = DateTime.Now;
        public DateTime createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }

    }
}
