using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    public class StudyReport
    {
        public int ID { get; set; }

        public int UserID{get;set;}
        public DateTime CreateTime { get; set; }

        public int ClassNum{get;set;}

        public int StudentStudyCount{get;set;}

        public int StudentCount{get;set;}

        public int VersionID{get;set;}
    }
}
