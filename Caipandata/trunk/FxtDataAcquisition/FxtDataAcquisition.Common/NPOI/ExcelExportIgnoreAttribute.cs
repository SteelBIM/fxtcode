using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Infrastructure.Common.NPOI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelExportIgnoreAttribute : Attribute
    {
        public ExcelExportIgnoreAttribute() { }
        public bool Ignore
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// 指定导出Excel类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelExportIgnoreTypeAttribute : Attribute
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        public ExcelExportType ExportType { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        public string Fomat {get;set;}

        /// <summary>
        /// 指定导出Excel类型
        /// </summary>
        /// <param name="type">导出类型</param>
        public ExcelExportIgnoreTypeAttribute(ExcelExportType type)
        {
            ExportType = type;
            switch (type)
            {
                case ExcelExportType.String:
                    break;
                case ExcelExportType.DataTime:
                    Fomat = "yyyy年mm月dd日";
                    break;
                case ExcelExportType.Numeric:
                    Fomat = "0";
                    break;
                case ExcelExportType.Money:
                    Fomat = "¥#,##0";
                    break;
                case ExcelExportType.Percent:
                    Fomat = "0.00%";
                    break;
                case ExcelExportType.ChineseCapital:
                    Fomat = "[DbNum2][$-804]0";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 指定导出Excel类型
        /// </summary>
        /// <param name="type">导出类型</param>
        /// <param name="type">格式化</param>
        public ExcelExportIgnoreTypeAttribute(ExcelExportType type,string fomat)
        {
            ExportType = type;
            Fomat = fomat;
        }
        public bool Ignore
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// 导出Excel类型
    /// </summary>
    public enum ExcelExportType
    {
        String,
        /// <summary>
        /// 日期/时间
        /// </summary>
        DataTime,
        /// <summary>
        /// 数值
        /// </summary>
        Numeric,
        /// <summary>
        /// 货币
        /// </summary>
        Money,
        /// <summary>
        /// 百分比
        /// </summary>
        Percent,
        /// <summary>
        /// 中文大写
        /// </summary>
        ChineseCapital
    }
}
