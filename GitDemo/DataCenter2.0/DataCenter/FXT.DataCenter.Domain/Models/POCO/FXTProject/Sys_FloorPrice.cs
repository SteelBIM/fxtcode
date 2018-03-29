using FXT.DataCenter.Infrastructure.Common.NPOI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Sys_FloorPrice
    {
        [ExcelExportIgnoreAttribute]
        public int id { get; set; }
        [ExcelExportIgnoreAttribute]
        public int CityId { get; set; }
        [ExcelExportIgnoreAttribute]
        public int fxtcompanyid { get; set; }

        private int 总楼层开始;
        [DisplayName("总楼层开始")]
        [ExcelExportIgnoreTypeAttribute(ExcelExportType.Numeric)]
        public int StartTotalFloor
        {
            get
            {
                return 总楼层开始;
            }
            set
            {
                总楼层开始 = value;
            }
        }

        private int 总楼层结束;
        [DisplayName("总楼层结束")]
        [ExcelExportIgnoreTypeAttribute(ExcelExportType.Numeric)]
        public int EndTotalFloor { get { return 总楼层结束; } set { 总楼层结束 = value; } }

        private int 所在楼层;
        [ExcelExportIgnoreTypeAttribute(ExcelExportType.Numeric)]
        [DisplayName("所在楼层")]
        public int CurrFloor { get { return 所在楼层; } set { 所在楼层 = value; } }

        private decimal 楼层差;
        [DisplayName("楼层差_百分比")]
        public decimal FloorDifference { get { return 楼层差; } set { 楼层差 = value; } }
        
        private int 是否带电梯;
        [ExcelExportIgnoreTypeAttribute(ExcelExportType.Numeric)]
        [DisplayName("是否带电梯")]
        public int IsLift { get { return 是否带电梯; } set { 是否带电梯 = value; } }

    }
}
