using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.IBS
{
    public class VailUserInfo
    {
        public int UserID { get; set; }

        public DateTime CreateTime { get; set; }

        public int VersionID { get; set; }
        public string Versions { get; set; }

        public int DownloadChannel { get; set; }

        public int IsValidUser { get; set; }

        public string TelePhone { get; set; }
        public DateTime ValidUserTime { get; set; }

        public int SchoolID { get; set; }
        public int AreaID { get; set; }

        public string Area { get; set; }
    }
}
