using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    /// <summary>
    /// (商铺)房号租客表
    /// </summary>
    public class Dat_Tenant_Biz
    {

        /// <summary>
        /// (商铺)房号租客表
        /// </summary>
        [ExcelExportIgnore]
        public long HouseTenantId { get; set; }

        [ExcelExportIgnore]
        public int CityId { get; set; }
        
        [ExcelExportIgnore]
        public int AreaId { get; set; }
        [ExcelExportIgnore]
        public int SubAreaBizId { get; set; }

        [ExcelExportIgnore]
        public long ProjectId { get; set; }
        [DisplayName("商业街")]
        public string ProjectName { get; set; }

        [ExcelExportIgnore]
        public long BuildingId { get; set; }
        [DisplayName("商业楼栋")]
        public string BuildingName { get; set; }

        [DisplayName("楼层")]
        public string FloorNum { get; set; }

        [ExcelExportIgnore]
        [DisplayName("房号")]
        public long HouseId { get; set; }
        [DisplayName("房号")]
        public string HouseName { get; set; }

        /// <summary>
        /// 是否空置
        /// </summary>
        [ExcelExportIgnore]
        public int IsVacant { get; set; }
        /// <summary>
        /// 租赁面积
        /// </summary>
        [DisplayName("租赁面积")]
        public decimal? BuildingArea { get; set; }
        /// <summary>
        /// 租金单价（元.天/平方米）
        /// </summary>
        [DisplayName("租金单价")]
        public decimal? Rent { get; set; }
        /// <summary>
        /// 租金方式1155
        /// </summary>
        [ExcelExportIgnore]
        public int? RentTypeCode { get; set; }
        [DisplayName("租金方式")]
        public string RentTypeName { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        [DisplayName("销售单价")]
        public decimal? SaleUnitPrice { get; set; }
        /// <summary>
        /// 商铺名称
        /// </summary>
        [DisplayName("商铺名称")]
        public string BizHouseName { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        [DisplayName("品牌名称")]
        public string BrandName { get; set; }
        /// <summary>
        /// 租客（商家）CompanyId
        /// </summary>
        [ExcelExportIgnore]
        public long? TenantID { get; set; }

        [ExcelExportIgnore]
        [DisplayName("消费定位ID")]
        public int? BizType { get; set; }
        [DisplayName("消费定位")]
        public string BizTypeName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("经营业态大类ID")]
        public int BizCode { get; set; }
        [DisplayName("经营业态大类")]
        public string BizName { get; set; }

        [ExcelExportIgnore]
        [DisplayName("经营业态小类ID")]
        public int? BizSubCode { get; set; }
        [DisplayName("经营业态小类")]
        public string BizSubName { get; set; }

        [DisplayName("进驻时间")]
        public DateTime? JoinDate { get; set; }

        [DisplayName("调查时间")]
        public DateTime? SurveyDate { get; set; }

        [DisplayName("调查人")]
        public string SurveyUser { get; set; }

        [ExcelExportIgnore]
        public int FxtCompanyId { get; set; }

        [ExcelExportIgnore]
        public string Creator { get; set; }

        [ExcelExportIgnore]
        public DateTime CreateTime { get; set; }

        [ExcelExportIgnore]
        public int Valid { get; set; }

        [ExcelExportIgnore]
        public DateTime? SaveDateTime { get; set; }

        [ExcelExportIgnore]
        public string SaveUser { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }

        [DisplayName("是否知名品牌")]
        public int? IsTypical { get; set; }
        
    }
}
