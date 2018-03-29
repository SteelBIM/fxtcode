using CAS.Entity.BaseDAModels;
using System;

namespace CAS.Entity.FxtUserCenter
{
    [Serializable]
    [TableAttribute("FxtDataCenter.dbo.SYS_Role_User")]
    public class SysRoleUser : BaseTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public int CityID { get; set; }
        public int FxtCompanyID { get; set; }
    }
}
