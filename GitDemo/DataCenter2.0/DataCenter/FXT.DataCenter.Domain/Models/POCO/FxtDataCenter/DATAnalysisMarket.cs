using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
   public class DatAnalysisMarket
    {
        /// <summary>
        /// 市场背景分析
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataTypeCode { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// Analysis
        /// </summary>
        [DisplayName("分析")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Analysis { get; set; }
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// SaveDateTime
        /// </summary>
        public DateTime? SaveDateTime { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public int? Valid { get; set; }
        /// <summary>
        /// AreaId
        /// </summary>
        [DisplayName("行政区")]
        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int AreaId { get; set; }
        /// <summary>
        /// SubAreaId
        /// </summary>
        public int SubAreaId { get; set; }

        public string AreaName { get; set; }

        public string SubAreaName { get; set; }

        /// <summary>
        /// 商圈
        /// </summary>
        public string BizSubAreaName { get; set; }

        /// <summary>
        /// 商务中心
        /// </summary>
        public string OfficeSubAreaName { get; set; }

        /// <summary>
        /// 工业片区
        /// </summary>
        public string IndustrySubAreaName { get; set; }
    }
}
