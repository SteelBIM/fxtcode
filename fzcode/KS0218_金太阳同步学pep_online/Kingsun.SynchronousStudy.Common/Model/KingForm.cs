using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SynchronousStudy.Common
{
    public class KingForm
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public string RID
        {
            get;
            set;
        }
        /// <summary>
        /// 请求服务关键字
        /// </summary>
        public string SKEY
        {
            get;
            set;
        }
        /// <summary>
        /// 业务数据包
        /// </summary>
        public string Pack
        {
            get;
            set;
        }

    }
}
