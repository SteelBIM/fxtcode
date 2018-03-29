using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_AssignUser:SYSAssignUser
    {
        [SQLReadOnly]
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        public string entrustcodename { get; set; }
        [SQLReadOnly]
        public string businesstypename { get; set; }
    }
}
