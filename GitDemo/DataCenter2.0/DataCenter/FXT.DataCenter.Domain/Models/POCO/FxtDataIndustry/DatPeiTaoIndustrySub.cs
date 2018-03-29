using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DatPeiTaoIndustrySub
    {
        /// <summary>
        /// 工业配套
        /// </summary>
        public long PeiTaoID { get; set; }
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 配套类型1149
        /// </summary>
        public int PeiTaoCode { get; set; }
        /// <summary>
        /// 配套名称
        /// </summary>
        public string PeiTaoName { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// 部位
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 面积_平方米
        /// </summary>
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 租户（商家）CompanyID
        /// </summary>
        public long TenantID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Creators
        /// </summary>
        public string Creators { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// SaveUser
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// SaveDate
        /// </summary>
        public DateTime? SaveDate { get; set; }
        /// <summary>
        /// FxtCompanyId
        /// </summary>
        public int FxtCompanyId { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public int Valid { get; set; }    
    }
}
