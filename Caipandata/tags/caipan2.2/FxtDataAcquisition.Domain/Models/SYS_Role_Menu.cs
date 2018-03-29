namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_Role_Menu
    {
        public int Id { get; set; }

        public int RoleID { get; set; }

        public int MenuID { get; set; }

        public int CityID { get; set; }

        public int FxtCompanyID { get; set; }

        public virtual SYS_Role Role { get; set; }

        public virtual SYS_Menu Menu { get; set; }

        public virtual ICollection<SYS_Role_Menu_Function> Functions { get; set; }
    }
}
