using System;
using System.ComponentModel.DataAnnotations;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace FXT.DataCenter.Domain.Models
{
    //LNK_B_Photo
    public class LnkBPhoto
    {

        /// <summary>
        /// 楼栋图片
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 楼栋ID
        /// </summary>
        public long BuildingId { get; set; }
        /// <summary>
        /// 图片类型1181
        /// </summary>
        [Range(1,int.MaxValue,ErrorMessage="请选择图片类型")]
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
        /// 照片类型
        /// </summary>
        public string PhotoTypeName { get; set; }

    }
}