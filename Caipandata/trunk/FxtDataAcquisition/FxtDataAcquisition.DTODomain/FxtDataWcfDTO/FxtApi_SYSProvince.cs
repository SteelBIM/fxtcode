using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.DTODomain.FxtDataWcfDTO
{
    public class FxtApi_SYSProvince
    {

        [JsonProperty(PropertyName = "provinceid")]
        /// <summary>
        /// ProvinceId
        /// </summary>
        public virtual int ProvinceId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "provincename")]
        /// <summary>
        /// 省的名称
        /// </summary>
        public virtual string ProvinceName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "alias")]
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string Alias
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "gisid")]
        /// <summary>
        /// GIS_ID
        /// </summary>
        public virtual int? GISID
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "oldid")]
        /// <summary>
        /// OldId
        /// </summary>
        public virtual int? OldId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "x")]
        /// <summary>
        /// X坐标
        /// </summary>
        public virtual decimal? X
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "y")]
        /// <summary>
        /// Y坐标
        /// </summary>
        public virtual decimal? Y
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "xyscale")]
        /// <summary>
        /// 比例尺
        /// </summary>
        public virtual int? XYScale
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "iszxs")]
        /// <summary>
        /// 是否直辖市
        /// </summary>
        public virtual int? IsZXS
        {
            get;
            set;
        }
    }
}
