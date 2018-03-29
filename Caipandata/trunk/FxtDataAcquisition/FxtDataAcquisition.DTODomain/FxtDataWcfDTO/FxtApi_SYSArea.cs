using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FxtDataAcquisition.DTODomain.FxtDataWcfDTO
{
    public  class FxtApi_SYSArea
    {
        [JsonProperty(PropertyName = "areaid")]
        public int AreaId
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "areaname")]
        public string AreaName
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "cityid")]
        public int CityId
        {
            get;
            set;
        }
    }
}
