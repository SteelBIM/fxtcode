using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_SubArea_Industry
    {
        /// <summary>
        /// 园区板块
        /// </summary>
        [ExcelExportIgnore]
        public int SubAreaId { get; set; }
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [ExcelExportIgnore]
        public int CityId { get; set; }
        
        [DisplayName("*行政区")]
        public string AreaName { get; set; }

        [DisplayName("*工业片区")]
        public string SubAreaName { get; set; }

        [DisplayName("环线")]
        public string AreaLine { get; set; }

        [DisplayName("描述")]
        public string Details { get; set; }

        [DisplayName("经度")]
        public decimal? X { get; set; }

        [DisplayName("纬度")]
        public decimal? Y { get; set; }

        [DisplayName("经纬度比例")]
        public int? XYScale { get; set; }

        [ExcelExportIgnore]
        public int? FxtCompanyId { get; set; }
        [ExcelExportIgnore]
        public DateTime? CreateDate { get; set; }
        [ExcelExportIgnore]
        public string Creators { get; set; }
        [ExcelExportIgnore]
        public string SaveUser { get; set; }
        [ExcelExportIgnore]
        public DateTime? SaveDate { get; set; }
        /// <summary>
        /// 地图标注集合
        /// </summary>
        [ExcelExportIgnore]
        public string LngOrLat { get; set; }
    }
}
