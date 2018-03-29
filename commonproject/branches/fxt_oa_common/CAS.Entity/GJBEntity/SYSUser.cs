using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_User : SYSUser
    {
        [SQLReadOnly]
        public SYSUserCert usercert { get; set; }
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        public int subcityid { get; set; }
        [SQLReadOnly]
        public DateTime certvalidate { get; set; }
        [SQLReadOnly]
        public int remainedday { get; set; }
        [SQLReadOnly]
        public string certtype { get; set; }
        [SQLReadOnly]
        public int isremind { get; set; }
        /// <summary>
        /// id+证书类型组合 1房地产 2 土地 3资产
        /// </summary>
        [SQLReadOnly]
        public string idkey { get; set; }
        /// <summary>
        /// 识别
        /// </summary>
        [SQLReadOnly]
        public int isspot { get; set; }
    }

    /// <summary>
    /// 是否初始化加载完成
    /// </summary>
    [Serializable]
    public class UserActiveLoaded
    {
        public bool Message { get; set; }
        public bool WaitDo { get; set; }
        public bool AboutMe { get; set; }
    }
}
