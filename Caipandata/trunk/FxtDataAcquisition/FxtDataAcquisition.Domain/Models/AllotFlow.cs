namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AllotFlow
    {
        public long Id { get; set; }

        public int CityId { get; set; }

        public int FxtCompanyId { get; set; }

        public int DatType { get; set; }

        public int DatId { get; set; }

        public string OtherId { get; set; }

        public int StateCode { get; set; }

        public DateTime? StateDate { get; set; }

        public DateTime CreateTime { get; set; }

        public string UserName { get; set; }

        public string SurveyUserName { get; set; }

        public string Remark { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public string UserTrueName { get; set; }

        public string SurveyUserTrueName { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<AllotSurvey> AllotSurveys { get; set; }
        public virtual ICollection<Check> Checks { get; set; }
    }
}
