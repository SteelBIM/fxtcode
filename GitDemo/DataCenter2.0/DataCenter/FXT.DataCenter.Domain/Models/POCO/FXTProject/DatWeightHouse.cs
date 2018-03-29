using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class DatWeightHouse
    {
        /// <summary>
        /// 房号系数均价
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        public int FxtCompanyId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 楼栋ID
        /// </summary>
        public int BuildingId { get; set; }

        /// <summary>
        /// 房号ID
        /// </summary>
        [Required(ErrorMessage = "房号不存在")]
        public int HouseId { get; set; }

        /// <summary>
        /// 系数
        /// </summary>
        [DisplayName("系数")] 
        public decimal? Weight { get; set; }

        /// <summary>
        /// 均价
        /// </summary>
        [DisplayName("均价")] 
        public decimal? AvgPrice { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 房号名称
        /// </summary>
        public string HouseName { get; set; }

        [DisplayName("估价公司名称")]
        public int? EvaluationCompanyId { get; set; }
        [DisplayName("修改人")]
        public string UpdateUser { get; set; }
    }
}
