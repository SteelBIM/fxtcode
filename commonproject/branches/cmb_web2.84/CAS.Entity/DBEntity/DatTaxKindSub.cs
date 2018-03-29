using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    /// <summary>
    /// 数据库中存储的第一项值为原记录表中的值（如询价记录表中的税费、净值值字段）
    /// 第二项值开始（包括第二项）才是税费2、净值2的值。
    /// </summary>
    [Serializable]
    [TableAttribute("dbo.Dat_TaxKindSub")]    
    public class DatTaxKindSub : BaseTO
    {
        private long _mappingid;
        [SQLField("taxkindsubid", EnumDBFieldUsage.PrimaryKey, true)]
        public long taxkindsubid
        {
            get { return _mappingid; }
            set { _mappingid = value; }
        }
        private long _taxkindid;
        public long taxkindid
        {
            get { return _taxkindid; }
            set { _taxkindid = value; }
        }
        private decimal _tax;
        public decimal tax
        {
            get { return _tax; }
            set { _tax = value; }
        }
        private decimal _netprice;
        public decimal netprice
        {
            get { return _netprice; }
            set { _netprice = value; }
        }
    }
}
