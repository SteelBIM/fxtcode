using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class BuildingApiDto
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? AppId { get; set; }

        [JsonProperty(PropertyName = "buildingid")]
        public int BuildingId { get; set; }

        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        [JsonProperty(PropertyName = "buildingname")]
        public string BuildingName { get; set; }
        /// <summary>
        /// 楼栋图片数量
        /// </summary>
        [JsonProperty(PropertyName = "buildimagecount")]
        public int BuildImageCount { get; set; }
        /// <summary>
        /// 楼栋坐标x
        /// </summary>
        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }
        /// <summary>
        /// 楼栋坐标y
        /// </summary>
        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }
        /// <summary>
        /// 单元室号数量
        /// </summary>
        [JsonProperty(PropertyName = "unitsnumber")]
        public int UnitsNumber { get; set; }
        /// <summary>
        /// 模板
        /// </summary>
        [JsonProperty(PropertyName = "templet")]
        public TempletDto Templet { get; set; }

        [JsonProperty(PropertyName = "houselist")]
        public List<HouseApiDto> HouseDtolist { get; set; }
    }
}
