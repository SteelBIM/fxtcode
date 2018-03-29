using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{

    public class DAT_SampleProject_Weight
    {

        /// <summary>
        /// 普通楼盘与样本楼盘关系
        /// </summary>
        [ExcelExportIgnore]
        public int Id { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        [ExcelExportIgnore]
        public int CityId { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 样本楼盘
        /// </summary>
        [ExcelExportIgnore]
        public int SampleProjectId { get; set; }
        /// <summary>
        /// 普通楼盘
        /// </summary>
        [ExcelExportIgnore]
        public int ProjectId { get; set; }
        
        [ExcelExportIgnore]
        public int SampleAreaId { get; set; }
        [ExcelExportIgnore]
        public int AreaId { get; set; }

        [DisplayName("*样本楼盘行政区")]
        public string SampleAreaName { get; set; }
        [DisplayName("*样本楼盘")]
        public string SampleProjectName { get; set; }

        [DisplayName("*关联楼盘行政区")]
        public string AreaName { get; set; }
        [DisplayName("*关联楼盘")]
        public string ProjectName { get; set; }

        [DisplayName("*系数")]
        public decimal Weight { get; set; }

        [ExcelExportIgnore]
        public int BuildingTypeCode { get; set; }

        [DisplayName("*建筑类型")]
        public string BuildingTypeCodeName { get; set; }
    }
}
