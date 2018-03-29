using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_SampleProject
    {
        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("片区")]
        public string SubAreaName { get; set; }

        [DisplayName("*样本楼盘")]
        public string ProjectName { get; set; }

        [ExcelExportIgnore]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "楼盘不能为空")]
        [ExcelExportIgnore]
        [Range(1, int.MaxValue, ErrorMessage = "请选择样本楼盘")]
        public int ProjectId { get; set; }
        
        [ExcelExportIgnore]
        public int CityId { get; set; }
        
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }

        [ExcelExportIgnore]
        [Required(ErrorMessage = "土地用途不能为空")]
        public int PurposeCode { get; set; }

        [DisplayName("*土地用途")]
        public string PurposeName { get; set; }

        [ExcelExportIgnore]
        [Required(ErrorMessage = "建筑类型不能为空")]
        public int BuildingTypeCode { get; set; }

        [DisplayName("*建筑类型")]
        public string BuildingTypeName { get; set; }

        [DisplayName("*竣工日期")]
        [Required(ErrorMessage = "竣工日期不能为空")]
        public DateTime BuildingDate { get; set; }
        
        [ExcelExportIgnore]
        [Required(ErrorMessage = "行政区不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "行政区不能为空")]
        public int AreaId { get; set; }
        
        [ExcelExportIgnore]
        public int? SubAreaId { get; set; }
       
        [ExcelExportIgnore]
        public int? Valid { get; set; }
        
        [DisplayName("案例数")]
        [RegularExpression(@"^[0-9]{0,10}$", ErrorMessage = "{0}必须是整数,且少于10位")]
        [Range(1, int.MaxValue, ErrorMessage = "您输入的数字已超过最大范围2147483647")]
        public int? CaseNumber { get; set; }

        [DisplayName("关联楼盘")]
        [ExcelExportIgnore]
        public string RelationProjects { get; set; }

        [ExcelExportIgnore]//楼盘的创建者，鉴于分辨案例库的基础楼盘权限
        public string projectCreator { get; set; }
        [ExcelExportIgnore]//楼盘的创建者，鉴于分辨案例库的基础楼盘权限
        public int projectFxtCompanyId { get; set; }
    }
}
