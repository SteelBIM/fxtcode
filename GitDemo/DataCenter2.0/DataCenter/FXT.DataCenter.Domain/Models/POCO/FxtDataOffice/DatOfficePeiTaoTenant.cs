using FXT.DataCenter.Infrastructure.Common.NPOI;
using System;
using System.ComponentModel;

namespace FXT.DataCenter.Domain.Models
{
    public class DatOfficePeiTaoTenant
    {
        /// <summary>
        /// 办公租户商家
        /// </summary>
        [ExcelExportIgnore]
        [DisplayName("城市ID")]
        public int cityId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("公司ID")]
        public int fxtCompanyId { get; set; }

        [ExcelExportIgnore]
        [DisplayName("商家ID")]
        public long TenantId { get; set; }

        [DisplayName("办公楼盘")]
        public string TenantName { get; set; }

    }
}
