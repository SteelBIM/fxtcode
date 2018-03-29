using System;
using System.Collections.Generic;

namespace FXT.DataCenter.Domain.Models.QueryObjects.House
{
    public class ProjectCaseParams : BaseParams
    {
        /// <summary>
        /// 行政区Id
        /// </summary>
        public int areaid { get; set; }

        /// <summary>
        /// 行政区名称
        /// </summary>
        public string areaname { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? casedateStart { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? casedateEnd { get; set; }
        /// <summary>
        /// 案例类型code
        /// </summary>
        public int caseTypeCode { get; set; }
        /// <summary>
        /// 建筑面积>=buildingAreaFrom
        /// </summary>
        public decimal? buildingAreaFrom { get; set; }
        /// <summary>
        /// 建筑面积<=buildingAreaTo
        /// </summary>
        public decimal? buildingAreaTo { get; set; }
        /// <summary>
        /// 用途code
        /// </summary>
        public int purposeCode { get; set; }
        /// <summary>
        /// 案例单价>=unitPriceFrom
        /// </summary>
        public decimal? unitPriceFrom { get; set; }
        /// <summary>
        /// 案例单价<=unitPriceTo
        /// </summary>
        public decimal? unitPriceTo { get; set; }
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int cityid { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int fxtcompanyid { get; set; }

        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime? savedatetime { get; set; }

        //建筑类型
        public int buildingTypeCode { get; set; }
    }

    public class ProjectCase_AvgPrice : BaseParams
    {
        public DateTime? timeFrom { get; set; }
        public DateTime? timeTo { get; set; }
        public string groupcycle { get; set; }
        public string grouparea { get; set; }
        public List<int> areaname { get; set; }
        public List<int> casetypecode { get; set; }
        public List<int> buildingtypecode { get; set; }
        public List<int> purposecode { get; set; }
        public List<int> buildingdatecode { get; set; }
        public string sampleproject { get; set; }
        public int cityid { get; set; }
        public int fxtcompanyid { get; set; }
    }
}
