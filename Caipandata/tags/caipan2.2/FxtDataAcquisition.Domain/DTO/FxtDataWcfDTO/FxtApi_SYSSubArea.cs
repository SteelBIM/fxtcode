using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO
{
    public class FxtApi_SYSSubArea
    {
        [JsonProperty(PropertyName = "areaid")]
        public int AreaId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "subareaid")]
        public int SubAreaId
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "subareaname")]
        public string SubAreaName
        {
            get;
            set;
        }
    }
}
