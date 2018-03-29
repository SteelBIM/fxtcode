using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    [Serializable]
    public class DatKnowledgeBase : CAS.Entity.DBEntity.DatKnowledgeBase
    {
        /// <summary>
        /// 类别
        /// </summary>
        [SQLReadOnly]
        public string knowledgetype { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [SQLReadOnly]
        public string lastupdateusername { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SQLReadOnly]
        public string createusername { get; set; }
    }
}