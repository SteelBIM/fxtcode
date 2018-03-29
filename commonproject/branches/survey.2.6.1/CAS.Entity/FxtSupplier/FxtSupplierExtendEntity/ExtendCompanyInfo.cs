using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    /// <summary>
    /// 基础信息扩展类
    /// </summary>
    public class ExtendCompanyInfo : CompanyInfo
    {
        /// <summary>
        /// 审核记录Id
        /// </summary>
        [SQLReadOnly]
        public int auditid { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 省份简称
        /// </summary>
        [SQLReadOnly]
        public string alias { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [SQLReadOnly]
        public string username { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [SQLReadOnly]
        public int? auditvalid { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? audittime { get; set; }
        /// <summary>
        /// 提交审核时间
        /// </summary>
        [SQLReadOnly]
        public DateTime? submittime { get; set; }
        private List<Aptitude> _aptitudeList = new List<Aptitude>();
        /// <summary>
        /// 资质列表
        /// </summary>
        [SQLReadOnly]
        public List<Aptitude> aptitudeList { get { return _aptitudeList; } set { _aptitudeList = value; } }
        private List<BusinessFile> _businessfilelist = new List<BusinessFile>();
        /// <summary>
        /// 附件
        /// </summary>
        [SQLReadOnly]
        public List<BusinessFile> businessfilelist { get { return _businessfilelist; } set { _businessfilelist = value; } }
    }
}
