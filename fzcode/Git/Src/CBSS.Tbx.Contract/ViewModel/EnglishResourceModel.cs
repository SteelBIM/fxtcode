using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.ViewModel
{
    public class EnglishResourceModel
    {        
        /// <summary>
        /// 密钥ID
        /// </summary>
        public string secretKeyId { get; set; }
    
        /// <summary>
        /// 目录ID
        /// </summary>
        public int catalogueId { get; set; }

        public int FirstTitleID { get; set; }

        public string SecondTitleID { get; set; }

        public int moduleID { get; set; }

        public int bookId { get; set; }
        /// <summary>
        /// 30,50,80 三种规格，代表是原始单词图片的30%，50%，80%的质量比例
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 同步听：0课本1练习册
        /// </summary>
        public int? type { get; set; }

        /// <summary>
        /// 一级模块ID
        /// </summary>
        public int FirstModularID { get; set; }

        /// <summary>
        /// 二级模块ID
        /// </summary>
        public int SecondModularID { get; set; }

    }

    public class EnglishSpokenModel {
        public int id { get; set; }
        public int page { get; set; }

        public int rowNum { get; set; }

        public int type { get; set; }

        public int flag { get; set; }
    }
}
