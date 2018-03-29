using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
   public class LNK_H_Photo
    {
        /// <summary>
        /// 房号商铺图片
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 房号ID
        /// </summary>
        public long HouseId { get; set; }
        /// <summary>
        /// 商铺ID
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// 图片类型1181
        /// </summary>
       [Range(1, int.MaxValue, ErrorMessage = "前选择图片类型")]
       public int PhotoTypeCode { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? PhotoDate { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
       [Required(ErrorMessage = "请填写图片名称")]
       public string PhotoName { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// Valid
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

        /// <summary>
        ///  图片类型
        /// </summary>
        public string PhotoTypeName { get; set; }
    }
}
