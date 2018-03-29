using System;

namespace FXT.DataCenter.Domain.Models.QueryObjects.House
{
    public class ProjectQueryParams : BaseParams
    {
        public ProjectQueryParams() { }
        public ProjectQueryParams(int _cityId,int _fxtId,int _projectId) {
            this.CityId = _cityId;
            this.FxtCompanyId = _fxtId;
            this.ProjectId = _projectId;
        }
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 行政区ID
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 片区
        /// </summary>
        public int SubAreaId { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int FxtCompanyId { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        public int PlanPurpose { get; set; }
        /// <summary>
        /// 产权形式
        /// </summary>
        public int RightCode { get; set; }

        /// <summary>
        /// 建筑类型
        /// </summary>
        public int BuildingTypeCode { get; set; }

        /// <summary>
        /// 楼盘使用年限
        /// </summary>
        public int UsableYear { get; set; }
        /// <summary>
        /// 宗地号
        /// </summary>
        public string FieldNo { get; set; }
        /// <summary>
        /// 楼盘关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 楼盘楼栋关键字模糊查找
        /// </summary>
        public string KeyString { get; set; }
        /// <summary>
        /// 土地起始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 土地终止日期
        /// </summary>
        public DateTime StartEndDate { get; set; }
        /// <summary>
        /// 开工日期start
        /// </summary>
        public DateTime BuildingStartDate { get; set; }
        /// <summary>
        /// 开工日期end
        /// </summary>
        public DateTime BuildingEndDate { get; set; }
    }
}
