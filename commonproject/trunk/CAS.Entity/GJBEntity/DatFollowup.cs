using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Followup:DatFollowUp
    {
        /// <summary>
        /// 询价业务类型
        /// </summary>
        [SQLReadOnly]
        public string objecttypecodename { get;set; }

        /// <summary>
        /// 创建人
        /// </summary
        [SQLReadOnly]
        public string createusername { get; set; }

        /// <summary>
        /// 业务环节流程
        /// </summary>
        [SQLReadOnly]
        public string businesstypecodename { get; set; }
    }
}
