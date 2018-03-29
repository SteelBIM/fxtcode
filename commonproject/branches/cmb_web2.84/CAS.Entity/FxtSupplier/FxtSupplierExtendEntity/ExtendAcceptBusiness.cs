using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    /// <summary>
    /// 业务受理扩展类
    /// </summary>
    public class ExtendAcceptBusiness : AcceptBusiness
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
        /// 报价
        /// </summary>
        [SQLReadOnly]
        public decimal? price{get;set;}
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
        /// <summary>
        /// 供应商简称
        /// </summary>
       [SQLReadOnly]
        public string shortname { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        [SQLReadOnly]
        public string companyname { get; set; }
        private List<BusinessFile> _listBusinessFile = new List<BusinessFile>();
        /// <summary>
        /// 业务附件
        /// </summary>
        [SQLReadOnly]
        public List<BusinessFile> listBusinessFile
        {
            get { return _listBusinessFile; }
            set { _listBusinessFile = value; }
        }
         /// <summary>
        /// 资产名称
        /// </summary>
         [SQLReadOnly]
        public string projectfullname { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
         [SQLReadOnly]
         public decimal? buildingarea { get; set;}
          /// <summary>
        /// 单价
        /// </summary>
         [SQLReadOnly]
        public decimal? unitprice{ get; set;}
        /// <summary>
        /// 总价
        /// </summary>
         [SQLReadOnly]
        public decimal? totalprice{ get; set;}
        /// <summary>
        /// 回价备注
        /// </summary>
         [SQLReadOnly]
        public string backpriceramark{ get; set;}
         /// <summary>
         /// 鉴价类型
         /// </summary>
         [SQLReadOnly]
         public string querytypename { get; set; }
         /// <summary>
        /// 回价时间
        /// </summary>
         [SQLReadOnly]
        public DateTime? backpricedate{ get; set;}
        /// <summary>
        /// 回价时间
        /// </summary>
        [SQLReadOnly]
        public int backpriceid { get; set; }

        private List<BusinessFile> _backPricefilelist = new List<BusinessFile>();
        /// <summary>
        /// 回价附件
        /// </summary>
        [SQLReadOnly]
        public List<BusinessFile> backpricefilelist
        {
            get { return _backPricefilelist; }
            set { _backPricefilelist = value; }
        }
    }
}
