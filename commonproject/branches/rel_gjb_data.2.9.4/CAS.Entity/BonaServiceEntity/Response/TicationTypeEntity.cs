namespace CAS.Entity.BonaServiceEntity.Response
{
    public class TicationTypeEntity
    {
        /// <summary>
        /// 认证结果
        /// </summary>
        public string TICATIONSTATUS { get; set; }

        /// <summary>
        /// 认证日期
        /// </summary>
        public string BUISNEESDATE { get; set; }

        /// <summary>
        /// 预授信额度
        /// </summary>
        public string CREDITAMOUNT { get; set; }

        /// <summary>
        /// 是否提现
        /// </summary>
        public string WITHDRAWSTATUS { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public string BUSINESSSUM { get; set; }

        /// <summary>
        /// 提现期数
        /// </summary>
        public string TOTALPERIOD { get; set; }

        /// <summary>
        /// 提现日期
        /// </summary>
        public string STARTDATE { get; set; }

        /// <summary>
        /// 还款状态
        /// </summary>
        public string REPAYMENTSTATUS { get; set; }


    }
}
