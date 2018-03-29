using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System.ComponentModel;

namespace FXT.DataCenter.Domain.Models
{
    public class LNK_P_PAddress
    {
        [ExcelExportIgnore]
        public int id { get; set; }
        [ExcelExportIgnore]
        public int? cityid { get; set; }
        [ExcelExportIgnore]
        public int areaid { get; set; }
        [DisplayName("楼盘ID")]
        public int? projectid { get; set; }
        [DisplayName("*行政区")]
        public string areaname { get; set; }
        [DisplayName("*楼盘名称")]
        public string projectname { get; set; }
        [DisplayName("楼盘别名")]
        public string othername { get; set; }
        [DisplayName("*房产证地址")]
        public string propertyaddress { get; set; }

        [ExcelExportIgnore]
        public int? fxtcompanyid { get; set; }
        [ExcelExportIgnore]
        public string creator { get; set; }
        [ExcelExportIgnore]
        public DateTime createdatetime { get; set; }
        [ExcelExportIgnore]
        public int? projectfxtcompanyid { get; set; }
        [ExcelExportIgnore]
        public string projectcreator { get; set; }
    }
}
