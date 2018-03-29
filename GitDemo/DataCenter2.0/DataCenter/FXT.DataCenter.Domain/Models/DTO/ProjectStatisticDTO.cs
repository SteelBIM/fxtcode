using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models.DTO
{
    public class Project_BHCount
    {
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [DisplayName("行政区")]
        public string AreaName { get; set; }
        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("楼栋总数")]
        public int BuildingNum { get; set; }
        [DisplayName("房号总数")]
        public int HouseName { get; set; }
        [ExcelExportIgnore]
        public string Creator { get; set; }
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
    }

    public class Project_PPCount
    {
        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [DisplayName("行政区")]
        public string AreaName { get; set; }
        [DisplayName("楼盘名称")]
        public string ProjectName { get; set; }
        [DisplayName("图片总数")]
        public int PicCount { get; set; }
        [DisplayName("logo图片总数")]
        public int PicLogoNum { get; set; }
        [DisplayName("标准层平面图图片总数")]
        public int PicBZCNum { get; set; }
        [DisplayName("户型图图片总数")]
        public int PicHXNum { get; set; }
        [DisplayName("实景图图片总数")]
        public int PicSJNum { get; set; }
        [DisplayName("外立面图图片总数")]
        public int PicWLMNum { get; set; }
        [DisplayName("位置图图片总数")]
        public int PicWZNum { get; set; }
        [DisplayName("效果图图片总数")]
        public int PicXGNum { get; set; }
        [DisplayName("总平面图图片总数")]
        public int PicZPMNum { get; set; }
        [DisplayName("其他图片总数")]
        public int PicQTNum { get; set; }
        [ExcelExportIgnore]
        public string Creator { get; set; }
        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }
    }
}
