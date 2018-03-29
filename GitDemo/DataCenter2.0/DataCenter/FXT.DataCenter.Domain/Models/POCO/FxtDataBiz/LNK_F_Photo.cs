using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// 楼层图片
    /// </summary>
    public class LNK_F_Photo
    {

        /// <summary>
        /// 楼层图片
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 楼层ID
        /// </summary>
        public long FloorId { get; set; }
        /// <summary>
        /// 图片类型1181
        /// </summary>
        [DisplayName("图片类型")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择{0}")]
        public int PhotoTypeCode { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [DisplayName("图片")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Path { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? PhotoDate { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [DisplayName("图片名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string PhotoName { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? SaveDate { get; set; }

    }
}
