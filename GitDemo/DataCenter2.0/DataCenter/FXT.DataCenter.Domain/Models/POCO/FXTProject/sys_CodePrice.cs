using FXT.DataCenter.Infrastructure.Common.NPOI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class sys_CodePrice
    {
        [ExcelExportIgnoreAttribute]
        public int id { get; set; }
        [ExcelExportIgnoreAttribute]
        public int cityid { get; set; }
        [ExcelExportIgnoreAttribute]
        public int fxtcompanyid { get; set; }

        [ExcelExportIgnoreAttribute]
        public int? code { get; set; }

        public string codename { get; set; }

        [ExcelExportIgnoreAttribute]
        public int? subcode { get; set; }

        public string SubCodeName { get; set; }

        public decimal price { get; set; }

        //[DisplayName("修正系数_百分比")]
        //public decimal pricePercent { get { return (price / 100); } }

        [ExcelExportIgnoreAttribute]
        public int? purposecode { get; set; }

        [ExcelExportIgnoreAttribute]
        public int? typecode { get; set; }
    }
}
