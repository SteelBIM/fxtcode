using CAS.Entity.BaseDAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Entity.FxtProject
{
    /// <summary>
    /// 城市均价推送
    /// </summary>
    public class DatAvgPricePush: BaseTO
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string statisdate { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int provinceid { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string provincename { get; set; }
        /// <summary>
        /// 省国际码
        /// </summary>
        public string provincecode { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int cityid { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityname { get; set; }
        /// <summary>
        /// 城市国际码
        /// </summary>
        public string citycode { get; set; }
        /// <summary>
        /// 行政区id
        /// </summary>
        public int areaid { get; set; }
        /// <summary>
        /// 行政区名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 行政区国际码
        /// </summary>
        public string areacode { get; set; }
        /// <summary>
        /// 均价
        /// </summary>
        public int cityavgprice { get; set; }
    }
}
