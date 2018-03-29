
using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_ContractManage : DatContractManage
    {
        [SQLReadOnly]
        /// <summary>
        /// 分支机构名称
        /// </summary>
        public string subcompanyname { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 业务类型
        /// </summary>
        public string biztypename { get; set; }
        [SQLReadOnly]
        /// <summary>
        /// 报告类型
        /// </summary>
        public string reporttype { get; set; }
        [SQLReadOnly]
        /// <summary>
        ///领取人
        /// </summary>
        public string receiveusertruename { get; set; }
    }
}
