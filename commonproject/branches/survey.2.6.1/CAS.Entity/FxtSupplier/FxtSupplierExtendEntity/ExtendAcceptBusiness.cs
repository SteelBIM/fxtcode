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
        /// 公司logo
        /// </summary>
        [SQLReadOnly]
        public string logo { get; set; }
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
        /// <summary>
        /// 业务状态
        /// </summary>
        [SQLReadOnly]
        public int valid { get; set; }
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
        /// <summary>
        /// 服务类型code
        /// </summary>
        [SQLReadOnly]
        public int service{get;set;}
        /// <summary>
        /// 服务类型
        /// </summary>
        [SQLReadOnly]
        public string servicename { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [SQLReadOnly]
        public string projectname { get; set; }
        /// <summary>
        /// 委估机构名称
        /// </summary>
        [SQLReadOnly]
        public string customercompanyfullname { get; set; }
        /// <summary>
        /// 业务范围类型。1:全国供应商都可以受理；2：整个城市；3：具体某供应商受理
        /// </summary>
        [SQLReadOnly]
        public int? businessscopetype{get;set;}
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [SQLReadOnly]
        public string clientname { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [SQLReadOnly]
        public string clientcontact { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [SQLReadOnly]
        public string clientemail { get; set; }
        /// <summary>
        /// 联系人id
        /// </summary>
        [SQLReadOnly]
        public int linknameid { get; set; }
        /// <summary>
        ///  供应商联系人邮箱
        /// </summary>
        [SQLReadOnly]
        public string email { get; set; }
        /// <summary>
        ///  供应商联系人手机号码
        /// </summary>
        [SQLReadOnly]
        public string mobile { get; set; }
        /// <summary>
        ///  供应商联系人电话
        /// </summary>
        [SQLReadOnly]
        public string telephone { get; set; }
        /// <summary>
        ///  供应商联系人
        /// </summary>
        [SQLReadOnly]
        public string truename { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [SQLReadOnly]
        public string username { get; set; }
        /// <summary>
        /// 交付业务信息
        /// </summary>
        [SQLReadOnly]
        public string entrustinfo { get; set; }
        /// <summary>
        /// 业务状态信息
        /// </summary>
        [SQLReadOnly]
        public string statemodel { get; set; }
        /// <summary>
        /// 业务分发附件
        /// </summary>
        [SQLReadOnly]
        public string sendfilemodel { get; set; }
    }
}
