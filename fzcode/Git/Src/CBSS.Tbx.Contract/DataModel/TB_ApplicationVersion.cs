using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TB_ApplicationVersion
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 上传者
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 更新文件地址
        /// </summary>
        public string FileAddress { get; set; }

        /// <summary>
        /// 更新文件MD5值
        /// </summary>
        public string FileMD5 { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool MandatoryUpdate { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        public int VersionID { get; set; }

        /// <summary>
        /// 1-IOS版本；2-Android版本
        /// </summary>
        public int VersionType { get; set; }

        /// <summary>
        /// 应用版本号
        /// </summary>
        public string VersionNumber { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        public string VersionDescription { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        //public int isEnabled { get; set; }
    }
}
