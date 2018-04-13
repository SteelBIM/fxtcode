using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    public class TB_UserInfo
    {
        public int? ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        
        public string NickName { get; set; }
        public string UserImage { get; set; }
        public int UserRoles { get; set; }
        public string TrueName { get; set; }
        
        public string TelePhone { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsLogState { get; set; }

        public int IsEnableOss { get; set; }

        public string AppId { get; set; }
    }
}
