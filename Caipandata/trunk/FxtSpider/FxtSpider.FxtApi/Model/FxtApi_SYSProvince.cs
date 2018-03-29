using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.Common;

namespace FxtSpider.FxtApi.Model
{
    public class FxtApi_SYSProvince
    {

        /// <summary>
        /// ProvinceId
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 省的名称
        /// </summary>
        public virtual string ProvinceName
        {
            get;
            set;
        }
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string Alias
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
        /// OldId
        /// </summary>
        public virtual int? OldId
        {
            get;
            set;
        }
        /// <summary>
        /// X坐标
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        /// <summary>
        /// Y坐标
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
        /// 是否直辖市
        /// </summary>
        public virtual int? IsZXS
        {
            get;
            set;
        }

        public static FxtApi_SYSProvince ConvertToObj(string json)
        {
            return JsonHelp.ParseJSONjss<FxtApi_SYSProvince>(json);
        }
        public static List<FxtApi_SYSProvince> ConvertToObjList(string json)
        {
            return JsonHelp.ParseJSONList<FxtApi_SYSProvince>(json);
        }
    }
}
