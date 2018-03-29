namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PCompany
    {
        public int ProjectId { get; set; }

        public string CompanyName { get; set; }

        public int CompanyType { get; set; }

        public int CityId { get; set; }

        public virtual Project Project { get; set; }
    }
}
