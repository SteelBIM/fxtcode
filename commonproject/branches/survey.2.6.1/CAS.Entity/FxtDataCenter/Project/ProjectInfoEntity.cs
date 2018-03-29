using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter.Project
{
    /// <summary>
    /// 楼盘信息类
    /// </summary>
    public class ProjectInfoEntity : BaseTO
    {
        public int? buildingid { get; set; }

        public int? floortotal { get; set; }

        public int? housetotal { get; set; }

        public string buildingname { get; set; }

        public DateTime? builddate { get; set; }

        public decimal? averageprice { get; set; }

        public int? isevalue { get; set; }

        public decimal? weight { get; set; }

        public decimal? totalbuildarea { get; set; }

        public string codename { get; set; }

        public int? purposecode { get; set; }

        public int? buildingtypecode { get; set; }

        public string ob_othername { get; set; }

        public int? ob_startnum { get; set; }

        public string ob_starletter { get; set; }

        public string ob_text { get; set; }

        public int? ob_number { get; set; }
    }

    /// <summary>
    /// 楼盘价格走势类
    /// </summary>
    public class ProjectPriceTrendEntity : BaseTO
    {
        public int projectid { get; set; }

        public string projectname { get; set; }

        public string sourcetypename { get; set; }

        public string purposename { get; set; }

        public string buildingtypename { get; set; }

        public string houseno { get; set; }

        public string buildingname { get; set; }

        public DateTime ? casedate { get; set; }

        public decimal? unitprice { get; set; }

        public decimal? buildingarea { get; set; }

        public decimal? totalprice { get; set; }
    }

    /// <summary>
    /// 楼盘照片信息类
    /// </summary>
    public class ProjectPhotoEntity : BaseTO
    {
        public int id { get; set; }

        /// <summary>
        /// 楼栋id
        /// </summary>
        public int? buildingid { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public int? projectid { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int? cityid { get; set; }

        public int? valid { get; set; }

        public int? fxtcompanyid { get; set; }

        public string phototypecode { get; set; }

        public string path { get; set; }

        public string photoname { get; set; }

        public string phototypename { get; set; }

        public DateTime? photodate { get; set; }

    }

    /// <summary>
    /// 楼盘详细信息类包括坐标--用于周边配套信息查询 潘锦发2015-05-21
    /// </summary>
    public class ProjectDetailInfoEntity : BaseTO
    {
        public int? projectid { get; set; }

        public int? areaid { get; set; }

        public string address { get; set; }

        public int? parkingnumber { get; set; }

        public string projectname { get; set; }

        public DateTime? enddate { get; set; }

        public decimal? managerprice { get; set; }

        public int? isevalue { get; set; }

        public string developcompanyname { get; set; }

        public string areaname { get; set; }

        public string managercompanyname { get; set; }

        public decimal? x { get; set; }

        public decimal? y { get; set; }

        public int? casecnt { get; set; }
    }
}
