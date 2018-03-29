using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.FxtSupplier.FxtSupplierExtendEntity
{
    public class ExtendAssetBackPrice : AssetBackPrice
    {
        private List<BusinessFile> _backPricefilelist = new List<BusinessFile>();
        /// <summary>
        /// 资产附件
        /// </summary>
        [SQLReadOnly]
        public List<BusinessFile> backpricefilelist
        {
            get { return _backPricefilelist; }
            set { _backPricefilelist = value; }
        }
    }
}
