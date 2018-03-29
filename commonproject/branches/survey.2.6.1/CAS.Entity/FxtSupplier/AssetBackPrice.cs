using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier
{
    [Serializable]
    [TableAttribute("dbo.AssetBackPrice")]
    public class AssetBackPrice : BaseTO
    {
        private int _backpriceid;
        [SQLField("backpriceid", EnumDBFieldUsage.PrimaryKey, true)]
        public int backpriceid
        {
            get { return _backpriceid; }
            set { _backpriceid = value; }
        }
        private int? _assetobjectid;
        /// <summary>
        /// 资产ID
        /// </summary>
        public int? assetobjectid
        {
            get { return _assetobjectid; }
            set { _assetobjectid = value; }
        }
        private int? _acceptbusinessid;
        /// <summary>
        /// 业务受理ID
        /// </summary>
        public int? acceptbusinessid
        {
            get { return _acceptbusinessid; }
            set { _acceptbusinessid = value; }
        }
        private decimal? _unitprice;
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }
        private decimal? _totalprice;
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? totalprice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        private string _backpriceramark;
        /// <summary>
        /// 回价备注
        /// </summary>
        public string backpriceramark
        {
            get { return _backpriceramark; }
            set { _backpriceramark = value; }
        }
        private DateTime? _backpricedate;
        /// <summary>
        /// 回价时间
        /// </summary>
        public DateTime? backpricedate
        {
            get { return _backpricedate; }
            set { _backpricedate = value; }
        }
        private DateTime? _createdate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createdate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
    }

}
