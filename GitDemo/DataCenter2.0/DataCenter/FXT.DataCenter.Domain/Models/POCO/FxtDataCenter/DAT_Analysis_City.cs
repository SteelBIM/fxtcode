using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 区域分析
    /// </summary>
    public class DAT_Analysis_City
    {
        /// <summary>
        /// 各公司，各用途的城市区域分析
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataTypeCode { get; set; }
        /// <summary>
        /// 评估机构
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        [DisplayName("行政区")]
        [Required(ErrorMessage = "请选择{0}")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int AreaId { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 城市、行政区、片区分析
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
        /// 片区or商圈
        /// </summary>
        [DisplayName("片区")]
        public int SubAreaId { get; set; }
        /// <summary>
        /// 片区
        /// </summary>
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
