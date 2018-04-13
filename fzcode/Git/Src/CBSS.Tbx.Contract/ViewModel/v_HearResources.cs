using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class v_HearResources
    { 
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Picture { get; set; }

        /// <summary>
        /// 跟读次数
        /// </summary>
        public int RepeatNumber { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        public int FirstModularID { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        public int SecondModularID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 子序号
        /// </summary>
        public int TextSerialNumber { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string TextDesc { get; set; }

        /// <summary>
        /// 音频
        /// </summary>
        public string AudioFrequency { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// 一级标题ID
        /// </summary>
        public int FirstTitleID { get; set; }

        /// <summary>
        /// 二级标题ID
        /// </summary>
        public int SecondTitleID { get; set; }

        /// <summary>
        /// 二级模块英文名
        /// </summary>
        public string ModularEN { get; set; }

    }
}
