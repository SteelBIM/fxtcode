using System.Collections.Generic;
using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;

namespace CAS.Entity.GJBEntity
{
    public class SYS_AssignUserExpand : BaseTO
    {
        private string _assinguserid;
        /// <summary>
        /// 分配人
        /// </summary>
        public string assinguserid
        {
            get { return _assinguserid; }
            set { _assinguserid = value; }
        }

        private string _assingusername;
        /// <summary>
        /// 分配人
        /// </summary>
        public string assingusername
        {
            get { return _assingusername; }
            set { _assingusername = value; }
        }

        private List<SYSAssignUser> _userassinglist;
        /// <summary>
        /// 用户分配列表
        /// </summary>
        public List<SYSAssignUser> userassinglist
        {
            get { return _userassinglist; }
            set { _userassinglist = value; }
        }
    }
}
