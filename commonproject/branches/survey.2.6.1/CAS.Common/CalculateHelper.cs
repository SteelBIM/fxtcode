using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common
{
    /// <summary>
    /// 计算帮助累
    /// </summary>
    public class CalculateHelper
    {
        /// <summary>
        /// 计算地球半径
        /// </summary>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public static double EarthRadius(double diameter)
        {
            return diameter * Math.PI / 180.0;
        }
        /// <summary>
        /// 根据百度地图坐标计算两点距离
        /// </summary>
        /// <param name="lng1">坐标1：Y</param>
        /// <param name="lat1">坐标1：X</param>
        /// <param name="lng2">坐标2：Y</param>
        /// <param name="lat2">坐标2：Y</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double EARTH_RADIUS = 6370996.81;
            double radLat1 = EarthRadius(lat1);
            double radLat2 = EarthRadius(lat2);
            double a = radLat1 - radLat2;
            double b = EarthRadius(lng1) - EarthRadius(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }  
    }
}
