using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Privi_User_Group
    {
        private int _ucpid = 0;
        /// <summary>
        /// 用户产品的配置信息Id
        /// </summary>
        public int ucpid
        {
            get { return _ucpid; }
            set { _ucpid = value; }
        }
        private int _groupid = 0;
        /// <summary>
        /// 对应的权限组
        /// </summary>
        public int groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }

    }
}
