using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FXT.DataCenter.Infrastructure.Common.NPOI;
using System.ComponentModel;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_ProjectMatch
    {
        /// <summary>
        /// Id
        /// </summary>
        [ExcelExportIgnore]
        public int Id { get; set; }

        [ExcelExportIgnore]
        public int? ProjectNameId { get; set; }

        [DisplayName("*系统名行政区")]
        public string AreaName { get; set; }

        [Required(ErrorMessage = "系统名不能为空")]
        [DisplayName("*系统名")]
        public string ProjectName { get; set; }

        [DisplayName("网络名行政区")]
        public string NetAreaName { get; set; }

        [Required(ErrorMessage = "网络名不能为空")]
        [DisplayName("*网络名")]
        public string NetName { get; set; }

        [ExcelExportIgnore]
        public int? CityId { get; set; }

        [ExcelExportIgnore]
        public int? FXTCompanyId { get; set; }

        [ExcelExportIgnore]
        public int? AreaId { get; set; }

        [ExcelExportIgnore]//楼盘的创建者，鉴于分辨案例库的基础楼盘权限
        public string projectCreator { get; set; }
        [ExcelExportIgnore]//楼盘的创建者，鉴于分辨案例库的基础楼盘权限
        public int projectFxtCompanyId { get; set; }

        [ExcelExportIgnore]
        public string Creator { get; set; }

        [ExcelExportIgnore]
        public DateTime CreateTime { get; set; }

        [ExcelExportIgnore]
        public string SaveUser { get; set; }

        [ExcelExportIgnore]
        public DateTime SaveTime { get; set; }
    }
}
