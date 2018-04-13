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
    public class SettingConfig : ConfigFileBase
    {
        public SettingConfig()
        {
        }

        #region 序列化属性
        public String WebSiteTitle { get; set; }
        public String WebSiteDescription { get; set; }
        public String WebSiteKeywords { get; set; }

        public String ModRequestUrl { get; set; }

        public String SubjectsApi { get; set; }

        public String VersionsApi { get; set; }

        public String CatalogApi { get; set; }
        #endregion
    }
}
