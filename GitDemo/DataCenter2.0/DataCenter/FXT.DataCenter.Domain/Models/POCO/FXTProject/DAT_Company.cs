using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FXT.DataCenter.Infrastructure.Common.NPOI;

namespace FXT.DataCenter.Domain.Models
{
    public class DAT_Company
    {

        /// <summary>
        /// CompanyId
        /// </summary>
        [ExcelExportIgnore]
        public int CompanyId { get; set; }
        /// <summary>
        /// ChineseName
        /// </summary>
        [Required(ErrorMessage = "中文名称不能为空")]
        [DisplayName("中文名")]
        public string ChineseName { get; set; }
        /// <summary>
        /// EnglishName
        /// </summary>
        [DisplayName("英文名")]
        public string EnglishName { get; set; }
        /// <summary>
        /// CompanyTypeCode
        /// </summary>
        [ExcelExportIgnore]
        public int? CompanyTypeCode { get; set; }
        /// <summary>
        /// CityId
        /// </summary>
        [ExcelExportIgnore]
        public int? CityId { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        [DisplayName("地址")]
        public string Address { get; set; }
        /// <summary>
        /// Telephone
        /// </summary>
        [DisplayName("电话")]
        public string Telephone { get; set; }
        /// <summary>
        /// Fax
        /// </summary>
        [DisplayName("传真")]
        public string Fax { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        [DisplayName("网址")]
        public string Website { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        [ExcelExportIgnore]
        public int? Valid { get; set; }
        /// <summary>
        /// 简称、别名
        /// </summary>
        [DisplayName("简称")]
        public string COtherName { get; set; }
        /// <summary>
        /// 主营品牌
        /// </summary>
        [DisplayName("主营品牌")]
        public string Brand { get; set; }
        /// <summary>
        /// 公司原注册国家
        /// </summary>
        [DisplayName("公司原注册国家")]
        public string FromCountry { get; set; }
        /// <summary>
        /// 原注册城市
        /// </summary>
        [DisplayName("原注册城市")]
        public string FromCity { get; set; }
        /// <summary>
        /// 性质，1178
        /// </summary>
        [DisplayName("性质")]
        [ExcelExportIgnore]
        public int? NatureCode { get; set; }
        /// <summary>
        /// 行业大类，1158
        /// </summary>
       [DisplayName("行业大类")]
       [ExcelExportIgnore]
        public int? IndustryCode { get; set; }
        /// <summary>
        /// 行业小类，1159~1177
        /// </summary>
       [DisplayName("行业小类")]
       [ExcelExportIgnore]
        public int? SubIndustryCode { get; set; }
        /// <summary>
        /// 公司规模（人）
        /// </summary>
        [DisplayName("公司规模")]
        [ExcelExportIgnore]
        public int? ScaleCode { get; set; }
        /// <summary>
        /// 注册资本（万）
        /// </summary>
        [DisplayName("注册资本")]
        public int? RegistCapital { get; set; }
        /// <summary>
        /// 行业地位，1179
        /// </summary>
        [ExcelExportIgnore]
        public int? StandingCode { get; set; }
        /// <summary>
        /// 集团公司ID
        /// </summary>
        [ExcelExportIgnore]
        public int? GroupId { get; set; }

        /// <summary>
        /// 集团名称
        /// </summary>
        [DisplayName("集团名称")]
        public string GroupName { get; set; }

        /// <summary>
        /// 是否是集团公司
        /// </summary>
        [ExcelExportIgnore]
        public int? IsGroupHQ { get; set; }

        /// <summary>
        /// 评估机构ID
        /// </summary>
       [ExcelExportIgnore]
        public int? FxtCompanyId { get; set; }


        #region 扩展字段
        /// <summary>
        /// 公司名称
        /// </summary>
        [DisplayName("公司名称")]
        public string CompanyTypeName { get; set; }

        //公司性质
        [DisplayName("公司性质")]
        public string NatureName { get; set; }
        //行业大类
        [DisplayName("行业大类")]
        public string IndustryName { get; set; }
        //行业小类
        [DisplayName("行业小类")]
        public string SubIndustryName { get; set; }
        //行业地位
        [DisplayName("行业地位")]
        public string StandingName { get; set; }

        #endregion
    }
}
