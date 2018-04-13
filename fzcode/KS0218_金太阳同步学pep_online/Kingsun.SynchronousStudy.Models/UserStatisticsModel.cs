using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingsun.SynchronousStudy.Models
{
    public class UserStatisticsModel
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string TelePhone { get; set; }

        public string VersionName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int Number { get; set; }

        public int UseTime { get; set; }

        public DateTime LoginTime { get; set; }

        public string ClassShortID { get; set; }

        public int VersionNumber { get; set; }

    }
}