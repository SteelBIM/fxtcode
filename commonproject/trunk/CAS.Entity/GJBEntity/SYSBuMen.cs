using System.Collections.Generic;
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_BuMen : SYSBuMen
    {
        [SQLReadOnly]
        public List<SYS_BuMen> subbumenlist { get; set; }
        [SQLReadOnly]
        public int subbumencount { get; set; }
        [SQLReadOnly]
        public int provinceid { get; set; }
        [SQLReadOnly]
        public List<SYS_User> userlist { get; set; }
        [SQLReadOnly]
        public string cityname { get; set; }
        private SYS_SubCompanyCert _subcompanycert = new SYS_SubCompanyCert();
        [SQLReadOnly]
        public SYS_SubCompanyCert subcompanycert
        {
            get
            {
                return _subcompanycert;
            }
            set
            {
                _subcompanycert = value;
            }
        }
        [SQLReadOnly]
        public string chargemantruename { get; set; }
        [SQLReadOnly]
        public string chargemantruephone { get; set; }
        /// <summary>
        /// 有待工作任务的用户
        /// </summary>
        [SQLReadOnly]
        public List<SYS_User> workuserlist { get; set; }
        /// <summary>
        /// 有待查勘工作任务的用户(用于获取所有工作量时)
        /// </summary>
        [SQLReadOnly]
        public List<SYS_User> surveyworkuserlist { get; set; }
        /// <summary>
        /// 绑定供前台下拉框展示的树形结构数据
        /// </summary>
        [SQLReadOnly]
        public List<TreeSelect> treeselectlist { get; set; }

        
    }
}
