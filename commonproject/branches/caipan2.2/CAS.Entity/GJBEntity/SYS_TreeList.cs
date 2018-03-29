using CAS.Entity.BaseDAModels;
using CAS.Entity.DBEntity;
using System.Collections.Generic;

namespace CAS.Entity.GJBEntity
{
    public class SYS_TreeList : SYSTreeList
    {
        /*
         * 命名方式tree_父ID_菜单ID
        */
        [SQLReadOnly]
        public List<string> childtreeidlist { get; set; }
        /// <summary>
        /// 收件箱总记录数
        /// </summary>
        [SQLReadOnly]
        public int tree_1_3 { get; set; }
        /// <summary>
        /// 通知公告总记录数
        /// </summary>
        [SQLReadOnly]
        public int tree_1_4 { get; set; }
        /// <summary>
        /// 待我审批总记录数
        /// </summary>
        [SQLReadOnly]
        public int tree_12_15 { get; set; }
    }
}
