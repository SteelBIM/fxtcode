using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendBusiness : Business
    {
        /// <summary>
        /// 业务受理Id
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// 业务受理公司
        /// </summary>
        public int? companyid{get ;set ;}
        /// <summary>
        /// 业务状态
        /// </summary>
        public int? state { get; set; }
        /// <summary>
        ///  业务所属城市
        /// </summary>
        public int? cityid { get; set; }
        private List<BusinessFile> _listBusinessFile = new List<BusinessFile>();
        /// <summary>
        /// 业务附件
        /// </summary>
        public List<BusinessFile> listBusinessFile 
        {
            get { return _listBusinessFile; }
            set { _listBusinessFile = value; }
        }
        /// <summary>
        /// 完成业务供应商的数目
        /// </summary>
        public int successnumber { get; set; }
        /// <summary>
        /// 受理业务供应商的数目
        /// </summary>
        public int totalnumber { get; set; }
        /// <summary>
        /// 拒接业务供应商的数目
        /// </summary>
        public int refusenumber { get; set; }
        /// <summary>
        /// 未受理业务供应商的数目
        /// </summary>
        public int noacceptnumber { get; set; }
        /// <summary>
        /// 业务进行中供应商的数目
        /// </summary>
        public int processingnumber { get; set; }
    }
}
