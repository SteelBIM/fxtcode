namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DepartmentUser
    {
        public int Id { get; set; }

        public int DepartmentID { get; set; }

        public string UserName { get; set; }

        public int CityID { get; set; }

        public int FxtCompanyID { get; set; }

        public DateTime? CreateDate { get; set; }
        public Department Department { get; set; }

    }
}
