namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Check
    {
        public long Id { get; set; }

        public int CityId { get; set; }

        public int FxtCompanyId { get; set; }

        public long AllotId { get; set; }

        public int DatType { get; set; }

        public long DatId { get; set; }

        public string OtherId { get; set; }

        public string CheckUserName1 { get; set; }

        public int? CheckState1 { get; set; }

        public string CheckRemark1 { get; set; }

        public DateTime? CheckDate1 { get; set; }

        public string CheckUserName2 { get; set; }

        public int? CheckState2 { get; set; }

        public string CheckRemark2 { get; set; }

        public DateTime? CheckDate2 { get; set; }

        public virtual AllotFlow AllotFlow { get; set; }

    }
}
