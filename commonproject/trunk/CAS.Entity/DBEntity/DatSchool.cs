using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 学校
    /// </summary>
    [Serializable]
    [TableAttribute("FxtDataCenter.dbo.Dat_School")]
    public class DatSchool : BaseTO
    {
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id { get; set; }
        public int cityid { get; set; }
        public int areaid { get; set; }
        public string schoolname { get; set; }
        public string address { get; set; }
        public decimal? x { get; set; }
        public decimal? y { get; set; }
        public int valid { get; set; }
        public int typecode { get; set; }
    }
}
