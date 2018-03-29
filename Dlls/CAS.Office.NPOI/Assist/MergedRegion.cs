using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

///该名称空间下专门存放辅助类，用来帮助EXCEL与WORD之间的转换使用
namespace CAS.Office.NPOI.Assist
{
    /// <summary>
    /// 合并的区域
    /// byte-侯湘岳 2015-04-09
    /// </summary>
    internal class MergedRegion
    {
        /// <summary>
        /// 开始行索引
        /// </summary>
        public int StartRowIndex { get; set; }
        /// <summary>
        /// 结束行索引
        /// </summary>
        public int EndRowIndex { get; set; }
        /// <summary>
        /// 开始列索引
        /// </summary>
        public int StartCellIndex { get; set; }
        /// <summary>
        /// 结束列索引
        /// </summary>
        public int EndCellIndex { get; set; }
        /// <summary>
        /// 合并方式
        /// </summary>
        public EnumMergeType MergeType { get; set; }

    }

    internal enum EnumMergeType
    {
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal,
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical
    }
}
