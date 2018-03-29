namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AllotSurvey
    {
        public long Id { get; set; }

        public long AllotId { get; set; }

        public int CityId { get; set; }

        public int FxtCompanyId { get; set; }

        public string UserName { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? StateCode { get; set; }

        public DateTime? StateDate { get; set; }

        public string Remark { get; set; }

        public string TrueName { get; set; }

        public virtual AllotFlow AllotFlow { get; set; }
        //public SYSCode SysCode { get; set; }
    }
}
