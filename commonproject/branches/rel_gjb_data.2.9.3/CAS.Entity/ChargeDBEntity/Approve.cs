using System;
using CAS.Entity.BaseDAModels;

namespace CAS.Entity.DBEntity
{
    [Serializable]
    public class Approve : BaseTO
    {
        /// <summary>
        /// id
        /// </summary>
        public int ApproveId
        {
            get;
            set;
        }

        /// <summary>
        ///编码类型
        /// </summary>
        public int IdOrCodeType
        {
            get;
            set;
        }
        /// <summary>
        /// 编码或者id
        /// </summary>
        public int CodeOrId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string ApproveName
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount
        {
            get;
            set;
        }
        ///// <summary>
        ///// 是否所有收费标准按此为最低优惠
        ///// </summary>
        //public bool IsSetThisValue
        //{
        //    get;
        //    set;
        //}

    }
}
