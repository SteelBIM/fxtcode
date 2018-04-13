using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class com_group
    {
        public int groupid
        {
            get;
            set;
        }

        public string groupname
        {
            get;
            set;
        }

        /// <summary>
        /// 数据查看查看权限
        /// </summary>
        public int dataauthority
        {
            get;
            set;
        }

        public string description
        {
            get;
            set;
        }

        public string creatername
        {
            get;
            set;
        }

        public string createtime
        {
            get;
            set;
        }
        public int delflg
        {
            get;
            set;
        }
    }
}
