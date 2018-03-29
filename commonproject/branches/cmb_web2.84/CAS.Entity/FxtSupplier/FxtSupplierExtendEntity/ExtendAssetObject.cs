using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendAssetObject : AssetObject
    {
        private List<BusinessFile> _filelist = new List<BusinessFile>();
        /// <summary>
        /// 资产附件
        /// </summary>
        [SQLReadOnly]
        public List<BusinessFile> filelist
        {
            get { return _filelist; }
            set { _filelist = value; }
        }
        /// <summary>
        /// 鉴价类型名称
        /// </summary>
        [SQLReadOnly]
        public string querytypename { get; set; }
        /// <summary>
        /// 资产类型名称
        /// </summary>
        [SQLReadOnly]
        public string assettypename { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [SQLReadOnly]
        public string cityname { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        [SQLReadOnly]
        public string provincename { get; set; }

        private AssetBackPrice _assetBackPrice = new AssetBackPrice();
        /// <summary>
        /// 回价信息
        /// </summary>
        [SQLReadOnly]
        public AssetBackPrice assetbackprice
        {
            get { return _assetBackPrice; }
            set { _assetBackPrice = value; }
        }
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
