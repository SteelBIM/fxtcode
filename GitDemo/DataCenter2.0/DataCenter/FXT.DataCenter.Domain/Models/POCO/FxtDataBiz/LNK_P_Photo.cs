using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models.FxtDataBiz
{
    /// <summary>
    /// 项目图片
    /// </summary>
    public class LNK_P_Photo
    {

        /// <summary>
        /// 项目图片
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 楼盘
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// 图片类型1181
        /// </summary>
        [Range(1,int.MaxValue,ErrorMessage="请选择图片类型")]
        public int? PhotoTypeCode { get; set; }
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
        /// 城市
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? Valid { get; set; }
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
        /// 图片类型名称
        /// </summary>
        public string PhotoTypeName { get; set; }

    }
}
