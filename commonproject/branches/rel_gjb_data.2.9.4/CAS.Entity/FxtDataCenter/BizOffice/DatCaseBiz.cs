using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter
{
    [Serializable]
    [TableAttribute("dbo.Dat_Case_Biz")]
    public class DatCaseBiz : BaseTO
    {
        public long id { get; set; }
        public int cityid { get; set; }
        public string cityname { get; set; }
        public string areaname { get; set; }
        public string subareaname { get; set; }
        public string projectname { get; set; }
        public string address { get; set; }
        public decimal buildingarea { get; set; }
        public decimal unitprice { get; set; }
        public decimal totalprice { get; set; }
        public string casetypecodename { get; set; }
        public DateTime casedate { get; set; }
        public string housetypename { get; set; }
        public string bizcodename { get; set; }
        public string floorno { get; set; }
        public int? totalfloor { get; set; }
        public string fitmentname { get; set; }
        public decimal? managerprice { get; set; }
        public string sourcename { get; set; }
        public string sourcelink { get; set; }

        [SQLReadOnly]
        public string buildingname { get; set; }
        [SQLReadOnly]
        public decimal? rentrate { get; set; }
        [SQLReadOnly]
        public string renttypecodename { get; set; }
        [SQLReadOnly]
        public string traffictypename { get; set; }
        [SQLReadOnly]
        public string trafficdetails { get; set; }
        [SQLReadOnly]
        public int? istypical { get; set; }
        /// <summary>
        /// 1:估价宝案例，2：数据中心案例
        /// </summary>
        [SQLReadOnly]
        public int casesource { get; set; }
        /// <summary>
        /// 物业名称
        /// </summary>
        [SQLReadOnly]
        public string casename
        {
            get;
            set;
        }
        /// <summary>
        /// 月租金  Alex 增加 2015-10-23
        /// </summary>
        public decimal? monthlyrental
        {
            get;
            set;
        }
        /// <summary>
        /// 建筑结构
        /// </summary>
        [SQLReadOnly]
        public string structurecodename { get; set; }
        /// <summary>
        /// 是否带电梯
        /// </summary>
        [SQLReadOnly]
        public string iselevatorname { get; set; }
    }
}
