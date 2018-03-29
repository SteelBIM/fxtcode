using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_SYSCity
    {
        /// <summary>
        /// CityId
        /// </summary>
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// CityName
        /// </summary>
        public virtual string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 省份ID
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// CityCode
        /// </summary>
        public virtual string CityCode
        {
            get;
            set;
        }
        /// <summary>
        /// GIS_ID
        /// </summary>
        public virtual int? GISID
        {
            get;
            set;
        }
        /// <summary>
        /// 楼盘数
        /// </summary>
        public virtual int? ProjectCount
        {
            get;
            set;
        }
        /// <summary>
        /// 报盘案例占在线估价的比重
        /// </summary>
        public virtual decimal? PriceBP
        {
            get;
            set;
        }
        /// <summary>
        /// 成交案例占在线估价的比重
        /// </summary>
        public virtual decimal? PriceCJ
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可以查案例
        /// </summary>
        public virtual int? IsCase
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可以在线估价
        /// </summary>
        public virtual int? IsEValue
        {
            get;
            set;
        }
        /// <summary>
        /// OldId
        /// </summary>
        public virtual int? OldId
        {
            get;
            set;
        }
        /// <summary>
        /// 在线估价选取案例时间（月）
        /// </summary>
        public virtual int? CaseMonth
        {
            get;
            set;
        }
        /// <summary>
        /// X
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        /// <summary>
        /// Y
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }
        /// <summary>
        /// 比例尺
        /// </summary>
        public virtual int? XYScale
        {
            get;
            set;
        }
        /// <summary>
        /// 简称
        /// </summary>
        public virtual string Alias
        {
            get;
            set;
        }

        public static FxtApi_SYSCity ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_SYSCity>(json);
        }
        public static List<FxtApi_SYSCity> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_SYSCity>(json);
        }
    }
}
