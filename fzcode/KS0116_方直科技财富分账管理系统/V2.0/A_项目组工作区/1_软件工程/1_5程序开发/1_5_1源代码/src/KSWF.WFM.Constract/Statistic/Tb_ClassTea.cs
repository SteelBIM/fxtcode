using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Statistic
{
    public class tb_classtea
    {
        public int? ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserTrueName { get; set; }
        public string UserInvitation { get; set; }
        public string ClassID { get; set; }
        public int? SubjectID { get; set; }
        public string SubjectName { get; set; }

        public string UserMobile { get; set; }
    }
}
