using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Domain.DTO
{
    public class ProjectApiDto
    {
        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }
        /// <summary>
        /// 楼盘名称
        /// </summary>
        [JsonProperty(PropertyName = "projectname")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        [JsonProperty(PropertyName = "areaname")]
        public string AreaName { get; set; }
        /// <summary>
        /// 片区
        /// </summary>
        [JsonProperty(PropertyName = "subareaname")]
        public string SubAreaName { get; set; }
        /// <summary>
        /// 照片数
        /// </summary>
        [JsonProperty(PropertyName = "photocount")]
        public int PhotoCount { get; set; }
        /// <summary>
        /// 楼盘坐标x
        /// </summary>
        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }
        /// <summary>
        /// 楼盘坐标y
        /// </summary>
        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }
        /// <summary>
        /// 当前楼栋数
        /// </summary>
        [JsonProperty(PropertyName = "tatolbuilddingnume")]
        public int TatolBuildingNum { get; set; }
        /// <summary>
        /// 模板
        /// </summary>
        [JsonProperty(PropertyName = "templet")]
        public TempletDto Templet { get; set; }

        [JsonProperty(PropertyName = "buildinglist")]
        public List<BuildingApiDto> BuildingDtolist { get; set; }
    }
}
