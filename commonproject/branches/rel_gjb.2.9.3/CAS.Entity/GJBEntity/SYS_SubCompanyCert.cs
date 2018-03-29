using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class SYS_SubCompanyCert : SYSSubCompanyCert
    {
        [SQLReadOnly]
        public string legalusername { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        [SQLReadOnly]
        public string bumenname { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        [SQLReadOnly]
        public int cityid { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 所在省份
        /// </summary>
        [SQLReadOnly]
        public string provincename { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [SQLReadOnly]
        public string linkman { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [SQLReadOnly]
        public string chargemantruenames { get; set; }
        /// <summary>
        /// 负责人电话
        /// </summary>
        [SQLReadOnly]
        public string chargemantruephones { get; set; }
        /// <summary>
        /// (部门/分支机构)联系电话
        /// </summary>
        [SQLReadOnly]
        public string telstr { get; set; }
        /// <summary>
        /// 业务范围
        /// </summary>
        [SQLReadOnly]
        public string businessscopecodenames { get; set; }
        /// <summary>
        /// 估价师配备情况
        /// </summary>
        [SQLReadOnly]
        public string appraisersituationnames { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [SQLReadOnly]
        public string address { get; set; }
        /// <summary>
        /// 自备估价师，多个truename用逗号分隔
        /// </summary>
        [SQLReadOnly]
        public string provideoneselfappraiserusernames { get; set; }
        /// <summary>
        /// 总公司分配估价师，多个truename用逗号分隔
        /// </summary>
        [SQLReadOnly]
        public string headquartersappraiserusernames { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SQLReadOnly]
        public string remarks { get; set; }
    }
}
