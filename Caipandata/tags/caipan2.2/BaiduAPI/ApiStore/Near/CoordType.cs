using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    /// <summary>
    /// 坐标类型
    /// </summary>
    public enum CoordType
    {
        /// <summary>
        /// 经过国测局加密的坐标
        /// </summary>
        gcj02 = 0,
        /// <summary>
        /// 百度经纬度坐标
        /// </summary>
        bd09ll = 1,
        /// <summary>
        /// 百度墨卡托坐标
        /// </summary>
        bd09mc = 2,
        /// <summary>
        /// gps获取的坐标
        /// </summary>
        wgs84 = 3
    }
}
