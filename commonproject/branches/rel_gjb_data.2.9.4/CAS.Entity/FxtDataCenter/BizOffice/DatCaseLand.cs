using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtDataCenter
{
    [Serializable]
    [TableAttribute("dbo.Dat_CaseLand")]
    public class DatCaseLand : BaseTO
    {
        public long caseid { get; set; }
        public string areaname { get; set; }
        public DateTime casedate { get; set; }
        public string landpurposecode { get; set; }
        public string landno { get; set; }
        public decimal? landarea { get; set; }
        public string bargaintypecodename { get; set; }
        public decimal? minbargainprice { get; set; }
        public decimal? landunitprice { get; set; }
        public decimal? dealtotalprice { get; set; }
        public DateTime? startusabledate { get; set; }
        public string landpurposedesc { get; set; }
        public string sourcename { get; set; }
        public string sourcelink { get; set; }

        /// <summary>
        /// 建面地价
        /// </summary>
        [SQLReadOnly]
        public decimal? buildunitprice { get; set; }
        [SQLReadOnly]
        public decimal? buildingarea { get; set; }
        [SQLReadOnly]
        public string landpurposecodename { get; set; }
        [SQLReadOnly]
        public DateTime? enddate { get; set; }
        [SQLReadOnly]
        public DateTime? windate { get; set; }
        [SQLReadOnly]
        public string bargainstatecodename { get; set; }
        [SQLReadOnly]
        public DateTime? dealdate { get; set; }
        [SQLReadOnly]
        public string developdegreecodename { get; set; }
        [SQLReadOnly]
        public int? usableyear { get; set; }
        [SQLReadOnly]
        public string landaddress { get; set; }
        [SQLReadOnly]
        public decimal? cubagerate { get; set; }
        [SQLReadOnly]
        public decimal? greenrage { get; set; }
        [SQLReadOnly]
        public decimal? coverrate { get; set; }
        [SQLReadOnly]
        public string landclassname { get; set; }
        [SQLReadOnly]
        public string landshapecodename { get; set; }
        [SQLReadOnly]
        public string landusestatus { get; set; }
        [SQLReadOnly]
        public string planlimited { get; set; }
        [SQLReadOnly]
        public decimal? businesscenterdistance { get; set; }
        [SQLReadOnly]
        public string traffic { get; set; }
        [SQLReadOnly]
        public string infrastructure { get; set; }
        [SQLReadOnly]
        public string publicservice { get; set; }
        [SQLReadOnly]
        public string environmentcodename { get; set; }
        [SQLReadOnly]
        public string landdetail { get; set; }
        [SQLReadOnly]
        public string east { get; set; }
        [SQLReadOnly]
        public string west { get; set; }
        [SQLReadOnly]
        public string south { get; set; }
        [SQLReadOnly]
        public string north { get; set; }
        [SQLReadOnly]
        public DateTime? bargaindate { get; set; }
        [SQLReadOnly]
        public string landsourcecodename { get; set; }
        [SQLReadOnly]
        public DateTime? arrangestartdate { get; set; }
        [SQLReadOnly]
        public DateTime? arrangeenddate { get; set; }
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
