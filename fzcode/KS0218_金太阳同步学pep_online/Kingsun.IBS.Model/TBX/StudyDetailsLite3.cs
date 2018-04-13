using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model.TBX
{
    public class StudyDetailsLite3
    {
        public int ID { get; set; }

        public int BookID { get; set; }
        public int VideoNumber { get; set; }

        public float TotalScore { get; set; }

        public int UserID { get; set; }

        public string VideoFileID { get; set; }

        public string VideoTitle { get; set; }

        public int FirstModularID { get; set; }

        public string FirstModular { get; set; }

        public int FirstTitleID { get; set; }
        public string FirstTitle { get; set; }
        public int SecondTitleID { get; set; }

        public string SecondTitle { get; set; }

        public DateTime CreateTime { get; set; }

        public string  NickName { get; set; }

        public string TrueName { get; set; }
        public string UserName { get; set; }

        public string UserImage { get; set; }

        public int IsEnableOss { get; set; }

        public int DubTimes { get; set; }

        public int VersionID { get; set; }
    }
}
