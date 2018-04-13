using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CBSS.Core.Config
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    [Serializable]
    public class DaoConfig : ConfigFileBase
    {
        public DaoConfig()
        {
        }
        #region 序列化属性
        public String Account { get; set; }
        public String Log { get; set; }
        public String Tbx { get; set; }
        public String Crm { get; set; }
        public String OA { get; set; }
        public String IBS { get; set; }
    #endregion
}
}
