using CAS.Entity.DBEntity;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.GJBEntity
{
    public class Dat_Estimates : DatEstimates
    {
        [SQLReadOnly]
        public string valuationmethodcodename
        {
            get;
            set;
        }

        [SQLReadOnly]
        public string guid
        {
            get;
            set;
        }
        [SQLReadOnly]
        #region 增加附属房屋相关字段 20151015 张磊
        //附属房屋面积
        public string subarea
        {
            get;
            set;
        }
        //附属房屋单价
        [SQLReadOnly]
        public string subunit
        {
            get;
            set;
        }
        //附属房屋值
        [SQLReadOnly]
        public string subzz
        {
            get;
            set;
        }
        //附属房屋总价
        [SQLReadOnly]
        public string subzj
        {
            get;
            set;
        }
        #endregion

        //装修总价
        [SQLReadOnly]
        public string decoratealltotalprice
        {
            get;
            set;
        }
        //动产总价
        [SQLReadOnly]
        public string movablealltotalprice
        {
            get;
            set;
        }
        //其他价值
        [SQLReadOnly]
        public string othervalue
        {
            get;
            set;
        }
        //土地单价
        [SQLReadOnly]
        public string landunitprice
        {
            get;
            set;
        }
        //土地总价
        [SQLReadOnly]
        public string landtotalprice
        {
            get;
            set;
        }
    }
}
