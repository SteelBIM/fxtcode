using System;
using System.Linq;
using CBSS.Framework.Contract;
using System.Collections.Generic;
using CBSS.Core.Utility;
using System.ComponentModel.DataAnnotations.Schema;
using CBSS.Account.Contract.ViewModel;

namespace CBSS.Account.Contract
{
    [Serializable]
    [Table("Sys_LoginInfo")]
    public class Sys_LoginInfo : ModelBase
    {
        public Sys_LoginInfo()
        {
            LastAccessTime = DateTime.Now;
            LoginToken = Guid.NewGuid();
        }

        public Sys_LoginInfo(int userID, string loginName)
        {
            LastAccessTime = DateTime.Now;
            LoginToken = Guid.NewGuid();

            UserID = userID;
            LoginName = loginName;
        }

        public Guid LoginToken { get; set; }
        public DateTime LastAccessTime { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public string ClientIP { get; set; }
        public IEnumerable<v_action> BusinessPermissionString { get; set; }

    }

    [Flags]
    public enum EnumLoginAccountType
    {
        [EnumTitle("[无]", IsDisplay = false)]
        Guest = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        [EnumTitle("管理员")]
        Administrator = 1,
    }


}



