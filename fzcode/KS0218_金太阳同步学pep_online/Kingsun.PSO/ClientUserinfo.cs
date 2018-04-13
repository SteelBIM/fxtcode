using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.PSO
{
    public class ClientUserinfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名

        /// </summary>
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 用户在线列表编号
        /// </summary>
        public string UserNumber
        {
            get;
            set;
        }

        public string UserAreaInfo
        {
            get;
            set;
        }

        public string Depth
        {
            get;
            set;
        }

        public ClientUserinfo()
        {
        }

        public ClientUserinfo(string userID, string userName, string userNum)
        {
            UserID = userID;
            UserName = userName;
            UserNumber = userNum;
        }
    }
}
