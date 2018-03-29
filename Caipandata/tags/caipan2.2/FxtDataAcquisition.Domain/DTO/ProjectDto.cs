using FxtDataAcquisition.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO
{
    public class ProjectDto
    {
        #region 楼盘基本信息
        [JsonProperty(PropertyName = "projectid")]
        public int ProjectId { get; set; }
        [JsonProperty(PropertyName = "fxtprojectid")]
        public int? FxtProjectId { get; set; }
        [JsonProperty(PropertyName = "projectname")]
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ProjectName { get; set; }
        [JsonProperty(PropertyName = "othername")]
        public string OtherName { get; set; }
        [JsonProperty(PropertyName = "cityid")]
        public int CityID { get; set; }
        [JsonProperty(PropertyName = "areaid")]
        public int AreaID { get; set; }
        [JsonProperty(PropertyName = "areaname")]
        public string AreaName { get; set; }
        [JsonProperty(PropertyName = "subareaid")]
        public int? SubAreaId { get; set; }
        [JsonProperty(PropertyName = "subareaname")]
        public string SubAreaName { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "east")]
        public string East { get; set; }
        [JsonProperty(PropertyName = "west")]
        public string West { get; set; }
        [JsonProperty(PropertyName = "south")]
        public string South { get; set; }
        [JsonProperty(PropertyName = "north")]
        public string North { get; set; }
        [JsonProperty(PropertyName = "purposecode")]
        public int PurposeCode { get; set; }
        [JsonProperty(PropertyName = "rightcode")]
        public int? RightCode { get; set; }
        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }
        [JsonProperty(PropertyName = "x")]
        /// <summary>
        /// 楼栋坐标x
        /// </summary>
        public decimal? X { get; set; }
        [JsonProperty(PropertyName = "y")]
        /// <summary>
        /// 楼栋坐标y
        /// </summary>
        public decimal? Y { get; set; }
        /// <summary>
        /// 楼栋数
        /// </summary>
        [JsonProperty(PropertyName = "tatolbuilddingnume")]
        public int TatolBuildingNum { get; set; }
        
        #endregion

        [JsonProperty(PropertyName = "parkingstatus")]
        /// <summary>
        /// 停车状况
        /// </summary>
        public int? ParkingStatus { get; set; }
        [JsonProperty(PropertyName = "photocount")]
        /// <summary>
        /// 照片数
        /// </summary>
        public int PhotoCount { get; set; }
        [JsonProperty(PropertyName = "statedate")]
        /// <summary>
        /// 状态更新时间
        /// </summary>
        public DateTime? StateDate { get; set; }
        [JsonProperty(PropertyName = "developers")]
        /// <summary>
        /// 开发商
        /// </summary>
        public string Developers { get; set; }
        [JsonProperty(PropertyName = "manager_company")]
        /// <summary>
        /// 物业管理公司
        /// </summary>
        public string ManagerCompany { get; set; }

        [JsonProperty(PropertyName = "allotflowremark")]
        /// <summary>
        /// 任务备注
        /// </summary>
        public string AllotFlowremark { get; set; }
        [JsonProperty(PropertyName = "allotid")]
        /// <summary>
        /// 任务id
        /// </summary>
        public long Allotid { get; set; }

        [JsonProperty(PropertyName = "buildinglist")]
        public List<BuildingDto> BuildingDtolist { get; set; }

        [JsonProperty(PropertyName = "enddate")]
        /// <summary>
        /// 竣工时间
        /// </summary>
        public DateTime? Enddate { get; set; }
        [JsonProperty(PropertyName = "buildingnum")]
        /// <summary>
        /// 总栋数
        /// </summary>
        public int? BuildingNum { get; set; }
        [JsonProperty(PropertyName = "totalnum")]
        /// <summary>
        /// 总套数
        /// </summary>
        public int? Totalnum { get; set; }
        [JsonProperty(PropertyName = "parkingnumber")]
        /// <summary>
        /// 车位数
        /// </summary>
        public int? Parkingnumber { get; set; }
        [JsonProperty(PropertyName = "managerquality")]
        /// <summary>
        /// 物业管理质量
        /// </summary>
        public int? Managerquality { get; set; }
    }
}
