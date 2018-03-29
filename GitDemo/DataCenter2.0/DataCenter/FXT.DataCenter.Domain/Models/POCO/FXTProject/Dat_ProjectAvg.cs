using FXT.DataCenter.Infrastructure.Common.NPOI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class Dat_ProjectAvg
    {
        [ExcelExportIgnore]
        public int projectavgid { get; set; }
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
        [ExcelExportIgnore]
        public int CityID { get; set; }
        [ExcelExportIgnore]
        public int? AreaId { get; set; }
        [ExcelExportIgnore]
        public int? SubAreaId { get; set; }
        [DisplayName("楼盘ID")]
        public int? ProjectId { get; set; }
        [DisplayName("行政区")]
        public string AreaName { get; set; }
        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("楼盘别名")]
        public string OtherName { get; set; }
        [DisplayName("案例月份")]
        [ExcelExportIgnore]
        public DateTime UseMonth { get; set; }

        [DisplayName("案例月份")]
        public string UseMonthN { get; set; }
        [DisplayName("楼盘均价_元/平方米")]
        public int? ProjectAvgPrice { get; set; }
        [DisplayName("涨跌幅_百分比")]
        public decimal? ProjectGained { get; set; }
        [ExcelExportIgnore]
        public int casecount { get; set; }
        [ExcelExportIgnore]
        public int casemaxprice { get; set; }
        [ExcelExportIgnore]
        public int caseminprice { get; set; }
        [DisplayName("案例均价值参考")]
        public string caseremark { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateDate { get; set; }
        [DisplayName("修改时间")]
        public DateTime UpdateDate { get; set; }
        [DisplayName("修改人")]
        public string UpdateUser { get; set; }
        [ExcelExportIgnore]
        public int? Valid { get; set; }
        [ExcelExportIgnore]
        public DateTime? startdate { get; set; }
        [ExcelExportIgnore]
        public DateTime? enddate { get; set; }
        [ExcelExportIgnore]
        public int isevalue { get; set; }
        [DisplayName("是否确认价格")]
        public string IsEValueName { get; set; }

        [ExcelExportIgnore]
        public int IsPrices { get; set; }

        [ExcelExportIgnore]
        [DisplayName("上个月的楼盘均价")]
        public int? ProjectAvgPriceBe { get; set; }
        [ExcelExportIgnore]
        [DisplayName("下个月的楼盘均价")]
        public int? ProjectAvgPriceAf { get; set; }

        [DisplayName("当前基准房价_元/平方米")]
        public int? WeightProjectPrice { get; set; }
    }
}
