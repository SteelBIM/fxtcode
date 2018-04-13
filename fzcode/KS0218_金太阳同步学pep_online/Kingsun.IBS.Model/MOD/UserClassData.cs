using Kingsun.SynchronousStudy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    public class UserClassData
    {
        public int UserID { get; set; }

        public UserTypeEnum UserType { get; set; }
        public string ClassID { get; set; }

        public int SchoolId { get; set; }

        public string SubjectID { get; set; }
        public ModRelationTypeEnum Type { get; set; }

        public string message { get; set; }

        public BCPointEnum flag { get; set; }
    }
}
