using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Core.Config
{
    /// <summary>
    /// OSS配置
    /// </summary>
    [Serializable]
    public class OSSConfig : ConfigFileBase
    {
        public OSSConfig() { }

        #region 序列化属性
        public string accessKeyId { get; set; }
        public string accessKeySecret { get; set; }
        public string host { get; set; }
        public string bucketName { get; set; } 
        #endregion
    }
}
