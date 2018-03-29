using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_Development
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _systemtype;
        /// <summary>
        /// 系统类别(0,iPhone;1,windows)
        /// </summary>
        public int systemtype
        {
            get { return _systemtype; }
            set { _systemtype = value; }
        }
        private DateTime _updatedate;
        /// <summary>
        /// 投票时间
        /// </summary>
        public DateTime updatedate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        private string _ip;
        /// <summary>
        /// 用户IP
        /// </summary>
        public string ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

    }
}
