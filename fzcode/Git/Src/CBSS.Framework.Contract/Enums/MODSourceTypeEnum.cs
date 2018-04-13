using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// MOD资源类型,注：此枚举不能修改，只能新增
    /// </summary>
    public enum MODSourceTypeEnum
    {

        ///// <summary>
        ///// 电子书
        ///// </summary>
        //[System.ComponentModel.Description("电子书")]
        //InnerModel = 1,
        ///// <summary>
        ///// 练习册
        ///// </summary>
        //[System.ComponentModel.Description("练习册")]
        //ExternalModel = 2

        /// <summary>
        /// 请选择
        /// </summary>
        [System.ComponentModel.Description("请选择")]
        PleaseChoose = -1,
        /// <summary>
        /// 单词
        /// </summary>
        [System.ComponentModel.Description("单词")]
        Word = 1,

        /// <summary>
        /// 课文跟读
        /// </summary>
        [System.ComponentModel.Description("课内课文")]
        Article = 2,
    

        /// <summary>
        /// 电子书
        /// </summary>
        [System.ComponentModel.Description("电子书")]
        EBook = 3,

        /// <summary>
        /// 同步听
        /// </summary>
        [System.ComponentModel.Description("同步听")]
        Listen = 4,

        /// <summary>
        /// 练习册
        /// </summary>
        [System.ComponentModel.Description("练习册")]
        Exercise = 5,

        /// <summary>
        /// 口语
        /// </summary>
        [System.ComponentModel.Description("口语")]
        Spoken = 6,

        /// <summary>
        /// 句子跟读
        /// </summary>
        [System.ComponentModel.Description("逐句精读")]
        FollowRead = 7,
        /// <summary>
        /// 练习册
        /// </summary>
        [System.ComponentModel.Description("说说看")]
        HearResource = 8,

        /// <summary>
        /// 口语
        /// </summary>
        [System.ComponentModel.Description("趣配音")]
        IntestingDubbing = 10,

        /// <summary>
        /// 口语
        /// </summary>
        [System.ComponentModel.Description("课内配音")]
        ClassDubbing = 11,

        /// <summary>
        /// 口语
        /// </summary>
        [System.ComponentModel.Description("电影配音")]
        MovieDubbing = 12,
    }
    
}
