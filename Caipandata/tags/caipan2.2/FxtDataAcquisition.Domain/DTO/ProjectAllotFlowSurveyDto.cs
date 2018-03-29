using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Domain.DTO
{
    public class ProjectAllotFlowSurveyDto
    {
        #region dat_project
        [JsonProperty(PropertyName = "fxtprojectid")]
        /// <summary>
        /// 正式库的楼盘ID
        /// </summary>
        public int? FxtprojectId { get; set; }
        [JsonProperty(PropertyName = "projectid")]
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int ProjectId { get; set; }
        [JsonProperty(PropertyName = "projectname")]
        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string projectName { get; set; }
        [JsonProperty(PropertyName = "cityid")]
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }
        [JsonProperty(PropertyName = "areaid")]
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int AreaID { get; set; }
        [JsonProperty(PropertyName = "areaname")]
        /// <summary>
        /// 行政区名称
        /// </summary>
        public string AreaName { get; set; }
        [JsonProperty(PropertyName = "subareaid")]
        /// <summary>
        /// 片区ID
        /// </summary>
        public int? SubAreaId { get; set; }
        [JsonProperty(PropertyName = "subareaname")]
        /// <summary>
        /// 片区名称
        /// </summary>
        public string SubAreaName { get; set; }
        [JsonProperty(PropertyName = "tatolbuilddingnume")]
        /// <summary>
        /// 楼栋数（非总栋数）
        /// </summary>
        public int TatolBuildingNum { get; set; }
        //public int? totalnum { get; set; }
        [JsonProperty(PropertyName = "photocount")]
        /// <summary>
        /// 图片数
        /// </summary>
        public int PhotoCount { get; set; }
        [JsonProperty(PropertyName = "address")]
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        [JsonProperty(PropertyName = "x")]
        public decimal? X { get; set; }
        [JsonProperty(PropertyName = "y")]
        public decimal? Y { get; set; }

        #endregion

        #region dat_allotflow
        [JsonProperty(PropertyName = "allotid")]
        /// <summary>
        /// 任务id
        /// </summary>
        public long AllotId { get; set; }
        [JsonProperty(PropertyName = "allotstate")]
        /// <summary>
        /// 任务状态
        /// </summary>
        public int? AllotState { get; set; }
        [JsonProperty(PropertyName = "username")]
        /// <summary>
        /// 分配人
        /// </summary>
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "usertruename")]
        /// <summary>
        /// 分配人姓名
        /// </summary>
        public string UserTrueName { get; set; }
        [JsonProperty(PropertyName = "surveyusername")]
        /// <summary>
        /// 接收人
        /// </summary>
        public string SurveyUserName { get; set; }
        [JsonProperty(PropertyName = "surveyusertruename")]
        /// <summary>
        /// 接收人
        /// </summary>
        public string SurveyUserTrueName { get; set; }
        [JsonProperty(PropertyName = "statedate")]
        /// <summary>
        /// 状态改变时间
        /// </summary>
        public DateTime? StateDate { get; set; }
        #endregion

        #region dat_allotsurvey
        [JsonProperty(PropertyName = "allotdate")]
        /// <summary>
        /// 状态改变时间
        /// </summary>
        public DateTime? AllotDate { get; set; }
        #endregion

    }
}
