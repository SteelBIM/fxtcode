using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class SYS_QueryAssignUser : SYSQueryAssignUser
    {
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        public string entrysttypename { get; set; }
        [SQLReadOnly]
        public string querytypename { get; set; }
    }
}
