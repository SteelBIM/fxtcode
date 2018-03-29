using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
   public class SYS_Role_Menu
    {
       public int ID { get; set; }
       public int RoleID { get; set; }
       public int MenuID { get; set; }
       public int CityID { get; set; }
       public int FxtCompanyID { get; set; }
    }
}
