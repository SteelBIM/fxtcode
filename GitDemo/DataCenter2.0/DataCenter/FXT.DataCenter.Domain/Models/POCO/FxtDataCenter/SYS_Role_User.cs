using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Role_User
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int FxtCompanyID { get; set; }
    }
}
