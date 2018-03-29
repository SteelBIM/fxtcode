using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO
{
    public class FxtApi_SYSCity
    {
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityname")]
        /// <summary>
        /// CityName
        /// </summary>
        public virtual string CityName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "provinceid")]
        /// <summary>
        /// 省份ID
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        //[JsonProperty(PropertyName = "citycode")]
        ///// <summary>
        ///// CityCode
        ///// </summary>
        //public virtual string CityCode
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "gisid")]
        ///// <summary>
        ///// GIS_ID
        ///// </summary>
        //public virtual int? GISID
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "projectcount")]
        ///// <summary>
        ///// 楼盘数
        ///// </summary>
        //public virtual int? ProjectCount
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "pricebp")]
        ///// <summary>
        ///// 报盘案例占在线估价的比重
        ///// </summary>
        //public virtual decimal? PriceBP
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "pricecj")]
        ///// <summary>
        ///// 成交案例占在线估价的比重
        ///// </summary>
        //public virtual decimal? PriceCJ
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "iscase")]
        ///// <summary>
        ///// 是否可以查案例
        ///// </summary>
        //public virtual int? IsCase
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "isevalue")]
        ///// <summary>
        ///// 是否可以在线估价
        ///// </summary>
        //public virtual int? IsEValue
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "oldid")]
        ///// <summary>
        ///// OldId
        ///// </summary>
        //public virtual int? OldId
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "casemonth")]
        ///// <summary>
        ///// 在线估价选取案例时间（月）
        ///// </summary>
        //public virtual int? CaseMonth
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "x")]
        ///// <summary>
        ///// X
        ///// </summary>
        //public virtual decimal? X
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "y")]
        ///// <summary>
        ///// Y
        ///// </summary>
        //public virtual decimal? Y
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "xyscale")]
        ///// <summary>
        ///// 比例尺
        ///// </summary>
        //public virtual int? XYScale
        //{
        //    get;
        //    set;
        //}
        //[JsonProperty(PropertyName = "alias")]
        ///// <summary>
        ///// 简称
        ///// </summary>
        //public virtual string Alias
        //{
        //    get;
        //    set;
        //}
    }
}
