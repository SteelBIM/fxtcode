namespace CAS.Entity.BonaServiceEntity.Request
{
    /// <summary>
    /// 微信对话框对象
    /// </summary>
    public class WXDialogEntity : BaseRequestEntity
    {
        public string BID { get; set; }
       

        public string TYPE { get; set; }
        public string VALUE { get; set; }
        public string BUSINESSTYPE { get; set; }


        /// <summary>
        /// 委托人
        /// </summary>
        public string CLIENTMAN { get; set; }

        /// <summary>
        /// 委托人电话
        /// </summary>
        public string CLIENTPHONE { get; set; }

        /// <summary>
        /// 委托时间
        /// </summary>
        public string ENTRUSTDATE { get; set; }

        /// <summary>
        /// 委托机构编码
        /// </summary>
        public string CLIENT { get; set; }

        /// <summary>
        /// 委托机构名
        /// </summary>
        public string CLIENTNAME { get; set; }

        public string INPUTUSERNAME { get; set; }
        /// <summary>
        /// 贷款人角色
        /// </summary>
        public string LENDERROLE { get; set; }
        /// <summary>
        /// 按揭类型
        /// </summary>
        public string MORTGAGETYPE { get; set; }
    }
}
