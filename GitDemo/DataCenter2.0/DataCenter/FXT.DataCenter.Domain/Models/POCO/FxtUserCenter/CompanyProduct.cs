using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class CompanyProduct
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 产品CODE
        /// </summary>
        public int ProductTypeCode { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        public string CurrentVersion { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? OverDate { get; set; }
        /// <summary>
        /// 产品网址
        /// </summary>
        public string WebUrl { get; set; }
        /// <summary>
        /// API
        /// </summary>
        public string APIUrl { get; set; }
        /// <summary>
        /// OutAPIUrl
        /// </summary>
        public string OutAPIUrl { get; set; }
        /// <summary>
        /// 消息服务器
        /// </summary>
        public string MsgServer { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public int Valid { get; set; }
        /// <summary>
        /// CPId
        /// </summary>
        public int CPId { get; set; }
        /// <summary>
        /// 应用简称
        /// </summary>
        public string AppAbbreviation { get; set; }
        /// <summary>
        /// 产品所在城市
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 产品网址1
        /// </summary>
        public string WebUrl1 { get; set; }
        /// <summary>
        /// 产品LOGO,52X280
        /// </summary>
        public string LogoPath { get; set; }
        /// <summary>
        /// 产品小LOGO,52X52
        /// </summary>
        public string SmallLogoPath { get; set; }
        /// <summary>
        /// 对外显示的产品名称
        /// </summary>
        public string TitleName { get; set; }
        /// <summary>
        /// 是否可以导出数据中心数据
        /// </summary>
        public int? IsExportHose { get; set; }
        /// <summary>
        /// 产品联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 是否显示附属房屋
        /// </summary>
        public int? ShowSubHouse { get; set; }
        /// <summary>
        /// 是否显示折扣价
        /// </summary>
        public int? IsShowDiscountPrice { get; set; }
        /// <summary>
        /// mapheight
        /// </summary>
        public decimal? mapheight { get; set; }
        /// <summary>
        /// mapwidth
        /// </summary>
        public decimal? mapwidth { get; set; }
        /// <summary>
        /// 是否自动生成物业全称；1自动（楼盘名称+楼栋名称+（栋）+楼层+层+房号名称），0原始（地址+楼盘名称+楼栋名称+房号名称）
        /// </summary>
        public int? AutoMakeName { get; set; }
        /// <summary>
        /// 是否直接删除数据
        /// </summary>
        public int? IsDeleteTrue { get; set; }
        /// <summary>
        /// 皮肤
        /// </summary>
        public string SkinPath { get; set; }
        /// <summary>
        /// 登录背景图,630X560
        /// </summary>
        public string BG_Pic { get; set; }
        /// <summary>
        /// 评估机构主页
        /// </summary>
        public string homepage { get; set; }
        /// <summary>
        /// 二维码图片
        /// </summary>
        public string twodimensionalcode { get; set; }

        public int ParentProductTypeCode { get; set; }
        public int ParentShowDataCompanyId { get; set; }
    }
}
