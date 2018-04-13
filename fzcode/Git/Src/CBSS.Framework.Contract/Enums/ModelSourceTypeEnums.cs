using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ModelSourceTypeEnums
    {
        /// <summary>
        /// 单词
        /// </summary>
        [System.ComponentModel.Description("单词")]
        Word = 1,

        /// <summary>
        /// 课文朗读
        /// </summary>
        [System.ComponentModel.Description("课文朗读")]
        Article = 2,

        /// <summary>
        /// 课文朗读
        /// </summary>
        [System.ComponentModel.Description("课文朗读")]
        Sentence = 3,

        /// <summary>
        /// 电子书
        /// </summary>
        [System.ComponentModel.Description("电子书")]
        DigitalBook = 4,

        /// <summary>
        /// 同步听
        /// </summary>
        [System.ComponentModel.Description("同步听")]
        Listen = 5
    }
}
