namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PAppendage
    {
        public int Id { get; set; }

        public int AppendageCode { get; set; }

        public int ProjectId { get; set; }

        public decimal? Area { get; set; }

        public string P_AName { get; set; }

        public bool IsInner { get; set; }

        public int? CityId { get; set; }

        public int? ClassCode { get; set; }

        public int? Distance { get; set; }

        public string Address { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public string Uid { get; set; }

        public virtual Project Project { get; set; }
    }
}
