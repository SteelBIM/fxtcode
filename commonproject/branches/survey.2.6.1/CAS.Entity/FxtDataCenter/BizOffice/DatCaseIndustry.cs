using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter
{
    [Serializable]
    [TableAttribute("dbo.Dat_Case_Industry")]
    public class DatCaseIndustry : BaseTO
    {
        public long id { get; set; }
        public string areaname { get; set; }
        public string subareaname { get; set; }
        public string projectname { get; set; }
        public string address { get; set; }
        public decimal buildingarea { get; set; }
        public decimal unitprice { get; set; }
        public decimal totalprice { get; set; }
        public string casetypecodename { get; set; }
        public DateTime casedate { get; set; }
        public string floorno { get; set; }
        public int? totalfloor { get; set; }
        public string sourcename { get; set; }
        public string sourcelink { get; set; }
        public string purposecodename { get; set; }




        [SQLReadOnly]
        public string buildingname { get; set; }
        [SQLReadOnly]
        public string housename { get; set; }
        [SQLReadOnly]
        public decimal? rentrate { get; set; }
        [SQLReadOnly]
        public string buildingtypename { get; set; }
        [SQLReadOnly]
        public int? usableyear { get; set; }
        [SQLReadOnly]
        public DateTime? startdate { get; set; }
        [SQLReadOnly]
        public decimal? totalbuildingarea { get; set; }
        [SQLReadOnly]
        public decimal? cubagerate { get; set; }
        [SQLReadOnly]
        public decimal? greenrate { get; set; }
        [SQLReadOnly]
        public int? buildingnum { get; set; }
        [SQLReadOnly]
        public DateTime? enddate { get; set; }
        [SQLReadOnly]
        public decimal? officearea { get; set; }
        [SQLReadOnly]
        public decimal? bizarea { get; set; }
        [SQLReadOnly]
        public decimal? industryarea { get; set; }
        [SQLReadOnly]
        public string traffictypename { get; set; }
        [SQLReadOnly]
        public string trafficdetails { get; set; }
        [SQLReadOnly]
        public string parkinglevelname { get; set; }
        [SQLReadOnly]
        public string parkingtypename { get; set; }
        [SQLReadOnly]
        public string details { get; set; }
        [SQLReadOnly]
        public string east { get; set; }
        [SQLReadOnly]
        public string west { get; set; }
        [SQLReadOnly]
        public string south { get; set; }
        [SQLReadOnly]
        public string north { get; set; }
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
    }
}

