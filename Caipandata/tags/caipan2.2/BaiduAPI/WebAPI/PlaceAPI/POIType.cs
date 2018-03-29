using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    public enum POIType
    {
        none = 0,
        /// <summary>
        /// 酒店
        /// </summary>
        hotel = 2008029,
        /// <summary>
        /// 医院
        /// </summary>
        hospital = 2008007,
        /// <summary>
        /// 学校
        /// </summary>
        education = 2008006,
        /// <summary>
        /// 银行
        /// </summary>
        life = 2008005,
        ///// <summary>
        ///// 会所
        ///// </summary>
        //life = 2008001,
        /// <summary>
        /// 超市
        /// </summary>
        shopping = 2008002,
        /// <summary>
        /// 幼儿园
        /// </summary>
        kindergarten = 2008003,

    }
}
