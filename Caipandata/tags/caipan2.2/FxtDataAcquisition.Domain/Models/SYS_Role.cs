namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string Remarks { get; set; }

        public int Valid { get; set; }

        public DateTime? CreateTime { get; set; }

        public int CityID { get; set; }

        public int FxtCompanyID { get; set; }
        public virtual ICollection<SYS_Role_User> RoleUsers { get; set; }

        public virtual ICollection<SYS_Role_Menu> RoleMenus { get; set; }
    }
}
