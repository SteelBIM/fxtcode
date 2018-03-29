namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_Role_User
    {
        public int Id { get; set; }

        public int RoleID { get; set; }

        public string UserName { get; set; }

        public int CityID { get; set; }

        public int FxtCompanyID { get; set; }

        public string TrueName { get; set; }
        public virtual SYS_Role Role { get; set; }
    }
}
