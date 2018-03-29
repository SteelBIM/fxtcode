using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class UnexpiredAppraiserInfo
    {
        /// <summary>
        /// 估价师名称
        /// </summary>
        [SQLReadOnly]
        public string appariserUserName { get; set; }
        /// <summary>
        /// 估价师注册号
        /// </summary>
        [SQLReadOnly]
        public string appariserNo { get; set; }

        [SQLReadOnly]
        public int appariserUserId { get; set; }

        [SQLReadOnly]
        public string type { get; set; }
    }
}
