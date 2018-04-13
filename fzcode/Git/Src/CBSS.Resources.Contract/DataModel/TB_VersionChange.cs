using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.ResourcesManager.Contract.DataModel
{
    public class TB_VersionChange
    {
        /// <summary>
        /// 模块版本号
        /// </summary>
        public string ModuleVersion { get; set; }

        /// <summary>
        /// 更新描述
        /// </summary>
        public string UpdateDescription { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool? IsUpdate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 增量包MD5
        /// </summary>
        public string IncrementalPacketMD5 { get; set; }

        /// <summary>
        /// 一级标题
        /// </summary>
        public string FirstTitle { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        public int? SecondTitleID { get; set; }

        /// <summary>
        /// 二级标题
        /// </summary>
        public string SecondTitle { get; set; }

        /// <summary>
        /// 模块地址
        /// </summary>
        public string ModuleAddress { get; set; }

        /// <summary>
        /// MD5值
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// 增量包地址
        /// </summary>
        public string IncrementalPacketAddress { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 主模块ID
        /// </summary>
        public int? ModuleID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int? BooKID { get; set; }

        /// <summary>
        /// 教材名称
        /// </summary>
        public string TeachingNaterialName { get; set; }

        /// <summary>
        /// 模块名
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        public int? FirstTitleID { get; set; }
    }
}
