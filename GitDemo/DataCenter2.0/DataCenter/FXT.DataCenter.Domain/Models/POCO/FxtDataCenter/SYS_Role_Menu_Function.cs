using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXT.DataCenter.Domain.Models
{
    public class SYS_Role_Menu_Function
    {

        public int ID { get; set; }
        public int RoleMenuID { get; set; }
        public int FunctionCode { get; set; }
        public int Valid { get; set; }
        public int CityID { get; set; }
        public int FxtCompanyID { get; set; }

        #region 扩展字段
        public bool Selected { get; set; }
        public string CodeName { get; set; }
        #endregion

    }
}
