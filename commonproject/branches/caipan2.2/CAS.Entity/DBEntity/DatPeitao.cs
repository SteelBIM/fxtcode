using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    [TableAttribute("FxtDataCenter.dbo.Dat_PeiTao")]
    public class DatPeitao : BaseTO
    {
        [SQLField("id", EnumDBFieldUsage.PrimaryKey, true)]
        public int id { get; set; }
        public int cityid { get; set; }
        public int areaid { get; set; }
        public string PeiTaoName { get; set; }
        public string Address { get; set; }
        public int TypeCode { get; set; }
        public decimal? x { get; set; }
        public decimal? y { get; set; }
        public int valid { get; set; }
    }
}
